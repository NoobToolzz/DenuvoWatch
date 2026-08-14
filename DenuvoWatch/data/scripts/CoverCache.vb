Imports System.IO
Imports System.Net.Http

' =============================================================================
' Module: CoverCache
' -----------------------------------------------------------------------------
' Manages the local cover-image cache. On startup the loader form calls
' RunCoverCaching which:
'   1. Fetches the remote games.json (via GamesData.LoadGames)
'   2. Ensures the data\covers folder tree exists next to the executable
'   3. Runs a verify → download → verify loop until every cover is present
'      and non-corrupt, or a maximum retry count is reached.
'
' Covers are saved as {sort_title}.jpg inside {AppContext.BaseDirectory}\data\covers.
' Only missing or corrupt covers are (re-)downloaded; valid ones are skipped
' so repeat launches are instant.
'
' All progress is reported live to a ProgressBar and Label via thread-safe
' Invoke calls, since the work runs on a background Task.
' =============================================================================
Public Module CoverCache
    ' Folder paths — created on demand next to the running executable.
    Private ReadOnly DataDir As String = Path.Combine(AppContext.BaseDirectory, "data")
    Private ReadOnly CoversDir As String = Path.Combine(DataDir, "covers")

    ' Shared HttpClient — one instance reused for all cover downloads.
    Private ReadOnly client As New HttpClient()

    ' Maximum verify→download rounds before giving up on stubborn covers.
    Private Const MaxRounds As Integer = 3

    ' Minimum file size for a cover to be considered valid (bytes).
    Private Const MinCoverSize As Integer = 500

    ' ---------------------------------------------------------------------------
    ' RunCoverCaching
    '   Main entry point called from frmLoader_Load on a background Task.
    '   Loads game data, creates folders, then loops verify→download→verify
    '   until all covers are valid or the retry limit is hit.
    '
    ' Parameters:
    '   pgb          – ProgressBar updated per file during each verify / download phase
    '   lbl          – Label showing the current task and step counter
    '   failedCount  – (out) number of covers that could not be verified after
    '                  exhausting all retry attempts; 0 when everything succeeded.
    '
    ' Returns: True if all covers verified successfully, False otherwise.
    ' ---------------------------------------------------------------------------
    Public Function RunCoverCaching(pgb As ProgressBar,
                                    lbl As Label,
                                    ByRef failedCount As Integer) As Boolean
        failedCount = 0

        ' First I need to pull the game data from GitHub
        UpdateStatus(lbl, "Fetching game data...")
        GamesData.LoadGames()

        If GamesData.AllGames.Count = 0 Then
            UpdateStatus(lbl, "Failed to load game data.")
            Return False
        End If

        ' Only grab games that actually have a sort_title and cover_url, otherwise I can't cache them
        Dim cacheable = GamesData.AllGames.Where(
            Function(g) Not String.IsNullOrWhiteSpace(g.SortTitle) AndAlso
                        Not String.IsNullOrWhiteSpace(g.CoverUrl)
            ).ToList()

        If cacheable.Count = 0 Then
            UpdateStatus(lbl, "No cacheable covers found.")
            Return False
        End If

        ' Make sure the folders exist before I start doing anything with files
        EnsureFolders()

        ' This is the main loop — I verify, download what's missing, then verify again until everything checks out
        Dim round = 0
        Do
            round += 1

            ' Check which covers are missing or broken
            Dim missing = VerifyCovers(cacheable, pgb, lbl)

            If missing.Count = 0 Then
                UpdateStatus(lbl, "All covers verified.")
                Return True
            End If

            If round > MaxRounds Then
                failedCount = missing.Count
                UpdateStatus(lbl,
                             $"Verification incomplete — {missing.Count} cover(s) failed after {MaxRounds} attempts.")
                Return False
            End If

            ' Only download the ones that failed verification
            DownloadCovers(missing, pgb, lbl, round)

        Loop
    End Function

    ' ---------------------------------------------------------------------------
    ' EnsureFolders
    '   Creates data\ and data\covers\ next to the executable if they don't
    '   already exist. Silent — no status or progress reporting.
    ' ---------------------------------------------------------------------------
    Private Sub EnsureFolders()
        If Not Directory.Exists(DataDir) Then Directory.CreateDirectory(DataDir)
        If Not Directory.Exists(CoversDir) Then Directory.CreateDirectory(CoversDir)
    End Sub

    ' ---------------------------------------------------------------------------
    ' VerifyCovers
    '   Checks every cacheable game's cover file for existence and validity.
    '   Returns the subset of games whose cover is missing or corrupt.
    '
    ' Progress: resets the progress bar, then increments per file checked.
    ' Status:   "Verifying integrity - [{current}/{total}]"
    ' ---------------------------------------------------------------------------
    Private Function VerifyCovers(games As List(Of GameItem),
                                  pgb As ProgressBar,
                                  lbl As Label) As List(Of GameItem)
        Dim missing As New List(Of GameItem)
        Dim total = games.Count

        ResetProgress(pgb, total)

        For i = 0 To total - 1
            Dim g = games(i)
            Dim coverPath = Path.Combine(CoversDir, g.SortTitle & ".jpg")

            If Not IsCoverValid(coverPath) Then
                missing.Add(g)
            End If

            UpdateProgress(pgb, i + 1)
            UpdateStatus(lbl, $"Verifying integrity - [{i + 1}/{total}]")
        Next

        Return missing
    End Function

    ' ---------------------------------------------------------------------------
    ' DownloadCovers
    '   Downloads each cover in parallel (max 8 concurrent requests) and saves
    '   the raw bytes to {sort_title}.jpg. Failed downloads are silently
    '   skipped — they'll be caught by the next verify pass.
    '
    '   Progress: resets the progress bar, then increments per file completed.
    '   Status:   "Downloading covers - [{current}/{total}] (Attempt {n})"
    ' ---------------------------------------------------------------------------
    Private Sub DownloadCovers(games As List(Of GameItem),
                               pgb As ProgressBar,
                               lbl As Label,
                               attempt As Integer)
        Dim total = games.Count
        Dim completed = 0
        Dim lockObj As New Object()

        ResetProgress(pgb, total)
        UpdateStatus(lbl, $"Downloading covers - [0/{total}] (Attempt {attempt})")

        ' Download up to 8 covers at a time so it's fast but doesn't hammer the server
        Parallel.ForEach(games, New ParallelOptions With {.MaxDegreeOfParallelism = 8}, Sub(g)
            Try
                Dim bytes = client.GetByteArrayAsync(g.CoverUrl).Result
                Dim coverPath = Path.Combine(CoversDir, g.SortTitle & ".jpg")
                File.WriteAllBytes(coverPath, bytes)
            Catch ex As Exception
                Console.WriteLine($"Failed to download cover for {g.SortTitle}: {ex.Message}")
            End Try

            ' Gotta lock this since multiple threads are updating the progress at the same time
            SyncLock lockObj
                completed += 1
                UpdateProgress(pgb, completed)
                UpdateStatus(lbl, $"Downloading covers - [{completed}/{total}] (Attempt {attempt})")
            End SyncLock
        End Sub)
    End Sub

    ' ---------------------------------------------------------------------------
    ' IsCoverValid
    '   Returns True when the file exists, is at least MinCoverSize bytes, and
    '   starts with a recognised image-format header (JPEG, PNG, WEBP/RIFF, GIF).
    '   This catches truncated/corrupt downloads without fully decoding the
    '   image, so it works even for formats GDI+ can't decode natively.
    '
    ' Examples:
    '   IsCoverValid("...\assassins_creed_shadows.jpg") → True  (valid webp)
    '   IsCoverValid("...\pragmata.jpg")                → False (file missing)
    '   IsCoverValid("...\partial.jpg")                 → False (truncated)
    ' ---------------------------------------------------------------------------
    Private Function IsCoverValid(path As String) As Boolean
        If Not File.Exists(path) Then Return False

        Try
            Dim info As New FileInfo(path)
            If info.Length < MinCoverSize Then Return False

            ' Just read the first 4 bytes to check what image format this is
            Using fs As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim buffer(3) As Byte
                If fs.Read(buffer, 0, 4) < 4 Then Return False

                ' JPEG starts with FF D8
                If buffer(0) = &HFF AndAlso buffer(1) = &HD8 Then Return True
                ' PNG starts with 89 50 4E 47
                If buffer(0) = &H89 AndAlso buffer(1) = &H50 AndAlso
                   buffer(2) = &H4E AndAlso buffer(3) = &H47 Then Return True
                ' WEBP is actually a RIFF container so it starts with 52 49 46 46
                If buffer(0) = &H52 AndAlso buffer(1) = &H49 AndAlso
                   buffer(2) = &H46 AndAlso buffer(3) = &H46 Then Return True
                ' GIF starts with 47 49 46 38
                If buffer(0) = &H47 AndAlso buffer(1) = &H49 AndAlso
                   buffer(2) = &H46 AndAlso buffer(3) = &H38 Then Return True

                Return False
            End Using
        Catch
            Return False
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' ResetProgress
    '   Thread-safe: sets the progress bar maximum and resets value to 0.
    ' ---------------------------------------------------------------------------
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

    ' ---------------------------------------------------------------------------
    ' UpdateProgress
    '   Thread-safe: sets the progress bar value.
    ' ---------------------------------------------------------------------------
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

    ' ---------------------------------------------------------------------------
    ' UpdateStatus
    '   Thread-safe: sets the label text.
    ' ---------------------------------------------------------------------------
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
