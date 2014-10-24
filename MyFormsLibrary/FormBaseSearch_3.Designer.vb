<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseSearch_3
    Inherits MyFormsLibrary.FormBaseDetail_3


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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseSearch_3))
        Me.UcPreviewDataTable1 = New MyControlsLibrary.UcPreviewDataTable()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnReset = New MyControlsLibrary.MyButton()
        Me.btnCerca = New MyControlsLibrary.MyButton()
        Me.TabPage1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UcPreviewDataTable1)
        Me.TabPage1.Size = New System.Drawing.Size(715, 197)
        Me.TabPage1.Text = "Ricerca"
        '
        'TabControlDetail
        '
        Me.TabControlDetail.Size = New System.Drawing.Size(723, 223)
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.Panel2)
        Me.panel1.Size = New System.Drawing.Size(723, 143)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Size = New System.Drawing.Size(723, 395)
        '
        'tabPageMain
        '
        Me.tabPageMain.Size = New System.Drawing.Size(729, 426)
        '
        'UcPreviewDataTable1
        '
        Me.UcPreviewDataTable1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcPreviewDataTable1.Location = New System.Drawing.Point(3, 3)
        Me.UcPreviewDataTable1.MyMultiSelect = False
        Me.UcPreviewDataTable1.MyShowButtonDelete = False
        Me.UcPreviewDataTable1.MyShowButtonFilter = False
        Me.UcPreviewDataTable1.MyShowIconHeaderRow = False
        Me.UcPreviewDataTable1.Name = "UcPreviewDataTable1"
        Me.UcPreviewDataTable1.Size = New System.Drawing.Size(709, 191)
        Me.UcPreviewDataTable1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnReset)
        Me.Panel2.Controls.Add(Me.btnCerca)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 106)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(723, 37)
        Me.Panel2.TabIndex = 0
        '
        'btnReset
        '
        Me.btnReset.BackColor = System.Drawing.Color.Transparent
        Me.btnReset.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnReset.FlatAppearance.BorderSize = 0
        Me.btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Image = CType(resources.GetObject("btnReset.Image"), System.Drawing.Image)
        Me.btnReset.Location = New System.Drawing.Point(0, 0)
        Me.btnReset.MyType = MyControlsLibrary.MyButton.ButtonType.btnReset
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(156, 37)
        Me.btnReset.TabIndex = 1
        Me.btnReset.Text = "Azzera campi"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnCerca
        '
        Me.btnCerca.BackColor = System.Drawing.Color.Transparent
        Me.btnCerca.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnCerca.FlatAppearance.BorderSize = 0
        Me.btnCerca.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnCerca.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnCerca.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCerca.Image = CType(resources.GetObject("btnCerca.Image"), System.Drawing.Image)
        Me.btnCerca.Location = New System.Drawing.Point(567, 0)
        Me.btnCerca.MyType = MyControlsLibrary.MyButton.ButtonType.btnFind
        Me.btnCerca.Name = "btnCerca"
        Me.btnCerca.Size = New System.Drawing.Size(156, 37)
        Me.btnCerca.TabIndex = 0
        Me.btnCerca.Text = "Cerca"
        Me.btnCerca.UseVisualStyleBackColor = True
        '
        'FormBaseSearch_3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(737, 452)
        Me.Name = "FormBaseSearch_3"
        Me.Text = "FormBaseSearch_3"
        Me.TabPage1.ResumeLayout(False)
        Me.TabControlDetail.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Protected WithEvents UcPreviewDataTable1 As MyControlsLibrary.UcPreviewDataTable
    Protected WithEvents Panel2 As System.Windows.Forms.Panel
    Protected WithEvents btnCerca As MyControlsLibrary.MyButton
    Protected WithEvents btnReset As MyControlsLibrary.MyButton
End Class
