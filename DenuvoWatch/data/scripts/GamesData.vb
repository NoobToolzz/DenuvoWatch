Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Text.RegularExpressions

' =============================================================================
' Module: GamesData
' Holds all the games we loaded from the JSON on GitHub and lets us search them.
' =============================================================================
Public Module GamesData
    Private Const GamesJsonUrl As String =
        "https://raw.githubusercontent.com/NoobToolzz/DenuvoWatch/refs/heads/main/DenuvoWatch/data/games.json"

    Private ReadOnly client As New HttpClient()

    Public Property AllGames As List(Of GameItem) = New List(Of GameItem)()

    ' Grab the JSON from GitHub and load it into AllGames
    Public Sub LoadGames()
        Try
            Console.WriteLine($"Fetching games from: {GamesJsonUrl}")

            Dim json = client.GetStringAsync(GamesJsonUrl).Result

            Dim options As New JsonSerializerOptions With {
                    .PropertyNameCaseInsensitive = True,
                    .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }

            Dim root = JsonSerializer.Deserialize (Of GamesRoot)(json, options)
            AllGames = If(root?.Games, New List(Of GameItem)())

            Console.WriteLine($"Loaded {AllGames.Count} games")
        Catch ex As Exception
            Console.WriteLine($"Failed to fetch games.json: {ex.Message}")
        End Try
    End Sub

    ' Unique developer names, sorted, no blanks
    Public Function GetUniqueDevelopers() As List(Of String)
        Return AllGames.
            Select(Function(g) g.GameInfo?.Developer).
            Where(Function(d) Not String.IsNullOrWhiteSpace(d)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(d) d).
            ToList()
    End Function

    ' Unique publisher names, sorted, no blanks
    Public Function GetUniquePublishers() As List(Of String)
        Return AllGames.
            Select(Function(g) g.GameInfo?.Publisher).
            Where(Function(p) Not String.IsNullOrWhiteSpace(p)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(p) p).
            ToList()
    End Function

    ' Unique scene group names, sorted, no blanks
    Public Function GetUniqueSceneGroups() As List(Of String)
        Return AllGames.
            Select(Function(g) g.CrackInfo?.SceneGroup).
            Where(Function(s) Not String.IsNullOrWhiteSpace(s)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(s) s).
            ToList()
    End Function

    ' Fuzzy text search + comma-separated filters. Strips special chars so
    ' "resident evil 4" matches "Resident Evil: 4".
    Public Function FilterGames(search As String, developers As String,
                                publishers As String, sceneGroups As String,
                                priceOperator As String, priceValue As String, priceCurrency As String) _
        As List(Of GameItem)
        Dim result = AllGames.AsEnumerable()

        If Not String.IsNullOrWhiteSpace(search) Then
            Dim normalizedTerm = NormalizeForSearch(search)
            result = result.Where(Function(g)
                Dim normTitle = NormalizeForSearch(If(g.Title, ""))
                Dim normSort = NormalizeForSearch(If(g.SortTitle, ""))
                Return normTitle.Contains(normalizedTerm) OrElse normSort.Contains(normalizedTerm)
            End Function)
        End If

        If Not String.IsNullOrWhiteSpace(developers) Then
            Dim list = developers.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(
                Function(g) list.Any(
                    Function(d) String.Equals(g.GameInfo?.Developer, d, StringComparison.OrdinalIgnoreCase)))
        End If

        If Not String.IsNullOrWhiteSpace(publishers) Then
            Dim list = publishers.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(
                Function(g) list.Any(
                    Function(p) String.Equals(g.GameInfo?.Publisher, p, StringComparison.OrdinalIgnoreCase)))
        End If

        If Not String.IsNullOrWhiteSpace(sceneGroups) Then
            Dim list = sceneGroups.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(
                Function(g) list.Any(
                    Function(s) String.Equals(g.CrackInfo?.SceneGroup, s, StringComparison.OrdinalIgnoreCase)))
        End If

        If Not String.IsNullOrWhiteSpace(priceCurrency) AndAlso
           Not String.IsNullOrWhiteSpace(priceOperator) Then
            Dim threshold As Decimal = 0
            Dim hasValue = Decimal.TryParse(priceValue, threshold)
            If hasValue AndAlso threshold > 0 Then
                Dim currencyCode = ExtractCurrencyCode(priceCurrency)
                result = result.Where(Function(g)
                    Dim priceStr = GetPriceForCurrency(g, currencyCode)
                    If String.IsNullOrWhiteSpace(priceStr) Then Return False
                    Dim gamePrice As Decimal = 0
                    If Not Decimal.TryParse(priceStr, gamePrice) Then Return False
                    Select Case priceOperator
                        Case ">" : Return gamePrice > threshold
                        Case "<" : Return gamePrice < threshold
                        Case "=" : Return gamePrice = threshold
                        Case Else : Return True
                    End Select
                End Function)
            End If
        End If

        Return result.ToList()
    End Function

    ' Direct AppID lookup - straight equality, no fuzzy matching needed.
    Public Function FilterByAppId(appId As String) As List(Of GameItem)
        Dim id = appId.Trim()
        Return AllGames.Where(
            Function(g) String.Equals(g.GameInfo?.AppId, id, StringComparison.OrdinalIgnoreCase)).ToList()
    End Function

    ' Throw away anything that isn't a letter, number, or space
    Private Function NormalizeForSearch(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then Return ""
        Dim sb As New StringBuilder()
        For Each c In s.ToLowerInvariant()
            If Char.IsLetterOrDigit(c) OrElse c = " "c Then sb.Append(c)
        Next
        Return Regex.Replace(sb.ToString(), "\s+", " ").Trim()
    End Function

    ' Pull "USD" out of a display string like "USD ($)"
    Public Function ExtractCurrencyCode(display As String) As String
        If String.IsNullOrWhiteSpace(display) Then Return ""
        Dim idx = display.IndexOf("("c)
        If idx > 0 Then Return display.Substring(0, idx).Trim()
        Return display.Trim()
    End Function

    ' Currency symbol for display
    Public Function GetCurrencySymbol(code As String) As String
        Select Case code.ToUpperInvariant()
            Case "USD" : Return "$"
            Case "AUD" : Return "A$"
            Case "EUR" : Return "€"
            Case Else : Return ""
        End Select
    End Function

    ' Get the price string for a given currency code from a game
    Public Function GetPriceForCurrency(g As GameItem, currencyCode As String) As String
        If g.GameInfo?.Prices Is Nothing Then Return Nothing
        Select Case currencyCode.ToUpperInvariant()
            Case "USD" : Return g.GameInfo.Prices.USD
            Case "AUD" : Return g.GameInfo.Prices.AUD
            Case "EUR" : Return g.GameInfo.Prices.EUR
            Case Else : Return Nothing
        End Select
    End Function
End Module

' JSON model classes - match the shape of games.json.

Public Class GamesRoot
    <JsonPropertyName("games")>
    Public Property Games As List(Of GameItem)
End Class

Public Class GameItem
    <JsonPropertyName("title")>
    Public Property Title As String

    <JsonPropertyName("sort_title")>
    Public Property SortTitle As String

    <JsonPropertyName("cover_url")>
    Public Property CoverUrl As String

    <JsonPropertyName("game_info")>
    Public Property GameInfo As GameInfo

    <JsonPropertyName("crack_info")>
    Public Property CrackInfo As CrackInfo
End Class

Public Class GameInfo
    <JsonPropertyName("appid")>
    Public Property AppId As String

    <JsonPropertyName("developer")>
    Public Property Developer As String

    <JsonPropertyName("publisher")>
    Public Property Publisher As String

    <JsonPropertyName("release_date")>
    Public Property ReleaseDate As String

    <JsonPropertyName("prices")>
    Public Property Prices As GamePrices
End Class

Public Class GamePrices
    <JsonPropertyName("USD")>
    Public Property USD As String

    <JsonPropertyName("AUD")>
    Public Property AUD As String

    <JsonPropertyName("EUR")>
    Public Property EUR As String
End Class

Public Class CrackInfo
    <JsonPropertyName("crack_status")>
    Public Property CrackStatus As String

    <JsonPropertyName("crack_date")>
    Public Property CrackDate As String

    <JsonPropertyName("crack_date_relative")>
    Public Property CrackDateRelative As String

    <JsonPropertyName("scene_group")>
    Public Property SceneGroup As String
End Class

Public Class SearchFilterState
    Public Property Query As String
    Public Property Developers As String
    Public Property Publishers As String
    Public Property SceneGroups As String
    Public Property PriceOperator As String
    Public Property PriceRange As String
    Public Property PriceCurrency As String
End Class
