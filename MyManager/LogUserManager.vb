
''' <summary>
''' La Classe si occupa di loggare le azioni di un utente.
''' Ad esempio con LogType = Login logghiamo tutti gli accessi di un utente
''' </summary>
''' <remarks></remarks>

Public Class LogUserManager
    Inherits Manager


    Public Enum LogType
        Login
        ResetPassword
        UpdatePassword
        URL
        Download
        NotAuthorized
        NotAuthenticated
        PageNotFound
        AccountEnable
        AccountDisable
        AccountDelete
    End Enum


    Public Sub New()
        MyBase.New("utenti")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Private Function getNewLogUserId() As Long
        Return Me._getSequence("S_LOG_UTENTE_ID")
    End Function

    Public Function insert(ByVal userId As Long, ByVal tipo As LogUserManager.LogType, ByVal nota As String) As Long
        Dim ctipo As String = tipo.ToString
        'id della login ricavata dalla sequence
        'Dim logUserId As Long = Me.getNewLogUserId
        Dim strSQL As String = ""


        'Per oracle
        'strSQL = "insert into log_utente (log_utente_id, user_id, type, nota, date_added) values " & _
        '               "(?, ?, ?, ?, sysdate)"

        'Per Access
        strSQL = "insert into log_utente (user_id, type, nota)  "
        Dim strSQLParametri As String = " values ("

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        If (userId <> -1) Then
            strSQLParametri &= userId
        Else
            strSQLParametri &= "null "
        End If



        If String.IsNullOrEmpty(tipo.ToString) Then
            strSQLParametri &= ", null "
        Else
            strSQLParametri &= ",@tipo "
            Me._addParameter(command, "@tipo", tipo.ToString)
        End If


        If String.IsNullOrEmpty(nota) Then
            strSQLParametri &= ", null "
        Else
            strSQLParametri &= ",@nota "
            Me._addParameter(command, "@nota", nota)
        End If


        command.CommandText = strSQL & strSQLParametri & " )"
        command.CommandType = CommandType.Text

        command.ExecuteNonQuery()

        Return 0
    End Function

    Public Function insert(ByVal userId As Long, ByVal tipo As LogUserManager.LogType) As Long
        Return insert(userId, tipo, Nothing)
    End Function


End Class
