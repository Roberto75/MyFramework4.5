Public Class UcPreviewDataTableWithFilter



    Private _dataTable As DataTable


    Public Function _init(ByVal dt As DataTable) As Boolean
        _dataTable = dt

        Dim dataSource As New BindingSource(dt, Nothing)
        Me.DataGridView1.DataSource = dataSource
    End Function



    Public Sub _clear()
        'DataGridView1.Rows.Clear()
        'DataGridView1.Columns.Clear()
        DataGridView1.DataSource = Nothing

        _dataTable = Nothing
    End Sub



   

End Class
