Public Class MercatinoManager
    Inherits Manager

    Public Sub New()
        MyBase.New("mercatino")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Enum StatoAnnuncio
        Pubblicato = 1
        OggettoNonPiuDisponibile
        ConclusoConSuccesso
        Altro
        OffLine
        DaCancellare
    End Enum

    Public Enum StatoTrattativa
        Attiva = 1
        NonPiuDiInteresse
        AnnuncioRimosso
        TerminataConSuccesso
        TerminataSenzaSuccesso
        TerminataConFrode
        Altro
    End Enum

    Public Enum TipoAnnuncio
        Vendo = 1
        Compro = 2
        Scambio = 3
    End Enum

    Public Enum TipoOggetto
        Libro = 1
        Informatica = 2
    End Enum

    Public Enum AttributiOggetto
        standard = 0
        genere = 2
        casaEditrice = 4
        autore = 8
    End Enum


    Public _tipoAnnuncio As tipoAnnuncio
    Public _titolo As String 'nome/titolo
    Public _nota As String 'nota/descizione
    Public _categoriaId As Long
    Public _casaEditrice As String
    Public _autore As String
    Public _prezzo As Decimal
    Public _genere As String
    Public _isbn As String
    Public _regione As String
    Public _provincia As String
    Public _photo As New ArrayList

    Public Function getTrattative(ByVal userId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM TRATTATIVA WHERE  DATE_DELETED IS NULL AND  fk_user_id  = " & userId
        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function getTrattative(ByVal userId As Long, ByVal annuncioId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM TRATTATIVA WHERE  DATE_DELETED IS NULL AND  fk_user_id  = " & userId & " AND FK_ANNUNCIO_ID= " & annuncioId
        'in teoria un utente non dovrebbe avere più trattative per uno stesso annuncio
        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function countTrattative(ByVal annuncioId As Long) As Integer
        mStrSQL = "SELECT count(*) FROM TRATTATIVA WHERE  DATE_DELETED IS NULL  AND FK_ANNUNCIO_ID= " & annuncioId
        Return Me.mExecuteScalar(mStrSQL)
    End Function


    Public Function countPhoto(ByVal annuncioId As Long) As Integer
        mStrSQL = "SELECT count(*) FROM PHOTO WHERE  FK_EXTERNAL_ID= " & annuncioId
        Return Me.mExecuteScalar(mStrSQL)
    End Function



    Public Function getTrattativeOnMyAnnuncio(ByVal userId As Long, ByVal annuncioId As Long) As Data.DataTable
        'per questione di sicurezza aggiungo anche ID dell'utente 
        'verifico che effettivamente l'annuncio sio il MIO!
        mStrSQL = "SELECT  FORMAT (TRATTATIVA.date_added,""dd-MM-yyyy"") as date_added " & _
            ", TRATTATIVA.trattativa_id , UTENTI.my_login, TRATTATIVA.stato" & _
        " FROM " & _
         " (TRATTATIVA INNER JOIN UTENTI ON TRATTATIVA.fk_user_id=UTENTI.user_id) INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id" & _
         " WHERE  TRATTATIVA.DATE_DELETED_OWNER IS NULL AND  ANNUNCIO.fk_user_id  = " & userId & " AND FK_ANNUNCIO_ID= " & annuncioId & _
        " order BY TRATTATIVA.date_added DESC "
        Return Me.mFillDataTable(mStrSQL)
    End Function


    Public Function getStatoTrattativa(ByVal trattativaId As Long) As MyManager.MercatinoManager.StatoTrattativa
        mStrSQL = "SELECT STATO FROM TRATTATIVA WHERE TRATTATIVA_ID = " & trattativaId

        Dim stato As String
        stato = Me.mExecuteScalar(mStrSQL)

        Select Case stato
            Case StatoTrattativa.Attiva.ToString
                Return StatoTrattativa.Attiva
            Case StatoTrattativa.AnnuncioRimosso.ToString
                Return StatoTrattativa.AnnuncioRimosso
            Case StatoTrattativa.Altro.ToString
                Return StatoTrattativa.Altro
            Case StatoTrattativa.NonPiuDiInteresse.ToString
                Return StatoTrattativa.NonPiuDiInteresse
            Case StatoTrattativa.TerminataConFrode.ToString
                Return StatoTrattativa.TerminataConFrode
            Case StatoTrattativa.TerminataConSuccesso.ToString
                Return StatoTrattativa.TerminataConSuccesso
            Case StatoTrattativa.TerminataSenzaSuccesso.ToString
                Return StatoTrattativa.TerminataSenzaSuccesso
            Case Else
                Throw New MyManager.ManagerException("Stato trattativa non gestito: " & stato)
        End Select

        'Return [Enum].GetValues(GetType(MyManager.MercatinoManager.StatoTrattativa), Int32.Parse(stato))
        Return stato
    End Function


    Public Function authorizeShowTrattativa(ByVal userId As Long, ByVal trattativaId As Long) As Boolean
        'verifico che user_id possa vedere la trattiva
        mStrSQL = "SELECT count(*) FROM TRATTATIVA INNER JOIN ANNUNCIO ON ANNUNCIO.annuncio_id=TRATTATIVA.fk_annuncio_id " & _
            " WHERE ANNUNCIO.FK_USER_ID = " & userId & " OR TRATTATIVA.FK_USER_ID = " & userId & _
            " AND TRATTATIVA.TRATTATIVA_ID =" & trattativaId

        Return mExecuteScalar(mStrSQL) > 0
    End Function


    Public Function getTrattativeOnMyAnnuncio(ByVal annuncioId As Long) As Data.DataTable
        'per questione di sicurezza aggiungo anche ID dell'utente 
        'verifico che effettivamente l'annuncio sio il MIO!
        mStrSQL = "SELECT TRATTATIVA.date_added, TRATTATIVA.trattativa_id , UTENTI.my_login, TRATTATIVA.stato" & _
        " FROM " & _
        " TRATTATIVA INNER JOIN UTENTI ON TRATTATIVA.fk_user_id=UTENTI.user_id" & _
        " WHERE  TRATTATIVA.DATE_DELETED IS NULL  AND FK_ANNUNCIO_ID= " & annuncioId & _
        " ORDER by TRATTATIVA.date_added  desc"

        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function insertTrattativa(ByVal annuncioId As Long, ByVal userId As Long) As Long
        mStrSQL = "INSERT INTO TRATTATIVA ( FK_USER_ID , FK_ANNUNCIO_ID, STATO )" & _
            " VALUES ( @USER_ID , @ANNUNCIO_ID , @STATO )"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@USER_ID", userId)
        Me.mAddParameter(command, "@ANNUNCIO_ID", annuncioId)
        Me.mAddParameter(command, "@STATO", MercatinoManager.StatoTrattativa.Attiva.ToString())

        mExecuteNoQuery(command)
        Return _getIdentity()
    End Function

    Public Function notificaOwner(ByVal trattativaId As Long, ByVal userId As Long) As Boolean
        mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_OWNER = " & userId & " WHERE trattativa_id = " & trattativaId
        mExecuteNoQuery(mStrSQL)
        Return True
    End Function

    Public Function notificaUser(ByVal trattativaId As Long, ByVal userId As Long) As Boolean
        mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_USER = " & userId & " WHERE trattativa_id = " & trattativaId
        mExecuteNoQuery(mStrSQL)
        Return True
    End Function




    Public Function rispondi(ByVal trattativaId As Long, ByVal userId As Long, ByVal testo As String) As Long
        'una nuova risposta per l'annuncio da parte di un utente
        mStrSQL = "INSERT INTO RISPOSTA ( FK_USER_ID, FK_TRATTATIVA_ID, TESTO , FK_RISPOSTA_ID , OWNER) " & _
                    " VALUES ( @FK_USER_ID ,  @FK_TRATTATIVA_ID , @TESTO , NULL, " & userId & " ) "

        '30/01/2011 VER. 1.0.0.5
        'l'inserimento di una nuova risposta comporta la notifica del messaggio 


        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        mAddParameter(command, "@FK_USER_ID", userId)
        mAddParameter(command, "@FK_TRATTATIVA_ID", trattativaId)
        mAddParameter(command, "@TESTO", testo)

        command.CommandText = mStrSQL

        mExecuteNoQuery(command)
        'Return Me._getIdentity
        Return 1
    End Function

    Public Function rispondi(ByVal trattativaId As Long, ByVal userId As Long, ByVal testo As String, ByVal risposta_id As Long) As Long
        'una nuova risposta per l'annuncio da parte di un utente
        mStrSQL = "SELECT OWNER FROM RISPOSTA WHERE RISPOSTA_ID  = " & risposta_id

        Dim owner As Long
        owner = mExecuteScalar(mStrSQL)

        mStrSQL = "INSERT INTO RISPOSTA ( FK_USER_ID, FK_TRATTATIVA_ID, TESTO , FK_RISPOSTA_ID , OWNER) " & _
                    " VALUES ( @FK_USER_ID ,  @FK_TRATTATIVA_ID , @TESTO , " & risposta_id & ", " & owner & " ) "


        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        mAddParameter(command, "@FK_USER_ID", userId)
        mAddParameter(command, "@FK_TRATTATIVA_ID", trattativaId)
        mAddParameter(command, "@TESTO", testo)

        command.CommandText = mStrSQL

        mExecuteNoQuery(command)

        'Return Me._getIdentity
        Return 1
    End Function

    Public Function insertAnnuncio(ByVal userId As Long) As Long

        If _categoriaId = 0 OrElse IsNothing(_categoriaId) Then
            Throw New ManagerException("La Categoria deve essere obbiligatoria")
        End If

        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO ANNUNCIO ( FK_CATEGORIA_ID , MY_STATO"
        strSQLParametri = " VALUES ( " & _categoriaId & ", '" & MercatinoManager.StatoAnnuncio.Pubblicato.ToString() & "' "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        If userId <> 0 Then
            strSQL &= ",FK_USER_ID "
            strSQLParametri &= ", @FK_USER_ID "
            mAddParameter(command, "@FK_USER_ID", userId)
        End If

        If _tipoAnnuncio > 0 Then
            strSQL &= ",TIPO "
            strSQLParametri &= ", @TIPO "
            mAddParameter(command, "@TIPO", Integer.Parse(_tipoAnnuncio.ToString("d")))
        End If

        If _nota <> "" Then
            strSQL &= ",DESCRIZIONE "
            strSQLParametri &= ", @DESCRIZIONE "
            mAddParameter(command, "@DESCRIZIONE", _nota)
        End If

        If Not String.IsNullOrEmpty(_titolo) Then
            strSQL &= ",NOME "
            strSQLParametri &= ", @NOME "
            mAddParameter(command, "@NOME", _titolo)
        End If

        If Not String.IsNullOrEmpty(_autore) Then
            strSQL &= ",AUTORE "
            strSQLParametri &= ", @AUTORE "
            mAddParameter(command, "@AUTORE", _autore)
        End If

        If Not String.IsNullOrEmpty(_casaEditrice) Then
            strSQL &= ",CASA_EDITRICE "
            strSQLParametri &= ", @CASA_EDITRICE "
            mAddParameter(command, "@CASA_EDITRICE", _casaEditrice)
        End If

        If Not String.IsNullOrEmpty(_genere) Then
            strSQL &= ",GENERE "
            strSQLParametri &= ", @GENERE "
            mAddParameter(command, "@GENERE", _genere)
        End If

        If _prezzo > 0 Then
            strSQL &= ",PREZZO "
            strSQLParametri &= ", @PREZZO "
            mAddParameter(command, "@PREZZO", _prezzo)
        End If

        If _photo.Count > 0 Then
            Dim tempPhoto As String = ""
            For Each photo As String In _photo
                tempPhoto &= photo & ";"
            Next
            strSQL &= ",PHOTO "
            strSQLParametri &= ", @PHOTO "
            mAddParameter(command, "@PHOTO", tempPhoto)
        End If

        '17/10/2010
        If Not String.IsNullOrEmpty(_isbn) Then
            strSQL &= ",ISBN "
            strSQLParametri &= ", @ISBN "
            mAddParameter(command, "@ISBN", _isbn)
        End If

        If Not String.IsNullOrEmpty(_regione) Then
            strSQL &= ",REGIONE "
            strSQLParametri &= ", @REGIONE "
            mAddParameter(command, "@REGIONE", _regione)
        End If

        If Not String.IsNullOrEmpty(_provincia) Then
            strSQL &= ",PROVINCIA "
            strSQLParametri &= ", @PROVINCIA "
            mAddParameter(command, "@PROVINCIA", _provincia)
        End If

        command.CommandText = strSQL & " ) " & strSQLParametri & " )"
        mExecuteNoQuery(command)

        Return _getIdentity()
    End Function

    Public Function deleteALLAnnunci() As Boolean
        mStrSQL = "SELECT ANNUNCIO_ID FROM ANNUNCIO "

        Dim annunci As Data.DataTable
        annunci = mFillDataTable(mStrSQL)

        For Each row As Data.DataRow In annunci.Rows
            deleteAnnuncio(row("ANNUNCIO_ID"), "")
        Next
        Return True
    End Function


    Public Function deleteAnnunciByUserId(ByVal userId As Long, ByVal absoluteServerPath As String) As Boolean
        mStrSQL = "SELECT ANNUNCIO_ID FROM ANNUNCIO WHERE FK_USER_ID = " & userId

        Dim annunci As Data.DataTable
        annunci = mFillDataTable(mStrSQL)

        For Each row As Data.DataRow In annunci.Rows
            deleteAnnuncio(row("ANNUNCIO_ID"), absoluteServerPath)
        Next
        Return True
    End Function


    Public Function deleteUser(ByVal userId As Long, ByVal absoluteServerPath As String) As Long
        deleteAnnunciByUserId(userId, absoluteServerPath)

        mStrSQL = "DELETE * FROM UTENTI WHERE USER_ID = " & userId
        Return mExecuteNoQuery(mStrSQL)
    End Function


    Public Function deleteUser(ByVal userId As Long) As Long
        mStrSQL = "DELETE * FROM UTENTI WHERE USER_ID = " & userId
        Return mExecuteNoQuery(mStrSQL)
    End Function


    Public Function deleteALLUtenti() As Long
        mStrSQL = "DELETE * FROM UTENTI"
        Return mExecuteNoQuery(mStrSQL)
    End Function



    Public Function deleteAnnuncio(ByVal annuncioId As Long, ByVal absoluteServerPath As String) As Boolean
        'Cancellazione fisica
        mStrSQL = "SELECT TRATTATIVA_ID FROM TRATTATIVA WHERE FK_ANNUNCIO_ID=" & annuncioId

        Dim trattative As Data.DataTable
        trattative = mFillDataTable(mStrSQL)

        For Each row As Data.DataRow In trattative.Rows
            mStrSQL = "UPDATE RISPOSTA SET FK_RISPOSTA_ID = NULL WHERE FK_TRATTATIVA_ID=" & row("trattativa_id")
            Me.mExecuteNoQuery(mStrSQL)

            mStrSQL = "DELETE * FROM RISPOSTA WHERE FK_TRATTATIVA_ID=" & row("trattativa_id")
            Me.mExecuteNoQuery(mStrSQL)
        Next

        mStrSQL = "DELETE * FROM TRATTATIVA WHERE FK_ANNUNCIO_ID=" & annuncioId
        Me.mExecuteNoQuery(mStrSQL)


        If Not String.IsNullOrEmpty(absoluteServerPath) Then
            Dim photo As New MyManager.PhotoManager(Me.mConnection)

            mStrSQL = "SELECT PHOTO_ID FROM PHOTO WHERE FK_EXTERNAL_ID=" & annuncioId
            Dim dt As Data.DataTable
            dt = mFillDataTable(mStrSQL)

            For Each row As Data.DataRow In dt.Rows
                photo.deletePhoto(row("PHOTO_ID"), absoluteServerPath)
            Next

            'cancello la directory!
            Dim folder As String
            folder = System.Configuration.ConfigurationManager.AppSettings("mercatino.images.folder")
            folder = absoluteServerPath & folder.Replace("~", "") & annuncioId & "/"
            If IO.Directory.Exists(folder) Then
                Try
                    IO.Directory.Delete(folder, True)
                Catch ex As Exception
                    'lo ignoro
                End Try
            End If
        End If

        mStrSQL = "DELETE * FROM PHOTO WHERE FK_EXTERNAL_ID=" & annuncioId
        Me.mExecuteNoQuery(mStrSQL)

        mStrSQL = "DELETE * FROM ANNUNCIO WHERE ANNUNCIO_ID=" & annuncioId
        Me.mExecuteNoQuery(mStrSQL)
        Return True
    End Function

    Public Function deleteTrattativaLogic(ByVal trattativaId As Long, ByVal userId As Long) As Boolean
        'se la trattativa è già stata cancellata da un utente devo semplicemente
        'aggiornatre lòa data per non farla vedere
        Dim userTrattativa As Long
        mStrSQL = "SELECT FK_USER_ID FROM TRATTATIVA WHERE TRATTATIVA_ID =" & trattativaId
        userTrattativa = Me.mExecuteScalar(mStrSQL)

        If userTrattativa = userId Then
            mStrSQL = "UPDATE TRATTATIVA SET DATE_DELETED = NOW() " & _
                    " WHERE TRATTATIVA_ID = " & trattativaId
        Else
            'si tratta del propriatario dell'annuncio che decide di eliminare la trattaritva
            mStrSQL = "UPDATE TRATTATIVA SET DATE_DELETED_OWNER = NOW() " & _
                  " WHERE TRATTATIVA_ID = " & trattativaId
        End If

        Me.mExecuteNoQuery(mStrSQL)

        Return True
    End Function


    Public Function deleteTrattativaLogic(ByVal trattativaId As Long, ByVal userId As Long, ByVal causale As MyManager.MercatinoManager.StatoTrattativa) As Boolean
        deleteTrattativaLogic(trattativaId, userId)
        updateStatoTrattativa(trattativaId, causale)
        Return True
    End Function

    Public Function updateStatoTrattativa(ByVal trattativaId As Long, ByVal causale As MyManager.MercatinoManager.StatoTrattativa) As Boolean
        mStrSQL = "UPDATE TRATTATIVA SET STATO = '" & causale.ToString & "' " & _
            " WHERE TRATTATIVA_ID = " & trattativaId
        Me.mExecuteNoQuery(mStrSQL)
        Return True
    End Function



    Public Function deleteAnnuncioLogic(ByVal annuncioId As Long, ByVal causale As MyManager.MercatinoManager.StatoAnnuncio _
                                        , ByVal absoluteServerPath As String) As Boolean
        mStrSQL = "UPDATE ANNUNCIO SET DATE_DELETED = NOW() , MY_STATO = '" & causale.ToString & "' WHERE ANNUNCIO_ID = " & annuncioId
        Me.mExecuteNoQuery(mStrSQL)

        'Dim statoTrattativa As MyManager.MercatinoManager.StatoTrattativa

        'If causale = StatoAnnuncio.ConclusoConSuccesso Then
        'StatoTrattativa = MercatinoManager.StatoTrattativa.AnnuncioRimosso
        'End If

        'tutte le trattative collegate all'annuncio vengono notificate 
        mStrSQL = "UPDATE TRATTATIVA SET STATO = '" & MercatinoManager.StatoTrattativa.AnnuncioRimosso.ToString & "' " & _
            " WHERE FK_ANNUNCIO_ID = " & annuncioId
        Me.mExecuteNoQuery(mStrSQL)


        If Not String.IsNullOrEmpty(absoluteServerPath) Then
            Dim photo As New MyManager.PhotoManager(Me.mConnection)

            mStrSQL = "SELECT PHOTO_ID FROM PHOTO WHERE FK_EXTERNAL_ID=" & annuncioId
            Dim dt As Data.DataTable
            dt = mFillDataTable(mStrSQL)

            For Each row As Data.DataRow In dt.Rows
                photo.deletePhoto(row("PHOTO_ID"), absoluteServerPath)
            Next

            'cancello la directory!
            Dim folder As String
            folder = System.Configuration.ConfigurationManager.AppSettings("mercatino.images.folder")
            folder = absoluteServerPath & folder.Replace("~", "") & annuncioId & "/"
            If IO.Directory.Exists(folder) Then
                Try
                    IO.Directory.Delete(folder, True)
                Catch ex As Exception
                    'lo ignoro
                End Try
            End If
        End If
        Return True
    End Function

    Public Function isOwner(ByVal annuncioId As Long, ByVal userId As Long) As Boolean
        mStrSQL = "SELECT COUNT(*) as TOT " & _
                    " FROM ANNUNCIO " & _
                    " WHERE ANNUNCIO.ANNUNCIO_ID = " & annuncioId & _
                    " AND ANNUNCIO.FK_USER_ID = " & userId
        Return mExecuteScalar(mStrSQL) = 1
    End Function


    Public Function getAnnucioIdFromTrattativa(ByVal trattativaId As Long) As Long
        mStrSQL = " SELECT FK_ANNUNCIO_ID  FROM TRATTATIVA WHERE  TRATTATIVA_ID = " & trattativaId
        Return mExecuteScalar(mStrSQL)
    End Function

    Public Function countMyAnnunciSourceExternal(ByVal userId As Long) As Long
        mStrSQL = "SELECT Count(*) AS tot FROM ANNUNCIO WHERE date_deleted Is Null and source_id is not null and FK_user_ID = " & userId & " AND MY_STATO = '" & StatoAnnuncio.Pubblicato.ToString & "' "

        Return mExecuteScalar(mStrSQL)
    End Function




    Public Function countMyAnnunci(ByVal userId As Long) As Long
        mStrSQL = "SELECT Count(*) AS tot FROM ANNUNCIO wHERE date_deleted Is Null and FK_user_ID = " & userId

        Return mExecuteScalar(mStrSQL)
    End Function


    Public Function getAnnunci(ByVal categoriaId As Long) As Data.DataSet
        mStrSQL = "select UTENTI.my_login, UTENTI.user_id, UTENTI.isModeratore, " & _
                        "ANNUNCIO.ANNUNCIO_ID, ANNUNCIO.DATE_ADDED, TIPO,ANNUNCIO.NOME, PREZZO " & _
                    " FROM ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.FK_user_ID = UTENTI.USER_ID " & _
                    " WHERE ANNUNCIO.DATE_DELETED IS NULL"

        If (categoriaId > 0) Then
            mStrSQL &= " and fk_CATEGORIA_id = " & categoriaId
        End If


        mStrSQL &= " order by ANNUNCIO.date_added ASC"
        Return _fillDataSet(mStrSQL)
    End Function


    Public Function isDeletedAnnuncio(ByVal annuncioId As Long) As Boolean
        mStrSQL = "SELECT DATE_DELETED FROM ANNUNCIO WHERE ANNUNCIO_ID =" & annuncioId

        If String.IsNullOrEmpty(mExecuteScalar(mStrSQL)) Then
            Return False
        End If

        Return True
    End Function

    Public Function getAnnuncio(ByVal annuncioId As Long) As Data.DataSet
        ' mStrSQL = "SELECT * FROM V_DETTAGLIO_ANNUNCIO"
        mStrSQL = "SELECT * FROM ANNUNCIO where annuncio_id = ? "


        mStrSQL = " SELECT UTENTI.my_login AS my_login, UTENTI.user_id AS user_id, UTENTI.isModeratore AS isModeratore, ANNUNCIO.annuncio_id AS annuncio_id, ANNUNCIO.date_added AS date_added, ANNUNCIO.tipo AS tipo, ANNUNCIO.nome AS nome, ANNUNCIO.prezzo AS prezzo, ANNUNCIO.autore AS autore, ANNUNCIO.marca AS marca, ANNUNCIO.modello AS modello, ANNUNCIO.casa_editrice AS casa_editrice, ANNUNCIO.descrizione AS descrizione, ANNUNCIO.stato AS stato, Switch(tipo=1,'Vendo',tipo=2,'Compro',tipo=3,'Scambio') AS tipo_desc, categorie.nome AS categoria, categorie.categoria_id AS categoria_id, ANNUNCIO.isbn AS isbn, ANNUNCIO.regione AS regione, ANNUNCIO.provincia AS provincia" & _
            " ,ANNUNCIO.count_click ,ANNUNCIO.date_start_click_parziale ,ANNUNCIO.date_last_click , ANNUNCIO.count_click_parziale " & _
            " FROM categorie INNER JOIN (ANNUNCIO LEFT JOIN UTENTI ON ANNUNCIO.fk_user_id=UTENTI.user_id) ON categorie.categoria_id=ANNUNCIO.fk_categoria_id " & _
            " WHERE (ANNUNCIO.date_deleted Is Null) And (ANNUNCIO.ANNUNCIO_ID=[?] )"
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "?", annuncioId)

        Return _fillDataSet(command)
    End Function

    Public Function getUtente(ByVal userId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM UTENTI WHERE user_id  =" & userId
        Return mFillDataTable(mStrSQL)
    End Function


    Public Function getUserIdFromLoginAndEmail(ByVal login As String, ByVal email As String) As Long
        Dim sqlQuery As String
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        sqlQuery = "select USER_ID from  utenti  " & _
                   "where UCASE(MY_LOGIN)= @myLogin "
        mAddParameter(command, "@myLogin", login.ToUpper.Trim)

        If Not String.IsNullOrEmpty(email) Then
            sqlQuery &= " and UCASE(EMAIL)= @myEmail "
            mAddParameter(command, "@myEmail", email.ToUpper.Trim)
        End If

        command.CommandText = sqlQuery

        Dim dataSet As Data.DataSet
        dataSet = Me._fillDataSet(command)

        If dataSet.Tables(0).Rows.Count > 1 Then
            Throw New ManagerException(ManagerException.ErrorNumber.LoginDuplicata)
        End If

        If dataSet.Tables(0).Rows.Count = 0 Then
            Throw New ManagerException("Attenzione: La Login o l'e-mail inserite non sono corrette")
        End If

        Return CLng(dataSet.Tables(0).Rows(0)("USER_ID"))
    End Function


    Public Function getRisposteFromTrattativa(ByVal trattativaId As Long) As System.Data.DataTable
        mStrSQL = "SELECT  UTENTI.my_login as my_login , UTENTI.isModeratore as isModeratore, UTENTI.user_id as user_id, risposta_id, fk_risposta_id ,RISPOSTA.date_added, testo " & _
                " FROM RISPOSTA " & _
                "LEFT  JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID " & _
                " WHERE FK_TRATTATIVA_ID = " & trattativaId
        mStrSQL = mStrSQL & " ORDER BY RISPOSTA.date_added ASC"

        Return mFillDataTable(mStrSQL)
    End Function



    Public Function getRisposteFromTrattativa(ByVal trattativaId As Long, ByVal annuncioId As Long) As System.Data.DataTable
        mStrSQL = "SELECT  UTENTI.my_login as my_login , UTENTI.isModeratore as isModeratore, UTENTI.user_id as user_id, risposta_id, fk_risposta_id ,RISPOSTA.date_added, testo " & _
                " FROM TRATTATIVA INNER JOIN (RISPOSTA LEFT  JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID ) ON RISPOSTA.FK_TRATTATIVA_ID = TRATTATIVA.TRATTATIVA_ID" & _
                " WHERE FK_TRATTATIVA_ID = " & trattativaId & " AND TRATTATIVA.FK_ANNUNCIO_ID =" & annuncioId
        mStrSQL &= " ORDER BY RISPOSTA.date_added ASC"

        Return mFillDataTable(mStrSQL)
    End Function

    Public Function updateNotificaLetturaRispostaOwner(ByVal trattativaId As Long) As Boolean
        'notifico la lettura di tutte le risposte che non sono state create da me ....
        mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_OWNER = 0  WHERE TRATTATIVA_ID = " & trattativaId
        mExecuteNoQuery(mStrSQL)
        Return True
    End Function

    Public Function updateNotificaLetturaRispostaUser(ByVal trattativaId As Long) As Boolean
        'notifico la lettura di tutte le risposte che non sono state create da me ....
        mStrSQL = "UPDATE TRATTATIVA SET NOTIFICA_USER = 0 WHERE TRATTATIVA_ID = " & trattativaId
        mExecuteNoQuery(mStrSQL)
        Return True
    End Function

    ' Public Function getTrattativeConMessaggi(ByVal userId As Long) As Data.DataTable
    '    mStrSQL = ""
    'End Function






    '    Public Function getRisposteFromAnnuncio(ByVal annuncioId As Long) As System.Data.DataTable
    'is questo caso sono il proprietario dell'annuncio e leggo TUTTE le RISPOSTE
    '    Return getRisposteFromAnnuncio(annuncioId, -1)
    'End Function



    Public Function getCategoriaFromAnnuncio(ByVal annuncioId As Long) As System.Data.DataSet
        Dim manager As New MyManager.CategorieManager(Me.mConnection)
        Return manager.getCategoriaFromAnnuncio(annuncioId)
    End Function


    Public Sub categoriaAddClick(ByVal categoriaId As Long)
        Dim manager As New MyManager.CategorieManager(Me.mConnection)
        manager.addClick(categoriaId)
    End Sub


    Public Sub resetContatoreParziale(ByVal annuncioId As Long)
        mStrSQL = "UPDATE ANNUNCIO SET date_start_click_parziale = NOW , COUNT_CLICK_PARZIALE = 0 " & _
                                       " WHERE ANNUNCIO_ID=" & annuncioId
        Me.mExecuteNoQuery(mStrSQL)
    End Sub



    Public Function updateAnnuncioDescrizione(ByVal annuncioId As Long, ByVal descrizione As String, test_mode As Boolean) As Integer
        mStrSQL = "UPDATE ANNUNCIO SET DATE_MODIFIED = NOW  "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        If Not String.IsNullOrEmpty(descrizione) Then
            mStrSQL &= " ,DESCRIZIONE = @DESCRIZIONE "
            mAddParameter(command, "@DESCRIZIONE", descrizione)
        End If

        mStrSQL &= " WHERE ANNUNCIO_ID=" & annuncioId

        command.CommandText = mStrSQL


        If test_mode = True Then
            Me._transactionBegin()
            mExecuteNoQuery(command)
            Me._transactionRollback()
            Return -1
        End If

        Return mExecuteNoQuery(command)
    End Function

    Public Function updateAnnuncioPrezzo(ByVal annuncioId As Long, prezzo As Decimal, test_mode As Boolean) As Integer
        mStrSQL = "UPDATE ANNUNCIO SET DATE_MODIFIED = NOW  "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        If prezzo <> Decimal.MinValue Then
            mStrSQL &= " ,PREZZO = @PREZZO "
            mAddParameter(command, "@PREZZO", prezzo)
        End If

        mStrSQL &= " WHERE ANNUNCIO_ID=" & annuncioId

        command.CommandText = mStrSQL


        If test_mode = True Then
            Me._transactionBegin()
            mExecuteNoQuery(command)
            Me._transactionRollback()
            Return -1
        End If

        Return mExecuteNoQuery(command)
    End Function



    Public Function updateUser(ByVal userId As Long, ByVal login As String, ByVal email As String) As Boolean

        mStrSQL = "UPDATE UTENTI SET DATE_MODIFIED = NOW  "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()


        If Not String.IsNullOrEmpty(email) Then
            mStrSQL &= " ,EMAIL = @EMAIL "
            mAddParameter(command, "@EMAIL", email)
        End If

        If Not String.IsNullOrEmpty(login) Then
            mStrSQL &= " ,MY_LOGIN = @LOGIN "
            mAddParameter(command, "@LOGIN", login)
        End If


        mStrSQL &= " WHERE USER_ID=" & userId

        command.CommandText = mStrSQL

        mExecuteNoQuery(command)
        Return True
    End Function



    Public Function updateEmail(ByVal userId As Long, ByVal email As String) As Boolean
        mStrSQL = "UPDATE UTENTI SET DATE_MODIFIED = NOW ,EMAIL = @email " & _
                                                     " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@email", email)
        Me.mExecuteNoQuery(command)

        Return True
    End Function




    Public Sub annuncioAddClick(ByVal annuncioId As Long)
        'mStrSQL = "UPDATE ANNUNCIO SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 " & _
        '                                    " WHERE ANNUNCIO_ID=" & annuncioId

        mStrSQL = "UPDATE ANNUNCIO SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 , COUNT_CLICK_PARZIALE = COUNT_CLICK_PARZIALE + 1" & _
                                       " WHERE ANNUNCIO_ID=" & annuncioId
        Me.mExecuteNoQuery(mStrSQL)

    End Sub


    Public Function getCategoria(ByVal categoriaId As Long) As System.Data.DataSet
        Dim manager As New MyManager.CategorieManager(Me.mConnection)
        Return manager.getCategoria(categoriaId)
    End Function

    Public Function getNumeroRisposteOfAnnuncio(ByVal annuncioId As Long, ByVal userId As Long) As Long
        'nel conteggio delle risposte escludo le risposte scritte da se stesso
        mStrSQL = "SELECT count(*) as NUMERO_RISPOSTE " & _
                    "FROM RISPOSTA  INNER JOIN  TRATTATIVA ON RISPOSTA.FK_TRATTATIVA_ID = TRATTATIVA.TRATTATIVA_ID" & _
                    " WHERE FK_ANNUNCIO_ID =" & annuncioId & " AND RISPOSTA.FK_USER_ID <> " & userId
        Return Me.mExecuteScalar(mStrSQL)
    End Function


    Public Function getNumeroRisposteOfTrattativa(ByVal trattativaId As Long, ByVal userId As Long) As Long
        'nel conteggio delle risposte escludo le risposte scritte da se stesso
        mStrSQL = "SELECT count(*) as NUMERO_RISPOSTE " & _
                    "FROM RISPOSTA  " & _
                    " WHERE FK_TRATTATIVA_ID =" & trattativaId & " AND RISPOSTA.FK_USER_ID <> " & userId
        Return Me.mExecuteScalar(mStrSQL)
    End Function

    Public Function getLastRisposta(ByVal annuncioId As Long) As System.Data.DataSet
        mStrSQL = "select top 1 * FROM RISPOSTA LEFT JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID WHERE FK_ANNUNCIO_ID =" & annuncioId & " ORDER BY RISPOSTA.DATE_ADDED DESC"
        Return Me._fillDataSet(mStrSQL)
    End Function

    Public Function getRisposta(ByVal rispostaId As Long) As System.Data.DataTable
        mStrSQL = "select * FROM RISPOSTA  LEFT JOIN UTENTI ON RISPOSTA.FK_user_ID = UTENTI.USER_ID WHERE RISPOSTA_ID =" & rispostaId
        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function getTipoDiOggetti() As System.Data.DataSet
        mStrSQL = "select * FROM TIPO_OGGETTO ORDER BY DATE_ADDED DESC"
        Return Me._fillDataSet(mStrSQL)
    End Function


    Public Function getEmailUtentiInTrattativa(ByVal annuncio_id As Long) As Data.DataTable
        'prelevo gli indizzi email di tutti gli utenti che stanno in trattativa su un annuncio
        'per inviargli un'email
        mStrSQL = "SELECT utenti.my_login, utenti.email " & _
                " FROM utenti INNER JOIN trattativa ON utenti.user_id = trattativa.fk_user_id " & _
                " WHERE trattativa.fk_annuncio_id = " & annuncio_id
        Return Me.mFillDataTable(mStrSQL)
    End Function


    'Public Function getEmailOwnerAnnnunci(ByVal annuncio_id As Long) As Data.DataTable
    '    'prelevo gli indizzi email di tutti gli utenti che stanno in trattativa su un annuncio
    '    'per inviargli un'email
    '    mStrSQL = "SELECT utenti.my_login, utenti.email " & _
    '            " FROM utenti INNER JOIN annuncio ON utenti.user_id = annuncio.fk_user_id " & _
    '            " WHERE annuncio.annuncio_id = " & annuncio_id
    '    Return Me.mFillDataTable(mStrSQL)
    'End Function


    Public Function getEmailReplyAnnnuncio(ByVal trattativaId As Long) As Data.DataTable
        'in teoria per ogni trattaviva sono in 2: owner + user
        mStrSQL = "SELECT utenti.my_login, utenti.email, utenti.user_id, trattativa.trattativa_id, utenti_1.my_login as login_owner , utenti_1.email as email_owner , utenti_1.user_id as user_id_owner " & _
                    " FROM utenti AS utenti_1 INNER JOIN (utenti INNER JOIN (annuncio INNER JOIN trattativa ON annuncio.annuncio_id = trattativa.fk_annuncio_id) ON utenti.user_id = trattativa.fk_user_id) ON utenti_1.user_id = annuncio.fk_user_id " & _
                    "  WHERE trattativa.trattativa_id = " & trattativaId

        Return Me.mFillDataTable(mStrSQL)
    End Function


    Public Function getTitoloOfAnnuncio(ByVal annuncioId As Long) As String
        mStrSQL = "SELECT NOME FROM ANNUNCIO WHERE ANNUNCIO_ID=" & annuncioId
        Return mExecuteScalar(mStrSQL)
    End Function


    Public Function insertUser(ByVal userId As Long, ByVal nome As String, ByVal cognome As String, ByVal email As String, ByVal mylogin As String, ByVal customerId As Long) As Long
        'in questo caso la userId non è un contatore in quanto il valore viene gestito da UserManager

        mStrSQL = "INSERT INTO UTENTI ( USER_ID,  NOME, COGNOME, MY_LOGIN, EMAIL , CUSTOMER_ID )" & _
            " VALUES ( @USER_ID , @NOME , @COGNOME , @LOGIN , @EMAIL , @CUSTOMER_ID )"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@USER_ID", userId)
        Me.mAddParameter(command, "@NOME", nome.Trim)
        Me.mAddParameter(command, "@COGNOME", cognome.Trim)
        Me.mAddParameter(command, "@LOGIN", mylogin.Trim)
        Me.mAddParameter(command, "@EMAIL", email.Trim)

        If customerId = -1 Then
            Me.mAddParameter(command, "@CUSTOMER_ID", DBNull.Value)
        Else
            Me.mAddParameter(command, "@CUSTOMER_ID", customerId)
        End If

        mExecuteNoQuery(command)

        Return _getIdentity()
    End Function



    Public Function getUtentiList() As Data.DataTable
        mStrSQL = "select * from utenti "
        Return mFillDataTable(mStrSQL)
    End Function

End Class
