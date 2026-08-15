Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.RegularExpressions

' frmSearch — pick your filters and search for games.
Public Class frmSearch
    Private ReadOnly http As New HttpClient()

    Private comboPublisher As MultiSelectCombo
    Private comboDeveloper As MultiSelectCombo
    Private comboSceneGroup As MultiSelectCombo

    ' Fill the dropdowns and wrap them with checkbox popups
    Private Sub frmSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleFormButtons(Me)
        PopulateFilterComboBoxes(cbDeveloper, cbPublisher, cbSceneGroup)

        comboPublisher = New MultiSelectCombo(cbPublisher, Me)
        comboDeveloper = New MultiSelectCombo(cbDeveloper, Me)
        comboSceneGroup = New MultiSelectCombo(cbSceneGroup, Me)
    End Sub

    ' If the user typed a pure number, that's an AppID — lock the filters down
    Private Sub tbQuery_TextChanged(sender As Object, e As EventArgs) Handles tbQuery.TextChanged
        Dim text = tbQuery.Text.Trim()

        If IsAppId(text) Then
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = False
            Next
            ' Tooltip goes on the GroupBox since disabled controls don't get mouse hover
            toolTipFilters.SetToolTip(gbFilters, "AppID detected in search query. Remove it to re-enable filters.")
        Else
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = True
            Next
            toolTipFilters.SetToolTip(gbFilters, Nothing)
        End If
    End Sub

    ' Make sure they picked something, hit the API, go to results or show an error
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim query = tbQuery.Text.Trim()

        If IsAppId(query) Then
            SearchByAppId(query)
            Return
        End If

        Dim developer = comboDeveloper.GetCheckedItems()
        Dim publisher = comboPublisher.GetCheckedItems()
        Dim sceneGroup = comboSceneGroup.GetCheckedItems()

        If String.IsNullOrWhiteSpace(query) AndAlso
           String.IsNullOrWhiteSpace(developer) AndAlso
           String.IsNullOrWhiteSpace(publisher) AndAlso
           String.IsNullOrWhiteSpace(sceneGroup) Then
            MessageBox.Show("Please enter at least one filter before searching.",
                            "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim url = BuildSearchUrl(query, developer, publisher, sceneGroup)

        Dim json As String = Nothing
        Try
            json = http.GetStringAsync(url).Result
        Catch ex As Exception
            MessageBox.Show($"Failed to fetch results from the API.{vbCrLf}{ex.Message}",
                            "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim games = ParseResultsJson(json)

        If games.Count = 0 Then
            MessageBox.Show($"No games found matching your search.{vbCrLf}Please try different filters.",
                            "No Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResetFilters()
            Return
        End If

        Dim filterDisplay = BuildFiltersDisplay(query, publisher, developer, sceneGroup)
        NavigateToResults(json, filterDisplay)
    End Sub

    ' Search by AppID only
    Private Sub SearchByAppId(appId As String)
        Dim url = $"http://localhost:5050/search?appid={Uri.EscapeDataString(appId)}"

        Dim json As String = Nothing
        Try
            json = http.GetStringAsync(url).Result
        Catch ex As Exception
            MessageBox.Show($"Failed to fetch results from the API.{vbCrLf}{ex.Message}",
                            "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim games = ParseResultsJson(json)

        If games.Count = 0 Then
            MessageBox.Show($"No game found with AppID {appId}.{vbCrLf}Please check the AppID and try again.",
                            "No Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResetFilters()
            Return
        End If

        Dim filterDisplay = $"AppID: {appId}"
        NavigateToResults(json, filterDisplay)
    End Sub

    ' Helper to avoid repeating NavigateTo boilerplate.
    Private Sub NavigateToResults(json As String, filterDisplay As String)
        NavigateTo(Me, Function()
            Dim results As New frmResults()
            results.ResultsJson = json
            results.SearchFilters = filterDisplay
            Return results
        End Function)
    End Sub

    ' Deserialises the API JSON into a list of GameItem.
    Private Function ParseResultsJson(json As String) As List(Of GameItem)
        Dim options As New JsonSerializerOptions With {
            .PropertyNameCaseInsensitive = True,
            .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }
        Dim root = JsonSerializer.Deserialize(Of GamesRoot)(json, options)
        Return If(root?.Games, New List(Of GameItem)())
    End Function

    ' True if the text is a pure 1-7 digit number (valid Steam AppID).
    Private Function IsAppId(text As String) As Boolean
        If String.IsNullOrWhiteSpace(text) Then Return False
        Return Regex.IsMatch(text, "^\d{1,7}$")
    End Function

    ' Builds the filter summary: "query" · publishers · developers · scene groups
    Private Function BuildFiltersDisplay(query As String, publisher As String,
                                         developer As String, sceneGroup As String) As String
        Dim parts As New List(Of String)

        If Not String.IsNullOrWhiteSpace(query) Then parts.Add($"""{query}""")

        Dim pubDisplay = comboPublisher.GetCheckedItemsDisplay()
        Dim devDisplay = comboDeveloper.GetCheckedItemsDisplay()
        Dim sceneDisplay = comboSceneGroup.GetCheckedItemsDisplay()

        If Not String.IsNullOrWhiteSpace(pubDisplay) Then parts.Add(pubDisplay)
        If Not String.IsNullOrWhiteSpace(devDisplay) Then parts.Add(devDisplay)
        If Not String.IsNullOrWhiteSpace(sceneDisplay) Then parts.Add(sceneDisplay)

        Return String.Join(" · ", parts)
    End Function

    ' Clears the query box and unchecks all MultiSelectCombos.
    Private Sub ResetFilters()
        tbQuery.Clear()
        comboPublisher.Reset()
        comboDeveloper.Reset()
        comboSceneGroup.Reset()
    End Sub

    ' Builds the encoded API URL — only non-blank params are included.
    Private Function BuildSearchUrl(query As String, developer As String,
                                    publisher As String, sceneGroup As String) As String
        Dim parts As New List(Of String)

        If Not String.IsNullOrWhiteSpace(query) Then parts.Add($"q={Uri.EscapeDataString(query)}")
        If Not String.IsNullOrWhiteSpace(developer) Then parts.Add($"developer={Uri.EscapeDataString(developer)}")
        If Not String.IsNullOrWhiteSpace(publisher) Then parts.Add($"publisher={Uri.EscapeDataString(publisher)}")
        If Not String.IsNullOrWhiteSpace(sceneGroup) Then parts.Add($"scene_group={Uri.EscapeDataString(sceneGroup)}")

        Return $"http://localhost:5050/search?{String.Join("&", parts)}"
    End Function
End Class
