Public Class UserManager
    Inherits Manager

    Public _nome As String = ""
    Public _cognome As String = ""
    Public _login As String = ""
    Public _password As String = ""
    Public _email As String = ""
    Public _telefono As String = ""
    Public _indirizzo As String = ""
    Public _numero_civico As String = ""
    Public _citta As String = ""
    ' Public _provincia As String = ""
    Public _cap As String = ""
    Public _dataDiNascita As Date = Date.MinValue
    Public _cittaNascita As String = ""
    Public _cellulare As String = ""
    Public _codiceFiscale As String = ""
    Public _sesso As Char = ""

    'Public _subscribeNewsLetter As Boolean = True
    'Public _subscribeNewsLetterLavoro As Boolean = True

    Public _photo As Boolean

    Public _http As String = ""
    Public _fax As String = ""
    Public _tipologia As String = ""
    Public _descrizione As String = ""
    Public _keywords As String = ""
    Public _isEnabled As Boolean = True

    '27/02/2011 gestione inserimento di un riferimento ad una persona giuridica
    Public _customerId As Long = -1

    Public _regione As String
    Public _provincia As String
    Public _comune As String

    Public _regioneId As Long = -1
    'Public _provinciaId As Long = -1
    'Public _comuneId As Long = -1

    Public _provinciaId As String
    Public _comuneId As String

    Public Sub New()
        MyBase.New("utenti")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Function activeUser(ByVal userId As Long) As Boolean
        mStrSQL = "UPDATE UTENTI SET CODICE_ATTIVAZIONE = NULL " & _
                                               ", DATE_MODIFIED = NOW " & _
                                                " WHERE USER_ID=" & userId

        mExecuteNoQuery(mStrSQL)
        Return True
    End Function


    Public Function getUtenteFromCodiceAttivazione(ByVal codiceAttivazione As String) As DataTable
        mStrSQL = "SELECT * FROM UTENTI WHERE IS_ENABLED = true AND CODICE_ATTIVAZIONE = @CODICE_ATTIVAZIONE "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@CODICE_ATTIVAZIONE", codiceAttivazione)

        Return _fillDataSet(command).Tables(0)
    End Function

    Public Function getRoles(ByVal userId As Long) As String
        mStrSQL = "SELECT PROFILO_ID FROM UTENTI WHERE user_id = @USER_ID "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@USER_ID", userId)

        Dim risultato As Object
        risultato = mExecuteScalar(command)
        If risultato Is System.DBNull.Value Then
            Return ""
        End If

        Return risultato
    End Function


    'Public Function getEmailFromLogin(ByVal login As String) As String
    '    getEmailFromLogin = False
    '    Dim strSQL As String = "SELECT EMAIL FROM UTENTI WHERE UCASE(mylogin) = ?"

    '    Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
    '    oleDbCommand.Parameters.Add("mylogin", OleDb.OleDbType.VarChar).Value = login.ToUpper

    '    getEmailFromLogin = oleDbCommand.ExecuteScalar()

    '    oleDbCommand.Dispose()
    'End Function


    Public Function getEmail(ByVal userId As Long) As String
        mStrSQL = "SELECT EMAIL FROM UTENTI WHERE user_id = @USER_ID "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@USER_ID", userId)
        Return Me.mExecuteScalar(command)
    End Function


    Public Function getLogin(ByVal userId As Long) As String
        mStrSQL = "SELECT MY_LOGIN FROM UTENTI WHERE user_id = @USER_ID "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@USER_ID", userId)
        Return Me.mExecuteScalar(command)
    End Function


    Public Function creaLogin(ByVal nome As String, ByVal cognome As String) As String
        Dim newLogin As String

        Dim myPassword As New PasswordManager()

        nome = nome.ToLower.Trim
        cognome = cognome.ToLower.Trim

        '*** elimino i caratteri speciali ***
        Dim carattereSpeciale As Char
        Dim i As Int16
        For i = 0 To myPassword.PASSWORD_CHARS_SPECIAL_DENY.Length - 1
            carattereSpeciale = myPassword.PASSWORD_CHARS_SPECIAL_DENY(i)

            nome = nome.Replace(carattereSpeciale, "")
            cognome = cognome.Replace(carattereSpeciale, "")
        Next



        '*** elimino gli spazi ***
        'uno o più spazi vengono sostituiti da un _
        ' Create a regular expression that matches a series of one or more white spaces.
        Dim pattern As String = "\s+"
        Dim rgx As New System.Text.RegularExpressions.Regex(pattern)
        nome = rgx.Replace(nome, "_")
        cognome = rgx.Replace(cognome, "_")

        pattern = "_+"
        rgx = New System.Text.RegularExpressions.Regex(pattern)
        nome = rgx.Replace(nome, "_")
        cognome = rgx.Replace(cognome, "_")



        'nome = nome.ToLower.Trim.Replace(" ", "_")
        'cognome = cognome.ToLower.Trim.Replace(" ", "_")

        If String.IsNullOrEmpty(cognome) Then
            newLogin = nome
        Else
            newLogin = nome & "." & cognome
        End If

        Dim result As Boolean = verificaLogin(newLogin)

        If (result) Then
            ' la login è già presente nel DB. Ne creo un'altra
            Dim addnum As Int64 = 0
            While (result)
                addnum += 1
                newLogin = nome & "." & cognome & "_" & addnum.ToString
                result = verificaLogin(newLogin)
            End While
        End If
        ' Per il login viene utilizzato nome.cognome
        Return newLogin
    End Function


    Public Function verificaLogin(ByVal login As String) As Boolean

        Dim strSQL As String
        strSQL = "SELECT count(*) FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  MY_LOGIN = @MY_LOGIN "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL

        Me.mAddParameter(command, "@MY_LOGIN", login)

        Return CInt(Me.mExecuteScalar(command)) > 0

    End Function




    'Public Function getNewUID(ByVal userId As Long) As String
    '    Dim tempCodice As Guid
    '    tempCodice = Guid.NewGuid

    '    mStrSQL = "UPDATE UTENTI SET MY_PASSWORD = @MY_PASSWORD" & _
    '                                         ", DATE_MODIFIED = NOW " & _
    '                                          " WHERE USER_ID=" & userId

    '    Dim command As System.Data.Common.DbCommand
    '    command = mConnection.CreateCommand()


    '    leDbCommand.Parameters.Add("@MY_PASSWORD", OleDb.OleDbType.VarChar).Value = MyManager.SecurityManager.getMD5Hash(newPassword.Trim)
    '    oleDbCommand.ExecuteNonQuery()



    'End Function


    Public Function getNewCodiceAttivazione(ByVal userId As Long) As String
        Dim tempCodice As String
        tempCodice = getNewCodiceAttivazione()

        mStrSQL = "UPDATE UTENTI SET CODICE_ATTIVAZIONE = @CODICE_ATTIVAZIONE WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        mAddParameter(command, "@CODICE_ATTIVAZIONE", tempCodice)
        command.CommandText = mStrSQL

        Me.mExecuteNoQuery(command)

        Return tempCodice
    End Function

    Public Function getNewEmailChanging(ByVal activationCodeEmail As String, ByVal userId As Long) As String
        Dim sqlQuery As String
        sqlQuery = "SELECT EMAIL_CHANGING FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  USER_ID = " & userId & " and EMAIL_ACTIVATION_CODE = '" & activationCodeEmail.Replace("'", "''") & "'"
        Return (Me.mExecuteScalar(sqlQuery)).ToString
    End Function

    Public Function getNewCodiceAttivazione() As String
        Dim myPassword As New PasswordManager

        Dim tempCodice As String
        'SOLO con caratteri alfanumerici
        tempCodice = myPassword.getCodiceAlfaNumerico(8)

        'per scupolo verifico che non ci sia un codice invito già presente nel sistema
        While verificaCodiceAttivazione(tempCodice)
            tempCodice = myPassword.getCodiceAlfaNumerico(8)
        End While


        Return tempCodice
    End Function


    'verifico che non sia già presente un codice invito nel sistema
    Public Function verificaCodiceAttivazione(ByVal value As String) As Boolean
        Dim strSQL As String = "SELECT count(*) as tot  FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  codice_attivazione = @codice_attivazione "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        command.CommandText = strSQL
        Me.mAddParameter(command, "@codice_attivazione", value)

        Return CInt(Me.mExecuteScalar(command)) > 0
    End Function


    Public Function deleteALLUsersAndLogs() As Integer
        mStrSQL = "DELETE * FROM LOG_UTENTE"
        Me.mExecuteNoQuery(mStrSQL)
        mStrSQL = "DELETE * FROM UTENTI"

        Return Me.mExecuteNoQuery(mStrSQL)
    End Function


    Public Function deleteUserAndLogs(ByVal userId As Long) As Integer
        mStrSQL = "DELETE * FROM LOG_UTENTE where USER_ID = " & userId
        Me.mExecuteNoQuery(mStrSQL)
        mStrSQL = "DELETE * FROM UTENTI where USER_ID = " & userId

        Return Me.mExecuteNoQuery(mStrSQL)
    End Function

    'Le password sono codificate il MD5 quindi non ha senso leggerle!

    'Public Function getPassword(ByVal user_id As String) As String
    '    Dim strSQL As String = "SELECT MYPASSWORD FROM UTENTI WHERE user_id = ?"

    '    Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
    '    oleDbCommand.Parameters.Add("user_id", OleDb.OleDbType.VarChar).Value = user_id

    '    Dim temp As String = oleDbCommand.ExecuteScalar()

    '    Dim manager As New SecurityManager
    '    temp = manager.decriptaDES(temp)
    '    getPassword = temp

    '    oleDbCommand.Dispose()
    'End Function


    Public Function getUtente(ByVal userId As Long) As System.Data.DataSet


        ' carico i dati utente
        'sqlQuery = "select CUSTOMER_ID, AUTHENTICATION_ID, USER_ID, MY_LOGIN, MY_PASSWORD, u.NOME, COGNOME, EMAIL, u.DATE_ADDED, u.DATE_MODIFIED, DATE_EXPIRE_ACCOUNT, " & _
        '        "DATE_MODIFIED_PASSWORD, IS_LOCKED, DATE_LAST_LOGIN, LOGIN_SUCCESS, LOGIN_FAILURE, DAY_EXPIRE_PASSWORD,  r.nome profilo_desc, ruolo_id profilo_id " & _
        '        "from  utenti u left join utente_ruoli p using(user_id) left join ruoli r using (ruolo_id) join CUSTOMER c using (customer_id) " & _
        '        "where user_id=" & userId

        mStrSQL = "SELECT * FROM utenti where user_id=" & userId

        Return _fillDataSet(mStrSQL)
    End Function


    Public Function getUtentiList() As Data.DataTable
        mStrSQL = "select * from utenti WHERE DATE_DELETED IS NULL AND IS_ENABLED = TRUE"
        Return mFillDataTable(mStrSQL)
    End Function



    Public Function getUserIdFromLogin(ByVal login As String) As Long
        Return getUserIdFromLoginAndEmail(login, "")
    End Function



    Public Function getUserIdFromLoginAndEmail(ByVal login As String, ByVal email As String) As Long
        Dim sqlQuery As String
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        sqlQuery = "select USER_ID from  utenti  " & _
                   "where UCASE(MY_LOGIN)= @myLogin "
        mAddParameter(command, "@myLogin", login.ToUpper.Trim)

        If email <> "" Then
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


    Public Function getUserIdFromEmail(ByVal email As String) As Long
        Dim sqlQuery As String
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        sqlQuery = "select USER_ID from  utenti  " & _
                   "where UCASE(EMAIL)= @myEmail AND DATE_DELETED IS NULL"
        mAddParameter(command, "@myEmail", email.ToUpper.Trim)

        command.CommandText = sqlQuery

        Dim dataSet As Data.DataSet
        dataSet = Me._fillDataSet(command)

        If dataSet.Tables(0).Rows.Count > 1 Then
            Throw New ManagerException(ManagerException.ErrorNumber.EmailDuplicata)
        End If

        If dataSet.Tables(0).Rows.Count = 0 Then
            Throw New ManagerException("Email non trovata")
        End If

        Return CLng(dataSet.Tables(0).Rows(0)("USER_ID"))
    End Function



    Public Function getFullName(ByVal userId As Long) As String
        mStrSQL = "SELECT NOME + ' ' + COGNOME FROM UTENTI WHERE user_id = " & userId
        Return mExecuteScalar(mStrSQL)
    End Function


    Public Function getFullIndirizzo(ByVal userId As Long) As String
        mStrSQL = "SELECT INDIRIZZO + ' N°' + NUMERO_CIVICO + '  Città: ' + CITTA + ' - ' + CAP + ' (' + PROVINCIA + ')' FROM UTENTI WHERE user_id =" & userId
        Return mExecuteScalar(mStrSQL)
    End Function

    'resrituisce -1 in caso di errore nelle credenziali oppure ID dell'utente
    Public Function isAuthenticated(ByVal myLogin As String, ByVal myPassword As String, ByRef mySessionData As MyManager.SessionData) As Long

        'Dim command As New System.Data.OleDb.OleDbCommand
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        command.Connection = Me.mConnection

        Dim dataSet As New DataSet
        Dim row As System.Data.DataRow
        Dim sqlQuery As String
        'Dim pwdExpired As Boolean = False
        Dim userId As Long

        Try
            ' sqlQuery = "select user_id from utenti where customer_id = " & customer_id & " and UCASE(my_login) = '" & myLogin.ToUpper.Replace("'", "''") & "'"
            'sqlQuery = "select user_id from utenti where UCASE(my_login) = '" & myLogin.Trim.ToUpper.Replace("'", "''") & "'"

            sqlQuery = "select user_id from utenti where UPPER(my_login) = @mylogin "

            'Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(sqlQuery, mConnection)
            'oleDbCommand.Parameters.Add("mylogin", OleDb.OleDbType.VarChar).Value = myLogin.ToUpper

            command.CommandText = sqlQuery




            mAddParameter(command, "@mylogin", myLogin.ToUpper)

            ' verifico l'esistenza del cliente
            dataSet = _fillDataSet(command)
        Catch ex As Exception
            'Ritorno errore
            Throw New ManagerException("Si è verificato un errore in fase di autenticazione. Non è possibile risalire al clinte dell'utente.", ex)
        End Try

        REM Controllo se l'utente è censito
        If dataSet.Tables(0).Rows.Count = 0 Then
            'Il cliente non è censito
            Throw New ManagerException(ManagerException.ErrorNumber.LoginPasswordErrati)

        End If

        userId = CLng(dataSet.Tables(0).Rows(0).Item("USER_ID"))
        dataSet = getUtente(userId)
        row = dataSet.Tables(0).Rows(0)

        If Not CBool(row("IS_ENABLED")) Then
            Dim ex As New ManagerException(ManagerException.ErrorNumber.UtenteDisabilitato)
            MyManager.MailManager.send(ex)
            Throw ex
        End If


        myPassword = MyManager.SecurityManager.getMD5Hash(myPassword.Trim)
        If row("MY_PASSWORD").ToString <> myPassword Then
            'Per ORALCE
            'sqlQuery = "update utenti set login_failure=nvl(login_failure,0)+1 where user_id=?"

            sqlQuery = "update utenti set login_failure=login_failure +1 where user_id=  @userId  "

            command.CommandText = sqlQuery


            If Me.mConnection.GetType().Name = "OleDbConnection" _
   OrElse Me.mConnection.GetType().Name = "OdbcConnection" Then
                'Per ACCESS e PostgreSQL ...
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
            End If




            command.Parameters.Clear()
            'command.Parameters.Add("@userId", System.Data.OleDb.OleDbType.BigInt, 38).Value = userId
            mAddParameter(command, "@userId", userId)

            command.Connection = Me.mConnection


            command.ExecuteNonQuery()
            Throw New ManagerException(ManagerException.ErrorNumber.LoginPasswordErrati)
        End If

        'If (mySessionData IsNot Nothing) Then
        '    mySessionData.SetIsLocal(True)
        'End If


        ' TUTTO OK!!!!
        'metto i dati dell'utente in sessione 
        If Not (mySessionData Is Nothing) Then
            ' sqlQuery = "update UTENTI set date_last_login=sysdate, login_success= login_success +1 "

            sqlQuery = "update UTENTI set date_last_login= GetDate() , login_success= login_success +1 " & _
                                     "where user_id=" & userId
            command.Parameters.Clear()
            command.CommandText = sqlQuery

            If Me.mConnection.GetType().Name = "OleDbConnection" _
   OrElse Me.mConnection.GetType().Name = "OdbcConnection" Then
                'Per ACCESS e PostgreSQL ...
                command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
            End If

            command.Connection = Me.mConnection

            command.ExecuteNonQuery()

            Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            managerLogUser.insert(userId, MyManager.LogUserManager.LogType.Login)


            'ATTENZIONE

            '*** trova-libro ***
            'sferr
            'userId = 114680869



            '*** cerco-vendo-casa ***
            'ABICASE
            'userId = 956667094

            'Fabianore
            'userId = -1289958842


            mySessionData.setUserId(userId)
            mySessionData.setLogin(myLogin)
            mySessionData.setNomeCognome(row("NOME").ToString, row("COGNOME").ToString)
            mySessionData.setEmail(getEmail(userId))
            If IsDBNull(row("date_last_login")) Then
                mySessionData.setLastLogin(Date.MinValue)
            Else
                mySessionData.setLastLogin(row("date_last_login"))
            End If

            Dim autorizzazioni As String
            autorizzazioni = ";" & getRoles(userId)
            autorizzazioni &= ";Authenticated;"
            mySessionData.setAuthorizations(autorizzazioni)

            '  mySessionData.setCustomerId(CLng(row("CUSTOMER_ID")))
            ' mySessionData.setAuthorizations(";" & Me.getCodiciAutorizzazioniForUser(userId, "PORTALE"))

        End If

        Return userId
    End Function





    Shared Function isStrongPassword(ByVal value As String) As Boolean
        Return MyManager.RegularExpressionManager.isStrongPassword(value)
    End Function






    ''' <summary>
    ''' Reset della Password
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>Viene generata una Password in modo Random, Codificata in MD5, e inoltrata una mail dell'utente corrente.
    ''' occore impostare lo User_id e la mail dell'utente in esame
    ''' </remarks>
    Public Function resetPassword(ByVal userId As Long) As String
        Dim passwordGenerata As String

        Dim myPassword As New PasswordManager()

        'faccio un controllo sulla password generata!!!!
        passwordGenerata = myPassword.GeneratePassword(10)
        While Not UserManager.isStrongPassword(passwordGenerata)
            passwordGenerata = myPassword.GeneratePassword(10)
        End While

        Dim esito As Boolean = False
        Dim sqlQuery As String = "UPDATE UTENTI SET MY_PASSWORD= @MY_Password , DATE_MODIFIED_PASSWORD = GetDate() WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()


        Try
            command.CommandText = sqlQuery

            Me.mAddParameter(command, "@MY_Password", MyManager.SecurityManager.getMD5Hash(passwordGenerata))
            Me.mExecuteNoQuery(command)

            Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
            managerLogUser.insert(userId, MyManager.LogUserManager.LogType.ResetPassword)

            esito = True
        Catch ex As Exception
            Throw New MyManager.ManagerException("Errore nella funzione di 'Reset Password", ex)
        End Try

        Return passwordGenerata
    End Function







    Public Function insert() As Long
        _cap = _cap.Trim
        _cellulare = _cellulare.Trim
        _citta = _citta.Trim
        _cittaNascita = _cittaNascita.Trim
        _codiceFiscale = _codiceFiscale.Trim
        _cognome = _cognome.Trim
        _descrizione = _descrizione.Trim
        _email = _email.Trim
        _fax = _fax.Trim
        _http = _http.Trim
        _indirizzo = _indirizzo.Trim
        _login = _login.Trim
        _nome = _nome.Trim
        _numero_civico = _numero_civico.Trim
        _provincia = _provincia.Trim
        _telefono = _telefono.Trim
        _tipologia = _tipologia.Trim



        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO UTENTI (DATE_ADDED  "
        strSQLParametri = " VALUES ( Now()  "


        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()


        If Not String.IsNullOrEmpty(_nome) Then
            strSQL &= ",NOME "
            strSQLParametri &= ", @NOME "
            mAddParameter(command, "@NOME", _nome)
        End If

        If Not String.IsNullOrEmpty(_cognome) Then
            strSQL &= ",COGNOME "
            strSQLParametri &= ", @COGNOME "
            mAddParameter(command, "@COGNOME", _cognome)
        End If


        If Not String.IsNullOrEmpty(_login) Then
            strSQL &= ",MY_LOGIN "
            strSQLParametri &= ", @MY_LOGIN "
            mAddParameter(command, "@MY_LOGIN", _login)
        End If

        If Not String.IsNullOrEmpty(_email) Then
            strSQL &= ",EMAIL "
            strSQLParametri &= ", @EMAIL "
            mAddParameter(command, "@EMAIL", _email)
        End If

        If Not String.IsNullOrEmpty(_indirizzo) Then
            strSQL &= ",INDIRIZZO "
            strSQLParametri &= " ,@INDIRIZZO "
            mAddParameter(command, "@INDIRIZZO", _indirizzo)
        End If

        If Not String.IsNullOrEmpty(_telefono) Then
            strSQL &= ",TELEFONO "
            strSQLParametri &= ", @TELEFONO "
            mAddParameter(command, "@TELEFONO", _telefono)
        End If

        If Not String.IsNullOrEmpty(_numero_civico) Then
            strSQL &= ",NUMERO_CIVICO "
            strSQLParametri &= ", @NUMERO_CIVICO "
            mAddParameter(command, "@NUMERO_CIVICO", _numero_civico)
        End If

        If Not String.IsNullOrEmpty(_citta) Then
            strSQL &= ",CITTA "
            strSQLParametri &= ", @CITTA "
            mAddParameter(command, "@CITTA", _citta)
        End If

        'If Not String.IsNullOrEmpty(_provincia) Then
        '    strSQL &= ",PROVINCIA "
        '    strSQLParametri &= ", @PROVINCIA "
        '    mAddParameter(command, "@PROVINCIA", _provincia)
        'End If

        If Not String.IsNullOrEmpty(_cap) Then
            strSQL &= ",CAP "
            strSQLParametri &= ", @CAP "
            mAddParameter(command, "@CAP", _cap)
        End If

        strSQL &= ",PHOTO "
        strSQLParametri &= ", @PHOTO "
        mAddParameter(command, "@PHOTO", _photo)


        If _dataDiNascita <> Date.MinValue Then
            strSQL &= ",data_nascita "
            strSQLParametri &= ", @data_nascita "
            mAddParameter(command, "@data_nascita", _dataDiNascita)
        End If

        If Not String.IsNullOrEmpty(_cittaNascita) Then
            strSQL &= ",CITTA_NASCITA "
            strSQLParametri &= ", @CITTA_NASCITA "
            mAddParameter(command, "@CITTA_NASCITA", _cittaNascita)
        End If

        If Not String.IsNullOrEmpty(_cellulare) Then
            strSQL &= ",CELLULARE "
            strSQLParametri &= ", @CELLULARE "
            mAddParameter(command, "@CELLULARE", _cellulare)
        End If

        If Not String.IsNullOrEmpty(_codiceFiscale) Then
            strSQL &= ",CODICEFISCALE "
            strSQLParametri &= ", @CODICEFISCALE "
            mAddParameter(command, "@CODICEFISCALE", _codiceFiscale)
        End If

        If Not String.IsNullOrEmpty(_sesso) Then
            strSQL &= ",SESSO "
            strSQLParametri &= ", @SESSO "
            mAddParameter(command, "@SESSO", _sesso)
        End If

        'Giugno 2007
        If Not String.IsNullOrEmpty(_http) Then
            strSQL &= ",HTTP "
            strSQLParametri &= ", @HTTP "
            mAddParameter(command, "@HTTP", _http)
        End If

        If Not String.IsNullOrEmpty(_fax) Then
            strSQL &= ",FAX "
            strSQLParametri &= ", @FAX "
            mAddParameter(command, "@FAX", _fax)
        End If

        If Not String.IsNullOrEmpty(_tipologia) Then
            strSQL &= ",TIPOLOGIA "
            strSQLParametri &= ", @TIPOLOGIA "
            mAddParameter(command, "@TIPOLOGIA", _tipologia)
        End If

        If Not String.IsNullOrEmpty(_descrizione) Then
            strSQL &= ",DESCRIZIONE "
            strSQLParametri &= ", @DESCRIZIONE "
            mAddParameter(command, "@DESCRIZIONE", _descrizione)
        End If

        If Not String.IsNullOrEmpty(_keywords) Then
            strSQL &= ",KEYWORDS "
            strSQLParametri &= ", @KEYWORDS "
            mAddParameter(command, "@KEYWORDS", _keywords)
        End If

        If _customerId <> -1 Then
            strSQL &= ",CUSTOMER_ID "
            strSQLParametri &= ", @CUSTOMER_ID "
            mAddParameter(command, "@CUSTOMER_ID", _customerId)
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

        If Not String.IsNullOrEmpty(_comune) Then
            strSQL &= ", COMUNE "
            strSQLParametri &= ", @COMUNE "
            mAddParameter(command, "@COMUNE", _comune)
        End If

        If _regioneId <> -1 Then
            strSQL &= ",REGIONE_ID "
            strSQLParametri &= ", @REGIONE_ID "
            mAddParameter(command, "@REGIONE_ID", _regioneId)
        End If

        ' If _provinciaId <> -1 Then
        If Not String.IsNullOrEmpty(_provinciaId) Then
            strSQL &= ",PROVINCIA_ID "
            strSQLParametri &= ", @PROVINCIA_ID "
            mAddParameter(command, "@PROVINCIA_ID", _provinciaId)
        End If

        'If _comuneId <> -1 Then
        If Not String.IsNullOrEmpty(_comuneId) Then
            strSQL &= ", COMUNE_ID "
            strSQLParametri &= ", @COMUNE_ID "
            mAddParameter(command, "@COMUNE_ID", _comuneId)
        End If


        strSQL &= ",IS_ENABLED "
        strSQLParametri &= ", @IS_ENABLED "
        mAddParameter(command, "@IS_ENABLED", _isEnabled)


        command.CommandText = strSQL & " ) " & strSQLParametri & " )"

        mExecuteNoQuery(command)

        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function





    'resrituisce -1 in caso di errore nelle credenziali oppure ID del NUOVO utente
    Public Function register() As Long
        Dim esito As Boolean = False
        Dim oleDbCommand As System.Data.OleDb.OleDbCommand

        Dim strSQLParametri As String = ""

        'verifico la presenza di un utente con la stessa login...
        Dim strSQL As String = "SELECT count(user_id) FROM UTENTI WHERE UCASE(MY_LOGIN) = @LOGIN"
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, mConnection)
        oleDbCommand.Parameters.Add("@LOGIN", OleDb.OleDbType.VarChar).Value = Me._login.ToUpper.Trim

        Dim newId As Long
        newId = CLng(oleDbCommand.ExecuteScalar)

        If (newId > 0) Then
            'vuo dire che esite un utente con la stessa login
            Throw New ManagerException(ManagerException.ErrorNumber.LoginDuplicata)
        End If

        newId = Me.insert()

        Return newId
    End Function


    Public Function deleteProfileImage(ByVal userId As Long) As Boolean
        mStrSQL = "UPDATE UTENTI SET PHOTO = FALSE " & _
                                              ", DATE_MODIFIED = NOW " & _
                                               " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        mExecuteNoQuery(command)


        Return True
    End Function






    Public Function updateProfile(ByVal userId As Long) As Boolean
        Dim esito As Boolean = False

        mStrSQL = "UPDATE UTENTI SET DATE_MODIFIED = NOW  "

        'Una volta registrato un utente non può certo modificare il suo nome e cognome e tanto meno la LOGIN
        'oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = Me._nome
        'oleDbCommand.Parameters.Add("@COGNOME", OleDb.OleDbType.VarChar).Value = Me._cognome
        'oleDbCommand.Parameters.Add("@LOGIN", OleDb.OleDbType.VarChar).Value = Me._login

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()


        If Not String.IsNullOrEmpty(_sesso) Then
            mStrSQL &= " ,SESSO = @SESSO "
            mAddParameter(command, "@SESSO", _sesso)
        End If


        If Not String.IsNullOrEmpty(_email) Then
            mStrSQL &= " ,EMAIL = @EMAIL "
            mAddParameter(command, "@EMAIL", _email)
        End If

        If Not String.IsNullOrEmpty(_telefono) Then
            mStrSQL &= " ,TELEFONO = @TELEFONO "
            mAddParameter(command, "@TELEFONO", _telefono)
        End If

        If Not String.IsNullOrEmpty(_indirizzo) Then
            mStrSQL &= " ,INDIRIZZO = @INDIRIZZO "
            mAddParameter(command, "@INDIRIZZO", _indirizzo)
        End If

        If Not String.IsNullOrEmpty(_numero_civico) Then
            mStrSQL &= " ,NUMERO_CIVICO = @NUMERO_CIVICO "
            mAddParameter(command, "@NUMERO_CIVICO", _numero_civico)
        End If

        'If Not String.IsNullOrEmpty(_citta) Then
        '    mStrSQL &= " ,CITTA = @CITTA "
        '    mAddParameter(command, "@CITTA", _citta)
        'End If

        'If Not String.IsNullOrEmpty(_provincia) Then
        '    mStrSQL &= " ,PROVINCIA = @PROVINCIA "
        '    mAddParameter(command, "@PROVINCIA", _provincia)
        'End If

        If Not String.IsNullOrEmpty(_cap) Then
            mStrSQL &= " ,CAP = @CAP "
            mAddParameter(command, "@CAP", _cap)
        End If

        'a livello di DB una volta che un utente ha cambiato la photo di default se la tiene...
        'al massimo la cambia!!!
        If (Me._photo) Then
            mStrSQL &= ", PHOTO = @PHOTO "
            mAddParameter(command, "@PHOTO", True)
        End If

        If Not String.IsNullOrEmpty(Me._http) Then
            mStrSQL &= ", HTTP = @HTTP "
            mAddParameter(command, "@HTTP", _http)
        End If

        If Not String.IsNullOrEmpty(Me._fax) Then
            mStrSQL &= ", FAX = @FAX "
            mAddParameter(command, "@FAX", _fax)
        End If

        If Not String.IsNullOrEmpty(Me._tipologia) Then
            mStrSQL &= ", TIPOLOGIA = @TIPOLOGIA "
            mAddParameter(command, "@TIPOLOGIA", _tipologia)
        End If

        If Not String.IsNullOrEmpty(Me._descrizione) Then
            mStrSQL &= ", DESCIRIZIONE = @DESCIRIZIONE "
            mAddParameter(command, "@DESCIRIZIONE", _descrizione)
        End If

        If Not String.IsNullOrEmpty(Me._keywords) Then
            mStrSQL &= ", KEYWORDS = @KEYWORDS "
            mAddParameter(command, "@KEYWORDS", _keywords)
        End If


        mStrSQL &= ", REGIONE = @REGIONE "
        If Not String.IsNullOrEmpty(_regione) Then
            mAddParameter(command, "@REGIONE", _regione)
        Else
            mAddParameter(command, "@REGIONE", DBNull.Value)
        End If

        mStrSQL &= ", PROVINCIA = @PROVINCIA "
        If Not String.IsNullOrEmpty(_provincia) Then
            mAddParameter(command, "@PROVINCIA", _provincia)
        Else
            mAddParameter(command, "@PROVINCIA", DBNull.Value)
        End If


        mStrSQL &= ", COMUNE = @COMUNE "
        If Not String.IsNullOrEmpty(_comune) Then
            mAddParameter(command, "@COMUNE", _comune)
        Else
            mAddParameter(command, "@COMUNE", DBNull.Value)
        End If


        mStrSQL &= ", REGIONE_ID = @REGIONE_ID "
        If _regioneId <> -1 Then
            mAddParameter(command, "@REGIONE_ID", _regioneId)
        Else
            mAddParameter(command, "@REGIONE_ID", DBNull.Value)
        End If


        mStrSQL &= ", PROVINCIA_ID = @PROVINCIA_ID "
        ' If _provinciaId <> -1 Then
        If Not String.IsNullOrEmpty(_provinciaId) Then
            mAddParameter(command, "@PROVINCIA_ID", _provinciaId)
        Else
            mAddParameter(command, "@PROVINCIA_ID", DBNull.Value)
        End If


        mStrSQL &= ", COMUNE_ID = @COMUNE_ID "
        '  If _comuneId <> -1 Then
        If Not String.IsNullOrEmpty(_comuneId) Then
            mAddParameter(command, "@COMUNE_ID", _comuneId)
        Else
            mAddParameter(command, "@COMUNE_ID", DBNull.Value)
        End If

        mStrSQL &= " WHERE USER_ID=" & userId

        command.CommandText = mStrSQL

        mExecuteNoQuery(command)


        '*** FORUM ***
        'If System.Configuration.ConfigurationManager.AppSettings("forum.isEnabled") = "true" Then
        '    '      Dim tempConnectionString As String = mConnection.ConnectionString.Replace("mdb", "forum.mdb")
        '    Dim managerForum As New ForumManager()
        '    managerForum.openConnection()
        '    Try
        '        managerForum.updateUser(user_id, Me._email)
        '    Catch ex As Exception
        '        ' caso di errore mi invio un'email di errore e continuo la registrazione normale
        '        'altrimenti dovrei cancellare il record che ho inseirto sul mdb
        '        MyManager.MailManager.send(ex)
        '    Finally
        '        managerForum.closeConnection()
        '    End Try
        'End If

        Return True
    End Function

    Public Function updatePassword(ByVal userId As Long, ByVal newPassword As String) As Boolean
        mStrSQL = "UPDATE UTENTI SET MY_PASSWORD = @MY_PASSWORD " & _
                                               ", DATE_MODIFIED = NOW " & _
                                                " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        mAddParameter(command, "@MY_PASSWORD", MyManager.SecurityManager.getMD5Hash(newPassword.Trim))

        mExecuteNoQuery(command)

        Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
        managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)

        Return True
    End Function

    Public Sub updateEmailChanged(ByVal userId As Long, ByVal email As String)
        Dim strSQL As String = "UPDATE UTENTI SET EMAIL = @email " & _
                                                ", EMAIL_ACTIVATION_CODE = null " & _
                                                ", EMAIL_CHANGING = null " & _
                                                " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL

        Me.mAddParameter(command, "@email", email)

        Me.mExecuteNoQuery(command)
    End Sub

    Public Function updateEmailChanging(ByVal userId As Long, ByVal email As String) As String
        Dim codiceAttivazioneEmail As String
        'codiceAttivazioneEmail = GeneraCodiceRandom()
        codiceAttivazioneEmail = System.Guid.NewGuid.ToString()


        Dim strSQL As String = "UPDATE UTENTI SET EMAIL_CHANGING = @email " & _
                                            ", EMAIL_ACTIVATION_CODE = @codiceAttivazioneEmail " & _
                                            " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL

        Me.mAddParameter(command, "@email", email)
        Me.mAddParameter(command, "@codiceAttivazioneEmail", codiceAttivazioneEmail)

        Me.mExecuteNoQuery(command)

        Return codiceAttivazioneEmail
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



    Public Function enableAccount(ByVal userId As Long, ByVal enabled As Boolean) As Boolean
        Dim manager As New SecurityManager

        'Dim strSQL As String = "UPDATE UTENTI SET MY_PASSWORD = @MY_PASSWORD" & _
        '                                       ", DATE_MODIFIED = NOW " & _
        '                                        " WHERE USER_ID=" & userId

        'Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, mConnection)

        'oleDbCommand.Parameters.Add("@MY_PASSWORD", OleDb.OleDbType.VarChar).Value = MyManager.SecurityManager.getMD5Hash(newPassword.Trim)
        'oleDbCommand.ExecuteNonQuery()

        'Dim managerLogUser As New MyManager.LogUserManager(Me.mConnection)
        'managerLogUser.insert(userId, MyManager.LogUserManager.LogType.UpdatePassword)


        Return True
    End Function



    Public Function getProfilo(ByVal user_id As Long) As System.Data.DataTable
        mStrSQL = "SELECT NOME, COGNOME, MY_LOGIN, MY_PASSWORD, EMAIL, TELEFONO " & _
            ", INDIRIZZO, NUMERO_CIVICO, CAP, PHOTO, HTTP, FAX , SESSO " & _
            ",regione_id , provincia_id, comune_id " & _
            "FROM UTENTI WHERE USER_ID = " & user_id
        Return mFillDataTable(mStrSQL)
    End Function


    Public Function getDateExpireAccount(ByVal userId As Long) As DateTime
        Dim gg As Integer
        Dim dateActivationAccount As DateTime = getDateActivationAccount(userId)
        gg = CInt(System.Configuration.ConfigurationManager.AppSettings("utenti.account.expire.day"))
        Return getDateExpireAccount(gg, dateActivationAccount)
    End Function

    Public Function getDateExpireAccount(ByVal dayExpireAccount As Int16, ByVal dateActivationAccount As DateTime) As DateTime
        Return DateAdd(DateInterval.Day, dayExpireAccount, dateActivationAccount)
    End Function

    Public Function getDateExpirePassword(ByVal userId As Long) As DateTime
        Dim sqlQuery As String = "SELECT DAY_EXPIRE_PASSWORD, DATE_MODIFIED_PASSWORD FROM UTENTI WHERE user_id = " & userId
        Dim row As System.Data.DataRow = Me._fillDataSet(sqlQuery).Tables(0).Rows(0)
        Return getDateExpirePassword(row("DAY_EXPIRE_PASSWORD"), row("DATE_MODIFIED_PASSWORD"))
    End Function

    Public Function getDateExpirePassword(ByVal dayExpirePassword As Int16, ByVal dateModifiedPassword As DateTime) As DateTime
        Return DateAdd(DateInterval.Day, dayExpirePassword, dateModifiedPassword)
    End Function


    Public Function getDateActivationAccount(ByVal userId As Long) As DateTime
        Dim sqlQuery As String
        sqlQuery = "select date_activation_account from utenti where user_id = " & userId

        Return Me.mExecuteScalar(sqlQuery)


    End Function

End Class

