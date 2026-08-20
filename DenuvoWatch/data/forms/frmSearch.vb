Imports System.ComponentModel
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.RegularExpressions

' =============================================================================
' Form: frmSearch
' Pick your filters and search for games, or type an AppID to jump straight to one.
' =============================================================================
Public Class frmSearch
    Private ReadOnly http As New HttpClient()

    Private comboPublisher As MultiSelectCombo
    Private comboDeveloper As MultiSelectCombo
    Private comboSceneGroup As MultiSelectCombo

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property RestoreState As SearchFilterState

    ' Fill the dropdowns and wrap them with checkbox popups
    Private Sub frmSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleFormButtons(Me)
        ApplyTheme(Me)
        PopulateFilterComboBoxes(cbDeveloper, cbPublisher, cbSceneGroup)

        comboPublisher = New MultiSelectCombo(cbPublisher, Me)
        comboDeveloper = New MultiSelectCombo(cbDeveloper, Me)
        comboSceneGroup = New MultiSelectCombo(cbSceneGroup, Me)

        toolTipFilters.SetToolTip(cbPriceRange,
                                  "Leave empty or set to 0 to skip price filtering. Type a custom whole-dollar amount or pick from the list.")

        If RestoreState IsNot Nothing Then RestoreFilterState()
    End Sub

    ' Restore all filter controls from a saved state (used by the back button)
    Private Sub RestoreFilterState()
        tbQuery.Text = RestoreState.Query
        comboDeveloper.SetCheckedItems(RestoreState.Developers)
        comboPublisher.SetCheckedItems(RestoreState.Publishers)
        comboSceneGroup.SetCheckedItems(RestoreState.SceneGroups)
        SetComboSelected(cbPriceOperator, RestoreState.PriceOperator)
        cbPriceRange.Text = RestoreState.PriceRange
        SetComboSelected(cbPriceCurrency, RestoreState.PriceCurrency)
    End Sub

    ' Find an item in a DropDownList combobox and select it
    Private Sub SetComboSelected(cb As ComboBox, text As String)
        If String.IsNullOrEmpty(text) Then
            cb.SelectedIndex = - 1
            Return
        End If
        Dim idx = cb.Items.IndexOf(text)
        If idx >= 0 Then
            cb.SelectedIndex = idx
        Else
            cb.SelectedIndex = - 1
        End If
    End Sub

    ' If the user typed a pure number, that's an AppID - lock the filters down
    Private Sub tbQuery_TextChanged(sender As Object, e As EventArgs) Handles tbQuery.TextChanged
        Dim text = tbQuery.Text.Trim()

        If IsAppId(text) Then
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = False
            Next
            ' Tooltip on the GroupBox and on each combobox inside it
            toolTipFilters.SetToolTip(gbFilters, "AppID detected in search query. Remove it to re-enable filters.")
            toolTipFilters.SetToolTip(cbDeveloper, "AppID detected in search query. Remove it to re-enable filters.")
            toolTipFilters.SetToolTip(cbPublisher, "AppID detected in search query. Remove it to re-enable filters.")
            toolTipFilters.SetToolTip(cbSceneGroup, "AppID detected in search query. Remove it to re-enable filters.")
        Else
            For Each ctrl In gbFilters.Controls
                ctrl.Enabled = True
            Next
            toolTipFilters.SetToolTip(gbFilters, Nothing)
            toolTipFilters.SetToolTip(cbDeveloper, Nothing)
            toolTipFilters.SetToolTip(cbPublisher, Nothing)
            toolTipFilters.SetToolTip(cbSceneGroup, Nothing)
        End If
    End Sub

    ' Block non-digit keypresses in the price range combobox
    Private Sub cbPriceRange_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbPriceRange.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Strip any non-digit characters that slip in via paste
    Private Sub cbPriceRange_TextChanged(sender As Object, e As EventArgs) Handles cbPriceRange.TextChanged
        Dim text = cbPriceRange.Text
        If String.IsNullOrEmpty(text) Then Return
        Dim clean = New String(text.Where(Function(c) Char.IsDigit(c)).ToArray())
        If clean <> text Then
            Dim pos = Math.Max(0, cbPriceRange.SelectionStart - (text.Length - clean.Length))
            cbPriceRange.Text = clean
            cbPriceRange.SelectionStart = pos
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
        Dim priceOperator = cbPriceOperator.Text
        Dim priceRange = cbPriceRange.Text
        Dim priceCurrency = cbPriceCurrency.Text

        If String.IsNullOrWhiteSpace(query) AndAlso
           String.IsNullOrWhiteSpace(developer) AndAlso
           String.IsNullOrWhiteSpace(publisher) AndAlso
           String.IsNullOrWhiteSpace(sceneGroup) AndAlso
           String.IsNullOrWhiteSpace(priceOperator) AndAlso
           String.IsNullOrWhiteSpace(priceCurrency) Then
            MessageBox.Show("Please enter at least one filter before searching.",
                            "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim url = BuildSearchUrl(query, developer, publisher, sceneGroup,
                                 priceOperator, priceRange, priceCurrency)

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

        Dim state = New SearchFilterState With {
                .Query = query,
                .Developers = developer,
                .Publishers = publisher,
                .SceneGroups = sceneGroup,
                .PriceOperator = priceOperator,
                .PriceRange = priceRange,
                .PriceCurrency = priceCurrency
                }
        Dim filterDisplay = BuildFiltersDisplay(query, publisher, developer, sceneGroup, state)
        NavigateToResults(json, filterDisplay, state)
    End Sub

    ' True if the price filter has a non-zero integer value and a valid operator/currency
    Private Function IsPriceFilterActive(priceOperator As String, priceRange As String, priceCurrency As String) _
        As Boolean
        If String.IsNullOrWhiteSpace(priceOperator) OrElse
           String.IsNullOrWhiteSpace(priceCurrency) Then Return False
        Dim value = 0
        Integer.TryParse(priceRange, value)
        Return value > 0
    End Function

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

        Dim state = New SearchFilterState With {.Query = appId}
        Dim filterDisplay = $"AppID: {appId}"
        NavigateToResults(json, filterDisplay, state)
    End Sub

    ' Helper to avoid repeating NavigateTo boilerplate.
    Private Sub NavigateToResults(json As String, filterDisplay As String, state As SearchFilterState)
        NavigateTo(Me, Function()
            Dim results As New frmResults()
            results.ResultsJson = json
            results.SearchFilters = filterDisplay
            results.FilterState = state
            Return results
        End Function)
    End Sub

    ' Deserialises the API JSON into a list of GameItem.
    Private Function ParseResultsJson(json As String) As List(Of GameItem)
        Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
        Dim root = JsonSerializer.Deserialize (Of GamesRoot)(json, options)
        Return If(root?.Games, New List(Of GameItem)())
    End Function

    ' True if the text is a pure 1-7 digit number (valid Steam AppID).
    Private Function IsAppId(text As String) As Boolean
        If String.IsNullOrWhiteSpace(text) Then Return False
        Return Regex.IsMatch(text, "^\d{1,7}$")
    End Function

    ' Builds the filter summary: "query" · publishers · developers · scene groups · price
    Private Function BuildFiltersDisplay(query As String, publisher As String,
                                         developer As String, sceneGroup As String, state As SearchFilterState) _
        As String
        Dim parts As New List(Of String)

        If Not String.IsNullOrWhiteSpace(query) Then parts.Add($"""{query}""")

        Dim pubDisplay = comboPublisher.GetCheckedItemsDisplay()
        Dim devDisplay = comboDeveloper.GetCheckedItemsDisplay()
        Dim sceneDisplay = comboSceneGroup.GetCheckedItemsDisplay()

        If Not String.IsNullOrWhiteSpace(pubDisplay) Then parts.Add(pubDisplay)
        If Not String.IsNullOrWhiteSpace(devDisplay) Then parts.Add(devDisplay)
        If Not String.IsNullOrWhiteSpace(sceneDisplay) Then parts.Add(sceneDisplay)

        If state IsNot Nothing AndAlso IsPriceFilterActive(state.PriceOperator, state.PriceRange, state.PriceCurrency) _
            Then
            Dim currencyCode = ExtractCurrencyCode(state.PriceCurrency)
            Dim symbol = GetCurrencySymbol(currencyCode)
            Dim amount = 0
            Integer.TryParse(state.PriceRange, amount)
            parts.Add($"Price {state.PriceOperator} {symbol}{amount} {currencyCode}")
        End If

        Return String.Join(" · ", parts)
    End Function

    ' Clears the query box and unchecks all MultiSelectCombos.
    Private Sub ResetFilters()
        tbQuery.Clear()
        comboPublisher.Reset()
        comboDeveloper.Reset()
        comboSceneGroup.Reset()
        SetComboSelected(cbPriceOperator, ">")
        cbPriceRange.Text = ""
        SetComboSelected(cbPriceCurrency, "USD ($)")
    End Sub

    ' Builds the encoded API URL - only non-blank params are included.
    Private Function BuildSearchUrl(query As String, developer As String,
                                    publisher As String, sceneGroup As String,
                                    priceOperator As String, priceRange As String, priceCurrency As String) As String
        Dim parts As New List(Of String)

        If Not String.IsNullOrWhiteSpace(query) Then parts.Add($"q={Uri.EscapeDataString(query)}")
        If Not String.IsNullOrWhiteSpace(developer) Then parts.Add($"developer={Uri.EscapeDataString(developer)}")
        If Not String.IsNullOrWhiteSpace(publisher) Then parts.Add($"publisher={Uri.EscapeDataString(publisher)}")
        If Not String.IsNullOrWhiteSpace(sceneGroup) Then parts.Add($"scene_group={Uri.EscapeDataString(sceneGroup)}")

        If IsPriceFilterActive(priceOperator, priceRange, priceCurrency) Then
            Dim amount = 0
            Integer.TryParse(priceRange, amount)
            parts.Add($"price_operator={Uri.EscapeDataString(priceOperator)}")
            parts.Add($"price_value={amount}")
            parts.Add($"price_currency={Uri.EscapeDataString(ExtractCurrencyCode(priceCurrency))}")
        End If

        Return $"http://localhost:5050/search?{String.Join("&", parts)}"
    End Function

    ' Toggle between dark and light theme
    Private Sub btnThemeToggle_Click(sender As Object, e As EventArgs) Handles btnThemeToggle.Click
        ToggleTheme(Me)
    End Sub
End Class
