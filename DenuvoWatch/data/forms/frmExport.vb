Imports System.ComponentModel
Imports System.IO
Imports System.Security
Imports System.Text
Imports System.Text.Json

' =============================================================================
' Form: frmExport
' Pick a format, see a live preview, tweak columns and sorting, save to a file.
' =============================================================================
Public Class frmExport
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ResultsJson As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SearchFilters As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property FilterState As SearchFilterState

    Private games As List(Of GameItem)

    Private ReadOnly columnKeys As String() =
                         {"title", "developer", "publisher", "release_date", "crack_status", "crack_date", "scene_group"}

    Private ReadOnly columnLabels As String() =
                         {"Title", "Developer", "Publisher", "Release Date", "Crack Status", "Crack Date", "Scene Group"}

    Private ReadOnly formatRadios As New List(Of RadioButton)
    Private ReadOnly columnChecks As New List(Of CheckBox)
    Private ReadOnly sortRadios As New List(Of RadioButton)

    ' Parse JSON, group up the controls, style buttons, show the preview
    Private Sub frmExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        formatRadios.AddRange({rbFormatText, rbFormatCSV, rbFormatJSON, rbFormatHTML, rbFormatMarkdown, rbFormatXML})
        columnChecks.AddRange({ _
                                  cbColTitle, cbColDeveloper, cbColPublisher, cbColReleaseDate,
                                  cbColCrackStatus, cbColCrackDate, cbColSceneGroup
                              })
        sortRadios.AddRange({rbSortNone, rbSortTitleAZ, rbSortTitleZA, rbSortCrackStatus, rbSortReleaseDate})

        Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }
        Dim root = JsonSerializer.Deserialize (Of GamesRoot)(ResultsJson, options)
        games = If(root?.Games, New List(Of GameItem)())

        StyleFormButtons(Me)
        ApplyTheme(Me)
        UpdatePreview()
    End Sub

    ' Rebuild the preview. Bail if games isn't ready yet (before Load fires)
    Private Sub UpdatePreview()
        If games Is Nothing Then Return
        rtbExportPreview.Text = GenerateExport(ApplySorting(games))
    End Sub

    ' Which columns the user ticked
    Private Function GetSelectedColumns() As List(Of String)
        Dim cols As New List(Of String)
        For i = 0 To columnChecks.Count - 1
            If columnChecks(i).Checked Then cols.Add(columnLabels(i))
        Next
        Return cols
    End Function

    ' Sort the games based on which radio is picked
    Private Function ApplySorting(source As List(Of GameItem)) As List(Of GameItem)
        If source Is Nothing Then Return New List(Of GameItem)()
        If rbSortTitleAZ.Checked Then Return source.OrderBy(Function(g) g.Title).ToList()
        If rbSortTitleZA.Checked Then Return source.OrderByDescending(Function(g) g.Title).ToList()
        If rbSortCrackStatus.Checked Then Return source.OrderBy(Function(g) If(g.CrackInfo?.CrackStatus, "")).ToList()
        If rbSortReleaseDate.Checked Then Return source.OrderBy(Function(g) If(g.GameInfo?.ReleaseDate, "")).ToList()
        Return source.ToList()
    End Function

    ' Send to the right format generator
    Private Function GenerateExport(games As List(Of GameItem)) As String
        If rbFormatText.Checked Then Return GenerateText(games)
        If rbFormatCSV.Checked Then Return GenerateCSV(games)
        If rbFormatJSON.Checked Then Return GenerateJSON(games)
        If rbFormatHTML.Checked Then Return GenerateHTML(games)
        If rbFormatMarkdown.Checked Then Return GenerateMarkdown(games)
        If rbFormatXML.Checked Then Return GenerateXML(games)
        Return ""
    End Function

    ' Get a field value by column name
    Private Function GetFieldValue(g As GameItem, columnLabel As String) As String
        Select Case columnLabel
            Case "Title" : Return If(g.Title, "")
            Case "Developer" : Return If(g.GameInfo?.Developer, "")
            Case "Publisher" : Return If(g.GameInfo?.Publisher, "")
            Case "Release Date" : Return If(g.GameInfo?.ReleaseDate, "")
            Case "Crack Status" : Return If(g.CrackInfo?.CrackStatus, "")
            Case "Crack Date"
                Dim dt = If(g.CrackInfo?.CrackDate, "")
                Dim rel = If(g.CrackInfo?.CrackDateRelative, "")
                If Not String.IsNullOrWhiteSpace(dt) Then
                    Return If(String.IsNullOrWhiteSpace(rel), dt, $"{dt} ({rel})")
                End If
                Return ""
            Case "Scene Group" : Return If(g.CrackInfo?.SceneGroup, "")
            Case Else : Return ""
        End Select
    End Function

    ' Steam URL for a game, or "" if no AppID
    Private Function GetSteamUrl(g As GameItem) As String
        Dim appid = If(g.GameInfo?.AppId, "")
        If String.IsNullOrWhiteSpace(appid) Then Return ""
        Return $"https://store.steampowered.com/app/{appid}"
    End Function

    ' What filters were used, for the header
    Private Function GetFilterHeader() As String
        Dim f = If(SearchFilters, "").Trim()
        If String.IsNullOrWhiteSpace(f) Then Return $"Exported {DateTime.Now.ToString("yyyy-MM-dd")}"
        Return f
    End Function

    ' Text - boxed blocks with borders. AppID in brackets, Steam link at the bottom
    Private Function GenerateText(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return ""

        Dim sb As New StringBuilder()
        sb.AppendLine("═══════════════════════════════════════════════════════")
        sb.AppendLine("  DenuvoWatch Export")
        sb.AppendLine($"  Filters: {GetFilterHeader()}")
        sb.AppendLine($"  Exported: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}")
        sb.AppendLine("═══════════════════════════════════════════════════════")
        sb.AppendLine()

        For Each g In games
            Dim titleText = g.Title
            Dim appid = If(g.GameInfo?.AppId, "")
            If Not String.IsNullOrWhiteSpace(appid) Then
                titleText &= $" [AppID: {appid}]"
            End If

            Dim titleLine = $"┌─ {titleText} "
            Dim innerWidth = Math.Max(50, titleLine.Length + 1)
            sb.AppendLine(titleLine & New String("─"c, innerWidth - titleLine.Length) & "┐")

            For i = 0 To cols.Count - 1
                Dim value = GetFieldValue(g, cols(i))
                If String.IsNullOrWhiteSpace(value) Then value = "—"
                sb.AppendLine("│ " & cols(i).PadRight(14) & " │ " & value)
            Next

            Dim steamUrl = GetSteamUrl(g)
            If Not String.IsNullOrWhiteSpace(steamUrl) Then
                sb.AppendLine("│ " & "Steam Link".PadRight(14) & " │ " & steamUrl)
            End If

            sb.AppendLine("└" & New String("─"c, innerWidth - 2) & "┘")
            sb.AppendLine()
        Next

        sb.AppendLine($"Total: {games.Count} game(s)")
        Return sb.ToString()
    End Function

    ' CSV - always include AppID and Steam Link columns
    Private Function GenerateCSV(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return ""

        Dim sb As New StringBuilder()
        sb.AppendLine($"# DenuvoWatch Export")
        sb.AppendLine($"# Filters: {GetFilterHeader()}")
        sb.AppendLine($"# Exported: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}")
        sb.AppendLine($"# Count: {games.Count} game(s)")

        Dim allCols = cols.ToList()
        allCols.AddRange({"AppID", "Steam Link"})

        sb.AppendLine(String.Join(",", allCols.Select(Function(c) EscapeCsv(c))))

        For Each g In games
            Dim values = allCols.Select(Function(c)
                If c = "AppID" Then Return EscapeCsv(If(g.GameInfo?.AppId, ""))
                If c = "Steam Link" Then Return EscapeCsv(GetSteamUrl(g))
                Return EscapeCsv(GetFieldValue(g, c))
            End Function)
            sb.AppendLine(String.Join(",", values))
        Next

        Return sb.ToString()
    End Function

    ' JSON - pretty, with metadata and appid + steam_link per game
    Private Function GenerateJSON(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return "{}"

        Dim options As New JsonSerializerOptions With {
                .WriteIndented = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }

        Dim rows As New List(Of Dictionary(Of String, String))
        For Each g In games
            Dim row As New Dictionary(Of String, String)
            For Each c In cols
                row(columnKeys(Array.IndexOf(columnLabels, c))) = GetFieldValue(g, c)
            Next
            row("appid") = If(g.GameInfo?.AppId, "")
            row("steam_link") = GetSteamUrl(g)
            rows.Add(row)
        Next

        Dim result As New Dictionary(Of String, Object)
        result("_filters") = GetFilterHeader()
        result("_exported_at") = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")
        result("_count") = games.Count
        result("games") = rows

        Return JsonSerializer.Serialize(result, options)
    End Function

    ' HTML - styled page, AppID hyperlinked, crack status color-coded
    Private Function GenerateHTML(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return ""

        Dim sb As New StringBuilder()
        sb.AppendLine("<!DOCTYPE html>")
        sb.AppendLine("<html lang='en'>")
        sb.AppendLine("<head>")
        sb.AppendLine("  <meta charset='UTF-8'>")
        sb.AppendLine("  <title>DenuvoWatch Export</title>")
        sb.AppendLine("  <style>")
        sb.AppendLine("    * { box-sizing: border-box; margin: 0; padding:0; }")
        sb.AppendLine("    body { font-family:'Segoe UI',sans-serif; background:#0a0a0c; color:#ececef; padding:24px; }")
        sb.AppendLine("    h1 { font-size:22px; margin-bottom:4px; }")
        sb.AppendLine("    .filters { color:#9a9aa3; font-size:13px; margin-bottom:20px; }")
        sb.AppendLine("    .meta { color:#5a5a63; font-size:11px; margin-bottom:16px; }")
        sb.AppendLine("    table { border-collapse:collapse; width:100%; font-size:13px; }")
        sb.AppendLine(
            "    th { background:#1a1a2e; color:#e0e0e0; border:1px solid #34343c; padding:8px 12px; text-align:left; }")
        sb.AppendLine("    td { border:1px solid #24242a; padding:8px 12px; color:#ccc; }")
        sb.AppendLine("    tr:nth-child(even) td { background:#111114; }")
        sb.AppendLine("    .cracked { color:#34d399; font-weight:600; }")
        sb.AppendLine("    .uncracked { color:#f43f5e; font-weight:600; }")
        sb.AppendLine("    .hypervisor { color:#fbbf24; font-weight:600; }")
        sb.AppendLine("    .appid-link { color:#00d9a3; text-decoration:none; }")
        sb.AppendLine("    .appid-link:hover { text-decoration:underline; }")
        sb.AppendLine("    .footer { margin-top:20px; color:#5a5a63; font-size:11px; }")
        sb.AppendLine("  </style>")
        sb.AppendLine("</head>")
        sb.AppendLine("<body>")
        sb.AppendLine("  <h1>DenuvoWatch Export</h1>")
        sb.AppendLine($"  <p class='filters'><strong>Filters:</strong> {GetFilterHeader()}</p>")
        sb.AppendLine(
            $"  <p class='meta'>Exported {DateTime.Now.ToString("yyyy-MM-dd HH:mm")} · {games.Count} game(s)</p>")
        sb.AppendLine("  <table>")

        sb.AppendLine("    <tr>")
        For Each c In cols
            sb.AppendLine($"      <th>{c}</th>")
        Next
        sb.AppendLine("    </tr>")

        For Each g In games
            sb.AppendLine("    <tr>")
            For Each c In cols
                Dim val = GetFieldValue(g, c)
                Dim cssClass = ""

                ' Color the crack status: green/red/yellow
                If c = "Crack Status" Then
                    If val?.Equals("Cracked", StringComparison.OrdinalIgnoreCase) Then cssClass = " class='cracked'"
                    If val?.Equals("Uncracked", StringComparison.OrdinalIgnoreCase) Then cssClass = " class='uncracked'"
                    If val?.Equals("Hypervisor", StringComparison.OrdinalIgnoreCase) Then _
                        cssClass = " class='hypervisor'"
                End If

                ' Link the AppID in the title
                If c = "Title" Then
                    Dim appid = If(g.GameInfo?.AppId, "")
                    Dim steamUrl = GetSteamUrl(g)
                    If Not String.IsNullOrWhiteSpace(appid) AndAlso Not String.IsNullOrWhiteSpace(steamUrl) Then
                        val &= $" [<a class='appid-link' href='{steamUrl}'>AppID: {appid}</a>]"
                    End If
                End If

                sb.AppendLine($"      <td{cssClass}>{val}</td>")
            Next
            sb.AppendLine("    </tr>")
        Next

        sb.AppendLine("  </table>")
        sb.AppendLine("  <p class='footer'>Generated by DenuvoWatch</p>")
        sb.AppendLine("</body>")
        sb.AppendLine("</html>")
        Return sb.ToString()
    End Function

    ' Markdown - AppID linked, crack status gets emojis, Steam link at the bottom
    Private Function GenerateMarkdown(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return ""

        Dim sb As New StringBuilder()
        sb.AppendLine("# DenuvoWatch Export")
        sb.AppendLine()
        sb.AppendLine($"> **Filters:** {GetFilterHeader()}")
        sb.AppendLine()
        sb.AppendLine($"> _Exported {DateTime.Now.ToString("yyyy-MM-dd HH:mm")} · {games.Count} game(s)_")
        sb.AppendLine()
        sb.AppendLine("---")
        sb.AppendLine()

        For idx = 0 To games.Count - 1
            Dim g = games(idx)

            ' AppID as a Markdown link in the heading
            Dim heading = g.Title
            Dim appid = If(g.GameInfo?.AppId, "")
            Dim steamUrl = GetSteamUrl(g)
            If Not String.IsNullOrWhiteSpace(appid) AndAlso Not String.IsNullOrWhiteSpace(steamUrl) Then
                heading &= $" [`AppID: {appid}`]({steamUrl})"
            End If

            sb.AppendLine($"### 🎮 {heading}")
            sb.AppendLine()
            sb.AppendLine("| Field | Value |")
            sb.AppendLine("|---|---|")

            For Each c In cols
                Dim val = GetFieldValue(g, c)
                If String.IsNullOrWhiteSpace(val) Then val = "—"

                ' Emojis for crack status
                If c = "Crack Status" Then
                    If val?.Equals("Cracked", StringComparison.OrdinalIgnoreCase) Then val = "✅ Cracked"
                    If val?.Equals("Uncracked", StringComparison.OrdinalIgnoreCase) Then val = "❌ Uncracked"
                    If val?.Equals("Hypervisor", StringComparison.OrdinalIgnoreCase) Then val = "⚠️ Hypervisor"
                End If

                sb.AppendLine($"| **{c}** | {val} |")
            Next

            If Not String.IsNullOrWhiteSpace(steamUrl) Then
                sb.AppendLine($"| **Steam Link** | [{steamUrl}]({steamUrl}) |")
            End If

            sb.AppendLine()
            If idx < games.Count - 1 Then
                sb.AppendLine("---")
                sb.AppendLine()
            End If
        Next

        sb.AppendLine($"_Generated by DenuvoWatch · {DateTime.Now.ToString("yyyy-MM-dd")}_")
        Return sb.ToString()
    End Function

    ' XML - always include appid and steam_link per game
    Private Function GenerateXML(games As List(Of GameItem)) As String
        Dim cols = GetSelectedColumns()
        If cols.Count = 0 OrElse games.Count = 0 Then Return "<denuvowatch />"

        Dim sb As New StringBuilder()
        sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>")
        sb.AppendLine("<denuvowatch>")
        sb.AppendLine($"  <filters>{SecurityElement.Escape(GetFilterHeader())}</filters>")
        sb.AppendLine($"  <exported_at>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}</exported_at>")
        sb.AppendLine($"  <count>{games.Count}</count>")
        sb.AppendLine("  <games>")

        For Each g In games
            sb.AppendLine("    <game>")
            For Each c In cols
                Dim key = columnKeys(Array.IndexOf(columnLabels, c))
                sb.AppendLine($"      <{key}>{SecurityElement.Escape(GetFieldValue(g, c))}</{key}>")
            Next
            sb.AppendLine($"      <appid>{SecurityElement.Escape(If(g.GameInfo?.AppId, ""))}</appid>")
            sb.AppendLine($"      <steam_link>{SecurityElement.Escape(GetSteamUrl(g))}</steam_link>")
            sb.AppendLine("    </game>")
        Next

        sb.AppendLine("  </games>")
        sb.AppendLine("</denuvowatch>")
        Return sb.ToString()
    End Function

    ' Quote it if it has a comma, quote, or newline
    Private Function EscapeCsv(value As String) As String
        Dim s = If(value, "")
        If s.Contains(",") OrElse s.Contains("""") OrElse s.Contains(vbCr) OrElse s.Contains(vbLf) Then
            Return $"""{s.Replace("""", """""")}"""
        End If
        Return s
    End Function

    ' Make the filter string safe for a filename
    Private Function SanitizeFileName(text As String) As String
        Dim s = If(text, "")
        s = s.Replace("""", "")
        s = s.Replace(" · ", "_").Replace("·", "_").Replace(" ", "_")
        Do While s.Contains("__")
            s = s.Replace("__", "_")
        Loop
        Return s.Trim("_"c)
    End Function

    ' Any format/column/sort change refreshes the preview
    Private Sub FormatRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbFormatText.CheckedChanged, rbFormatCSV.CheckedChanged,
                rbFormatJSON.CheckedChanged, rbFormatHTML.CheckedChanged,
                rbFormatMarkdown.CheckedChanged, rbFormatXML.CheckedChanged
        UpdatePreview()
    End Sub

    Private Sub ColumnCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
        Handles cbColTitle.CheckedChanged, cbColDeveloper.CheckedChanged,
                cbColPublisher.CheckedChanged, cbColReleaseDate.CheckedChanged,
                cbColCrackStatus.CheckedChanged, cbColCrackDate.CheckedChanged,
                cbColSceneGroup.CheckedChanged
        UpdatePreview()
    End Sub

    Private Sub SortRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbSortNone.CheckedChanged, rbSortTitleAZ.CheckedChanged,
                rbSortTitleZA.CheckedChanged, rbSortCrackStatus.CheckedChanged,
                rbSortReleaseDate.CheckedChanged
        UpdatePreview()
    End Sub

    ' Save to a file with the right extension and a neat name
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim extension = GetCurrentExtension()
        Dim filter = $"{GetCurrentFormatName()} files (*.{extension})|*.{extension}"

        Dim filterPart = SanitizeFileName(GetFilterHeader())
        Dim baseName = "DenuvoWatch"
        If Not String.IsNullOrWhiteSpace(filterPart) Then baseName &= $"_{filterPart}"
        baseName &= $"_{DateTime.Now.ToString("yyyy-MM-dd")}"

        Using sfd As New SaveFileDialog()
            sfd.Filter = filter
            sfd.DefaultExt = extension
            sfd.AddExtension = True
            sfd.FileName = $"{baseName}.{extension}"

            If sfd.ShowDialog() <> DialogResult.OK Then Return

            Dim content = GenerateExport(ApplySorting(games))
            File.WriteAllText(sfd.FileName, content, New UTF8Encoding(True))

            MessageBox.Show($"Exported {games.Count} game(s) to:{vbCrLf}{sfd.FileName}",
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Using
    End Sub

    Private Function GetCurrentExtension() As String
        If rbFormatCSV.Checked Then Return "csv"
        If rbFormatJSON.Checked Then Return "json"
        If rbFormatHTML.Checked Then Return "html"
        If rbFormatMarkdown.Checked Then Return "md"
        If rbFormatXML.Checked Then Return "xml"
        Return "txt"
    End Function

    Private Function GetCurrentFormatName() As String
        If rbFormatCSV.Checked Then Return "CSV"
        If rbFormatJSON.Checked Then Return "JSON"
        If rbFormatHTML.Checked Then Return "HTML"
        If rbFormatMarkdown.Checked Then Return "Markdown"
        If rbFormatXML.Checked Then Return "XML"
        Return "Text"
    End Function

    ' Back to results with the same data
    Private Sub btnReturnExplorer_Click(sender As Object, e As EventArgs) Handles btnReturnExplorer.Click
        NavigateTo(Me, Function()
            Dim results As New frmResults()
            results.ResultsJson = ResultsJson
            results.SearchFilters = SearchFilters
            results.FilterState = FilterState
            Return results
        End Function)
    End Sub

    ' Back to search, restore saved filter state
    Private Sub btnReturnSearch_Click(sender As Object, e As EventArgs) Handles btnReturnSearch.Click
        NavigateTo(Me, Function()
            Dim search As New frmSearch()
            search.RestoreState = FilterState
            Return search
        End Function)
    End Sub

    ' Toggle between dark and light theme
    Private Sub btnThemeToggle_Click(sender As Object, e As EventArgs) Handles btnThemeToggle.Click
        ToggleTheme(Me)
    End Sub
End Class
