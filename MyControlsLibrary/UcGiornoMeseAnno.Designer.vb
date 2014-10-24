<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcGiornoMeseAnno
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
        Me.mese = New System.Windows.Forms.ComboBox
        Me.labelMese = New System.Windows.Forms.Label
        Me.lblAnno = New System.Windows.Forms.Label
        Me.anno = New System.Windows.Forms.NumericUpDown
        Me.giorno = New System.Windows.Forms.DateTimePicker
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblGiorno = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.anno, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mese
        '
        Me.mese.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mese.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mese.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mese.FormattingEnabled = True
        Me.mese.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.mese.Location = New System.Drawing.Point(70, 44)
        Me.mese.Name = "mese"
        Me.mese.Size = New System.Drawing.Size(61, 21)
        Me.mese.TabIndex = 11
        '
        'labelMese
        '
        Me.labelMese.AutoSize = True
        Me.labelMese.Cursor = System.Windows.Forms.Cursors.Default
        Me.labelMese.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMese.Location = New System.Drawing.Point(6, 47)
        Me.labelMese.Name = "labelMese"
        Me.labelMese.Size = New System.Drawing.Size(33, 13)
        Me.labelMese.TabIndex = 10
        Me.labelMese.Text = "Mese"
        '
        'lblAnno
        '
        Me.lblAnno.AutoSize = True
        Me.lblAnno.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAnno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAnno.Location = New System.Drawing.Point(6, 73)
        Me.lblAnno.Name = "lblAnno"
        Me.lblAnno.Size = New System.Drawing.Size(32, 13)
        Me.lblAnno.TabIndex = 14
        Me.lblAnno.Text = "Anno"
        '
        'anno
        '
        Me.anno.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.anno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.anno.Location = New System.Drawing.Point(70, 71)
        Me.anno.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
        Me.anno.Minimum = New Decimal(New Integer() {1900, 0, 0, 0})
        Me.anno.Name = "anno"
        Me.anno.Size = New System.Drawing.Size(88, 20)
        Me.anno.TabIndex = 15
        Me.anno.Value = New Decimal(New Integer() {1900, 0, 0, 0})
        '
        'giorno
        '
        Me.giorno.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.giorno.Checked = False
        Me.giorno.Location = New System.Drawing.Point(70, 19)
        Me.giorno.Name = "giorno"
        Me.giorno.ShowCheckBox = True
        Me.giorno.Size = New System.Drawing.Size(183, 20)
        Me.giorno.TabIndex = 17
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblGiorno)
        Me.GroupBox1.Controls.Add(Me.labelMese)
        Me.GroupBox1.Controls.Add(Me.giorno)
        Me.GroupBox1.Controls.Add(Me.anno)
        Me.GroupBox1.Controls.Add(Me.mese)
        Me.GroupBox1.Controls.Add(Me.lblAnno)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(272, 105)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Data"
        '
        'lblGiorno
        '
        Me.lblGiorno.AutoSize = True
        Me.lblGiorno.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGiorno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGiorno.Location = New System.Drawing.Point(6, 25)
        Me.lblGiorno.Name = "lblGiorno"
        Me.lblGiorno.Size = New System.Drawing.Size(38, 13)
        Me.lblGiorno.TabIndex = 18
        Me.lblGiorno.Text = "Giorno"
        '
        'UcGiornoMeseAnno
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "UcGiornoMeseAnno"
        Me.Size = New System.Drawing.Size(272, 105)
        CType(Me.anno, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mese As System.Windows.Forms.ComboBox
    Friend WithEvents labelMese As System.Windows.Forms.Label
    Friend WithEvents lblAnno As System.Windows.Forms.Label
    Friend WithEvents anno As System.Windows.Forms.NumericUpDown
    Friend WithEvents giorno As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblGiorno As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
