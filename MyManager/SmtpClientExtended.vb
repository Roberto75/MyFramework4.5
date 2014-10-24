'Dit lost het probleem op met FQDN en het HELO domain required hier word in de localhostname
'variabel een FQDN gestoken en zo kan de mail perfect worden verzonden
'It resolves the problem with te FQDN and the HELO domain required. 
'In the localhostname variable comes the FQDN and this way your mail can be send perfectly

Imports System.Reflection

Public Class SmtpClientExtended
    Inherits System.Net.Mail.SmtpClient

    Private Shared ReadOnly localHostName As FieldInfo = GetLocalHostNameField()

    Public Sub New(ByRef host As String, ByVal port As Integer)
        MyBase.New(host, port)
        Initialize()
    End Sub

    Public Sub New(ByRef host As String)
        MyBase.New(host)
        Initialize()
    End Sub

    Public Sub New()
        MyBase.New()
        Initialize()
    End Sub

    Public Sub Initialize()
        Dim h As String = getFQDN()
        If h <> "" Then
            HELODomain = h
        End If
    End Sub

    Public Property HELODomain() As String
        Get
            If localHostName Is Nothing Then
                Return Nothing
            Else
                Return localHostName.GetValue(Me)
            End If
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Throw New ArgumentNullException("value")
            Else
                localHostName.SetValue(Me, value)
            End If
        End Set
    End Property

    Private Shared Function GetLocalHostNameField() As FieldInfo
        Dim flags As BindingFlags = (BindingFlags.Instance Or BindingFlags.NonPublic)
        Return GetType(System.Net.Mail.SmtpClient).GetField("localHostName", flags)
    End Function

    Public Function getFQDN() As String
        Return System.Net.Dns.GetHostEntry("").HostName
    End Function

End Class
