Imports System.Net
Imports System.Text.Json
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http

' =============================================================================
' Module: WebServer
' Local API that serves search results as JSON on localhost:5050.
' =============================================================================
Public Module WebServer
    Friend webApp As WebApplication

    Private ReadOnly jsonOpts As New JsonSerializerOptions With {
        .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        .WriteIndented = False
        }

    ' Set up the web server on localhost only
    Friend Function CreateWebApp() As WebApplication
        Dim builder = WebApplication.CreateBuilder()

        builder.WebHost.ConfigureKestrel(Sub(options)
            options.Listen(IPAddress.Loopback, 5050)
        End Sub)

        Dim app = builder.Build()
        ConfigureEndpoints(app)
        Return app
    End Function

    Private Sub ConfigureEndpoints(app As WebApplication)
        app.MapGet("/search", Function(ctx As HttpContext) As IResult
            Return HandleSearch(ctx)
        End Function)
    End Sub

    ' If they gave an appid, just search by that. Otherwise do a normal search
    Private Function HandleSearch(ctx As HttpContext) As IResult
        Dim appid = ctx.Request.Query("appid").ToString()
        Dim games As List(Of GameItem)

        If Not String.IsNullOrWhiteSpace(appid) Then
            games = FilterByAppId(appid)
        Else
            Dim query = ctx.Request.Query("q").ToString()
            Dim developer = ctx.Request.Query("developer").ToString()
            Dim publisher = ctx.Request.Query("publisher").ToString()
            Dim sceneGroup = ctx.Request.Query("scene_group").ToString()
            Dim priceOperator = ctx.Request.Query("price_operator").ToString()
            Dim priceValue = ctx.Request.Query("price_value").ToString()
            Dim priceCurrency = ctx.Request.Query("price_currency").ToString()
            games = FilterGames(query, developer, publisher, sceneGroup,
                                priceOperator, priceValue, priceCurrency)
        End If

        Dim resultObj = New With {.games = games}
        Dim json = JsonSerializer.Serialize(resultObj, jsonOpts)

        Return Results.Content(json, "application/json")
    End Function
End Module
