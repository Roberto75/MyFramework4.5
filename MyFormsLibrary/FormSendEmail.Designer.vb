<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSendEmail
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSendEmail))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCcn = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTo = New System.Windows.Forms.TextBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.txtFromDisplayName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFrom = New System.Windows.Forms.TextBox()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtLogin = New System.Windows.Forms.TextBox()
        Me.txtServer = New System.Windows.Forms.TextBox()
        Me.lblEnabled = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.lblLogin = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me._listBoxAttach = New System.Windows.Forms.CheckedListBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.MyButton2 = New MyControlsLibrary.MyButton()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.TabPage1.SuspendLayout()
        Me.TabControlDetail.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtMessage)
        Me.TabPage1.Size = New System.Drawing.Size(707, 89)
        Me.TabPage1.Text = "Message"
        '
        'TabControlDetail
        '
        Me.TabControlDetail.Controls.Add(Me.TabPage2)
        Me.TabControlDetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControlDetail.Size = New System.Drawing.Size(715, 115)
        Me.TabControlDetail.Controls.SetChildIndex(Me.TabPage2, 0)
        Me.TabControlDetail.Controls.SetChildIndex(Me.TabPage1, 0)
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.GroupBox2)
        Me.panel1.Controls.Add(Me.GroupBox1)
        Me.panel1.Size = New System.Drawing.Size(715, 312)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Size = New System.Drawing.Size(715, 456)
        Me.SplitContainer1.SplitterDistance = 337
        '
        'tabPageMain
        '
        Me.tabPageMain.Size = New System.Drawing.Size(721, 487)
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtSubject)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtCcn)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtTo)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(715, 117)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Email"
        '
        'txtSubject
        '
        Me.txtSubject.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubject.Location = New System.Drawing.Point(71, 17)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(612, 20)
        Me.txtSubject.TabIndex = 15
        Me.txtSubject.Text = "Test Send Mail"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Subject"
        '
        'txtCcn
        '
        Me.txtCcn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCcn.Location = New System.Drawing.Point(71, 76)
        Me.txtCcn.Name = "txtCcn"
        Me.txtCcn.Size = New System.Drawing.Size(614, 20)
        Me.txtCcn.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 76)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "ToCcn"
        '
        'txtTo
        '
        Me.txtTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTo.Location = New System.Drawing.Point(71, 46)
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(614, 20)
        Me.txtTo.TabIndex = 8
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(8, 49)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 1
        Me.lblTo.Text = "To"
        '
        'txtFromDisplayName
        '
        Me.txtFromDisplayName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFromDisplayName.Location = New System.Drawing.Point(115, 151)
        Me.txtFromDisplayName.Name = "txtFromDisplayName"
        Me.txtFromDisplayName.Size = New System.Drawing.Size(583, 20)
        Me.txtFromDisplayName.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 154)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "From Dislpay Name"
        '
        'txtFrom
        '
        Me.txtFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFrom.Location = New System.Drawing.Point(115, 123)
        Me.txtFrom.Name = "txtFrom"
        Me.txtFrom.Size = New System.Drawing.Size(583, 20)
        Me.txtFrom.TabIndex = 7
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(11, 125)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(30, 13)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtPassword)
        Me.GroupBox2.Controls.Add(Me.txtLogin)
        Me.GroupBox2.Controls.Add(Me.txtFromDisplayName)
        Me.GroupBox2.Controls.Add(Me.txtServer)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.lblEnabled)
        Me.GroupBox2.Controls.Add(Me.lblPassword)
        Me.GroupBox2.Controls.Add(Me.lblLogin)
        Me.GroupBox2.Controls.Add(Me.lblServer)
        Me.GroupBox2.Controls.Add(Me.txtFrom)
        Me.GroupBox2.Controls.Add(Me.lblFrom)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 117)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(715, 183)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Server SMTP"
        '
        'txtPassword
        '
        Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.Location = New System.Drawing.Point(115, 97)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(583, 20)
        Me.txtPassword.TabIndex = 6
        '
        'txtLogin
        '
        Me.txtLogin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLogin.Location = New System.Drawing.Point(115, 69)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(583, 20)
        Me.txtLogin.TabIndex = 5
        '
        'txtServer
        '
        Me.txtServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServer.Location = New System.Drawing.Point(115, 43)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(583, 20)
        Me.txtServer.TabIndex = 4
        '
        'lblEnabled
        '
        Me.lblEnabled.AutoSize = True
        Me.lblEnabled.Location = New System.Drawing.Point(11, 20)
        Me.lblEnabled.Name = "lblEnabled"
        Me.lblEnabled.Size = New System.Drawing.Size(46, 13)
        Me.lblEnabled.TabIndex = 3
        Me.lblEnabled.Text = "Enabled"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(11, 100)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(53, 13)
        Me.lblPassword.TabIndex = 2
        Me.lblPassword.Text = "Password"
        '
        'lblLogin
        '
        Me.lblLogin.AutoSize = True
        Me.lblLogin.Location = New System.Drawing.Point(11, 72)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.Size = New System.Drawing.Size(33, 13)
        Me.lblLogin.TabIndex = 1
        Me.lblLogin.Text = "Login"
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.Location = New System.Drawing.Point(11, 46)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(38, 13)
        Me.lblServer.TabIndex = 0
        Me.lblServer.Text = "Server"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me._listBoxAttach)
        Me.TabPage2.Controls.Add(Me.Panel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(698, 197)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Attach"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        '_listBoxAttach
        '
        Me._listBoxAttach.Dock = System.Windows.Forms.DockStyle.Fill
        Me._listBoxAttach.FormattingEnabled = True
        Me._listBoxAttach.Location = New System.Drawing.Point(0, 0)
        Me._listBoxAttach.Name = "_listBoxAttach"
        Me._listBoxAttach.Size = New System.Drawing.Size(707, 49)
        Me._listBoxAttach.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.MyButton2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 49)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(707, 40)
        Me.Panel2.TabIndex = 1
        '
        'MyButton2
        '
        Me.MyButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MyButton2.BackColor = System.Drawing.Color.Transparent
        Me.MyButton2.FlatAppearance.BorderSize = 0
        Me.MyButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.MyButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.MyButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MyButton2.Image = CType(resources.GetObject("MyButton2.Image"), System.Drawing.Image)
        Me.MyButton2.Location = New System.Drawing.Point(538, 7)
        Me.MyButton2.MyType = MyControlsLibrary.MyButton.ButtonType.btnOpen
        Me.MyButton2.Name = "MyButton2"
        Me.MyButton2.Size = New System.Drawing.Size(156, 30)
        Me.MyButton2.TabIndex = 0
        Me.MyButton2.Text = "Aggiungi allegati"
        Me.MyButton2.UseVisualStyleBackColor = True
        '
        'txtMessage
        '
        Me.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMessage.Location = New System.Drawing.Point(3, 3)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(701, 83)
        Me.txtMessage.TabIndex = 0
        Me.txtMessage.Text = "Test message"
        '
        'FormSendEmail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(729, 513)
        Me.Name = "FormSendEmail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "FormTestEmail"
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControlDetail.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblEnabled As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents lblLogin As System.Windows.Forms.Label
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Protected WithEvents txtMessage As System.Windows.Forms.TextBox
    Protected WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Protected WithEvents txtFrom As System.Windows.Forms.TextBox
    Protected WithEvents txtPassword As System.Windows.Forms.TextBox
    Protected WithEvents txtLogin As System.Windows.Forms.TextBox
    Protected WithEvents txtServer As System.Windows.Forms.TextBox
    Protected WithEvents txtTo As System.Windows.Forms.TextBox
    Protected WithEvents txtCcn As System.Windows.Forms.TextBox
    Protected WithEvents txtFromDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents MyButton2 As MyControlsLibrary.MyButton
    Protected WithEvents _listBoxAttach As System.Windows.Forms.CheckedListBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
End Class
