<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBackgroundWorker_0
    Inherits MyFormsLibrary.FormBaseDetail_3


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
        Me.txtLogging = New System.Windows.Forms.TextBox()
        Me.TabPage1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtLogging)
        Me.TabPage1.Text = "Console"
        '
        'SplitContainer1
        '
        '
        'txtLogging
        '
        Me.txtLogging.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLogging.Location = New System.Drawing.Point(3, 3)
        Me.txtLogging.Multiline = True
        Me.txtLogging.Name = "txtLogging"
        Me.txtLogging.ReadOnly = True
        Me.txtLogging.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLogging.Size = New System.Drawing.Size(692, 191)
        Me.txtLogging.TabIndex = 0
        '
        'FormBackgroundWorker_0
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBackgroundWorker_0"
        Me.Text = "FormCheckServices"
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControlDetail.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtLogging As System.Windows.Forms.TextBox
End Class
