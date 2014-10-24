Public Class LogHttpRequestManager
    Inherits Manager


    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    'Public Function insertAnonymous(ByVal request As System.Web.HttpRequest) As Long
    '    Return insert(request, -1)
    'End Function

    'Public Function insert(ByVal request As System.Web.HttpRequest, ByVal userId As Long) As Long

    'End Function
End Class
