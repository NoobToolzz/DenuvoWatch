Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Text.Json
Imports System.Windows.Forms

' =============================================================================
' Class: frmResults
' -----------------------------------------------------------------------------
' Results form — receives the JSON search results from frmSearch via the
' ResultsJson property, parses them on load, populates the games ListBox, and
' binds all detail fields (text boxes, labels, cover image) to the selected
' game. Includes buttons to return to search and open the Steam store page.
' =============================================================================
Public Class frmResults
    ' JSON string passed from frmSearch (confirmed to contain >= 1 result).
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        >
    Public Property ResultsJson As String

    ' Filter summary string passed from frmSearch (e.g. "Ubisoft · Capcom · EMPRESS").
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        >
    Public Property SearchFilters As String

    ' Parsed game list, kept in sync with the ListBox order.
    Private games As List(Of GameItem)

    ' Path to the local covers folder (same as CoverCache uses).
    Private ReadOnly CoversDir As String = Path.Combine(AppContext.BaseDirectory, "data", "covers")

    ' ---------------------------------------------------------------------------
    ' frmResults_Load
    '   Parses the JSON, fills the ListBox with game titles, and selects the
    '   first item to trigger the detail-field binding.
    ' ---------------------------------------------------------------------------
    Private Sub frmResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True,
                .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                }

        Dim root = JsonSerializer.Deserialize(Of GamesRoot)(ResultsJson, options)
        games = If(root?.Games, New List(Of GameItem)())

        ' Show whatever filters the user searched with, and stick them in the window title too
        Dim filterText = If(SearchFilters, "")
        lblSearchFilters.Text = filterText
        FitLabelText(lblSearchFilters, filterText)

        ' Window title looks like: DenuvoWatch - Search Results | "query" · filters
        If Not String.IsNullOrWhiteSpace(filterText) Then
            Me.Text = "DenuvoWatch - Search Results | " & filterText
        Else
            Me.Text = "DenuvoWatch - Search Results"
        End If

        lbGames.BeginUpdate()
        lbGames.Items.Clear()
        For Each g In games
            lbGames.Items.Add(g.Title)
        Next
        lbGames.EndUpdate()

        ' Pick the first game by default so the form isn't empty when it opens
        If lbGames.Items.Count > 0 Then
            lbGames.SelectedIndex = 0
        End If
    End Sub

    ' ---------------------------------------------------------------------------
    ' lbGames_SelectedIndexChanged
    '   When the user selects a game (or the default first-item selection fires
    '   on load), updates every detail field on the form.
    ' ---------------------------------------------------------------------------
    Private Sub lbGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbGames.SelectedIndexChanged
        If lbGames.SelectedIndex < 0 OrElse games Is Nothing Then Return

        Dim g = games(lbGames.SelectedIndex)

        ' --- Game info section ---
        txtPublisher.Text = If(g.GameInfo?.Publisher, "—")
        txtDeveloper.Text = If(g.GameInfo?.Developer, "—")
        txtReleaseDate.Text = If(g.GameInfo?.ReleaseDate, "—")

        ' --- Crack info section ---
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

        ' --- Scene group ---
        txtSceneGroup.Text = If(Not String.IsNullOrWhiteSpace(g.CrackInfo?.SceneGroup),
                                g.CrackInfo.SceneGroup, "—")

        ' --- Load the cover image from the local cache ---
        LoadCover(g.SortTitle)
    End Sub

    ' ---------------------------------------------------------------------------
    ' ApplyCrackStatusColor
    '   Sets the ForeColor of the crack status text box based on the status:
    '     Cracked    -> Green
    '     Uncracked  -> Red
    '     Hypervisor -> Yellow/Gold
    '     Other      -> Default text color
    ' ---------------------------------------------------------------------------
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

    ' ---------------------------------------------------------------------------
    ' LoadCover
    '   Loads the cover image for the given sort_title from the local
    '   data\covers\{sort_title}.jpg cache. Clears the PictureBox if the
    '   file is missing or fails to load.
    ' ---------------------------------------------------------------------------
    Private Sub LoadCover(sortTitle As String)
        If String.IsNullOrWhiteSpace(sortTitle) Then
            picGameCover.Image = Nothing
            Return
        End If

        Dim coverPath = Path.Combine(CoversDir, sortTitle & ".jpg")
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

    ' ---------------------------------------------------------------------------
    ' FitLabelText
    '   Shrinks the font size of the label so the text fits within its bounds.
    '   I start from the label's current font size and keep dropping it until
    '   TextRenderer says the text fits — if it already fits, I leave it alone.
    ' ---------------------------------------------------------------------------
    Private Sub FitLabelText(lbl As Label, text As String)
        If String.IsNullOrEmpty(text) Then Return

        ' Start from the original font size the designer gave us
        Dim baseFont = lbl.Font
        Dim fontSize As Single = baseFont.Size

        ' Keep shrinking until it fits or we hit a minimum of 5pt
        Do While fontSize > 5
            lbl.Font = New Font(baseFont.FontFamily, fontSize, baseFont.Style)
            Dim measured = TextRenderer.MeasureText(text, lbl.Font, lbl.ClientSize, TextFormatFlags.NoPadding)
            If measured.Width <= lbl.ClientSize.Width AndAlso
               measured.Height <= lbl.ClientSize.Height Then
                Exit Do
            End If
            fontSize -= 0.5F
        Loop
    End Sub

    ' ---------------------------------------------------------------------------
    ' btnReturn_Click
    '   Returns to the frmSearch form.
    ' ---------------------------------------------------------------------------
    Private Sub btnReturn_Click(sender As Object, e As EventArgs) Handles btnReturn.Click
        NavigateTo(Me, Function() New frmSearch())
    End Sub

    ' ---------------------------------------------------------------------------
    ' btnSteamPage_Click
    '   Opens the Steam store page for the currently selected game in the
    '   user's default browser. URL: https://store.steampowered.com/app/{appid}
    ' ---------------------------------------------------------------------------
    Private Sub btnSteamPage_Click(sender As Object, e As EventArgs) Handles btnSteamPage.Click
        If lbGames.SelectedIndex < 0 OrElse games Is Nothing Then Return

        Dim g = games(lbGames.SelectedIndex)
        If String.IsNullOrWhiteSpace(g.GameInfo?.AppId) Then
            MessageBox.Show("No Steam App ID available for this game.",
                            "Steam", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim url = $"https://store.steampowered.com/app/{g.GameInfo.AppId}"
        Process.Start(New ProcessStartInfo(url) With {.UseShellExecute = True})
    End Sub

    Private Sub grpGameInfo_Enter(sender As Object, e As EventArgs) Handles grpGameInfo.Enter
    End Sub
End Class
