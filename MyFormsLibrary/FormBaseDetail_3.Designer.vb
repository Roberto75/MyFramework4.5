<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseDetail_3
    Inherits FormBaseDetail_2

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
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControlDetail = New System.Windows.Forms.TabControl()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.AutoScroll = True
        '
        'SplitContainer1
        '
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControlDetail)
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(698, 197)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPageDetail"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabControlDetail
        '
        Me.TabControlDetail.Controls.Add(Me.TabPage1)
        Me.TabControlDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlDetail.Location = New System.Drawing.Point(0, 0)
        Me.TabControlDetail.Name = "TabControlDetail"
        Me.TabControlDetail.SelectedIndex = 0
        Me.TabControlDetail.Size = New System.Drawing.Size(706, 223)
        Me.TabControlDetail.TabIndex = 0
        '
        'FormBaseDetail_3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseDetail_3"
        Me.Text = "FormBaseDetail_3"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.TabControlDetail.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Protected Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Protected Friend WithEvents TabControlDetail As System.Windows.Forms.TabControl
End Class
