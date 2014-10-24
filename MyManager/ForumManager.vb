Imports Microsoft.VisualBasic

Public Class ForumManager
    Inherits Manager

    Public Sub New()
        MyBase.New("forum")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Function reply(ByVal threadId As Long, ByVal testo As String, ByVal userId As Long) As Long
        Return insertPost(threadId, -1, testo, userId, False)
    End Function


    Public Function quote(ByVal threadId As Long, ByVal postId As Long, ByVal testo As String, ByVal userId As Long) As Long
        Return insertPost(threadId, postId, testo, userId, False)
    End Function



    Public Function insertFirstPost(ByVal threadId As Long, ByVal testo As String, ByVal userId As Long) As Long
        Return insertPost(threadId, -1, testo, userId, True)
    End Function


    Public Function insertUser(ByVal userId As Long, ByVal nome As String, ByVal cognome As String, ByVal email As String, ByVal mylogin As String) As Boolean
        'in questo caso la userId non è un contatore in quanto il valore viene gestito da UserManager

        Dim strSQL As String

        strSQL = "INSERT INTO UTENTI ( USER_ID,  NOME, COGNOME, MY_LOGIN, EMAIL )" & _
            " VALUES (@USER_ID, @NOME, @COGNOME, @LOGIN, @EMAIL )"


        Dim oleDbCommand As System.Data.OleDb.OleDbCommand

        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)
        oleDbCommand.Parameters.Add("@USER_ID", OleDb.OleDbType.VarChar).Value = userId
        oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = nome
        oleDbCommand.Parameters.Add("@COGNOME", OleDb.OleDbType.VarChar).Value = cognome
        oleDbCommand.Parameters.Add("@LOGIN", OleDb.OleDbType.VarChar).Value = mylogin
        oleDbCommand.Parameters.Add("@EMAIL", OleDb.OleDbType.VarChar).Value = email


        oleDbCommand.ExecuteNonQuery()

        Return True
    End Function


    Public Function updateEmail(ByVal userId As Long, ByVal email As String) As Boolean
        _strSql = "UPDATE UTENTI SET DATE_MODIFIED = NOW ,EMAIL = @email " & _
                                                     " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = _strSql

        Me._addParameter(command, "@email", email)
        Me._executeNoQuery(command)

        Return True
    End Function



    ''' <summary>
    ''' Funzione di servizio per modificare il contenuto di un Post. Non si dovrebbe fare!!!
    ''' Per questo motivo non passiamo l'id dell'utente e tanto meno aggiorniamo la data di modifica
    ''' </summary>
    Public Function updatePost(ByVal postId As Long, ByVal testo As String) As Boolean
        Dim strSQL As String = "UPDATE POST SET TESTO = @TESTO" & _
                                                " WHERE POST_ID=" & postId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        oleDbCommand.Parameters.Add("@TESTO", OleDb.OleDbType.VarChar).Value = testo
        oleDbCommand.ExecuteNonQuery()

        Return True
    End Function





    'post che fa rifermento ad un altro post = 'Quote'
    Private Function insertPost(ByVal threadId As Long, ByVal postId As Long, ByVal testo As String, ByVal userId As Long, ByVal isFirstPostOfThread As Boolean) As Long
        Dim sqlQuery As String = ""

        If (postId = -1) Then
            'si tratta di un reply
            sqlQuery = "INSERT INTO POST ( FK_POST_ID, TESTO, FK_USER_ID, FK_THREAD_ID )" & _
                   " VALUES ( NULL, '" & testo.Replace("'", "''") & "', " & userId & ", " & threadId & " )"
        Else
            'si tratta di un quote
            sqlQuery = "INSERT INTO POST ( FK_POST_ID, TESTO, FK_USER_ID, FK_THREAD_ID )" & _
                   " VALUES ( " & postId & ", '" & testo.Replace("'", "''") & "', " & userId & ", " & threadId & " )"
        End If


        If (isFirstPostOfThread) Then
            sqlQuery = "INSERT INTO POST ( FK_POST_ID, TESTO, FK_USER_ID, FK_THREAD_ID,  isFirstPostOfThread )" & _
                  " VALUES ( NULL, '" & testo.Replace("'", "''") & "', " & userId & ", " & threadId & ", true )"
        End If

        Dim command As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)
        command.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function


    Private Function getUser(ByVal user_id As Long) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT * FROM UTENTI WHERE USER_ID = " & user_id

        Return _fillDataSet(sqlQuery)
    End Function

    'restituisco i dati dell'utente che ha creato il THREAD
    Public Function getUserFromThread(ByVal threadId As Long) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT UTENTI.* FROM THREAD  left join UTENTI  on  UTENTI.user_id = thread.fk_user_id WHERE THREAD_ID = " & threadId

        Return _fillDataSet(sqlQuery)
    End Function


    'restituisco i dati dell'utente che ha creato il POST
    Public Function getUserFromPost(ByVal postId As Long) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT UTENTI.* FROM POST  left join UTENTI  on  UTENTI.user_id = POST.fk_user_id WHERE POST_ID = " & postId

        Return _fillDataSet(sqlQuery)
    End Function




    Public Function insertThread(ByVal forumId As Long, ByVal nome As String, ByVal userId As Long) As Long
        Dim sqlQuery As String = ""

        sqlQuery = "INSERT INTO THREAD ( FK_FORUM_ID, NOME, FK_USER_ID )" & _
                   " VALUES ( " & forumId & ", '" & nome.Replace("'", "''") & "', " & userId & " )"

        Dim command As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)
        command.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function


    Public Function insertForum(ByVal nome As String, ByVal descrizione As String) As Long
        Return insertForum(nome, descrizione, -1)
    End Function

    Public Function insertForum(ByVal nome As String, ByVal descrizione As String, ByVal discussion_id As Long) As Long
        Dim sqlQuery As String = ""

        ' se non viene passata un tipo di discussione a cui associare il forum allora viene impostata quella di defalut = 1 = Forum
        If (discussion_id = -1) Then
            discussion_id = 1
        End If

        sqlQuery = "INSERT INTO FORUM (NOME, DESCRIZIONE, fk_discussion_id  )" & _
                   " VALUES (  '" & nome.Replace("'", "''") & "', '" & descrizione.Replace("'", "''") & "' , " & discussion_id & ")"

        Dim command As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)
        command.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...

        Return _getIdentity()
    End Function






    Public Function getPost(ByVal postId As Long) As Data.DataSet
        Dim sqlQuery As String = ""

        sqlQuery = "SELECT UTENTI.my_login, POST.* FROM POST LEFT JOIN UTENTI ON POST.FK_user_ID = UTENTI.USER_ID WHERE POST_ID = " & postId
        Return _fillDataSet(sqlQuery)
    End Function



    'Public Function getMercatinoList(ByVal discussionId As Long, ByVal ordinamento As String) As Data.DataSet
    '    Dim command As New System.Data.OleDb.OleDbCommand("listaForumMercatino", _connection)
    '    command.CommandType = CommandType.StoredProcedure
    '    ' command.Parameters.Add(New System.Data.OleDb.OleDbParameter("THREADID", OleDb.OleDbType.Numeric).Value = threadId)

    '    Dim objAdap As New System.Data.OleDb.OleDbDataAdapter
    '    Dim ds As New Data.DataSet

    '    Try
    '        objAdap = New System.Data.OleDb.OleDbDataAdapter(command)
    '        objAdap.Fill(ds)
    '    Finally
    '        If Not IsNothing(objAdap) Then
    '            objAdap.Dispose()
    '            objAdap = Nothing
    '        End If
    '        If Not IsNothing(command) Then
    '            command.Dispose()
    '            command = Nothing
    '        End If
    '    End Try




    '    'If (discussionId <> -1) Then
    '    '    sqlQuery &= " where fk_discussion_id = " & discussionId
    '    'End If

    '    'If (ordinamento = "") Then
    '    '    sqlQuery &= " order by nome"
    '    'Else
    '    '    sqlQuery &= " order by " & ordinamento
    '    'End If

    '    'Return fillDataSet(sqlQuery)
    '    Return ds
    'End Function




    Public Function getForumList(ByVal discussionId As Long, ByVal ordinamento As String) As Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "  SELECT discussion.nome as discussione_nome, discussion.descrizione as discussione_desc, forum.forum_id, forum.nome, forum.descrizione, A.NUMERO_THREAD, ( B.NUMERO_POST -  A.NUMERO_THREAD) AS NUMERO_POST " & _
                    " FROM  " & _
                        " forum , discussion, " & _
                        " [SELECT FK_FORUM_ID , COUNT (*) AS NUMERO_THREAD FROM THREAD GROUP BY FK_FORUM_ID]. AS A, " & _
                        " [SELECT FK_FORUM_ID, COUNT (*) AS NUMERO_POST FROM POST LEFT JOIN THREAD ON POST.FK_THREAD_ID = THREAD.THREAD_ID GROUP BY FK_FORUM_ID]. AS B " & _
                    " WHERE b.FK_FORUM_ID=forum.forum_id and a.FK_FORUM_ID=forum.forum_id  and forum.fk_discussion_id=discussion.discussion_id"

        If (discussionId <> -1) Then
            sqlQuery &= " and forum.fk_discussion_id = " & discussionId
        End If


        If (ordinamento = "") Then
            sqlQuery &= " order by forum.nome"
        Else
            sqlQuery &= " order by " & ordinamento
        End If
        Return _fillDataSet(sqlQuery)
    End Function



    Public Function getForumListOLD(ByVal discussionId As Long, ByVal ordinamento As String) As Data.DataSet
        Dim command As New System.Data.OleDb.OleDbCommand("listaForum", _connection)
        command.CommandType = CommandType.StoredProcedure
        ' command.Parameters.Add(New System.Data.OleDb.OleDbParameter("THREADID", OleDb.OleDbType.Numeric).Value = threadId)

        Dim objAdap As New System.Data.OleDb.OleDbDataAdapter
        Dim ds As New Data.DataSet

        Try
            objAdap = New System.Data.OleDb.OleDbDataAdapter(command)
            objAdap.Fill(ds)
        Finally
            If Not IsNothing(objAdap) Then
                objAdap.Dispose()
                objAdap = Nothing
            End If
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try




        'If (discussionId <> -1) Then
        '    sqlQuery &= " where fk_discussion_id = " & discussionId
        'End If

        'If (ordinamento = "") Then
        '    sqlQuery &= " order by nome"
        'Else
        '    sqlQuery &= " order by " & ordinamento
        'End If

        'Return fillDataSet(sqlQuery)
        Return ds
    End Function

    Public Function getThreadList(ByVal forumId As Long, ByVal ordinamento As String) As Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "SELECT thread_id, nome, NUMERO_POST, DATE_LAST_POST " & _
                    "FROM thread LEFT JOIN (" & _
                                        " SELECT FK_THREAD_ID , COUNT (*) -1 AS NUMERO_POST , Max(DATE_ADDED) as DATE_LAST_POST  FROM POST GROUP BY FK_THREAD_ID " & _
                                        ") AS B ON B.FK_THREAD_ID = thread.THREAD_id "
        If (forumId <> -1) Then
            sqlQuery &= " where fk_forum_id = " & forumId
        End If

        If (ordinamento = "") Then
            sqlQuery &= " order by nome"
        Else
            sqlQuery &= " order by " & ordinamento
        End If
        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getPostList(ByVal threadId As Long, ByVal ordinamento As String) As Data.DataSet
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select UTENTI.my_login, UTENTI.user_id, post_id, POST.date_added, testo , fk_post_id, post.isFirstPostOfThread, UTENTI.isModeratore " & _
                " FROM POST LEFT JOIN UTENTI ON POST.FK_user_ID = UTENTI.USER_ID "

        If (threadId <> -1) Then
            sqlQuery &= " where fk_thread_id = " & threadId
        End If

        If (ordinamento = "") Then
            sqlQuery &= " order by POST.date_added ASC"
        Else
            sqlQuery &= " order by " & ordinamento
        End If
        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getLastPostFromThread(ByVal threadId As Long) As Data.DataSet

        Dim sqlQuery As String

        sqlQuery = " SELECT TOP 1 post.post_id, post.date_added, post.testo, post.fk_post_id, utenti.[my_login], utenti.user_id, thread.nome AS THREAD " & _
                " FROM utenti INNER JOIN (thread INNER JOIN post ON thread.thread_id=post.fk_thread_id) ON utenti.user_id=post.fk_user_id " & _
                " WHERE thread.thread_id  =  " & threadId & _
                " ORDER BY post.date_added DESC "


        'Dim command As New System.Data.OleDb.OleDbCommand("lastPostFromThread", _connection)
        'command.CommandType = CommandType.StoredProcedure
        'command.Parameters.Add("@THREADID", OleDb.OleDbType.Integer).Value = threadId


        'Dim objAdap As New System.Data.OleDb.OleDbDataAdapter
        'Dim ds As New Data.DataSet

        'Try

        '    objAdap = New System.Data.OleDb.OleDbDataAdapter(command)

        '    objAdap.Fill(ds)
        'Finally
        '    If Not IsNothing(objAdap) Then
        '        objAdap.Dispose()
        '        objAdap = Nothing
        '    End If
        '    If Not IsNothing(command) Then
        '        command.Dispose()
        '        command = Nothing
        '    End If
        'End Try


        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getLastPostFromForum(ByVal forumId As Long) As Data.DataSet


        Dim sqlQuery As String

        sqlQuery = " SELECT TOP 1 post.post_id, post.date_added, post.testo, post.fk_post_id, post.isFirstPostOfThread, utenti.[my_login], utenti.user_id, thread.nome AS THREAD, thread.thread_id AS THREAD_ID " & _
                    " FROM forum INNER JOIN (utenti INNER JOIN (thread INNER JOIN post ON thread.thread_id=post.fk_thread_id) ON utenti.user_id=post.fk_user_id) ON forum.forum_id=thread.fk_forum_id " & _
                    " WHERE forum.forum_id = " & forumId & _
                    " ORDER BY post.date_added DESC "

        'Dim command As New System.Data.OleDb.OleDbCommand("lastPostFromForum", _connection)
        'command.CommandType = CommandType.StoredProcedure

        'command.Parameters.Add("FORUMID", OleDb.OleDbType.Integer).Value = forumId




        'Dim objAdap As New System.Data.OleDb.OleDbDataAdapter
        'Dim ds As New Data.DataSet

        'Try

        '    objAdap = New System.Data.OleDb.OleDbDataAdapter(command)

        '    objAdap.Fill(ds)
        'Finally
        '    If Not IsNothing(objAdap) Then
        '        objAdap.Dispose()
        '        objAdap = Nothing
        '    End If
        '    If Not IsNothing(command) Then
        '        command.Dispose()
        '        command = Nothing
        '    End If
        'End Try


        Return _fillDataSet(sqlQuery)
    End Function


    Public Function getThreadIdFromPost(ByVal postId As Long) As Long
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select fk_thread_id from post where post_id = " & postId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As Long
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function

    Public Function getForumIdFromThread(ByVal threadId As Long) As Long
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select fk_forum_id from thread where thread_id = " & threadId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As Long
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function

    Public Function getTextFromPost(ByVal postId As Long) As String
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select TESTO from post where post_id = " & postId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As String
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function

    Public Function getThreadName(ByVal threadId As Long) As String
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select NOME from thread where thread_id = " & threadId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As String
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function

    Public Function getForumName(ByVal forumId As Long) As String
        'restiruisce il dettaglio di un Thread ossia l'elenco di tutti i post che lo compongono...
        Dim sqlQuery As String
        sqlQuery = "select NOME from forum where forum_id = " & forumId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As String
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function

    Public Function countPost(ByVal threadId As Long) As Integer
        Dim sqlQuery As String
        sqlQuery = "select count(*) from post where fk_thread_id = " & threadId

        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim risultato As Integer
        risultato = oleDbCommand.ExecuteScalar()
        oleDbCommand.Dispose()

        Return risultato
    End Function





    Public Function deleteThread(ByVal threadId As Long) As Boolean
        Dim sqlQuery As String = ""
        Dim esito As Boolean = False

        Dim command As System.Data.OleDb.OleDbCommand
        command = New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        transaction = Me._connection.BeginTransaction()

        command.Transaction = transaction
        Try

            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro

            sqlQuery = "UPDATE POST SET FK_POST_ID = NULL WHERE fk_thread_ID=" & threadId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM POST WHERE fk_thread_ID=" & threadId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM THREAD WHERE THREAD_ID =" & threadId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            transaction.Commit()

            esito = True
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione di un Thread." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito

    End Function

    Public Function deletePost(ByVal postId As Long) As Boolean
        Dim sqlQuery As String = ""
        Dim esito As Boolean = False

        Dim command As System.Data.OleDb.OleDbCommand
        command = New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        transaction = Me._connection.BeginTransaction()

        command.Transaction = transaction
        Try

            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro

            sqlQuery = "UPDATE POST SET FK_POST_ID = NULL WHERE post_ID=" & postId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM POST WHERE post_ID=" & postId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()


            esito = True

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione di un Thread." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito

    End Function


    'Attenzione da completare!!!
    Public Function deleteForum(ByVal forumId As Long) As Boolean

        Dim sqlQuery As String = ""
        Dim esito As Boolean = False

        Dim command As System.Data.OleDb.OleDbCommand
        command = New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        transaction = Me._connection.BeginTransaction()

        command.Transaction = transaction
        Try

            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro

            Dim temp As String = "( SELECT THREAD_ID FROM THREAD WHERE FK_Forum_ID = " & forumId & " ) "


            sqlQuery = "UPDATE POST SET FK_POST_ID = NULL WHERE fk_thread_ID in " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM POST WHERE fk_thread_ID in " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM THREAD WHERE THREAD_ID in " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM FORUM WHERE FORUM_ID =" & forumId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()


            esito = True

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione di un Forum." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito

    End Function


    'prima di poter cancellare un utente dal db devo poter eliminare tutti i suoi post e thread
    Public Function deleteUser(ByVal userId As Long) As Boolean
        Dim sqlQuery As String = ""
        Dim esito As Boolean = False

        Dim command As System.Data.OleDb.OleDbCommand
        command = New System.Data.OleDb.OleDbCommand(sqlQuery, _connection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        transaction = Me._connection.BeginTransaction()

        command.Transaction = transaction
        Try

            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro

            'resetto i collegamenti a tutti i post che dovrò cancellare 
            sqlQuery = "UPDATE POST SET FK_POST_ID = NULL WHERE FK_POST_ID in (SELECT POST_ID FROM POST WHERE fk_user_ID=" & userId & " )"
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM POST WHERE fk_user_ID=" & userId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()




            'cancello i THREAD
            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro
            Dim temp As String = "( SELECT THREAD_ID FROM THREAD WHERE FK_USER_ID = " & userId & " ) "

            sqlQuery = "UPDATE POST SET FK_POST_ID = NULL WHERE fk_thread_ID in " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM POST WHERE fk_thread_ID IN " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "DELETE * FROM THREAD WHERE THREAD_ID in " & temp
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()




            sqlQuery = "DELETE * FROM UTENTI WHERE USER_ID =" & userId
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            esito = True

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione di un Utente." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito
    End Function

End Class
