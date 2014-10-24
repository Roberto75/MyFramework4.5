Public Class SessionData
    Private _userID As Long = -1
    Private _login As String
    Private _profiloID As String
    Private _profiloDesc As String
    Private _nome As String
    Private _cognome As String
    Private _email As String
    Private _dateLastLogin As Date
    Private _customerID As Long
    Private _customer As String
    Private _authorizations As String = ""
    Private _cellulare As String
    Private _cultureInfo As System.Globalization.CultureInfo
    Private _javaScriptMessage As String
    Private _ip As String = ""
    Private _sessionId As String = ""




    Public Function getCustomer() As String
        Return Me._customer
    End Function

    Public Sub setCustomer(ByVal value As String)
        Me._customer = value
    End Sub



    Public Function getJavaScriptMessage() As String
        Return Me._javaScriptMessage
    End Function

    Public Sub setJavaScriptMessage(ByVal value As String)
        Me._javaScriptMessage = value
    End Sub

    Public Sub setCultureInfo(ByVal value As System.Globalization.CultureInfo)
        Me._cultureInfo = value
    End Sub

    Public Sub setCustomerID(ByVal customerid As String)
        Me._customerID = customerid
    End Sub

    Public Sub setCultureInfo(ByVal value As String)
        Me._cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(value)
    End Sub


    Public Function getCultureInfo() As System.Globalization.CultureInfo
        Return _cultureInfo
    End Function

    Public Sub setCellulare(ByVal value As String)
        _cellulare = value
    End Sub
    Public Function getCellulare() As String
        Return _cellulare
    End Function

    Public Sub setAuthorizations(ByVal value As String)
        _authorizations = value
    End Sub
    Public Function getAuthorizations() As String
        Return _authorizations
    End Function

    Public Function checkAuthorization(ByVal value As String) As Boolean

        'riformatto la stringa nel formato  AUTH1;AUTH2;AUTH3
        If value.StartsWith(";") Then
            value = value.Substring(1)
        End If

        If value.EndsWith(";") Then
            value = value.Substring(0, value.Length - 1)
        End If

        Dim valori As String()
        valori = value.Split(";")

        Dim i As Int16
        For i = 0 To valori.Length - 1
            If Me._authorizations.ToUpper.IndexOf(";" & valori(i).ToUpper & ";") <> -1 Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub setEmail(ByVal value As String)
        _email = value
    End Sub
    Public Function getEmail() As String
        Return _email
    End Function

    Public Sub setLastLogin(ByVal value As Date)
        _dateLastLogin = value
    End Sub
    Public Function getLastLogin() As Date
        Return _dateLastLogin
    End Function

    Public Sub setIp(ByVal ip As String)
        _ip = ip
    End Sub
    Public Function getIp() As String
        Return _ip
    End Function

    Public Sub setSessionId(ByVal value As String)
        _sessionId = value
    End Sub
    Public Function getSessionId() As String
        Return _sessionId
    End Function


    Public Sub setUserId(ByVal userid As String)
        _userID = userid
    End Sub
    Public Function getUserId() As String
        Return _userID
    End Function
    Public Function getCustomerId() As String
        Return _customerID
    End Function
    Public Sub setLogin(ByVal login As String)
        _login = login
    End Sub
    Public Function getLogin() As String
        Return _login
    End Function
    Public Sub setProfiloID(ByVal profiloID As String)
        _profiloID = profiloID
    End Sub
    Public Function getProfiloID() As String
        Return _profiloID
    End Function
    'Public Sub setProfiloDesc(ByVal profiloDesc As String)
    '    _profiloDesc = profiloDesc
    'End Sub
    'Public Function getProfiloDesc() As String
    '    Return _profiloDesc
    'End Function
    Public Sub setNomeCognome(ByVal nome As String, ByVal cognome As String)
        _nome = nome
        _cognome = cognome
    End Sub
    Public Function getNomeCognome() As String
        Return _nome & " " & _cognome
    End Function
    Public Function getNome() As String
        Return _nome
    End Function
    Public Function getCognome() As String
        Return _cognome
    End Function
   
    Public Function isUserAuthenticated() As Boolean
        Return (Me._userID <> -1)
    End Function


    ' il metodo resatta tutti i valori della sessione tranne il messaggio javascript
    Public Function reset() As Boolean
        _userID = -1
        _nome = ""
        _cognome = ""
        _email = ""
        _login = ""
        _sessionId = ""
    End Function

End Class

