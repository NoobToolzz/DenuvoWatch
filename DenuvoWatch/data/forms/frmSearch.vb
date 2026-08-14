Imports System.Net.Http
Imports System.Text.Json

' =============================================================================
' Class: frmSearch
' -----------------------------------------------------------------------------
' Search filter form. Populates the developer / publisher / scene-group
' ComboBoxes from loaded game data, with multi-select support via
' MultiSelectCombo (CheckedListBox popup). When btnSearch is clicked, validates
' at least one filter is active, fetches results from the local API, and
' either navigates to frmResults (passing the JSON + filter display string)
' or shows an error + resets the form if no results found.
' =============================================================================
Public Class frmSearch
    Private ReadOnly http As New HttpClient()

    ' Multi-select wrappers for each filter ComboBox.
    Private comboPublisher As MultiSelectCombo
    Private comboDeveloper As MultiSelectCombo
    Private comboSceneGroup As MultiSelectCombo

    ' ---------------------------------------------------------------------------
    ' frmSearch_Load
    '   Fills the three filter ComboBoxes from GamesData and wraps each in a
    '   MultiSelectCombo for multi-select behaviour.
    ' ---------------------------------------------------------------------------
    Private Sub frmSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateFilterComboBoxes(cbDeveloper, cbPublisher, cbSceneGroup)

        ' Wrap each one so clicking the dropdown shows checkboxes instead of a normal list
        comboPublisher = New MultiSelectCombo(cbPublisher, Me)
        comboDeveloper = New MultiSelectCombo(cbDeveloper, Me)
        comboSceneGroup = New MultiSelectCombo(cbSceneGroup, Me)
    End Sub

    ' ---------------------------------------------------------------------------
    ' btnSearch_Click
    '   Validates at least one filter is active, builds the encoded API URL,
    '   fetches the JSON response, and either navigates to frmResults (passing
    '   the JSON + filter display string) or shows an error + resets the form
    '   if no results found.
    ' ---------------------------------------------------------------------------
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim query = tbQuery.Text.Trim()
        Dim developer = comboDeveloper.GetCheckedItems()
        Dim publisher = comboPublisher.GetCheckedItems()
        Dim sceneGroup = comboSceneGroup.GetCheckedItems()

        ' Gotta have at least one filter or the search doesn't make sense
        If String.IsNullOrWhiteSpace(query) AndAlso
           String.IsNullOrWhiteSpace(developer) AndAlso
           String.IsNullOrWhiteSpace(publisher) AndAlso
           String.IsNullOrWhiteSpace(sceneGroup) Then
            MessageBox.Show("Please enter at least one filter before searching.",
                            "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Build the URL and hit the local API to get results
        Dim url = BuildSearchUrl(query, developer, publisher, sceneGroup)

        Dim json As String = Nothing
        Try
            json = http.GetStringAsync(url).Result
        Catch ex As Exception
            MessageBox.Show("Failed to fetch results from the API." & vbCrLf & ex.Message,
                            "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Parse the JSON and see if we actually got anything back
        Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
        Dim root = JsonSerializer.Deserialize (Of GamesRoot)(json, options)
        Dim games = If(root?.Games, New List(Of GameItem)())

        If games.Count = 0 Then
            MessageBox.Show("No games found matching your search." & vbCrLf &
                            "Please try different filters.",
                            "No Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResetFilters()
            Return
        End If

        ' Build the nice filter string to show on the results page (like "Ubisoft · Capcom")
        Dim filterDisplay = BuildFiltersDisplay(publisher, developer, sceneGroup)

        ' We got results — pass the raw JSON and the filter string over to frmResults
        NavigateTo(Me, Function()
            Dim results As New frmResults()
            results.ResultsJson = json
            results.SearchFilters = filterDisplay
            Return results
        End Function)
    End Sub

    ' ---------------------------------------------------------------------------
    ' BuildFiltersDisplay
    '   Builds the filter summary string using the centered dot (·) separator.
    '   Format: "{publisher(s)} · {developer(s)} · {scene group(s)}"
    '   Only non-empty categories are included.
    ' ---------------------------------------------------------------------------
    Private Function BuildFiltersDisplay(publisher As String,
                                         developer As String,
                                         sceneGroup As String) As String
        Dim parts As New List(Of String)

        Dim pubDisplay = comboPublisher.GetCheckedItemsDisplay()
        Dim devDisplay = comboDeveloper.GetCheckedItemsDisplay()
        Dim sceneDisplay = comboSceneGroup.GetCheckedItemsDisplay()

        If Not String.IsNullOrWhiteSpace(pubDisplay) Then parts.Add(pubDisplay)
        If Not String.IsNullOrWhiteSpace(devDisplay) Then parts.Add(devDisplay)
        If Not String.IsNullOrWhiteSpace(sceneDisplay) Then parts.Add(sceneDisplay)

        Return String.Join(" · ", parts)
    End Function

    ' ---------------------------------------------------------------------------
    ' ResetFilters
    '   Clears the query text box and unchecks all items in every
    '   MultiSelectCombo.
    ' ---------------------------------------------------------------------------
    Private Sub ResetFilters()
        tbQuery.Clear()
        comboPublisher.Reset()
        comboDeveloper.Reset()
        comboSceneGroup.Reset()
    End Sub

    ' ---------------------------------------------------------------------------
    ' BuildSearchUrl
    '   Constructs the fully encoded API URL. Only non-blank parameters are
    '   appended. Multi-select values are already comma-separated.
    ' ---------------------------------------------------------------------------
    Private Function BuildSearchUrl(query As String,
                                    developer As String,
                                    publisher As String,
                                    sceneGroup As String) As String
        Dim parts As New List(Of String)

        If Not String.IsNullOrWhiteSpace(query) Then
            parts.Add("q=" & Uri.EscapeDataString(query))
        End If
        If Not String.IsNullOrWhiteSpace(developer) Then
            parts.Add("developer=" & Uri.EscapeDataString(developer))
        End If
        If Not String.IsNullOrWhiteSpace(publisher) Then
            parts.Add("publisher=" & Uri.EscapeDataString(publisher))
        End If
        If Not String.IsNullOrWhiteSpace(sceneGroup) Then
            parts.Add("scene_group=" & Uri.EscapeDataString(sceneGroup))
        End If

        Return "http://localhost:5050/search?" & String.Join("&", parts)
    End Function
End Class
