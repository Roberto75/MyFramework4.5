<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormClickOnce
    Inherits FormBaseDetail_3

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormClickOnce))
        Me.MyButton1 = New MyControlsLibrary.MyButton()
        Me.lblSourceFolder = New System.Windows.Forms.Label()
        Me.lblDeployVer = New System.Windows.Forms.Label()
        Me.txtDeployVersion = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TabPage1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TextBox1)
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.txtDeployVersion)
        Me.panel1.Controls.Add(Me.lblDeployVer)
        Me.panel1.Controls.Add(Me.lblSourceFolder)
        Me.panel1.Controls.Add(Me.MyButton1)
        '
        'SplitContainer1
        '
        '
        'MyButton1
        '
        Me.MyButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MyButton1.BackColor = System.Drawing.Color.Transparent
        Me.MyButton1.FlatAppearance.BorderSize = 0
        Me.MyButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.MyButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
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
        'lblSourceFolder
        '
        Me.lblSourceFolder.AutoSize = True
        Me.lblSourceFolder.Location = New System.Drawing.Point(350, 12)
        Me.lblSourceFolder.Name = "lblSourceFolder"
        Me.lblSourceFolder.Size = New System.Drawing.Size(80, 13)
        Me.lblSourceFolder.TabIndex = 1
        Me.lblSourceFolder.Text = "lblSourceFolder"
        '
        'lblDeployVer
        '
        Me.lblDeployVer.AutoSize = True
        Me.lblDeployVer.Location = New System.Drawing.Point(353, 45)
        Me.lblDeployVer.Name = "lblDeployVer"
        Me.lblDeployVer.Size = New System.Drawing.Size(66, 13)
        Me.lblDeployVer.TabIndex = 2
        Me.lblDeployVer.Text = "lblDeployVer"
        '
        'txtDeployVersion
        '
        Me.txtDeployVersion.Location = New System.Drawing.Point(474, 42)
        Me.txtDeployVersion.Name = "txtDeployVersion"
        Me.txtDeployVersion.Size = New System.Drawing.Size(97, 20)
        Me.txtDeployVersion.TabIndex = 16
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(3, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(692, 191)
        Me.TextBox1.TabIndex = 1
        '
        'FormClickOnce
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormClickOnce"
        Me.Text = "FormClickOnce"
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControlDetail.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MyButton1 As MyControlsLibrary.MyButton
    Friend WithEvents lblSourceFolder As System.Windows.Forms.Label
    Friend WithEvents lblDeployVer As System.Windows.Forms.Label
    Private WithEvents txtDeployVersion As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
