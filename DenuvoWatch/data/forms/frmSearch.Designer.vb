<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch
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
        components = New ComponentModel.Container()
        lblTitle = New Label()
        cbPublisher = New ComboBox()
        cbDeveloper = New ComboBox()
        cbSceneGroup = New ComboBox()
        btnSearch = New Button()
        tbQuery = New TextBox()
        Label1 = New Label()
        gbFilters = New GroupBox()
        toolTipFilters = New ToolTip(components)
        gbFilters.SuspendLayout()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = SystemColors.Control
        lblTitle.Font = New Font("JetBrains Mono", 21.75F, FontStyle.Bold Or FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(13, 9)
        lblTitle.Margin = New Padding(4, 0, 4, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(584, 39)
        lblTitle.TabIndex = 0
        lblTitle.Text = "DenuvoWatch"
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' cbPublisher
        ' 
        cbPublisher.BackColor = SystemColors.ButtonFace
        cbPublisher.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cbPublisher.FormattingEnabled = True
        cbPublisher.Location = New Point(20, 31)
        cbPublisher.Margin = New Padding(4, 3, 4, 3)
        cbPublisher.Name = "cbPublisher"
        cbPublisher.Size = New Size(229, 24)
        cbPublisher.TabIndex = 1
        cbPublisher.Text = "Publisher"
        ' 
        ' cbDeveloper
        ' 
        cbDeveloper.BackColor = SystemColors.ButtonFace
        cbDeveloper.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cbDeveloper.FormattingEnabled = True
        cbDeveloper.Location = New Point(20, 88)
        cbDeveloper.Margin = New Padding(4, 3, 4, 3)
        cbDeveloper.Name = "cbDeveloper"
        cbDeveloper.Size = New Size(229, 24)
        cbDeveloper.TabIndex = 2
        cbDeveloper.Text = "Developer"
        ' 
        ' cbSceneGroup
        ' 
        cbSceneGroup.BackColor = SystemColors.ButtonFace
        cbSceneGroup.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cbSceneGroup.FormattingEnabled = True
        cbSceneGroup.Location = New Point(20, 143)
        cbSceneGroup.Margin = New Padding(4, 3, 4, 3)
        cbSceneGroup.Name = "cbSceneGroup"
        cbSceneGroup.Size = New Size(229, 24)
        cbSceneGroup.TabIndex = 3
        cbSceneGroup.Text = "Scene Group"
        ' 
        ' btnSearch
        ' 
        btnSearch.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(352, 179)
        btnSearch.Margin = New Padding(4, 3, 4, 3)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(208, 57)
        btnSearch.TabIndex = 4
        btnSearch.Text = "Search for Games"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' tbQuery
        ' 
        tbQuery.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tbQuery.Location = New Point(106, 70)
        tbQuery.Margin = New Padding(4, 3, 4, 3)
        tbQuery.Name = "tbQuery"
        tbQuery.PlaceholderText = "Enter a search query / AppID"
        tbQuery.Size = New Size(464, 23)
        tbQuery.TabIndex = 5
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(39, 75)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(49, 16)
        Label1.TabIndex = 6
        Label1.Text = "Query:"
        ' 
        ' gbFilters
        ' 
        gbFilters.Controls.Add(cbPublisher)
        gbFilters.Controls.Add(cbDeveloper)
        gbFilters.Controls.Add(cbSceneGroup)
        gbFilters.Location = New Point(39, 110)
        gbFilters.Name = "gbFilters"
        gbFilters.Size = New Size(267, 187)
        gbFilters.TabIndex = 7
        gbFilters.TabStop = False
        gbFilters.Text = "Filters"
        ' 
        ' frmSearch
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.Control
        ClientSize = New Size(610, 320)
        Controls.Add(gbFilters)
        Controls.Add(Label1)
        Controls.Add(tbQuery)
        Controls.Add(btnSearch)
        Controls.Add(lblTitle)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmSearch"
        Text = "DenuvoWatch - Search"
        gbFilters.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents cbPublisher As ComboBox
    Friend WithEvents cbDeveloper As ComboBox
    Friend WithEvents cbSceneGroup As ComboBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents tbQuery As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents gbFilters As GroupBox
    Friend WithEvents toolTipFilters As System.Windows.Forms.ToolTip
End Class
