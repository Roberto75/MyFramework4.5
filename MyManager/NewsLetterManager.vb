Imports Microsoft.VisualBasic

Public Class NewsLetterManager
    Inherits Manager

    Public Sub New()
        MyBase.New("rubrica")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function copyNewsLetter(ByVal newsletterId As Long) As Long
        Dim sqlQuery As String
        sqlQuery = "select * from NEWSLETTER WHERE newsletter_Id = " & newsletterId

        Dim dataSet As System.Data.DataSet = _fillDataSet(sqlQuery)
        Dim row As System.Data.DataRow
        row = dataSet.Tables(0).Rows(0)


        Return (insert("Copia di " & row("oggetto"), IIf(IsDBNull(row("descrizione")), "", row("descrizione")), IIf(IsDBNull(row("MITTENTE_INDIRIZZO")), "", row("MITTENTE_INDIRIZZO")), IIf(IsDBNull(row("MITTENTE_ALIAS")), "", row("MITTENTE_ALIAS")), False))
    End Function



    Public Function getElencoNewsLetter() As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select newsletter_id, date_added, oggetto, isPubblica from NEWSLETTER order by date_added desc"


        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getComboNewsLetter() As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select newsletter_id, oggetto from NEWSLETTER WHERE isPubblica = true "

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getNewsLetter(ByVal newsletterId As Long) As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select * from NEWSLETTER WHERE newsletter_Id = " & newsletterId

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function delete(ByVal newsletter_id As Long) As Boolean

        Dim sqlQuery As String = ""
        Dim esito As Boolean = False

        Dim command As System.Data.OleDb.OleDbCommand
        command = New System.Data.OleDb.OleDbCommand(sqlQuery, mConnection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        transaction = Me.mConnection.BeginTransaction()

        command.Transaction = transaction
        Try

            'devo resettare i collegamenti altrimenti non posso eseguire la cancellazione i
            'quanto ci sono POST correlati tra loro


            sqlQuery = "DELETE * FROM NEWSLETTER WHERE NEWSLETTER_ID=" & newsletter_id
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()


            esito = True

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione della newslwtter." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito

    End Function

    Public Function insert(ByVal oggetto As String, _
            ByVal descrizione As String, _
            ByVal mittente_indirizzo As String, _
            ByVal mittente_alias As String, _
            ByVal isPubblica As Boolean _
        ) As Long

        Dim strSQL As String



        strSQL = "INSERT INTO NEWSLETTER ( OGGETTO, DESCRIZIONE, MITTENTE_INDIRIZZO, MITTENTE_ALIAS, isPubblica )" & _
            " VALUES (@OGGETTO, @HTML, @MITTENTE_INDIRIZZO, @MITTENTE_ALIAS, @IS_PUBBLICA)"


        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
        oleDbCommand.Parameters.Add("@OGGETTO", OleDb.OleDbType.VarChar).Value = oggetto
        oleDbCommand.Parameters.Add("@DESCRIZIONE", OleDb.OleDbType.VarChar).Value = descrizione
        oleDbCommand.Parameters.Add("@MITTENTE_INDIRIZZO", OleDb.OleDbType.VarChar).Value = mittente_indirizzo
        oleDbCommand.Parameters.Add("@MITTENTE_ALIAS", OleDb.OleDbType.VarChar).Value = mittente_alias
        oleDbCommand.Parameters.Add("@IS_PUBBLICA", OleDb.OleDbType.Boolean).Value = isPubblica


        oleDbCommand.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function




    Public Function getContattoId(ByVal userId As Long) As Long
        Dim sqlquery As String

        sqlquery = "SELECT CONTATTO_ID FROM CONTATTI WHERE USER_ID = " & userId

        Dim dataSet As System.Data.DataSet
        dataSet = Me._fillDataSet(sqlquery)

        If (dataSet.Tables(0).Rows.Count > 1) Then
            Dim ex As Exception
            ex = New Exception("Nella rubrica ci sono più contatti con la USER_ID = " & userId)
            MyManager.MailManager.send(ex)
            Throw ex
        End If

        Return CLng(dataSet.Tables(0).Rows(0)("CONTATTO_ID"))
    End Function





    Public Function unsubscribe(ByVal userId As Long, ByVal newsLetterId As Long) As Boolean

        'dall USER_ID risalgo al CONTATTO_ID
        Dim contattoId As Long = getContattoId(userId)

        Dim strSQL As String = "UPDATE CONTATTO_NEWSLETTER SET DATE_UNSUBSCRIBE = NOW WHERE DATE_UNSUBSCRIBE = NULL AND  CONTATTO_ID=" & contattoId

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, mConnection)

        Try
            oleDbCommand.ExecuteNonQuery()
        Finally
            oleDbCommand.Dispose()
        End Try
        Return True
    End Function



    Public Function update(ByVal newsletterId As Long, ByVal oggetto As String, _
            ByVal descrizione As String, _
            ByVal mittente_indirizzo As String, _
            ByVal mittente_alias As String, _
            ByVal isPubblica As Boolean) As Boolean
        Dim esito As Boolean = False



        Dim strSQL As String = "UPDATE NEWSLETTER SET DATE_MODIFIED = NOW "

        strSQL &= ", OGGETTO = @OGGETTO "
        strSQL &= ", DESCRIZIONE = @DESCRIZIONE "
        strSQL &= ", MITTENTE_INDIRIZZO = @MITTENTE_INDIRIZZO "
        strSQL &= ", MITTENTE_ALIAS = @MITTENTE_ALIAS "
        strSQL &= ", isPubblica = @isPubblica "

        strSQL &= " WHERE NEWSLETTER_ID=" & newsletterId


        Dim oleDbCommand As System.Data.OleDb.OleDbCommand

        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
        oleDbCommand.Parameters.Add("@OGGETTO", OleDb.OleDbType.VarChar).Value = oggetto
        oleDbCommand.Parameters.Add("@DESCRIZIONE", OleDb.OleDbType.VarChar).Value = descrizione
        oleDbCommand.Parameters.Add("@MITTENTE_INDIRIZZO", OleDb.OleDbType.VarChar).Value = mittente_indirizzo
        oleDbCommand.Parameters.Add("@MITTENTE_ALIAS", OleDb.OleDbType.VarChar).Value = mittente_alias
        oleDbCommand.Parameters.Add("@IS_PUBBLICA", OleDb.OleDbType.Boolean).Value = isPubblica


        oleDbCommand.ExecuteNonQuery()

        Return True
    End Function




End Class
