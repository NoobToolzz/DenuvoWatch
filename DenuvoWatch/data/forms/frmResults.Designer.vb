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
        lbGames = New ListBox()
        lblGames = New Label()
        txtPublisher = New TextBox()
        lblPublisher = New Label()
        lblDeveloper = New Label()
        txtDeveloper = New TextBox()
        lblSceneGroup = New Label()
        txtSceneGroup = New TextBox()
        lblReleaseDate = New Label()
        txtReleaseDate = New TextBox()
        grpGameInfo = New GroupBox()
        grpCrackInfo = New GroupBox()
        txtCrackStatus = New TextBox()
        lblCrackStatus = New Label()
        txtCrackDate = New TextBox()
        lblCrackType = New Label()
        lblCrackDate = New Label()
        txtCrackType = New TextBox()
        picGameCover = New PictureBox()
        btnSave = New Button()
        btnReturn = New Button()
        Button1 = New Button()
        grpGameInfo.SuspendLayout()
        grpCrackInfo.SuspendLayout()
        CType(picGameCover, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lbGames
        ' 
        lbGames.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lbGames.FormattingEnabled = True
        lbGames.Items.AddRange(New Object() {"Final Fantasy VII", "Harry Hogwart Butt Tickling School"})
        lbGames.Location = New Point(47, 81)
        lbGames.Margin = New Padding(4, 3, 4, 3)
        lbGames.Name = "lbGames"
        lbGames.Size = New Size(408, 690)
        lbGames.TabIndex = 3
        ' 
        ' lblGames
        ' 
        lblGames.AutoSize = True
        lblGames.Font = New Font("JetBrains Mono", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblGames.Location = New Point(189, 22)
        lblGames.Margin = New Padding(4, 0, 4, 0)
        lblGames.Name = "lblGames"
        lblGames.Size = New Size(95, 36)
        lblGames.TabIndex = 8
        lblGames.Text = "Games"
        ' 
        ' txtPublisher
        ' 
        txtPublisher.Enabled = False
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
        lblDeveloper.Location = New Point(35, 115)
        lblDeveloper.Margin = New Padding(4, 0, 4, 0)
        lblDeveloper.Name = "lblDeveloper"
        lblDeveloper.Size = New Size(77, 14)
        lblDeveloper.TabIndex = 15
        lblDeveloper.Text = "Developer:"
        ' 
        ' txtDeveloper
        ' 
        txtDeveloper.Enabled = False
        txtDeveloper.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtDeveloper.Location = New Point(35, 138)
        txtDeveloper.Margin = New Padding(4, 3, 4, 3)
        txtDeveloper.Name = "txtDeveloper"
        txtDeveloper.ReadOnly = True
        txtDeveloper.Size = New Size(300, 29)
        txtDeveloper.TabIndex = 14
        ' 
        ' lblSceneGroup
        ' 
        lblSceneGroup.AutoSize = True
        lblSceneGroup.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblSceneGroup.Location = New Point(35, 196)
        lblSceneGroup.Margin = New Padding(4, 0, 4, 0)
        lblSceneGroup.Name = "lblSceneGroup"
        lblSceneGroup.Size = New Size(91, 14)
        lblSceneGroup.TabIndex = 17
        lblSceneGroup.Text = "Scene Group:"
        ' 
        ' txtSceneGroup
        ' 
        txtSceneGroup.Enabled = False
        txtSceneGroup.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtSceneGroup.Location = New Point(35, 219)
        txtSceneGroup.Margin = New Padding(4, 3, 4, 3)
        txtSceneGroup.Name = "txtSceneGroup"
        txtSceneGroup.ReadOnly = True
        txtSceneGroup.Size = New Size(300, 29)
        txtSceneGroup.TabIndex = 16
        ' 
        ' lblReleaseDate
        ' 
        lblReleaseDate.AutoSize = True
        lblReleaseDate.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblReleaseDate.Location = New Point(35, 277)
        lblReleaseDate.Margin = New Padding(4, 0, 4, 0)
        lblReleaseDate.Name = "lblReleaseDate"
        lblReleaseDate.Size = New Size(98, 14)
        lblReleaseDate.TabIndex = 19
        lblReleaseDate.Text = "Release Date:"
        ' 
        ' txtReleaseDate
        ' 
        txtReleaseDate.Enabled = False
        txtReleaseDate.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtReleaseDate.Location = New Point(35, 300)
        txtReleaseDate.Margin = New Padding(4, 3, 4, 3)
        txtReleaseDate.Name = "txtReleaseDate"
        txtReleaseDate.ReadOnly = True
        txtReleaseDate.Size = New Size(300, 29)
        txtReleaseDate.TabIndex = 18
        ' 
        ' grpGameInfo
        ' 
        grpGameInfo.Controls.Add(txtPublisher)
        grpGameInfo.Controls.Add(lblPublisher)
        grpGameInfo.Controls.Add(txtDeveloper)
        grpGameInfo.Controls.Add(lblReleaseDate)
        grpGameInfo.Controls.Add(lblDeveloper)
        grpGameInfo.Controls.Add(txtReleaseDate)
        grpGameInfo.Controls.Add(txtSceneGroup)
        grpGameInfo.Controls.Add(lblSceneGroup)
        grpGameInfo.Location = New Point(507, 46)
        grpGameInfo.Margin = New Padding(4, 3, 4, 3)
        grpGameInfo.Name = "grpGameInfo"
        grpGameInfo.Padding = New Padding(4, 3, 4, 3)
        grpGameInfo.Size = New Size(371, 369)
        grpGameInfo.TabIndex = 22
        grpGameInfo.TabStop = False
        grpGameInfo.Text = "Game Information"
        ' 
        ' grpCrackInfo
        ' 
        grpCrackInfo.Controls.Add(txtCrackStatus)
        grpCrackInfo.Controls.Add(lblCrackStatus)
        grpCrackInfo.Controls.Add(txtCrackDate)
        grpCrackInfo.Controls.Add(lblCrackType)
        grpCrackInfo.Controls.Add(lblCrackDate)
        grpCrackInfo.Controls.Add(txtCrackType)
        grpCrackInfo.Location = New Point(507, 438)
        grpCrackInfo.Margin = New Padding(4, 3, 4, 3)
        grpCrackInfo.Name = "grpCrackInfo"
        grpCrackInfo.Padding = New Padding(4, 3, 4, 3)
        grpCrackInfo.Size = New Size(371, 278)
        grpCrackInfo.TabIndex = 28
        grpCrackInfo.TabStop = False
        grpCrackInfo.Text = "Crack Information"
        ' 
        ' txtCrackStatus
        ' 
        txtCrackStatus.Enabled = False
        txtCrackStatus.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCrackStatus.Location = New Point(35, 58)
        txtCrackStatus.Margin = New Padding(4, 3, 4, 3)
        txtCrackStatus.Name = "txtCrackStatus"
        txtCrackStatus.ReadOnly = True
        txtCrackStatus.Size = New Size(300, 29)
        txtCrackStatus.TabIndex = 12
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
        ' txtCrackDate
        ' 
        txtCrackDate.Enabled = False
        txtCrackDate.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCrackDate.Location = New Point(35, 138)
        txtCrackDate.Margin = New Padding(4, 3, 4, 3)
        txtCrackDate.Name = "txtCrackDate"
        txtCrackDate.ReadOnly = True
        txtCrackDate.Size = New Size(300, 29)
        txtCrackDate.TabIndex = 14
        ' 
        ' lblCrackType
        ' 
        lblCrackType.AutoSize = True
        lblCrackType.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCrackType.Location = New Point(35, 198)
        lblCrackType.Margin = New Padding(4, 0, 4, 0)
        lblCrackType.Name = "lblCrackType"
        lblCrackType.Size = New Size(84, 14)
        lblCrackType.TabIndex = 19
        lblCrackType.Text = "Crack Type:"
        ' 
        ' lblCrackDate
        ' 
        lblCrackDate.AutoSize = True
        lblCrackDate.Font = New Font("JetBrains Mono", 8.249999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblCrackDate.Location = New Point(35, 115)
        lblCrackDate.Margin = New Padding(4, 0, 4, 0)
        lblCrackDate.Name = "lblCrackDate"
        lblCrackDate.Size = New Size(84, 14)
        lblCrackDate.TabIndex = 15
        lblCrackDate.Text = "Crack Date:"
        ' 
        ' txtCrackType
        ' 
        txtCrackType.Enabled = False
        txtCrackType.Font = New Font("JetBrains Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCrackType.Location = New Point(35, 221)
        txtCrackType.Margin = New Padding(4, 3, 4, 3)
        txtCrackType.Name = "txtCrackType"
        txtCrackType.ReadOnly = True
        txtCrackType.Size = New Size(300, 29)
        txtCrackType.TabIndex = 18
        ' 
        ' picGameCover
        ' 
        picGameCover.Location = New Point(916, 58)
        picGameCover.Margin = New Padding(4, 3, 4, 3)
        picGameCover.Name = "picGameCover"
        picGameCover.Size = New Size(373, 554)
        picGameCover.SizeMode = PictureBoxSizeMode.StretchImage
        picGameCover.TabIndex = 29
        picGameCover.TabStop = False
        ' 
        ' btnSave
        ' 
        btnSave.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnSave.Location = New Point(997, 646)
        btnSave.Margin = New Padding(4, 3, 4, 3)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(208, 57)
        btnSave.TabIndex = 30
        btnSave.Text = "Save as CSV"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' btnReturn
        ' 
        btnReturn.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReturn.Location = New Point(997, 727)
        btnReturn.Margin = New Padding(4, 3, 4, 3)
        btnReturn.Name = "btnReturn"
        btnReturn.Size = New Size(208, 57)
        btnReturn.TabIndex = 31
        btnReturn.Text = "Return to Search"
        btnReturn.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button1.Location = New Point(1249, 58)
        Button1.Name = "Button1"
        Button1.Size = New Size(40, 37)
        Button1.TabIndex = 32
        Button1.Text = "🌐"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' frmResults
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1323, 832)
        Controls.Add(Button1)
        Controls.Add(btnReturn)
        Controls.Add(btnSave)
        Controls.Add(picGameCover)
        Controls.Add(grpCrackInfo)
        Controls.Add(grpGameInfo)
        Controls.Add(lblGames)
        Controls.Add(lbGames)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmResults"
        Text = "Denuvo Watch: Game Status"
        grpGameInfo.ResumeLayout(False)
        grpGameInfo.PerformLayout()
        grpCrackInfo.ResumeLayout(False)
        grpCrackInfo.PerformLayout()
        CType(picGameCover, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lbGames As ListBox
    Friend WithEvents lblGames As Label
    Friend WithEvents txtPublisher As TextBox
    Friend WithEvents lblPublisher As Label
    Friend WithEvents lblDeveloper As Label
    Friend WithEvents txtDeveloper As TextBox
    Friend WithEvents lblSceneGroup As Label
    Friend WithEvents txtSceneGroup As TextBox
    Friend WithEvents lblReleaseDate As Label
    Friend WithEvents txtReleaseDate As TextBox
    Friend WithEvents grpGameInfo As GroupBox
    Friend WithEvents grpCrackInfo As GroupBox
    Friend WithEvents txtCrackStatus As TextBox
    Friend WithEvents lblCrackStatus As Label
    Friend WithEvents txtCrackDate As TextBox
    Friend WithEvents lblCrackType As Label
    Friend WithEvents lblCrackDate As Label
    Friend WithEvents txtCrackType As TextBox
    Friend WithEvents picGameCover As PictureBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnReturn As Button
    Friend WithEvents Button1 As Button
End Class
