<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMessageBox
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMessageBox))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lblTitolo = New System.Windows.Forms.Label
        Me.cmdChiudi = New MyControlsLibrary.MyButton
        Me.picTipo = New System.Windows.Forms.PictureBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblInfoAdd = New System.Windows.Forms.Label
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.picTipo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblTitolo)
        Me.SplitContainer1.Panel1.Controls.Add(Me.picTipo)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmdChiudi)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblInfoAdd)
        Me.SplitContainer1.Size = New System.Drawing.Size(334, 261)
        Me.SplitContainer1.SplitterDistance = 105
        Me.SplitContainer1.TabIndex = 0
        '
        'lblTitolo
        '
        Me.lblTitolo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblTitolo.Location = New System.Drawing.Point(89, 12)
        Me.lblTitolo.Name = "lblTitolo"
        Me.lblTitolo.Size = New System.Drawing.Size(233, 52)
        Me.lblTitolo.TabIndex = 0
        Me.lblTitolo.Text = "Operazione Conclusa con Successo"
        Me.lblTitolo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdChiudi
        '
        Me.cmdChiudi.FlatAppearance.BorderSize = 0
        Me.cmdChiudi.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdChiudi.Image = CType(resources.GetObject("cmdChiudi.Image"), System.Drawing.Image)
        Me.cmdChiudi.Location = New System.Drawing.Point(196, 67)
        Me.cmdChiudi.MySize = MyControlsLibrary.MyButton.ButtonSize.px122
        Me.cmdChiudi.MyType = MyControlsLibrary.MyButton.ButtonType.btnNext
        Me.cmdChiudi.Name = "cmdChiudi"
        Me.cmdChiudi.Size = New System.Drawing.Size(130, 30)
        Me.cmdChiudi.TabIndex = 1
        Me.cmdChiudi.Text = "Chiudi"
        Me.cmdChiudi.UseVisualStyleBackColor = True
        '
        'picTipo
        '
        Me.picTipo.Image = CType(resources.GetObject("picTipo.Image"), System.Drawing.Image)
        Me.picTipo.Location = New System.Drawing.Point(5, 12)
        Me.picTipo.Name = "picTipo"
        Me.picTipo.Size = New System.Drawing.Size(80, 75)
        Me.picTipo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picTipo.TabIndex = 2
        Me.picTipo.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Elimina")
        Me.ImageList1.Images.SetKeyName(1, "Info")
        Me.ImageList1.Images.SetKeyName(2, "Warning")
        Me.ImageList1.Images.SetKeyName(3, "Error")
        Me.ImageList1.Images.SetKeyName(4, "Faq")
        Me.ImageList1.Images.SetKeyName(5, "OK")
        Me.ImageList1.Images.SetKeyName(6, "Warnings")
        '
        'lblInfoAdd
        '
        Me.lblInfoAdd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfoAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblInfoAdd.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblInfoAdd.Location = New System.Drawing.Point(0, 0)
        Me.lblInfoAdd.Name = "lblInfoAdd"
        Me.lblInfoAdd.Padding = New System.Windows.Forms.Padding(4)
        Me.lblInfoAdd.Size = New System.Drawing.Size(334, 152)
        Me.lblInfoAdd.TabIndex = 0
        Me.lblInfoAdd.Text = "informazioni aggiuntive....."
        '
        'FormMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 261)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormMessageBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Messaggio Informativo"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.picTipo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdChiudi As MyControlsLibrary.MyButton
    Friend WithEvents lblTitolo As System.Windows.Forms.Label
    Friend WithEvents picTipo As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lblInfoAdd As System.Windows.Forms.Label
End Class
