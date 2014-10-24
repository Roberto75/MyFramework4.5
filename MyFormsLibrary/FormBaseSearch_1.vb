Public Class FormBaseSearch_1
    Inherits MyFormsLibrary.FormBaseSearch_0

    Friend _pageCurrent As Int16
    Friend _pageSize As Int16


    Protected _columnDelete As System.Windows.Forms.DataGridViewImageColumn
    Protected _columnEdit As System.Windows.Forms.DataGridViewImageColumn

    Private tsbtnCountItems As System.Windows.Forms.ToolStripLabel

    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        tsbtnCountItems = New System.Windows.Forms.ToolStripLabel

        Me.tsbtnCountItems.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnCountItems.Name = "tsbtnCountItems"
        tsbtnCountItems.Text = ""
        MyToolStrip3.Items.Add(tsbtnCountItems)
        Return MyBase._init(connection, statusBar, progressBar)

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