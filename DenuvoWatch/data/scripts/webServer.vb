Imports System.Net
Imports System.Text.Json
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http

' =============================================================================
' Module: WebServer
' -----------------------------------------------------------------------------
' Embedded ASP.NET Core Kestrel host serving the /search API endpoint on
' localhost:5050. Started by Utils.StartWebServer after game data is loaded,
' and stopped by Utils.StopWebServer on app exit.
'
' Endpoint:
'   GET /search?q={query}&developer={devs}&publisher={pubs}&scene_group={groups}
'     Returns JSON in the same shape as data/games.json — a "games" array
'     where each item has title, sort_title, cover_url, game_info {developer,
'     publisher, release_date} and crack_info {crack_status, crack_date,
'     crack_date_relative, scene_group}.
' =============================================================================
Public Module WebServer
    ' Singleton web application instance. Nothing when the server is stopped.
    Friend webApp As WebApplication

    ' JSON serializer options shared across all responses.
    Private ReadOnly jsonOpts As New JsonSerializerOptions With {
        .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        .WriteIndented = False
        }

    ' ---------------------------------------------------------------------------
    ' CreateWebApp
    '   Builds and configures the Kestrel web application listening on port
    '   5050 on the loopback adapter only.
    ' ---------------------------------------------------------------------------
    Friend Function CreateWebApp() As WebApplication
        Dim builder = WebApplication.CreateBuilder()

        ' Bind to localhost
        builder.WebHost.ConfigureKestrel(Sub(options)
            options.Listen(IPAddress.Loopback, 5050)
        End Sub)

        Dim app = builder.Build()
        ConfigureEndpoints(app)
        Return app
    End Function

    ' ---------------------------------------------------------------------------
    ' ConfigureEndpoints
    '   Wires up every route the server should handle.
    ' ---------------------------------------------------------------------------
    Private Sub ConfigureEndpoints(app As WebApplication)
        ' This is the main search endpoint that frmSearch calls
        app.MapGet("/search", Function(ctx As HttpContext) As IResult
            Return HandleSearch(ctx)
        End Function)
    End Sub

    ' ---------------------------------------------------------------------------
    ' HandleSearch
    '   Reads the query-string filters. If "appid" is present, I search by
    '   appid only and ignore everything else. Otherwise I do a normal
    '   fuzzy text + filter search.
    '
    ' Query parameters:
    '   appid       – Steam appid (1-7 digits), takes priority over everything
    '   q           – fuzzy text search (matched against title / sort_title)
    '   developer   – comma-separated developer names (OR match)
    '   publisher   – comma-separated publisher names (OR match)
    '   scene_group – comma-separated scene group names (OR match)
    ' ---------------------------------------------------------------------------
    Private Function HandleSearch(ctx As HttpContext) As IResult
        Dim appid = ctx.Request.Query("appid").ToString()

        Dim games As List(Of GameItem)

        ' If an appid was provided, I search only by that — ignore all other filters
        If Not String.IsNullOrWhiteSpace(appid) Then
            games = GamesData.FilterByAppId(appid)
        Else
            Dim query = ctx.Request.Query("q").ToString()
            Dim developer = ctx.Request.Query("developer").ToString()
            Dim publisher = ctx.Request.Query("publisher").ToString()
            Dim sceneGroup = ctx.Request.Query("scene_group").ToString()
            games = GamesData.FilterGames(query, developer, publisher, sceneGroup)
        End If

        ' Wrap it up in the same { "games": [...] } shape as the original games.json
        Dim resultObj As New With {.games = games}
        Dim json = JsonSerializer.Serialize(resultObj, jsonOpts)

        Return Results.Content(json, "application/json")
    End Function
End Module
