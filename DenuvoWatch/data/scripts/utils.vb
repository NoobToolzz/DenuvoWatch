Module Utils
    ' ===========================================================================
    ' Module: Utils
    ' -----------------------------------------------------------------------------
    ' Thin orchestration layer between the WinForms UI forms and the data /
    ' server layer. Exposes simple Sub/Function entry points so form
    ' code-behind stays trivial.
    ' ===========================================================================

    ' ---------------------------------------------------------------------------
    ' NavigateTo
    '   Generic form-to-form transition. Hides the current form, creates and
    '   shows the target form, and wires up its FormClosed event so that when
    '   the target closes, the current form closes too (exiting the app when
    '   the chain leads back to the root loader).
    '
    ' This is the single entry point for all navigation between forms:
    '   frmLoader -> frmSearch -> frmResults -> frmExport -> frmResults / frmSearch
    '
    ' Example:
    '   NavigateTo(Me, Function() New frmSearch())
    ' ---------------------------------------------------------------------------
    Public Sub NavigateTo(current As Form, createNext As Func(Of Form))
        current.Hide()

        Dim nextForm = createNext()
        AddHandler nextForm.FormClosed, Sub(s, ev) current.Close()

        nextForm.Show()
    End Sub

    ' ---------------------------------------------------------------------------
    ' StartWebServer
    '   Starts the embedded Kestrel host on localhost:5050. Does nothing if
    '   the server is already running. GamesData must be loaded beforehand.
    ' ---------------------------------------------------------------------------
    Public Sub StartWebServer()
        If webApp IsNot Nothing Then Return

        Try
            webApp = CreateWebApp()
            webApp.RunAsync()
        Catch ex As Exception
            Console.WriteLine("Failed to start web server: " & ex.Message)
            webApp = Nothing
        End Try
    End Sub

    ' ---------------------------------------------------------------------------
    ' StopWebServer
    '   Gracefully stops the running web application (3-second timeout) and
    '   releases the Kestrel resources. Safe to call when the server is off.
    ' ---------------------------------------------------------------------------
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

    ' ---------------------------------------------------------------------------
    ' PopulateFilterComboBoxes
    '   Fills the three filter ComboBoxes on the search form with the unique
    '   developer / publisher / scene-group values derived from the loaded
    '   game collection. No blank sentinel item is needed — MultiSelectCombo
    '   treats "nothing checked" as "no filter".
    '
    ' Example:
    '   PopulateFilterComboBoxes(cbDeveloper, cbPublisher, cbSceneGroup)
    ' ---------------------------------------------------------------------------
    Public Sub PopulateFilterComboBoxes(cbDeveloper As ComboBox,
                                        cbPublisher As ComboBox,
                                        cbSceneGroup As ComboBox)

        cbDeveloper.Items.Clear()
        cbPublisher.Items.Clear()
        cbSceneGroup.Items.Clear()

        For Each d In GamesData.GetUniqueDevelopers()
            cbDeveloper.Items.Add(d)
        Next
        For Each p In GamesData.GetUniquePublishers()
            cbPublisher.Items.Add(p)
        Next
        For Each s In GamesData.GetUniqueSceneGroups()
            cbSceneGroup.Items.Add(s)
        Next
    End Sub
End Module
