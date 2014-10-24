Public Class ReflectionManager
    Inherits Manager

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Function buildInsertCommand() As System.Data.OleDb.OleDbCommand
        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO CURRICULUM ( date_added  "
        strSQLParametri = " VALUES ( Now()  "


        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand()


        'If _userId <> "" Then
        '    strSQL &= ",USER_ID "
        '    strSQLParametri &= ",@USER_ID "
        '    oleDbCommand.Parameters.Add("@USER_ID", OleDb.OleDbType.Numeric).Value = _userId
        'End If


        Dim fi() As System.Reflection.FieldInfo = ReflectionManager._getFields(Me)


        Dim field As System.Reflection.FieldInfo
        Dim valore As String
        Dim campo As String
        For Each field In fi

            If (field.GetValue(Me) IsNot Nothing) And (field.Name.StartsWith("__")) Then
                campo = field.Name.Replace("__", "")

                strSQL &= ", " & campo
                strSQLParametri &= ", @" & campo

                valore = field.GetValue(Me).ToString

                Select Case field.FieldType().ToString
                    Case "System.String"
                        oleDbCommand.Parameters.Add("@" & field.Name, OleDb.OleDbType.VarChar).Value = valore
                    Case "System.Int32"
                        oleDbCommand.Parameters.Add("@" & field.Name, OleDb.OleDbType.Numeric).Value = valore
                    Case "System.DateTime"
                        oleDbCommand.Parameters.Add("@" & field.Name, OleDb.OleDbType.Date).Value = valore

                    Case Else
                        Throw New ManagerException("ReflectionManager:  tipo di parametro non supportato " & field.FieldType().ToString)
                End Select
            End If
        Next

        oleDbCommand.CommandText = strSQL & " ) " & strSQLParametri & " )"
        Return oleDbCommand
    End Function

    Public Shared Function _getFields(ByVal val As Object) As System.Reflection.FieldInfo()
        Dim fi() As System.Reflection.FieldInfo
        Dim ty As Type = val.GetType
        fi = ty.GetFields(System.Reflection.BindingFlags.NonPublic Or _
                            System.Reflection.BindingFlags.Instance Or _
                            System.Reflection.BindingFlags.Public Or _
                            System.Reflection.BindingFlags.Static)
        Return fi
    End Function



    Public Shared Function _ToDataTable(ByVal values As Object) As Data.DataTable
        Dim field As System.Reflection.FieldInfo
        Dim fi() As System.Reflection.FieldInfo = ReflectionManager._getFields(values(0))

        Dim dt As New Data.DataTable
        'creo le colonne coni nomi dei campi
        For Each field In fi
            dt.Columns.Add(field.Name)
        Next

        Dim valore As String
        Dim item As Object
        Dim newRow As Data.DataRow

        For Each item In values
            newRow = dt.NewRow()
            For Each field In fi
                valore = field.GetValue(item)
                If (valore IsNot Nothing) Then
                    newRow(field.Name) = valore
                End If
            Next
        Next
        Return dt
    End Function

End Class
