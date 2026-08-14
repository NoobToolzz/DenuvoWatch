' =============================================================================
' Class: frmSearch
' -----------------------------------------------------------------------------
' Search filter form. Populates the developer / publisher / scene-group
' ComboBoxes with unique values from the loaded game data on form load.
' =============================================================================
Public Class frmSearch

    ' ---------------------------------------------------------------------------
    ' frmSearch_Load
    '   Fills the three filter ComboBoxes from GamesData when the form opens.
    '   GamesData is already loaded by frmLoader before navigation, so the
    '   data is ready to query immediately.
    ' ---------------------------------------------------------------------------
    Private Sub frmSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateFilterComboBoxes(cbDeveloper, cbPublisher, cbSceneGroup)
    End Sub

End Class
