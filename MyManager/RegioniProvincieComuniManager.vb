﻿Public Class RegioniProvinceComuniManager
    Inherits Manager

    'http://www.istat.it/strumenti/definizioni/comuni/ripartizioni_regioni_province.xls


    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub


    Public Function getProvince(ByVal regioneId As Long) As Data.DataTable
        _strSql = "SELECT * FROM PROVINCE WHERE regione_id =" & regioneId & "  ORDER BY VALORE"
        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)
        Return dt
    End Function


    Public Function getComuni(ByVal provinciaId As String) As Data.DataTable
        'provinciaId = SIGLA
        'Escludo l'ITALIA tra gli STATI ESTERI
        _strSql = "SELECT * FROM COMUNI WHERE provincia_id = '" & provinciaId & "' AND ID <> 'IT' ORDER BY VALORE"
        Dim dt As Data.DataTable
        dt = _fillDataTable(_strSql)
        Return dt
    End Function


    Public Function getRegioneByLabel(ByVal regione As String) As Data.DataTable
        ' _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = '" & regione.ToUpper & "'"
        _strSql = "SELECT * FROM REGIONI WHERE UCASE (VALORE)  = @REGIONE "

        Dim command As System.Data.Common.DbCommand
        command = _connection.CreateCommand()

        command.CommandText = _strSql

        _addParameter(command, "@REGIONE", regione.ToUpper)

        Return _fillDataSet(command).Tables(0)
    End Function

    Public Function getProvinciaBySigla(ByVal sigla As String) As Data.DataTable
        _strSql = "SELECT * FROM PROVINCE WHERE UCASE (ID)  = '" & sigla.ToUpper & "'"
        Return _fillDataTable(_strSql)
    End Function

    Public Function getProvinciaByValore(ByVal valore As String) As Data.DataTable
        _strSql = "SELECT * FROM PROVINCE WHERE UCASE (valore)  = '" & valore.ToUpper.Replace("'", "''") & "'"
        Return _fillDataTable(_strSql)
    End Function

    Public Function getComuneByCodiceISTAT(ByVal cidiceISTAT As String) As Data.DataTable
        _strSql = "SELECT * FROM COMUNI WHERE UCASE (ID)  = '" & cidiceISTAT.ToUpper & "'"
        Return _fillDataTable(_strSql)
    End Function

    Public Function getComuneByValore(ByVal valore As String) As Data.DataTable
        _strSql = "SELECT * FROM COMUNI WHERE UCASE (valore)  = '" & valore.ToUpper.Replace("'", "''") & "'"
        Return _fillDataTable(_strSql)
    End Function

    Public Function getNazioneByDescrizione(ByVal valore As String) As Data.DataTable
        _strSql = "SELECT * FROM COMUNI WHERE UCASE (VALORE)  = '" & valore.ToUpper.Replace("'", "''") & "'"
        Return _fillDataTable(_strSql)
    End Function



    

End Class
