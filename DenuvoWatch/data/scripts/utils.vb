Imports System.Drawing
Imports System.Drawing.Drawing2D

Module Utils
    ' Moving between forms, starting/stopping the server, styling buttons, filling dropdowns.

    ' Hide me, show the next form, close me when it closes
    Public Sub NavigateTo(current As Form, createNext As Func(Of Form))
        current.Hide()
        Dim nextForm = createNext()
        AddHandler nextForm.FormClosed, Sub(s, ev) current.Close()
        nextForm.Show()
    End Sub

    ' Recursively finds all buttons on a form and wires up flat hover effects:
    ' white text on grey hover, black text on unhover, rounded corners on hover.
    Public Sub StyleFormButtons(form As Form)
        For Each ctrl In form.Controls
            StyleControlRecursive(ctrl)
        Next
    End Sub

    Private Sub StyleControlRecursive(ctrl As Control)
        If TypeOf ctrl Is Button Then
            Dim btn = DirectCast(ctrl, Button)
            ' Leave transparent buttons alone, they have their own look
            If btn.FlatStyle = FlatStyle.Flat AndAlso btn.BackColor = Color.Transparent Then Return

            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0
            btn.Cursor = Cursors.Hand

            AddHandler btn.MouseEnter, Sub(s, e)
                btn.BackColor = Color.FromArgb(45, 45, 50)
                btn.ForeColor = Color.White
                RoundButtonCorners(btn, 6)
            End Sub
            AddHandler btn.MouseLeave, Sub(s, e)
                btn.BackColor = SystemColors.Control
                btn.ForeColor = Color.Black
                btn.Region = Nothing
            End Sub
            AddHandler btn.MouseDown, Sub(s, e)
                btn.BackColor = Color.FromArgb(60, 60, 66)
                btn.ForeColor = Color.White
            End Sub
            AddHandler btn.MouseUp, Sub(s, e)
                btn.BackColor = Color.FromArgb(45, 45, 50)
                btn.ForeColor = Color.White
            End Sub
        End If

        If ctrl.HasChildren Then
            For Each child In ctrl.Controls
                StyleControlRecursive(child)
            Next
        End If
    End Sub

    ' Round the corners on hover so it looks smooth
    Private Sub RoundButtonCorners(btn As Button, radius As Integer)
        Dim path As New GraphicsPath()
        Dim r = New Rectangle(0, 0, btn.Width, btn.Height)
        Dim d = radius * 2
        path.AddArc(r.X, r.Y, d, d, 180, 90)
        path.AddLine(r.X + d, r.Y, r.Right - d, r.Y)
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90)
        path.AddLine(r.Right, r.Y + d, r.Right, r.Bottom - d)
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90)
        path.AddLine(r.Right - d, r.Bottom, r.X + d, r.Bottom)
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90)
        path.AddLine(r.X, r.Bottom - d, r.X, r.Y + d)
        path.CloseFigure()
        btn.Region = New Region(path)
    End Sub

    ' Start the web server if it's not already going
    Public Sub StartWebServer()
        If webApp IsNot Nothing Then Return
        Try
            webApp = CreateWebApp()
            webApp.RunAsync()
        Catch ex As Exception
            Console.WriteLine($"Failed to start web server: {ex.Message}")
            webApp = Nothing
        End Try
    End Sub

    ' Stop the web server � safe even if it's already off
    Public Sub StopWebServer()
        If webApp Is Nothing Then Return
        Try
            webApp.StopAsync().Wait(TimeSpan.FromSeconds(3))
            DirectCast(webApp, IDisposable).Dispose()
        Catch
        Finally
            webApp = Nothing
        End Try
    End Sub

    ' Fill the filter dropdowns with unique values from the game data
    Public Sub PopulateFilterComboBoxes(cbDeveloper As ComboBox, cbPublisher As ComboBox, cbSceneGroup As ComboBox)
        cbDeveloper.Items.Clear()
        cbPublisher.Items.Clear()
        cbSceneGroup.Items.Clear()

        For Each d In GamesData.GetUniqueDevelopers() : cbDeveloper.Items.Add(d)
        Next
        For Each p In GamesData.GetUniquePublishers() : cbPublisher.Items.Add(p)
        Next
        For Each s In GamesData.GetUniqueSceneGroups() : cbSceneGroup.Items.Add(s)
        Next
    End Sub
End Module
