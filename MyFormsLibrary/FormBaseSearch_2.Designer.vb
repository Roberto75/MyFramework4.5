<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseSearch_2
    Inherits MyFormsLibrary.FormBaseSearch_0

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
        Me.UcPreviewDataTable1 = New MyControlsLibrary.UcPreviewDataTable()
        Me.panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.UcPreviewDataTable1)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        '
        'UcPreviewDataTable1
        '
        Me.UcPreviewDataTable1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcPreviewDataTable1.Location = New System.Drawing.Point(0, 0)
        Me.UcPreviewDataTable1.MyMultiSelect = False
        Me.UcPreviewDataTable1.MyShowButtonDelete = False
        Me.UcPreviewDataTable1.MyShowButtonFilter = True
        Me.UcPreviewDataTable1.MyShowIconHeaderRow = False
        Me.UcPreviewDataTable1.Name = "UcPreviewDataTable1"
        Me.UcPreviewDataTable1.Size = New System.Drawing.Size(706, 143)
        Me.UcPreviewDataTable1.TabIndex = 0
        '
        'FormBaseSearch_2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseSearch_2"
        Me.Text = "FormBaseSearch_2"
        Me.panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents UcPreviewDataTable1 As MyControlsLibrary.UcPreviewDataTable
End Class
