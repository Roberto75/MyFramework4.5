Public Class MyApplicationManager
    Inherits MyManager.Manager

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub


End Class
