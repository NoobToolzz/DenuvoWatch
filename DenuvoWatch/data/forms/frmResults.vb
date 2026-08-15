Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Text.Json
Imports System.Windows.Forms

' frmResults — shows your search results with all the game details.
Public Class frmResults
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ResultsJson As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SearchFilters As String

    Private games As List(Of GameItem)
    Private ReadOnly CoversDir As String = Path.Combine(AppContext.BaseDirectory, "data", "covers")

    ' Parse the JSON, fill the list, set the title
    Private Sub frmResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleFormButtons(Me)

        Dim options As New JsonSerializerOptions With {
            .PropertyNameCaseInsensitive = True,
            .PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }

        Dim root = JsonSerializer.Deserialize(Of GamesRoot)(ResultsJson, options)
        games = If(root?.Games, New List(Of GameItem)())

        Dim filterText = If(SearchFilters, "")
        lblSearchFilters.Text = filterText
        FitLabelText(lblSearchFilters, filterText)

        Me.Text = If(Not String.IsNullOrWhiteSpace(filterText),
            $"DenuvoWatch - Search Results | {filterText}",
            "DenuvoWatch - Search Results")

        lbGames.BeginUpdate()
        lbGames.Items.Clear()
        For Each g In games
            lbGames.Items.Add(g.Title)
        Next
        lbGames.EndUpdate()

        ' Pick the first game so the form isn't empty
        If lbGames.Items.Count > 0 Then lbGames.SelectedIndex = 0
    End Sub

    ' When the user picks a game, fill in all the details
    Private Sub lbGames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbGames.SelectedIndexChanged
        If lbGames.SelectedIndex < 0 OrElse games Is Nothing Then Return

        Dim g = games(lbGames.SelectedIndex)

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
        If lbGames.SelectedIndex < 0 OrElse games Is Nothing Then Return

        Dim g = games(lbGames.SelectedIndex)
        If String.IsNullOrWhiteSpace(g.GameInfo?.AppId) Then
            MessageBox.Show("No Steam App ID available for this game.",
                            "Steam", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Process.Start(New ProcessStartInfo($"https://store.steampowered.com/app/{g.GameInfo.AppId}") With {.UseShellExecute = True})
    End Sub

    Private Sub grpGameInfo_Enter(sender As Object, e As EventArgs) Handles grpGameInfo.Enter
    End Sub
End Class
