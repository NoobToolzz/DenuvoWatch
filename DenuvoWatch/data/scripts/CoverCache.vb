Imports System.IO
Imports System.Net.Http

' =============================================================================
' Module: CoverCache
' Downloads and verifies cover images so they're ready locally on next launch.
' =============================================================================
Public Module CoverCache
    Private ReadOnly DataDir As String = Path.Combine(AppContext.BaseDirectory, "data")
    Private ReadOnly CoversDir As String = Path.Combine(DataDir, "covers")
    Private ReadOnly client As New HttpClient()

    Private Const MaxRounds As Integer = 3
    Private Const MinCoverSize As Integer = 500

    ' Lock so parallel download threads don't interleave their log lines
    Private ReadOnly logLock As New Object()

    ' Append a timestamped line to the RichTextBox.
    ' The timestamp stays default color; only the log text gets the color.
    Friend Sub LogRichText(rtb As RichTextBox, text As String, Optional clr As Color = Nothing)
        Try
            SyncLock logLock
                rtb.Invoke(Sub()
                    Dim ts = $"[{DateTime.Now:HH:mm:ss}] "
                    Dim logLine = $"{text}{vbCrLf}"

                    ' Write the timestamp in the default text color
                    rtb.SelectionStart = rtb.TextLength
                    rtb.SelectionLength = 0
                    rtb.SelectionColor = rtb.ForeColor
                    rtb.AppendText(ts)

                    ' Write the log text in the specified color (or default if none)
                    rtb.SelectionStart = rtb.TextLength
                    rtb.SelectionLength = 0
                    rtb.SelectionColor = If(clr = Nothing, rtb.ForeColor, clr)
                    rtb.AppendText(logLine)

                    rtb.ScrollToCaret()
                End Sub)
            End SyncLock
        Catch
        End Try
    End Sub

    ' Load game data, make folders, then keep checking and downloading until everything's good
    Public Function RunCoverCaching(pgb As ProgressBar, lbl As Label, rtb As RichTextBox,
        ByRef failedCount As Integer) As Boolean
        failedCount = 0

        LogRichText(rtb, "▸ Initializing cover cache...", Color.Gray)

        LogRichText(rtb, "→ Fetching game data from GitHub...")
        UpdateStatus(lbl, "Fetching game data...")
        LoadGames()

        If AllGames.Count = 0 Then
            LogRichText(rtb, "✗ Failed to load game data.", Color.Red)
            UpdateStatus(lbl, "Failed to load game data.")
            Return False
        End If

        LogRichText(rtb, $"✓ Loaded {AllGames.Count} games", Color.Green)

        ' Skip games without a sort_title or cover_url
        Dim cacheable = AllGames.Where(
            Function(g) Not String.IsNullOrWhiteSpace(g.SortTitle) AndAlso
                Not String.IsNullOrWhiteSpace(g.CoverUrl)).ToList()

        If cacheable.Count = 0 Then
            LogRichText(rtb, "✗ No cacheable covers found.", Color.Red)
            UpdateStatus(lbl, "No cacheable covers found.")
            Return False
        End If

        LogRichText(rtb, $"■ {cacheable.Count} cacheable game(s) with cover URLs")
        EnsureFolders(rtb)

        ' Keep checking, downloading what's missing, checking again
        Dim round = 0
        Do
            round += 1
            LogRichText(rtb, $"▸ Verifying integrity - round {round}", Color.Gray)
            Dim missing = VerifyCovers(cacheable, pgb, lbl, rtb)

            If missing.Count = 0 Then
                LogRichText(rtb, "✓ All covers verified.", Color.Green)
                UpdateStatus(lbl, "All covers verified.")
                Return True
            End If

            LogRichText(rtb, $"■ Verification complete - {missing.Count} cover(s) need downloading")

            If round > MaxRounds Then
                failedCount = missing.Count
                LogRichText(rtb,
                    $"✗ Verification incomplete - {missing.Count} cover(s) failed after {MaxRounds} attempts.",
                    Color.Red)
                UpdateStatus(lbl,
                    $"Verification incomplete - {missing.Count} cover(s) failed after {MaxRounds} attempts.")
                Return False
            End If

            DownloadCovers(missing, pgb, lbl, rtb, round)
        Loop
    End Function

    ' Make the folders if they're not there
    Private Sub EnsureFolders(rtb As RichTextBox)
        Dim createdData = Not Directory.Exists(DataDir)
        Dim createdCovers = Not Directory.Exists(CoversDir)

        If createdData Then Directory.CreateDirectory(DataDir)
        If createdCovers Then Directory.CreateDirectory(CoversDir)

        If createdData OrElse createdCovers Then
            LogRichText(rtb, $"■ Created data\ and data\covers\ folders")
        Else
            LogRichText(rtb, "✓ Folders already exist", Color.Green)
        End If
    End Sub

    ' See which covers are missing or broken
    Private Function VerifyCovers(games As List(Of GameItem), pgb As ProgressBar,
        lbl As Label, rtb As RichTextBox) As List(Of GameItem)
        Dim missing As New List(Of GameItem)
        Dim total = games.Count
        Dim valid = 0

        ResetProgress(pgb, total)

        For i = 0 To total - 1
            Dim g = games(i)
            Dim coverPath = Path.Combine(CoversDir, $"{g.SortTitle}.jpg")

            If IsCoverValid(coverPath) Then
                valid += 1
            Else
                missing.Add(g)
                LogRichText(rtb, $"  ✗ {g.SortTitle} - missing or corrupt", Color.Red)
            End If

            UpdateProgress(pgb, i + 1)
            UpdateStatus(lbl, $"Verifying integrity - [{i + 1}/{total}]")
        Next

        LogRichText(rtb, $"  ✓ {valid} valid, {missing.Count} missing")
        Return missing
    End Function

    ' Download 8 at a time. Broken ones get caught on the next pass
    Private Sub DownloadCovers(games As List(Of GameItem), pgb As ProgressBar, lbl As Label, rtb As RichTextBox,
        attempt As Integer)
        Dim total = games.Count
        Dim completed = 0
        Dim succeeded = 0
        Dim failed = 0
        Dim lockObj As New Object()

        ResetProgress(pgb, total)
        UpdateStatus(lbl, $"Downloading covers - [0/{total}] (Attempt {attempt})")
        LogRichText(rtb, $"▸ Downloading {total} cover(s) - attempt {attempt}", Color.Gray)

        Parallel.ForEach(games, New ParallelOptions With {.MaxDegreeOfParallelism = 8}, Sub(g)
            Try
                Dim bytes = client.GetByteArrayAsync(g.CoverUrl).Result
                File.WriteAllBytes(Path.Combine(CoversDir, $"{g.SortTitle}.jpg"), bytes)
                LogRichText(rtb, $"  ↓ {g.SortTitle} - downloaded", Color.Green)
                SyncLock lockObj
                    succeeded += 1
                End SyncLock
            Catch ex As Exception
                LogRichText(rtb, $"  ✗ {g.SortTitle} - {ex.Message}", Color.Red)
                SyncLock lockObj
                    failed += 1
                End SyncLock
            End Try

            ' Gotta lock this since threads are sharing the counter
            SyncLock lockObj
                completed += 1
                UpdateProgress(pgb, completed)
                UpdateStatus(lbl, $"Downloading covers - [{completed}/{total}] (Attempt {attempt})")
            End SyncLock
        End Sub)

        LogRichText(rtb, $"■ Downloaded {succeeded}/{total} ({failed} failed)", If(failed > 0, Color.Red, Color.Green))
    End Sub

    ' Check if the file is real - big enough and has a known image header
    Private Function IsCoverValid(path As String) As Boolean
        If Not File.Exists(path) Then Return False

        Try
            Dim info As New FileInfo(path)
            If info.Length < MinCoverSize Then Return False

            ' Just peek at the first 4 bytes
            Using fs As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim buffer(3) As Byte
                If fs.Read(buffer, 0, 4) < 4 Then Return False

                If buffer(0) = &HFF AndAlso buffer(1) = &HD8 Then Return True  ' JPEG
                If buffer(0) = &H89 AndAlso buffer(1) = &H50 AndAlso
                   buffer(2) = &H4E AndAlso buffer(3) = &H47 Then Return True  ' PNG
                If buffer(0) = &H52 AndAlso buffer(1) = &H49 AndAlso
                   buffer(2) = &H46 AndAlso buffer(3) = &H46 Then Return True  ' WEBP/RIFF
                If buffer(0) = &H47 AndAlso buffer(1) = &H49 AndAlso
                   buffer(2) = &H46 AndAlso buffer(3) = &H38 Then Return True  ' GIF

                Return False
            End Using
        Catch
            Return False
        End Try
    End Function

    ' Thread-safe helpers so I can touch the UI from a background thread
    Private Sub ResetProgress(pgb As ProgressBar, maximum As Integer)
        Try
            If pgb.InvokeRequired Then
                pgb.Invoke(Sub()
                    pgb.Maximum = maximum
                    pgb.Value = 0
                End Sub)
            Else
                pgb.Maximum = maximum
                pgb.Value = 0
            End If
        Catch
        End Try
    End Sub

    Private Sub UpdateProgress(pgb As ProgressBar, value As Integer)
        Try
            If pgb.InvokeRequired Then
                pgb.Invoke(Sub() pgb.Value = value)
            Else
                pgb.Value = value
            End If
        Catch
        End Try
    End Sub

    Private Sub UpdateStatus(lbl As Label, text As String)
        Try
            If lbl.InvokeRequired Then
                lbl.Invoke(Sub() lbl.Text = text)
            Else
                lbl.Text = text
            End If
        Catch
        End Try
    End Sub
End Module
