<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExport
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
    'Do not modify using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExport))
        rtbExportPreview = New RichTextBox()
        btnExport = New Button()
        btnReturnExplorer = New Button()
        btnReturnSearch = New Button()
        Label1 = New Label()
        Label2 = New Label()
        gbExportFormats = New GroupBox()
        rbFormatXML = New RadioButton()
        rbFormatMarkdown = New RadioButton()
        rbFormatHTML = New RadioButton()
        rbFormatJSON = New RadioButton()
        rbFormatCSV = New RadioButton()
        rbFormatText = New RadioButton()
        btnThemeToggle = New Button()
        gbExportColumns = New GroupBox()
        cbColSceneGroup = New CheckBox()
        cbColCrackDate = New CheckBox()
        cbColCrackStatus = New CheckBox()
        cbColReleaseDate = New CheckBox()
        cbColPublisher = New CheckBox()
        cbColDeveloper = New CheckBox()
        cbColTitle = New CheckBox()
        gbExportSorting = New GroupBox()
        rbSortReleaseDate = New RadioButton()
        rbSortCrackStatus = New RadioButton()
        rbSortTitleZA = New RadioButton()
        rbSortTitleAZ = New RadioButton()
        rbSortNone = New RadioButton()
        gbExportFormats.SuspendLayout()
        gbExportColumns.SuspendLayout()
        gbExportSorting.SuspendLayout()
        SuspendLayout()
        ' 
        ' rtbExportPreview
        ' 
        rtbExportPreview.Location = New Point(112, 110)
        rtbExportPreview.Margin = New Padding(4, 3, 4, 3)
        rtbExportPreview.Name = "rtbExportPreview"
        rtbExportPreview.Size = New Size(689, 160)
        rtbExportPreview.TabIndex = 0
        rtbExportPreview.Text = ""
        ' 
        ' btnExport
        ' 
        btnExport.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnExport.Location = New Point(351, 439)
        btnExport.Margin = New Padding(4, 3, 4, 3)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(210, 58)
        btnExport.TabIndex = 31
        btnExport.Text = "Export"
        btnExport.UseVisualStyleBackColor = True
        ' 
        ' btnReturnExplorer
        ' 
        btnReturnExplorer.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReturnExplorer.Location = New Point(59, 439)
        btnReturnExplorer.Margin = New Padding(4, 3, 4, 3)
        btnReturnExplorer.Name = "btnReturnExplorer"
        btnReturnExplorer.Size = New Size(210, 58)
        btnReturnExplorer.TabIndex = 32
        btnReturnExplorer.Text = "Return to Explorer"
        btnReturnExplorer.UseVisualStyleBackColor = True
        ' 
        ' btnReturnSearch
        ' 
        btnReturnSearch.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReturnSearch.Location = New Point(643, 439)
        btnReturnSearch.Margin = New Padding(4, 3, 4, 3)
        btnReturnSearch.Name = "btnReturnSearch"
        btnReturnSearch.Size = New Size(210, 58)
        btnReturnSearch.TabIndex = 33
        btnReturnSearch.Text = "Return to Search"
        btnReturnSearch.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.Font = New Font("JetBrains Mono", 20.25F, FontStyle.Bold Or FontStyle.Underline)
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(891, 41)
        Label1.TabIndex = 34
        Label1.Text = "Export"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.Font = New Font("JetBrains Mono", 20.25F, FontStyle.Bold)
        Label2.Location = New Point(112, 66)
        Label2.Name = "Label2"
        Label2.Size = New Size(689, 36)
        Label2.TabIndex = 35
        Label2.Text = "Live Preview"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' gbExportFormats
        ' 
        gbExportFormats.Controls.Add(rbFormatXML)
        gbExportFormats.Controls.Add(rbFormatMarkdown)
        gbExportFormats.Controls.Add(rbFormatHTML)
        gbExportFormats.Controls.Add(rbFormatJSON)
        gbExportFormats.Controls.Add(rbFormatCSV)
        gbExportFormats.Controls.Add(rbFormatText)
        gbExportFormats.Location = New Point(112, 276)
        gbExportFormats.Name = "gbExportFormats"
        gbExportFormats.Size = New Size(140, 150)
        gbExportFormats.TabIndex = 36
        gbExportFormats.TabStop = False
        gbExportFormats.Text = "Format"
        ' 
        ' rbFormatXML
        ' 
        rbFormatXML.AutoSize = True
        rbFormatXML.Location = New Point(15, 120)
        rbFormatXML.Name = "rbFormatXML"
        rbFormatXML.Size = New Size(49, 19)
        rbFormatXML.TabIndex = 5
        rbFormatXML.TabStop = True
        rbFormatXML.Text = "XML"
        rbFormatXML.UseVisualStyleBackColor = True
        ' 
        ' rbFormatMarkdown
        ' 
        rbFormatMarkdown.AutoSize = True
        rbFormatMarkdown.Location = New Point(15, 100)
        rbFormatMarkdown.Name = "rbFormatMarkdown"
        rbFormatMarkdown.Size = New Size(82, 19)
        rbFormatMarkdown.TabIndex = 4
        rbFormatMarkdown.TabStop = True
        rbFormatMarkdown.Text = "Markdown"
        rbFormatMarkdown.UseVisualStyleBackColor = True
        ' 
        ' rbFormatHTML
        ' 
        rbFormatHTML.AutoSize = True
        rbFormatHTML.Location = New Point(15, 80)
        rbFormatHTML.Name = "rbFormatHTML"
        rbFormatHTML.Size = New Size(58, 19)
        rbFormatHTML.TabIndex = 3
        rbFormatHTML.TabStop = True
        rbFormatHTML.Text = "HTML"
        rbFormatHTML.UseVisualStyleBackColor = True
        ' 
        ' rbFormatJSON
        ' 
        rbFormatJSON.AutoSize = True
        rbFormatJSON.Location = New Point(15, 60)
        rbFormatJSON.Name = "rbFormatJSON"
        rbFormatJSON.Size = New Size(53, 19)
        rbFormatJSON.TabIndex = 2
        rbFormatJSON.TabStop = True
        rbFormatJSON.Text = "JSON"
        rbFormatJSON.UseVisualStyleBackColor = True
        ' 
        ' rbFormatCSV
        ' 
        rbFormatCSV.AutoSize = True
        rbFormatCSV.Location = New Point(15, 40)
        rbFormatCSV.Name = "rbFormatCSV"
        rbFormatCSV.Size = New Size(46, 19)
        rbFormatCSV.TabIndex = 1
        rbFormatCSV.TabStop = True
        rbFormatCSV.Text = "CSV"
        rbFormatCSV.UseVisualStyleBackColor = True
        ' 
        ' rbFormatText
        ' 
        rbFormatText.AutoSize = True
        rbFormatText.Checked = True
        rbFormatText.Location = New Point(15, 20)
        rbFormatText.Name = "rbFormatText"
        rbFormatText.Size = New Size(46, 19)
        rbFormatText.TabIndex = 0
        rbFormatText.TabStop = True
        rbFormatText.Text = "Text"
        rbFormatText.UseVisualStyleBackColor = True
        ' 
        ' btnThemeToggle
        ' 
        btnThemeToggle.FlatAppearance.BorderSize = 0
        btnThemeToggle.FlatStyle = FlatStyle.Flat
        btnThemeToggle.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnThemeToggle.Location = New Point(858, 9)
        btnThemeToggle.Name = "btnThemeToggle"
        btnThemeToggle.Size = New Size(45, 41)
        btnThemeToggle.TabIndex = 39
        btnThemeToggle.Text = "☀️"
        btnThemeToggle.UseVisualStyleBackColor = True
        ' 
        ' gbExportColumns
        ' 
        gbExportColumns.Controls.Add(cbColSceneGroup)
        gbExportColumns.Controls.Add(cbColCrackDate)
        gbExportColumns.Controls.Add(cbColCrackStatus)
        gbExportColumns.Controls.Add(cbColReleaseDate)
        gbExportColumns.Controls.Add(cbColPublisher)
        gbExportColumns.Controls.Add(cbColDeveloper)
        gbExportColumns.Controls.Add(cbColTitle)
        gbExportColumns.Location = New Point(260, 276)
        gbExportColumns.Name = "gbExportColumns"
        gbExportColumns.Size = New Size(230, 150)
        gbExportColumns.TabIndex = 37
        gbExportColumns.TabStop = False
        gbExportColumns.Text = "Columns"
        ' 
        ' cbColSceneGroup
        ' 
        cbColSceneGroup.AutoSize = True
        cbColSceneGroup.Checked = True
        cbColSceneGroup.CheckState = CheckState.Checked
        cbColSceneGroup.Location = New Point(15, 128)
        cbColSceneGroup.Name = "cbColSceneGroup"
        cbColSceneGroup.Size = New Size(93, 19)
        cbColSceneGroup.TabIndex = 6
        cbColSceneGroup.Text = "Scene Group"
        cbColSceneGroup.UseVisualStyleBackColor = True
        ' 
        ' cbColCrackDate
        ' 
        cbColCrackDate.AutoSize = True
        cbColCrackDate.Checked = True
        cbColCrackDate.CheckState = CheckState.Checked
        cbColCrackDate.Location = New Point(15, 110)
        cbColCrackDate.Name = "cbColCrackDate"
        cbColCrackDate.Size = New Size(83, 19)
        cbColCrackDate.TabIndex = 5
        cbColCrackDate.Text = "Crack Date"
        cbColCrackDate.UseVisualStyleBackColor = True
        ' 
        ' cbColCrackStatus
        ' 
        cbColCrackStatus.AutoSize = True
        cbColCrackStatus.Checked = True
        cbColCrackStatus.CheckState = CheckState.Checked
        cbColCrackStatus.Location = New Point(15, 92)
        cbColCrackStatus.Name = "cbColCrackStatus"
        cbColCrackStatus.Size = New Size(91, 19)
        cbColCrackStatus.TabIndex = 4
        cbColCrackStatus.Text = "Crack Status"
        cbColCrackStatus.UseVisualStyleBackColor = True
        ' 
        ' cbColReleaseDate
        ' 
        cbColReleaseDate.AutoSize = True
        cbColReleaseDate.Checked = True
        cbColReleaseDate.CheckState = CheckState.Checked
        cbColReleaseDate.Location = New Point(15, 74)
        cbColReleaseDate.Name = "cbColReleaseDate"
        cbColReleaseDate.Size = New Size(92, 19)
        cbColReleaseDate.TabIndex = 3
        cbColReleaseDate.Text = "Release Date"
        cbColReleaseDate.UseVisualStyleBackColor = True
        ' 
        ' cbColPublisher
        ' 
        cbColPublisher.AutoSize = True
        cbColPublisher.Checked = True
        cbColPublisher.CheckState = CheckState.Checked
        cbColPublisher.Location = New Point(15, 56)
        cbColPublisher.Name = "cbColPublisher"
        cbColPublisher.Size = New Size(75, 19)
        cbColPublisher.TabIndex = 2
        cbColPublisher.Text = "Publisher"
        cbColPublisher.UseVisualStyleBackColor = True
        ' 
        ' cbColDeveloper
        ' 
        cbColDeveloper.AutoSize = True
        cbColDeveloper.Checked = True
        cbColDeveloper.CheckState = CheckState.Checked
        cbColDeveloper.Location = New Point(15, 38)
        cbColDeveloper.Name = "cbColDeveloper"
        cbColDeveloper.Size = New Size(79, 19)
        cbColDeveloper.TabIndex = 1
        cbColDeveloper.Text = "Developer"
        cbColDeveloper.UseVisualStyleBackColor = True
        ' 
        ' cbColTitle
        ' 
        cbColTitle.AutoSize = True
        cbColTitle.Checked = True
        cbColTitle.CheckState = CheckState.Checked
        cbColTitle.Location = New Point(15, 20)
        cbColTitle.Name = "cbColTitle"
        cbColTitle.Size = New Size(49, 19)
        cbColTitle.TabIndex = 0
        cbColTitle.Text = "Title"
        cbColTitle.UseVisualStyleBackColor = True
        ' 
        ' gbExportSorting
        ' 
        gbExportSorting.Controls.Add(rbSortReleaseDate)
        gbExportSorting.Controls.Add(rbSortCrackStatus)
        gbExportSorting.Controls.Add(rbSortTitleZA)
        gbExportSorting.Controls.Add(rbSortTitleAZ)
        gbExportSorting.Controls.Add(rbSortNone)
        gbExportSorting.Location = New Point(500, 276)
        gbExportSorting.Name = "gbExportSorting"
        gbExportSorting.Size = New Size(230, 150)
        gbExportSorting.TabIndex = 38
        gbExportSorting.TabStop = False
        gbExportSorting.Text = "Sorting"
        ' 
        ' rbSortReleaseDate
        ' 
        rbSortReleaseDate.AutoSize = True
        rbSortReleaseDate.Location = New Point(15, 100)
        rbSortReleaseDate.Name = "rbSortReleaseDate"
        rbSortReleaseDate.Size = New Size(91, 19)
        rbSortReleaseDate.TabIndex = 4
        rbSortReleaseDate.TabStop = True
        rbSortReleaseDate.Text = "Release Date"
        rbSortReleaseDate.UseVisualStyleBackColor = True
        ' 
        ' rbSortCrackStatus
        ' 
        rbSortCrackStatus.AutoSize = True
        rbSortCrackStatus.Location = New Point(15, 80)
        rbSortCrackStatus.Name = "rbSortCrackStatus"
        rbSortCrackStatus.Size = New Size(90, 19)
        rbSortCrackStatus.TabIndex = 3
        rbSortCrackStatus.TabStop = True
        rbSortCrackStatus.Text = "Crack Status"
        rbSortCrackStatus.UseVisualStyleBackColor = True
        ' 
        ' rbSortTitleZA
        ' 
        rbSortTitleZA.AutoSize = True
        rbSortTitleZA.Location = New Point(15, 60)
        rbSortTitleZA.Name = "rbSortTitleZA"
        rbSortTitleZA.Size = New Size(71, 19)
        rbSortTitleZA.TabIndex = 2
        rbSortTitleZA.TabStop = True
        rbSortTitleZA.Text = "Title Z-A"
        rbSortTitleZA.UseVisualStyleBackColor = True
        ' 
        ' rbSortTitleAZ
        ' 
        rbSortTitleAZ.AutoSize = True
        rbSortTitleAZ.Location = New Point(15, 40)
        rbSortTitleAZ.Name = "rbSortTitleAZ"
        rbSortTitleAZ.Size = New Size(71, 19)
        rbSortTitleAZ.TabIndex = 1
        rbSortTitleAZ.TabStop = True
        rbSortTitleAZ.Text = "Title A-Z"
        rbSortTitleAZ.UseVisualStyleBackColor = True
        ' 
        ' rbSortNone
        ' 
        rbSortNone.AutoSize = True
        rbSortNone.Checked = True
        rbSortNone.Location = New Point(15, 20)
        rbSortNone.Name = "rbSortNone"
        rbSortNone.Size = New Size(54, 19)
        rbSortNone.TabIndex = 0
        rbSortNone.TabStop = True
        rbSortNone.Text = "None"
        rbSortNone.UseVisualStyleBackColor = True
        ' 
        ' frmExport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(915, 509)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Controls.Add(btnThemeToggle)
        Controls.Add(gbExportSorting)
        Controls.Add(gbExportColumns)
        Controls.Add(gbExportFormats)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnReturnSearch)
        Controls.Add(btnReturnExplorer)
        Controls.Add(btnExport)
        Controls.Add(rtbExportPreview)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmExport"
        Text = "DenuvoWatch - Export"
        gbExportFormats.ResumeLayout(False)
        gbExportFormats.PerformLayout()
        gbExportColumns.ResumeLayout(False)
        gbExportColumns.PerformLayout()
        gbExportSorting.ResumeLayout(False)
        gbExportSorting.PerformLayout()
        ResumeLayout(False)

    End Sub

    Friend WithEvents rtbExportPreview As RichTextBox
    Friend WithEvents btnExport As Button
    Friend WithEvents btnReturnExplorer As Button
    Friend WithEvents btnReturnSearch As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents gbExportFormats As GroupBox
    Friend WithEvents gbExportColumns As GroupBox
    Friend WithEvents gbExportSorting As GroupBox
    Friend WithEvents rbFormatText As RadioButton
    Friend WithEvents rbFormatCSV As RadioButton
    Friend WithEvents rbFormatJSON As RadioButton
    Friend WithEvents rbFormatHTML As RadioButton
    Friend WithEvents rbFormatMarkdown As RadioButton
    Friend WithEvents rbFormatXML As RadioButton
    Friend WithEvents cbColTitle As CheckBox
    Friend WithEvents cbColDeveloper As CheckBox
    Friend WithEvents cbColPublisher As CheckBox
    Friend WithEvents cbColReleaseDate As CheckBox
    Friend WithEvents cbColCrackStatus As CheckBox
    Friend WithEvents cbColCrackDate As CheckBox
    Friend WithEvents cbColSceneGroup As CheckBox
    Friend WithEvents rbSortNone As RadioButton
    Friend WithEvents rbSortTitleAZ As RadioButton
    Friend WithEvents rbSortTitleZA As RadioButton
    Friend WithEvents rbSortCrackStatus As RadioButton
    Friend WithEvents rbSortReleaseDate As RadioButton
    Friend WithEvents btnThemeToggle As Button
End Class
