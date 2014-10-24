
Public Class CSVManager

    Protected Shared _progressBar As Windows.Forms.ToolStripProgressBar
    Protected Shared _statusBar As Windows.Forms.ToolStripLabel

    Public Shared Sub _setProgressBar(ByRef value As Windows.Forms.ToolStripProgressBar)
        _progressBar = value
    End Sub

    Public Shared Sub _setStatusBar(ByRef value As Windows.Forms.ToolStripLabel)
        _statusBar = value
    End Sub

    Public Shared Function toXML1(ByVal pathSourceCSV As String, ByVal pathOutputXML As String) As Boolean
        Dim srdr As New System.IO.StreamReader(pathSourceCSV)
        Dim strLine As String = srdr.ReadLine()
        Dim columnArray As String() = strLine.Split("-")
        Dim writer As New System.Xml.XmlTextWriter(pathOutputXML, Nothing)

        Try
            writer.WriteStartDocument()
            writer.WriteStartElement("root")
            writer.WriteStartElement("columns")
            For i As Integer = 0 To columnArray.Length() - 1
                writer.WriteStartElement("column", "")
                writer.WriteString(columnArray(i))
                writer.WriteEndElement()
            Next
            writer.WriteEndElement()

            writer.WriteStartElement("rows")
            While Not srdr.EndOfStream()
                writer.WriteStartElement("row")
                strLine = srdr.ReadLine()
                columnArray = strLine.Split("-")
                For i As Integer = 0 To columnArray.Length() - 1
                    writer.WriteStartElement("cell", "")
                    writer.WriteString(columnArray(i))
                    writer.WriteEndElement()
                Next
                writer.WriteEndElement()
            End While
            writer.WriteEndElement()

            writer.WriteEndElement()
            writer.WriteEndDocument()
        Finally
            writer.Close()
        End Try
        Return True

    End Function

    Public Shared Function toXML(ByVal pathSourceCSV As String, ByVal pathOutputXML As String) As Boolean
        Dim srdr As New System.IO.StreamReader(pathSourceCSV)
        Dim strLine As String = srdr.ReadLine()
        Dim columnArray As String() = strLine.Split("-")
        Dim cellArray As String() = strLine.Split("-")
        Dim writer As New System.Xml.XmlTextWriter(pathOutputXML, Nothing)

        Try
            writer.WriteStartDocument()
            writer.WriteStartElement("root")
            writer.WriteStartElement("rows")
            While Not srdr.EndOfStream()
                writer.WriteStartElement("row")
                strLine = srdr.ReadLine()
                columnArray = strLine.Split("-")
                For i As Integer = 0 To columnArray.Length() - 1
                    writer.WriteStartElement(cellArray(i), "")
                    writer.WriteString(columnArray(i))
                    writer.WriteEndElement()
                Next
                writer.WriteEndElement()
            End While
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndDocument()
        Finally
            writer.Close()
        End Try
        Return True
    End Function

    Public Shared Function toCSV(ByVal dtable As DataTable, Optional ByVal delimitedBy As String = ";", Optional ByVal includeColumnsHeader As Boolean = True) As String
        Dim sb As New System.Text.StringBuilder

        If Not _progressBar Is Nothing Then
            _progressBar.Maximum = dtable.Rows.Count
            _progressBar.Value = 0
            _progressBar.Step = 1
        End If

        Dim result As String = ""
        Dim icolcount As Integer = dtable.Columns.Count

        If includeColumnsHeader Then
            For i As Integer = 0 To icolcount - 1
                sb.Append(dtable.Columns(i))
                If i < icolcount - 1 Then
                    sb.Append(delimitedBy)
                End If
            Next
            sb.AppendLine()
        End If

        Dim conta = 0
        For Each drow As DataRow In dtable.Rows
            For i As Integer = 0 To icolcount - 1
                If Not Convert.IsDBNull(drow(i)) Then
                    sb.Append(drow(i).ToString())
                End If
                If i < icolcount - 1 Then
                    sb.Append(delimitedBy)
                End If
            Next
            sb.AppendLine()
            conta = conta + 1

            If Not _statusBar Is Nothing Then
                _statusBar.Text = "Generazione file CSV: Record " & conta & "/" & dtable.Rows.Count
                Windows.Forms.Application.DoEvents()
            End If

            If Not _progressBar Is Nothing Then
                _progressBar.PerformStep()
                Windows.Forms.Application.DoEvents()
            End If

        Next

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If
        If Not _statusBar Is Nothing Then
            _statusBar.Text = ""
        End If

        result = sb.ToString()
        Return result
    End Function

    Public Shared Function writeFileCSV(ByVal dtable As DataTable, ByVal strFilePath As String, Optional ByVal delimitedBy As String = ";", Optional ByVal includeColumnsHeader As Boolean = True) As Boolean
        Dim result As String
        result = CSVManager.toCSV(dtable, delimitedBy, includeColumnsHeader)
        If String.IsNullOrEmpty(result) Then
            Return False
        End If

        IO.File.WriteAllText(strFilePath, result)
        Return True
    End Function




    Public Shared Function writeFileCSV(ByVal dataGrid As System.Windows.Forms.DataGridView, ByVal strFilePath As String, Optional ByVal delimitedBy As String = ";", Optional ByVal includeColumnsHeader As Boolean = True) As Boolean
        Dim result As String
        result = CSVManager.toCSV(dataGrid, delimitedBy, includeColumnsHeader)
        If String.IsNullOrEmpty(result) Then
            Return False
        End If

        IO.File.WriteAllText(strFilePath, result)
        Return True
    End Function

    Public Shared Function toCSV(ByVal dataGrid As System.Windows.Forms.DataGridView, Optional ByVal delimitedBy As String = ";", Optional ByVal includeColumnsHeader As Boolean = True) As String
        Dim sb As New System.Text.StringBuilder

        If Not _progressBar Is Nothing Then
            _progressBar.Maximum = dataGrid.Rows.Count
            _progressBar.Value = 0
            _progressBar.Step = 1
        End If

        Dim result As String = ""
        Dim icolcount As Integer = dataGrid.Columns.Count

        If includeColumnsHeader Then
            For i As Integer = 0 To icolcount - 1
                sb.Append(dataGrid.Columns(i).HeaderText)
                If i < icolcount - 1 Then
                    sb.Append(delimitedBy)
                End If
            Next
            sb.AppendLine()
        End If

        Dim conta = 0
        For Each drow As Windows.Forms.DataGridViewRow In dataGrid.Rows


            If Not drow.IsNewRow Then
                For i As Integer = 0 To icolcount - 1
                    If Not Convert.IsDBNull(drow.Cells(i)) Then
                        sb.Append(drow.Cells(i).Value)
                    End If
                    If i < icolcount - 1 Then
                        sb.Append(delimitedBy)
                    End If
                Next
                sb.AppendLine()
            End If

           
            conta = conta + 1

            If Not _statusBar Is Nothing Then
                _statusBar.Text = "Generazione file CSV: Record " & conta & "/" & dataGrid.Rows.Count
                Windows.Forms.Application.DoEvents()
            End If

            If Not _progressBar Is Nothing Then
                _progressBar.PerformStep()
                Windows.Forms.Application.DoEvents()
            End If

        Next

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If
        If Not _statusBar Is Nothing Then
            _statusBar.Text = ""
        End If

        result = sb.ToString()
        Return result
    End Function

  
End Class
