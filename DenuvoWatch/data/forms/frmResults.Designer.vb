<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResults
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResults))
        lbGames = New ListBox()
        lblGames = New Label()
        txtPublisher = New TextBox()
        lblPublisher = New Label()
        lblDeveloper = New Label()
        txtDeveloper = New TextBox()
        lblReleaseDate = New Label()
        txtReleaseDate = New TextBox()
        grpGameInfo = New GroupBox()
        grpCrackInfo = New GroupBox()
        picGameCover = New PictureBox()
        btnSteamPage = New Button()
        btnProceedToExport = New Button()
        btnReturn = New Button()
        lblSearchFilters = New Label()
        Label2 = New Label()
        txtGameSearch = New TextBox()
        btnThemeToggle = New Button()
        gbMiscInfo = New GroupBox()
        txtEstimatedRevenueLost = New TextBox()
        lblEstimatedRevenueLost = New Label()
        lblCrackStatus = New Label()
        txtCrackStatus = New TextBox()
        txtSceneGroup = New TextBox()
        lblSceneGroup = New Label()
        lblCrackDate = New Label()
        txtCrackDate = New TextBox()
        grpGameInfo.SuspendLayout()
        grpCrackInfo.SuspendLayout()
        CType(picGameCover, ComponentModel.ISupportInitialize).BeginInit()
        picGameCover.SuspendLayout()
        gbMiscInfo.SuspendLayout()
        SuspendLayout()
        ' 
        ' lbGames
        ' 
        lbGames.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lbGames.DrawMode = DrawMode.OwnerDrawFixed
        lbGames.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lbGames.FormattingEnabled = True
        lbGames.Location = New Point(47, 81)
        lbGames.Margin = New Padding(4, 3, 4, 3)
        lbGames.Name = "lbGames"
        lbGames.Size = New Size(408, 676)
        lbGames.TabIndex = 3
        ' 
        ' lblGames
        ' 
        lblGames.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblGames.Font = New Font("JetBrains Mono", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblGames.ImageAlign = ContentAlignment.MiddleRight
        lblGames.Location = New Point(47, 42)
        lblGames.Margin = New Padding(4, 0, 4, 0)
        lblGames.Name = "lblGames"
        lblGames.Size = New Size(408, 36)
        lblGames.TabIndex = 8
        lblGames.Text = "Games"
        lblGames.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtPublisher
        ' 
        txtPublisher.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtPublisher.Location = New Point(35, 58)
        txtPublisher.Margin = New Padding(4, 3, 4, 3)
        txtPublisher.Name = "txtPublisher"
        txtPublisher.ReadOnly = True
        txtPublisher.Size = New Size(300, 29)
        txtPublisher.TabIndex = 12
        ' 
        ' lblPublisher
        ' 
        lblPublisher.AutoSize = True
        lblPublisher.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblPublisher.Location = New Point(35, 35)
        lblPublisher.Margin = New Padding(4, 0, 4, 0)
        lblPublisher.Name = "lblPublisher"
        lblPublisher.Size = New Size(77, 14)
        lblPublisher.TabIndex = 13
        lblPublisher.Text = "Publisher:"
        ' 
        ' lblDeveloper
        ' 
        lblDeveloper.AutoSize = True
        lblDeveloper.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblDeveloper.Location = New Point(35, 112)
        lblDeveloper.Margin = New Padding(4, 0, 4, 0)
        lblDeveloper.Name = "lblDeveloper"
        lblDeveloper.Size = New Size(77, 14)
        lblDeveloper.TabIndex = 15
        lblDeveloper.Text = "Developer:"
        ' 
        ' txtDeveloper
        ' 
        txtDeveloper.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtDeveloper.Location = New Point(35, 135)
        txtDeveloper.Margin = New Padding(4, 3, 4, 3)
        txtDeveloper.Name = "txtDeveloper"
        txtDeveloper.ReadOnly = True
        txtDeveloper.Size = New Size(300, 29)
        txtDeveloper.TabIndex = 14
        ' 
        ' lblReleaseDate
        ' 
        lblReleaseDate.AutoSize = True
        lblReleaseDate.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblReleaseDate.Location = New Point(35, 189)
        lblReleaseDate.Margin = New Padding(4, 0, 4, 0)
        lblReleaseDate.Name = "lblReleaseDate"
        lblReleaseDate.Size = New Size(98, 14)
        lblReleaseDate.TabIndex = 19
        lblReleaseDate.Text = "Release Date:"
        ' 
        ' txtReleaseDate
        ' 
        txtReleaseDate.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtReleaseDate.Location = New Point(35, 212)
        txtReleaseDate.Margin = New Padding(4, 3, 4, 3)
        txtReleaseDate.Name = "txtReleaseDate"
        txtReleaseDate.ReadOnly = True
        txtReleaseDate.Size = New Size(300, 29)
        txtReleaseDate.TabIndex = 18
        ' 
        ' grpGameInfo
        ' 
        grpGameInfo.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        grpGameInfo.Controls.Add(txtPublisher)
        grpGameInfo.Controls.Add(lblPublisher)
        grpGameInfo.Controls.Add(txtDeveloper)
        grpGameInfo.Controls.Add(lblReleaseDate)
        grpGameInfo.Controls.Add(lblDeveloper)
        grpGameInfo.Controls.Add(txtReleaseDate)
        grpGameInfo.Location = New Point(503, 91)
        grpGameInfo.Margin = New Padding(4, 3, 4, 3)
        grpGameInfo.Name = "grpGameInfo"
        grpGameInfo.Padding = New Padding(4, 3, 4, 3)
        grpGameInfo.Size = New Size(371, 255)
        grpGameInfo.TabIndex = 22
        grpGameInfo.TabStop = False
        grpGameInfo.Text = "Game Information"
        ' 
        ' grpCrackInfo
        ' 
        grpCrackInfo.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        grpCrackInfo.Controls.Add(txtCrackStatus)
        grpCrackInfo.Controls.Add(lblCrackStatus)
        grpCrackInfo.Controls.Add(txtCrackDate)
        grpCrackInfo.Controls.Add(lblCrackDate)
        grpCrackInfo.Controls.Add(lblSceneGroup)
        grpCrackInfo.Controls.Add(txtSceneGroup)
        grpCrackInfo.Location = New Point(503, 357)
        grpCrackInfo.Margin = New Padding(4, 3, 4, 3)
        grpCrackInfo.Name = "grpCrackInfo"
        grpCrackInfo.Padding = New Padding(4, 3, 4, 3)
        grpCrackInfo.Size = New Size(371, 255)
        grpCrackInfo.TabIndex = 28
        grpCrackInfo.TabStop = False
        grpCrackInfo.Text = "Crack Information"
        ' 
        ' picGameCover
        ' 
        picGameCover.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        picGameCover.Controls.Add(btnSteamPage)
        picGameCover.Location = New Point(916, 58)
        picGameCover.Margin = New Padding(4, 3, 4, 3)
        picGameCover.Name = "picGameCover"
        picGameCover.Size = New Size(373, 554)
        picGameCover.SizeMode = PictureBoxSizeMode.StretchImage
        picGameCover.TabIndex = 29
        picGameCover.TabStop = False
        ' 
        ' btnSteamPage
        ' 
        btnSteamPage.BackColor = Color.Transparent
        btnSteamPage.FlatAppearance.BorderSize = 0
        btnSteamPage.FlatStyle = FlatStyle.Flat
        btnSteamPage.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnSteamPage.ForeColor = Color.Gray
        btnSteamPage.Location = New Point(333, 0)
        btnSteamPage.Name = "btnSteamPage"
        btnSteamPage.Size = New Size(40, 37)
        btnSteamPage.TabIndex = 32
        btnSteamPage.Text = "🌐"
        btnSteamPage.UseVisualStyleBackColor = False
        ' 
        ' btnProceedToExport
        ' 
        btnProceedToExport.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnProceedToExport.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnProceedToExport.Location = New Point(997, 646)
        btnProceedToExport.Margin = New Padding(4, 3, 4, 3)
        btnProceedToExport.Name = "btnProceedToExport"
        btnProceedToExport.Size = New Size(208, 57)
        btnProceedToExport.TabIndex = 30
        btnProceedToExport.Text = "Proceed to Export"
        btnProceedToExport.UseVisualStyleBackColor = True
        ' 
        ' btnReturn
        ' 
        btnReturn.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnReturn.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReturn.Location = New Point(997, 727)
        btnReturn.Margin = New Padding(4, 3, 4, 3)
        btnReturn.Name = "btnReturn"
        btnReturn.Size = New Size(208, 57)
        btnReturn.TabIndex = 31
        btnReturn.Text = "Return to Search"
        btnReturn.UseVisualStyleBackColor = True
        ' 
        ' lblSearchFilters
        ' 
        lblSearchFilters.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblSearchFilters.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearchFilters.Location = New Point(503, 57)
        lblSearchFilters.Margin = New Padding(4, 0, 4, 0)
        lblSearchFilters.Name = "lblSearchFilters"
        lblSearchFilters.Size = New Size(371, 16)
        lblSearchFilters.TabIndex = 32
        lblSearchFilters.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label2.Font = New Font("JetBrains Mono", 20.25F, FontStyle.Bold Or FontStyle.Underline)
        Label2.Location = New Point(503, 21)
        Label2.Margin = New Padding(4, 0, 4, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(371, 36)
        Label2.TabIndex = 33
        Label2.Text = "Search Results"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtGameSearch
        ' 
        txtGameSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtGameSearch.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtGameSearch.Location = New Point(47, 776)
        txtGameSearch.Margin = New Padding(4, 3, 4, 3)
        txtGameSearch.Name = "txtGameSearch"
        txtGameSearch.PlaceholderText = "Search results..."
        txtGameSearch.Size = New Size(408, 23)
        txtGameSearch.TabIndex = 34
        ' 
        ' btnThemeToggle
        ' 
        btnThemeToggle.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnThemeToggle.FlatAppearance.BorderSize = 0
        btnThemeToggle.FlatStyle = FlatStyle.Flat
        btnThemeToggle.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnThemeToggle.Location = New Point(1266, 12)
        btnThemeToggle.Name = "btnThemeToggle"
        btnThemeToggle.Size = New Size(45, 33)
        btnThemeToggle.TabIndex = 35
        btnThemeToggle.Text = "☀️"
        btnThemeToggle.UseVisualStyleBackColor = True
        ' 
        ' gbMiscInfo
        ' 
        gbMiscInfo.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        gbMiscInfo.Controls.Add(txtEstimatedRevenueLost)
        gbMiscInfo.Controls.Add(lblEstimatedRevenueLost)
        gbMiscInfo.Location = New Point(503, 618)
        gbMiscInfo.Margin = New Padding(4, 3, 4, 3)
        gbMiscInfo.Name = "gbMiscInfo"
        gbMiscInfo.Padding = New Padding(4, 3, 4, 3)
        gbMiscInfo.Size = New Size(371, 98)
        gbMiscInfo.TabIndex = 29
        gbMiscInfo.TabStop = False
        gbMiscInfo.Text = "Miscellaneous"
        ' 
        ' txtEstimatedRevenueLost
        ' 
        txtEstimatedRevenueLost.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtEstimatedRevenueLost.Location = New Point(35, 58)
        txtEstimatedRevenueLost.Margin = New Padding(4, 3, 4, 3)
        txtEstimatedRevenueLost.Name = "txtEstimatedRevenueLost"
        txtEstimatedRevenueLost.ReadOnly = True
        txtEstimatedRevenueLost.Size = New Size(300, 29)
        txtEstimatedRevenueLost.TabIndex = 12
        ' 
        ' lblEstimatedRevenueLost
        ' 
        lblEstimatedRevenueLost.AutoSize = True
        lblEstimatedRevenueLost.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblEstimatedRevenueLost.Location = New Point(36, 35)
        lblEstimatedRevenueLost.Margin = New Padding(4, 0, 4, 0)
        lblEstimatedRevenueLost.Name = "lblEstimatedRevenueLost"
        lblEstimatedRevenueLost.Size = New Size(168, 14)
        lblEstimatedRevenueLost.TabIndex = 13
        lblEstimatedRevenueLost.Text = "Estimated Revenue Lost:"
        ' 
        ' lblCrackStatus
        ' 
        lblCrackStatus.AutoSize = True
        lblCrackStatus.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCrackStatus.Location = New Point(35, 35)
        lblCrackStatus.Margin = New Padding(4, 0, 4, 0)
        lblCrackStatus.Name = "lblCrackStatus"
        lblCrackStatus.Size = New Size(98, 14)
        lblCrackStatus.TabIndex = 13
        lblCrackStatus.Text = "Crack Status:"
        ' 
        ' txtCrackStatus
        ' 
        txtCrackStatus.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCrackStatus.Location = New Point(35, 58)
        txtCrackStatus.Margin = New Padding(4, 3, 4, 3)
        txtCrackStatus.Name = "txtCrackStatus"
        txtCrackStatus.ReadOnly = True
        txtCrackStatus.Size = New Size(300, 29)
        txtCrackStatus.TabIndex = 12
        ' 
        ' txtSceneGroup
        ' 
        txtSceneGroup.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtSceneGroup.Location = New Point(35, 212)
        txtSceneGroup.Margin = New Padding(4, 3, 4, 3)
        txtSceneGroup.Name = "txtSceneGroup"
        txtSceneGroup.ReadOnly = True
        txtSceneGroup.Size = New Size(300, 29)
        txtSceneGroup.TabIndex = 16
        ' 
        ' lblSceneGroup
        ' 
        lblSceneGroup.AutoSize = True
        lblSceneGroup.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblSceneGroup.Location = New Point(35, 189)
        lblSceneGroup.Margin = New Padding(4, 0, 4, 0)
        lblSceneGroup.Name = "lblSceneGroup"
        lblSceneGroup.Size = New Size(91, 14)
        lblSceneGroup.TabIndex = 17
        lblSceneGroup.Text = "Scene Group:"
        ' 
        ' lblCrackDate
        ' 
        lblCrackDate.AutoSize = True
        lblCrackDate.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCrackDate.Location = New Point(35, 112)
        lblCrackDate.Margin = New Padding(4, 0, 4, 0)
        lblCrackDate.Name = "lblCrackDate"
        lblCrackDate.Size = New Size(84, 14)
        lblCrackDate.TabIndex = 15
        lblCrackDate.Text = "Crack Date:"
        ' 
        ' txtCrackDate
        ' 
        txtCrackDate.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCrackDate.Location = New Point(35, 135)
        txtCrackDate.Margin = New Padding(4, 3, 4, 3)
        txtCrackDate.Name = "txtCrackDate"
        txtCrackDate.ReadOnly = True
        txtCrackDate.Size = New Size(300, 29)
        txtCrackDate.TabIndex = 14
        ' 
        ' frmResults
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1323, 832)
        Controls.Add(gbMiscInfo)
        Controls.Add(btnThemeToggle)
        Controls.Add(Label2)
        Controls.Add(lblSearchFilters)
        Controls.Add(btnReturn)
        Controls.Add(btnProceedToExport)
        Controls.Add(picGameCover)
        Controls.Add(grpCrackInfo)
        Controls.Add(grpGameInfo)
        Controls.Add(lblGames)
        Controls.Add(lbGames)
        Controls.Add(txtGameSearch)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4, 3, 4, 3)
        MaximizeBox = False
        Name = "frmResults"
        Text = "injected"
        grpGameInfo.ResumeLayout(False)
        grpGameInfo.PerformLayout()
        grpCrackInfo.ResumeLayout(False)
        grpCrackInfo.PerformLayout()
        CType(picGameCover, ComponentModel.ISupportInitialize).EndInit()
        picGameCover.ResumeLayout(False)
        gbMiscInfo.ResumeLayout(False)
        gbMiscInfo.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lbGames As ListBox
    Friend WithEvents lblGames As Label
    Friend WithEvents txtPublisher As TextBox
    Friend WithEvents lblPublisher As Label
    Friend WithEvents lblDeveloper As Label
    Friend WithEvents txtDeveloper As TextBox
    Friend WithEvents lblReleaseDate As Label
    Friend WithEvents txtReleaseDate As TextBox
    Friend WithEvents grpGameInfo As GroupBox
    Friend WithEvents grpCrackInfo As GroupBox
    Friend WithEvents picGameCover As PictureBox
    Friend WithEvents btnProceedToExport As Button
    Friend WithEvents btnReturn As Button
    Friend WithEvents btnSteamPage As Button
    Friend WithEvents lblSearchFilters As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtGameSearch As TextBox
    Friend WithEvents btnThemeToggle As Button
    Friend WithEvents txtCrackStatus As TextBox
    Friend WithEvents lblCrackStatus As Label
    Friend WithEvents txtCrackDate As TextBox
    Friend WithEvents lblCrackDate As Label
    Friend WithEvents lblSceneGroup As Label
    Friend WithEvents txtSceneGroup As TextBox
    Friend WithEvents gbMiscInfo As GroupBox
    Friend WithEvents txtEstimatedRevenueLost As TextBox
    Friend WithEvents lblEstimatedRevenueLost As Label
End Class
