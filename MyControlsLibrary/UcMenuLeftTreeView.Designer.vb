<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcMenuLeftTreeView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UcMenuLeftTreeView))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.treeViewLeft = New System.Windows.Forms.TreeView
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "4puntini.gif")
        Me.ImageList1.Images.SetKeyName(1, "3puntini.gif")
        Me.ImageList1.Images.SetKeyName(2, "zoom1_16x16.gif")
        Me.ImageList1.Images.SetKeyName(3, "yellow2_16x16.png")
        Me.ImageList1.Images.SetKeyName(4, "amber_16x16.png")
        Me.ImageList1.Images.SetKeyName(5, "green2_16x16.png")
        Me.ImageList1.Images.SetKeyName(6, "red2_16x16.png")
        Me.ImageList1.Images.SetKeyName(7, "blue2_16x16.png")
        '
        'treeViewLeft
        '
        Me.treeViewLeft.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.treeViewLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeViewLeft.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.treeViewLeft.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.treeViewLeft.FullRowSelect = True
        Me.treeViewLeft.HotTracking = True
        Me.treeViewLeft.ImageIndex = 0
        Me.treeViewLeft.ImageList = Me.ImageList1
        Me.treeViewLeft.ItemHeight = 22
        Me.treeViewLeft.Location = New System.Drawing.Point(0, 0)
        Me.treeViewLeft.Name = "treeViewLeft"
        Me.treeViewLeft.SelectedImageIndex = 0
        Me.treeViewLeft.ShowLines = False
        Me.treeViewLeft.ShowPlusMinus = False
        Me.treeViewLeft.Size = New System.Drawing.Size(152, 153)
        Me.treeViewLeft.TabIndex = 0
        '
        'UcMenuLeftTreeView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.treeViewLeft)
        Me.Name = "UcMenuLeftTreeView"
        Me.Size = New System.Drawing.Size(152, 153)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents treeViewLeft As System.Windows.Forms.TreeView

End Class
