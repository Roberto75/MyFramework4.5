Public Class AziendaManager
    Inherits Manager

    Public _ragioneSociale As String
    Public _indirizzo As String
    Public _numeroCivico As String
    Public _citta As String
    Public _provincia As String
    Public _cap As String
    Public _codiceFiscale As String
    Public _http As String
    Public _partitaiva As String

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub


    Public Function insertInvitoDelegatoAziendale(ByVal ragioneSociale As String, ByVal codiceFiscale As String) As Long

        Dim strSQL As String
        strSQL = "INSERT INTO CUSTOMER (DATE_ADDED, RAGIONE_SOCIALE, CODICE_FISCALE) VALUES ( getDate(),  @RAGIONE_SOCIALE ,  @codice_Fiscale  )"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        If ragioneSociale <> "" Then
            Me._addParameter(command, "@RAGIONE_SOCIALE", ragioneSociale)
        End If

        If codiceFiscale <> "" Then
            Me._addParameter(command, "@codice_Fiscale", codiceFiscale)
        End If

        'Dim command As System.Data.Common.DbCommand
        'command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.ExecuteNonQuery()

        Return _getIdentity()
    End Function


    Public Sub insertAzienda(ByVal aziendaId As String)

        Dim strSQL As String = "UPDATE CUSTOMER SET DATE_MODIFIED = getDate()"
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        If Me._ragioneSociale <> "" Then
            strSQL &= " ,RAGIONE_SOCIALE = @RAGIONE_SOCIALE"
            Me._addParameter(command, "@RAGIONE_SOCIALE", Me._ragioneSociale)
        End If

        If Me._codiceFiscale <> "" Then
            strSQL &= " ,CODICE_FISCALE = @CODICE_FISCALE"
            Me._addParameter(command, "@CODICE_FISCALE", Me._codiceFiscale)
        End If

        If Me._partitaiva <> "" Then
            strSQL &= " ,PARTITA_IVA = @PARTITA_IVA"
            Me._addParameter(command, "@PARTITA_IVA", Me._partitaiva)
        End If

        If Me._indirizzo <> "" Then
            strSQL &= " ,INDIRIZZO = @INDIRIZZO"
            Me._addParameter(command, "@INDIRIZZO", Me._indirizzo)
        End If

        If Me._numeroCivico <> "" Then
            strSQL &= " ,NUMERO_CIVICO = @NUMERO_CIVICO"
            Me._addParameter(command, "@NUMERO_CIVICO", Me._numeroCivico)
        End If

        If Me._citta <> "" Then
            strSQL &= " ,CITTA = @CITTA"
            Me._addParameter(command, "@CITTA", Me._citta)
        End If

        If Me._provincia <> "" Then
            strSQL &= " ,PROVINCIA = @PROVINCIA"
            Me._addParameter(command, "@PROVINCIA", Me._provincia)
        End If

        If Me._cap <> "" Then
            strSQL &= " ,CAP = @CAP"
            Me._addParameter(command, "@CAP", Me._cap)
        End If

        If Me._http <> "" Then
            strSQL &= ", HTTP = @HTTP"
            Me._addParameter(command, "@HTTP", Me._http)
        End If

        strSQL &= " WHERE CUSTOMER_ID=" & aziendaId
        command.CommandText = strSQL
        command.ExecuteNonQuery()

    End Sub

    Public Function getAzienda(ByVal AziendaId As Long) As DataTable

        Dim sqlQuery As String = "SELECT * FROM CUSTOMER WHERE CUSTOMER_ID = " & AziendaId
        Return _fillDataSet(sqlQuery).Tables(0)

    End Function

    Public Function getNomeAzienda(ByVal AziendaId As Long) As DataTable

        Dim sqlQuery As String = "SELECT CUSTOMER_ID, RAGIONE_SOCIALE, CODICE_FISCALE FROM CUSTOMER WHERE CUSTOMER_ID = " & AziendaId & " ORDER BY RAGIONE_SOCIALE"
        Return _fillDataSet(sqlQuery).Tables(0)

    End Function

    Public Function getNumAccount(ByVal AziendaId As Long) As Integer

        Dim sqlQuery As String = "SELECT count(*) FROM UTENTI where DATE_DELETED is null and CUSTOMER_ID = " & AziendaId
        Return CInt(Me._executeScalar(sqlQuery))

    End Function

    Public Function getNumAccountActive(ByVal AziendaId As Long) As Integer

        Dim sqlQuery As String = "SELECT count(*) FROM UTENTI where DATE_DELETED is null and is_enabled = 1 and CUSTOMER_ID = " & AziendaId
        Return CInt(Me._executeScalar(sqlQuery))

    End Function


    Public Function getAllAziende() As DataTable

        Dim sqlQuery As String = "SELECT CUSTOMER_ID, RAGIONE_SOCIALE FROM CUSTOMER ORDER BY RAGIONE_SOCIALE"
        Return _fillDataSet(sqlQuery).Tables(0)

    End Function


    Public Function getUtentiFromProfilo(ByVal profilo As String, Optional ByVal customerId As Long = -1) As DataTable

        Dim sqlQuery As String = "SELECT * FROM UTENTI where DATE_DELETED is null and PROFILO_ID = '" & profilo & "'"
        If (customerId <> -1) Then
            sqlQuery = sqlQuery & " and CUSTOMER_Id = " & customerId
        End If
        Return _fillDataSet(sqlQuery).Tables(0)

    End Function


    Public Function insert() As Long

        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO CUSTOMER (DATE_ADDED  "
        strSQLParametri = " VALUES ( Now()  "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        If Not String.IsNullOrEmpty(_ragioneSociale) Then
            strSQL &= ",RAGIONE_SOCIALE "
            strSQLParametri &= ", @RAGIONE_SOCIALE "
            _addParameter(command, "@RAGIONE_SOCIALE", _ragioneSociale)
        End If

        command.CommandText = strSQL & " ) " & strSQLParametri & " )"
        command.ExecuteNonQuery()

        Return _getIdentity()
    End Function





End Class
