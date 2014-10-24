<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcDataGridView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UcDataGridView))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblItems = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblItems)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 150)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(403, 23)
        Me.Panel1.TabIndex = 2
        '
        'lblItems
        '
        Me.lblItems.AutoSize = True
        Me.lblItems.Location = New System.Drawing.Point(3, 7)
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(44, 13)
        Me.lblItems.TabIndex = 2
        Me.lblItems.Text = "Items: 0"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(403, 150)
        Me.DataGridView1.TabIndex = 3
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "tsbtnCancel.png")
        Me.ImageList1.Images.SetKeyName(1, "blocco.png")
        Me.ImageList1.Images.SetKeyName(2, "dett.png")
        Me.ImageList1.Images.SetKeyName(3, "pbVuota.png")
        Me.ImageList1.Images.SetKeyName(4, "excel.png")
        Me.ImageList1.Images.SetKeyName(5, "Anonimo.png")
        Me.ImageList1.Images.SetKeyName(6, "execbtn.png")
        Me.ImageList1.Images.SetKeyName(7, "meno.png")
        Me.ImageList1.Images.SetKeyName(8, "piu.png")
        Me.ImageList1.Images.SetKeyName(9, "path3420.png")
        Me.ImageList1.Images.SetKeyName(10, "pbNew.png")
        Me.ImageList1.Images.SetKeyName(11, "tsbtnOk.png")
        Me.ImageList1.Images.SetKeyName(12, "user.png")
        '
        'UcDataGridView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UcDataGridView"
        Me.Size = New System.Drawing.Size(403, 173)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblItems As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList

End Class
