Public Class FormSendEmail
    Private _isDebuggMode As Boolean = False
    'Private _subject As String

    Public Sub _clearAttach()
        Me._listBoxAttach.Items.Clear()
    End Sub

    Public Sub _addAttach(ByVal listOfAttach() As IO.FileInfo)
        If Not listOfAttach Is Nothing Then
            For Each attach As IO.FileInfo In listOfAttach
                Me._listBoxAttach.Items.Add(attach.FullName, True)
            Next
        End If
    End Sub

    Public Sub _setSubject(ByVal value As String)
        Me.txtSubject.Text = value
    End Sub


    Public Sub _setTo(ByVal value As String)
        Me.txtTo.Text = value
    End Sub



    Public Sub _setBodyMessage(ByVal value As String)
        Me.txtMessage.Text = value
    End Sub

    Overloads Function _init(ByVal isDebuggMode As Boolean) As Boolean
        Me._isDebuggMode = isDebuggMode

        Me.lblEnabled.Text = "Enabled: " & System.Configuration.ConfigurationManager.AppSettings("mail.isEnabled")
        Me.txtFrom.Text = System.Configuration.ConfigurationManager.AppSettings("mail.From")
        Me.txtLogin.Text = System.Configuration.ConfigurationManager.AppSettings("mail.server.userName")
        Me.txtPassword.Text = System.Configuration.ConfigurationManager.AppSettings("mail.server.password")
        Me.txtServer.Text = System.Configuration.ConfigurationManager.AppSettings("mail.server")
        Me.txtTo.Text = ""
        Me.txtCcn.Text = System.Configuration.ConfigurationManager.AppSettings("mail.To.Ccn")
        Me.txtFromDisplayName.Text = System.Configuration.ConfigurationManager.AppSettings("mail.From.displayName")

        If _isDebuggMode Then
            Me.GroupBox2.Visible = True
        Else
            'La apro in finestra popup
            Me.tabPageMain.Text = "Send email"
            Me.GroupBox2.Visible = False
            Me.SplitContainer1.SplitterDistance = 200
            Me.SplitContainer1.FixedPanel = FixedPanel.Panel1
            Me.Width = 600
            Me.Height = 600
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.MinimumSize = Me.Size
            Me.MaximumSize = Me.Size
        End If


        If Not Me.Owner Is Nothing Then
            Me.Icon = Owner.Icon
        End If

        'Button send email 
        Me._addButtonToToolStrip("Send email", "btnSendEmail", CType(MyControlsLibrary.My.Resources.MyResource.sendEmail_16x32, System.Drawing.Image), ToolStripItemAlignment.Right)


        Return MyBase._init(Nothing, Nothing, Nothing)
    End Function



    Public Overrides Function _toolStripButtonClicked(ByVal myButton As System.Windows.Forms.ToolStripButton) As Boolean
        Select Case myButton.Name
            Case "btnSendEmail"
                Me._ErrorProvider.SetError(txtTo, "")
                If String.IsNullOrEmpty(Me.txtTo.Text.Trim) Then
                    Me._ErrorProvider.SetError(txtTo, "Indirizzo obbligatorio")
                    Exit Function
                End If

                _buttonSendOnClick()
        End Select

        Return True
    End Function




    Public Overridable Function _buttonSendOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


    Private Sub MyButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyButton2.Click
        Dim OpenFileDialog1 As New Windows.Forms.OpenFileDialog
        'OpenFileDialog1.Filter = "Excel (*.xls;*.xlsx))|*.xls;*.xlsx"
        'OpenFileDialog1.InitialDirectory = Application.ExecutablePath
        OpenFileDialog1.Title = "Select attach"
        'OpenFileDialog1.FileName = "source.xls"

        If (OpenFileDialog1.ShowDialog(tabPageMain) = Windows.Forms.DialogResult.OK) Then
            Me._listBoxAttach.Items.Add(OpenFileDialog1.FileName, True)
        End If
    End Sub
End Class