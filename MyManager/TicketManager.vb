Public Class TicketManager
    Inherits Manager

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub


    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function getTicketTypeId(ByVal ticketId As Long) As Long
        Dim sqlQuery As String

        sqlQuery = "SELECT TICKET_TYPE_ID FROM TICKET where ticket_ID = " & tickeTId
        Return Long.Parse(_executeScalar(sqlQuery))
    End Function




    Public Function existTicketOpen(ByVal userIdCliente As String, ByVal ticketType As Int16) As Long
        'verifico che per questo CLIENTE non ci sia già un ticket APERTO di questo TIPO
        'se non esiste vine restituito 0 
        Dim strSQL As String
        strSQL = "select ticket_id from Ticket where ticket_status_id = 'OPEN' and user_id_cliente = @user_id_cliente and ticket_type_id= @ticket_type_id"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection

        Me._addParameter(command, "@user_id_cliente", userIdCliente)
        Me._addParameter(command, "@ticket_type_id", ticketType)

        Dim risultato As String
        risultato = Me._executeScalar(command)
        If String.IsNullOrEmpty(risultato) Then
            Return 0
        Else
            Return Long.Parse(risultato)
        End If
    
    End Function

    Public Function insertNewTicket(ByVal userIdOperatore As String, ByVal userIdCliente As String, ByVal descrizione As String, ByVal testo As String, ByVal ticketType As Int16) As Long
        Dim strSQL As String
        strSQL = "INSERT INTO TICKET ( USER_ID_CLIENTE, NOME, TICKET_TYPE_ID, TICKET_STATUS_ID , DATE_LAST_MODIFIED)" & _
            " VALUES (@USER_ID_CLIENTE, @NOME, @TICKET_TYPE_ID, 'OPEN', GETDATE())"

        '*** STATO = APERTO
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection

        Me._addParameter(command, "@USER_ID_CLIENTE", userIdCliente)
        Me._addParameter(command, "@NOME", descrizione)
        Me._addParameter(command, "@TICKET_TYPE_ID", ticketType)


        Dim newTicketId As Long
        Me._executeNoQuery(command)
        newTicketId = Me._getIdentity()


        'aggiungo il testo ...
        strSQL = "INSERT INTO TICKET_POST ( isFirstPost, USER_ID_OPERATORE, TICKET_ID, NOTE )" & _
                   " VALUES (1, @USER_ID_OPERATORE, @TICKET_ID, @NOTE )"
        command.Parameters.Clear()
        command.CommandText = strSQL
        Me._addParameter(command, "@USER_ID_OPERATORE", userIdOperatore)
        Me._addParameter(command, "@TICKET_ID", newTicketId)
        Me._addParameter(command, "@NOTE", testo)

        Me._executeNoQuery(command)


        Return newTicketId
    End Function


    Public Function reply(ByVal userIdOperatore As String, ByVal ticket_id As Long, ByVal testo As String) As Long
        ' ByVal ticket_post_id As Long,
        Dim newPost As Long
        'operatore risponde ad un ticket già esistente ...
        Dim strSQL As String
        strSQL = "INSERT INTO TICKET_POST ( isFirstPost, USER_ID_OPERATORE, TICKET_ID, NOTE )" & _
                    " VALUES (0, @USER_ID_OPERATORE, @TICKET_ID, @NOTE )"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection

        Me._addParameter(command, "@USER_ID_OPERATORE", userIdOperatore)
        Me._addParameter(command, "@TICKET_ID", ticket_id)
        Me._addParameter(Command, "@NOTE", testo)

        Me._executeNoQuery(command)

        newPost = Me._getIdentity()


        strSQL = "UPDATE TICKET SET  DATE_LAST_MODIFIED = GETDATE() WHERE TICKET_ID =  " & ticket_id
        Me._executeNoQuery(strSQL)

        Return newPost



    End Function


    Public Function getTicket(ByVal ticketId As Long) As System.Data.DataSet
        Dim sqlQuery As String

        sqlQuery = "SELECT * FROM V_ELENCO_TICKET where ticket_ID = " & ticketId
        Return _fillDataSet(sqlQuery)

    End Function

    
    Public Function getLastPost(ByVal ticketId As Long) As System.Data.DataTable
        Dim strSQL As String
        strSQL = "select ticket_post_id , max (date_added) " & _
                        " from ticket_post " & _
                        " where ticket_id = @TICKET_ID " & _
                        " group by ticket_post_id "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection

        Me._addParameter(command, "@TICKET_ID", ticketId)

        Return Me._fillDataSet(command).Tables(0)
    End Function


    Public Function closeTicket(ByVal ticketId As Long) As Boolean

        Dim strSQL As String = "UPDATE TICKET SET DATE_CLOSED = GETDATE(), DATE_LAST_MODIFIED = GETDATE(), TICKET_STATUS_ID = 'CLOSED' WHERE TICKET_ID = @TICKET_ID "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me._connection


        Me._addParameter(command, "@TICKET_ID", ticketId)

        Me._executeNoQuery(command)

        Return True

    End Function
End Class


Public Class TicketEventArgs
    Inherits EventArgs

    Public _ticketId As Long

    Public Sub New(ByVal ticketId As Long)
        Me._ticketId = ticketId
    End Sub

End Class