<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcPeriodoCompetenza
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
        Me.components = New System.ComponentModel.Container()
        Me.GroupPeriodo = New System.Windows.Forms.GroupBox()
        Me.anno = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbMese = New System.Windows.Forms.ComboBox()
        Me.labelMese = New System.Windows.Forms.Label()
        Me.upDownNumGiorni = New System.Windows.Forms.NumericUpDown()
        Me.labelPeriodo = New System.Windows.Forms.Label()
        Me.labelGiorni = New System.Windows.Forms.Label()
        Me.dtA = New System.Windows.Forms.DateTimePicker()
        Me.dtDa = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupPeriodo.SuspendLayout()
        CType(Me.anno, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.upDownNumGiorni, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupPeriodo
        '
        Me.GroupPeriodo.Controls.Add(Me.anno)
        Me.GroupPeriodo.Controls.Add(Me.Label5)
        Me.GroupPeriodo.Controls.Add(Me.cmbMese)
        Me.GroupPeriodo.Controls.Add(Me.labelMese)
        Me.GroupPeriodo.Controls.Add(Me.upDownNumGiorni)
        Me.GroupPeriodo.Controls.Add(Me.labelPeriodo)
        Me.GroupPeriodo.Controls.Add(Me.labelGiorni)
        Me.GroupPeriodo.Controls.Add(Me.dtA)
        Me.GroupPeriodo.Controls.Add(Me.dtDa)
        Me.GroupPeriodo.Controls.Add(Me.Label2)
        Me.GroupPeriodo.Controls.Add(Me.Label1)
        Me.GroupPeriodo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupPeriodo.Location = New System.Drawing.Point(0, 0)
        Me.GroupPeriodo.Name = "GroupPeriodo"
        Me.GroupPeriodo.Size = New System.Drawing.Size(309, 166)
        Me.GroupPeriodo.TabIndex = 3
        Me.GroupPeriodo.TabStop = False
        Me.GroupPeriodo.Text = "Periodo di riferimento"
        '
        'anno
        '
        Me.anno.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.anno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.anno.Location = New System.Drawing.Point(223, 136)
        Me.anno.Maximum = New Decimal(New Integer() {3000, 0, 0, 0})
        Me.anno.Minimum = New Decimal(New Integer() {1900, 0, 0, 0})
        Me.anno.Name = "anno"
        Me.anno.Size = New System.Drawing.Size(77, 20)
        Me.anno.TabIndex = 16
        Me.anno.Value = New Decimal(New Integer() {1900, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(181, 138)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Anno"
        '
        'cmbMese
        '
        Me.cmbMese.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbMese.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMese.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbMese.FormattingEnabled = True
        Me.cmbMese.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbMese.Location = New System.Drawing.Point(101, 135)
        Me.cmbMese.Name = "cmbMese"
        Me.cmbMese.Size = New System.Drawing.Size(61, 21)
        Me.cmbMese.TabIndex = 7
        '
        'labelMese
        '
        Me.labelMese.AutoSize = True
        Me.labelMese.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelMese.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMese.Location = New System.Drawing.Point(6, 138)
        Me.labelMese.Name = "labelMese"
        Me.labelMese.Size = New System.Drawing.Size(33, 13)
        Me.labelMese.TabIndex = 6
        Me.labelMese.Text = "Mese"
        '
        'upDownNumGiorni
        '
        Me.upDownNumGiorni.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.upDownNumGiorni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.upDownNumGiorni.Location = New System.Drawing.Point(101, 90)
        Me.upDownNumGiorni.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.upDownNumGiorni.Name = "upDownNumGiorni"
        Me.upDownNumGiorni.Size = New System.Drawing.Size(61, 20)
        Me.upDownNumGiorni.TabIndex = 5
        Me.upDownNumGiorni.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'labelPeriodo
        '
        Me.labelPeriodo.AutoSize = True
        Me.labelPeriodo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelPeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPeriodo.Location = New System.Drawing.Point(6, 40)
        Me.labelPeriodo.Name = "labelPeriodo"
        Me.labelPeriodo.Size = New System.Drawing.Size(43, 13)
        Me.labelPeriodo.TabIndex = 4
        Me.labelPeriodo.Text = "Periodo"
        '
        'labelGiorni
        '
        Me.labelGiorni.AutoSize = True
        Me.labelGiorni.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelGiorni.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelGiorni.Location = New System.Drawing.Point(6, 92)
        Me.labelGiorni.Name = "labelGiorni"
        Me.labelGiorni.Size = New System.Drawing.Size(83, 13)
        Me.labelGiorni.TabIndex = 4
        Me.labelGiorni.Text = "Numero di giorni"
        '
        'dtA
        '
        Me.dtA.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtA.Checked = False
        Me.dtA.Location = New System.Drawing.Point(117, 50)
        Me.dtA.Name = "dtA"
        Me.dtA.ShowCheckBox = True
        Me.dtA.Size = New System.Drawing.Size(183, 20)
        Me.dtA.TabIndex = 3
        '
        'dtDa
        '
        Me.dtDa.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtDa.Checked = False
        Me.dtDa.Location = New System.Drawing.Point(117, 24)
        Me.dtDa.Name = "dtDa"
        Me.dtDa.ShowCheckBox = True
        Me.dtDa.Size = New System.Drawing.Size(183, 20)
        Me.dtDa.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(98, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(13, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "a"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(92, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(19, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "da"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'UcPeriodoCompetenza
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupPeriodo)
        Me.Name = "UcPeriodoCompetenza"
        Me.Size = New System.Drawing.Size(309, 166)
        Me.GroupPeriodo.ResumeLayout(False)
        Me.GroupPeriodo.PerformLayout()
        CType(Me.anno, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.upDownNumGiorni, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupPeriodo As System.Windows.Forms.GroupBox
    Friend WithEvents labelMese As System.Windows.Forms.Label
    Friend WithEvents upDownNumGiorni As System.Windows.Forms.NumericUpDown
    Friend WithEvents labelGiorni As System.Windows.Forms.Label
    Friend WithEvents dtA As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtDa As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbMese As System.Windows.Forms.ComboBox
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents labelPeriodo As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents anno As System.Windows.Forms.NumericUpDown

End Class
