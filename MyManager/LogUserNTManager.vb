
''' <summary>
''' La Classe si occupa di loggare le azioni di un utente.
''' Ad esempio con LogType = Login logghiamo tutti gli accessi di un utente
''' </summary>
''' <remarks></remarks>

Public Class LogUserNTManager
    Inherits Manager


    Public Enum LogType
        Login
        Logout
        ResetPassword
        UpdatePassword
        URL
        Download
        NotAuthorized
        NotAuthenticated
        CommandInsert
        CommandUpdate
        CommandDelete
    End Enum


    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub


    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Private Function getNewLogUserId() As Long
        Return Me._getSequence("S_LOG_UTENTE_ID")
    End Function


    Public Function insert(ByVal accountNT As String, ByVal tipo As LogUserNTManager.LogType, ByVal azione As String, ByVal nota As String) As Long
        'id della login ricavata dalla sequence
        'Dim logUserId As Long = Me.getNewLogUserId

        Dim strSQL As String
        Dim strSQLParametri As String


        'Per oracle
        'sqlQuery = "insert into log_utente (log_utente_id, user_id, type, nota, date_added) values " & _
        '               "(?, ?, ?, ?, sysdate)"

        'Per Access
        'sqlQuery = "insert into log_utente (user_id, type, nota) values (?, ?, ?)"


        strSQL = "INSERT INTO LOG_UTENTE_NT ( account, type "
        strSQLParametri = " VALUES ( '" & accountNT & "', '" & tipo.ToString & "' "


        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = _connection


        If nota <> "" Then
            strSQL &= ",nota "
            strSQLParametri &= ",@nota "
            Me._addParameter(command, "@nota", nota)
        End If

        If azione <> "" Then
            strSQL &= ",azione "
            strSQLParametri &= ",@azione "
            Me._addParameter(command, "@azione", azione)
        End If


        command.CommandText = strSQL & " ) " & strSQLParametri & " )"
        command.CommandType = CommandType.Text

        command.ExecuteNonQuery()


        'Dim a As String
        'command.CommandText = "SELECT SCOPE_IDENTITY()"
        'a = command.ExecuteScalar().ToString




        ' Return _getIdentity()

        Return -1

    End Function

    Public Function countLog(ByVal accountNT As String)
        Dim strSQL As String
        strSQL = "SELECT Count(*) FROM(Log_Utente_NT) WHERE [date_added]=Date() and [account]='" & accountNT.Trim & "'"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = _connection
        command.CommandText = strSQL
        command.CommandType = CommandType.Text
        Return Me._executeScalar(strSQL)
    End Function


    Public Function insert(ByVal accountNT As String, ByVal tipo As LogUserNTManager.LogType) As Long
        Return insert(accountNT, tipo, Nothing, Nothing)
    End Function

    Public Function insert(ByVal accountNT As String, ByVal tipo As LogUserNTManager.LogType, ByVal nota As String) As Long
        Return insert(accountNT, tipo, Nothing, nota)
    End Function



End Class
