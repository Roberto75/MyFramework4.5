<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcMenuLeftStack
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
    	Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UcMenuLeftStack))
    	Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
    	Me.SuspendLayout
    	'
    	'imageList1
    	'
    	Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
    	Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
    	Me.imageList1.Images.SetKeyName(0, "generic.ico")
    	'
    	'UcMenuLeftStack
    	'
    	Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    	Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    	Me.Name = "UcMenuLeftStack"
    	Me.Size = New System.Drawing.Size(151, 134)
    	Me.ResumeLayout(false)
    End Sub
    Private imageList1 As System.Windows.Forms.ImageList

End Class
