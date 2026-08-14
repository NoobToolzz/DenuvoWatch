Imports System.Linq
Imports System.Net.Http
Imports System.Text.Json
Imports System.Text.Json.Serialization

' =============================================================================
' Module: GamesData
' -----------------------------------------------------------------------------
' In-memory game catalogue backed by a remote games.json hosted on GitHub.
' Fetches the raw JSON at startup into AllGames and exposes helper queries for
' the unique filter values (developers, publishers, scene groups) plus the
' main FilterGames search used by the /search endpoint.
' =============================================================================
Public Module GamesData

    ' Raw URL of the canonical games.json on GitHub (main branch).
    Private Const GamesJsonUrl As String =
        "https://raw.githubusercontent.com/NoobToolzz/DenuvoWatch/refs/heads/main/data/games.json"

    ' Shared HttpClient instance - reused across calls to avoid socket exhaustion.
    Private ReadOnly client As New HttpClient()

    ' The full loaded game collection; empty list until LoadGames is called.
    Public Property AllGames As List(Of GameItem) = New List(Of GameItem)()

    ' ---------------------------------------------------------------------------
    ' LoadGames
    '   Fetches games.json from the remote GitHub raw URL, deserialises it
    '   into AllGames, and logs a summary line. On any failure AllGames is
    '   left unchanged and the error is written to the console.
    '
    ' JSON options:
    '   - PropertyNameCaseInsensitive = True   (tolerant of casing drift)
    '   - PropertyNamingPolicy = SnakeCaseLower (matches the snake_case keys)
    ' ---------------------------------------------------------------------------
    Public Sub LoadGames()
        Try
            Console.WriteLine("Fetching games from: " & GamesJsonUrl)

            Dim json = client.GetStringAsync(GamesJsonUrl).Result

            Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            }

            Dim root = JsonSerializer.Deserialize(Of GamesRoot)(json, options)
            ' Fall back to an empty list when the JSON has no games array.
            AllGames = If(root?.Games, New List(Of GameItem)())

            Console.WriteLine($"Loaded {AllGames.Count} games")

        Catch ex As Exception
            Console.WriteLine("Failed to fetch games.json: " & ex.Message)
        End Try
    End Sub

    ' ---------------------------------------------------------------------------
    ' GetUniqueDevelopers
    '   Returns the distinct, case-insensitively sorted list of developer
    '   names across all loaded games (blanks excluded).
    '
    ' Example:
    '   { "CD Projekt Red", "EA Vancouver", "Square Enix" }
    ' ---------------------------------------------------------------------------
    Public Function GetUniqueDevelopers() As List(Of String)
        Return AllGames.
            Select(Function(g) g.GameInfo?.Developer).
            Where(Function(d) Not String.IsNullOrWhiteSpace(d)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(d) d).
            ToList()
    End Function

    ' ---------------------------------------------------------------------------
    ' GetUniquePublishers
    '   Returns the distinct, case-insensitively sorted list of publisher
    '   names across all loaded games (blanks excluded).
    ' ---------------------------------------------------------------------------
    Public Function GetUniquePublishers() As List(Of String)
        Return AllGames.
            Select(Function(g) g.GameInfo?.Publisher).
            Where(Function(p) Not String.IsNullOrWhiteSpace(p)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(p) p).
            ToList()
    End Function

    ' ---------------------------------------------------------------------------
    ' GetUniqueSceneGroups
    '   Returns the distinct, case-insensitively sorted list of scene group
    '   names across all loaded games (blanks excluded).
    ' ---------------------------------------------------------------------------
    Public Function GetUniqueSceneGroups() As List(Of String)
        Return AllGames.
            Select(Function(g) g.CrackInfo?.SceneGroup).
            Where(Function(s) Not String.IsNullOrWhiteSpace(s)).
            Distinct(StringComparer.OrdinalIgnoreCase).
            OrderBy(Function(s) s).
            ToList()
    End Function

    ' ---------------------------------------------------------------------------
    ' FilterGames
    '   Applies the four optional filters to AllGames and returns the matching
    '   subset as a new list. Every filter is AND-combined; within the
    '   developer / publisher / scene-group filters a comma-separated string
    '   is treated as an OR of the individual values.
    '
    ' Parameters:
    '   search      - substring matched against Title or SortTitle (case-ins.)
    '   developers  - comma-separated developer names (OR), or blank for all
    '   publishers   - comma-separated publisher names (OR), or blank for all
    '   sceneGroups - comma-separated scene group names (OR), or blank for all
    '
    ' Example:
    '   FilterGames("cyberpunk", "CD Projekt Red", "", "")
    '   -> games whose title contains "cyberpunk" by CD Projekt Red
    ' ---------------------------------------------------------------------------
    Public Function FilterGames(search As String,
                                developers As String,
                                publishers As String,
                                sceneGroups As String) As List(Of GameItem)

        Dim result = AllGames.AsEnumerable()

        ' Free-text search across title and sort title.
        If Not String.IsNullOrWhiteSpace(search) Then
            Dim term = search.Trim().ToLowerInvariant()
            result = result.Where(Function(g)
                Return (g.Title IsNot Nothing AndAlso g.Title.ToLowerInvariant().Contains(term)) OrElse
                       (g.SortTitle IsNot Nothing AndAlso g.SortTitle.ToLowerInvariant().Contains(term))
            End Function)
        End If

        ' Developer filter: comma-separated list -> OR match.
        If Not String.IsNullOrWhiteSpace(developers) Then
            Dim list = developers.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(Function(g) list.Any(Function(d) String.Equals(g.GameInfo?.Developer, d, StringComparison.OrdinalIgnoreCase)))
        End If

        ' Publisher filter: comma-separated list -> OR match.
        If Not String.IsNullOrWhiteSpace(publishers) Then
            Dim list = publishers.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(Function(g) list.Any(Function(p) String.Equals(g.GameInfo?.Publisher, p, StringComparison.OrdinalIgnoreCase)))
        End If

        ' Scene group filter: comma-separated list -> OR match.
        If Not String.IsNullOrWhiteSpace(sceneGroups) Then
            Dim list = sceneGroups.Split(","c).Select(Function(x) x.Trim()).Where(Function(x) x <> "").ToList()
            result = result.Where(Function(g) list.Any(Function(s) String.Equals(g.CrackInfo?.SceneGroup, s, StringComparison.OrdinalIgnoreCase)))
        End If

        Return result.ToList()
    End Function

End Module

' =============================================================================
' JSON model classes (System.Text.Json)
' Each class mirrors the shape of data/games.json. Property names use
' JsonPropertyName to map snake_case JSON keys to PascalCase VB properties.
' =============================================================================

' Root object: { "games": [ ... ] }
Public Class GamesRoot
    <JsonPropertyName("games")>
    Public Property Games As List(Of GameItem)
End Class

' Single game entry.
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

' General release metadata.
Public Class GameInfo
    <JsonPropertyName("developer")>
    Public Property Developer As String

    <JsonPropertyName("publisher")>
    Public Property Publisher As String

    <JsonPropertyName("release_date")>
    Public Property ReleaseDate As String
End Class

' DRM / crack metadata.
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
