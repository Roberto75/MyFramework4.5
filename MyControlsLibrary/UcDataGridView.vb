Public Class UcDataGridView

    Private mStrSQL As String

    Private _withButtonDelete As Boolean
    Public Property MyButtonDelete() As Boolean
        Get
            Return _withButtonDelete
        End Get
        Set(ByVal value As Boolean)
            _withButtonDelete = value
        End Set
    End Property



    Public Function _init(ByVal strSQL As String) As Boolean
        Me.mStrSQL = strSQL


        Return True
    End Function


    Friend Function _addButtonDelete() As Boolean

        Return False
    End Function


End Class
