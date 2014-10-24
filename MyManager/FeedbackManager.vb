Public Class FeedbackManager
    Inherits Manager

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function insert(ByVal testo As String, ByVal userId As Long, ByVal externalId As Long) As Long
        _strSql = "INSERT INTO feedback (DATE_ADDED, FK_USER_ID , FK_EXTERNAL_ID, TESTO )" & _
            " VALUES ( NOW() , @FK_USER_ID , @FK_EXTERNAL_ID , @TESTO )"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = _strSql

        Me._addParameter(command, "@FK_USER_ID", userId)
        Me._addParameter(command, "@FK_EXTERNAL_ID", externalId)
        Me._addParameter(command, "@TESTO", testo)

        _executeNoQuery(command)
        Return _getIdentity()
    End Function





End Class
