' frmLoader — downloads cover images then sends you to the search form.
Public Class frmLoader
    ' I run the caching on a background thread so the UI doesn't freeze
    Private Sub frmLoader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleFormButtons(Me)

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

    ' Caching's done — warn if anything failed, then off you go
    Private Sub OnCachingComplete(failedCount As Integer)
        If failedCount > 0 Then
            MessageBox.Show($"{failedCount} image(s) failed to download after 3 attempts.{vbCrLf}Their covers may be missing.",
                            "Cover Download Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        StartWebServer()
        NavigateTo(Me, Function() New frmSearch())
    End Sub
End Class
