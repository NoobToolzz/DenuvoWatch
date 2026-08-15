Imports System.Linq
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.Json.Serialization

' GamesData — holds all the games we loaded from the JSON on GitHub.
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

            Dim root = JsonSerializer.Deserialize(Of GamesRoot)(json, options)
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
                                publishers As String, sceneGroups As String) As List(Of GameItem)
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
            result = result.Where(Function(g) list.Any(Function(d) String.Equals(g.GameInfo?.Developer, d, StringComparison.OrdinalIgnoreCase)))
        End If

        If Not String.IsNullOrWhiteSpace(publishers) Then
            Dim list = publishers.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(Function(g) list.Any(Function(p) String.Equals(g.GameInfo?.Publisher, p, StringComparison.OrdinalIgnoreCase)))
        End If

        If Not String.IsNullOrWhiteSpace(sceneGroups) Then
            Dim list = sceneGroups.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(Function(g) list.Any(Function(s) String.Equals(g.CrackInfo?.SceneGroup, s, StringComparison.OrdinalIgnoreCase)))
        End If

        Return result.ToList()
    End Function

    ' Direct AppID lookup — straight equality, no fuzzy matching needed.
    Public Function FilterByAppId(appId As String) As List(Of GameItem)
        Dim id = appId.Trim()
        Return AllGames.Where(Function(g) String.Equals(g.GameInfo?.AppId, id, StringComparison.OrdinalIgnoreCase)).ToList()
    End Function

    ' Throw away anything that isn't a letter, number, or space
    Private Function NormalizeForSearch(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then Return ""
        Dim sb As New Text.StringBuilder()
        For Each c In s.ToLowerInvariant()
            If Char.IsLetterOrDigit(c) OrElse c = " "c Then sb.Append(c)
        Next
        Return System.Text.RegularExpressions.Regex.Replace(sb.ToString(), "\s+", " ").Trim()
    End Function
End Module

' JSON model classes — match the shape of games.json.

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
