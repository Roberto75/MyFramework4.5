Public Class FormMaster

    Public Event MyMenuTopSubItemOnClick(ByVal item As System.Windows.Forms.ToolStripMenuItem)

    Private _connection As Data.Common.DbConnection
    '*** LOGGING ***
    Private _pathFileLogMain As String
    Private _pathFileLogFileProcessed As String
    Private _patternFileLogName As String
    Public _traceListenerGeneral As System.Diagnostics.TextWriterTraceListener


    '*** FOLDER *** 
    'in questa cartella sono memorizzate tutte le informazioni che NON devono esssere sovrascitte in caso di aggiornamento
    'dell'applicazione
    Public _folderUserData As String
    Public _folderTemp As String


    'Cross-Thread
    Private Delegate Sub delegateCursorDefault()
    Private Delegate Sub delegateCursorWait()
    Private Delegate Sub delegateProgressBarPerformStep()
    Private Delegate Sub delegateProgressBarSetValue(ByVal value As Integer)
    Private Delegate Sub delegateProgressBarSetMaximumValue(ByVal value As Integer)
    Private Delegate Sub delegateProgressBarChangeStyle()
    Private Delegate Sub delegateStatusBarUpdate(ByVal value As String)
    Private Delegate Sub delegateStatusBarUpdateStep()
    ' Private Delegate Function delegateGetMyOwner() As Windows.Forms.Form

    Private _logEnabled As Boolean
    Public Property MyLogEnabled() As Boolean
        Get
            Return _logEnabled
        End Get
        Set(ByVal value As Boolean)
            _logEnabled = value
            If _logEnabled AndAlso Me.DesignMode = False Then
                '*** LOGGING
                Trace.Listeners.Add(New TextWriterTraceListener(Console.Out))
                'Log su file
                If Not System.IO.Directory.Exists(Application.StartupPath & "\logs\") Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\logs\")
                End If

                _patternFileLogName = Format(Now, "yyyy_MM_dd-HH_mm_ss")
                _pathFileLogMain = Application.StartupPath & "\logs\" & _patternFileLogName & ".log"

                _traceListenerGeneral = New TextWriterTraceListener(System.IO.File.Create(_pathFileLogMain), "traceListener")
                Trace.Listeners.Add(_traceListenerGeneral)
            End If
        End Set
    End Property



    Public Property MyConnection() As Data.Common.DbConnection
        Get
            If Me._connection Is Nothing Then
                Return Nothing
            End If


            If Me._connection.State = ConnectionState.Closed Then
                Try
                    _connection.Open()
                Catch ex As Exception
                    Windows.Forms.MessageBox.Show("Impossibile stabilire una connessione con il database. Verificare la stringa di connessione e la presenza dei Driver." & vbCrLf & ex.Message, _
                        My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
                    Return Nothing
                End Try
            End If
            Return Me._connection
        End Get
        Set(ByVal value As Data.Common.DbConnection)
            Me._connection = value
        End Set
    End Property

    Public ReadOnly Property MyStatusBar() As Windows.Forms.ToolStripStatusLabel
        Get
            Return Me.tsStatusBar
        End Get
    End Property

    Public ReadOnly Property MyNotifyIcon() As Windows.Forms.NotifyIcon
        Get
            Return Me.NotifyIcon1
        End Get
    End Property

    Public ReadOnly Property MyMenuTop() As Windows.Forms.MenuStrip
        Get
            Return Me.MenuTop
        End Get
    End Property

    Public ReadOnly Property MyProgressBar() As Windows.Forms.ToolStripProgressBar
        Get
            Return Me.tsProgressBar
        End Get
    End Property

    Public Sub _hideMenuLeft()
        Me.SplitContainer1.Panel1Collapsed = True
    End Sub

   

    Protected Sub _addTabPage(ByVal tp As TabPage)
        tp.ImageIndex = 0
        'Roberto Rutigliano 08/07/2010
        If Not tabControlMain.TabPages.ContainsKey(tp.Name) Then
            tabControlMain.TabPages.Add(tp)
            tabControlMain.SelectTab(tabControlMain.TabPages.Count - 1)
        Else
            tabControlMain.SelectedIndex = tabControlMain.TabPages.IndexOfKey(tp.Name)
        End If


        'If tabControlMain.TabPages.ContainsKey(tp.Name) Then
        '    tabControlMain.TabPages.RemoveByKey(tp.Name)
        'End If

        'tabControlMain.TabPages.Add(tp)
        'tabControlMain.SelectTab(tabControlMain.TabPages.Count - 1)

    End Sub

    Overridable Sub _formMasterOnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tsStatusBar.Text = ""
        Me.Text = My.Application.Info.ProductName & " - " & My.Application.Info.Version.ToString()
        Me.MyProgressBar.Step = 1
    End Sub

    Overridable Sub _formMasterOnFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Trace.Flush()
        Trace.Close()

        If _connection IsNot Nothing Then
            _connection.Close()
            _connection.Dispose()
        End If

        RemoveHandler Me.FormClosing, AddressOf _formMasterOnFormClosing

        MyBase.Close()

    End Sub


    Overridable Function _init() As Boolean
     

        '*** FOLDER ***
        _folderUserData = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\"
        _folderTemp = IO.Path.GetTempPath()

        Dim d As IO.DirectoryInfo
        If Not String.IsNullOrEmpty(My.Application.Info.CompanyName) Then
            _folderUserData &= My.Application.Info.CompanyName & "\"
            d = New IO.DirectoryInfo(_folderUserData)
            If Not d.Exists Then
                d.Create()
            End If
        End If



        If Not String.IsNullOrEmpty(My.Application.Info.ProductName) Then
            _folderUserData &= My.Application.Info.ProductName & "\"
            d = New IO.DirectoryInfo(_folderUserData)
            If Not d.Exists Then
                d.Create()
            End If
        End If


        'Dim d As New IO.DirectoryInfo(_folderUserData & "temp\")
        'If d.Exists Then
        '    d.Delete(True)
        'End If
        'd.Create()

        'Roberto Rutigliano 26/03/2010 
        'Setting folder for DATABASE
        AppDomain.CurrentDomain.SetData("DataDirectory", _folderUserData)
        'AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath & "\Data\")


        If System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection") IsNot Nothing Then
            Try
                Dim provider As String = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ProviderName
                Dim factory As System.Data.Common.DbProviderFactory
                factory = System.Data.Common.DbProviderFactories.GetFactory(provider)

                _connection = factory.CreateConnection()
                _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

                '20/05/2010 Roberto Rutigliano 
                'apro la connessione 

                _connection.Open()
            Catch ex As Exception
                Windows.Forms.MessageBox.Show("Impossibile stabilire una connessione con il database. Verificare la stringa di connessione e la presenza dei Driver." & vbCrLf & ex.Message, _
                    My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)

            End Try


        End If

        _buildMenuTop()
        _buildMenuLeft()


        '20/09/2010 Notify Icon
        Me.NotifyIcon1.Icon = Me.Icon
        Me.NotifyIcon1.Text = My.Application.Info.ProductName
        Me.NotifyIcon1.Visible = True

        Return True
    End Function


    Overridable Function _buildMenuLeft() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function




#Region "Cross-Threading"

    Public Sub _cursorDefault()
        'per consentire l'accesso a Thread diversi (Cross-thread)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateCursorDefault(AddressOf Me._cursorDefault)
            Me.Invoke(theDelegate)
        Else
            Me.Cursor = Windows.Forms.Cursors.Default
        End If
    End Sub


    Public Sub _cursorWait()

        'per consentire l'accesso a Thread diversi (Cross-thread)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateCursorWait(AddressOf Me._cursorWait)
            Me.Invoke(theDelegate)
        Else
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
        End If
    End Sub

    Public Sub _progressBarPerformStep()
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateProgressBarPerformStep(AddressOf Me._progressBarPerformStep)
            Me.Invoke(theDelegate)
        Else
            Me.tsProgressBar.PerformStep()
            Windows.Forms.Application.DoEvents()
        End If
    End Sub


    Public Sub _progressBarStyleMarquee()
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateProgressBarChangeStyle(AddressOf Me._progressBarStyleMarquee)
            Me.Invoke(theDelegate)
        Else
            Me.tsProgressBar.Style = ProgressBarStyle.Marquee
            Windows.Forms.Application.DoEvents()
        End If
    End Sub


    Public Sub _progressBarStyleBlock()
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateProgressBarChangeStyle(AddressOf Me._progressBarStyleBlock)
            Me.Invoke(theDelegate)
        Else
            Me.tsProgressBar.Style = ProgressBarStyle.Blocks
            Windows.Forms.Application.DoEvents()
        End If
    End Sub


    Public Sub _progressBarSetValue(ByVal value As Integer)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateProgressBarSetValue(AddressOf Me._progressBarSetValue)
            Me.Invoke(theDelegate, New Object() {value})
        Else
            Me.tsProgressBar.Value = value
            Windows.Forms.Application.DoEvents()
        End If
    End Sub

    Public Sub _progressBarSetMaximumValue(ByVal value As Integer)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateProgressBarSetMaximumValue(AddressOf Me._progressBarSetMaximumValue)
            Me.Invoke(theDelegate, New Object() {value})
        Else
            Me.tsProgressBar.Maximum = value
        End If
    End Sub



    Public Sub _statusBarUpdate(ByVal value As String)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateStatusBarUpdate(AddressOf Me._statusBarUpdate)
            Me.Invoke(theDelegate, New Object() {value})
        Else
            Me.tsStatusBar.Text = value
            Windows.Forms.Application.DoEvents()
        End If
    End Sub



    Public Sub _statusBarUpdateStep()
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateStatusBarUpdateStep(AddressOf Me._statusBarUpdateStep)
            Me.Invoke(theDelegate)
        Else
            Me.tsStatusBar.Text = Me.tsStatusBar.Text & "."
            If Me.tsStatusBar.Text.Length > 50 Then
                Me.tsStatusBar.Text = Me.tsStatusBar.Text.Replace(".", "")
            End If
            Windows.Forms.Application.DoEvents()
        End If
    End Sub



    'Public Function _getMyOwner() As Windows.Forms.Form
    '    If InvokeRequired Then
    '        Dim theDelegate As New delegateGetMyOwner(AddressOf Me._getMyOwner)
    '        Me.Invoke(theDelegate)
    '    End If

    '    Return Me
    'End Function

#End Region

#Region "Menu TOP"

    Overridable Function _buildMenuTop() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


    Function _addMenuTopLevel_1(ByVal key As String, ByVal text As String, ByVal index As Integer) As System.Windows.Forms.ToolStripMenuItem
        Dim n As New System.Windows.Forms.ToolStripMenuItem
        n.Name = key
        n.Text = text
        Me.MenuTop.Items.Insert(index, n)
        Return n
    End Function

    Function _addMenuTopLevel_1(ByVal text As String) As System.Windows.Forms.ToolStripMenuItem
        Return _addMenuTopLevel_1("key_" & text.Replace(" ", "_"), text, MenuTop.Items.Count)
    End Function

    Function _addMenuTopLevel_1(ByVal text As String, ByVal index As Integer) As System.Windows.Forms.ToolStripMenuItem
        Return _addMenuTopLevel_1("key_" & text.Replace(" ", "_"), text, index)
    End Function

    Function _addMenuTopLevel_2(ByVal key As String, ByVal text As String, ByVal parent As System.Windows.Forms.ToolStripMenuItem, ByVal index As Integer) As System.Windows.Forms.ToolStripMenuItem
        Dim n As New System.Windows.Forms.ToolStripMenuItem
        n.Name = key
        n.Text = text
        AddHandler n.Click, AddressOf subItemOnClick
        parent.DropDownItems.Insert(index, n)
        Return n
    End Function


    Function _addMenuTopLevel_2(ByVal key As String, ByVal text As String, ByVal parent As System.Windows.Forms.ToolStripMenuItem, _
                                ByVal index As Integer, ByVal checked As Boolean) As System.Windows.Forms.ToolStripMenuItem
        Dim n As New System.Windows.Forms.ToolStripMenuItem
        n = _addMenuTopLevel_2(key, text, parent, index)
        n.CheckOnClick = True
        n.Checked = checked
        Return n
    End Function

    Sub subItemOnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        '     RaiseEvent MyMenuTopSubItemOnClick(sender)
        _menuTopSubItemClicked(CType(sender, System.Windows.Forms.ToolStripItem))
    End Sub


    Function _addMenuTopLevel_2(ByVal text As String, ByVal parent As System.Windows.Forms.ToolStripMenuItem) As System.Windows.Forms.ToolStripMenuItem
        Return _addMenuTopLevel_2("key_" & text.Replace(" ", "_"), text, parent, parent.DropDown.Items.Count)
    End Function


    '  Private Sub MenuTop_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuTop.ItemClicked
    '      _menuTopClicked(e.ClickedItem)
    '  End Sub


    Overridable Function _menuTopSubItemClicked(ByVal clickedItem As System.Windows.Forms.ToolStripItem) As Boolean
        Return False
    End Function


    Overridable Sub _menuTop_ExitOnClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Overridable Sub _menuTop_AboutOnClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim f As New FormAbout()
        f.Owner = Me
        'f._init(New IO.FileInfo(Application.StartupPath & "\logo.jpg"), "http://www.techub.it/")
        f._init(Nothing, "http://www.techub.it/", _folderUserData)
        f.ShowDialog(Me)
    End Sub

    Friend Sub FileLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileLogToolStripMenuItem.Click
        If String.IsNullOrEmpty(_pathFileLogMain) Then
            Exit Sub
        End If
        Dim f As New FormLog()
        f._init(New IO.FileInfo(_pathFileLogMain))
        f.ShowDialog(Me)
    End Sub

    Private Sub KillProcessExcelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KillProcessExcelToolStripMenuItem.Click
        If Windows.Forms.MessageBox.Show("Confirm kill process Excel", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OKCancel, Windows.Forms.MessageBoxIcon.Error) <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If

        Dim proc As System.Diagnostics.Process
        For Each proc In System.Diagnostics.Process.GetProcessesByName("EXCEL")
            proc.Kill()
        Next

        Windows.Forms.MessageBox.Show("Completed", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OKCancel, Windows.Forms.MessageBoxIcon.Information)
    End Sub

    Public Sub _displayMenuTopKillProcessExcel()
        Me.KillProcessExcelToolStripMenuItem.Visible = True
    End Sub

    Public Sub _displayMenuTopHelpDocumentation()
        Me.HelpDocumentationToolStripMenuItem.Visible = True
    End Sub

    Private Sub HelpDocumentationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpDocumentationToolStripMenuItem.Click
        _menuTop_HelpDocumentationOnClick()
    End Sub

    Overridable Sub _menuTop_HelpDocumentationOnClick()
        Windows.Forms.MessageBox.Show("Function Help Doucumentation not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Sub

#End Region




#Region "___ NOTIFY ICON___"

    Public Sub _notifyIcon(ByVal message As String)
        NotifyIcon1.ShowBalloonTip(2000, My.Application.Info.ProductName, message, ToolTipIcon.Info)
    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        If Not Me.Visible OrElse Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

#End Region

    Overridable Sub _menuTop_OptionsOnClick()
        Windows.Forms.MessageBox.Show("Function Options not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Sub

    
    Public Sub _displayMenuTopOptions()
        Me.OptionsToolStripMenuItem.Visible = True
    End Sub
  
    
    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        _menuTop_OptionsOnClick()
    End Sub
End Class