Public Class DataTableManager

    Public Enum MyGroupBy
        count
        sum
        max
        min
        avg
    End Enum

    Protected Shared _progressBar As Windows.Forms.ToolStripProgressBar
    Protected Shared _statusBar As Windows.Forms.ToolStripLabel

    Public Shared Sub _setProgressBar(ByRef value As Windows.Forms.ToolStripProgressBar)
        _progressBar = value
    End Sub

    Public Shared Sub _setStatusBar(ByRef value As Windows.Forms.ToolStripLabel)
        _statusBar = value
    End Sub

    Public Shared Function getGroupedDatatable(ByVal inputDataTable As System.Data.DataTable, ByVal label As String, ByVal values() As String) As DataTable
        Dim key As String
        Dim tempSum As Double
        'crea un hashtable per ogni campo da raggruppare
        Dim goupBy(values.Length) As Hashtable
        Dim i = 0
        For i = 0 To values.Length - 1
            goupBy(i) = New Hashtable()
        Next

        'riempie gli hashtable con le somme
        For i = 0 To values.Length - 1
            For Each row As DataRow In inputDataTable.Rows
                key = row(label)
                Try
                    If goupBy(i).Contains(key) Then
                        tempSum = goupBy(i)(key)
                        tempSum = tempSum + row(values(i))
                        goupBy(i).Remove(key)
                        goupBy(i).Add(key, tempSum)
                    Else
                        goupBy(i).Add(key, row(values(i)))
                    End If
                Catch ex As Exception 'se il campo da raggruppare non è presenta lui lo conta come ZERO per evitare problemi in runtime
                    If Not goupBy(i).Contains(key) Then
                        goupBy(i).Add(key, 0)
                    End If
                End Try
            Next
        Next
        'crea un nuovo DataTable del tipo {LABEL|VAL_1|VAL_2|...|VAL_N}
        Dim ordered As New DataTable(inputDataTable.TableName + "_ordered")
        Dim column(values.Length + 1) As DataColumn
        column(0) = New DataColumn("Label")
        column(0).DataType = System.Type.GetType("System.String")
        ordered.Columns.Add(column(0))
        For i = 0 To values.Length - 1
            column(i + 1) = New DataColumn(values(i))
            column(i + 1).DataType = System.Type.GetType("System.Double")
            ordered.Columns.Add(column(i + 1))
        Next
        'riempie il nuovo DataTable
        Dim newRow As DataRow
        For Each keys As String In goupBy(0).Keys
            newRow = ordered.NewRow()
            newRow.Item("Label") = keys
            For i = 0 To values.Length - 1
                newRow.Item(values(i)) = goupBy(i)(keys)
            Next
            ordered.Rows.Add(newRow)
        Next
        Return ordered
    End Function



    Public Shared Function printDataTable(ByVal inputDataTable As System.Data.DataTable) As String
        Dim row As System.Data.DataRow
        Dim risultato As String = ""
        Dim i As Integer
        For Each row In inputDataTable.Rows
            For i = 0 To inputDataTable.Columns.Count - 1
                risultato &= row(i).ToString & " "
            Next
            risultato &= vbCrLf
        Next
        Return risultato

    End Function


    Public Shared Function toXML(ByVal dt As DataTable, Optional ByVal includeColumnsHeader As Boolean = False) As String
        Dim xmlString As String
        Dim j As Integer = 0
        Dim columnsCount As Integer = dt.Columns.Count - 1

        If Not _progressBar Is Nothing Then
            _progressBar.Maximum = dt.Rows.Count
            _progressBar.Value = 0
            _progressBar.Step = 1
        End If

        xmlString = "<root>" & vbCrLf

        If includeColumnsHeader Then
            xmlString &= "<columns>" & vbCrLf
            For Each dc As DataColumn In dt.Columns
                xmlString &= "<column>" & dc.ToString & "</column>" & vbCrLf
            Next
            xmlString &= "</columns>"
        End If

        xmlString &= vbCrLf & "<rows>"
        Dim conta = 0
        For Each row As Data.DataRow In dt.Rows
            If Not j > columnsCount Then
                xmlString &= vbCrLf & "<row>" & vbCrLf
                For Each col As DataColumn In dt.Columns
                    xmlString &= "<" & col.ToString & ">" & row(col).ToString & "</" & col.ToString & ">" & vbCrLf
                    j += 1
                Next
                xmlString &= "</row>"
                conta = conta + 1

                If Not _statusBar Is Nothing Then
                    _statusBar.Text = "Generazione file XML: Record " & conta & "/" & dt.Rows.Count
                    Windows.Forms.Application.DoEvents()
                End If

                If Not _progressBar Is Nothing Then
                    _progressBar.PerformStep()
                    Windows.Forms.Application.DoEvents()
                End If
            End If
            j = 0
        Next
        xmlString &= vbCrLf & "</rows>" & vbCrLf
        xmlString &= "</root>"

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If
        If Not _statusBar Is Nothing Then
            _statusBar.Text = ""
        End If

        Return xmlString
    End Function

    Public Shared Function toXML(ByVal dtable As DataTable, ByVal strFilePath As String, Optional ByVal includeColumnsHeader As Boolean = False) As Boolean
        Dim result As String
        result = toXML(dtable, includeColumnsHeader)
        If String.IsNullOrEmpty(result) Then
            Return False
        End If

        IO.File.WriteAllText(strFilePath, result)
        Return True
    End Function

    Public Function toXML(ByVal dt As DataTable, ByVal myFile As IO.FileInfo) As Boolean
        Dim writer As New System.Xml.XmlTextWriter(myFile.FullName, Nothing)
        Try
            If Not dt Is Nothing Then
                If dt.Columns.Count <> 0 Then

                    If Not _progressBar Is Nothing Then
                        _progressBar.Maximum = dt.Rows.Count
                        _progressBar.Value = 0
                        _progressBar.Step = 1
                    End If

                    Dim columnsCount As Integer = dt.Columns.Count - 1
                    Dim j As Integer = 0
                    writer.WriteStartDocument()
                    writer.WriteStartElement("root")

                    '					writer.WriteStartElement("columns")
                    '					For Each dc As DataColumn In dt.Columns
                    '						writer.WriteStartElement("column", "")
                    '	                	writer.WriteString(dc.ToString)
                    '	                	writer.WriteEndElement()
                    '					Next
                    '					writer.WriteEndElement()

                    writer.WriteStartElement("rows")
                    Dim conta = 0
                    For Each row As Data.DataRow In dt.Rows

                        If Not j > columnsCount Then
                            writer.WriteStartElement("row")
                            For Each col As DataColumn In dt.Columns
                                writer.WriteStartElement(col.ToString, "")
                                writer.WriteString(row(col).ToString)
                                writer.WriteEndElement()
                                j += 1
                            Next
                            writer.WriteEndElement()
                            conta = conta + 1
                            If Not _progressBar Is Nothing Then
                                _progressBar.PerformStep()
                            End If

                            If Not _statusBar Is Nothing Then
                                _statusBar.Text = "Generazione file XML: Record " & conta & "/" & dt.Rows.Count
                                Windows.Forms.Application.DoEvents()
                            End If

                        End If
                        j = 0
                    Next
                    writer.WriteEndElement()
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                End If

            End If
        Finally
            writer.Close()
        End Try

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If
        If Not _statusBar Is Nothing Then
            _statusBar.Text = ""
        End If

        Return True
    End Function


End Class
