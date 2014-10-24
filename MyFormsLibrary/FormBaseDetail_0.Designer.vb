<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseDetail_0
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseDetail_0))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabPageMain = New System.Windows.Forms.TabPage()
        Me.MyToolstripTop = New System.Windows.Forms.ToolStrip()
        Me.MyBtnClose = New System.Windows.Forms.ToolStripButton()
        Me.btnTopDown = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MyButtonHelp = New System.Windows.Forms.ToolStripButton()
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.tabPageMain.SuspendLayout()
        Me.MyToolstripTop.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabPageMain)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(813, 417)
        Me.TabControl1.TabIndex = 0
        '
        'tabPageMain
        '
        Me.tabPageMain.Controls.Add(Me.MyToolstripTop)
        Me.tabPageMain.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMain.Name = "tabPageMain"
        Me.tabPageMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPageMain.Size = New System.Drawing.Size(805, 391)
        Me.tabPageMain.TabIndex = 0
        Me.tabPageMain.Text = "tabPageMain"
        Me.tabPageMain.UseVisualStyleBackColor = True
        '
        'MyToolstripTop
        '
        Me.MyToolstripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MyToolstripTop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MyBtnClose, Me.btnTopDown, Me.ToolStripSeparator1, Me.MyButtonHelp})
        Me.MyToolstripTop.Location = New System.Drawing.Point(3, 3)
        Me.MyToolstripTop.Name = "MyToolstripTop"
        Me.MyToolstripTop.Size = New System.Drawing.Size(799, 25)
        Me.MyToolstripTop.TabIndex = 1
        Me.MyToolstripTop.Text = "ToolStrip1"
        '
        'MyBtnClose
        '
        Me.MyBtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.MyBtnClose.AutoSize = False
        Me.MyBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.MyBtnClose.Image = CType(resources.GetObject("MyBtnClose.Image"), System.Drawing.Image)
        Me.MyBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MyBtnClose.Name = "MyBtnClose"
        Me.MyBtnClose.Size = New System.Drawing.Size(23, 22)
        Me.MyBtnClose.Text = "Close"
        '
        'btnTopDown
        '
        Me.btnTopDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnTopDown.Image = CType(resources.GetObject("btnTopDown.Image"), System.Drawing.Image)
        Me.btnTopDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnTopDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnTopDown.Name = "btnTopDown"
        Me.btnTopDown.Size = New System.Drawing.Size(23, 22)
        Me.btnTopDown.Visible = False
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'MyButtonHelp
        '
        Me.MyButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.MyButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.MyButtonHelp.Image = CType(resources.GetObject("MyButtonHelp.Image"), System.Drawing.Image)
        Me.MyButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MyButtonHelp.Name = "MyButtonHelp"
        Me.MyButtonHelp.Size = New System.Drawing.Size(23, 22)
        Me.MyButtonHelp.Text = "Help"
        Me.MyButtonHelp.Visible = False
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        'FormBaseDetail_0
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(813, 417)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "FormBaseDetail_0"
        Me.Text = "FormBaseDetail_0"
        Me.TabControl1.ResumeLayout(False)
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.MyToolstripTop.ResumeLayout(False)
        Me.MyToolstripTop.PerformLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Protected _ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents MyBtnClose As System.Windows.Forms.ToolStripButton
    Protected WithEvents tabPageMain As System.Windows.Forms.TabPage
    Friend WithEvents btnTopDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents MyButtonHelp As System.Windows.Forms.ToolStripButton
    Protected WithEvents MyToolstripTop As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
End Class
