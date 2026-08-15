Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.RegularExpressions

' =============================================================================
' Class: frmSearch
' -----------------------------------------------------------------------------
' Search filter form. Populates the developer / publisher / scene-group
' ComboBoxes from loaded game data, with multi-select support via
' MultiSelectCombo (CheckedListBox popup). The query field also accepts a
' Steam AppID (1-7 digit number) — when one is detected, the filter
' ComboBoxes are disabled since the search becomes appid-only.
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
    ' tbQuery_TextChanged
    '   Watches the query field as the user types. If I detect a valid AppID
    '   (1-7 digit pure number), I disable all the filter ComboBoxes inside
    '   gbFilters and stick a tooltip on them explaining why. When the user
    '   clears the AppID or types something that isn't just digits, I
    '   re-enable everything.
    ' ---------------------------------------------------------------------------
    Private Sub tbQuery_TextChanged(sender As Object, e As EventArgs) Handles tbQuery.TextChanged
        Dim text = tbQuery.Text.Trim()

        If IsAppId(text) Then
            ' AppID detected — lock down the comboboxes
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = False
            Next
            ' Disabled controls don't fire mouse events, so I put the tooltip
            ' on the GroupBox itself which stays enabled and covers the same area
            toolTipFilters.SetToolTip(gbFilters, "AppID detected in search query. Remove it to re-enable filters.")
        Else
            ' No AppID — unlock everything
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = True
            Next
            toolTipFilters.SetToolTip(gbFilters, Nothing)
        End If
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

        ' If it's an AppID, I take a different path — search only by appid
        If IsAppId(query) Then
            SearchByAppId(query)
            Return
        End If

        ' Normal search path — query + filter comboboxes
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
        Dim games = ParseResultsJson(json)

        If games.Count = 0 Then
            MessageBox.Show("No games found matching your search." & vbCrLf &
                            "Please try different filters.",
                            "No Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResetFilters()
            Return
        End If

        ' Build the nice filter string to show on the results page
        Dim filterDisplay = BuildFiltersDisplay(query, publisher, developer, sceneGroup)

        ' We got results — pass the raw JSON and the filter string over to frmResults
        NavigateToResults(json, filterDisplay)
    End Sub

    ' ---------------------------------------------------------------------------
    ' SearchByAppId
    '   Hits the API with just the appid parameter. If no game matches, I
    '   show an error and reset.
    ' ---------------------------------------------------------------------------
    Private Sub SearchByAppId(appId As String)
        Dim url = "http://localhost:5050/search?appid=" & Uri.EscapeDataString(appId)

        Dim json As String = Nothing
        Try
            json = http.GetStringAsync(url).Result
        Catch ex As Exception
            MessageBox.Show("Failed to fetch results from the API." & vbCrLf & ex.Message,
                            "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim games = ParseResultsJson(json)

        If games.Count = 0 Then
            MessageBox.Show($"No game found with AppID {appId}." & vbCrLf &
                            "Please check the AppID and try again.",
                            "No Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ResetFilters()
            Return
        End If

        ' For the filter display I show "AppID: {id}" — no quotes, explicitly labeled
        Dim filterDisplay = $"AppID: {appId}"

        NavigateToResults(json, filterDisplay)
    End Sub

    ' ---------------------------------------------------------------------------
    ' NavigateToResults
    '   Small helper so I don't repeat the NavigateTo boilerplate.
    ' ---------------------------------------------------------------------------
    Private Sub NavigateToResults(json As String, filterDisplay As String)
        NavigateTo(Me, Function()
            Dim results As New frmResults()
            results.ResultsJson = json
            results.SearchFilters = filterDisplay
            Return results
        End Function)
    End Sub

    ' ---------------------------------------------------------------------------
    ' ParseResultsJson
    '   Deserialises the API JSON response into a list of GameItem.
    ' ---------------------------------------------------------------------------
    Private Function ParseResultsJson(json As String) As List(Of GameItem)
        Dim options As New JsonSerializerOptions With {
            .PropertyNameCaseInsensitive = True,
            .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }
        Dim root = JsonSerializer.Deserialize(Of GamesRoot)(json, options)
        Return If(root?.Games, New List(Of GameItem)())
    End Function

    ' ---------------------------------------------------------------------------
    ' IsAppId
    '   Returns True if the text is a pure number with 1-7 digits — that's a
    '   valid Steam AppID as far as I'm concerned.
    ' ---------------------------------------------------------------------------
    Private Function IsAppId(text As String) As Boolean
        If String.IsNullOrWhiteSpace(text) Then Return False
        Return Regex.IsMatch(text, "^\d{1,7}$")
    End Function

    ' ---------------------------------------------------------------------------
    ' BuildFiltersDisplay
    '   Builds the filter summary string using the centered dot (·) separator.
    '   If the user typed a query, it goes first wrapped in quotes:
    '   "query" · publishers · developers · scene groups
    '   Only non-empty categories are included.
    ' ---------------------------------------------------------------------------
    Private Function BuildFiltersDisplay(query As String,
                                         publisher As String,
                                         developer As String,
                                         sceneGroup As String) As String
        Dim parts As New List(Of String)

        ' Query goes first, wrapped in quotes so it stands out from the filter names
        If Not String.IsNullOrWhiteSpace(query) Then parts.Add($"""{query}""")

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
