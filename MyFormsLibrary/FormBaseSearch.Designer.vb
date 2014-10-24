<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseSearch
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseSearch))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabPageMain = New System.Windows.Forms.TabPage
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.myDataGridView = New System.Windows.Forms.DataGridView
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.btnCollapse = New System.Windows.Forms.ToolStripButton
        Me.tsbtnCountItems = New System.Windows.Forms.ToolStripLabel
        Me.tabControlSearchFilter = New System.Windows.Forms.TabControl
        Me.TabPageRicercaBase = New System.Windows.Forms.TabPage
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnAdd = New MyControlsLibrary.MyButton
        Me.btnReset = New MyControlsLibrary.MyButton
        Me.btnSearch = New MyControlsLibrary.MyButton
        Me.MyToolstripTop = New System.Windows.Forms.ToolStrip
        Me.MyBtnClose = New System.Windows.Forms.ToolStripButton
        Me.tsbtnPrint = New System.Windows.Forms.ToolStripButton
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.tabPageMain.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.myDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip2.SuspendLayout()
        Me.tabControlSearchFilter.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.MyToolstripTop.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabPageMain)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(661, 495)
        Me.TabControl1.TabIndex = 0
        '
        'tabPageMain
        '
        Me.tabPageMain.Controls.Add(Me.SplitContainer1)
        Me.tabPageMain.Controls.Add(Me.MyToolstripTop)
        Me.tabPageMain.Location = New System.Drawing.Point(4, 22)
        Me.tabPageMain.Name = "tabPageMain"
        Me.tabPageMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPageMain.Size = New System.Drawing.Size(653, 469)
        Me.tabPageMain.TabIndex = 0
        Me.tabPageMain.Text = "TabPage1"
        Me.tabPageMain.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 28)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.myDataGridView)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabControlSearchFilter)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(647, 438)
        Me.SplitContainer1.SplitterDistance = 230
        Me.SplitContainer1.TabIndex = 2
        '
        'myDataGridView
        '
        Me.myDataGridView.AllowUserToAddRows = False
        Me.myDataGridView.AllowUserToDeleteRows = False
        Me.myDataGridView.AllowUserToResizeRows = False
        Me.myDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.myDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.myDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.myDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.myDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.myDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.myDataGridView.Name = "myDataGridView"
        Me.myDataGridView.ReadOnly = True
        Me.myDataGridView.Size = New System.Drawing.Size(647, 205)
        Me.myDataGridView.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCollapse, Me.tsbtnCountItems})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 205)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(647, 25)
        Me.ToolStrip2.TabIndex = 2
        Me.ToolStrip2.Text = "ToolStrip2"
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
        Me.btnCollapse.Text = "Nascondi i filtri di ricerca"
        '
        'tsbtnCountItems
        '
        Me.tsbtnCountItems.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnCountItems.Name = "tsbtnCountItems"
        Me.tsbtnCountItems.Size = New System.Drawing.Size(0, 22)
        '
        'tabControlSearchFilter
        '
        Me.tabControlSearchFilter.Controls.Add(Me.TabPageRicercaBase)
        Me.tabControlSearchFilter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlSearchFilter.Location = New System.Drawing.Point(0, 0)
        Me.tabControlSearchFilter.Name = "tabControlSearchFilter"
        Me.tabControlSearchFilter.SelectedIndex = 0
        Me.tabControlSearchFilter.Size = New System.Drawing.Size(647, 165)
        Me.tabControlSearchFilter.TabIndex = 1
        '
        'TabPageRicercaBase
        '
        Me.TabPageRicercaBase.Location = New System.Drawing.Point(4, 22)
        Me.TabPageRicercaBase.Name = "TabPageRicercaBase"
        Me.TabPageRicercaBase.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageRicercaBase.Size = New System.Drawing.Size(639, 139)
        Me.TabPageRicercaBase.TabIndex = 0
        Me.TabPageRicercaBase.Text = "Ricerca"
        Me.TabPageRicercaBase.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnAdd)
        Me.Panel1.Controls.Add(Me.btnReset)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 165)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(647, 39)
        Me.Panel1.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(486, 7)
        Me.btnAdd.MyType = MyControlsLibrary.MyButton.ButtonType.btnNew
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(156, 30)
        Me.btnAdd.TabIndex = 4
        Me.btnAdd.Text = "Nuovo ..."
        Me.btnAdd.UseVisualStyleBackColor = True
        Me.btnAdd.Visible = False
        '
        'btnReset
        '
        Me.btnReset.FlatAppearance.BorderSize = 0
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Image = CType(resources.GetObject("btnReset.Image"), System.Drawing.Image)
        Me.btnReset.Location = New System.Drawing.Point(5, 7)
        Me.btnReset.MyType = MyControlsLibrary.MyButton.ButtonType.btnReset
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(156, 30)
        Me.btnReset.TabIndex = 3
        Me.btnReset.Text = "Azzera campi"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.FlatAppearance.BorderSize = 0
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(201, 7)
        Me.btnSearch.MyType = MyControlsLibrary.MyButton.ButtonType.btnFind
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(156, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Cerca"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'MyToolstripTop
        '
        Me.MyToolstripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MyToolstripTop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MyBtnClose, Me.tsbtnPrint})
        Me.MyToolstripTop.Location = New System.Drawing.Point(3, 3)
        Me.MyToolstripTop.Name = "MyToolstripTop"
        Me.MyToolstripTop.Size = New System.Drawing.Size(647, 25)
        Me.MyToolstripTop.TabIndex = 0
        Me.MyToolstripTop.Text = "ToolStrip1"
        '
        'MyBtnClose
        '
        Me.MyBtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.MyBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.MyBtnClose.Image = CType(resources.GetObject("MyBtnClose.Image"), System.Drawing.Image)
        Me.MyBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MyBtnClose.Name = "MyBtnClose"
        Me.MyBtnClose.Size = New System.Drawing.Size(23, 22)
        Me.MyBtnClose.Text = "Chiudi scheda"
        '
        'tsbtnPrint
        '
        Me.tsbtnPrint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnPrint.Image = CType(resources.GetObject("tsbtnPrint.Image"), System.Drawing.Image)
        Me.tsbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnPrint.Name = "tsbtnPrint"
        Me.tsbtnPrint.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnPrint.Text = "Stampa"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'FormBaseSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 495)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "FormBaseSearch"
        Me.Text = "FormBaseSearch"
        Me.TabControl1.ResumeLayout(False)
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.myDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.tabControlSearchFilter.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.MyToolstripTop.ResumeLayout(False)
        Me.MyToolstripTop.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPageMain As System.Windows.Forms.TabPage
    Friend WithEvents MyToolstripTop As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbtnPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnCollapse As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbtnCountItems As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tabControlSearchFilter As System.Windows.Forms.TabControl
    Protected WithEvents TabPageRicercaBase As System.Windows.Forms.TabPage
    Protected WithEvents myDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnReset As MyControlsLibrary.MyButton
    Friend WithEvents btnSearch As MyControlsLibrary.MyButton
    Friend WithEvents MyBtnClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAdd As MyControlsLibrary.MyButton
    Protected WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
