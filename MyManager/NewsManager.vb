Public Class NewsManager
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


    Public Function getNews(ByVal newsId As Long) As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select titolo, sottotitolo, html from NEWS WHERE news_Id = " & newsId

        Return _fillDataSet(sqlQuery)
    End Function


    Public Sub addClick(ByVal newsId As Long)
        Dim sqlQuery As String = "UPDATE NEWS SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 " & _
                                            " WHERE NEWS_ID=" & newsId
        Me.mExecuteNoQuery(sqlQuery)
    End Sub



    Public Function getBox(ByVal numero As Int16) As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select TOP " & numero & "  news_id, date_added, date_publisched, titolo, sottotitolo from NEWS WHERE ISPUBBLICA = TRUE  "
        sqlQuery &= " order by date_added desc"

        Return _fillDataSet(sqlQuery)
    End Function


    Public Function getElencoNews() As System.Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "select news_id, date_added, date_publisched, titolo, sottotitolo, isPubblica from NEWS "
        sqlQuery &= " order by date_added desc"

        Return _fillDataSet(sqlQuery)
    End Function


    Public Function insert(ByVal titolo As String, ByVal sottotitolo As String, _
           ByVal bodyHTML As String, _
           ByVal isPubblica As Boolean _
       ) As Long

        Dim strSQL As String


        strSQL = "INSERT INTO NEWS ( TITOLO, SOTTOTITOLO, HTML, isPubblica )" & _
            " VALUES (@TITOLO, @SOTTOTITOLO, @HTML, @IS_PUBBLICA )"



        Dim oleDbCommand As System.Data.OleDb.OleDbCommand

        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
        oleDbCommand.Parameters.Add("@TITOLO", OleDb.OleDbType.VarChar).Value = titolo
        oleDbCommand.Parameters.Add("@SOTTOTITOLO", OleDb.OleDbType.VarChar).Value = sottotitolo
        oleDbCommand.Parameters.Add("@HTML", OleDb.OleDbType.VarChar).Value = bodyHTML
        oleDbCommand.Parameters.Add("@IS_PUBBLICA", OleDb.OleDbType.Boolean).Value = isPubblica


        Me.mExecuteNoQuery(oleDbCommand)



        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()

    End Function


    Public Function delete(ByVal news_id As Long) As Boolean

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


            sqlQuery = "DELETE * FROM NEWS WHERE NEWS_ID=" & news_id
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()


            esito = True

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw New ApplicationException("Errore durante la cancellazione della news." & vbCrLf & "Impossibile eseguire la query: " & sqlQuery, ex)
        Finally
            transaction = Nothing
            command.Dispose()
            command = Nothing
        End Try

        Return esito

    End Function


    Public Function copyNews(ByVal newsId As Long) As Long
        Dim sqlQuery As String
        sqlQuery = "select * from NEWS WHERE news_Id = " & newsId

        Dim dataSet As System.Data.DataSet = _fillDataSet(sqlQuery)
        Dim row As System.Data.DataRow
        row = dataSet.Tables(0).Rows(0)

        Return (insert("Copia di " & row("titolo"), row("sottotitolo"), row("HTML"), False))
    End Function


End Class
