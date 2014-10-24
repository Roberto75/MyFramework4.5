Public Class BlogManager
    Inherits Manager
    Public Sub New(ByVal connection As System.Data.OleDb.OleDbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub

End Class
