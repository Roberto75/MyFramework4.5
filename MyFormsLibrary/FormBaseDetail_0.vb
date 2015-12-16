Public Class FormBaseDetail_0

    Protected _statusBar As Windows.Forms.ToolStripStatusLabel
    Protected _progressBar As Windows.Forms.ToolStripProgressBar
    Protected mConnection As System.Data.Common.DbConnection

    Private _helpIndexTopicForm As String
    Private _helpNameSpace As String


    Private Delegate Sub delegateStatusBarUpdate(ByVal value As String)

    'Public Overridable Overloads Function _init() As Boolean
    '    Me._statusBar = Nothing
    '    Me.mConnection = Nothing
    '    Me._progressBar = Nothing
    '    Return True
    'End Function


    'Public Overridable Overloads Function _init(ByRef connection As System.Data.Common.DbConnection) As Boolean
    '    Me.mConnection = connection
    '    Me._statusBar = Nothing
    '    Me._progressBar = Nothing
    '    Return True
    'End Function


    'Public Overridable Overloads Function _init(ByRef statusBar As Windows.Forms.ToolStripStatusLabel, ByRef progressBar As Windows.Forms.ToolStripProgressBar) As Boolean
    '    Me._statusBar = statusBar
    '    Me._progressBar = progressBar
    '    Me.mConnection = Nothing
    '    Return True
    'End Function

    Public Overridable Overloads Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As Windows.Forms.ToolStripStatusLabel, ByRef progressBar As Windows.Forms.ToolStripProgressBar) As Boolean
        Me.mConnection = connection
        Me._progressBar = progressBar
        Me._statusBar = statusBar
        Return True
    End Function


    'Private _refFormMain As FormMaster
    'Public Property RefFormMain() As FormMaster
    '    Get
    '        Return _refFormMain
    '    End Get
    '    Set(ByVal value As FormMaster)
    '        _refFormMain = value
    '    End Set
    'End Property


    Public Sub _closeMe()
        MyBtnCloseOnClick()
    End Sub


    'Public Sub _showButtonWizard()
    '    Me.btnWizard.Visible = True
    'End Sub



    Public Sub _setHelp(ByVal myNamaspace As IO.FileInfo, ByVal pageTopicIndex As String)
        _helpNameSpace = myNamaspace.FullName
        _helpIndexTopicForm = pageTopicIndex
        Me.MyButtonHelp.Visible = True
    End Sub


    Public Sub _statusBarUpdate(ByVal value As String)
        If Me.InvokeRequired Then
            Dim theDelegate As New delegateStatusBarUpdate(AddressOf Me._statusBarUpdate)
            Me.Invoke(theDelegate, New Object() {value})
        Else
            Try
                Me._statusBar.Text = value
            Catch ex As Exception

            End Try

            Windows.Forms.Application.DoEvents()
        End If
    End Sub


    Public Sub _hideButtonClose()
        Me.MyBtnClose.Visible = False
    End Sub


    Public Function _getTabPage() As System.Windows.Forms.TabPage
        Return tabPageMain
    End Function



    Public Sub _addSeparatorToHeader(ByVal myAlignement As System.Windows.Forms.ToolStripItemAlignment)
        Dim t As New System.Windows.Forms.ToolStripSeparator
        t.Alignment = myAlignement
        Me.MyToolstripTop.Items.Add(t)
    End Sub

    Public Sub _addButtonToHeader(ByVal myLabel As String _
                                     , ByVal myName As String _
                                     , ByVal myImage As System.Drawing.Image _
                                     , ByVal myAlignement As System.Windows.Forms.ToolStripItemAlignment)


        Dim myButton As New System.Windows.Forms.ToolStripButton
        myButton = New System.Windows.Forms.ToolStripButton
        myButton.Alignment = myAlignement

        If Not myImage Is Nothing Then
            myButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            'Me.myButton.Image = CType(Resources.GetObject("btnCollapse.Image"), System.Drawing.Image)
            myButton.Image = myImage
            myButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        End If

        myButton.Name = myName
        myButton.Size = myImage.Size
        myButton.Text = myLabel

        Me.MyToolstripTop.Items.Add(myButton)
    End Sub


    Private Sub MyToolstripHeader_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MyToolstripTop.ItemClicked
        If e.ClickedItem.GetType.Name = "ToolStripButton" Then
            If e.ClickedItem.Name = "MyBtnClose" Then
                MyBtnCloseOnClick()
            Else
                _toolStripHeaderButtonClicked(CType(e.ClickedItem, Windows.Forms.ToolStripButton))
            End If

        End If
    End Sub


    Public Overridable Function _toolStripHeaderButtonClicked(ByVal myButton As Windows.Forms.ToolStripButton) As Boolean
        'Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function




    Public Sub _addTitleToHeader(ByVal label As String, ByVal value As String)
        value = value.Replace("_", " ")
        'Dim name As String
        'name = value.Replace(" ", "")

        Dim ts As ToolStripLabel

        If MyToolstripTop.Items.ContainsKey(label) Then
            ts = CType(MyToolstripTop.Items(MyToolstripTop.Items.IndexOfKey("valueOf" & label)), System.Windows.Forms.ToolStripLabel)
        Else
            ts = New ToolStripLabel(label)
            ts.Name = label.Replace(" ", "")

            ts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            ts.Alignment = ToolStripItemAlignment.Left

            MyToolstripTop.Items.Add(ts)

            ts = New ToolStripLabel(value.Replace(" ", ""))
            ts.Name = "valueOf" & label
            ts.Alignment = ToolStripItemAlignment.Left

            MyToolstripTop.Items.Add(ts)
        End If

        ts.Text = value
    End Sub

    Public Sub _setTitolo(ByVal value As String)
        value = value.Replace("_", " ")
        Dim ts As ToolStripLabel
        ts = New ToolStripLabel(value)
        ts.Name = "valueOf" & value.Replace(" ", "")
        ts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        ts.Alignment = ToolStripItemAlignment.Left

        MyToolstripTop.Items.Add(ts)
        Me.tabPageMain.Name = value.Replace(" ", "")
        Me.tabPageMain.Text = value
    End Sub

    Public Sub _setTitolo(ByVal label As String, ByVal value As String)
        value = value.Replace("_", " ")
        Me._addTitleToHeader(label, value)
        Me.tabPageMain.Name = label.Replace(" ", "") & value.Replace(" ", "")
        Me.tabPageMain.Text = label & " " & value
    End Sub


    Private Sub MyBtnCloseOnClick()
        If _OnTabClosing() Then

            Dim ctrl As System.Windows.Forms.Control = MyBtnClose.GetCurrentParent.Parent
            'Search TabControl ...
            While Not TypeOf ctrl Is System.Windows.Forms.TabControl
                ctrl = ctrl.Parent
                If TypeOf ctrl Is Windows.Forms.Form Then Throw New MyControlsLibrary.MyException("UIFeedback can't find TabControl and remove panel. ")
            End While



            Dim t As System.Windows.Forms.TabControl = CType(ctrl, System.Windows.Forms.TabControl)
            Dim tab As System.Windows.Forms.TabPage
            tab = t.SelectedTab

            t.TabPages.Remove(tab)
            t.SelectedIndex = t.TabPages.Count - 1

            'Roberto Rutigliano 04/08/2010
            tab.Dispose()
        End If
    End Sub

    'evento richiamato nel momento in cui si stà chiudendo la TAB
    Overridable Function _OnTabClosing() As Boolean
        Return True
    End Function


    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopDown.Click
        _OnTopButtonDownClick()
    End Sub

    Overridable Function _OnTopButtonDownClick() As Boolean
        Return True
    End Function

    Public Sub _cursorDefault()
        Me.tabPageMain.Cursor = Windows.Forms.Cursors.Default
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _cursorWait()
        Me.tabPageMain.Cursor = Cursors.WaitCursor
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Overridable Sub MyButtonHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyButtonHelp.Click
        Help.ShowHelp(Me, _helpNameSpace, HelpNavigator.Topic, Me._helpIndexTopicForm)

    End Sub


    Sub _addTabPageDetail(ByVal tp As Windows.Forms.TabPage)
        _addTabPageDetail(tp, True)
    End Sub

    Sub _addTabPageDetail(ByVal tp As Windows.Forms.TabPage, ByVal andSelectIt As Boolean)
        Dim ctrl As System.Windows.Forms.Control

        ctrl = Me.tabPageMain
        If ctrl Is Nothing Then
            Throw New MyControlsLibrary.MyException("Form MAIN, the control il NULL")
        End If

        'Search TabControl ...
        While Not TypeOf ctrl Is System.Windows.Forms.TabControl
            'OrElse ctrl.Tag Is Nothing _
            ' OrElse ctrl.Tag <> PlugInName

            ctrl = ctrl.Parent
            If ctrl Is Nothing OrElse TypeOf ctrl Is Windows.Forms.Form Then
                Throw New MyControlsLibrary.MyException("Form MAIN, can't find TabControl ")
            End If
        End While

        'ctrl = ctrl.Controls.Find("tabControlMain", False)(0)
        If ctrl Is Nothing OrElse Not (TypeOf ctrl Is Windows.Forms.TabControl) Then
            Throw New MyControlsLibrary.MyException("Form MAIN, can't find tabControlMain.")
        End If

        Dim myTabControl As System.Windows.Forms.TabControl = CType(ctrl, System.Windows.Forms.TabControl)

        'change image index from FormMain
        tp.ImageIndex = 0

        If Not myTabControl.TabPages.ContainsKey(tp.Name) Then
            myTabControl.TabPages.Add(tp)

            If andSelectIt = True Then
                myTabControl.SelectTab(tp)
            End If
        Else
            myTabControl.SelectedIndex = myTabControl.TabPages.IndexOfKey(tp.Name)
        End If



    End Sub


    'Private Sub btnWizard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWizard.Click
    '    _btnWizardOnClick()
    'End Sub


    'Public Overridable Function _btnWizardOnClick() As Boolean
    '    Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    '    Return False
    'End Function
End Class