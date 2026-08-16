Imports System.ComponentModel
Imports System.IO
Imports System.Text.Json

' =============================================================================
' Form: frmResults
' Shows your search results with all the game details, grouped by crack status.
' =============================================================================
Public Class frmResults
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ResultsJson As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SearchFilters As String

    Private games As List(Of GameItem)
    ' Full unfiltered list so I can re-filter when the search bar changes
    Private allGames As List(Of GameItem)
    Private ReadOnly CoversDir As String = Path.Combine(AppContext.BaseDirectory, "data", "covers")

    ' Maps each ListBox index to its GameItem (or Nothing for headers)
    Private ReadOnly itemGames As New List(Of GameItem)
    ' Which indices are category headers (for quick lookup)
    Private ReadOnly headerIndices As New HashSet(Of Integer)

    ' Header labels and their colors
    Private ReadOnly headerColors As New Dictionary(Of String, Color) From {
        {"Cracked", Color.Green},
        {"Hypervisor", Color.Goldenrod},
        {"Uncracked", Color.Red}
        }

    ' Parse the JSON, fill the list, set the title
    Private Sub frmResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleFormButtons(Me)
        ApplyTheme(Me)

        Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }

        Dim root = JsonSerializer.Deserialize (Of GamesRoot)(ResultsJson, options)
        games = If(root?.Games, New List(Of GameItem)())
        allGames = games.ToList()

        Dim filterText = If(SearchFilters, "")
        lblSearchFilters.Text = filterText
        FitLabelText(lblSearchFilters, filterText)

        Me.Text = If(Not String.IsNullOrWhiteSpace(filterText),
                     $"DenuvoWatch - Search Results | {filterText}",
                     "DenuvoWatch - Search Results")

        PopulateListBox(games)

        ' Lock the width so the user can only resize vertically, not horizontally
        ' Don't let them shrink below the groupboxes
        Me.MinimumSize = New Size(Me.Width, grpCrackInfo.Bottom + 60)

        ' Pick the first real game so the form isn't empty
        SelectFirstGame()
    End Sub

    ' Build the ListBox with category headers and sorted games
    Private Sub PopulateListBox(list As List(Of GameItem))
        lbGames.BeginUpdate()
        lbGames.Items.Clear()
        itemGames.Clear()
        headerIndices.Clear()

        ' Group by crack status: Cracked → Hypervisor → Uncracked
        Dim cracked =
                list.Where(Function(g) g.CrackInfo?.CrackStatus?.Equals("Cracked", StringComparison.OrdinalIgnoreCase)).
                ToList()
        Dim hypervisor =
                list.Where(
                    Function(g) g.CrackInfo?.CrackStatus?.Equals("Hypervisor", StringComparison.OrdinalIgnoreCase)).
                ToList()
        Dim uncracked =
                list.Where(Function(g) g.CrackInfo?.CrackStatus?.Equals("Uncracked", StringComparison.OrdinalIgnoreCase)) _
                .ToList()
        Dim other =
                list.Where(
                    Function(g) _
                              Not _
                              headerColors.Keys.Contains(If(g.CrackInfo?.CrackStatus, "Unknown"),
                                                         StringComparer.OrdinalIgnoreCase)).ToList()

        If cracked.Count > 0 Then AddSection("Cracked", cracked)
        If hypervisor.Count > 0 Then AddSection("Hypervisor", hypervisor)
        If uncracked.Count > 0 Then AddSection("Uncracked", uncracked)
        If other.Count > 0 Then AddSection("Other", other)

        lbGames.EndUpdate()
    End Sub

    ' Add a header row then the games under it
    Private Sub AddSection(header As String, sectionGames As List(Of GameItem))
        headerIndices.Add(lbGames.Items.Count)
        itemGames.Add(Nothing)
        lbGames.Items.Add(BuildHeaderText(header))

        For Each g In sectionGames
            itemGames.Add(g)
            lbGames.Items.Add(g.Title)
        Next
    End Sub

    ' Build the centered header text with = padding
    Private Function BuildHeaderText(header As String) As String
        ' Figure out how many = fit on each side
        Dim textSize = TextRenderer.MeasureText(header & " ", lbGames.Font)
        Dim availableWidth = lbGames.ClientSize.Width - 6  ' account for scrollbar
        Dim equalsCount = Math.Max(5, (availableWidth - textSize.Width)\2\11)  ' each = is ~11px wide
        Return New String("="c, equalsCount) & " " & header & " " & New String("="c, equalsCount)
    End Function

    ' Live filter the ListBox as the user types in the search bar
    Private Sub txtGameSearch_TextChanged(sender As Object, e As EventArgs) Handles txtGameSearch.TextChanged
        Dim term = txtGameSearch.Text.Trim().ToLowerInvariant()

        games = allGames.Where(Function(g) g.Title.ToLowerInvariant().Contains(term)).ToList()
        PopulateListBox(games)

        If lbGames.Items.Count > 0 Then
            SelectFirstGame()
        Else
            ClearDetailFields()
        End If
    End Sub

    ' Owner-draw: headers get colored, games get drawn normally. Adapts to dark/light theme.
    Private Sub lbGames_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lbGames.DrawItem
        If e.Index < 0 Then Return

        Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected

        ' Fill the background - dark theme uses dark surface, light theme uses system default
        If isSelected Then
            e.DrawBackground()
        Else
            Dim bgColor = If(IsDarkTheme, DarkSurface, SystemColors.Window)
            Using brush As New SolidBrush(bgColor)
                e.Graphics.FillRectangle(brush, e.Bounds)
            End Using
        End If

        If headerIndices.Contains(e.Index) Then
            ' Draw a header - centered with its category color
            Dim headerText = lbGames.Items(e.Index).ToString()
            Dim category = ExtractCategory(headerText)
            Dim headerColor As Color = Nothing
            headerColors.TryGetValue(category, headerColor)
            Dim drawColor = If(headerColor <> Nothing, headerColor, SystemColors.ControlText)

            Using brush As New SolidBrush(drawColor)
                Dim sf As New StringFormat With {
                        .Alignment = StringAlignment.Center,
                        .LineAlignment = StringAlignment.Center
                        }
                e.Graphics.DrawString(headerText, lbGames.Font, brush, e.Bounds, sf)
            End Using
        Else
            ' Draw a normal game title
            Dim text = lbGames.Items(e.Index).ToString()
            Dim textColor =
                    If(isSelected, SystemColors.HighlightText, If(IsDarkTheme, DarkText, SystemColors.ControlText))
            TextRenderer.DrawText(e.Graphics, text, lbGames.Font, e.Bounds, textColor,
                                  TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
        End If

        e.DrawFocusRectangle()
    End Sub

    ' Measure item height - all items same height
    Private Sub lbGames_MeasureItem(sender As Object, e As MeasureItemEventArgs) Handles lbGames.MeasureItem
        e.ItemHeight = lbGames.Font.Height + 2
    End Sub

    ' When the user picks a game, fill in all the details
    Private Sub lbGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbGames.SelectedIndexChanged
        ' If a header got clicked, don't do anything - it's not a real game
        If lbGames.SelectedIndex < 0 Then Return
        If headerIndices.Contains(lbGames.SelectedIndex) Then Return

        Dim g = itemGames(lbGames.SelectedIndex)
        If g Is Nothing Then Return

        txtPublisher.Text = If(g.GameInfo?.Publisher, "—")
        txtDeveloper.Text = If(g.GameInfo?.Developer, "—")
        txtReleaseDate.Text = If(g.GameInfo?.ReleaseDate, "—")

        Dim status = If(g.CrackInfo?.CrackStatus, "Unknown")
        txtCrackStatus.Text = status
        ApplyCrackStatusColor(status)

        Dim crackDate = If(g.CrackInfo?.CrackDate, "")
        Dim crackDateRel = If(g.CrackInfo?.CrackDateRelative, "")
        If Not String.IsNullOrWhiteSpace(crackDate) Then
            txtCrackDate.Text = If(String.IsNullOrWhiteSpace(crackDateRel),
                                   crackDate, $"{crackDate} ({crackDateRel})")
        Else
            txtCrackDate.Text = "—"
        End If

        txtSceneGroup.Text = If(Not String.IsNullOrWhiteSpace(g.CrackInfo?.SceneGroup),
                                g.CrackInfo.SceneGroup, "—")

        LoadCover(g.SortTitle)
    End Sub

    ' Pick the first non-header item
    Private Sub SelectFirstGame()
        For i = 0 To lbGames.Items.Count - 1
            If Not headerIndices.Contains(i) Then
                lbGames.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    ' Clear all detail fields to empty state
    Private Sub ClearDetailFields()
        txtPublisher.Text = "—"
        txtDeveloper.Text = "—"
        txtReleaseDate.Text = "—"
        txtCrackStatus.Text = "—"
        txtCrackDate.Text = "—"
        txtSceneGroup.Text = "—"
        picGameCover.Image = Nothing
    End Sub

    ' Pull the category name out of a header string like "=== Cracked ==="
    Private Function ExtractCategory(headerText As String) As String
        For Each key In headerColors.Keys
            If headerText.Contains(key) Then Return key
        Next
        Return ""
    End Function

    ' Green=Cracked, Red=Uncracked, Gold=Hypervisor
    Private Sub ApplyCrackStatusColor(status As String)
        If status.Equals("Cracked", StringComparison.OrdinalIgnoreCase) Then
            txtCrackStatus.ForeColor = Color.Green
        ElseIf status.Equals("Uncracked", StringComparison.OrdinalIgnoreCase) Then
            txtCrackStatus.ForeColor = Color.Red
        ElseIf status.Equals("Hypervisor", StringComparison.OrdinalIgnoreCase) Then
            txtCrackStatus.ForeColor = Color.Goldenrod
        Else
            txtCrackStatus.ForeColor = SystemColors.ControlText
        End If
    End Sub

    ' Load the cover from the local cache
    Private Sub LoadCover(sortTitle As String)
        If String.IsNullOrWhiteSpace(sortTitle) Then
            picGameCover.Image = Nothing
            Return
        End If

        Dim coverPath = Path.Combine(CoversDir, $"{sortTitle}.jpg")
        Try
            If File.Exists(coverPath) Then
                picGameCover.Image = Image.FromFile(coverPath)
            Else
                picGameCover.Image = Nothing
            End If
        Catch
            picGameCover.Image = Nothing
        End Try
    End Sub

    ' Shrink the font until the text fits (min 5pt)
    Private Sub FitLabelText(lbl As Label, text As String)
        If String.IsNullOrEmpty(text) Then Return

        Dim baseFont = lbl.Font
        Dim fontSize As Single = baseFont.Size

        Do While fontSize > 5
            lbl.Font = New Font(baseFont.FontFamily, fontSize, baseFont.Style)
            Dim measured = TextRenderer.MeasureText(text, lbl.Font, lbl.ClientSize, TextFormatFlags.NoPadding)
            If measured.Width <= lbl.ClientSize.Width AndAlso
               measured.Height <= lbl.ClientSize.Height Then Exit Do
            fontSize -= 0.5F
        Loop
    End Sub

    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        NavigateTo(Me, Function() New frmSearch())
    End Sub

    ' Send the data to the export form
    Private Sub btnProceedToExport_Click(sender As Object, e As EventArgs) Handles btnProceedToExport.Click
        NavigateTo(Me, Function()
            Dim exportForm As New frmExport()
            exportForm.ResultsJson = ResultsJson
            exportForm.SearchFilters = SearchFilters
            Return exportForm
        End Function)
    End Sub

    ' Open the Steam page in the browser
    Private Sub btnSteamPage_Click(sender As Object, e As EventArgs) Handles btnSteamPage.Click
        If lbGames.SelectedIndex < 0 OrElse itemGames(lbGames.SelectedIndex) Is Nothing Then Return

        Dim g = itemGames(lbGames.SelectedIndex)
        If String.IsNullOrWhiteSpace(g.GameInfo?.AppId) Then
            MessageBox.Show("No Steam App ID available for this game.",
                            "Steam", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Process.Start(New ProcessStartInfo($"https://store.steampowered.com/app/{g.GameInfo.AppId}") _
                         With {.UseShellExecute = True})
    End Sub

    Private Sub grpGameInfo_Enter(sender As Object, e As EventArgs) Handles grpGameInfo.Enter
    End Sub

    ' Toggle between dark and light theme
    Private Sub btnThemeToggle_Click(sender As Object, e As EventArgs) Handles btnThemeToggle.Click
        ToggleTheme(Me)
    End Sub

    ' When the form resizes, rebuild the headers so the ='s adapt to the new width
    Private Sub frmResults_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If lbGames Is Nothing OrElse lbGames.Items.Count = 0 Then Return

        ' Rebuild the header text for each header index with the new listbox width
        For Each idx In headerIndices
            Dim category = ExtractCategory(lbGames.Items(idx).ToString())
            If Not String.IsNullOrEmpty(category) Then
                lbGames.Items(idx) = BuildHeaderText(category)
            End If
        Next
    End Sub

    Private Sub lblSearchFilters_Click(sender As Object, e As EventArgs)
    End Sub
End Class
