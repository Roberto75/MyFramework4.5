Imports Microsoft.VisualBasic


'Questa classe deve essere ereditatata per personalizzare i messaggi
'tra cui _http e _applicationName
'In App_Code della WebApplication definisco MyEmailMessage

Public Class MailMessage
    Inherits MyManager.MailManager

    Public Enum Lingua
        IT = 0
        EN = 1
    End Enum

    Public _http As String
    Public _applicationName As String

    Public Sub New(http As String, applicationName As String)
        _http = http
        _applicationName = applicationName
    End Sub


    Public Overridable Function getFirma(Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim risultato As String = ""
        Select Case lingua
            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                risultato = "<br />Cordiali saluti dallo staff di " & _applicationName & "." & vbCrLf & _
                      "<br><br><br> " & _applicationName & vbCrLf & _
                      "<br><a href=""" & _http & """>" & _http & "</a>" & vbCrLf
        End Select
        Return risultato
    End Function


    Public Overridable Function getBodyDonazione() As String
        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        "Gentile utente,  " & _
        "<br /> Sperando che il servizio offerto sia stato gradito, ti ricordiamo la possibilità di eseguire una donazione in modo sicuro e protetto, tramite il servizio di PAYPAL" & _
        "<p>Fai una donazione, grande o piccola che sia.</p>" & _
        "<p>Il tuo contributo ci aiuterà a mantenere il servizio gratuito e a ripagare le spese di gestione del dominio e dello spazio web.</p>" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "donate/default.aspx"">qui</a> per effettuare una donazione.<br /><br />"

        temp &= getFirma()

        Return temp
    End Function



#Region "Utenti"

    Public Overridable Function getBodyRegistrazioneUtente(ByVal nome As String, ByVal cognome As String, _
            ByVal login As String, ByVal email As String, _
            ByVal telefono As String, ByVal indirizzo As String, ByVal numeroCivico As String, ByVal cap As String, ByVal provincia As String, ByVal comune As String, _
            Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String

        Dim temp As String = ""
        Dim nome_cognome As String

        If String.IsNullOrEmpty(cognome) Then
            nome_cognome = nome
        Else
            nome_cognome = nome & " " & cognome
        End If

        Select Case lingua
            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Registrazione al Portale di " & _applicationName & "</h1>" & vbCrLf & _
                "Gentile " & nome_cognome & ",  " & vbCrLf & _
                "<br /> benvenuto come utente registrato del Portale di " & _applicationName & ". Per accedere al Portale utilizza le seguenti credenziali, digitando correttamente lettere maiuscole e minuscole:" & _
                "<br />" & vbCrLf & _
                "<br /> <b>Login</b>: " & login & vbCrLf & _
                "<br />" & vbCrLf & _
                "<p>Per questione di sicurezza riceverai una <b>seconda email con una password</b> generata in automatico dal sistema.</p>" & vbCrLf

                If CBool(System.Configuration.ConfigurationManager.AppSettings("utenti.password.canSet")) Then
                    temp &= "<p>In seguito, se lo desideri, potrai effettuare il login e accedere al menu ""Utente"" per impostare una password in modo che sia più semplice ricordarla per te. </p>" & vbCrLf & _
                             "<br /> " & vbCrLf
                End If

                temp &= "<p>In un secondo momento potrai sempre accedere al menu ""Utente"" per <b>modificare e aggiornare i tuoi dati anagrafici</b> dopo esserti loggato nel sistema.</p>" & vbCrLf & _
                "<br />" & vbCrLf & _
                "<p>Email: " & email & " </p>" & vbCrLf & _
                "<p>Telefono: " & telefono & " </p>" & vbCrLf & _
                "<br />" & vbCrLf & _
                "<p>Indirizzo: " & indirizzo & " n° " & numeroCivico & ", " & cap & " - " & comune & " (" & provincia & ") </p>" & vbCrLf & _
                "<br /> " & vbCrLf & _
                "<p>Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale.</p>" & vbCrLf & _
                "<p>Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione ""Genera password"" presente nel menu ""Utente"".</p>" & vbCrLf
        End Select


        temp &= getFirma()

        Return temp
    End Function

    Public Overridable Function getBodyNotificaRegistrazioneUtente(ByVal nome As String, ByVal cognome As String, _
           ByVal email As String, _
           ByVal telefono As String) As String

        Dim temp As String = ""
        temp = "<h1>Notifica Registrazione al Portale di " & _applicationName & "</h1>" & vbCrLf & _
        "Gentile " & nome & " " & cognome & ",  " & vbCrLf & _
        "<br /> benvenuto nel Portale " & _applicationName & ". " & vbCrLf & _
        "<br />" & vbCrLf & _
        "<br />Ti sarà inviata una mail con la Login da utilizzare per accedere ai servizi del Portale. " & vbCrLf & _
        "<p>Per questione di sicurezza riceverai una <b>seconda email con una password</b> generata in automatico dal sistema.</p>" & vbCrLf & _
        "<p>In seguito, se lo desideri, potrai effettuare il login e accedere al menu ""Utente"" per impostare una password in modo che sia più semplice ricordarla per te. </p>" & vbCrLf & _
        "<br /> " & vbCrLf & _
        "<p>In un secondo momento potrai sempre accedere al menu ""Utente"" per <b>modificare e aggiornare i tuoi dati anagrafici</b> dopo esserti loggato nel sistema.</p>" & vbCrLf & _
        "<br />" & vbCrLf & _
        "<p>Email: " & email & " </p>" & vbCrLf & _
        "<p>Telefono: " & telefono & " </p>" & vbCrLf & _
        "<br />" & vbCrLf & _
        "<br /> " & vbCrLf & _
        "<p>Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale.</p>" & vbCrLf & _
        "<p>Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione ""Genera password"" presente nel menu ""Utente"".</p>" & vbCrLf

        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyInvioCodiceCambioEmail(ByVal nome, ByVal cognome, ByVal codice) As String
        Dim temp As String = ""
        temp = "<h1>Verifica nuovo indirizzo email " & _applicationName & "</h1>" & vbCrLf & _
        "Gentile " & nome & " " & cognome & ",  " & vbCrLf & _
        "<br/> <p>con la presente le inviamo un codice per confermare la sua nuova Email.<p>" & vbCrLf & _
        "<br/> <p> Acceda al Portale" & _applicationName & " dal seguente al seguente indirizzo: <a href=""" & _http & "utenti/confermaEmail.aspx?uid=" & codice & """ traget=""_blank"">Conferma Email</a>  e digiti il codice sotto.</p>" & vbCrLf & _
        "<p><b>Codice: </b>" & codice & "</p>" & vbCrLf
        temp &= getFirma()
        Return temp
    End Function


    Public Overridable Function getBodyInvioCodiceAttivazione(ByVal nome As String, ByVal cognome As String, ByVal codice As String) As String

        Dim temp As String = ""
        temp = "<h1>Codice Invito per " & _applicationName & "</h1>" & vbCrLf & _
        "Gentile " & nome & " " & cognome & ",  " & vbCrLf & _
        "<br/> <p>con la presente le inviamo un codice personale per procedere alla registrazione nel Portale " & _applicationName & ".</p>" & vbCrLf & _
        "<p>Codice: <b>" & codice & "</b></p>" & vbCrLf & _
        "<p>Con questo codice può completare la procedura di registrazione al seguente indirizzo: <a href=""" & _http & "utenti/registrazione.aspx"" traget=""_blank"">Registrazione</a> per ottenere la Login e la Password necessarie ad accedere al Sistema.</p>" & vbCrLf
        temp &= getFirma()
        Return temp

    End Function

    Public Overridable Function getBodyLoginUtente(ByVal nome As String, ByVal cognome As String, _
                ByVal email As String, _
                ByVal telefono As String, ByVal login As String) As String

        Dim temp As String = ""
        temp = "<h1>Credenziali accesso al Portale di " & _applicationName & "</h1>" & vbCrLf & _
        "Gentile utente " & nome & " " & cognome & ",  " & vbCrLf & _
        "<br /> Queste sono le credenziali che dovrà utilizzare per accedere ai servizi del Portale" & vbCrLf & _
        "<p>Credenziali Utente : </p>" & vbCrLf & _
        "<br />" & vbCrLf & _
        "<p>Login: <b> " & login & " </b> </p>" & vbCrLf & _
        "<p>Nome: " & nome & " </p>" & vbCrLf & _
        "<p>Cognome: " & cognome & " </p>" & vbCrLf & _
        "<p>Email: " & email & " </p>" & vbCrLf & _
        "<p>Telefono: " & telefono & " </p>" & vbCrLf

        temp &= getFirma()
        Return temp
    End Function



    Public Overridable Function getBodyResetPassword(ByVal nome As String, ByVal cognome As String, ByVal passwordGenerata As String, Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim temp As String = ""
        Select Case lingua
            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Generazione nuova password</h1>" & vbCrLf & _
                "Gentile " & nome & " " & cognome & "," & vbCrLf & _
                "<br /> come richiesto dal servizio ""Genera Password"" ti inviamo questo messaggio con le tue credenziali." & vbCrLf & _
                "<p>Per accedere al Portale utilizza le seguenti credenziali, digitando correttamente lettere maiuscole e minuscole: </p>" & vbCrLf & _
                "<br />" & vbCrLf & _
                "<br/> Password: <b>" & passwordGenerata & "</b> <br /> " & vbCrLf & _
                "<p> In seguito potrai effettuare il login e impostare una password in modo che sia più semplice ricordarla per te.</p>" & vbCrLf & _
                "<p> Per questione di sicurezza le consigliamo di modificare periodicamnte la tua password tramite la funzione ""Modifica password"" del menu Utente. </p>" & vbCrLf & _
                "<p> Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale. </p>" & vbCrLf & _
                "<p> Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione ""Genera password"" presente nel menu ""Utente"". </p>" & vbCrLf
        End Select
        temp &= getFirma()

        Return temp
    End Function

    Public Overridable Function getBodyModificaPassword(ByVal nome As String, ByVal cognome As String, ByVal newPassword As String, Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim temp As String = ""
        Select Case lingua

            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Modifica della password</h1>" & vbCrLf & _
             " Gentile " & nome & " " & cognome & "," & vbCrLf & _
             " <br />La informiano che è avvenuta la modifica della sua password." & vbCrLf & _
             " <br />La nuova password è: " & newPassword & _
             " <p>Nel caso in cui non sia stato/a lei ad effettuare tale operazione, la preghiamo di comunicarcelo per <a href=""mailto:" & System.Configuration.ConfigurationManager.AppSettings("mail.From") & """>email</a>.</p> " & vbCrLf & _
             " <br>" & vbCrLf & _
             " <p>Per questione di sicurezza le consigliamo di modificare periodicamnte la tua password tramite la funzione ""Modifica password"" del menu Utente. </p>" & vbCrLf & _
             " <p>Le consigliamo di conservare o stampare questa mail come promemoria. In caso di smarrimento della password potrà, comunque, utilizzare la funzione ""Genera password"" presente nel menu ""Utente"".</p>" & vbCrLf
        End Select

        temp &= getFirma()

        Return temp

    End Function

    Public Overridable Function getBodyAccountEnabled(ByVal nome As String, ByVal cognome As String, ByVal passwordGenerata As String, Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim temp As String = ""
        Select Case lingua
            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Account abilitato</h1>" & vbCrLf & _
                " Gentile " & nome & " " & cognome & "," & vbCrLf & _
                " <br/>La informiano che è il suo accout è stato <b>abilitato</b>." & vbCrLf & _
                "<p>Per accedere al Portale utilizza la login scelta durante la procedura di registrazione e la seguente password digitando correttamente lettere maiuscole e minuscole: </p>" & vbCrLf & _
                "<br />" & vbCrLf & _
                "<br /> Password: <b>" & passwordGenerata & "</b> <br /> " & vbCrLf & _
                "<p> In seguito potrai effettuare il login e impostare una password in modo che sia più semplice ricordarla per te.</p>" & vbCrLf & _
                "<p> Per questione di sicurezza le consigliamo di modificare periodicamnte la tua password tramite la funzione ""Modifica password"" del menu Utente. </p>" & vbCrLf & _
                "<p> Ti rammentiamo che come utente registrato puoi accedere gratuitamente a tutti servizi on line individuali del Portale. </p>" & vbCrLf & _
                "<p> Conserva o stampa questa mail come promemoria. In caso di smarrimento della password potrai, comunque, utilizzare la funzione ""Genera password"" presente nel menu ""Utente"". </p>" & vbCrLf
        End Select

        temp &= getFirma()

        Return temp

    End Function

    Public Overridable Function getBodyAccountDisabled(ByVal nome As String, ByVal cognome As String, Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim temp As String = ""
        Select Case lingua

            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Account disabilitato</h1>" & vbCrLf & _
             " Gentile " & nome & " " & cognome & "," & vbCrLf & _
             " <br/>La informiano che il suo account è stato <b>disabilitato</b>." & vbCrLf & _
             " <p>Per qualsisi tipo di comunicazione la preghiamo di comunicarcelo per <a href=""mailto:" & System.Configuration.ConfigurationManager.AppSettings("mail.From") & """>email</a>.</p> " & vbCrLf
        End Select

        temp &= getFirma()

        Return temp

    End Function

    Public Overridable Function getBodyAlertNewAccount(ByVal nome As String, ByVal cognome As String, ByVal login As String, ByVal userId As Long, ByVal profilo As String, Optional ByVal lingua As MailMessage.Lingua = Lingua.IT) As String
        Dim temp As String = ""
        Select Case lingua

            Case MailMessage.Lingua.IT, MailMessage.Lingua.EN
                temp = "<h1>Attivazione nuovo account</h1>" & vbCrLf & _
                " Gentile amministratore," & vbCrLf & _
                "<br/>La informiamo che il " & Now() & " un nuovo utente si è registrato al portale." & vbCrLf & _
                "<p>Per consentire l'accesso al nuovo utente occorre accedere al pannello di amministrazione degli utenti e abilitare il nuovo account.</p>" & vbCrLf & _
                "<p>Di seguito vengono riportati i dati nel nuovo utente per poterlo individuare e abilitare:</p>" & vbCrLf


                If Not String.IsNullOrEmpty(profilo) Then
                    temp = temp & "<br />Profilo : " & profilo & vbCrLf
                End If

                temp = temp & _
                "<br />Nome: " & nome & vbCrLf & _
                "<br />Cognome: " & cognome & vbCrLf & _
                "<br />Login: " & login & vbCrLf & _
                "<br />User ID: " & userId & vbCrLf & _
                "<p>Clicca sul link per accedere al <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "admin/"" >Pannello di amministrazione<a></p>" & vbCrLf
        End Select

        temp &= getFirma()

        Return temp

    End Function
#End Region


#Region "Forum"
    ''' <summary>
    ''' Notifica dell'inserimento di un nuovo post
    ''' </summary>
    Public Overridable Function getBodyForumReply(ByVal threadId As Long, ByVal nomeThread As String, ByVal nickName As String) As String
        'una volta inserito un post invio un'email a chi ha aperto il Thread

        Dim temp As String = ""
        temp = "<h1>Il Forum di " & _applicationName & "</h1>" & _
        " <p>Gentile  {0},  " & _
        "<br /> hai ricevuto un nuovo messaggio nel Thread: {1} " & _
        "<br />" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "forum/thread.aspx?threadId={2}"">qui</a> per visualizzare il Thread.<br /><br />"

        temp = String.Format(temp, nickName, nomeThread, threadId)

        temp &= getFirma()


        Return temp
    End Function
#End Region


#Region "Mercatino"

    Public Overridable Function getBodyMercatinoModificaTestoAnnuncio(ByVal annuncio_id As Long, ByVal titoloAnnuncio As String) As String
        'una volta inserito un post invio un'email a chi ha aperto il Thread

        Dim temp As String = ""
        temp = "<h1>Il Mercatino di " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> Il proprietartio dell'annuncio ""{0}"" ha modificato il testo del descrizione." & _
        "<br /> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/annuncio.aspx?annuncioId={1}"">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />"

        temp = String.Format(temp, titoloAnnuncio, annuncio_id)

        temp &= getFirma()

        Return temp
    End Function





    Public Overridable Function getBodyMercatinoReply(ByVal trattativaId As Long, ByVal annuncioId As Long, ByVal titoloAnnuncio As String) As String
        'una volta inserita una risposta invio un'email a chi ha inserito l'annucio

        Dim temp As String = ""
        temp = "<h1>Il Mercatino di " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> hai ricevuto un nuovo messaggio per l'annuncio: {1} " & _
        "<br />" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/trattativa.aspx?trattativaId={0}&annuncioId={2}"" > qui </a> per visualizzare la risposta.<br /><br />"
        temp = String.Format(temp, trattativaId, titoloAnnuncio, annuncioId)

        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyMercatinoInserimentoAnnuncio(ByVal annuncioId As Long, ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1>Il Mercatino di " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> ti ricordiamo che il {2} hai inseriro l'annuncio per: {1} " & _
        "<br />" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/annuncio.aspx?annuncioId={0}"">qui</a> per visualizzare l'annuncio.<br /><br />"

        temp = String.Format(temp, annuncioId, titoloAnnuncio, Date.Now.ToShortDateString)

        temp &= getFirma()

        Return temp
    End Function




    Public Overridable Function getBodyMercatinoCancellaAnnuncio(ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1>Il Mercatino di " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  </p>" & _
        "<br />ti segnaliamo la cancellazione dell'annuncio per: {0} " & _
        "<br />" & _
        "<p>Di conseguenza la compravendita è stata interrotta. </p> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/myContactsList.aspx"">qui</a> per eliminare la compravendita  dalle tue trattative.<br /><br />"
        temp = String.Format(temp, titoloAnnuncio)

        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyMercatinoCancellaTrattativa(ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1>Il Mercatino di " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  </p>" & _
        "<br />ti segnaliamo la cancellazione della trattativa di compravendita dell'annuncio per: {0} " & _
        "<br />" & _
        "<p> La compravendita è stata interrotta. </p> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/myContactsList.aspx"">qui</a> per eliminare la compravendita  dalle tue trattative.<br /><br />"
        temp = String.Format(temp, titoloAnnuncio)

        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyMercatinoAggiornamentoPrezzoAnnuncio(ByVal annuncio_id As Long, ByVal titoloAnnuncio As String, vecchioPrezzo As Decimal, nuovoPrezzo As Decimal) As String
        'una volta inserito un post invio un'email a chi ha aperto il Thread

        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> Il proprietartio dell'annuncio ""{0}"" ha modificato il prezzo dell'annuncio." & _
        "<br /> " & _
        "<p> " & String.Format("Vecchio prezzo: € {0:N2}", vecchioPrezzo) & "</p>" & _
        "<p> " & String.Format("Nuovo prezzo: € {0:N2}", nuovoPrezzo) & "</p>" & _
        "<br /> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "mercatino/annuncio.aspx?annuncioId={1}"">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />"

        temp = String.Format(temp, titoloAnnuncio, annuncio_id)

        temp &= getFirma()

        Return temp
    End Function


#End Region


#Region "Immobiliare"


    Public Overridable Function getBodyImmobiliareInserimentoAnnuncio(ByVal annuncioId As Long, ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> ti ricordiamo che il {2} hai inseriro l'annuncio per: {1} " & _
        "<br />" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/annuncio.aspx?annuncioId={0}"">qui</a> per visualizzare l'annuncio.<br /><br />"

        temp = String.Format(temp, annuncioId, titoloAnnuncio, Date.Now.ToShortDateString)

        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyImmobiliareModificaTestoAnnuncio(ByVal annuncio_id As Long, ByVal titoloAnnuncio As String) As String
        'una volta inserito un post invio un'email a chi ha aperto il Thread

        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> Il proprietartio dell'annuncio ""{0}"" ha modificato il testo della descrizione." & _
        "<br /> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/annuncio.aspx?annuncioId={1}"">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />"

        temp = String.Format(temp, titoloAnnuncio, annuncio_id)

        temp &= getFirma()

        Return temp
    End Function

    Public Overridable Function getBodyImmobiliareAggiornamentoPrezzoAnnuncio(ByVal annuncio_id As Long, ByVal titoloAnnuncio As String, vecchioPrezzo As Decimal, nuovoPrezzo As Decimal) As String
        'una volta inserito un post invio un'email a chi ha aperto il Thread

        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> Il proprietartio dell'annuncio ""{0}"" ha modificato il prezzo dell'annuncio." & _
        "<br /> " & _
        "<p> " & String.Format("Vecchio prezzo: € {0:N2}", vecchioPrezzo) & "</p>" & _
        "<p> " & String.Format("Nuovo prezzo: € {0:N2}", nuovoPrezzo) & "</p>" & _
        "<br /> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/annuncio.aspx?annuncioId={1}"">qui</a> per visualizzare le modiche apportate all'annuncio .<br /><br />"

        temp = String.Format(temp, titoloAnnuncio, annuncio_id)

        temp &= getFirma()

        Return temp
    End Function

    Public Overridable Function getBodyImmobiliareReply(ByVal trattativaId As Long, ByVal annuncioId As Long, ByVal titoloAnnuncio As String) As String
        'una volta inserita una risposta invio un'email a chi ha inserito l'annucio

        Dim temp As String = ""
        temp = "<h1> " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  " & _
        "<br /> hai ricevuto un nuovo messaggio per l'annuncio: {1} " & _
        "<br />" & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/trattativa.aspx?trattativaId={0}&annuncioId={2}"" > qui </a> per visualizzare la risposta.<br /><br />"

        temp = String.Format(temp, trattativaId, titoloAnnuncio, annuncioId)

        temp &= getFirma()

        Return temp
    End Function




    Public Overridable Function getBodyImmobiliareCancellaAnnuncio(ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1> " & _applicationName & "</h1>" & _
        " <p>Gentile utente,  </p>" & _
        "<br />ti segnaliamo la cancellazione dell'annuncio per: {0} " & _
        "<br />" & _
        "<p>Di conseguenza la compravendita è stata interrotta. </p> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/myContactsList.aspx"">qui</a> per eliminare la compravendita  dalle tue trattative.<br /><br />"
        temp = String.Format(temp, titoloAnnuncio)


        temp &= getFirma()

        Return temp
    End Function


    Public Overridable Function getBodyImmobiliareCancellaTrattativa(ByVal titoloAnnuncio As String) As String
        Dim temp As String = ""
        temp = "<h1>" & _applicationName & "</h1>" & _
        " <p>Gentile utente,  </p>" & _
        "<br />ti segnaliamo la cancellazione della trattativa di compravendita dell'annuncio per: {0} " & _
        "<br />" & _
        "<p> La compravendita è stata interrotta. </p> " & _
        "<br /> Clicca <a href=""" & System.Configuration.ConfigurationManager.AppSettings("application.url") & "immobiliare/myContactsList.aspx"">qui</a> per eliminare la compravendita  dalle tue trattative.<br /><br />"
        temp = String.Format(temp, titoloAnnuncio)

        temp &= getFirma()

        Return temp
    End Function



#End Region


End Class
