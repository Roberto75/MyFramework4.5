
'06/03/2010
'Appllicazione di test on line: http://gskinner.com/RegExr/


'http://www.regular-expressions.info
'http://regexlib.com/Search.aspx?k=URL




Public Class RegularExpressionManager

    Public Shared Function isValidEmail(ByVal value As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(value, "\S+@\S+\.\S{2,3}\b")


        'Dim myMatches As System.Text.RegularExpressions.MatchCollection
        'myMatches = System.Text.RegularExpressions.Regex.Matches(value, "\S+@\S+\.\S{2,3}")

        'Return myMatches.Count > 0

    End Function

    Public Shared Function isValidCodiceFiscale(ByVal value As String) As String
        'Return System.Text.RegularExpressions.Regex.IsMatch(value, "[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$")

        value = value.ToUpper().Trim()

        Dim esito As String = ""
        If Not System.Text.RegularExpressions.Regex.IsMatch(value, "[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z][0-9][0-9][A-Z][0-9][0-9][A-Z][0-9][0-9][0-9][A-Z]") Then
            esito = "Il codice fiscale digitato non è sintatticamente corretto"
            Return esito
        End If

        Dim mese As String = value.ToUpper().Substring(8, 1)
        If mese <> "A" And _
          mese <> "B" And _
           mese <> "C" And _
           mese <> "D" And _
           mese <> "E" And _
           mese <> "F" And _
           mese <> "G" And _
           mese <> "H" And _
           mese <> "L" And _
           mese <> "M" And _
           mese <> "P" And _
           mese <> "R" And _
           mese <> "S" And _
           mese <> "T" Then
            esito = "Inserire un codice fiscale corretto,  il carattere in posizione 9 deve essere compreso da A-T"
            Return esito
        End If

        Dim giorno As Int16 = value.Substring(9, 2)
        If (giorno > 31 And giorno < 41) Or (giorno <= 0) Or (giorno > 71) Then
            esito = "Inserire un codice fiscale corretto il carattere in posizione 10 e 11 deve essere compreso tra 1 e 71"
            Return esito
        End If

        Return ""
    End Function

    Public Shared Function isValidLogin(ByVal value As String) As Boolean

        If value.IndexOf(" ") <> -1 Then
            Return False
        End If


        Dim psw As New MyManager.PasswordManager
        Dim temp As String = psw.PASSWORD_CHARS_SPECIAL_DENY

        For Each c As Char In temp
            If value.IndexOf(c) <> -1 Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function isValidHttp(ByVal value As String) As Boolean
        If Not value.StartsWith("http://") Then
            value = "http://" & value
        End If
        Return System.Text.RegularExpressions.Regex.IsMatch(value, "http://\S+\.\S+")
    End Function


    Public Shared Function isValidItalianTelephone(ByVal value As String) As Boolean
        If value.StartsWith("+39") Then
            value = value.Replace("+39", "")
        End If

        value = value.Replace(" ", "")

        'Per esempio [0-9]{3, 4}\-[0-9]{7} individua tutti i numeri telefonici con un prefisso composto da 3 o 4 cifre e un suffisso di esattamente 7 cifre

        'Return System.Text.RegularExpressions.Regex.IsMatch(value, "[0-9]{3, 4}\-[0-9]{7}")
        'System.Text.RegularExpressions.Regex.IsMatch(value, "[0-9]{9, 10}")
        Return True
    End Function


    Public Shared Function isValidItalianMobile(ByVal value As String) As Boolean
        If Not value.StartsWith("+39") Then
            value = "+39" & value
        End If

        value = value.Replace(" ", "")

        Return System.Text.RegularExpressions.Regex.IsMatch(value, "^((\+\s?\d{2}|\(?00\s?\d{2}\)?)\s?\d{2}\s?\d{3}\s?\d{4})")
    End Function


    Public Shared Function isValidCap(ByVal value As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(value, "\d{5}")
    End Function

    Public Shared Function isValidIpAddress(ByVal value As String) As Boolean
        'http://channel9.msdn.com/wiki/default.aspx/SecurityWiki.RegExInputValCode2

        'IP address  
        'Matches 0.0.0.0 through 255.255.255.255  
        'Use this regex to match IP numbers with accurracy, without access to the individual IP numbers.  

        Dim pattern As New System.Text.RegularExpressions.Regex("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
        Return pattern.IsMatch(value)
    End Function


    Shared Function isStrongPassword(ByVal value As String) As Boolean



        'http://msdn.microsoft.com/library/ITA/jscript7/html/jsreconIntroductionToRegularExpressions.asp

        'At least 8 charcaters long
        'Includes at least one numeric digit
        'Includes at least one lowercase alpha character
        'Includes at least one uppercase alpha character
        'Includes at least one special character of PASSWORD_CHARS_SPECIAL

        Dim passwordStrengthRegularExpression As String


        '^           # anchor at the start
        '               (?=.*\d)     # must contain at least one numeric character
        '              (?=.*[a-z])  # must contain one lowercase character
        '             (?=.*[A-Z])  # must contain one uppercase character
        '            .{8,10}      # From 8 to 10 characters in length
        '           \s           # allows a space 
        '          $            # anchor at the end", 
        '           (?!.*\s)   #non sono ammessi spazi
        '        (?!.*\W)       #escludo tutti i caratteri non alfanumerico. Equivale a "[^A-Za-z0-9_]". 



        'passwordStrengthRegularExpression = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[ ! "" @\$%&/\(\)=\?'`\*\+~#\-_\.,;:\{\[\]\}\\< >\|]).{8,}$"
        'passwordStrengthRegularExpression = "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[" & PASSWORD_CHARS_SPECIAL & "]).{8,}$"


        'ci sono dei caratteri spciali usati nelle espressioni regolari che devono essere sostitui

        Dim manager As New PasswordManager()

        passwordStrengthRegularExpression = "(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[" & replaceCharactersInRegularExplression(manager.PASSWORD_CHARS_SPECIAL) & "])" & _
                 "(?!.*\s)(?!.*[" & replaceCharactersInRegularExplression(manager.PASSWORD_CHARS_SPECIAL_DENY) & "]).{8,}$"


        passwordStrengthRegularExpression = "(.{8,})" 'almeno 8 caratteri
        passwordStrengthRegularExpression = "(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)(?=.*[" & replaceCharactersInRegularExplression(manager.PASSWORD_CHARS_SPECIAL) & "])(?!.*[" & replaceCharactersInRegularExplression(manager.PASSWORD_CHARS_SPECIAL_DENY) & "])(.{8,})"



        Return System.Text.RegularExpressions.Regex.IsMatch(value, passwordStrengthRegularExpression)

    End Function

    Public Shared Function replaceCharactersInRegularExplression(ByVal value As String) As String
        'i caratteri spciali per le espressioni regolari sono: http://msdn.microsoft.com/library/ita/default.asp?url=/library/ITA/jscript7/html/jsjsgrpregexpsyntax.asp
        'prima di tutto sostiuisco l'evenuale \
        value = value.Replace("\", "\\")
        value = value.Replace("$", "\$").Replace("(", "\(").Replace(")", "\)").Replace("*", "\*").Replace("+", "\+").Replace("-", "\-").Replace("[", "\[").Replace("]", "\]").Replace("?", "\?").Replace("/", "\/").Replace("^", "\^").Replace("{", "\{").Replace("}", "\}").Replace("|", "\|")
        Return value

    End Function


End Class
