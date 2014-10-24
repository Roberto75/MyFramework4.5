<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseDetail_4
    Inherits MyFormsLibrary.FormBaseDetail_2

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseDetail_4))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSalvaDetail = New MyControlsLibrary.MyButton()
        Me.btnNuovoDetail = New MyControlsLibrary.MyButton()
        Me.TabControlDetail = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnReloadDetail = New System.Windows.Forms.ToolStripButton()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControlDetail)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel2)
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.FlowLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 184)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(706, 39)
        Me.Panel2.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSalvaDetail)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNuovoDetail)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(706, 39)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'btnSalvaDetail
        '
        Me.btnSalvaDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSalvaDetail.FlatAppearance.BorderSize = 0
        Me.btnSalvaDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalvaDetail.Image = CType(resources.GetObject("btnSalvaDetail.Image"), System.Drawing.Image)
        Me.btnSalvaDetail.Location = New System.Drawing.Point(547, 3)
        Me.btnSalvaDetail.MyType = MyControlsLibrary.MyButton.ButtonType.btnSave
        Me.btnSalvaDetail.Name = "btnSalvaDetail"
        Me.btnSalvaDetail.Size = New System.Drawing.Size(156, 30)
        Me.btnSalvaDetail.TabIndex = 0
        Me.btnSalvaDetail.Text = "Salva"
        Me.btnSalvaDetail.UseVisualStyleBackColor = True
        '
        'btnNuovoDetail
        '
        Me.btnNuovoDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNuovoDetail.FlatAppearance.BorderSize = 0
        Me.btnNuovoDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuovoDetail.Image = CType(resources.GetObject("btnNuovoDetail.Image"), System.Drawing.Image)
        Me.btnNuovoDetail.Location = New System.Drawing.Point(385, 3)
        Me.btnNuovoDetail.MyType = MyControlsLibrary.MyButton.ButtonType.btnNew
        Me.btnNuovoDetail.Name = "btnNuovoDetail"
        Me.btnNuovoDetail.Size = New System.Drawing.Size(156, 30)
        Me.btnNuovoDetail.TabIndex = 1
        Me.btnNuovoDetail.Text = "Nuovo ..."
        Me.btnNuovoDetail.UseVisualStyleBackColor = True
        '
        'TabControlDetail
        '
        Me.TabControlDetail.Controls.Add(Me.TabPage1)
        Me.TabControlDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlDetail.Location = New System.Drawing.Point(0, 0)
        Me.TabControlDetail.Name = "TabControlDetail"
        Me.TabControlDetail.SelectedIndex = 0
        Me.TabControlDetail.Size = New System.Drawing.Size(674, 184)
        Me.TabControlDetail.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(666, 158)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPageDetail"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Right
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnReloadDetail})
        Me.ToolStrip1.Location = New System.Drawing.Point(674, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.ShowItemToolTips = False
        Me.ToolStrip1.Size = New System.Drawing.Size(32, 184)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnReloadDetail
        '
        Me.btnReloadDetail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnReloadDetail.Image = Global.MyFormsLibrary.My.Resources.Resources.reload1_16x16
        Me.btnReloadDetail.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnReloadDetail.Name = "btnReloadDetail"
        Me.btnReloadDetail.Size = New System.Drawing.Size(29, 20)
        Me.btnReloadDetail.Text = "Aggiorna dettagli"
        '
        'FormBaseDetail_4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseDetail_4"
        Me.Text = "FormBaseDetail_4"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.TabControlDetail.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Protected Friend WithEvents TabControlDetail As System.Windows.Forms.TabControl
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnReloadDetail As System.Windows.Forms.ToolStripButton
    Protected WithEvents btnNuovoDetail As MyControlsLibrary.MyButton
    Protected WithEvents btnSalvaDetail As MyControlsLibrary.MyButton
    Protected WithEvents TabPage1 As System.Windows.Forms.TabPage
    Protected WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Protected WithEvents Panel2 As System.Windows.Forms.Panel
End Class
