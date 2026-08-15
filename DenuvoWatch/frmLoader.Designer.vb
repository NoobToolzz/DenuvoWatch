<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLoader
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLoader))
        pgbLoader = New ProgressBar()
        lblStatus = New Label()
        rtbLoaderLogs = New RichTextBox()
        btnThemeToggle = New Button()
        SuspendLayout()
        ' 
        ' pgbLoader
        ' 
        pgbLoader.Location = New Point(12, 28)
        pgbLoader.Name = "pgbLoader"
        pgbLoader.Size = New Size(495, 32)
        pgbLoader.TabIndex = 0
        ' 
        ' lblStatus
        ' 
        lblStatus.Location = New Point(10, 9)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(497, 16)
        lblStatus.TabIndex = 1
        lblStatus.Text = "Initializing..."
        lblStatus.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' rtbLoaderLogs
        ' 
        rtbLoaderLogs.Location = New Point(12, 66)
        rtbLoaderLogs.Name = "rtbLoaderLogs"
        rtbLoaderLogs.ReadOnly = True
        rtbLoaderLogs.Size = New Size(495, 138)
        rtbLoaderLogs.TabIndex = 2
        rtbLoaderLogs.Text = ""
        ' 
        ' btnThemeToggle
        ' 
        btnThemeToggle.FlatAppearance.BorderSize = 0
        btnThemeToggle.FlatStyle = FlatStyle.Flat
        btnThemeToggle.Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnThemeToggle.Location = New Point(462, 2)
        btnThemeToggle.Name = "btnThemeToggle"
        btnThemeToggle.Size = New Size(45, 23)
        btnThemeToggle.TabIndex = 3
        btnThemeToggle.Text = "☀️"
        btnThemeToggle.UseVisualStyleBackColor = True
        ' 
        ' frmLoader
        ' 
        AutoScaleDimensions = New SizeF(7F, 16F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(519, 216)
        Controls.Add(btnThemeToggle)
        Controls.Add(rtbLoaderLogs)
        Controls.Add(lblStatus)
        Controls.Add(pgbLoader)
        Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "frmLoader"
        Text = "DenuvoWatch - Loader"
        ResumeLayout(False)
    End Sub
    Friend WithEvents pgbLoader As ProgressBar
    Friend WithEvents lblStatus As Label
    Friend WithEvents rtbLoaderLogs As RichTextBox
    Friend WithEvents btnThemeToggle As Button
End Class
