<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
    Inherits MyFormsLibrary.FormMaster


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
        Me.UcMenuLeftTreeView1 = New MyControlsLibrary.UcMenuLeftTreeView
        Me.UcMenuLeftStack1 = New MyControlsLibrary.UcMenuLeftStack
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer2
        '
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.UcMenuLeftTreeView1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.UcMenuLeftStack1)
        '
        'SplitContainer1
        '
        '
        'UcMenuLeftTreeView1
        '
        Me.UcMenuLeftTreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcMenuLeftTreeView1.Location = New System.Drawing.Point(0, 0)
        Me.UcMenuLeftTreeView1.Name = "UcMenuLeftTreeView1"
        Me.UcMenuLeftTreeView1.Size = New System.Drawing.Size(230, 209)
        Me.UcMenuLeftTreeView1.TabIndex = 0
        '
        'UcMenuLeftStack1
        '
        Me.UcMenuLeftStack1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcMenuLeftStack1.Location = New System.Drawing.Point(0, 0)
        Me.UcMenuLeftStack1.Name = "UcMenuLeftStack1"
        Me.UcMenuLeftStack1.Size = New System.Drawing.Size(230, 212)
        Me.UcMenuLeftStack1.TabIndex = 1
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(827, 471)
        Me.MyLogEnabled = False
        Me.Name = "FormMain"
        Me.Text = "FormMain"
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcMenuLeftTreeView1 As MyControlsLibrary.UcMenuLeftTreeView
    Friend WithEvents UcMenuLeftStack1 As MyControlsLibrary.UcMenuLeftStack
End Class
