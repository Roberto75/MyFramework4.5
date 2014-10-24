Public Class LogSessionManager
    Inherits Manager

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub


    Public Function sessionStart(ByVal userId As Long, ByVal sessionId As String) As Long
        _strSql = "insert into LOG_SESSION (user_id, session_id ) values ( @userId, @sessionId )"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = _strSql

        Me._addParameter(command, "@userId", userId)
        Me._addParameter(command, "@sessionId", sessionId)

        Me._executeNoQuery(command)
        Return 0
    End Function


    Public Function sessionEnd(ByVal userId As Long, ByVal sessionId As String) As Long
        _strSql = "UPDATE LOG_SESSION SET DATE_END = getDate() " & _
                    "WHERE (user_id =@userId and session_id= @sessionId and DATE_END  is NULL)"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = _strSql

        Me._addParameter(command, "@userId", userId)
        Me._addParameter(command, "@sessionId", sessionId)

        'restituisce il numero di record aggiornati
        Return Me._executeNoQuery(command)
    End Function


    Public Function sessionReset(ByVal userId As Long) As Long
        _strSql = "UPDATE LOG_SESSION SET DATE_END = getDate() " & _
                   "WHERE (user_id =@userId and DATE_END  is NULL)"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = _strSql

        Me._addParameter(command, "@userId", userId)

        'restituisce il numero di record aggiornati
        Return Me._executeNoQuery(command)
    End Function




End Class
