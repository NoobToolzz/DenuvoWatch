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
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        RichTextBox1 = New RichTextBox()
        btnSearch = New Button()
        btnReturnExplorer = New Button()
        btnReturnSearch = New Button()
        SuspendLayout()
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Location = New Point(183, 53)
        RichTextBox1.Margin = New Padding(4, 3, 4, 3)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(530, 316)
        RichTextBox1.TabIndex = 0
        RichTextBox1.Text = ""
        ' 
        ' btnSearch
        ' 
        btnSearch.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(350, 404)
        btnSearch.Margin = New Padding(4, 3, 4, 3)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(210, 58)
        btnSearch.TabIndex = 31
        btnSearch.Text = "Export"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' btnReturnExplorer
        ' 
        btnReturnExplorer.Font = New Font("JetBrains Mono", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnReturnExplorer.Location = New Point(58, 404)
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
        btnReturnSearch.Location = New Point(642, 404)
        btnReturnSearch.Margin = New Padding(4, 3, 4, 3)
        btnReturnSearch.Name = "btnReturnSearch"
        btnReturnSearch.Size = New Size(210, 58)
        btnReturnSearch.TabIndex = 33
        btnReturnSearch.Text = "Return to Search"
        btnReturnSearch.UseVisualStyleBackColor = True
        ' 
        ' frmExport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(915, 509)
        Controls.Add(btnReturnSearch)
        Controls.Add(btnReturnExplorer)
        Controls.Add(btnSearch)
        Controls.Add(RichTextBox1)
        Margin = New Padding(4, 3, 4, 3)
        Name = "frmExport"
        Text = "Denuvo Watch: Export"
        ResumeLayout(False)

    End Sub

    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnReturnExplorer As Button
    Friend WithEvents btnReturnSearch As Button
End Class
