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
        pgbLoader = New ProgressBar()
        lblStatus = New Label()
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
        lblStatus.Location = New Point(12, 9)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(495, 16)
        lblStatus.TabIndex = 1
        lblStatus.Text = "Initializing..."
        lblStatus.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' frmLoader
        ' 
        AutoScaleDimensions = New SizeF(7F, 16F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(519, 71)
        Controls.Add(lblStatus)
        Controls.Add(pgbLoader)
        Font = New Font("JetBrains Mono", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Name = "frmLoader"
        Text = "DenuvoWatch - Loader"
        ResumeLayout(False)
    End Sub
    Friend WithEvents pgbLoader As ProgressBar
    Friend WithEvents lblStatus As Label
End Class
