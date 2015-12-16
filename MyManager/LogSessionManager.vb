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
        mStrSQL = "insert into LOG_SESSION (user_id, session_id ) values ( @userId, @sessionId )"
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@userId", userId)
        Me.mAddParameter(command, "@sessionId", sessionId)

        Me.mExecuteNoQuery(command)
        Return 0
    End Function


    Public Function sessionEnd(ByVal userId As Long, ByVal sessionId As String) As Long
        mStrSQL = "UPDATE LOG_SESSION SET DATE_END = getDate() " & _
                    "WHERE (user_id =@userId and session_id= @sessionId and DATE_END  is NULL)"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@userId", userId)
        Me.mAddParameter(command, "@sessionId", sessionId)

        'restituisce il numero di record aggiornati
        Return Me.mExecuteNoQuery(command)
    End Function


    Public Function sessionReset(ByVal userId As Long) As Long
        mStrSQL = "UPDATE LOG_SESSION SET DATE_END = getDate() " & _
                   "WHERE (user_id =@userId and DATE_END  is NULL)"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@userId", userId)

        'restituisce il numero di record aggiornati
        Return Me.mExecuteNoQuery(command)
    End Function




End Class
