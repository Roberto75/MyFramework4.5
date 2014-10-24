<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseDetail_1_2
    Inherits MyFormsLibrary.FormBaseDetail_1_1


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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseDetail_1_2))
        Me.btnNuovo = New MyControlsLibrary.MyButton()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnNuovo)
        '
        'btnNuovo
        '
        Me.btnNuovo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuovo.FlatAppearance.BorderSize = 0
        Me.btnNuovo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNuovo.Image = CType(resources.GetObject("btnNuovo.Image"), System.Drawing.Image)
        Me.btnNuovo.Location = New System.Drawing.Point(304, 1)
        Me.btnNuovo.MyType = MyControlsLibrary.MyButton.ButtonType.btnNew
        Me.btnNuovo.Name = "btnNuovo"
        Me.btnNuovo.Size = New System.Drawing.Size(156, 30)
        Me.btnNuovo.TabIndex = 1
        Me.btnNuovo.Text = "Nuovo ..."
        Me.btnNuovo.UseVisualStyleBackColor = True
        '
        'FormBaseDetail_1_2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(672, 587)
        Me.Name = "FormBaseDetail_1_2"
        Me.Text = "FormBaseDetail_1_2"
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnNuovo As MyControlsLibrary.MyButton
End Class
