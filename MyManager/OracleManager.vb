Public Class OracleManager
    ' Inherits Manager


#Const Oracle = False


#If Oracle Then



    Public _connectionOracle As Oracle.DataAccess.Client.OracleConnection
    'Private _adapterOracle As Oracle.DataAccess.Client.OracleDataAdapter
    Private _strSql As String

    Public Sub New(ByVal connectionName As String)
        _connectionOracle = New Oracle.DataAccess.Client.OracleConnection()
        _connectionOracle.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(connectionName).ConnectionString
    End Sub

    Public Sub openConnectionOracle()
        If _connectionOracle.State <> ConnectionState.Open Then
            _connectionOracle.Open()
        End If
    End Sub

    Public Sub closeConnectionOracle()
        _connectionOracle.Close()
        _connectionOracle.Dispose()
    End Sub



    Public Function getTablesByOwner(ByVal owner As String) As Data.DataTable
        _strSql = "select TABLE_NAME, table_type from ALL_CATALOG where owner = '" & owner & "' AND table_type <> 'SYNONYM' "

        Return _fillDataTable(_strSql)
    End Function


    'Public Function getScriptCreateTable2(ByVal oracleTableName As String, ByVal oracleOwner As String) As String

    '    oracleTableName = "STS_ARRAY"

    '    _strSql = "select * from STSVIEW." & oracleTableName & " where 1=2"

    '    Dim command As Oracle.DataAccess.Client.OracleCommand
    '    command = _connectionOracle.CreateCommand()
    '    command.Connection = _connectionOracle
    '    command.CommandText = _strSql
    '    command.CommandType = CommandType.Text



    '    'Oracle.DataAccess.Client.oracle()


    '    Dim ds As New Data.DataSet
    '    _adapterOracle = New Oracle.DataAccess.Client.OracleDataAdapter()
    '    _adapterOracle.SelectCommand = command
    '    _adapterOracle.Fill(ds)

    '    For Each col As Data.DataColumn In ds.Tables(0).Columns

    '        Console.WriteLine(col.ColumnName & " -> " & col.DataType.Name)

    '    Next

    '    Return True
    'End Function



    Public Function getScriptCreateTable(ByVal oracleTableName As String, ByVal oracleOwner As String) As String

        _strSql = "select  column_name,  data_type, data_length, data_scale  from ALL_TAB_COLUMNS where table_name = '" & oracleTableName & "' AND owner ='" & oracleOwner & "'"

        'Dim command As Oracle.DataAccess.Client.OracleCommand
        'command = _connectionOracle.CreateCommand()
        'command.Connection = _connectionOracle
        'command.CommandText = _strSql
        'command.CommandType = CommandType.Text





        'Dim dr As Oracle.DataAccess.Client.OracleDataReader
        'dr = command.ExecuteReader
        'While dr.Read
        '    Console.WriteLine(dr.ToString)
        'End While
        'dr.Close()


        '        command.ExecuteReader()

       '

        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)

        Dim temp As String = "CREATE TABLE " & oracleTableName & " (" & vbCrLf
        Dim SQLSERVER_dataType As String

        For Each row As Data.DataRow In dt.Rows

            'Data type mapping Oracle VS T-SQL 
            'http://msdn.microsoft.com/en-us/library/ms151817.aspx

            Select Case row("data_type").ToString.ToUpper
                Case "VARCHAR2"

                    If row("data_length") = 0 Then
                        SQLSERVER_dataType = "VARCHAR"
                    Else
                        SQLSERVER_dataType = "VARCHAR(" & row("data_length") & ")"
                    End If

                Case "NUMBER"
                    ' SQLSERVER_dataType = "NUMERIC(" & row("data_length") & ",38 )"
                    'SQLSERVER_dataType = "NUMERIC(38,38 )"

                    If Not IsDBNull(row("data_scale")) AndAlso row("data_scale") = 0 Then
                        SQLSERVER_dataType = "NUMERIC(38,0)"
                    Else
                        SQLSERVER_dataType = "FLOAT"
                    End If

                Case "DATE"
                    SQLSERVER_dataType = "DATETIME"
                Case "FLOAT"
                    SQLSERVER_dataType = "FLOAT(" & row("data_length") & ")"
                Case "CHAR"
                    SQLSERVER_dataType = "CHAR(" & row("data_length") & ")"
                Case Else
                    Throw New ManagerException("Tipo di dato non supportato: " & row("data_type").ToString)
            End Select

            temp &= String.Format(vbTab & "{0} {1} NULL,", row("column_name"), SQLSERVER_dataType) & vbCrLf
        Next

        'per ognbi tabella aggiungo i seguenti campi

        temp &= vbTab & "CHAR_FIELD_1 CHAR(255) NULL," & vbCrLf
        temp &= vbTab & "CHAR_FIELD_2 CHAR(255) NULL," & vbCrLf
        temp &= vbTab & "CHAR_FIELD_3 CHAR(255) NULL," & vbCrLf
        temp &= vbTab & "CHAR_FIELD_4 CHAR(255) NULL," & vbCrLf
        temp &= vbTab & "CHAR_FIELD_5 CHAR(255) NULL," & vbCrLf

        temp &= vbTab & "DATE_FIELD_1 DATETIME NULL," & vbCrLf
        temp &= vbTab & "DATE_FIELD_2 DATETIME NULL," & vbCrLf
        temp &= vbTab & "DATE_FIELD_3 DATETIME NULL," & vbCrLf
        temp &= vbTab & "DATE_FIELD_4 DATETIME NULL," & vbCrLf
        temp &= vbTab & "DATE_FIELD_5 DATETIME NULL," & vbCrLf

        temp &= vbTab & "VARCHAR_FIELD_1 CHAR(512) NULL," & vbCrLf
        temp &= vbTab & "VARCHAR_FIELD_2 CHAR(512) NULL," & vbCrLf
        temp &= vbTab & "VARCHAR_FIELD_3 CHAR(512) NULL," & vbCrLf
        temp &= vbTab & "VARCHAR_FIELD_4 CHAR(512) NULL," & vbCrLf
        temp &= vbTab & "VARCHAR_FIELD_5 CHAR(512) NULL," & vbCrLf

        temp &= vbTab & "DATACENTER_ID CHAR(3) NULL," & vbCrLf
        temp &= vbTab & "IMPORT_ID int  NULL," & vbCrLf
        temp &= vbTab & "LOAD_DATE DATETIME DEFAULT GETDATE()," & vbCrLf
        temp &= vbTab & "IS_RECORD_ACTIVE NUMERIC(1)  DEFAULT 1," & vbCrLf
        temp &= vbTab & "ID int IDENTITY (1,1) PRIMARY KEY " & vbCrLf

        'temp = temp.Substring(0, temp.Length - 3)

        temp &= ");" & vbCrLf



        temp &= "ALTER TABLE " & oracleTableName & " ADD CONSTRAINT " & _
            "FK_" & oracleTableName & "_IMPORT_ID FOREIGN KEY " & _
            " (  import_id ) REFERENCES ECC_IMPORT ( id ) " & _
            " ON UPDATE  NO ACTION  " & _
            " ON DELETE  NO ACTION ; " & vbCrLf


        temp &= "ALTER TABLE " & oracleTableName & " ADD CONSTRAINT " & _
          "FK_" & oracleTableName & "_DC_ID FOREIGN KEY " & _
          " (  DATACENTER_ID ) REFERENCES DC ( DC_SIGLA ) " & _
          " ON UPDATE  NO ACTION  " & _
          " ON DELETE  NO ACTION ; " & vbCrLf


        temp &= "GO" & vbCrLf

        Return temp

    End Function





    Public Function getScriptINSERT_INTO(ByVal oracleTableName As String, ByVal oracleOwner As String) As String
        _strSql = "select  column_name,  data_type, data_length, data_scale  from ALL_TAB_COLUMNS where table_name = '" & oracleTableName & "' AND owner ='" & oracleOwner & "'"
        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)

        Dim temp As String = "INSERT INTO " & oracleTableName
        Dim myFields As String = ""
        Dim myFields_2 As String = ""
     
        For Each row As Data.DataRow In dt.Rows
            myFields &= row("column_name") & ", "

            Select Case row("data_type").ToString.ToUpper
                Case "NUMBER"
                   
                    If Not IsDBNull(row("data_scale")) AndAlso row("data_scale") = 0 Then
                        myFields_2 &= String.Format("CAST(REPLACE ({0},',','.') as NUMERIC) AS {0}, ", row("column_name"))
                    Else
                        myFields_2 &= String.Format("CAST(REPLACE ({0},',','.') as FLOAT) AS {0}, ", row("column_name"))
                    End If

                   
                Case Else
                    myFields_2 &= row("column_name") & ", "
                    '   Throw New ManagerException("Tipo di dato non supportato: " & row("data_type").ToString)
            End Select
        Next

        myFields = myFields.Substring(0, myFields.Length - 2)

        myFields_2 = myFields_2.Substring(0, myFields_2.Length - 2)

        temp = temp & "( DATACENTER_ID,  " & myFields & ") SELECT 'BO1'AS DATACENTER_ID , " & myFields_2 & " FROM [RAMBDB]..[STSVIEW]." & oracleTableName & ";" & vbCrLf


        If oracleTableName = "STS_ARRAY" Then
            Dim debugg As Boolean
            debugg = True

        End If

        Return temp

    End Function




    Public Function getScriptEXEC(ByVal oracleTableName As String, ByVal oracleOwner As String) As String
        _strSql = "select  column_name,  data_type, data_length, data_scale  from ALL_TAB_COLUMNS where table_name = '" & oracleTableName & "' AND owner ='" & oracleOwner & "'"
        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)

        Dim temp As String = ""

        temp &= "SELECT @isEnabled = is_enabled FROM ECC_TABLES WHERE table_name = '" & oracleTableName & "';" & vbCrLf
        temp &= "if @isEnabled  = 1 " & vbCrLf

            temp &= "EXEC ('INSERT INTO " & oracleTableName
            Dim myFields As String = ""
            Dim myFields_2 As String = ""

            For Each row As Data.DataRow In dt.Rows
                myFields &= row("column_name") & ", "

                Select Case row("data_type").ToString.ToUpper
                    Case "NUMBER"

                        If Not IsDBNull(row("data_scale")) AndAlso row("data_scale") = 0 Then
                            myFields_2 &= String.Format("CAST({0} as NUMERIC) AS {0}, ", row("column_name"))
                        Else
                            myFields_2 &= String.Format("CAST({0} as FLOAT) AS {0}, ", row("column_name"))
                        End If


                    Case Else
                        myFields_2 &= row("column_name") & ", "
                        '   Throw New ManagerException("Tipo di dato non supportato: " & row("data_type").ToString)
                End Select
            Next

            myFields = myFields.Substring(0, myFields.Length - 2)

            myFields_2 = myFields_2.Substring(0, myFields_2.Length - 2)

        temp = temp & "( DATACENTER_ID, IMPORT_ID, " & myFields & ")  "


        temp &= "SELECT * FROM OPENQUERY(' + @stLinkedServer + ', ''SELECT ''''' + @stDataCenterID + ''''',  ''''' + @newRowId + ''''' AS REF_ID,  " & myFields_2 & " FROM STSVIEW." & oracleTableName & "'')');" & vbCrLf & vbCrLf


            If oracleTableName = "STS_ARRAY" Then
                Dim debugg As Boolean
                debugg = True

            End If

            Return temp

    End Function


    Public Function getScriptReports(ByVal oracleTableName As String, ByVal oracleOwner As String) As String
        _strSql = "select  column_name,  data_type, data_length, data_scale  from ALL_TAB_COLUMNS where table_name = '" & oracleTableName & "' AND owner ='" & oracleOwner & "'"
        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)

        Dim temp As String = ""

        temp &= "SELECT DATACENTER_ID , CAST (LOAD_DATE as DATE) as LOAD_DATE, "

        Dim myFields As String = ""

        For Each row As Data.DataRow In dt.Rows
            myFields &= row("column_name") & ", "
        Next

        myFields = myFields.Substring(0, myFields.Length - 2)


        temp &= myFields & " FROM  " & oracleTableName & ";" & vbCrLf


        Return temp

    End Function


    Public Function _fillDataTable(ByVal sqlQuery As String) As DataTable
        Dim command As Oracle.DataAccess.Client.OracleCommand
        command = _connectionOracle.CreateCommand()
        command.Connection = _connectionOracle
        command.CommandText = _strSql
        command.CommandType = CommandType.Text

        Dim ds As New Data.DataSet

        Dim objAdapter As New Oracle.DataAccess.Client.OracleDataAdapter()
        objAdapter.SelectCommand = command

        Try
            objAdapter.Fill(ds)

            Return ds.Tables(0)
        Finally
            If Not IsNothing(objAdapter) Then
                objAdapter.Dispose()
                objAdapter = Nothing
            End If
            If Not IsNothing(command) Then
                command.Dispose()
                command = Nothing
            End If
        End Try
    End Function
#End If
End Class
