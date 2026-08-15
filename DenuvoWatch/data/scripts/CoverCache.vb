Imports System.IO
Imports System.Net.Http

' CoverCache — downloads and verifies cover images locally.
Public Module CoverCache
    Private ReadOnly DataDir As String = Path.Combine(AppContext.BaseDirectory, "data")
    Private ReadOnly CoversDir As String = Path.Combine(DataDir, "covers")
    Private ReadOnly client As New HttpClient()

    Private Const MaxRounds As Integer = 3
    Private Const MinCoverSize As Integer = 500

    ' Load game data, make folders, then keep checking and downloading until everything's good
    Public Function RunCoverCaching(pgb As ProgressBar, lbl As Label, ByRef failedCount As Integer) As Boolean
        failedCount = 0

        UpdateStatus(lbl, "Fetching game data...")
        GamesData.LoadGames()

        If GamesData.AllGames.Count = 0 Then
            UpdateStatus(lbl, "Failed to load game data.")
            Return False
        End If

        ' Skip games without a sort_title or cover_url
        Dim cacheable = GamesData.AllGames.Where(
            Function(g) Not String.IsNullOrWhiteSpace(g.SortTitle) AndAlso
                         Not String.IsNullOrWhiteSpace(g.CoverUrl)
        ).ToList()

        If cacheable.Count = 0 Then
            UpdateStatus(lbl, "No cacheable covers found.")
            Return False
        End If

        EnsureFolders()

        ' Keep checking, downloading what's missing, checking again
        Dim round = 0
        Do
            round += 1
            Dim missing = VerifyCovers(cacheable, pgb, lbl)

            If missing.Count = 0 Then
                UpdateStatus(lbl, "All covers verified.")
                Return True
            End If

            If round > MaxRounds Then
                failedCount = missing.Count
                UpdateStatus(lbl, $"Verification incomplete — {missing.Count} cover(s) failed after {MaxRounds} attempts.")
                Return False
            End If

            DownloadCovers(missing, pgb, lbl, round)
        Loop
    End Function

    ' Make the folders if they're not there
    Private Sub EnsureFolders()
        If Not Directory.Exists(DataDir) Then Directory.CreateDirectory(DataDir)
        If Not Directory.Exists(CoversDir) Then Directory.CreateDirectory(CoversDir)
    End Sub

    ' See which covers are missing or broken
    Private Function VerifyCovers(games As List(Of GameItem), pgb As ProgressBar, lbl As Label) As List(Of GameItem)
        Dim missing As New List(Of GameItem)
        Dim total = games.Count

        ResetProgress(pgb, total)

        For i = 0 To total - 1
            Dim g = games(i)
            Dim coverPath = Path.Combine(CoversDir, $"{g.SortTitle}.jpg")

            If Not IsCoverValid(coverPath) Then missing.Add(g)

            UpdateProgress(pgb, i + 1)
            UpdateStatus(lbl, $"Verifying integrity - [{i + 1}/{total}]")
        Next

        Return missing
    End Function

    ' Download 8 at a time. Broken ones get caught on the next pass
    Private Sub DownloadCovers(games As List(Of GameItem), pgb As ProgressBar, lbl As Label, attempt As Integer)
        Dim total = games.Count
        Dim completed = 0
        Dim lockObj As New Object()

        ResetProgress(pgb, total)
        UpdateStatus(lbl, $"Downloading covers - [0/{total}] (Attempt {attempt})")

        Parallel.ForEach(games, New ParallelOptions With {.MaxDegreeOfParallelism = 8}, Sub(g)
            Try
                Dim bytes = client.GetByteArrayAsync(g.CoverUrl).Result
                File.WriteAllBytes(Path.Combine(CoversDir, $"{g.SortTitle}.jpg"), bytes)
            Catch ex As Exception
                Console.WriteLine($"Failed to download cover for {g.SortTitle}: {ex.Message}")
            End Try

            ' Gotta lock this since threads are sharing the counter
            SyncLock lockObj
                completed += 1
                UpdateProgress(pgb, completed)
                UpdateStatus(lbl, $"Downloading covers - [{completed}/{total}] (Attempt {attempt})")
            End SyncLock
        End Sub)
    End Sub

    ' Check if the file is real — big enough and has a known image header
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
