' =============================================================================
' Class: frmLoader
' -----------------------------------------------------------------------------
' Splash-style form shown at startup. Runs the cover-caching process on a
' background thread (CoverCache.RunCoverCaching) which verifies and downloads
' game cover images, reporting live progress to pgbLoader and lblStatus.
' Once caching completes, transitions to frmSearch. If any covers failed
' after all retry attempts, a warning is shown first.
' =============================================================================
Public Class frmLoader

    ' ---------------------------------------------------------------------------
    ' frmLoader_Load
    '   Kicks off the cover caching on a background Task so the UI thread
    '   stays responsive. When the task finishes, marshals back to the UI
    '   thread to show a warning (if needed) and transition to frmSearch.
    ' ---------------------------------------------------------------------------
    Private Sub frmLoader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim failedCount As Integer = 0

        Dim cacheTask = Task.Run(Function() CoverCache.RunCoverCaching(pgbLoader, lblStatus, failedCount))

        cacheTask.ContinueWith(Sub(t)
                                   If Me.InvokeRequired Then
                                       Me.Invoke(Sub() OnCachingComplete(failedCount))
                                   Else
                                       OnCachingComplete(failedCount)
                                   End If
                               End Sub)
    End Sub

    ' ---------------------------------------------------------------------------
    ' OnCachingComplete
    '   Called on the UI thread after the background caching finishes. If any
    '   covers failed, shows a warning MessageBox before navigating to frmSearch.
    ' ---------------------------------------------------------------------------
    Private Sub OnCachingComplete(failedCount As Integer)
        If failedCount > 0 Then
            MessageBox.Show($"{failedCount} image(s) failed to download after 3 attempts." & vbCrLf &
                            "Their covers may be missing.",
                            "Cover Download Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        End If

        NavigateTo(Me, Function() New frmSearch())
    End Sub

End Class
