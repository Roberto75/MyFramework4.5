Public Class OrdineManager
    Private _pathDB As String

    Public _pcID As String
    Public _nota As String
    Public _userID As Long
    Public _corsoID As String
    Public _orario As String
    Public _spedizione As String
    Public _pagamento As String

    Public Sub New(ByVal pathDB As String)
        _pathDB = pathDB
    End Sub


    'resrituisce -1 in caso di errore nelle credenziali oppure ID del NUOVO utente
    Public Function insertOrdinePC() As Long
        Dim oleDbConnection As System.Data.OleDb.OleDbConnection
        oleDbConnection = New System.Data.OleDb.OleDbConnection("PROVIDER=Microsoft.Jet.OLEDB.4.0; DATA Source= " + _pathDB)
        oleDbConnection.Open()

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand


        Dim strSQL As String = "INSERT INTO ORDINEPC ( FK_USER_ID, NOTA, PC_ID, SPEDIZIONE, PAGAMENTO)" & _
            " VALUES (@FK_USER_ID, @NOTA, @PC, @SPEDIZIONE, @PAGAMENTO)"

        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, oleDbConnection)
        oleDbCommand.Parameters.Add("@FK_USER_ID", OleDb.OleDbType.VarChar).Value = Me._userID
        oleDbCommand.Parameters.Add("@NOTA", OleDb.OleDbType.VarChar).Value = Me._nota
        oleDbCommand.Parameters.Add("@PC", OleDb.OleDbType.VarChar).Value = Me._pcID
        oleDbCommand.Parameters.Add("@SPEDIZIONE", OleDb.OleDbType.VarChar).Value = Me._spedizione
        oleDbCommand.Parameters.Add("@PAGAMENTO", OleDb.OleDbType.VarChar).Value = Me._pagamento
     
        oleDbCommand.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...
        Dim cmdGetIdentity As New System.Data.OleDb.OleDbCommand("SELECT @@IDENTITY", oleDbConnection)
        Dim codiceID As Long = cmdGetIdentity.ExecuteScalar

        oleDbCommand.Dispose()
        oleDbConnection.Close()

        Return codiceID
    End Function

    Public Function insertPrenotazioneCorso() As Long
        Dim oleDbConnection As System.Data.OleDb.OleDbConnection
        oleDbConnection = New System.Data.OleDb.OleDbConnection("PROVIDER=Microsoft.Jet.OLEDB.4.0; DATA Source= " + _pathDB)
        oleDbConnection.Open()

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand


        Dim strSQL As String = "INSERT INTO PRENOTAZIONECORSO ( FK_USER_ID, NOTA, CORSO_ID, ORARIO_PREFERENZA)" & _
            " VALUES (@FK_USER_ID, @NOTA, @CORSO, @ORARIO)"

        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, oleDbConnection)
        oleDbCommand.Parameters.Add("@FK_USER_ID", OleDb.OleDbType.Integer).Value = Me._userID
        oleDbCommand.Parameters.Add("@NOTA", OleDb.OleDbType.VarChar).Value = Me._nota
        oleDbCommand.Parameters.Add("@CORSO", OleDb.OleDbType.VarChar).Value = Me._corsoID
        oleDbCommand.Parameters.Add("@ORARIO", OleDb.OleDbType.VarChar).Value = Me._orario


        oleDbCommand.ExecuteNonQuery()

        'Ottengo in codice id del nuovo elemento appena inserito...
        Dim cmdGetIdentity As New System.Data.OleDb.OleDbCommand("SELECT @@IDENTITY", oleDbConnection)
        Dim codiceID As Long = cmdGetIdentity.ExecuteScalar

        oleDbCommand.Dispose()
        oleDbConnection.Close()

        Return codiceID
    End Function
End Class

