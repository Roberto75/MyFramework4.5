Public Class LogManager
    Inherits Manager

    Public Enum MyLevel
        undefined
        MyInfo
        MyWarning
        MyError
    End Enum


    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New()
        MyBase.New("Log")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub insert(ByVal tipo As String, ByVal nota As String, ByVal referenceId As String, referenceType As String, myLevel As MyLevel)
        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO LOG ( DATE_ADDED  "
        strSQLParametri = " VALUES ( @DATE_ADDED  "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = _connection

        Me._addParameter(command, "@DATE_ADDED", DateTime.Now)

        If Not String.IsNullOrEmpty(nota) Then
            strSQL &= ",nota "
            strSQLParametri &= ", @nota "
            Me._addParameter(command, "@nota", nota)
        End If

        If Not String.IsNullOrEmpty(tipo) Then
            strSQL &= ",tipo "
            strSQLParametri &= ", @tipo "
            Me._addParameter(command, "@tipo", tipo)
        End If

        If Not String.IsNullOrEmpty(referenceId) Then
            strSQL &= ",reference_id "
            strSQLParametri &= ", @referenceId "
            Me._addParameter(command, "@referenceId", referenceId)
        End If

        If Not String.IsNullOrEmpty(referenceType) Then
            strSQL &= ",reference_type "
            strSQLParametri &= ", @referenceType "
            Me._addParameter(command, "@referenceType", referenceType)
        End If

        If myLevel <> LogManager.MyLevel.undefined Then
            strSQL &= ",my_level "
            strSQLParametri &= ", @MY_LEVEL "
            Me._addParameter(command, "@MY_LEVEL", myLevel.ToString)
        End If

        command.CommandText = strSQL & " ) " & strSQLParametri & " )"
        command.CommandType = CommandType.Text

        command.ExecuteNonQuery()
    End Sub



    Public Function getLogById(logId As Long) As Data.DataTable
        _strSql = "select * from LOG  WHERE LOG_ID = " & logId
        Return _fillDataTable(_strSql)
    End Function
End Class
