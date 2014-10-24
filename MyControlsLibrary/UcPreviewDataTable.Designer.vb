<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcPreviewDataTable
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UcPreviewDataTable))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExportCVSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExportXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.lblItems = New System.Windows.Forms.ToolStripLabel
        Me.tsbtnSave = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbFilterEnable = New System.Windows.Forms.ToolStripButton
        Me.tsbFilterRemove = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsAutoSizeColums = New System.Windows.Forms.ToolStripLabel
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.Size = New System.Drawing.Size(508, 194)
        Me.DataGridView1.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportCVSToolStripMenuItem, Me.ExportXMLToolStripMenuItem, Me.DeleteSelectedToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'ExportCVSToolStripMenuItem
        '
        Me.ExportCVSToolStripMenuItem.Name = "ExportCVSToolStripMenuItem"
        Me.ExportCVSToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExportCVSToolStripMenuItem.Text = "Export CSV"
        '
        'ExportXMLToolStripMenuItem
        '
        Me.ExportXMLToolStripMenuItem.Name = "ExportXMLToolStripMenuItem"
        Me.ExportXMLToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExportXMLToolStripMenuItem.Text = "Export XML"
        '
        'DeleteSelectedToolStripMenuItem
        '
        Me.DeleteSelectedToolStripMenuItem.Name = "DeleteSelectedToolStripMenuItem"
        Me.DeleteSelectedToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.DeleteSelectedToolStripMenuItem.Text = "Delete selected"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ToolStrip1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 194)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(508, 26)
        Me.Panel1.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblItems, Me.tsbtnSave, Me.ToolStripSeparator1, Me.tsbFilterEnable, Me.tsbFilterRemove, Me.ToolStripSeparator2, Me.tsAutoSizeColums})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(508, 23)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'lblItems
        '
        Me.lblItems.Margin = New System.Windows.Forms.Padding(0, 4, 0, 2)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(80, 15)
        Me.lblItems.Text = "Found item: 0"
        '
        'tsbtnSave
        '
        Me.tsbtnSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnSave.Image = CType(resources.GetObject("tsbtnSave.Image"), System.Drawing.Image)
        Me.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSave.Name = "tsbtnSave"
        Me.tsbtnSave.Size = New System.Drawing.Size(23, 20)
        Me.tsbtnSave.Text = "Export"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 23)
        '
        'tsbFilterEnable
        '
        Me.tsbFilterEnable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFilterEnable.Image = Global.MyControlsLibrary.My.Resources.MyResource.filtro_20x20
        Me.tsbFilterEnable.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFilterEnable.Name = "tsbFilterEnable"
        Me.tsbFilterEnable.Size = New System.Drawing.Size(23, 20)
        Me.tsbFilterEnable.Text = "Enable auto filter column"
        '
        'tsbFilterRemove
        '
        Me.tsbFilterRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFilterRemove.Image = Global.MyControlsLibrary.My.Resources.MyResource.filtroRemove_20x20
        Me.tsbFilterRemove.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFilterRemove.Name = "tsbFilterRemove"
        Me.tsbFilterRemove.Size = New System.Drawing.Size(23, 20)
        Me.tsbFilterRemove.Text = "Remove all filter column"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 23)
        '
        'tsAutoSizeColums
        '
        Me.tsAutoSizeColums.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.tsAutoSizeColums.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.tsAutoSizeColums.ForeColor = System.Drawing.Color.DarkOrange
        Me.tsAutoSizeColums.Margin = New System.Windows.Forms.Padding(0, 4, 0, 2)
        Me.tsAutoSizeColums.Name = "tsAutoSizeColums"
        Me.tsAutoSizeColums.Size = New System.Drawing.Size(56, 13)
        Me.tsAutoSizeColums.Text = "Auto-Size"
        '
        'UcPreviewDataTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UcPreviewDataTable"
        Me.Size = New System.Drawing.Size(508, 220)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExportCVSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportXMLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog


    Sub ExportCVSToolStripMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents lblItems As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbtnSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbFilterEnable As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFilterRemove As System.Windows.Forms.ToolStripButton
    Friend WithEvents DeleteSelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsAutoSizeColums As System.Windows.Forms.ToolStripLabel
End Class
