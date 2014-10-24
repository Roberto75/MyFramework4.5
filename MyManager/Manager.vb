Public Class Manager
    'Direttive di compilazione per le librerire esterne
#Const MySQL = False
#Const SqlServerCe = False
#Const PostgreSQL = False
#Const Oracle = False


    'Roberto Rutigliano 17/12/2007
    'Aggiunta la gestione per Oracle System.Data.OracleClient


    'define FALSE 0
    'define TRUE 1

    'Da Oracle a SQL Server 
    'http://vyaskn.tripod.com/oracle_sql_server_differences_equivalents.htm

    Protected _connection As System.Data.Common.DbConnection
    Protected _factory As System.Data.Common.DbProviderFactory
    Protected _connectionName As String
    Protected _provider As String
    Protected _strSql As String
    Protected _transaction As Data.Common.DbTransaction


#Region "Windows Forms ProgressBar and Status Bar"
    Protected _progressBar As Windows.Forms.ToolStripProgressBar
    Protected _statusBar As Windows.Forms.ToolStripLabel

    Public Sub _setProgressBar(ByRef value As Windows.Forms.ToolStripProgressBar)
        _progressBar = value
    End Sub

    Public Sub _setStatusBar(ByRef value As Windows.Forms.ToolStripLabel)
        _statusBar = value
    End Sub

    Public Sub _statusBarUpdate(ByVal value As String)
        _statusBar.Text = value
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _progressBarPerformStep()
        _progressBar.PerformStep()
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _progressBarSetValue(ByVal value As Integer)
        _progressBar.Value = value
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _progressBarSetMaximumValue(ByVal value As Integer)
        _progressBar.Maximum = value
    End Sub
#End Region


#Region "__TRANSACTION___"

    Public Sub _transactionBegin(ByRef transaction As Data.Common.DbTransaction)
        If transaction Is Nothing Then
            Throw New ManagerException("Transazione NON inizializzata")
            Exit Sub
        End If
        _transaction = transaction
    End Sub

    Public Sub _transactionBegin()
        If Not _transaction Is Nothing Then
            Throw New ManagerException("Transazione già aperta")
            Exit Sub
        End If
        _transaction = Me._connection.BeginTransaction()
    End Sub

    Public Sub _transactionCommit()
        If _transaction Is Nothing Then
            Throw New ManagerException("Transazione NON inizializzata")
            Exit Sub
        End If
        _transaction.Commit()
        _transaction.Dispose()
        _transaction = Nothing
    End Sub

    Public Sub _transactionRollback()
        If _transaction Is Nothing Then
            Throw New ManagerException("Transazione NON inizializzata")
            Exit Sub
        End If
        _transaction.Rollback()
        _transaction.Dispose()
        _transaction = Nothing
    End Sub

    Public Function _getTransaction() As Data.Common.DbTransaction
        '26/01/2012 commento
        ' If _transaction Is Nothing Then
        ' Throw New ManagerException("Transazione NON inizializzata")
        'xit Function
        '  End If
        Return _transaction
    End Function
#End Region

    'Protected _enableLogging As Boolean = False

    Sub New(ByVal connection As System.Data.Common.DbConnection)
        'In questo costruttore viene passata la connessione da utilizzare 
        _connection = connection

        If _connection Is Nothing Then
            Exit Sub
        End If


        Select Case _connection.GetType().Name
            Case "OdbcConnection"
                _provider = "System.Data.Odbc"
            Case "OleDbConnection"
                _provider = "System.Data.OleDb"
            Case "SqlConnection"
                _provider = "System.Data.SqlClient"
            Case "MySqlConnection"
                _provider = "MySql.Data.MySqlClient"
            Case "SqlCeConnection"
                _provider = "System.Data.SqlServerCe.3.5"
            Case "NpgsqlConnection"
                _provider = "Npgsql"
            Case Else
                Throw New ManagerException("Il costruttore non gestisce  questo tipo di connessione: " & _connection.GetType().Name)
        End Select

        _factory = System.Data.Common.DbProviderFactories.GetFactory(_provider)

    End Sub

    Sub New(ByVal connectionName As String)
        'In questo costruttore viene passata la connessione da utilizzare 
        _provider = System.Configuration.ConfigurationManager.ConnectionStrings(connectionName).ProviderName
        ' Creazione dell'oggetto factory

        _factory = System.Data.Common.DbProviderFactories.GetFactory(_provider)

        _connection = _factory.CreateConnection()
        _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(connectionName).ConnectionString
    End Sub

    Sub New()
        'Throw New ManagerException("Il costruttore vuoto non è gestito.")
    End Sub

    'Friend Sub New(ByVal connectionString As String)
    '    '_pathDB = pathDB
    '    '_connectionString _connectionString "PROVIDER=Microsoft.Jet.OLEDB.4.0; DATA Source= " & _pathDB
    '    _connectionString = connectionString
    '    _connection = New System.Data.OleDb.OleDbConnection(_connectionString)
    'End Sub

    'Friend Sub New(ByVal connection As System.Data.OleDb.OleDbConnection)
    '    _connection = connection
    'End Sub

    Public Function getFactory() As System.Data.Common.DbProviderFactory
        Return Me._factory
    End Function

    Public Function getConnection() As System.Data.Common.DbConnection
        Return Me._connection
    End Function

    Public Sub openConnection()
        If _connection.State <> ConnectionState.Open Then
            _connection.Open()
        End If
    End Sub

    Public Sub closeConnection()
        _connection.Close()
        _connection.Dispose()
    End Sub


#If MySQL Then
    Protected Function getMySqlDbType(ByVal value As Object) As MySql.Data.MySqlClient.MySqlDbType
        Select Case value.GetType.Name
            Case "Boolean"
                Return MySql.Data.MySqlClient.MySqlDbType.Bit
            Case "Int64" 'Long
                Return MySql.Data.MySqlClient.MySqlDbType.Int64
            Case "Int32" 'Int
                Return MySql.Data.MySqlClient.MySqlDbType.Int32
            Case "Int16" 'SmallInt
                Return MySql.Data.MySqlClient.MySqlDbType.Int16
            Case "String"
                Return MySql.Data.MySqlClient.MySqlDbType.String
            Case "Byte[]"
                Return MySql.Data.MySqlClient.MySqlDbType.Binary
            Case "DateTime"
                Return MySql.Data.MySqlClient.MySqlDbType.Timestamp
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function
#End If

#If PostgreSQL Then
    Protected Function getNpgsqlDbType(ByVal value As Object) As NpgsqlTypes.NpgsqlDbType
        Select Case value.GetType.Name
            Case "Boolean"
                Return NpgsqlTypes.NpgsqlDbType.Boolean
            Case "Int64" 'Long
                Return NpgsqlTypes.NpgsqlDbType.Bigint
            Case "Int32" 'Int
                Return NpgsqlTypes.NpgsqlDbType.Integer
            Case "Int16" 'SmallInt
                Return NpgsqlTypes.NpgsqlDbType.Smallint
            Case "String"
                Return NpgsqlTypes.NpgsqlDbType.Varchar
            Case "Byte[]"
                Return NpgsqlTypes.NpgsqlDbType.Bytea
            Case "DateTime"
                Return NpgsqlTypes.NpgsqlDbType.Date
            Case "Decimal"
                Return NpgsqlTypes.NpgsqlDbType.Numeric
                'Case "Char"
                '   Return Data.DbType.Byte
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function
#End If

    Protected Function getDbType(ByVal value As Object) As System.Data.DbType
        Select Case value.GetType.Name
            Case "Boolean"
                Return Data.DbType.Boolean
            Case "Int64" 'Long
                Return Data.DbType.Int64
            Case "Int32" 'Int
                Return Data.DbType.Int32
            Case "Int16" 'SmallInt
                Return Data.DbType.Int16
            Case "String"
                Return Data.DbType.String
            Case "Byte[]"
                Return Data.DbType.Binary
            Case "DateTime"
                Return Data.DbType.DateTime
            Case "Decimal"
                Return Data.DbType.Decimal
                'Case "Char"
                '   Return Data.DbType.Byte
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function




    Protected Function getSqlDbType(ByVal value As Object) As System.Data.SqlDbType
        Select Case value.GetType.Name
            Case "Boolean"
                Return Data.SqlDbType.Bit
            Case "Int64" 'Long
                Return Data.SqlDbType.BigInt
            Case "Int32" 'Int
                Return Data.SqlDbType.Int
            Case "Int16" 'SmallInt
                Return SqlDbType.SmallInt
            Case "String"
                Return Data.SqlDbType.VarChar
            Case "Byte[]"
                Return Data.SqlDbType.Binary
            Case "DateTime"
                Return SqlDbType.DateTime
            Case "Decimal"
                Return SqlDbType.Decimal
            Case "Char"
                Return SqlDbType.Char
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function




    Protected Function getOdbcDbType(ByVal value As Object) As System.Data.Odbc.OdbcType
        Select Case value.GetType.Name

            Case "Char"

                Return Data.Odbc.OdbcType.Char
            Case "Boolean"
                Return Data.Odbc.OdbcType.Char
                '  Case "Int64" 'Long
                '     Return Data.OracleClient.OracleType.
            Case "Int32" 'Int
                Return Data.Odbc.OdbcType.Int
            Case "Int16" 'SmallInt
                Return Data.Odbc.OdbcType.SmallInt
            Case "Int64" 'SmallInt
                Return Data.Odbc.OdbcType.BigInt
            Case "String"
                Return Data.Odbc.OdbcType.VarChar
                'Case "Byte[]"
                '   Return Data.OleDb.OleDbType.Binary
            Case "DateTime"
                Return Data.Odbc.OdbcType.DateTime
            Case "Decimal"
                Return Data.Odbc.OdbcType.Double

            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select

    End Function

#If Oracle Then


    Protected Function getOracleDbType(ByVal value As Object) As System.Data.OracleClient.OracleType
        Select Case value.GetType.Name
            Case "Boolean"
                Return Data.OracleClient.OracleType.Char
                '  Case "Int64" 'Long
                '     Return Data.OracleClient.OracleType.
            Case "Int32" 'Int
                Return Data.OracleClient.OracleType.Int32
            Case "Int16" 'SmallInt
                Return Data.OracleClient.OracleType.Int16
            Case "String"
                Return Data.OracleClient.OracleType.VarChar
                'Case "Byte[]"
                '   Return Data.OleDb.OleDbType.Binary
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function

#End If


    Protected Function getOleDbType(ByVal value As Object) As System.Data.OleDb.OleDbType
        Select Case value.GetType.Name
            Case "Char"
                Return Data.OleDb.OleDbType.Char
            Case "Boolean"
                Return Data.OleDb.OleDbType.Boolean
            Case "Int64" 'Long
                Return Data.OleDb.OleDbType.BigInt
            Case "Int32" 'Int
                Return Data.OleDb.OleDbType.Integer
            Case "Int16" 'SmallInt
                Return Data.OleDb.OleDbType.SmallInt
            Case "String"
                Return Data.OleDb.OleDbType.VarChar
            Case "Byte[]"
                Return Data.OleDb.OleDbType.Binary
            Case "Decimal"
                'Roberto Rutigliano 24/02/2010 con Decimal su access non funziona!!!
                Return Data.OleDb.OleDbType.Double
            Case "Double"
                Return Data.OleDb.OleDbType.Double
            Case "DateTime"
                Return OleDb.OleDbType.Date
            Case Else
                Throw New ManagerException("Tipo di dato non supportato: " & value.GetType.Name)
        End Select
    End Function




    Public Function _addParameter(ByRef command As System.Data.Common.DbCommand, ByVal name As String, ByVal value As Object) As System.Data.Common.DbParameter
        Dim parameter As System.Data.Common.DbParameter = Nothing

        Select Case command.GetType().Name
#If MySQL Then
            Case "MySqlCommand"
                    parameter = New MySql.Data.MySqlClient.MySqlParameter
                    parameter.Value = value
                    If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                        CType(parameter, MySql.Data.MySqlClient.MySqlParameter).MySqlDbType = getMySqlDbType(value)
                    Else
                        parameter.Value = DBNull.Value
                    End If
                    parameter.ParameterName = name
                    command.Parameters.Add(parameter)
#End If
            Case "SqlCommand"
                parameter = New System.Data.SqlClient.SqlParameter
                parameter.Value = value
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    CType(parameter, System.Data.SqlClient.SqlParameter).SqlDbType = getSqlDbType(value)
                Else
                    parameter.Value = DBNull.Value
                End If
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
            Case "OleDbCommand"
                parameter = New System.Data.OleDb.OleDbParameter
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    parameter.Value = value
                    CType(parameter, System.Data.OleDb.OleDbParameter).OleDbType = getOleDbType(value)
                Else
                    parameter.Value = DBNull.Value
                End If
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
            Case "OdbcCommand"
                parameter = New System.Data.Odbc.OdbcParameter
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    parameter.Value = value
                    CType(parameter, System.Data.Odbc.OdbcParameter).OdbcType = getOdbcDbType(value)
                Else
                    parameter.Value = DBNull.Value
                End If
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
#If Oracle Then
            Case "OracleDbCommand"
                parameter = New System.Data.OracleClient.OracleParameter
                parameter.Value = value
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    CType(parameter, System.Data.OracleClient.OracleParameter).OracleType = getOracleDbType(value)
                End If
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
#End If
#If SqlServerCe Then
            Case "SqlCeCommand"
                parameter = New System.Data.SqlServerCe.SqlCeParameter
                parameter.Value = value
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    CType(parameter, System.Data.SqlServerCe.SqlCeParameter).DbType = getDbType(value)
                Else
                    parameter.Value = DBNull.Value
                End If
                ' parameter.DbTyp
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
#End If

#If PostgreSQL Then
            Case "NpgsqlCommand"
                parameter = New Npgsql.NpgsqlParameter
                parameter.Value = value
                If (value IsNot Nothing) AndAlso Not IsDBNull(value) Then
                    CType(parameter, Npgsql.NpgsqlParameter).NpgsqlDbType = getNpgsqlDbType(value)
                Else
                    parameter.Value = DBNull.Value
                End If
                ' parameter.DbTyp
                parameter.ParameterName = name
                command.Parameters.Add(parameter)
#End If
            Case Else
                Throw New ManagerException("Tipo di comando non supportato: " & command.GetType().Name)
        End Select
        Return parameter
    End Function


    Public Function _fillDataSet(ByVal command As System.Data.Common.DbCommand, _
                                Optional ByVal table As String = Nothing) As Data.DataSet


        'Roberto Rutigliano 21/12/2009
        If Me._connection.GetType().Name = "OleDbConnection" _
            OrElse Me._connection.GetType().Name = "OdbcConnection" Then
            'Per ACCESS e PostgreSQL ...
            command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
        End If


        Dim objAdap As System.Data.Common.DataAdapter
        objAdap = _factory.CreateDataAdapter()




        Dim ds As New System.Data.DataSet
        Try

            Select Case objAdap.GetType().Name
#If MySQL Then
                Case "MySqlDataAdapter"
                    DirectCast(objAdap, MySql.Data.MySqlClient.MySqlDataAdapter).SelectCommand = DirectCast(command, MySql.Data.MySqlClient.MySqlCommand)
#End If
                Case "SqlDataAdapter"
                    DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlClient.SqlCommand)
                Case "OleDbDataAdapter"
                    DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).SelectCommand = DirectCast(command, System.Data.OleDb.OleDbCommand)
#If orcle Then
                Case "OracleDataAdapter"
                    DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).SelectCommand = DirectCast(command, System.Data.OracleClient.OracleCommand)
#End If

                Case "OdbcDataAdapter"
                    DirectCast(objAdap, System.Data.Odbc.OdbcDataAdapter).SelectCommand = DirectCast(command, System.Data.Odbc.OdbcCommand)
#If PostgreSQL Then
                Case "NpgsqlDataAdapter"
                    DirectCast(objAdap, Npgsql.NpgsqlDataAdapter).SelectCommand = DirectCast(command, Npgsql.NpgsqlCommand)
#End If
                Case Else
                    Throw New ManagerException("Tipo di Adapter non supportato: " & objAdap.GetType().Name)
            End Select


            If IsNothing(table) Then
                objAdap.Fill(ds)
            Else
                Select Case objAdap.GetType().Name
#If MySQL Then
                    Case "MySqlDataAdapter"
                        DirectCast(objAdap, MySql.Data.MySqlClient.MySqlDataAdapter).Fill(ds, table)
#End If
                    Case "SqlDataAdapter"
                        DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).Fill(ds, table)
                    Case "OleDbDataAdapter"
                        DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).Fill(ds, table)
#If Oracle Then
                    Case "OracleDataAdapter"
                        DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).Fill(ds, table)
#End If
                    Case "OdbcDataAdapter"
                        DirectCast(objAdap, System.Data.Odbc.OdbcDataAdapter).Fill(ds, table)
#If PostgreSQL Then
                    Case "NpgsqlDataAdapter"
                        DirectCast(objAdap, Npgsql.NpgsqlDataAdapter).Fill(ds, table)
#End If
                    Case Else
                        Throw New ManagerException("Tipo di Adapter non supportato: " & objAdap.GetType().Name)
                End Select
            End If
        Finally
            If Not IsNothing(objAdap) Then
                objAdap.Dispose()
                objAdap = Nothing
            End If
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try
        Return ds
    End Function


    Public Function _fillDataTable(ByVal sqlQuery As String) As DataTable
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function


    Protected Sub _fillDataSet(ByVal sqlQuery As String, _
                            ByRef ds As System.Data.DataSet, _
                            Optional ByVal table As String = Nothing)

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.Connection = _connection
        command.CommandText = sqlQuery

        If Not _transaction Is Nothing Then
            command.Transaction = _transaction
        End If


        'Roberto Rutigliano 21/12/2009
        If Me._connection.GetType().Name = "OleDbConnection" _
            OrElse Me._connection.GetType().Name = "OdbcConnection" Then
            'Per ACCESS e PostgreSQL ...
            command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
        End If


        Dim objAdap As System.Data.Common.DataAdapter = Nothing

        Try

            'MODIFICA PER GESTIRE IL CASO DEL COSTRUTTORE con la CONNESSIONE.
            'in questo caso non ho il FACTORY!!!

            'Select Case objAdap.GetType().Name
            '    Case "SqlDataAdapter"
            '        DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlClient.SqlCommand)
            '    Case "OleDbDataAdapter"
            '        DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).SelectCommand = DirectCast(command, System.Data.OleDb.OleDbCommand)
            '    Case Else
            '        Throw New ManagerException("Tipo di Adapter non supportato: " & objAdap.GetType().Name)
            'End Select



            objAdap = getDataAdapter(command)


            If IsNothing(table) Then
                objAdap.Fill(ds)
            Else
                Select Case objAdap.GetType().Name
#If MySQL Then
                    Case "MySqlDataAdapter"
                        DirectCast(objAdap, MySql.Data.MySqlClient.MySqlDataAdapter).Fill(ds, table)
#End If
                    Case "SqlDataAdapter"
                        DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).Fill(ds, table)
                    Case "OleDbDataAdapter"
                        DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).Fill(ds, table)
#If Oracle Then
                    Case "OracleDataAdapter"
                        DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).Fill(ds, table)
#End If
                    Case "OdbcDataAdapter"
                        DirectCast(objAdap, System.Data.Odbc.OdbcDataAdapter).Fill(ds, table)
                    Case Else
                        Throw New ManagerException("Tipo di Adapter non supportato: " & objAdap.GetType().Name)
                End Select
            End If



        Finally
            If Not IsNothing(objAdap) Then
                objAdap.Dispose()
                objAdap = Nothing
            End If
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try
    End Sub

    Protected Function _fillDataSet(ByVal sqlQuery As String, _
                                    Optional ByVal dataSetName As String = Nothing, _
                                    Optional ByVal tableName As String = Nothing) As Data.DataSet

        Dim dataSet As System.Data.DataSet
        If dataSetName Is Nothing Then
            dataSet = New System.Data.DataSet
        Else
            dataSet = New System.Data.DataSet(dataSetName)
        End If

        _fillDataSet(sqlQuery, dataSet, tableName)

        Return dataSet

    End Function

    Public Function _executeNoQuery(ByVal sqlQuery As String) As Integer
        Dim numeroDiRecordAggiornati As Integer = 0
        Dim command As System.Data.Common.DbCommand

        command = _connection.CreateCommand()
        command.CommandText = sqlQuery
        command.Connection = Me._connection


        If Not _transaction Is Nothing Then
            command.Transaction = _transaction
        End If

        Try
            numeroDiRecordAggiornati = command.ExecuteNonQuery()

            'If (_enableLogging) Then

            'End If
        Catch ex As Exception
            Throw New ManagerException("ExecuteNonQuery terminato con errori. " & vbCrLf & command.CommandText, ex)
        Finally
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try

        Return numeroDiRecordAggiornati
    End Function


    Protected Function parseSQLforAccessAndPostgreSQL(ByVal strSQL As String) As String
        'analizzo la stringa SQL per renderla compatibile con ACCESS e PostgreSQL
        ' sostituisco @PARAMETRO con ?

        Dim pattern As New System.Text.RegularExpressions.Regex("@(([a-z]|[A-Z]|[0-9]|[_])+)")
        Dim parametro As String

        parametro = pattern.Match(strSQL).Value
        While parametro <> ""
            strSQL = strSQL.Replace(parametro & " ", "? ")
            parametro = pattern.Match(strSQL).Value
        End While

        strSQL = strSQL.Replace("GetDate()", "Now()")

        '20/05/2013
        strSQL = strSQL.Replace(" UPPER(", " UCASE(")

        Return strSQL
    End Function


    Protected Function _executeNoQuery(ByVal command As System.Data.Common.DbCommand) As Integer
        'Roberto Rutigliano 04/04/2008
        If Me._connection.GetType().Name = "OleDbConnection" _
            OrElse Me._connection.GetType().Name = "OdbcConnection" Then
            'Per ACCESS e PostgreSQL ...
            command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
        End If

        If Not _transaction Is Nothing Then
            command.Transaction = _transaction
        End If

        Dim risultato As Integer
        Try
            risultato = command.ExecuteNonQuery()
        Catch ex As Exception
            Throw New ManagerException("ExecuteNonQuery terminato con errori. " & vbCrLf & command.CommandText, ex)
        Finally
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try

        Return risultato
    End Function

    Public Function _executeScalar(ByVal sqlQuery As String) As String
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = sqlQuery
        command.Connection = Me._connection

        If Not _transaction Is Nothing Then
            command.Transaction = _transaction
        End If

        Return _executeScalar(command)
    End Function

    Public Function _executeScalar(ByVal command As System.Data.Common.DbCommand) As String
        'Roberto Rutigliano 21/12/2009
        If Me._connection.GetType().Name = "OleDbConnection" _
            OrElse Me._connection.GetType().Name = "OdbcConnection" Then
            'Per ACCESS e PostgreSQL ...
            command.CommandText = parseSQLforAccessAndPostgreSQL(command.CommandText)
        End If

        Dim obj As Object
        Dim value As String = ""
        Try

            If Not _transaction Is Nothing Then
                command.Transaction = _transaction
            End If

            obj = command.ExecuteScalar()
        Finally
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try

        If IsNothing(obj) Then
            Return ""
        Else
            Return obj.ToString
        End If
    End Function

    Protected Function _getSequence(ByVal nameSequence As String) As Integer
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        Select Case command.GetType().Name
            Case "OleDbCommand"
                command.CommandText = "select " & nameSequence & ".nextval from dual"
            Case "OdbcCommand"
                command.CommandText = "select nextval('" & nameSequence & "')"
            Case "NpgsqlCommand"
                'Postgres
                command.CommandText = "select nextval('" & nameSequence & "')"
            Case Else
                Throw New ManagerException("Get Sequence non supportato per il seguente comando: " & command.GetType().Name)
        End Select
        command.Connection = Me._connection
        Return Long.Parse(Me._executeScalar(command))
    End Function

    ''' <summary>
    ''' restituisce una stringa concatenata da gli elementi dell'array separati da un carattere di separazione
    ''' </summary>
    Public Shared Function joinStringFromArray(ByVal ar As ArrayList, ByVal delimeter As String) As String
        Return Join(ar.ToArray(), delimeter)
    End Function

    Public Shared Function isEmpty(ByVal value As DateTime) As Boolean
        '*** Attenzione con IsNothing non funziona!!!!!
        'If IsNothing(a) Then
        '    Console.WriteLine("La data è NULLA )")
        'Else
        '    Console.WriteLine("La data NON è NULLA (ERRORE!")
        'End If
        Return (value = Nothing)

        'If a = Nothing Then
        '    Console.WriteLine("La data è NULLA")
        'Else
        '    Console.WriteLine("La data NON è NULLA")
        'End If

    End Function


    Protected Function _getIdentity() As Long
        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        Select Case command.GetType().Name
            Case "MySqlCommand"
                command.CommandText = "SELECT LAST_INSERT_ID()"
            Case "SqlCommand", "OleDbCommand", "SqlCeCommand"
                command.CommandText = "SELECT @@IDENTITY"
            Case Else
                Throw New ManagerException("GetIdentity(): tipo non supportato " & command.GetType().Name)
        End Select

        command.Connection = Me._connection


        If Not _transaction Is Nothing Then
            command.Transaction = _transaction
        End If


        'Return Long.Parse(Me._executeScalar(command))
        '26/12/2009
        'Se passo per la funzione di Pasing della stringa di blocca con @@identity

        Dim obj As Object
        Dim value As String = ""
        Try
            obj = command.ExecuteScalar()
        Finally
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try

        If IsNothing(obj) Then
            Return ""
        Else
            Return obj.ToString
        End If
    End Function


    'Public Function _getIdentityOLD() As Long
    '    'Dim a As String
    '    'a = _executeScalar("SELECT SCOPE_IDENTITY() AS MY_ID")
    '    'Return Int64.Parse(a)


    '    Dim command As System.Data.Common.DbCommand
    '    command = _connection.CreateCommand()
    '    command.CommandText = "SELECT @IDENTITY"
    '    'command.CommandText = "SELECT SCOPE_IDENTITY()"
    '    'command.Connection = Me._connection


    '    'Dim reader As System.Data.Common.DbDataReader


    '    Dim temp As String
    '    Dim codiceID As Long
    '    temp = command.ExecuteReader.GetString(0)

    '    codiceID = Int64.Parse(temp)

    '    command.Dispose()
    '    Return codiceID
    'End Function


    Public Function _fillDataTable(ByVal value As Hashtable) As Data.DataTable
        Dim en As IDictionaryEnumerator
        en = value.GetEnumerator

        Dim risultato As New System.Data.DataTable
        risultato.Columns.Add("LABEL")
        risultato.Columns.Add("VALUE")

        Dim newRow As Data.DataRow
        While en.MoveNext
            newRow = risultato.NewRow
            newRow("LABEL") = en.Key
            newRow("VALUE") = en.Value
            risultato.Rows.Add(newRow)
        End While

        Return risultato
    End Function

    Public _dataAdapter As System.Data.Common.DataAdapter
    Public _dataSet As System.Data.DataSet

    'Public Function _getAdapterWithInsertAndUpdate(ByVal selectQuery As String, ByRef dt As System.Data.DataTable) As System.Data.Common.DataAdapter
    '    Dim objAdap As System.Data.Common.DataAdapter = Nothing
    '    objAdap = _getAdapterWithInsertAndUpdate(selectQuery)

    '    Dim ds As New System.Data.DataSet
    '    objAdap.Fill(ds)

    '    dt = ds.Tables(0)

    '    Return objAdap
    'End Function


    Public Function _saveByAdapter() As Integer
        'resstituisce il numero di record modificati
        Return _dataAdapter.Update(_dataSet)
    End Function

    Public Sub _initAdapterWithInsertAndUpdate(ByVal selectQuery As String)

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.Connection = _connection
        command.CommandText = selectQuery


        If Not _dataAdapter Is Nothing Then
            _dataAdapter.Dispose()
        End If

        'Dim objAdap As System.Data.Common.DataAdapter = Nothing
        _dataAdapter = getDataAdapter(command)

        'Generate automaticalli Insert, UPDATE, DELETE Command
        Dim builder As System.Data.Common.DbCommandBuilder = Nothing

        Select Case _dataAdapter.GetType().Name
#If MySQL Then
                    Case "MySqlDataAdapter"
                  '      DirectCast(objAdap, MySql.Data.MySqlClient.MySqlDataAdapter).Fill(ds, table)
#End If
            Case "SqlDataAdapter"
                builder = New System.Data.SqlClient.SqlCommandBuilder(_dataAdapter)
            Case "OleDbDataAdapter"
                builder = New System.Data.OleDb.OleDbCommandBuilder(_dataAdapter)
#If Oracle Then
            Case "OracleDataAdapter"
                builder = New System.Data.OracleClient.OracleCommandBuilder(_dataAdapter)
#End If
            Case "OdbcDataAdapter"
                builder = New System.Data.Odbc.OdbcCommandBuilder(_dataAdapter)
            Case Else
                Throw New ManagerException("Tipo di CommandBuilder non supportato: " & _dataAdapter.GetType().Name)
        End Select

        If Not _dataSet Is Nothing Then
            _dataSet.Dispose()
        End If

        _dataSet = New System.Data.DataSet
        _dataAdapter.Fill(_dataSet)

        '    dt = ds.Tables(0)

        ' Return _dataSet

    End Sub


    Private Function getDataAdapter(ByVal command As System.Data.Common.DbCommand) As System.Data.Common.DataAdapter
        Dim objAdap As System.Data.Common.DataAdapter


        Select Case _connection.GetType().Name
#If MySQL Then
                Case "MySqlConnection"
                        objAdap = New MySql.Data.MySqlClient.MySqlDataAdapter
                        DirectCast(objAdap, MySql.Data.MySqlClient.MySqlDataAdapter).SelectCommand = DirectCast(command, MySql.Data.MySqlClient.MySqlCommand)
#End If
            Case "SqlConnection"
                objAdap = New System.Data.SqlClient.SqlDataAdapter
                DirectCast(objAdap, System.Data.SqlClient.SqlDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlClient.SqlCommand)
#If SqlServerCe Then
            Case "SqlCeConnection"
                objAdap = New System.Data.SqlServerCe.SqlCeDataAdapter
                DirectCast(objAdap, System.Data.SqlServerCe.SqlCeDataAdapter).SelectCommand = DirectCast(command, System.Data.SqlServerCe.SqlCeCommand)
#End If
            Case "OleDbConnection"
                objAdap = New System.Data.OleDb.OleDbDataAdapter
                DirectCast(objAdap, System.Data.OleDb.OleDbDataAdapter).SelectCommand = DirectCast(command, System.Data.OleDb.OleDbCommand)
#If Oracle Then
            Case "OracleConnection"
                objAdap = New System.Data.OracleClient.OracleDataAdapter
                DirectCast(objAdap, System.Data.OracleClient.OracleDataAdapter).SelectCommand = DirectCast(command, System.Data.OracleClient.OracleCommand)
#End If
            Case "OdbcConnection"
                objAdap = New System.Data.Odbc.OdbcDataAdapter
                DirectCast(objAdap, System.Data.Odbc.OdbcDataAdapter).SelectCommand = DirectCast(command, System.Data.Odbc.OdbcCommand)
#If PostgreSQL Then
            Case "NpgsqlConnection"
                objAdap = New Npgsql.NpgsqlDataAdapter
                DirectCast(objAdap, Npgsql.NpgsqlDataAdapter).SelectCommand = DirectCast(command, Npgsql.NpgsqlCommand)
#End If
            Case Else
                Throw New ManagerException("Tipo di connessione non riconosciuta: " & _connection.GetType().Name)
        End Select

        Return objAdap
    End Function


    Public Shared Function updateOdbcConnectionString(ByVal value As String, ByVal newIp As String, ByVal newUser As String, ByVal newPsw As String, ByVal newDbIstance As String) As String
        Dim temp As String = value
        Dim x, y As Integer

        'Aggiorno l'indirizzo del SERVER
        x = temp.ToLower().IndexOf("server=")
        y = temp.IndexOf(";", x + 7)

        If String.IsNullOrEmpty(newIp) Then
            Throw New MyManager.ManagerException("Specificare il nuovo HOST")
            Return ""
        End If
        temp = temp.Substring(0, x + 7) & newIp & temp.Substring(y)

        'aggiorno l'istanza del database
        x = temp.ToLower().IndexOf("database=")
        y = temp.IndexOf(";", x + 9)

        If String.IsNullOrEmpty(newDbIstance) Then
            Throw New MyManager.ManagerException("Specificare il nuovo Database")
            Return ""
        End If
        temp = temp.Substring(0, x + 9) & newDbIstance & temp.Substring(y)

        'aggiorno username
        x = temp.ToLower().IndexOf("uid=")
        y = temp.IndexOf(";", x + 4)

        If String.IsNullOrEmpty(newUser) Then
            Throw New MyManager.ManagerException("Specificare la nuova user name")
            Return ""
        End If
        temp = temp.Substring(0, x + 4) & newUser & temp.Substring(y)


        'aggiorno password
        x = temp.ToLower().IndexOf("pwd=")
        y = temp.IndexOf(";", x + 4)

        If String.IsNullOrEmpty(newPsw) Then
            Throw New MyManager.ManagerException("Specificare la nuova password")
            Return ""
        End If
        temp = temp.Substring(0, x + 4) & newPsw & temp.Substring(y)

        Return temp
    End Function

    Public Shared Function updateOledbConnectionString(ByVal value As String, ByVal newDataSource As String) As String
        Dim temp As String = value
        Dim x, y As Integer

        'Aggiorno l'indirizzo del SERVER
        x = temp.ToLower().IndexOf("data source=")
        y = temp.IndexOf(";", x + 12)

        If String.IsNullOrEmpty(newDataSource) Then
            Throw New MyManager.ManagerException("Specificare il nuovo Data Source")
            Return ""
        End If
        temp = temp.Substring(0, x + 12) & newDataSource & temp.Substring(y)

        Return temp
    End Function

    Public Function _openFileAccess(ByVal fileAccess As IO.FileInfo) As Boolean
        _provider = "System.Data.OleDb"
        _factory = System.Data.Common.DbProviderFactories.GetFactory(_provider)
        _connection = _factory.CreateConnection()

        If fileAccess.Name.ToLower.EndsWith(".mdb") Then
            'Access 2003
            _connection.ConnectionString = String.Format("Provider=Microsoft.Jet.Oledb.4.0;Data Source={0};", fileAccess.FullName)
        ElseIf fileAccess.Name.ToLower.EndsWith(".accdb") Then
            'Access 2007
            _connection.ConnectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source={0};", fileAccess.FullName)
        Else
            Throw New MyManager.ManagerException("Database Access non riconosciuto" & fileAccess.Name)
            Return False
        End If

        _connection.Open()

        Return True
    End Function


    Public Shared Function getOdbcConnectionFromNpgsql(ByVal npgsqlConnection As String) As String
        Dim server As String
        Dim database As String
        Dim user As String
        Dim psw As String
        'connectionString="server=pi-cl-db2.techub.lan;database=pitalia_collaudo;uid=pitalia_collaudo_root;pwd=pitalia_collaudo;Driver={PostgreSQL Unicode};port=5432;"

        Dim x, y As Integer

        'l'indirizzo del SERVER
        x = npgsqlConnection.ToLower().IndexOf("server=")
        y = npgsqlConnection.IndexOf(";", x + 7)
        server = npgsqlConnection.Substring(x + 7, y - x - 7)

        'l'istanza del database
        x = npgsqlConnection.ToLower().IndexOf("database=")
        y = npgsqlConnection.IndexOf(";", x + 9)
        database = npgsqlConnection.Substring(x + 9, y - x - 9)

        'aggiorno username
        x = npgsqlConnection.ToLower().IndexOf("user id=")
        y = npgsqlConnection.IndexOf(";", x + 8)
        user = npgsqlConnection.Substring(x + 8, y - x - 8)

        'aggiorno password
        x = npgsqlConnection.ToLower().IndexOf("password=")
        y = npgsqlConnection.IndexOf(";", x + 9)
        psw = npgsqlConnection.Substring(x + 9, y - x - 9)

        Return String.Format("Driver={{PostgreSQL Unicode}};port=5432;server={0};database={1};uid={2};pwd={3};", server, database, user, psw)
    End Function


    Public Shared Function updateSqlClientConnectionString(ByVal value As String, ByVal newIp As String, ByVal newUser As String, ByVal newPsw As String, ByVal newDbIstance As String) As String
        Dim temp As String = value
        Dim x, y As Integer

        'Aggiorno l'indirizzo del SERVER
        x = temp.ToLower().IndexOf("data source=")
        y = temp.IndexOf(";", x + 12)

        If String.IsNullOrEmpty(newIp) Then
            Throw New MyManager.ManagerException("Specificare il nuovo HOST")
            Return ""
        End If
        temp = temp.Substring(0, x + 12) & newIp & temp.Substring(y)

        'aggiorno l'istanza del database
        x = temp.ToLower().IndexOf("initial catalog=")
        y = temp.IndexOf(";", x + 16)

        If String.IsNullOrEmpty(newDbIstance) Then
            Throw New MyManager.ManagerException("Specificare il nuovo Database")
            Return ""
        End If
        temp = temp.Substring(0, x + 16) & newDbIstance & temp.Substring(y)

        'aggiorno username
        x = temp.ToLower().IndexOf("user id=")
        y = temp.IndexOf(";", x + 8)

        If String.IsNullOrEmpty(newUser) Then
            Throw New MyManager.ManagerException("Specificare la nuova user name")
            Return ""
        End If
        temp = temp.Substring(0, x + 8) & newUser & temp.Substring(y)


        'aggiorno password
        x = temp.ToLower().IndexOf("password=")
        y = temp.IndexOf(";", x + 9)

        If String.IsNullOrEmpty(newPsw) Then
            Throw New MyManager.ManagerException("Specificare la nuova password")
            Return ""
        End If
        temp = temp.Substring(0, x + 9) & newPsw & temp.Substring(y)

        Return temp
    End Function


    Public Shared Function updateNpgsqlConnectionString(ByVal value As String, ByVal newIp As String, ByVal newUser As String, ByVal newPsw As String, ByVal newDbIstance As String) As String
        Dim temp As String = value
        Dim x, y As Integer

        'Aggiorno l'indirizzo del SERVER
        x = temp.ToLower().IndexOf("server=")
        y = temp.IndexOf(";", x + 7)

        If String.IsNullOrEmpty(newIp) Then
            Throw New MyManager.ManagerException("Specificare il nuovo HOST")
            Return ""
        End If
        temp = temp.Substring(0, x + 7) & newIp & temp.Substring(y)

        'aggiorno l'istanza del database
        x = temp.ToLower().IndexOf("database=")
        y = temp.IndexOf(";", x + 9)

        If String.IsNullOrEmpty(newDbIstance) Then
            Throw New MyManager.ManagerException("Specificare il nuovo Database")
            Return ""
        End If
        temp = temp.Substring(0, x + 9) & newDbIstance & temp.Substring(y)

        'aggiorno username
        x = temp.ToLower().IndexOf("user id=")
        y = temp.IndexOf(";", x + 8)

        If String.IsNullOrEmpty(newUser) Then
            Throw New MyManager.ManagerException("Specificare la nuova user name")
            Return ""
        End If
        temp = temp.Substring(0, x + 8) & newUser & temp.Substring(y)


        'aggiorno password
        x = temp.ToLower().IndexOf("password=")
        y = temp.IndexOf(";", x + 9)

        If String.IsNullOrEmpty(newPsw) Then
            Throw New MyManager.ManagerException("Specificare la nuova password")
            Return ""
        End If
        temp = temp.Substring(0, x + 9) & newPsw & temp.Substring(y)

        Return temp
    End Function


End Class

