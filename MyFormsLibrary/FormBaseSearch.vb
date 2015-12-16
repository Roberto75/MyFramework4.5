Public Class FormBaseSearch
    Friend _pageCurrent As Int16
    Friend _pageSize As Int16
    Friend mConnection As Data.Common.DbConnection

    Protected _columnDelete As System.Windows.Forms.DataGridViewImageColumn
    Protected _columnEdit As System.Windows.Forms.DataGridViewImageColumn

  

    Public Overridable Function _init(ByVal connection As Data.Common.DbConnection) As Boolean
        Me.mConnection = connection
        Me.tsbtnCountItems.Text = ""
        _initColumns()
        Return True
    End Function

    Public Overridable Function _initColumns() As Boolean
        Return True
    End Function




    Public Sub _showButtonAdd()
        Me.btnAdd.Visible = True
    End Sub

    Public Sub _hideButtonAdd()
        Me.btnAdd.Visible = False
    End Sub


    Sub _AddTabpageDetail(ByVal tp As Windows.Forms.TabPage)
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
            myTabControl.SelectTab(myTabControl.TabPages.Count - 1)
        Else
            myTabControl.SelectedIndex = myTabControl.TabPages.IndexOfKey(tp.Name)
        End If

    End Sub



    Public Function _getTabPage() As System.Windows.Forms.TabPage
        Return tabPageMain
    End Function


    Public Sub _setTitolo(ByVal value As String)
        Dim ts As ToolStripLabel
        ts = New ToolStripLabel(value)
        ts.Name = "valueOf" & value.Replace(" ", "")
        ts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)

        MyToolstripTop.Items.Add(ts)
        Me.tabPageMain.Name = value.Replace(" ", "")
        Me.tabPageMain.Text = value
    End Sub


    Public Sub _hideButtonPrint()
        Me.tsbtnPrint.Visible = False
    End Sub

    Private Sub tsbtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnPrint.Click
        CType(sender, Windows.Forms.ToolStripButton).GetCurrentParent.Cursor = Windows.Forms.Cursors.WaitCursor
        _buttonPrintOnClick()
        CType(sender, Windows.Forms.ToolStripButton).GetCurrentParent.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Overridable Function _buttonPrintOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Private Sub myDataGridView_RowHeaderMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles myDataGridView.RowHeaderMouseDoubleClick
        CType(sender, Windows.Forms.DataGridView).Cursor = Windows.Forms.Cursors.WaitCursor
        _dataGridViewItemOnDoubleClick(e)
        CType(sender, Windows.Forms.DataGridView).Cursor = Windows.Forms.Cursors.Default
    End Sub

    Overridable Function _dataGridViewItemOnDoubleClick(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Private Sub myDataGridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles myDataGridView.CellContentClick
        If e.RowIndex = -1 Then Exit Sub
        CType(sender, Windows.Forms.DataGridView).Cursor = Windows.Forms.Cursors.WaitCursor

        If Not (_columnDelete Is Nothing) AndAlso myDataGridView.Columns(e.ColumnIndex).Name = _columnDelete.Name Then
            _dataGridViewItemButtonDeleteOnClick(e)
        ElseIf Not (_columnEdit Is Nothing) AndAlso myDataGridView.Columns(e.ColumnIndex).Name = _columnEdit.Name Then
            _dataGridViewItemButtonEditOnClick(e)
        Else
            _dataGridViewItemCellContentClick(e)
        End If

        CType(sender, Windows.Forms.DataGridView).Cursor = Windows.Forms.Cursors.Default
    End Sub

    Overridable Function _dataGridViewItemCellContentClick(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean
        Windows.Forms.MessageBox.Show("Item OnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Overridable Function _dataGridViewItemButtonEditOnClick(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean
        Windows.Forms.MessageBox.Show("Button Edit OnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Overridable Function _dataGridViewItemButtonDeleteOnClick(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean
        Windows.Forms.MessageBox.Show("Button Delete OnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Overridable Function _buttonResetOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Overridable Function _buttonAddOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


    Overridable Function _doSearch(ByVal pageNumber As Int16, ByVal pageSize As Int16) As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


    

    Private Sub btnCollapse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCollapse.Click
        If SplitContainer1.Panel2Collapsed Then
            CType(sender, System.Windows.Forms.ToolStripButton).Text = "Nascondi i filtri di ricerca"
            SplitContainer1.Panel2Collapsed = False
        Else
            CType(sender, System.Windows.Forms.ToolStripButton).Text = "Visualizza i filtri di ricerca"
            SplitContainer1.Panel2Collapsed = True
        End If
    End Sub

    Private Sub MyBtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBtnClose.Click
        Dim ctrl As System.Windows.Forms.Control = CType(sender, System.Windows.Forms.ToolStripButton).GetCurrentParent.Parent
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
        'Roberto Rutigliano 04/08/2010 problemi di allocazione memoria
        tab.Dispose()
    End Sub


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor

        Me.myDataGridView.Rows.Clear()
        If _doSearch(_pageCurrent, _pageSize) Then
            Me.myDataGridView.Visible = True
            Dim count As Integer
            count = Me.myDataGridView.Rows.Count
            Me.tsbtnCountItems.Text = "Elementi visualizzati: " & count
        End If
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
    End Sub


    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor
        _buttonResetOnClick()
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
    End Sub




    Public Function _getReferenceToMainForm() As System.Windows.Forms.Form
        Return _getReferenceToMainForm("FRMMAIN")
    End Function


    Public Function _getReferenceToMainForm(ByVal nameForm As String) As System.Windows.Forms.Form

        Dim ctrl As System.Windows.Forms.Control
        ctrl = Me.tabPageMain
        'Search TabControl ...
        While Not (TypeOf ctrl Is System.Windows.Forms.Form AndAlso ctrl.Name.ToUpper() = nameForm.ToUpper)
            If ctrl.Parent Is Nothing Then
                Throw New MyControlsLibrary.MyException("Can't found reference to FormMain ")
            End If
            ctrl = ctrl.Parent
            'If TypeOf ctrl Is Windows.Forms.Form Then 
        End While
        Return CType(ctrl, System.Windows.Forms.Form)
    End Function

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        _buttonAddOnClick()
    End Sub

    Protected Sub _addColumnButtonDelete()
        _columnDelete = New System.Windows.Forms.DataGridViewImageColumn

        With _columnDelete
            .Name = "ColumnDelete"
            .HeaderText = ""
            .Width = 20
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            .ImageLayout = DataGridViewImageCellLayout.Normal
            .Image = MyControlsLibrary.My.Resources.MyResource.elimina_16x16
            .ToolTipText = "Delete"
        End With

        myDataGridView.Columns.Add(_columnDelete)
    End Sub


    Protected Sub _addColumnButtonEdit()
        _columnEdit = New System.Windows.Forms.DataGridViewImageColumn

        With _columnEdit
            .Name = "ColumnEdit"
            .HeaderText = ""
            .Width = 20
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            .ImageLayout = DataGridViewImageCellLayout.Normal
            .Image = MyControlsLibrary.My.Resources.MyResource.edit1_16x16
            .ToolTipText = "Edit"
        End With

        myDataGridView.Columns.Add(_columnEdit)
    End Sub



End Class