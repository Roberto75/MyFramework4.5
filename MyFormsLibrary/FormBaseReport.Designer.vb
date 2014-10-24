<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseReport))
        Me.UcPreviewDataTable1 = New MyControlsLibrary.UcPreviewDataTable
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.creaCSV = New System.Windows.Forms.CheckBox
        Me.creaXML = New System.Windows.Forms.CheckBox
        Me.creaExel = New System.Windows.Forms.CheckBox
        Me.btnReport = New MyControlsLibrary.MyButton
        Me.btnReset = New MyControlsLibrary.MyButton
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
        Me.TabPage1.Size = New System.Drawing.Size(687, 197)
        Me.TabPage1.Text = "Preview"
        '
        'TabControlDetail
        '
        Me.TabControlDetail.Size = New System.Drawing.Size(695, 223)
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.Panel2)
        Me.panel1.Size = New System.Drawing.Size(695, 143)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Size = New System.Drawing.Size(695, 395)
        '
        'tabPageMain
        '
        Me.tabPageMain.Size = New System.Drawing.Size(701, 426)
        '
        'UcPreviewDataTable1
        '
        Me.UcPreviewDataTable1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcPreviewDataTable1.Location = New System.Drawing.Point(3, 3)
        Me.UcPreviewDataTable1.MyShowButtonDelete = False
        Me.UcPreviewDataTable1.MyShowButtonFilter = False
        Me.UcPreviewDataTable1.MyShowIconHeaderRow = False
        Me.UcPreviewDataTable1.Name = "UcPreviewDataTable1"
        Me.UcPreviewDataTable1.Size = New System.Drawing.Size(681, 191)
        Me.UcPreviewDataTable1.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.creaCSV)
        Me.Panel2.Controls.Add(Me.creaXML)
        Me.Panel2.Controls.Add(Me.creaExel)
        Me.Panel2.Controls.Add(Me.btnReport)
        Me.Panel2.Controls.Add(Me.btnReset)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 107)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(695, 36)
        Me.Panel2.TabIndex = 0
        '
        'creaCSV
        '
        Me.creaCSV.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.creaCSV.AutoSize = True
        Me.creaCSV.Location = New System.Drawing.Point(486, 11)
        Me.creaCSV.Name = "creaCSV"
        Me.creaCSV.Size = New System.Drawing.Size(47, 17)
        Me.creaCSV.TabIndex = 19
        Me.creaCSV.Text = "CSV"
        Me.creaCSV.UseVisualStyleBackColor = True
        '
        'creaXML
        '
        Me.creaXML.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.creaXML.AutoSize = True
        Me.creaXML.Location = New System.Drawing.Point(434, 11)
        Me.creaXML.Name = "creaXML"
        Me.creaXML.Size = New System.Drawing.Size(48, 17)
        Me.creaXML.TabIndex = 18
        Me.creaXML.Text = "XML"
        Me.creaXML.UseVisualStyleBackColor = True
        '
        'creaExel
        '
        Me.creaExel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.creaExel.AutoSize = True
        Me.creaExel.Location = New System.Drawing.Point(376, 11)
        Me.creaExel.Name = "creaExel"
        Me.creaExel.Size = New System.Drawing.Size(52, 17)
        Me.creaExel.TabIndex = 17
        Me.creaExel.Text = "Excel"
        Me.creaExel.UseVisualStyleBackColor = True
        '
        'btnReport
        '
        Me.btnReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReport.FlatAppearance.BorderSize = 0
        Me.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReport.Image = CType(resources.GetObject("btnReport.Image"), System.Drawing.Image)
        Me.btnReport.Location = New System.Drawing.Point(539, 3)
        Me.btnReport.MyType = MyControlsLibrary.MyButton.ButtonType.btnReport
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(156, 30)
        Me.btnReport.TabIndex = 1
        Me.btnReport.Text = "Report"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReset.FlatAppearance.BorderSize = 0
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Image = CType(resources.GetObject("btnReset.Image"), System.Drawing.Image)
        Me.btnReset.Location = New System.Drawing.Point(0, 3)
        Me.btnReset.MyType = MyControlsLibrary.MyButton.ButtonType.btnReset
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(156, 30)
        Me.btnReset.TabIndex = 0
        Me.btnReset.Text = "Azzera campi"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'FormBaseReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 452)
        Me.Name = "FormBaseReport"
        Me.Text = "FormBaseReport"
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
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnReport As MyControlsLibrary.MyButton
    Friend WithEvents btnReset As MyControlsLibrary.MyButton
    Friend WithEvents creaCSV As System.Windows.Forms.CheckBox
    Friend WithEvents creaXML As System.Windows.Forms.CheckBox
    Friend WithEvents creaExel As System.Windows.Forms.CheckBox
    Protected WithEvents UcPreviewDataTable1 As MyControlsLibrary.UcPreviewDataTable
End Class
