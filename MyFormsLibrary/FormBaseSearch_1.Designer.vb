<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseSearch_1
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
        Me.myDataGridView = New System.Windows.Forms.DataGridView
        Me.panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        CType(Me.myDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.myDataGridView)
        '
        'SplitContainer1
        '
        '
        'myDataGridView
        '
        Me.myDataGridView.AllowUserToAddRows = False
        Me.myDataGridView.AllowUserToDeleteRows = False
        Me.myDataGridView.AllowUserToResizeRows = False
        Me.myDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.myDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.myDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.myDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.myDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.myDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.myDataGridView.Name = "myDataGridView"
        Me.myDataGridView.ReadOnly = True
        Me.myDataGridView.Size = New System.Drawing.Size(706, 143)
        Me.myDataGridView.TabIndex = 2
        '
        'FormBaseSearch_1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseSearch_1"
        Me.Text = "FormBaseSearch_1"
        Me.panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        CType(Me.myDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Protected WithEvents myDataGridView As System.Windows.Forms.DataGridView
End Class
