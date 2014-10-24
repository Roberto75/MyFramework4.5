Public Class RubricaManager
    Inherits Manager


   
    Public Enum Fonte
        WebCrawler = 1
        PortaleWeb = 2
        Manuale = 3
    End Enum


    '  Public _fonte As Manager.RubricaManager.Fonte

    Public _nome As String
    Public _userId As String
    Public _cognome As String
    Public _societa As String

    Public _emailPrivata As String

    Public _ufficioIndirizzo As String
    Public _ufficioEmail As String
    Public _ufficioCivico As String
    Public _ufficioCitta As String
    Public _ufficioProvincia As String
    Public _ufficioZona As String
    Public _ufficioCap As String
    Public _ufficioTelefono As String
    Public _ufficioFax As String
    Public _ufficioCellulare As String
    Public _ufficioHTTP As String

    Public _nota As String




    Public Sub New()
        MyBase.New("rubrica")
    End Sub

    'Public Sub New(ByVal connectionName As String)
    '    MyBase.New(connectionName)
    'End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub



   
    Public Function unsubscribe(ByVal contattoId As Long, ByVal newsLetterId As Long) As Boolean
        Dim strSQL As String = "UPDATE CONTATTO_NEWSLETTER SET DATE_UNSUBSCRIBE = NOW WHERE DATE_UNSUBSCRIBE = NULL AND  CONTATTO_ID=" & contattoId

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        Try
            oleDbCommand.ExecuteNonQuery()
        Finally
            oleDbCommand.Dispose()
        End Try
        Return True
    End Function



    Public Function subscribe(ByVal contattoId As Long, ByVal listaNewsLetter As ArrayList) As Boolean
        Dim esito As Boolean = False
        Dim strSQL As String = ""

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        Dim transaction As System.Data.OleDb.OleDbTransaction
        ' inzio la transazione... 
        transaction = Me._connection.BeginTransaction()

        Try
            oleDbCommand.Transaction = transaction

            ' se ci sono newsletter da associare ...
            If Not IsNothing(listaNewsLetter) Then
                'non posso cancellare fisicamnte i record altrimeni mi perdo le newslettes da cui l'utente si era cancellato
                'strSQL = "DELETE * FROM CONTATTO_NEWSLETTER WHERE CONTATTO_ID=" & contattoId
                strSQL = "UPDATE CONTATTO_NEWSLETTER SET DATE_UNSUBSCRIBE = NOW WHERE DATE_UNSUBSCRIBE = NULL AND  CONTATTO_ID=" & contattoId

                oleDbCommand.CommandText = strSQL
                oleDbCommand.ExecuteNonQuery()

                Dim i As Int16
                For i = 0 To listaNewsLetter.Count - 1
                    strSQL = "INSERT INTO CONTATTO_NEWSLETTER (NEWSLETTER_ID, CONTATTO_ID) VALUES (" & listaNewsLetter(i) & ", " & contattoId & ")"
                    oleDbCommand.CommandText = strSQL
                    Try
                        oleDbCommand.ExecuteNonQuery()
                    Catch ex As OleDb.OleDbException
                        'se esite già il record vuol dire che prima di era cancellato dalla newsletter e ora ci si rimette
                        If ex.ErrorCode = -2147217873 Then
                            strSQL = "UPDATE CONTATTO_NEWSLETTER SET DATE_UNSUBSCRIBE = NULL WHERE NEWSLETTER_ID =" & listaNewsLetter(i) & "AND CONTATTO_ID=" & contattoId
                            oleDbCommand.CommandText = strSQL
                            oleDbCommand.ExecuteNonQuery()
                        Else
                            Throw ex
                        End If
                    End Try
                Next
            End If

            transaction.Commit()
            esito = True
        Catch ex As Exception
            transaction.Rollback()
            Throw ex
        Finally
            oleDbCommand.Dispose()
            oleDbCommand = Nothing
        End Try
        Return esito
    End Function




    Public Function insertContattoFromPortaleWeb(ByVal userId As Long) As Long
        Me._userId = userId
        Return insertContatto(Fonte.PortaleWeb)
    End Function

    Public Function insertContatto() As Long
        Return insertContatto(Fonte.Manuale)
    End Function

    Public Function insertContatto(ByVal fonte As MyManager.RubricaManager.Fonte) As Long
        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO CONTATTI (FK_FONTE "

        strSQLParametri = " VALUES ( "


        '***Per limportazione devo controolare la fonte!!!!

        If fonte = 0 Or IsNothing(fonte) Then
            Throw New ApplicationException("La fonte deve essere obbiligatoria")
        End If

        strSQLParametri &= fonte

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)


        If _userId <> "" Then
            strSQL &= ",USER_ID "
            strSQLParametri &= ",@USER_ID "
            oleDbCommand.Parameters.Add("@USER_ID", OleDb.OleDbType.Numeric).Value = _userId
        End If

        If _nome <> "" Then
            strSQL &= ",NOME "
            strSQLParametri &= ",@NOME "
            oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = _nome
        End If

        If _cognome <> "" Then
            strSQL &= ",COGNOME "
            strSQLParametri &= ",@COGNOME "
            oleDbCommand.Parameters.Add("@COGNOME", OleDb.OleDbType.VarChar).Value = _cognome
        End If

        If _societa <> "" Then
            strSQL &= ",SOCIETA "
            strSQLParametri &= ",@SOCIETA "
            oleDbCommand.Parameters.Add("@SOCIETA", OleDb.OleDbType.VarChar).Value = _societa
        End If


        If _emailPrivata <> "" Then
            strSQL &= ",EMAIL_PRIVATA "
            strSQLParametri &= ",@EMAIL_PRIVATA "
            oleDbCommand.Parameters.Add("@EMAIL_PRIVATA", OleDb.OleDbType.VarChar).Value = _emailPrivata
        End If


        If _nota <> "" Then
            strSQL &= ",NOTA "
            strSQLParametri &= ",@NOTA "
            oleDbCommand.Parameters.Add("@NOTA", OleDb.OleDbType.VarChar).Value = _nota
        End If

        If _ufficioIndirizzo <> "" Then
            strSQL &= ",UFFICIO_INDIRIZZO "
            strSQLParametri &= ",@UFFICIO_INDIRIZZO "
            oleDbCommand.Parameters.Add("@UFFICIO_INDIRIZZO", OleDb.OleDbType.VarChar).Value = _ufficioIndirizzo
        End If

        If _ufficioEmail <> "" Then
            strSQL &= ",UFFICIO_EMAIL "
            strSQLParametri &= ",@UFFICIO_EMAIL "
            oleDbCommand.Parameters.Add("@UFFICIO_EMAIL", OleDb.OleDbType.VarChar).Value = _ufficioEmail
        End If

        If _ufficioCivico <> "" Then
            strSQL &= ",UFFICIO_CIVICO "
            strSQLParametri &= ",@UFFICIO_CIVICO "
            oleDbCommand.Parameters.Add("@UFFICIO_CIVICO", OleDb.OleDbType.VarChar).Value = _ufficioCivico
        End If

        If _ufficioCitta <> "" Then
            strSQL &= ",UFFICIO_CITTA "
            strSQLParametri &= ",@UFFICIO_CITTA "
            oleDbCommand.Parameters.Add("@UFFICIO_CITTA", OleDb.OleDbType.VarChar).Value = _ufficioCitta
        End If

        If _ufficioProvincia <> "" Then
            strSQL &= ",UFFICIO_PROVINCIA "
            strSQLParametri &= ",@UFFICIO_PROVINCIA "
            oleDbCommand.Parameters.Add("@UFFICIO_PROVINCIA", OleDb.OleDbType.VarChar).Value = _ufficioProvincia
        End If

        If _ufficioZona <> "" Then
            strSQL &= ",UFFICIO_ZONA "
            strSQLParametri &= ",@UFFICIO_ZONA "
            oleDbCommand.Parameters.Add("@UFFICIO_ZONA", OleDb.OleDbType.VarChar).Value = _ufficioZona
        End If


        If _ufficioCap <> "" Then
            strSQL &= ",UFFICIO_CAP "
            strSQLParametri &= ",@UFFICIO_CAP "
            oleDbCommand.Parameters.Add("@UFFICIO_CAP", OleDb.OleDbType.VarChar).Value = _ufficioCap
        End If

        If _ufficioTelefono <> "" Then
            strSQL &= ",UFFICIO_TELEFONO "
            strSQLParametri &= ",@UFFICIO_TELEFONO "
            oleDbCommand.Parameters.Add("@UFFICIO_TELEFONO", OleDb.OleDbType.VarChar).Value = _ufficioTelefono
        End If

        If _ufficioFax <> "" Then
            strSQL &= ",UFFICIO_FAX "
            strSQLParametri &= ",@UFFICIO_FAX "
            oleDbCommand.Parameters.Add("@UFFICIO_FAX", OleDb.OleDbType.VarChar).Value = _ufficioFax
        End If

        If _ufficioCellulare <> "" Then
            strSQL &= ",UFFICIO_CELLULARE "
            strSQLParametri &= ",@UFFICIO_CELLULARE "
            oleDbCommand.Parameters.Add("@UFFICIO_CELLULARE", OleDb.OleDbType.VarChar).Value = _ufficioCellulare
        End If


        If _ufficioHTTP <> "" Then
            strSQL &= ",UFFICIO_HTTP "
            strSQLParametri &= ",@UFFICIO_HTTP "
            oleDbCommand.Parameters.Add("@UFFICIO_HTTP", OleDb.OleDbType.VarChar).Value = _ufficioHTTP
        End If


        oleDbCommand.CommandText = strSQL & " ) " & strSQLParametri & " )"

        oleDbCommand.ExecuteNonQuery()


        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function


    Public Function getElencoContatti() As System.Data.DataSet
        Dim sqlQuery As String = "SELECT CONTATTO_ID, COGNOME, CONTATTI.NOME as NOME, SOCIETA, EMAIL_privata , ufficio_email, DATE_ADDED , fonte.nome AS fonte, Fonte.fonte_id " & _
            " FROM CONTATTI, FONTE" & _
            " WHERE contatti.fk_fonte= fonte.fonte_id"

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getComboNewsLetter() As System.Data.DataSet
        Return getComboNewsLetter(False)
    End Function

    Public Function getComboNewsLetterPubbliche() As System.Data.DataSet
        Return getComboNewsLetter(True)
    End Function

    Private Function getComboNewsLetter(ByVal onlyPubbliche As Boolean) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT NEWSLETTER_ID , OGGETTO FROM NEWSLETTER"
        If onlyPubbliche Then
            sqlQuery &= " WHERE isPubblica = true"
        End If

        Return _fillDataSet(sqlQuery)
    End Function


    Public Function getNewsLetterAssociate(ByVal contattoId As Long) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT NEWSLETTER.NEWSLETTER_ID , OGGETTO " & _
            " FROM NEWSLETTER INNER JOIN contatto_newsletter ON NEWSLETTER.newsletter_id = contatto_newsletter.newsletter_id " & _
        " WHERE contatto_id = " & contattoId

        Return _fillDataSet(sqlQuery)
    End Function


    Public Function getElencoGruppiDisponibili() As System.Data.DataSet
        Dim sqlQuery As String = "SELECT GRUPPO_ID, NOME FROM GRUPPI"

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getElencoGruppiAssociati(ByVal contattoId) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT GRUPPI.GRUPPO_ID, NOME FROM GRUPPI " & _
                " LEFT JOIN gruppo_contatto ON gruppi.gruppo_id = gruppo_contatto.gruppo_id " & _
                " WHERE contatto_id =" & contattoId
        Return _fillDataSet(sqlQuery)
    End Function


    Public Function getComboFonte() As System.Data.DataSet
        Dim sqlQuery As String = "SELECT FONTE_ID, NOME FROM FONTE"
        Return _fillDataSet(sqlQuery)
    End Function


    Public Sub updateContatto(ByVal contatto_Id As Long)
        updateContatto(contatto_Id, Nothing)
    End Sub

    Public Sub updateContatto(ByVal contatto_Id As Long, ByVal listaGruppi As System.Collections.ArrayList)
        Dim strSQL As String = "UPDATE CONTATTI SET DATE_MODIFIED = NOW "

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        If _nome <> "" Then
            strSQL &= " ,nome = ?"
            oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = _nome
        End If

        If _cognome <> "" Then
            strSQL &= " ,cognome = ?"
            oleDbCommand.Parameters.Add("@COGNOME", OleDb.OleDbType.VarChar).Value = _cognome
        End If

        If _emailPrivata <> "" Then
            strSQL &= " ,EMAIL_PRIVATA = ?"
            oleDbCommand.Parameters.Add("@EMAIL_PRIVATA", OleDb.OleDbType.VarChar).Value = _emailPrivata
        End If

        If _nota <> "" Then
            strSQL &= " ,NOTA = ?"
            oleDbCommand.Parameters.Add("@NOTA", OleDb.OleDbType.VarChar).Value = _nota
        End If

        If _societa <> "" Then
            strSQL &= " ,SOCIETA = ?"
            oleDbCommand.Parameters.Add("@SOCIETA", OleDb.OleDbType.VarChar).Value = _societa
        End If

        If _ufficioCap <> "" Then
            strSQL &= " ,UFFICIO_CAP = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_CAP", OleDb.OleDbType.VarChar).Value = _ufficioCap
        End If

        If _ufficioCellulare <> "" Then
            strSQL &= " ,UFFICIO_CELLULARE = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_CELLULARE", OleDb.OleDbType.VarChar).Value = _ufficioCellulare
        End If

        If _ufficioProvincia <> "" Then
            strSQL &= " ,UFFICIO_PROVINCIA = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_PROVINCIA", OleDb.OleDbType.VarChar).Value = _ufficioProvincia
        End If

        If _ufficioCitta <> "" Then
            strSQL &= " ,UFFICIO_CITTA = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_CITTA", OleDb.OleDbType.VarChar).Value = _ufficioCitta
        End If

        If _ufficioCivico <> "" Then
            strSQL &= " ,UFFICIO_CIVICO = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_CIVICO", OleDb.OleDbType.VarChar).Value = _ufficioCivico
        End If

        If _ufficioEmail <> "" Then
            strSQL &= " ,UFFICIO_EMAIL = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_EMAIL", OleDb.OleDbType.VarChar).Value = _ufficioEmail
        End If

        If _ufficioFax <> "" Then
            strSQL &= " ,UFFICIO_FAX = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_FAX", OleDb.OleDbType.VarChar).Value = _ufficioFax
        End If

        If _ufficioHTTP <> "" Then
            strSQL &= " ,UFFICIO_HTTP = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_HTTP", OleDb.OleDbType.VarChar).Value = _ufficioHTTP
        End If

        If _ufficioIndirizzo <> "" Then
            strSQL &= " ,UFFICIO_INDIRIZZO = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_INDIRIZZO", OleDb.OleDbType.VarChar).Value = _ufficioIndirizzo
        End If

        If _ufficioTelefono <> "" Then
            strSQL &= " ,UFFICIO_TELEFONO = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_TELEFONO", OleDb.OleDbType.VarChar).Value = _ufficioTelefono
        End If

        If _ufficioZona <> "" Then
            strSQL &= " ,UFFICIO_ZONA = ?"
            oleDbCommand.Parameters.Add("@UFFICIO_ZONA", OleDb.OleDbType.VarChar).Value = _ufficioZona
        End If




        strSQL &= " WHERE CONTATTO_ID=" & contatto_Id




        Dim transaction As System.Data.OleDb.OleDbTransaction
        ' inzio la transazione... 
        transaction = Me._connection.BeginTransaction()

        Try
            oleDbCommand.Transaction = transaction

            oleDbCommand.CommandText = strSQL
            oleDbCommand.ExecuteNonQuery()


            ' se ci sono anche i gruppi da associare ...
            If Not IsNothing(listaGruppi) Then
                strSQL = "DELETE * FROM GRUPPO_CONTATTO WHERE CONTATTO_ID=" & contatto_Id

                oleDbCommand.CommandText = strSQL
                oleDbCommand.ExecuteNonQuery()

                Dim i As Int16
                For i = 0 To listaGruppi.Count - 1
                    strSQL = "INSERT INTO GRUPPO_CONTATTO (GRUPPO_ID, CONTATTO_ID) VALUES (" & listaGruppi(i) & ", " & contatto_Id & ")"

                    oleDbCommand.CommandText = strSQL
                    oleDbCommand.ExecuteNonQuery()
                Next
            End If

            transaction.Commit()
        Catch ex As Exception
            transaction.Rollback()
            Throw ex
        Finally
            Me._connection.Close()
            oleDbCommand.Dispose()
            oleDbCommand = Nothing
        End Try
    End Sub


    Public Sub deleteContatto(ByVal contatto_Id As Long)
        Dim strSQL As String = "DELETE * FROM CONTATTI WHERE CONTATTO_ID=" & contatto_Id
        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, _connection)
        oleDbCommand.ExecuteNonQuery()
    End Sub


    Public Sub deleteAssociazioneGruppi(ByVal contatto_Id As Long)
        Dim strSQL As String = "DELETE * FROM GRUPPO_CONTATTO WHERE CONTATTO_ID=" & contatto_Id
        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, _connection)
        oleDbCommand.ExecuteNonQuery()
    End Sub



    Public Function getContatto(ByVal contattoId As Long) As System.Data.DataSet
        Dim sqlQuery As String = "SELECT * FROM CONTATTI WHERE contatto_id = " & contattoId
        Return _fillDataSet(sqlQuery)
    End Function



   


   
End Class



