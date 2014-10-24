Public Class ManagerException
    Inherits System.Exception


    Public Enum ErrorNumber
        NotDefined = 0
        PaginaNonTrovata
        UtenteNonAutorizzato
        UtenteNonAutenticato
        UtenteDisabilitato
        LoginDuplicata
        LoginInesistente
        LoginPasswordErrati
        FunzioneNonImplementata

        RecordDuplicato
        CA_DownloadAbort
        EmailDuplicata
        UtenteLoggato

        AuthenticationMultilevelFailed
        PasswordExpired
    End Enum


    'Public _messageColor As System.ConsoleColor
    Public ErrorCode As ManagerException.ErrorNumber

    Public Sub New()
        MyBase.New()
        Me.ErrorCode = ManagerException.ErrorNumber.NotDefined
    End Sub

    REM Crea un eccezione caratterizzata dall'avere come massaggio una stringa
    Public Sub New(ByVal message As String)
        MyBase.New(message)
        Me.ErrorCode = ManagerException.ErrorNumber.NotDefined
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
        Me.ErrorCode = ManagerException.ErrorNumber.NotDefined
    End Sub

    Public Sub New(ByVal errorNumber As ErrorNumber)
        MyBase.New("")
        Me.ErrorCode = errorNumber
    End Sub



    Public Sub New(ByVal errorNumber As ErrorNumber, ByVal message As String)
        MyBase.New(message)
        Me.ErrorCode = errorNumber
    End Sub



    Public Overrides ReadOnly Property Message() As String
        Get
            Dim messaggio As String = ""
            Select Case ErrorCode
                Case ErrorNumber.NotDefined
                    '08/12/2009
                    'mi duplica il messaggio
                    'messaggio = MyBase.Message
                    messaggio = ""
                Case ErrorNumber.PaginaNonTrovata
                    messaggio = "La pagina richiesta è inesistente."
                Case ErrorNumber.UtenteNonAutenticato
                    messaggio = "La risorsa richiesta richiede l'autenticazione. Eseguire prima la procedura di login."
                Case ErrorNumber.UtenteNonAutorizzato
                    messaggio = "Non si dispone delle autorizzazioni necessarie per poter accedere alla risorsa richiesta. Contattare l'amministratore di sistema."
                Case ErrorNumber.UtenteDisabilitato
                    messaggio = "Utente disabilitato. Contattare l'amministratore di sistema."
                Case ErrorNumber.LoginPasswordErrati
                    messaggio = "Login utente e/o password errati. Verificare e ripetere l'operazione."
                Case ErrorNumber.LoginInesistente
                    messaggio = "Attenzione non esite nessun utente con questa login."
                Case ErrorNumber.FunzioneNonImplementata
                    messaggio = "La funzionalità richiesta non è stata ancora implementata."
                Case ErrorNumber.LoginDuplicata
                    messaggio = "Attenzione esitono più utenti con la stessa login."
                Case ErrorNumber.RecordDuplicato
                    messaggio = "Attenzione esiste gia un record con la stessa chiave. (Errore di integrità referenziale)"
                Case ErrorNumber.CA_DownloadAbort
                    messaggio = "Non è più possibile scaricare il certificato. Contattare l'amministratore del sistema."
                Case ErrorNumber.EmailDuplicata
                    messaggio = "Attenzione esitono più utenti con la stessa email."
                Case ErrorNumber.UtenteLoggato
                    messaggio = "Attenzione esiste già un utente loggato con queste credenziali."
                Case ErrorNumber.AuthenticationMultilevelFailed
                    messaggio = "Autenticazione MultiLivello Fallita."
                Case ErrorNumber.PasswordExpired
                    messaggio = "Password Expired."
            End Select

            If Not String.IsNullOrEmpty(MyBase.Message) Then
                If String.IsNullOrEmpty(messaggio) Then
                    messaggio = MyBase.Message
                Else
                    messaggio &= vbCrLf & MyBase.Message
                End If

            End If

            If Not MyBase.InnerException Is Nothing Then
                messaggio &= vbCrLf & MyBase.InnerException.Message
            End If

            Return messaggio
        End Get
    End Property




    REM Crea un eccezione caratterizzata dall'avere come massaggio una stringa e un colore  
    REM con cui colorare il testo se sarà contenuto da un web control
    'Public Sub New(ByVal message As String, ByVal MessageColor As System.ConsoleColor)
    '    MyBase.New(message)
    '    Me.MessageColor = MessageColor
    'End Sub

    REM Crea un eccezione con una stringa che ne definisce il messaggio, il colore,
    REM e con un eccezione al suo interno
    'Public Sub New(ByVal message As String, ByVal innerException As Exception, ByVal MessageColor As System.ConsoleColor)
    '    MyBase.New(message, innerException)
    '    Me.MessageColor = MessageColor
    'End Sub


End Class
