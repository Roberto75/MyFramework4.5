'*** Add Reference:  
'*** Microsoft.Office.Interop.Execel 12.0.0.0

Public Class ExcelManager
    Inherits MyManager.Manager


#Const lsActive = False


#If lsActive Then


    Public Function _getVersion() As String
        If _objExcel Is Nothing Then
            _objExcel = New Microsoft.Office.Interop.Excel.Application
            _objExcel.Visible = False
        End If
        Return _objExcel.Version
    End Function


    Public Sub _showExcel(ByVal rowIndex As Integer)
        'deseleziono eventuali celle
        '_objExcel.ActiveCell (
        'posiziono il cursore nella cella A1
        _cell = _objExcel.Worksheets(1).Cells(rowIndex, "A")
        _cell.Select()

        _objExcel.Visible = True
    End Sub


    Public Sub _showExcel()
        '_showExcel(1)
        _objExcel.Visible = True
    End Sub

    Public Sub _clearAllSheet()
        _workSheet.Cells.Clear()
    End Sub

    Public Sub _clearColumn(ByVal columnLetter As String)
        _cell = _workSheet.Range(columnLetter & ":" & columnLetter)
        _cell.Clear()

    End Sub

    Public Sub _hideExcel()
        _objExcel.Visible = False
    End Sub

    Public _objExcel As Microsoft.Office.Interop.Excel.Application
    Public _workBooks As Microsoft.Office.Interop.Excel.Workbook
    Public _workSheet As Microsoft.Office.Interop.Excel.Worksheet
    Public _cell As Microsoft.Office.Interop.Excel.Range

    Public _currentLine As Long

    Public Sub New(ByRef connection As System.Data.Common.DbConnection, ByRef objExcel As Microsoft.Office.Interop.Excel.Application)
        MyBase.New(connection)
        Me._objExcel = objExcel
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function _saveFileExcel() As Boolean

        If _workBooks Is Nothing Then
            Return False
        End If

        'lo rendo visibile perchè potrebbe comparire un messaggio per l'utente da ceccare!
        _objExcel.Visible = True

        'MI DA ERRORE!!!
        '_workBooks.Save()

        Return True
    End Function


      

    Public Function _openFileExcel(ByVal fullFileName As String) As Boolean
        Try
            _workBooks = _objExcel.Workbooks.Open(fullFileName)
        Catch ex As Exception
            _workSheet = Nothing
            _workBooks = Nothing
            _objExcel = Nothing
            _objExcel = New Microsoft.Office.Interop.Excel.Application
            _objExcel.Visible = False
            _workBooks = _objExcel.Workbooks.Open(fullFileName)
        End Try

        Return True
    End Function

    Public Function _closeFileExcel() As Boolean
        _workBooks.Close(False)

        Return True
    End Function


    Public Sub _formatEuroCurrency(ByVal cell As Microsoft.Office.Interop.Excel.Range)
        cell.NumberFormat = "€ #.##0,00;-€ #.##0,00"
    End Sub


    Public Sub _format2Decimal(ByVal cell As Microsoft.Office.Interop.Excel.Range)
        cell.NumberFormat = "#.##0,00"
    End Sub


    Public Sub _formatPercent(ByVal cell As Microsoft.Office.Interop.Excel.Range)
        cell.NumberFormat = "0,00%"
    End Sub

    ' Public Sub _formatEuroCurrencyRed(ByVal cell As Microsoft.Office.Interop.Excel.Range)
    '    cell.NumberFormat = "€ #.##0,00;[Red] [<0] -€ #.##0,00 "
    'End Sub


    Public Sub _openExcel()
        If _objExcel Is Nothing Then
            _objExcel = New Microsoft.Office.Interop.Excel.Application
            _objExcel.Visible = False
        End If
    End Sub


    Public Sub _closeExcel()

        If Not _workSheet Is Nothing Then
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_workSheet)
            _workSheet = Nothing
        End If


        If Not _workBooks Is Nothing Then
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_workBooks)
            _workBooks = Nothing
        End If


        If Not _objExcel Is Nothing Then
            Try
                _objExcel.Quit()
            Catch ex As Exception
                Dim proc As System.Diagnostics.Process
                For Each proc In System.Diagnostics.Process.GetProcessesByName("EXCEL")
                    proc.Kill()
                Next
            End Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_objExcel)
        End If
        _objExcel = Nothing
        GC.Collect()

    End Sub


    Public Function _openTemplate(ByVal teplateName As String) As String
        Return _openTemplate(teplateName, "")
    End Function


    Public Function _openTemplate(ByVal teplateName As String, ByVal newFileName As String) As String

        If Not IO.File.Exists(Windows.Forms.Application.StartupPath & "\Template\" & teplateName) Then
            Throw New MyManager.ManagerException("Template non presente nella cartella")
            Return ""
        End If

        Dim newFullFileName As String

        If String.IsNullOrEmpty(newFileName) Then
            newFullFileName = Windows.Forms.Application.StartupPath & "\Reports\"
            If Not System.IO.Directory.Exists(newFullFileName) Then
                System.IO.Directory.CreateDirectory(newFullFileName)
            End If
            newFullFileName = newFullFileName & teplateName.Replace(".xls", "") & "_" & Now.Year.ToString & String.Format("{0:00}", Now.Month) & String.Format("{0:00}", Now.Day) & "_" & String.Format("{0:00}", Now.Hour) & String.Format("{0:00}", Now.Minute) & String.Format("{0:00}", Now.Second) & ".xls"
        Else
            newFullFileName = newFileName
        End If


        Try
            System.IO.File.Copy(Windows.Forms.Application.StartupPath & "\Template\" & teplateName, newFullFileName, True)
        Catch ex As System.IO.IOException
            Throw New MyManager.ManagerException("Il file " & newFullFileName & " è già in uso. " & vbCrLf & "Chiudere il file e riprovare.")
            Return False
        End Try

        If _objExcel Is Nothing Then
            _objExcel = New Microsoft.Office.Interop.Excel.Application
            _objExcel.Visible = False
        End If

        Try
            _workBooks = _objExcel.Workbooks.Open(newFullFileName)
        Catch ex As Exception
            Dim messaggio As String

            messaggio = ex.GetType.Name
            messaggio &= vbCrLf & ex.Message


            System.Windows.Forms.MessageBox.Show(messaggio, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)

            'Se Excell mi ha generato un errore .. riprovo  		
            ' If ex.GetType().Name = "COMException" Then
            _closeExcel()
            _openExcel()
            _workBooks = _objExcel.Workbooks.Open(newFullFileName)
            ' End If
        End Try
        Return newFullFileName
    End Function


    Public Sub _hideColumn(ByVal columnIndex As Integer)
        _hideColumn(Me._workSheet, columnIndex)
    End Sub


    Public Sub _hideColumn(ByRef xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet, ByVal columnIndex As Integer)
        _cell = xlWorkSheet.Cells(1, columnIndex)
        _cell.EntireColumn.Hidden = True
    End Sub


    Public Sub _freezeRow(ByVal rowIndex As Int16)

        _objExcel.ActiveWindow.SplitRow = rowIndex
        _objExcel.ActiveWindow.FreezePanes = True

        'With ActiveWindow

        '    .SplitColumn = 0
        '    .SplitRow = 1
        'End With
    End Sub



    Public Sub _autoFitColumn(ByRef xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet, Optional ByVal rowIndex As Integer = 1)
        Dim i As Integer = 1

        _cell = xlWorkSheet.Cells(rowIndex, i)
        While Not String.IsNullOrEmpty(_cell.Value)
            _cell.EntireColumn.AutoFit()
            i = i + 1
            _cell = xlWorkSheet.Cells(rowIndex, i)
        End While
    End Sub


    Public Sub _autoFitColumn(ByRef xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet, ByVal startRowIndex As Integer, ByVal endColumnIndex As Integer)
        Dim cell As Microsoft.Office.Interop.Excel.Range

        For i As Integer = 1 To endColumnIndex
            cell = xlWorkSheet.Cells(startRowIndex, i)
            cell.EntireColumn.AutoFit()
        Next
    End Sub


    Public Function _formatToShortDate(ByVal myDate As Object) As String
        If myDate Is Nothing OrElse IsDBNull(myDate) Then
            Return ""
        End If

        Dim temp As Date
        temp = myDate

        Return temp.ToShortDateString()
    End Function


    Public Function _fill(ByVal dt As DataTable, ByVal myFileExcel As IO.FileInfo, Optional ByVal includeColumnsHeader As Boolean = True, Optional ByVal startRowIndex As Int16 = 0) As Integer
        _objExcel.Visible = False
        Dim result As Integer
        result = _fill(dt, includeColumnsHeader, startRowIndex)
        _workBooks.SaveAs(myFileExcel.FullName)
        Return result
    End Function



    Public Function _fill(ByVal dt As DataTable, Optional ByVal includeColumnsHeader As Boolean = True, Optional ByVal startRowIndex As Integer = 1) As Long
        'restituisce il numero dell'ultima riga caricata

        _objExcel.Visible = False
        'create a new file Excel 

        'attenzione! excel 2003 ha un limite di 65.536 righe per foglio
        'http://office.microsoft.com/en-us/excel/HP051992911033.aspx

        If _workSheet Is Nothing Then
            _workBooks = _objExcel.Workbooks.Add()
            'apro il foglio 1
            _workSheet = _objExcel.Worksheets(1)
        End If

        If Not _progressBar Is Nothing Then
            _progressBar.Maximum = dt.Rows.Count
            _progressBar.Value = 0
            _progressBar.Step = 1
        End If

        Dim rowIndex As Long = startRowIndex
        Dim columnIndex As Long = 0

        Dim count As Long = 1

        Dim col As DataColumn
        If includeColumnsHeader Then
            For Each col In dt.Columns
                columnIndex = columnIndex + 1
                _workSheet.Cells(rowIndex, columnIndex) = col.ColumnName.Replace("__EURO", "").Replace("__euro", "")
            Next
            rowIndex = rowIndex + 1
        End If

        For Each row As DataRow In dt.Rows

            columnIndex = 0
            For Each col In dt.Columns
                columnIndex = columnIndex + 1
                Select Case col.DataType.FullName

                    'Case "System.Decimal"
                    '    Dim cDecim As Decimal = Decimal.Zero
                    '    If Not IsDBNull(row(col.ColumnName)) Then cDecim = CDec(row(col.ColumnName))
                    '    _workSheet.Cells(rowIndex, columnIndex) = cDecim '.ToString("N2")
                    '    CType(_workSheet.Cells(rowIndex, columnIndex), Microsoft.Office.Interop.Excel.Range).NumberFormat = "€ #.##0,00;-€ #.##0,00"

                    '  Case "System.Int32"
                    '     _workSheet.Cells(rowIndex, columnIndex) = row(col.ColumnName).ToString
                    Case "System.DateTime"
                        _workSheet.Cells(rowIndex, columnIndex) = row(col.ColumnName).ToString
                        CType(_workSheet.Cells(rowIndex, columnIndex), Microsoft.Office.Interop.Excel.Range).NumberFormat = "gg/mm/aaaa"
                    Case Else 'System.String
                        If col.ColumnName.ToUpper.EndsWith("__EURO") Then
                            CType(_workSheet.Cells(rowIndex, columnIndex), Microsoft.Office.Interop.Excel.Range).NumberFormat = "€ #.##0,00;-€ #.##0,00"
                        End If

                        _workSheet.Cells(rowIndex, columnIndex) = row(col.ColumnName).ToString
                End Select

            Next

            If Not _progressBar Is Nothing Then
                _progressBar.PerformStep()
            End If

            If Not _statusBar Is Nothing Then
                _statusBarUpdate(String.Format("Excel: record {0:N0}/{1:N0}", count, dt.Rows.Count))
                System.Windows.Forms.Application.DoEvents()
            End If

            count = count + 1
            rowIndex = rowIndex + 1
        Next

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If

        If Not _statusBar Is Nothing Then
            _statusBarUpdate("")
        End If

        Return rowIndex
    End Function

    Public Sub _insertBorders(ByVal range As Microsoft.Office.Interop.Excel.Range)
        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With

        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With

        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With

        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With

        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With

        With range.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal)
            .LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            .ColorIndex = 0
            .TintAndShade = 0
            .Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
        End With
    End Sub


    Public Sub _copyRow(ByVal sourceRowIndex As Integer, ByVal destinationRowIndex As Integer)
        Dim range As Microsoft.Office.Interop.Excel.Range

        range = _workSheet.Cells(sourceRowIndex, "A")
        range.EntireRow.Copy()
        _workSheet.Range("A" & destinationRowIndex).PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, False, False)
    End Sub

    Public Sub _copyRowBeforeInsert(ByVal sourceRowIndex As Integer, ByVal destinationRowIndex As Integer)
        'add new row 
        CType(_workSheet.Cells(destinationRowIndex, "A"), Microsoft.Office.Interop.Excel.Range).EntireRow.Insert()

        _copyRow(sourceRowIndex, destinationRowIndex)
    End Sub


    Public Sub _deleteRow(ByVal rowIndex As Integer)
        CType(_workSheet.Cells(rowIndex, "A"), Microsoft.Office.Interop.Excel.Range).EntireRow.Delete(False)
    End Sub

#End If


End Class
