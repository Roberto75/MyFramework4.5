<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseDetail_2
    Inherits FormBaseDetail_1

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseDetail_2))
        Me.MyToolStrip3 = New System.Windows.Forms.ToolStrip
        Me.btnCollapse = New System.Windows.Forms.ToolStripButton
        Me.btnUp = New System.Windows.Forms.ToolStripButton
        Me.panel1 = New System.Windows.Forms.Panel
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.MyToolStrip3.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MyToolStrip3)
        Me.SplitContainer1.Size = New System.Drawing.Size(706, 395)
        Me.SplitContainer1.SplitterDistance = 168
        '
        'tabPageMain
        '
        Me.tabPageMain.Size = New System.Drawing.Size(712, 426)
        '
        'MyToolStrip3
        '
        Me.MyToolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.MyToolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MyToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCollapse, Me.btnUp})
        Me.MyToolStrip3.Location = New System.Drawing.Point(0, 143)
        Me.MyToolStrip3.Name = "MyToolStrip3"
        Me.MyToolStrip3.Size = New System.Drawing.Size(706, 25)
        Me.MyToolStrip3.TabIndex = 0
        Me.MyToolStrip3.Text = "ToolStrip1"
        '
        'btnCollapse
        '
        Me.btnCollapse.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCollapse.Image = CType(resources.GetObject("btnCollapse.Image"), System.Drawing.Image)
        Me.btnCollapse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnCollapse.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCollapse.Name = "btnCollapse"
        Me.btnCollapse.Size = New System.Drawing.Size(23, 22)
        '
        'btnUp
        '
        Me.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(23, 22)
        '
        'panel1
        '
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel1.Location = New System.Drawing.Point(0, 0)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(706, 143)
        Me.panel1.TabIndex = 1
        '
        'FormBaseDetail_2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseDetail_2"
        Me.Text = "FormBaseDetail_2"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.MyToolStrip3.ResumeLayout(False)
        Me.MyToolStrip3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MyToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCollapse As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnUp As System.Windows.Forms.ToolStripButton
    Protected WithEvents panel1 As System.Windows.Forms.Panel
End Class
