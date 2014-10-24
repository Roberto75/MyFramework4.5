Public Class CurriculumManager
    Inherits ReflectionManager

    Public __userId As String
    Public __nome As String
    Public __cognome As String
    Public __email As String
    Public __sesso As String

    Public __luogoDiNascita As String
    Public __provinciaDiNascita As String
    Public __dataDiNascita As Date



    Public _societa As String


   


    Public Sub New()
        MyBase.New("curriculum")
    End Sub

    Public Function insert() As Long
        Dim command As OleDb.OleDbCommand
        command = Me.buildInsertCommand()
        Console.WriteLine(command.CommandText)

        command.Connection = Me._connection
        command.ExecuteNonQuery()

        Return _getIdentity()
    End Function




    Public Function insertOLD() As Long



        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO CURRICULUM ( date_added  "
        strSQLParametri = " VALUES ( Now()  "


        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)


        'If _userId <> "" Then
        '    strSQL &= ",USER_ID "
        '    strSQLParametri &= ",@USER_ID "
        '    oleDbCommand.Parameters.Add("@USER_ID", OleDb.OleDbType.Numeric).Value = _userId
        'End If

        'If _nome <> "" Then
        '    strSQL &= ",NOME "
        '    strSQLParametri &= ",@NOME "
        '    oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = _nome
        'End If

        'If _cognome <> "" Then
        '    strSQL &= ",COGNOME "
        '    strSQLParametri &= ",@COGNOME "
        '    oleDbCommand.Parameters.Add("@COGNOME", OleDb.OleDbType.VarChar).Value = _cognome
        'End If

        'If _societa <> "" Then
        '    strSQL &= ",SOCIETA "
        '    strSQLParametri &= ",@SOCIETA "
        '    oleDbCommand.Parameters.Add("@SOCIETA", OleDb.OleDbType.VarChar).Value = _societa
        'End If


        'If _email <> "" Then
        '    strSQL &= ",EMAIL "
        '    strSQLParametri &= ",@EMAIL "
        '    oleDbCommand.Parameters.Add("@EMAIL", OleDb.OleDbType.VarChar).Value = _email
        'End If


        'If _nota <> "" Then
        '    strSQL &= ",NOTA "
        '    strSQLParametri &= ",@NOTA "
        '    oleDbCommand.Parameters.Add("@NOTA", OleDb.OleDbType.VarChar).Value = _nota
        'End If

        'If _ufficioIndirizzo <> "" Then
        '    strSQL &= ",UFFICIO_INDIRIZZO "
        '    strSQLParametri &= ",@UFFICIO_INDIRIZZO "
        '    oleDbCommand.Parameters.Add("@UFFICIO_INDIRIZZO", OleDb.OleDbType.VarChar).Value = _ufficioIndirizzo
        'End If

        'If _ufficioEmail <> "" Then
        '    strSQL &= ",UFFICIO_EMAIL "
        '    strSQLParametri &= ",@UFFICIO_EMAIL "
        '    oleDbCommand.Parameters.Add("@UFFICIO_EMAIL", OleDb.OleDbType.VarChar).Value = _ufficioEmail
        'End If

        'If _ufficioCivico <> "" Then
        '    strSQL &= ",UFFICIO_CIVICO "
        '    strSQLParametri &= ",@UFFICIO_CIVICO "
        '    oleDbCommand.Parameters.Add("@UFFICIO_CIVICO", OleDb.OleDbType.VarChar).Value = _ufficioCivico
        'End If

        'If _ufficioCitta <> "" Then
        '    strSQL &= ",UFFICIO_CITTA "
        '    strSQLParametri &= ",@UFFICIO_CITTA "
        '    oleDbCommand.Parameters.Add("@UFFICIO_CITTA", OleDb.OleDbType.VarChar).Value = _ufficioCitta
        'End If

        'If _ufficioProvincia <> "" Then
        '    strSQL &= ",UFFICIO_PROVINCIA "
        '    strSQLParametri &= ",@UFFICIO_PROVINCIA "
        '    oleDbCommand.Parameters.Add("@UFFICIO_PROVINCIA", OleDb.OleDbType.VarChar).Value = _ufficioProvincia
        'End If

        'If _ufficioZona <> "" Then
        '    strSQL &= ",UFFICIO_ZONA "
        '    strSQLParametri &= ",@UFFICIO_ZONA "
        '    oleDbCommand.Parameters.Add("@UFFICIO_ZONA", OleDb.OleDbType.VarChar).Value = _ufficioZona
        'End If


        'If _ufficioCap <> "" Then
        '    strSQL &= ",UFFICIO_CAP "
        '    strSQLParametri &= ",@UFFICIO_CAP "
        '    oleDbCommand.Parameters.Add("@UFFICIO_CAP", OleDb.OleDbType.VarChar).Value = _ufficioCap
        'End If

        'If _ufficioTelefono <> "" Then
        '    strSQL &= ",UFFICIO_TELEFONO "
        '    strSQLParametri &= ",@UFFICIO_TELEFONO "
        '    oleDbCommand.Parameters.Add("@UFFICIO_TELEFONO", OleDb.OleDbType.VarChar).Value = _ufficioTelefono
        'End If

        'If _ufficioFax <> "" Then
        '    strSQL &= ",UFFICIO_FAX "
        '    strSQLParametri &= ",@UFFICIO_FAX "
        '    oleDbCommand.Parameters.Add("@UFFICIO_FAX", OleDb.OleDbType.VarChar).Value = _ufficioFax
        'End If

        'If _ufficioCellulare <> "" Then
        '    strSQL &= ",UFFICIO_CELLULARE "
        '    strSQLParametri &= ",@UFFICIO_CELLULARE "
        '    oleDbCommand.Parameters.Add("@UFFICIO_CELLULARE", OleDb.OleDbType.VarChar).Value = _ufficioCellulare
        'End If


        'If _ufficioHTTP <> "" Then
        '    strSQL &= ",UFFICIO_HTTP "
        '    strSQLParametri &= ",@UFFICIO_HTTP "
        '    oleDbCommand.Parameters.Add("@UFFICIO_HTTP", OleDb.OleDbType.VarChar).Value = _ufficioHTTP
        'End If


        oleDbCommand.CommandText = strSQL & " ) " & strSQLParametri & " )"

        oleDbCommand.ExecuteNonQuery()


        'Ottengo in codice id del nuovo elemento appena inserito...
        Return _getIdentity()
    End Function



End Class
