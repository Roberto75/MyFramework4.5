Public Class MailManager

    'Roberto Rutigliano 26/12/2007
    'http://www.systemnetmail.com/
    'Invio email tramite autenticazone 
    '<add key="mail.server.userName" value=""/>
    '<add key="mail.server.password"  value=""/>  


    'Roberto Rutigliano 04/12/2007
    'Gestione degli Attachments
    'posso aggiungere delle Stringhe che rappresentano il path assoluto dell'allegato
    'oppure direttamente un memoryStram


    Private p_To As New List(Of Net.Mail.MailAddress)

    Public Sub _To(ByVal address As String)
        If Not String.IsNullOrEmpty(address) Then

            '25/10/2010 Roberto Rutigliano ho più indirizzi separati da ;
            Dim myArray() As String = address.Split(";")

            For Each value As String In myArray
                If Not String.IsNullOrEmpty(value) Then
                    value = value.Replace("""", "")
                    p_To.Add(New System.Net.Mail.MailAddress(value))
                End If

            Next
        End If
    End Sub

    Public Sub _To(ByVal address As String, ByVal displayName As String)
        If Not String.IsNullOrEmpty(address) Then
            p_To.Add(New System.Net.Mail.MailAddress(address, displayName))
        End If
    End Sub

    Public Sub _To(ByVal item As Net.Mail.MailAddress)
        p_To.Add(item)
    End Sub

    Public Sub _ToClearField()
        p_To.Clear()
    End Sub



    Private p_From As Net.Mail.MailAddress = Nothing

    Public Sub _From(ByVal address As String)
        If Not String.IsNullOrEmpty(address) Then
            p_From = New System.Net.Mail.MailAddress(address)
        End If
    End Sub

    Public Sub _From(ByVal address As String, ByVal displayName As String)
        If Not String.IsNullOrEmpty(address) Then
            p_From = New System.Net.Mail.MailAddress(address, displayName)
        End If
    End Sub

    Public Sub _From(ByVal item As Net.Mail.MailAddress)
        p_From = item
    End Sub


    Private p_Cc As New List(Of Net.Mail.MailAddress)

    Public Sub _Cc(ByVal address As String)
        If Not String.IsNullOrEmpty(address) Then
            p_Cc.Add(New System.Net.Mail.MailAddress(address))
        End If
    End Sub

    Public Sub _Cc(ByVal address As String, ByVal displayName As String)
        If Not String.IsNullOrEmpty(address) Then
            p_Cc.Add(New System.Net.Mail.MailAddress(address, displayName))
        End If
    End Sub

    Public Sub _Cc(ByVal item As Net.Mail.MailAddress)
        p_Cc.Add(item)
    End Sub





    'Bcc = Ccn (copia nascosta)
    Private p_Bcc As New List(Of Net.Mail.MailAddress)

    Public Sub _Bcc(ByVal address As String)
        If Not String.IsNullOrEmpty(address) Then
            p_Bcc.Add(New System.Net.Mail.MailAddress(address))
        End If
    End Sub

    Public Sub _Bcc(ByVal address As String, ByVal displayName As String)
        If Not String.IsNullOrEmpty(address) Then
            p_Bcc.Add(New System.Net.Mail.MailAddress(address, displayName))
        End If
    End Sub

    Public Sub _Bcc(ByVal item As Net.Mail.MailAddress)
        p_Bcc.Add(item)
    End Sub

    Public Sub _BccClearField()
        p_Bcc.Clear()
    End Sub



    ' Public _To As String
    ' Public _From As String
    Public _Subject As String
    Public _Body As String
    Public _MailServer As String
    'Public _Bcc As String


    Public _Attachments As New List(Of System.Net.Mail.Attachment)



    Public Shared Function isValidEmail(ByVal value As String) As Boolean
        Return MyManager.RegularExpressionManager.isValidEmail(value)
    End Function



    Public Shared Function send(ByVal ex As Exception, messaggio as string) As String
        If CBool(System.Configuration.ConfigurationManager.AppSettings("mail.isEnabled")) Then

            Dim MyMail As System.Net.Mail.MailMessage
            MyMail = New System.Net.Mail.MailMessage

            'leggo il nome del mail server dal file Web.Config
            If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("mail.From.displayName")) Then
                MyMail.From = New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mail.From"), System.Configuration.ConfigurationManager.AppSettings("mail.From.displayName"))
            Else
                MyMail.From = New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mail.From"))
            End If


            'in caso di errore invio l'email a me stesso e anche a .... se è presente nel file di configurazione 
            MyMail.To.Add(New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mail.From")))

            If (System.Configuration.ConfigurationManager.AppSettings("mail.To.Ccn") <> "") Then
                MyMail.Bcc.Add(New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mail.To.Ccn")))
            End If




            'Dim hostIPAddress As System.Net.IPAddress = System.Net.IPAddress.Parse(System.Net.IPAddress)
            'Dim hostInfo As System.Net.IPHostEntry = System.Net.Dns.GetHostByAddress(hostIPAddress)
            'Dim remoteHostName As String
            'remoteHostName = hostInfo.HostName


            If (ex.Source Is Nothing) OrElse String.IsNullOrEmpty(ex.Source) Then
                MyMail.Subject = System.Net.Dns.GetHostName() & " - Exception "
            Else
                MyMail.Subject = System.Net.Dns.GetHostName() & " - Exception: " & ex.Source
            End If

            '*** SPAMMING: G.a.p.p.y - T.e.x.t
            MyMail.Subject = MyMail.Subject.Replace(".", "")



            '*** BODY ***
            MyMail.Body = "<html><body>"


            If Not String.IsNullOrEmpty(ex.Message) Then
                MyMail.Body &= "<h2>Exception  </h2> " & ex.Message
            End If



            
            If Not ex.InnerException Is Nothing Then
                MyMail.Body &= "<br /> <br /> <h2>Inner Exception</h2> " & ex.InnerException.Message


                If Not ex.InnerException.StackTrace Is Nothing Then
                    MyMail.Body &= "<br /> <br /> <h2>Inner Exception - Stack Trace</h2> " & ex.InnerException.StackTrace.ToString
                End If


            End If



            If Not ex.StackTrace Is Nothing Then
                MyMail.Body &= "<br /> <br /><h2>Stack Trace</h2> " & ex.StackTrace.ToString
            End If


            If Not String.IsNullOrEmpty(messaggio) Then
                MyMail.Body &= "<br /> <br />  <h2>Messaggio </h2> " & messaggio
            End If

           
            MyMail.Body &= "</body></html>"


            MyMail.IsBodyHtml = True


            Dim smtp As New System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings("mail.server"))
            If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("mail.server.userName")) Then
                'invio email tramite autenticazione
                smtp.Credentials = New System.Net.NetworkCredential( _
                            System.Configuration.ConfigurationManager.AppSettings("mail.server.userName"), _
                            System.Configuration.ConfigurationManager.AppSettings("mail.server.password"))
            Else
                smtp.UseDefaultCredentials = True
            End If

            Dim esito As String = ""
            Try
                smtp.Send(MyMail)
            Catch e As Exception
                esito = e.Source & vbCrLf & e.Message & vbCrLf & vbCrLf & e.StackTrace
            End Try


            Return esito
        Else
            Return "mail.isEnabled NON è abilitato"
        End If

    End Function

    Public Shared Function send(ByVal ex As Exception) As String
        Return send(ex, "")
    End Function

    Public Function send() As String

        If CBool(System.Configuration.ConfigurationManager.AppSettings("mail.isEnabled")) Then

            Dim MyMail As System.Net.Mail.MailMessage
            MyMail = New System.Net.Mail.MailMessage

            If p_From Is Nothing Then
                'leggo il nome del mail server dal file Web.Config
                If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("mail.From.displayName")) Then
                    Me._From(System.Configuration.ConfigurationManager.AppSettings("mail.From"), System.Configuration.ConfigurationManager.AppSettings("mail.From.displayName"))
                Else
                    Me._From(System.Configuration.ConfigurationManager.AppSettings("mail.From"))
                End If
            End If

            MyMail.From = p_From


            Dim item As System.Net.Mail.MailAddress
            For Each item In p_To
                MyMail.To.Add(item)
            Next

            For Each item In p_Cc
                MyMail.CC.Add(item)
            Next

            For Each item In p_Bcc
                MyMail.Bcc.Add(item)
            Next

            '*** SPAMMING: G.a.p.p.y - T.e.x.t
            MyMail.Subject = _Subject.Replace(".", "")

            If _Body.ToLower.StartsWith("<html>") Then
                MyMail.Body = _Body
            Else
                MyMail.Body = "<html><body>" & _Body & "</body></html>"
            End If


            MyMail.IsBodyHtml = True


            '*** Attachment ***
            Dim attachment As System.Net.Mail.Attachment
            For Each attachment In Me._Attachments
                MyMail.Attachments.Add(attachment)
            Next


            If (_MailServer = "") Then
                'leggo il nome del mail server dal file Web.Config
                _MailServer = System.Configuration.ConfigurationManager.AppSettings("mail.server")
            End If


            Dim smtp As New System.Net.Mail.SmtpClient(_MailServer)
            If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("mail.server.userName")) Then
                'invio email tramite autenticazione
                smtp.Credentials = New System.Net.NetworkCredential( _
                    System.Configuration.ConfigurationManager.AppSettings("mail.server.userName"), _
                    System.Configuration.ConfigurationManager.AppSettings("mail.server.password"))
            Else
                smtp.UseDefaultCredentials = True
            End If


            Dim esito As String = ""
            Try
                smtp.Send(MyMail)
            Catch ex As Exception
                esito = ex.Source & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            End Try

            Return esito
        Else
            Return "mail.isEnabled NON è abilitato"
        End If

    End Function


End Class
