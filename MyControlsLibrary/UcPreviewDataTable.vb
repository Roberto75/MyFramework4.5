Public Class UcPreviewDataTable

    Public _includeColumnHeaderInCSVFile As Boolean = True

    Private _dataTable As DataTable

    Public Event MyPreviewDataTableOnDubleClick(ByVal rowNumber As Integer)
    Public Event MyPreviewDataTableOnDeleteClick(ByVal rowNumbers As Integer())
    

    Private _autoSizeColumn As Boolean = True

    Private _isActiveColumnsFilter As Boolean

    Private _showButtonFilter As Boolean = True
    Public Property MyShowButtonFilter() As Boolean
        Get
            Return _showButtonFilter
        End Get
        Set(ByVal value As Boolean)
            _showButtonFilter = value

            If _showButtonFilter Then
                ' _hideLimitMaxResults()
                Me.tsbFilterRemove.Visible = False
                Me.tsbFilterEnable.Visible = True
                ' _isActiveColumnsFilter = True
            Else
                Me.tsbFilterRemove.Visible = False
                Me.tsbFilterEnable.Visible = False
                '    _isActiveColumnsFilter = False

            End If

        End Set
    End Property

    Private _showIconHeaderRow As Boolean
    Public Property MyShowIconHeaderRow() As Boolean
        Get
            Return _showIconHeaderRow
        End Get
        Set(ByVal value As Boolean)
            _showIconHeaderRow = value

            If _showIconHeaderRow Then
                AddHandler DataGridView1.CellPainting, AddressOf Me.DataGridView1_CellPainting
            Else
                RemoveHandler DataGridView1.CellPainting, AddressOf Me.DataGridView1_CellPainting
            End If
        End Set
    End Property

    Private _showButtonDelete As Boolean
    Public Property MyShowButtonDelete() As Boolean
        Get
            Return _showButtonDelete
        End Get
        Set(ByVal value As Boolean)
            _showButtonDelete = value

            If _showButtonDelete Then
                AddHandler DataGridView1.CellContentClick, AddressOf Me.DataGridView1_CellContentClick
                Me.DeleteSelectedToolStripMenuItem.Visible = True
            Else
                RemoveHandler DataGridView1.CellContentClick, AddressOf Me.DataGridView1_CellContentClick
                Me.DeleteSelectedToolStripMenuItem.Visible = False
            End If
        End Set
    End Property


    Public Property MyMultiSelect() As Boolean
        Get
            Return DataGridView1.MultiSelect
        End Get
        Set(ByVal value As Boolean)
            DataGridView1.MultiSelect = value
        End Set
    End Property



    Public ReadOnly Property _Rows() As Windows.Forms.DataGridViewRowCollection
        Get
            Return Me.DataGridView1.Rows
        End Get
    End Property

    Public ReadOnly Property _RowsSelected() As Windows.Forms.DataGridViewSelectedRowCollection
        Get
            Return Me.DataGridView1.SelectedRows
        End Get
    End Property



    Public ReadOnly Property _RowCount() As Long
        Get
            Return Me.DataGridView1.RowCount
        End Get
    End Property

    

    'Public Sub _hideLimitMaxResults()
    '    _showLimitMaxResult = False
    '    txtMaxResults.Visible = False
    '    Label1.Visible = False
    'End Sub


    Public Sub _clear()
        'DataGridView1.Rows.Clear()
        'DataGridView1.Columns.Clear()
        DataGridView1.DataSource = Nothing
        Me.lblItems.Text = "Found items: 0"
        Me.tsbtnSave.Enabled = False
        If Not _dataTable Is Nothing Then
            _dataTable.Dispose()
        End If

        _dataTable = Nothing
    End Sub


    Public Sub New()
        InitializeComponent()
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        If _showButtonFilter Then
            Me.tsbFilterRemove.Visible = False
            Me.tsbFilterEnable.Visible = True
        Else
            Me.tsbFilterRemove.Visible = False
            Me.tsbFilterEnable.Visible = False
        End If
    End Sub


    'Public Sub _setMaxResults(ByRef value As Long)
    '    txtMaxResults.Text = value
    'End Sub

    'Public Sub _resetMaxResults()
    '    txtMaxResults.Text = "0"
    'End Sub


    Public Function _init(ByVal dt As DataTable) As Boolean

        Me._dataTable = dt

        DataGridView1.AutoGenerateColumns = True
      
        DataGridView1.AllowUserToResizeColumns = True
        DataGridView1.AllowUserToResizeRows = True

        DataGridView1.DataSource = Nothing
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()

        If _dataTable Is Nothing OrElse _dataTable.Rows.Count = 0 Then
            Me.lblItems.Text = "Found items: 0"
            Me.tsbtnSave.Enabled = False
            Return False
        Else
            If _dataTable.Columns.Count <> 0 Then

                '24/11/2010 Roberto Rutigliano
                'Se imposto l'AutoSize l'utente NON può più ridimensionare le colonne!
                'Quindi lo imposto e alla fine lo disabilito
                If _autoSizeColumn = True Then
                    Me.DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                Else
                    Me.DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                End If
                

                'Roberto Rutigliano 25/05/2010
                'aggiunto per la formattazione in EURO nei file EXCEL
                For Each col As Data.DataColumn In _dataTable.Columns
                    If col.ColumnName.ToUpper.EndsWith("__EURO") Then
                        col.ColumnName = col.ColumnName.Replace("__EURO", "").Replace("__euro", "")
                    End If
                Next



                '  Dim limitResult As Long
                'If String.IsNullOrEmpty(Me.txtMaxResults.Text) Then
                '    limitResult = 0
                'Else
                '    limitResult = Long.Parse(Me.txtMaxResults.Text.Replace(".", ""))
                'End If

                'If limitResult > 0 AndAlso dt.Rows.Count > limitResult Then
                '    'ci sono + dati 
                '    Me.Label1.ForeColor = Color.Red

                '    Dim dataSource As New BindingSource(dt.AsEnumerable().Take(limitResult).CopyToDataTable, Nothing)
                '    Me.DataGridView1.DataSource = dataSource
                'Else
                Dim dataSource As New BindingSource(_dataTable, Nothing)

                Me.DataGridView1.DataSource = dataSource
                '    End If

                'If Me.DataGridView1.Columns.Count > 0 Then
                '    Dim idcolon As Integer
                '    idcolon = Me.DataGridView1.Columns.Count - 1
                '    Me.DataGridView1.Columns(idcolon).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                '    Me.DataGridView1.Columns(idcolon).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                '    'Me.DataGridView1.Columns(0).Visible = False

                'End If

                'Me.DataGridView1.Cursor = Cursors.Hand


                If Me._showButtonDelete Then
                    Dim imageColumn As New Windows.Forms.DataGridViewImageColumn
                    imageColumn.HeaderText = ""
                    imageColumn.Name = "btnMyDelete"
                    imageColumn.Image = My.Resources.MyResource.elimina_16x16
                    imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    Me.DataGridView1.Columns.Add(imageColumn)
                End If


                If Not _isActiveColumnsFilter Then
                    Me.lblItems.Text = String.Format("Found items: {0:N0}", dt.Rows.Count)
                End If


                Me.tsbtnSave.Enabled = True
                Windows.Forms.Application.DoEvents()

                '24/11/2010 Roberto Rutigliano
                'Disabilito l'AutoSize in modo da dare la possibilità all'utente di trascinare le colonne!

                'Me.DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

            End If
        End If
        Return True
    End Function

#Region "___ MENU CONTESTUALE ___"


    Private Sub DeleteSelectedToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteSelectedToolStripMenuItem.Click
        If _dataTable Is Nothing OrElse _dataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        If DataGridView1.SelectedRows.Count = 0 Then
            Exit Sub
        End If

        Dim temp(DataGridView1.SelectedRows.Count - 1) As Integer
        'For Each row As System.Windows.Forms.DataGridViewRow In DataGridView1.SelectedRows
        For i As Integer = 0 To DataGridView1.SelectedRows.Count - 1
            temp(i) = DataGridView1.SelectedRows(i).Index
        Next


        RaiseEvent MyPreviewDataTableOnDeleteClick(temp)




    End Sub


    Private Sub ExportCVSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportCVSToolStripMenuItem.Click
        If _dataTable Is Nothing OrElse _dataTable.Rows.Count = 0 Then
            Exit Sub
        End If


        If MessageBox.Show(String.Format("Confirm export of {0:N0} record?", _dataTable.Rows.Count), My.Application.Info.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
            Exit Sub
        End If

        _saveToCSV()
    End Sub

    Public Function _saveToCSV() As Boolean
        SaveFileDialog1.Filter = "CSV files (*.csv)|*.csv"
        'setting filters so that Text files and All Files choice appears in the Files of Type box
        'in the dialog
        Dim dr As System.Windows.Forms.DialogResult
        dr = SaveFileDialog1.ShowDialog()

        If dr <> System.Windows.Forms.DialogResult.OK Then
            Return False
        End If

        'showDialog method makes the dialog box visible at run time
        Dim outputFileName As String
        outputFileName = SaveFileDialog1.FileName

        CSVManager.writeFileCSV(_dataTable, outputFileName, ";", _includeColumnHeaderInCSVFile)

        MessageBox.Show("Export completed", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True
    End Function



    Private Sub ExportXMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportXMLToolStripMenuItem.Click
        If _dataTable Is Nothing OrElse _dataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        If MessageBox.Show(String.Format("Confirm export of {0:N0} record?", _dataTable.Rows.Count), My.Application.Info.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) <> DialogResult.OK Then
            Exit Sub
        End If
        _saveToXML()
    End Sub

    Public Function _saveToXML() As Boolean
        SaveFileDialog1.Filter = "XML files (*.xml)|*.xml"

        Dim dr As System.Windows.Forms.DialogResult
        dr = SaveFileDialog1.ShowDialog()

        If dr <> System.Windows.Forms.DialogResult.OK Then
            Return False
        End If

        Dim outputFileName As String
        outputFileName = SaveFileDialog1.FileName


        DataTableManager.toXML(_dataTable, outputFileName)

        MessageBox.Show("Export completed", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True
    End Function

#End Region

    Private Sub tsbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnSave.Click
        Me.ContextMenuStrip1.Show(Me.DataGridView1, 100, 100)
    End Sub


    Private Sub DataGridView1_RowHeaderMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.RowHeaderMouseDoubleClick
        RaiseEvent MyPreviewDataTableOnDubleClick(e.RowIndex)
    End Sub


    Public Function _getCellValue(ByVal rowIndex As Integer, ByVal columnName As String) As String
        If (rowIndex < 0) Then
            Return String.Empty
        End If

        Return DataGridView1.Rows(rowIndex).Cells(columnName).Value.ToString
    End Function

    'Public Function _getCellValue(ByVal rowIndex As Integer, ByVal columnIndex As Integer) As String
    '    Return DataGridView1.Rows(rowIndex).Cells(columnIndex).Value.ToString
    'End Function



#Region "Column Filer"

    ' Configures the autogenerated columns, replacing their header
    ' cells with AutoFilter header cells. 
    Private Sub dataGridView1_DataSourceChanged(ByVal sender As Object, _
        ByVal e As EventArgs)
        ' Handles DataGridView1.DataSourceChanged

        'If Me._isActiveColumnsFilter Then



        ' Continue only if the data source has been set.
        If DataGridView1.DataSource Is Nothing Then
            Return
        End If

        ' Add the AutoFilter header cell to each column.
        For Each col As DataGridViewColumn In DataGridView1.Columns
            col.HeaderCell = New  _
                DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell)
        Next

        ' Format the OrderTotal column as currency. 
        ' dataGridView1.Columns("OrderTotal").DefaultCellStyle.Format = "c"

        ' Resize the columns to fit their contents.
        DataGridView1.AutoResizeColumns()
        '  DataGridView1.InvalidateColumn(1)
        ' End If
    End Sub



    ' Updates the filter status label. 
    Private Sub dataGridView1_DataBindingComplete(ByVal sender As Object, _
        ByVal e As DataGridViewBindingCompleteEventArgs)
        'Handles DataGridView1.DataBindingComplete

        If Me._isActiveColumnsFilter Then
            Dim filterStatus As String = DataGridViewAutoFilterColumnHeaderCell _
                       .GetFilterStatus(DataGridView1)
            If String.IsNullOrEmpty(filterStatus) Then
                Me.lblItems.Text = String.Format("Display records: {0:N0}", _dataTable.Rows.Count)
                Me.tsbFilterRemove.Visible = False
            Else
                Me.lblItems.Text = "Display " & filterStatus
                Me.tsbFilterRemove.Visible = True
            End If
        Else
            Me.lblItems.Text = String.Format("Display records: {0:N0}", _dataTable.Rows.Count)
        End If
    End Sub

    Private Sub tsbFilterEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFilterEnable.Click

        If _dataTable Is Nothing OrElse _dataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        If _isActiveColumnsFilter Then
            _isActiveColumnsFilter = False
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(DataGridView1)
            RemoveHandler DataGridView1.DataSourceChanged, AddressOf Me.dataGridView1_DataSourceChanged
            RemoveHandler DataGridView1.DataBindingComplete, AddressOf Me.dataGridView1_DataBindingComplete
            Me.DataGridView1.DataSource = Nothing
            Me.tsbFilterEnable.Image = My.Resources.MyResource.filtro_20x20
            Me.tsbFilterEnable.ToolTipText = "Enable auto filter column"
        Else
            _isActiveColumnsFilter = True
            AddHandler DataGridView1.DataSourceChanged, AddressOf Me.dataGridView1_DataSourceChanged
            AddHandler DataGridView1.DataBindingComplete, AddressOf Me.dataGridView1_DataBindingComplete
            Me.tsbFilterEnable.Image = My.Resources.MyResource.filtroActive_20x20
            Me.tsbFilterEnable.ToolTipText = "Disable auto filter column"
        End If

        'Fix
        'http://stackoverflow.com/questions/1386960/operation-can-only-be-performed-on-cells-that-belong-to-a-datagridview-control
        _dataTable.DefaultView.Sort = ""
        Me.DataGridView1.Columns.Clear()
        Me.DataGridView1.DataSource = New BindingSource(_dataTable, Nothing)
        If Me._showButtonDelete Then
            Dim imageColumn As New Windows.Forms.DataGridViewImageColumn
            imageColumn.HeaderText = ""
            imageColumn.Name = "btnMyDelete"
            imageColumn.Image = My.Resources.MyResource.elimina_16x16
            Me.DataGridView1.Columns.Add(imageColumn)
        End If
        Me.tsbFilterRemove.Visible = False
    End Sub

    Private Sub tsbFilterRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbFilterRemove.Click
        If _isActiveColumnsFilter Then
            Me.tsbFilterRemove.Visible = False
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(DataGridView1)
        End If
    End Sub





#End Region




    Private Sub DataGridView1_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs)
        'disegnare l'icona sulla colonna 1
        If e.ColumnIndex = -1 AndAlso e.RowIndex >= 0 Then
            'http://www.eggheadcafe.com/software/aspnet/29776618/datagridview--icon-link.aspx

            DataGridView1.Rows(e.RowIndex).HeaderCell.ToolTipText = String.Format("row #{0:N0} click to open detail", e.RowIndex + 1)
            ' DataGridView1.Rows(e.RowIndex).HeaderCell.Value = 
            e.Paint(e.ClipBounds, e.PaintParts)

            Dim rect As Drawing.Rectangle
            rect = e.CellBounds
            rect.Width = 16
            rect.Height = 16
            rect.X = rect.X + 3
            rect.Y = rect.Y + 3 'padding in altezza
            e.Graphics.DrawImage(My.Resources.MyResource.info1_16x16, rect)
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        If DataGridView1.Columns(e.ColumnIndex).Name = "btnMyDelete" Then
            Dim temp(0) As Integer
            temp(0) = e.RowIndex
            RaiseEvent MyPreviewDataTableOnDeleteClick(temp)
        Else
            RaiseEvent MyPreviewDataTableOnDubleClick(e.RowIndex)
        End If

    End Sub

   
   
    Private Sub tsAutoSizeColums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsAutoSizeColums.Click
        If _autoSizeColumn Then
            _autoSizeColumn = False
            Me.tsAutoSizeColums.ForeColor = Color.DarkGray
            Me.tsAutoSizeColums.ToolTipText = "Attiva l'auto-size delle colonne"
            Me.DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        Else
            _autoSizeColumn = True
            Me.tsAutoSizeColums.ForeColor = Color.DarkOrange
            Me.tsAutoSizeColums.ToolTipText = "Disattiva l'auto-size delle colonne"
            Me.DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End If
    End Sub
End Class
