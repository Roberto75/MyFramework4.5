Public Class UserManager2
    Inherits Manager

    Public _nome As String
    Public _cognome As String
    Public _login As String
    Public _password As String
    Public _email As String
    Public _telefono As String
    Public _codiceFiscale As String

    ' Define default min and max password lengths.
    Private Shared DEFAULT_MIN_PASSWORD_LENGTH As Integer = 8
    Private Shared DEFAULT_MAX_PASSWORD_LENGTH As Integer = 10

    ' Define supported password characters divided into groups.
    ' You can add (or remove) characters to (from) these groups.
    Private Shared PASSWORD_CHARS_LCASE As String = "abcdefgijkmnopqrstwxyz"
    Private Shared PASSWORD_CHARS_UCASE As String = "ABCDEFGHJKLMNPQRSTWXYZ"
    Public Const PASSWORD_CHARS_SPECIAL As String = "_.!?/"
    Public Const PASSWORD_CHARS_SPECIAL_DENY As String = "{}[]()-+*@;,|%$#<>'&"""
    ' apici e doppi apici
    Private Shared PASSWORD_CHARS_NUMERIC As String = "0123456789"

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub


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


    Public Function getEmailAdministrators() As DataTable
        Dim sqlQuery As String = "SELECT EMAIL FROM UTENTI WHERE profilo_id = 'ADMIN' AND (DATE_DELETED IS NULL) "
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function

    Public Function updateProfilo(ByVal userId As Long, ByVal profiloNew As String) As Int16
        Dim strSQL As String = "update utenti set Profilo_ID = @profiloNew where user_Id = " & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        Me._addParameter(command, "@profiloNew", profiloNew)
        Return _executeNoQuery(command)
    End Function

    Public Function isVisibleUser(ByVal aziendaId As Long, ByVal userId As Long) As Boolean
        If aziendaId = -1 Then
            Return True
        Else
            Dim ds As DataSet = getUtente(userId)
            Dim customerId As Long = ds.Tables(0).Rows(0).Item("CUSTOMER_ID")
            Return (aziendaId = customerId)
        End If
    End Function

    Public Function getUtentiFromAzienda(ByVal aziendaId As Long) As DataSet
        Dim sqlQuery As String
        sqlQuery = "select * from utenti where customer_id = " & aziendaId
        Return _fillDataSet(sqlQuery)
    End Function

    Public Function registrationIsComplete(ByVal userId As Long) As Boolean
        'verifico che l'utente abbia terminato la procedura di registrazione....
        'dopo che ha ricevuto l'invito 
        Dim sqlQuery As String
        sqlQuery = "select count(*) as tot  from utenti where not(DATE_ACTIVATION_ACCOUNT is null) AND (DATE_DELETED IS NULL)  AND codice_attivazione is null and  user_id = " & userId

        Return CInt(Me._executeScalar(sqlQuery)) > 0
    End Function


    Public Function getDateActivationAccount(ByVal userId As Long) As DateTime
        Dim sqlQuery As String
        sqlQuery = "select date_activation_account from utenti where user_id = " & userId

        Return Me._executeScalar(sqlQuery)

        'Dim dataSet As New DataSet
        'Dim command As System.Data.Common.DbCommand
        'command = _connection.CreateCommand()
        'command.CommandText = sqlQuery
        'dataSet = _fillDataSet(sqlQuery)


        'If Not IsDBNull(dataSet.Tables(0).Rows(0).Item("DATE_ACTIVATION_ACCOUNT")) Then
        '    Return 1
        'Else
        '    Return 0
        'End If
        'Catch ex As Exception

        'End Try
    End Function

    Public Function isEnabled(ByVal userId As Long) As Boolean
        Dim sqlQuery As String
        sqlQuery = "select is_enabled from utenti where user_id = " & userId

        Dim temp As String
        temp = Me._executeScalar(sqlQuery)

        If String.IsNullOrEmpty(temp) Then
            Return False
        End If

        Return CBool(temp)
    End Function
    'resrituisce -1 in caso di errore nelle credenziali oppure ID dell'utente
    Public Function isAuthenticated(ByVal myLogin As String, ByVal myPassword As String, ByRef mySessionData As MyManager.SessionData) As Long

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        Dim dataSet As New DataSet
        Dim row As System.Data.DataRow
        Dim sqlQuery As String
        Dim pwdExpired As Boolean = False
        Dim userId As Long
        Dim customerId As Long
        Dim customer As String
        Dim lastLogin As Date
        Try
            ' sqlQuery = "select user_id from utenti where customer_id = " & customer_id & " and UCASE(my_login) = '" & myLogin.ToUpper.Replace("'", "''") & "'"
            'sqlQuery = "select user_id from utenti where UCASE(my_login) = '" & myLogin.Trim.ToUpper.Replace("'", "''") & "'"

            sqlQuery = "select user_id, date_last_login, DAY_EXPIRE_PASSWORD, DATE_MODIFIED_PASSWORD , utenti.CUSTOMER_ID, customer.RAGIONE_SOCIALE as CUSTOMER " & _
                            " from utenti left join CUSTOMER on utenti.CUSTOMER_ID = CUSTOMER.CUSTOMER_ID " & _
                            " where UPPER(my_login) = @MY_LOGIN and DATE_DELETED is null"


            Me._addParameter(command, "@MY_LOGIN", myLogin.ToUpper.Trim)
            command.CommandText = sqlQuery
            ' verifico l'esistenza dell'utente
            dataSet = _fillDataSet(command)
        Catch ex As Exception
            'Ritorno errore
            Throw New ManagerException("Si è verificato un errore in fase di autenticazione. Non è possibile risalire all'utente.", ex)
        End Try

        'controllo se la password dell'utente è scaduta
        Dim giorniExpire As String = "1000"
        If dataSet.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(dataSet.Tables(0).Rows(0)("DATE_MODIFIED_PASSWORD")) Then
            Dim gg As Integer = CInt(MyManager.GUIManager.getInteger(dataSet.Tables(0).Rows(0)("DAY_EXPIRE_PASSWORD")))
            Dim Data As Date = MyManager.GUIManager.getDateTime(dataSet.Tables(0).Rows(0)("DATE_MODIFIED_PASSWORD"))
            Data = DateAdd(DateInterval.Day, gg, Data)
            giorniExpire = DateDiff("y", FormatDateTime(Now, DateFormat.ShortDate), FormatDateTime(Data, DateFormat.ShortDate))
            If CInt(giorniExpire) <= 0 Then
                ' lancio una eccezione
                Throw New ManagerException(ManagerException.ErrorNumber.PasswordExpired)
            End If
        End If

        REM Controllo se l'utente è censito
        If dataSet.Tables(0).Rows.Count = 0 Then
            'l'utente non è censito
            Throw New ManagerException(ManagerException.ErrorNumber.LoginPasswordErrati)
        End If


        row = dataSet.Tables(0).Rows(0)
        userId = CLng(row("USER_ID"))
        If Not IsDBNull(row("DATE_LAST_LOGIN")) Then
            lastLogin = CDate(row("DATE_LAST_LOGIN"))
        Else
            lastLogin = Now
        End If


        If Not IsDBNull(row("CUSTOMER_ID")) Then
            customerId = row("CUSTOMER_ID")
            customer = row("CUSTOMER")
        Else
            customerId = -1
            customer = ""
        End If




        'dato che l'utente esiste allora carico le sue informazioni
        sqlQuery = "SELECT IS_ENABLED, MY_PASSWORD, NOME, COGNOME, EMAIL, TELEFONO, PROFILO_ID  " & _
                    "  FROM utenti where user_id=" & userId

        dataSet = _fillDataSet(sqlQuery)
        row = dataSet.Tables(0).Rows(0)

        If Not CBool(row("IS_ENABLED")) Then
            Dim ex As New ManagerException(ManagerException.ErrorNumber.UtenteDisabilitato)
            MyManager.MailManager.send(ex)
            Throw ex
        End If

        myPassword = MyManager.SecurityManager.getMD5Hash(myPassword.Trim)
        If row("MY_PASSWORD").ToString.Trim.ToUpper <> myPassword.ToUpper Then
            'Per ORALCE
            'sqlQuery = "update utenti set login_failure=nvl(login_failure,0)+1 where user_id=?"

            sqlQuery = "update utenti set login_failure=login_failure +1 where user_id=" & userId & ""

            command.CommandText = sqlQuery
            command.ExecuteNonQuery()
            Throw New ManagerException(ManagerException.ErrorNumber.LoginPasswordErrati)
        End If

        ' TUTTO OK!!!!
        'metto i dati dell'utente in sessione 
        If Not (mySessionData Is Nothing) Then
            ' sqlQuery = "update UTENTI set date_last_login=sysdate, login_success= login_success +1 " & _
            sqlQuery = "update UTENTI set date_last_login= getdate() , login_success= login_success +1, IP_ADDRESS = @IP " & _
                                     "where user_id=" & userId
            command.Parameters.Clear()
            If (mySessionData.getIp() = "") Then
                _addParameter(command, "@IP", System.DBNull.Value)
            Else
                _addParameter(command, "@IP", mySessionData.getIp())
            End If
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            'Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            'managerLogUser.insert(userId, MyManager.LogUserManager.LogType.Login)

            mySessionData.setUserId(userId)
            mySessionData.setLastLogin(lastLogin)
            mySessionData.setLogin(myLogin)
            mySessionData.setNomeCognome(row("NOME").ToString, row("COGNOME").ToString)
            mySessionData.setEmail(row("EMAIL").ToString)
            If Not IsDBNull(row("TELEFONO")) Then
                mySessionData.setCellulare(row("TELEFONO").ToString)
            Else
                mySessionData.setCellulare("")
            End If
            'Dim autorizzazioni As String
            'autorizzazioni = ";" & getRoles(userId)
            'autorizzazioni &= ";Authenticated;"
            mySessionData.setProfiloID(row("PROFILO_ID"))
            mySessionData.setAuthorizations(";" & row("PROFILO_ID") & ";")

            If (customerId <> -1 AndAlso customer <> "") Then
                mySessionData.setCustomerID(customerId)
                mySessionData.setCustomer(customer)
            Else
                mySessionData.setCustomerID(-1)
                mySessionData.setCustomer("")
            End If
            ' mySessionData.setAuthorizations(";" & Me.getCodiciAutorizzazioniForUser(userId, "PORTALE"))
        End If

        If giorniExpire = 1 Then
            mySessionData.setJavaScriptMessage("La Password scade fra " & giorniExpire & " giorno")
        ElseIf giorniExpire < 10 AndAlso giorniExpire > 0 Then
            mySessionData.setJavaScriptMessage("La Password scade fra " & giorniExpire & " giorni")
        End If

        Return userId
    End Function

    ' Cancellazione logica dell'account
    Public Sub deleteAccount(ByVal userId As Long)
        Dim sqlQuery As String

        sqlQuery = "Update UTENTI SET IS_ENABLED = 0, DATE_DELETED = getDate() where user_id=" & userId
        _executeNoQuery(sqlQuery)
    End Sub

    Public Function getUtente(ByVal userId As Long) As System.Data.DataSet
        Dim sqlQuery As String

        sqlQuery = "SELECT * FROM utenti where user_id=" & userId

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function getUtenteFromCodiceAttivazione(ByVal codiceAttivazione As String) As DataTable
        Dim sqlQuery As String = "SELECT * FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND (CODICE_ATTIVAZIONE = @CODICE_ATTIVAZIONE)"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = sqlQuery

        Me._addParameter(command, "@CODICE_ATTIVAZIONE", codiceAttivazione.Trim())

        Return _fillDataSet(command).Tables(0)
    End Function

    Public Sub terminaRegistrazioneInvito(ByVal userId As Long)
        ' Il codice_attivazione viene valorizzato a NULL altrimenti l'utente potrebbe ri-eseguire la procedura di registrazione
        Dim strSQL As String = "UPDATE UTENTI SET DATE_ACTIVATION_ACCOUNT = getDate(), DATE_MODIFIED = getDate(), CODICE_ATTIVAZIONE = null "
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        If Me._login <> "" Then
            strSQL &= " ,MY_LOGIN = @MY_LOGIN"
            Me._addParameter(command, "@MY_LOGIN", Me._login)
        End If

        If Me._nome <> "" Then
            strSQL &= " ,NOME = @NOME"
            Me._addParameter(command, "@NOME", Me._nome)
        End If

        If Me._codiceFiscale <> "" Then
            strSQL &= " ,CODICE_FISCALE = @CODICE_FISCALE"
            Me._addParameter(command, "@CODICE_FISCALE", Me._codiceFiscale)
        End If

        If Me._cognome <> "" Then
            strSQL &= " ,COGNOME = @COGNOME"
            Me._addParameter(command, "@COGNOME", Me._cognome)
        End If

        If Me._email <> "" Then
            strSQL &= " ,EMAIL = @EMAIL"
            Me._addParameter(command, "@EMAIL", Me._email)
        End If

        If Me._telefono <> "" Then
            strSQL &= " ,TELEFONO = @TELEFONO"
            Me._addParameter(command, "@TELEFONO", Me._telefono)
        End If

        strSQL &= " WHERE USER_ID=" & userId
        command.CommandText = strSQL
        command.ExecuteNonQuery()

    End Sub

    Public Function insertUtente(ByVal nome As String, ByVal cognome As String, ByVal email As String, ByVal codiceAttivazione As String, ByVal customerId As Long, ByVal profiloId As String) As Long
        Dim strSQL As String = " INSERT INTO UTENTI (DATE_ADDED, NOME, COGNOME, EMAIL, CODICE_ATTIVAZIONE, PROFILO_ID, CUSTOMER_ID) " & _
                                            " VALUES (getdate(),  @NOME , @cognome , @email , @codiceAttivazione , @profiloId, @customerId) "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        Me._addParameter(command, "@NOME", nome)
        Me._addParameter(command, "@cognome", cognome)
        Me._addParameter(command, "@email", email)
        Me._addParameter(command, "@codiceAttivazione", codiceAttivazione)
        Me._addParameter(command, "@profiloId", profiloId)
        Me._addParameter(command, "@customerId", customerId)


        Me._executeNoQuery(command)
        Return _getIdentity()
    End Function

    Public Sub updateEmailChanged(ByVal userId As Long, ByVal email As String)
        Dim strSQL As String = "UPDATE UTENTI SET EMAIL = @email " & _
                                             ", EMAIL_ACTIVATION_CODE = null " & _
                                              ", EMAIL_CHANGING = null " & _
                                               " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        Me._addParameter(command, "@email", email)

        Me._executeNoQuery(command)
    End Sub

    Public Function updateEmailChanging(ByVal userId As Long, ByVal email As String) As String
        Dim codiceAttivazioneEmail As String
        'codiceAttivazioneEmail = GeneraCodiceRandom()
        codiceAttivazioneEmail = System.Guid.NewGuid.ToString()


        Dim strSQL As String = "UPDATE UTENTI SET EMAIL_CHANGING = @email " & _
                                            ", EMAIL_ACTIVATION_CODE = @codiceAttivazioneEmail " & _
                                            " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        Me._addParameter(command, "@email", email)
        Me._addParameter(command, "@codiceAttivazioneEmail", codiceAttivazioneEmail)

        Me._executeNoQuery(command)

        Return codiceAttivazioneEmail
    End Function



   



    'Public Sub updateTelefono(ByVal userId As Long, ByVal telefono As String)
    '    Dim strSQL As String = "UPDATE UTENTI SET TELEFONO = '" & telefono & "' WHERE USER_ID=" & userId
    '    Me._executeNoQuery(strSQL)
    'End Sub


    Public Function updateCodiceAttivazioneAccount(ByVal userId As Long) As String
        Return updateCodiceAttivazioneAccount(userId, GeneraCodiceRandom)
    End Function

    Public Function updateCodiceAttivazioneAccount(ByVal userId As Long, ByVal codice As String) As String
        Dim strSQL As String = "UPDATE UTENTI SET CODICE_ATTIVAZIONE = @codice WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        Me._addParameter(command, "@codice", codice)
        
        Me._executeNoQuery(command)

        Return codice
    End Function

    Public Sub updateUser(ByVal userId As Long, ByVal telefono As String)
        Dim strSQL As String = "UPDATE UTENTI SET DATE_MODIFIED = getDate() "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL

        If (telefono <> "-1") Then
            strSQL &= ", TELEFONO = @telefono "
            Me._addParameter(command, "@telefono", telefono)
        End If

        strSQL &= " WHERE USER_ID=" & userId

        Me._executeNoQuery(command)

    End Sub

    Public Function enableAccount(ByVal userId As Long, ByVal enabled As Boolean) As String
        'Viene eseguito dall'amministratore!!!

        Dim manager As New SecurityManager
        Dim password As String = ""
        Dim strSQL As String = "UPDATE UTENTI SET IS_ENABLED = @IS_ENABLED" & _
                                             ", DATE_MODIFIED = getDate() "
        Dim where As String = " WHERE USER_ID=" & userId

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        ' verifico se devo generare la password o meno 
        If CBool(enabled) Then
            password = resetPassword(userId)
            strSQL &= " , MY_PASSWORD = '" & MyManager.SecurityManager.getMD5Hash(password) & "'"
            Me._addParameter(command, "@IS_ENABLED", 1)
        Else
            strSQL &= " , MY_PASSWORD =  null "
            Me._addParameter(command, "@IS_ENABLED", 0)
        End If

        command.CommandText = strSQL & where
        command.ExecuteNonQuery()
        Return password
    End Function

    Public Shared Function getRandomIP()
        Dim random As Random = New Random()
        Return String.Format("{0}.{1}.{2}.{3}", random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255))
    End Function


    Public Shared Function getRandomUser()
        Dim random As Random = New Random()
        Return String.Format("USER_{0}{1}{2}{3}", random.Next(0, 1000), random.Next(0, 1000), random.Next(0, 1000), random.Next(0, 1000))
    End Function


    'verifico che non sia già presente un codice invito nel sistema
    Public Function verificaCodiceInvito(ByVal value As String) As Boolean
        Dim strSQL As String = "SELECT count(*) as tot  FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  codice_attivazione = @codice_attivazione"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        command.CommandText = strSQL
        Me._addParameter(command, "@codice_attivazione", value)

        Return CInt(Me._executeScalar(command)) > 0
    End Function

    Public Shared Function GeneraCodiceRandom(ByVal length As Integer) As String
        Return MyManager.UserManager2.GeneratePassword(length)
    End Function

    Public Shared Function GeneraCodiceRandom() As String
        Return MyManager.UserManager2.GeneratePassword(4, 8)
    End Function

    Public Shared Function GeneratePassword() As String
        Return MyManager.UserManager2.GeneratePassword(DEFAULT_MIN_PASSWORD_LENGTH, _
                            DEFAULT_MAX_PASSWORD_LENGTH)
    End Function

    Public Shared Function GeneratePassword(ByVal length As Integer) As String
        Return MyManager.UserManager2.GeneratePassword(length, length)
    End Function



    Public Shared Function GeneratePassword(ByVal minLength As Integer, ByVal maxLength As Integer) As String

        ' Make sure that input parameters are valid.
        If (minLength <= 0 Or maxLength <= 0 Or minLength > maxLength) Then
            Return Nothing
        End If

        ' Create a local array containing supported password characters
        ' grouped by types. You can remove character groups from this
        ' array, but doing so will weaken the password strength.
        Dim charGroups As Char()() = New Char()() _
        { _
            PASSWORD_CHARS_LCASE.ToCharArray(), _
            PASSWORD_CHARS_UCASE.ToCharArray(), _
            PASSWORD_CHARS_NUMERIC.ToCharArray(), _
            PASSWORD_CHARS_SPECIAL.ToCharArray() _
        }

        ' Use this array to track the number of unused characters in each
        ' character group.
        Dim charsLeftInGroup As Integer() = New Integer(charGroups.Length - 1) {}

        ' Initially, all characters in each group are not used.
        Dim I As Integer
        For I = 0 To charsLeftInGroup.Length - 1
            charsLeftInGroup(I) = charGroups(I).Length
        Next

        ' Use this array to track (iterate through) unused character groups.
        Dim leftGroupsOrder As Integer() = New Integer(charGroups.Length - 1) {}

        ' Initially, all character groups are not used.
        For I = 0 To leftGroupsOrder.Length - 1
            leftGroupsOrder(I) = I
        Next

        ' Because we cannot use the default randomizer, which is based on the
        ' current time (it will produce the same "random" number within a
        ' second), we will use a random number generator to seed the
        ' randomizer.

        ' Use a 4-byte array to fill it with random bytes and convert it then
        ' to an integer value.
        Dim randomBytes As Byte() = New Byte(3) {}

        ' Generate 4 random bytes.
        Dim rng As System.Security.Cryptography.RNGCryptoServiceProvider = New System.Security.Cryptography.RNGCryptoServiceProvider

        rng.GetBytes(randomBytes)

        ' Convert 4 bytes into a 32-bit integer value.
        Dim seed As Integer = ((randomBytes(0) And &H7F) << 24 Or _
                                randomBytes(1) << 16 Or _
                                randomBytes(2) << 8 Or _
                                randomBytes(3))

        ' Now, this is real randomization.
        Dim random As Random = New Random(seed)

        ' This array will hold password characters.
        Dim password As Char() = Nothing

        ' Allocate appropriate memory for the password.
        If (minLength < maxLength) Then
            password = New Char(random.Next(minLength - 1, maxLength)) {}
        Else
            password = New Char(minLength - 1) {}
        End If

        ' Index of the next character to be added to password.
        Dim nextCharIdx As Integer

        ' Index of the next character group to be processed.
        Dim nextGroupIdx As Integer

        ' Index which will be used to track not processed character groups.
        Dim nextLeftGroupsOrderIdx As Integer

        ' Index of the last non-processed character in a group.
        Dim lastCharIdx As Integer

        ' Index of the last non-processed group.
        Dim lastLeftGroupsOrderIdx As Integer = leftGroupsOrder.Length - 1

        ' Generate password characters one at a time.
        For I = 0 To password.Length - 1

            ' If only one character group remained unprocessed, process it;
            ' otherwise, pick a random character group from the unprocessed
            ' group list. To allow a special character to appear in the
            ' first position, increment the second parameter of the Next
            ' function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            If (lastLeftGroupsOrderIdx = 0) Then
                nextLeftGroupsOrderIdx = 0
            Else
                nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx)
            End If

            ' Get the actual index of the character group, from which we will
            ' pick the next character.
            nextGroupIdx = leftGroupsOrder(nextLeftGroupsOrderIdx)

            ' Get the index of the last unprocessed characters in this group.
            lastCharIdx = charsLeftInGroup(nextGroupIdx) - 1

            ' If only one unprocessed character is left, pick it; otherwise,
            ' get a random character from the unused character list.
            If (lastCharIdx = 0) Then
                nextCharIdx = 0
            Else
                nextCharIdx = random.Next(0, lastCharIdx + 1)
            End If

            ' Add this character to the password.
            password(I) = charGroups(nextGroupIdx)(nextCharIdx)

            ' If we processed the last character in this group, start over.
            If (lastCharIdx = 0) Then
                charsLeftInGroup(nextGroupIdx) = _
                                charGroups(nextGroupIdx).Length
                ' There are more unprocessed characters left.
            Else
                ' Swap processed character with the last unprocessed character
                ' so that we don't pick it until we process all characters in
                ' this group.
                If (lastCharIdx <> nextCharIdx) Then
                    Dim temp As Char = charGroups(nextGroupIdx)(lastCharIdx)
                    charGroups(nextGroupIdx)(lastCharIdx) = _
                                charGroups(nextGroupIdx)(nextCharIdx)
                    charGroups(nextGroupIdx)(nextCharIdx) = temp
                End If

                ' Decrement the number of unprocessed characters in
                ' this group.
                charsLeftInGroup(nextGroupIdx) = _
                           charsLeftInGroup(nextGroupIdx) - 1
            End If

            ' If we processed the last group, start all over.
            If (lastLeftGroupsOrderIdx = 0) Then
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1
                ' There are more unprocessed groups left.
            Else
                ' Swap processed group with the last unprocessed group
                ' so that we don't pick it until we process all groups.
                If (lastLeftGroupsOrderIdx <> nextLeftGroupsOrderIdx) Then
                    Dim temp As Integer = _
                                leftGroupsOrder(lastLeftGroupsOrderIdx)
                    leftGroupsOrder(lastLeftGroupsOrderIdx) = _
                                leftGroupsOrder(nextLeftGroupsOrderIdx)
                    leftGroupsOrder(nextLeftGroupsOrderIdx) = temp
                End If

                ' Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1
            End If
        Next

        ' Convert password characters into a string and return the result.
         Return New String(password)
    End Function

    Private Function verificaLogin(ByVal login As String) As Boolean
        Dim m As New MyManager.UserManager(_connection)
        Return m.verificaLogin(login)
    End Function

    Public Function getNewEmailChanging(ByVal activationCodeEmail As String, ByVal userId As Long) As String
        Dim sqlQuery As String
        sqlQuery = "SELECT EMAIL_CHANGING FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  USER_ID = " & userId & " and EMAIL_ACTIVATION_CODE = '" & activationCodeEmail.Replace("'", "''") & "'"
        Return (Me._executeScalar(sqlQuery)).ToString
    End Function


    Public Function verificaCodiceFiscale(ByVal codFiscale As String, ByVal customerId As Long) As Boolean
        Dim sqlQuery As String = "SELECT count(*) FROM CUSTOMER WHERE CODICE_FISCALE = @codFiscale "

        If customerId <> -1 Then
            sqlQuery &= " AND CUSTOMER_ID <> " & customerId
        End If

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = sqlQuery

        Me._addParameter(command, "@codFiscale", codFiscale)

        Return CInt(Me._executeScalar(command)) > 0
    End Function

    Public Function verificaCodiceFiscale(ByVal codFiscale As String) As Boolean
        Return verificaCodiceFiscale(codFiscale, -1)
    End Function

    Public Function verificaEmailUtente(ByVal email As String, ByVal userId As Long) As Boolean
        Dim sqlQuery As String = "SELECT count(*) FROM UTENTI WHERE (EMAIL =  @email or EMAIL_CHANGING = @email2 ) and DATE_DELETED is null "

        If userId <> -1 Then
            sqlQuery &= " AND USER_ID <> " & userId
        End If
        sqlQuery &= " AND PROFILO_ID = 'DELEGATO'"

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = sqlQuery

        Me._addParameter(command, "@email", email)
        Me._addParameter(command, "@email2", email)

        Return CInt(Me._executeScalar(command)) > 0

    End Function

    Public Function verificaEmailUtente(ByVal email As String) As Boolean
        Return verificaEmailUtente(email, -1)
    End Function

    Public Function verificaEmailDelegato(ByVal email As String) As Boolean
        Return verificaEmailDelegato(email, -1)
    End Function

    Public Function verificaEmailDelegato(ByVal email As String, ByVal userId As Long) As Boolean
        Dim sqlQuery As String = "SELECT count(*) FROM UTENTI WHERE (EMAIL = @email  or EMAIL_CHANGING = @email2 ) and DATE_DELETED is null "

        If userId <> -1 Then
            sqlQuery &= " AND USER_ID <> " & userId
        End If

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = sqlQuery

        Me._addParameter(command, "@email", email)
        Me._addParameter(command, "@email2", email)

        Return CInt(Me._executeScalar(command)) > 0

    End Function

    Public Function getCodiceFiscale(ByVal userId As Long) As String
        Dim sqlQuery As String = "SELECT CODICE_FISCALE FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  USER_ID = " & userId
        Return Me._executeScalar(sqlQuery)
    End Function


    ' recupero tutti gli utenti con un particolare profil_id
    Public Function cercaDestinatari(ByVal profilo_id As String) As DataTable

        Dim sqlQuery As String = "SELECT EMAIL FROM UTENTI WHERE  (DATE_DELETED IS NULL)  AND  IS_ENABLED = 1 AND PROFILO_ID = '" & profilo_id.Replace("'", "''") & "'"
        Return _fillDataSet(sqlQuery).Tables(0)

    End Function

    Public Function creaLogin(ByVal nome As String, ByVal cognome As String) As String
        Dim m As New MyManager.UserManager(Me._connection)
        Return m.creaLogin(nome, cognome)
    End Function

    Public Function getLogin(ByVal user_id As Long) As String
        Dim strSQL As String = "SELECT MY_LOGIN FROM UTENTI WHERE user_id = " & user_id
        Return Me._executeScalar(strSQL)
    End Function


    Public Function getNomeAziendaByUSerId(ByVal user_id As Long) As String
        Dim strSQL As String = "SELECT customer.RAGIONE_SOCIALE    FROM [TechSecurity].[UTENTI] join CUSTOMER on utenti.CUSTOMER_ID  = customer.CUSTOMER_ID " & _
                "   where user_id = " & user_id
        Return Me._executeScalar(strSQL)
    End Function


    Public Function getEmail(ByVal user_id As Long) As String
        Dim strSQL As String = "SELECT EMAIL FROM UTENTI WHERE user_id = " & user_id
        Return Me._executeScalar(strSQL)
    End Function

    Public Function getUserIdFromLoginAndEmail(ByVal login As String, ByVal email As String) As Long
        Dim sqlQuery As String = ""

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        'eseguo questa doppia query come controllo
        sqlQuery = "select USER_ID from  utenti  " & _
                    "where (DATE_DELETED is NULL) AND  UPPER(MY_LOGIN)=@mylogin  "

        Me._addParameter(command, "mylogin", login.ToUpper)

        
        If email <> "" Then
            sqlQuery &= " and UPPER(EMAIL)=  @email"
            Me._addParameter(command, "email", email.ToUpper)
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


    'Public Function resetPassword(ByVal userId As Long) As String
    '    Dim email As String
    '    email = getEmail(userId)
    '    Return resetPassword(userId, email)

    'End Function

    ''' <summary>
    ''' Reset della Password
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>Viene generata una Password in modo Random, Codificata in MD5, e inoltrata una mail dell'utente corrente.
    ''' occore impostare lo User_id e la mail dell'utente in esame
    ''' </remarks>
    Public Function resetPassword(ByVal userId As Long) As String
        Dim passwordGenerata As String

        'faccio un controllo sulla password generata!!!!
        passwordGenerata = GeneratePassword(10)
        While Not UserManager.isStrongPassword(passwordGenerata)
            passwordGenerata = GeneratePassword(10)
        End While

        Dim esito As Boolean = False
        Dim sqlQuery As String = "UPDATE UTENTI SET DATE_MODIFIED = getDate(), DATE_MODIFIED_PASSWORD = getDate()" & _
                    " ,MY_PASSWORD=@MY_Password   WHERE USER_ID=@UserId"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()


        Try

            command.CommandText = sqlQuery

            Me._addParameter(command, "@MY_Password", MyManager.SecurityManager.getMD5Hash(passwordGenerata))
            Me._addParameter(command, "@UserId", userId)

            Me._executeNoQuery(command)


            Dim managerLogUser As New MyManager.LogUserManager(Me._connection)
            managerLogUser.insert(userId, MyManager.LogUserManager.LogType.ResetPassword)


            esito = True
        Catch ex As Exception
            Throw New MyManager.ManagerException("Errore nella funzione di 'Reset Password", ex)
        End Try

        Return passwordGenerata
    End Function

End Class
