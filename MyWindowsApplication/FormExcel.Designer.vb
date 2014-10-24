<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormExcel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormExcel))
        Me.MyButton1 = New MyControlsLibrary.MyButton
        Me.UcPreviewDataTable1 = New MyControlsLibrary.UcPreviewDataTable
        Me.chkExcel = New System.Windows.Forms.CheckBox
        Me.TabPage1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UcPreviewDataTable1)
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.chkExcel)
        Me.panel1.Controls.Add(Me.MyButton1)
        '
        'SplitContainer1
        '
        '
        'MyButton1
        '
        Me.MyButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MyButton1.FlatAppearance.BorderSize = 0
        Me.MyButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MyButton1.Image = CType(resources.GetObject("MyButton1.Image"), System.Drawing.Image)
        Me.MyButton1.Location = New System.Drawing.Point(545, 110)
        Me.MyButton1.MyType = MyControlsLibrary.MyButton.ButtonType.btnExecute
        Me.MyButton1.Name = "MyButton1"
        Me.MyButton1.Size = New System.Drawing.Size(156, 30)
        Me.MyButton1.TabIndex = 0
        Me.MyButton1.Text = "Esegui"
        Me.MyButton1.UseVisualStyleBackColor = True
        '
        'UcPreviewDataTable1
        '
        Me.UcPreviewDataTable1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcPreviewDataTable1.Location = New System.Drawing.Point(3, 3)
        Me.UcPreviewDataTable1.Name = "UcPreviewDataTable1"
        Me.UcPreviewDataTable1.Size = New System.Drawing.Size(692, 191)
        Me.UcPreviewDataTable1.TabIndex = 0
        '
        'chkExcel
        '
        Me.chkExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkExcel.AutoSize = True
        Me.chkExcel.Location = New System.Drawing.Point(440, 118)
        Me.chkExcel.Name = "chkExcel"
        Me.chkExcel.Size = New System.Drawing.Size(52, 17)
        Me.chkExcel.TabIndex = 1
        Me.chkExcel.Text = "Excel"
        Me.chkExcel.UseVisualStyleBackColor = True
        '
        'FormExcel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormExcel"
        Me.Text = "FormExcel"
        Me.TabPage1.ResumeLayout(False)
        Me.TabControlDetail.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UcPreviewDataTable1 As MyControlsLibrary.UcPreviewDataTable
    Friend WithEvents MyButton1 As MyControlsLibrary.MyButton
    Friend WithEvents chkExcel As System.Windows.Forms.CheckBox
End Class
