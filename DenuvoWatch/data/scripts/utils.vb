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
    ' PopulateFilterComboBoxes
    '   Fills the three filter ComboBoxes on the search form with the unique
    '   developer / publisher / scene-group values derived from the loaded
    '   game collection. A blank entry is inserted first so the user can
    '   indicate "no filter".
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

        ' Leading blank item = "no filter" sentinel.
        cbDeveloper.Items.Add("")
        cbPublisher.Items.Add("")
        cbSceneGroup.Items.Add("")

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
