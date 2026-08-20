Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

' =============================================================================
' Module: Utils
' Moving between forms, starting/stopping the server, styling buttons, and theming.
' =============================================================================
Module Utils
    ' Theme state - default to light
    Public IsDarkTheme As Boolean = False

    ' Dark theme palette - softer dark grey, not near-black
    Public ReadOnly DarkBg As Color = ColorTranslator.FromHtml("#2b2b33")
    Public ReadOnly DarkSurface As Color = ColorTranslator.FromHtml("#393941")
    Public ReadOnly DarkBorder As Color = ColorTranslator.FromHtml("#4a4a55")
    Public ReadOnly DarkText As Color = ColorTranslator.FromHtml("#dcdce0")

    <DllImport("uxtheme.dll", CharSet := CharSet.Unicode, SetLastError := True)>
    Private Function SetWindowTheme(hwnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
    End Function

    <DllImport("user32.dll", CharSet := CharSet.Auto)>
    Private Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    Private Const WM_THEMECHANGED As Integer = &H31A

    Public Sub ApplyScrollbarTheme(ctrl As Control, dark As Boolean)
        If Not ctrl.IsHandleCreated Then Return
        SetWindowTheme(ctrl.Handle, If(dark, "DarkMode_Explorer", "Explorer"), "ScrollBar")
        SendMessage(ctrl.Handle, WM_THEMECHANGED, IntPtr.Zero, IntPtr.Zero)
    End Sub

    ' Hide me, show the next form, close me when it closes
    Public Sub NavigateTo(current As Form, createNext As Func(Of Form))
        current.Hide()
        Dim nextForm = createNext()
        AddHandler nextForm.FormClosed, Sub(s, ev) current.Close()
        nextForm.Show()
    End Sub

    ' Find every button on the form and make it look nice on hover.
    ' Hover colors are theme-aware: dark theme = white bg + black text on hover,
    ' light theme = grey bg + white text on hover.
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
                If IsDarkTheme Then
                    btn.BackColor = Color.FromArgb(220, 220, 225)
                    btn.ForeColor = Color.Black
                Else
                    btn.BackColor = Color.FromArgb(45, 45, 50)
                    btn.ForeColor = Color.White
                End If
                RoundButtonCorners(btn, 6)
            End Sub
            AddHandler btn.MouseLeave, Sub(s, e)
                If IsDarkTheme Then
                    btn.BackColor = DarkSurface
                    btn.ForeColor = DarkText
                Else
                    btn.BackColor = SystemColors.Control
                    btn.ForeColor = Color.Black
                End If
                btn.Region = Nothing
            End Sub
            AddHandler btn.MouseDown, Sub(s, e)
                If IsDarkTheme Then
                    btn.BackColor = Color.FromArgb(200, 200, 208)
                    btn.ForeColor = Color.Black
                Else
                    btn.BackColor = Color.FromArgb(60, 60, 66)
                    btn.ForeColor = Color.White
                End If
            End Sub
            AddHandler btn.MouseUp, Sub(s, e)
                If IsDarkTheme Then
                    btn.BackColor = Color.FromArgb(220, 220, 225)
                    btn.ForeColor = Color.Black
                Else
                    btn.BackColor = Color.FromArgb(45, 45, 50)
                    btn.ForeColor = Color.White
                End If
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
        Dim d = radius*2
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

    ' Toggle between dark and light, then re-apply colors live
    Public Sub ToggleTheme(form As Form)
        IsDarkTheme = Not IsDarkTheme
        ApplyTheme(form)
    End Sub

    ' Apply the current theme to a form and all its controls
    Public Sub ApplyTheme(form As Form)
        If IsDarkTheme Then
            form.BackColor = DarkBg
            form.ForeColor = DarkText
        Else
            form.BackColor = SystemColors.Control
            form.ForeColor = SystemColors.ControlText
        End If

        ApplyThemeRecursive(form)

        ' Update the toggle button text
        UpdateToggleText(form)
    End Sub

    Private Sub ApplyThemeRecursive(parent As Control)
        For Each ctrl In parent.Controls
            ApplyThemeToControl(ctrl)
            If ctrl.HasChildren Then ApplyThemeRecursive(ctrl)
        Next
    End Sub

    Private Sub ApplyThemeToControl(ctrl As Control)
        Dim dark = IsDarkTheme

        If TypeOf ctrl Is Label Then
            ctrl.BackColor = If(dark, DarkBg, SystemColors.Control)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.ControlText)

        ElseIf TypeOf ctrl Is RichTextBox Then
            ctrl.BackColor = If(dark, DarkSurface, SystemColors.Window)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.WindowText)
            ApplyScrollbarTheme(ctrl, dark)

        ElseIf TypeOf ctrl Is TextBoxBase Then
            ctrl.BackColor = If(dark, DarkSurface, SystemColors.Window)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.WindowText)

        ElseIf TypeOf ctrl Is GroupBox Then
            ctrl.BackColor = If(dark, DarkBg, SystemColors.Control)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.ControlText)

        ElseIf TypeOf ctrl Is ComboBox Then
            ctrl.BackColor = If(dark, DarkSurface, SystemColors.Window)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.WindowText)

        ElseIf TypeOf ctrl Is ListBox Then
            ctrl.BackColor = If(dark, DarkSurface, SystemColors.Window)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.WindowText)
            ApplyScrollbarTheme(ctrl, dark)

        ElseIf TypeOf ctrl Is Button Then
            Dim btn = DirectCast(ctrl, Button)
            ' Leave transparent buttons alone
            If btn.FlatStyle = FlatStyle.Flat AndAlso btn.BackColor = Color.Transparent Then Return
            btn.BackColor = If(dark, DarkSurface, SystemColors.Control)
            btn.ForeColor = If(dark, DarkText, Color.Black)

        ElseIf TypeOf ctrl Is CheckBox Then
            ctrl.BackColor = If(dark, DarkBg, SystemColors.Control)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.ControlText)

        ElseIf TypeOf ctrl Is RadioButton Then
            ctrl.BackColor = If(dark, DarkBg, SystemColors.Control)
            ctrl.ForeColor = If(dark, DarkText, SystemColors.ControlText)

        ElseIf TypeOf ctrl Is ProgressBar Then
            ctrl.BackColor = If(dark, DarkBg, SystemColors.Control)

        ElseIf TypeOf ctrl Is PictureBox Then
            Dim pb = DirectCast(ctrl, PictureBox)
            ' Only set background if no image loaded
            If pb.Image Is Nothing Then
                pb.BackColor = If(dark, DarkSurface, SystemColors.Control)
            End If
        End If
    End Sub

    ' Find the toggle button and update its text
    Private Sub UpdateToggleText(parent As Control)
        For Each ctrl In parent.Controls
            If TypeOf ctrl Is Button AndAlso ctrl.Name = "btnThemeToggle" Then
                DirectCast(ctrl, Button).Text = If(IsDarkTheme, "☀️", "🌙")
                Return
            End If
            If ctrl.HasChildren Then UpdateToggleText(ctrl)
        Next
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

    ' Stop the web server - safe even if it's already off
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

        For Each d In GetUniqueDevelopers()
            cbDeveloper.Items.Add(d)
        Next
        For Each p In GetUniquePublishers()
            cbPublisher.Items.Add(p)
        Next
        For Each s In GetUniqueSceneGroups()
            cbSceneGroup.Items.Add(s)
        Next
    End Sub
End Module
