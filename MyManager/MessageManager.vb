Public Class MessageManager
    Inherits Manager


    Public Sub New()
        MyBase.New("message")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function insert(ByVal userId As Long, ByVal testo As String) As Long
        Dim strSQL As String
        strSQL = "INSERT INTO MESSAGE ( USER_ID_ADDED, TESTO)" & _
            " VALUES (@FK_USER_ID, @TESTO)"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection

        Me._addParameter(command, "@USER_ID_ADDED ", userId)
        Me._addParameter(command, "@TESTO", testo)
       
        Me._executeNoQuery(command)
    End Function



    Public Function update(ByVal userId As Long, ByVal testo As String, ByVal messageId As Long) As Long

        Dim strSQL As String = "UPDATE MESSAGE SET DATE_MODIFIED = GetDate() " & _
                 ",USER_ID_MODIFIED = @USER_ID " & _
                 ",TESTO = @TESTO " & _
                 " WHERE MESSAGE_ID =  " & messageId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        Me._addParameter(command, "@USER_ID ", userId)
        Me._addParameter(command, "@TESTO ", testo)
    
        command.CommandText = strSQL
        
        Me._executeNoQuery(command)
    End Function


    Public Function getMessage(ByVal messageId As Long) As Data.DataTable
        Dim sqlQuery As String = "SELECT * FROM MESSAGE WHERE message_id = " & messageId
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function

    Public Function getMessageText(ByVal messageId As Long) As String
        Dim sqlQuery As String = "SELECT testo FROM MESSAGE WHERE message_id = " & messageId
        Return _executeScalar(sqlQuery).ToString
    End Function

End Class
