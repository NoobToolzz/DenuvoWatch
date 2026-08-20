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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearch))
        lblTitle = New Label()
        cbPublisher = New ComboBox()
        cbDeveloper = New ComboBox()
        cbSceneGroup = New ComboBox()
        btnSearch = New Button()
        tbQuery = New TextBox()
        Label1 = New Label()
        gbFilters = New GroupBox()
        toolTipFilters = New ToolTip(components)
        btnThemeToggle = New Button()
        fbFiltersPrice = New GroupBox()
        cbPriceCurrency = New ComboBox()
        cbPriceRange = New ComboBox()
        cbPriceOperator = New ComboBox()
        gbFilters.SuspendLayout()
        fbFiltersPrice.SuspendLayout()
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
        btnSearch.Location = New Point(362, 210)
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
        gbFilters.Controls.Add(fbFiltersPrice)
        gbFilters.Controls.Add(cbPublisher)
        gbFilters.Controls.Add(cbDeveloper)
        gbFilters.Controls.Add(cbSceneGroup)
        gbFilters.Location = New Point(39, 110)
        gbFilters.Name = "gbFilters"
        gbFilters.Size = New Size(267, 250)
        gbFilters.TabIndex = 7
        gbFilters.TabStop = False
        gbFilters.Text = "Filters"
        ' 
        ' btnThemeToggle
        ' 
        btnThemeToggle.FlatAppearance.BorderSize = 0
        btnThemeToggle.FlatStyle = FlatStyle.Flat
        btnThemeToggle.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnThemeToggle.Location = New Point(552, 9)
        btnThemeToggle.Name = "btnThemeToggle"
        btnThemeToggle.Size = New Size(45, 39)
        btnThemeToggle.TabIndex = 8
        btnThemeToggle.Text = "☀️"
        btnThemeToggle.UseVisualStyleBackColor = True
        ' 
        ' fbFiltersPrice
        ' 
        fbFiltersPrice.Controls.Add(cbPriceCurrency)
        fbFiltersPrice.Controls.Add(cbPriceRange)
        fbFiltersPrice.Controls.Add(cbPriceOperator)
        fbFiltersPrice.Location = New Point(20, 186)
        fbFiltersPrice.Name = "fbFiltersPrice"
        fbFiltersPrice.Size = New Size(229, 56)
        fbFiltersPrice.TabIndex = 4
        fbFiltersPrice.TabStop = False
        fbFiltersPrice.Text = "Price"
        ' 
        ' cbPriceCurrency
        ' 
        cbPriceCurrency.DropDownStyle = ComboBoxStyle.DropDownList
        cbPriceCurrency.FormattingEnabled = True
        cbPriceCurrency.Items.AddRange(New Object() {"USD ($)", "AUD (A$)", "EUR (€)"})
        cbPriceCurrency.Location = New Point(146, 22)
        cbPriceCurrency.Name = "cbPriceCurrency"
        cbPriceCurrency.Size = New Size(77, 23)
        cbPriceCurrency.TabIndex = 2
        cbPriceCurrency.SelectedIndex = 0
        ' 
        ' cbPriceRange
        ' 
        cbPriceRange.FormattingEnabled = True
        cbPriceRange.Items.AddRange(New Object() {"", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100"})
        cbPriceRange.Location = New Point(76, 22)
        cbPriceRange.Name = "cbPriceRange"
        cbPriceRange.Size = New Size(64, 23)
        cbPriceRange.TabIndex = 1
        ' 
        ' cbPriceOperator
        ' 
        cbPriceOperator.DropDownStyle = ComboBoxStyle.DropDownList
        cbPriceOperator.FormattingEnabled = True
        cbPriceOperator.Items.AddRange(New Object() {"", ">", "<", "="})
        cbPriceOperator.Location = New Point(6, 22)
        cbPriceOperator.Name = "cbPriceOperator"
        cbPriceOperator.Size = New Size(64, 23)
        cbPriceOperator.TabIndex = 0
        cbPriceOperator.SelectedIndex = 1
        ' 
        ' frmSearch
        ' 
        AcceptButton = btnSearch
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.Control
        ClientSize = New Size(610, 375)
        Controls.Add(btnThemeToggle)
        Controls.Add(gbFilters)
        Controls.Add(Label1)
        Controls.Add(tbQuery)
        Controls.Add(btnSearch)
        Controls.Add(lblTitle)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(4, 3, 4, 3)
        MaximizeBox = False
        Name = "frmSearch"
        Text = "DenuvoWatch - Search"
        gbFilters.ResumeLayout(False)
        fbFiltersPrice.ResumeLayout(False)
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
    Friend WithEvents btnThemeToggle As Button
    Friend WithEvents fbFiltersPrice As GroupBox
    Friend WithEvents cbPriceOperator As ComboBox
    Friend WithEvents cbPriceRange As ComboBox
    Friend WithEvents cbPriceCurrency As ComboBox
End Class
