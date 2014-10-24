'*** Coratteristiche BASE di un documento PDF
'*** Ad esempio si definisce l'HEADER e il FOOTER
Public Class iTextDocumentBase
    Inherits MyManager.Manager

    Protected _fontTitoloReport As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 38, 1, iTextSharp.text.BaseColor.BLACK)
    Protected _fontTitolo0 As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 24, 1, iTextSharp.text.BaseColor.BLACK)
    Protected _fontTitolo1 As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, 1, iTextSharp.text.BaseColor.BLACK)
    Protected _fontTitolo2 As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 14, 1, iTextSharp.text.BaseColor.BLACK)
    Protected _fontNormale As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1, iTextSharp.text.BaseColor.BLACK)


  
    'Friend _chart As dotnetCHARTING.Chart
    Protected _document As iTextSharp.text.Document
    Protected _image As iTextSharp.text.Image
    Protected _capitolo As iTextSharp.text.Chapter
    Protected _paragrafo As iTextSharp.text.Paragraph
    Protected _section As iTextSharp.text.Section
    Protected _phrase As iTextSharp.text.Phrase
    Protected _table As iTextSharp.text.pdf.PdfPTable

    Protected _risultatoMemoryStream As New System.IO.MemoryStream


    Public Sub New()
        MyBase.New()
        initPage()
        initHeader()
        initFooter()
    End Sub


    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
        '   _ChartManager = New DotNetChartingManager.DotNetChartingManager(Me._connection.ConnectionString)
        initPage()
        initHeader()
        initFooter()
        ' initDotNetCharting()
    End Sub


    Friend Function crateNewInstanceOfDotNetCharting() As Boolean
        'Me._chart.Dispose()
        'Me._chart = Nothing
        'Return initDotNetCharting()
    End Function

    'Friend Function initDotNetCharting() As Boolean
    '    ''impostazioni di DEFAULT per i grafici

    '    '_chart = New dotnetCHARTING.Chart

    '    '' _chart.DefaultSeries.ConnectionString = Me._connection.ConnectionString
    '    '_chart.ImageFormat = dotnetCHARTING.ImageFormat.Jpg
    '    ''Enable streaming
    '    '_chart.UseFile = False
    '    '_chart.Debug = True
    '    ''Change the shading mode
    '    '_chart.ShadingEffect = True

    '    Return True

    'End Function

    Friend Function initPage() As Boolean
        'Page SIZE A4
        _document = New iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 80, 50, 30, 65)
        '_document.AddAuthor("Author")
        '_document.

        'Generazione PDF in Memoria
        'Attenzione: in questo modo utilizzo un memory stream per generare il PDF
        Dim writer As iTextSharp.text.pdf.PdfWriter
        writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, _risultatoMemoryStream)

        '***************************************
        '    http://www.eggheadcafe.com/community/aspnet/2/10197628/problem-getting-page-header-in-pdf-file-using-itextsharp-version-502.aspx
        '**************************************************


        ' Dim myEvent As New iTextSharp.text.pdf.


        ' writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, New System.IO.FileStream(My.Request.MapPath("~\public\" & _fileName), IO.FileMode.Create))
        'writer.SetPdfVersion(New iTextSharp.text.pdf.PdfName(iTextSharp.text.pdf.PdfWriter.VERSION_1_7))
        _document.Open()

    End Function

    Friend Function initHeader() As Boolean
        Dim fontHeader As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE)

        ' _image = iTextSharp.text.Image.GetInstance(My.Request.MapPath("~\images\Pdf\logoTop.jpg"))
        '_image.ScalePercent(20 / _image.Height * 100)

        '  _chunk = New iTextSharp.text.Phrase(_image, 0.0F, 0.0F)

        ' Dim header As New iTextSharp.text.Heade



        '        '
        '.(New iTextSharp.text.Phrase(_chunk), False)

        '        header.BorderWidthTop = 0
        '        header.BorderWidthBottom = 0.5F
        '        _document. .Header = header

        Return True

    End Function

    Friend Function initFooter() As Boolean
        Dim fontFooter As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)

        'Dim footer As New iTextSharp.text.HeaderFooter(New iTextSharp.text.Phrase("Page ", fontFooter), New iTextSharp.text.Phrase(".", fontFooter))
        'footer.Alignment = iTextSharp.text.HeaderFooter.ALIGN_RIGHT

        'footer.BorderWidth = 1

        '_document.Footer = footer
        Return True
    End Function



    Public Function getPdfTable(ByVal dataTable As System.Data.DataTable, Optional ByVal showHeader As Boolean = True) As iTextSharp.text.pdf.PdfPTable
        Dim table As iTextSharp.text.pdf.PdfPTable = Nothing
        Dim cell As iTextSharp.text.pdf.PdfPCell = Nothing
        Dim fontCell As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7, 0, iTextSharp.text.BaseColor.BLACK)
        Dim fontHeader As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 0, iTextSharp.text.BaseColor.WHITE)
        Dim chunk As iTextSharp.text.Phrase


        Dim numeroDiColonne As Integer = dataTable.Columns.Count
        Dim numeroDiRighe As Integer = dataTable.Rows.Count

        'table = New iTextSharp.text.Table(numeroDiColonne, numeroDiRighe)
        table = New iTextSharp.text.pdf.PdfPTable(numeroDiColonne)
        'table.DefaultVerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
        'table.DefaultHorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
        'table.Width = 90
        table.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
        table.WidthPercentage = 90
        table.SpacingBefore = 20

        'table.LockedWidth = True



        'in percentuale
        'Dim wi(3) As Integer
        'wi(0) = 45
        ' wi(1) = 15
        ' wi(2) = 15
        ' wi(3) = 15

        'table.SetWidths(wi)
        'table.Padding = 2.0F

        Dim dataRow As System.Data.DataRow
        Dim i, j As Integer

        'Header
        If showHeader Then
            For i = 0 To numeroDiColonne - 1
                '   _chunk = New iTextSharp.text.Chunk(dataTable.Columns(i).ToString, fontHeader)
                _phrase = New iTextSharp.text.Phrase(dataTable.Columns(i).ToString, fontHeader)
                cell = New iTextSharp.text.pdf.PdfPCell(_phrase)
                cell.BackgroundColor = iTextSharp.text.BaseColor.BLACK
                cell.NoWrap = True
                'cell.Leading = 8
                'table.AddCell(cell, 0, i)
                table.AddCell(cell)

            Next
        End If

        For j = 0 To numeroDiRighe - 1
            dataRow = dataTable.Rows(j)
            For i = 0 To numeroDiColonne - 1

                'If isPercentuale AndAlso i > 0 Then
                '    If IsDBNull(dataRow(i).ToString) Then
                '        valorePercentuale = Double.Parse("0")
                '    Else
                '        valorePercentuale = Double.Parse(dataRow(i).ToString)
                '    End If

                '    If valorePercentuale = 0 Then
                '        chunk = New iTextSharp.text.Chunk("0%", fontCell)
                '    Else
                '        chunk = New iTextSharp.text.Chunk(String.Format("{0:#.##}%", valorePercentuale), fontCell)
                '    End If



                'Else
                '    chunk = New iTextSharp.text.Chunk(dataRow(i).ToString, fontCell)
                'End If

                chunk = New iTextSharp.text.Phrase(dataRow(i).ToString, fontCell)

                cell = New iTextSharp.text.pdf.PdfPCell(chunk)

                If i = 0 Then
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT
                Else
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                End If

                If (j Mod 2) = 1 Then
                    cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY
                End If

                '                cell.Leading = 8

                '               table.AddCell(cell, j + 1, i)
                table.AddCell(cell)
            Next
        Next

        Return table
    End Function


    Public Function getPdfTable(ByVal strSQL As String, Optional ByVal isPercentuale As Boolean = True) As iTextSharp.text.pdf.PdfPTable
        Dim dataTable As System.Data.DataTable
        dataTable = Me._fillDataSet(strSQL).Tables(0)

        Return getPdfTable(dataTable)
    End Function


    Public Function getPdfTable(ByVal param As Hashtable, Optional ByVal showHeader As Boolean = True) As iTextSharp.text.pdf.PdfPTable
        'trasformo il mio Hashtable in un DataTable
        Return getPdfTable(_fillDataTable(param), showHeader)
    End Function


    Public Function _saveToFile(ByVal outPutFile As IO.FileInfo) As Boolean
        Dim fs As New IO.FileStream(outPutFile.FullName, IO.FileMode.Create, IO.FileAccess.Write)
        Dim s As New IO.BinaryWriter(fs)
        Try
            s.Write(_risultatoMemoryStream.GetBuffer)
        Finally
            s.Close()
            fs.Close()
        End Try
        Return True
    End Function

    'Friend Sub addLineaDa2()
    '    _image = iTextSharp.text.Image.GetInstance(My.Request.MapPath("~\images\lineaCharter.jpg"))
    '    _image.Alignment = iTextSharp.text.Element.ALIGN_LEFT
    '    _capitolo.Add(_image)

    'End Sub

    Friend Function getPageInnerWidth(ByVal myDocument As iTextSharp.text.Document) As Single
        Return myDocument.PageSize.Width - myDocument.LeftMargin - myDocument.RightMargin
    End Function

    Friend Function getPageInnerHeight(ByVal myDocument As iTextSharp.text.Document) As Single
        Return myDocument.PageSize.Height - myDocument.TopMargin - myDocument.BottomMargin
    End Function

    Public Function getDidascalia(ByVal testo As String) As iTextSharp.text.pdf.PdfPTable
        Dim fontCell As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0, iTextSharp.text.BaseColor.BLACK)

        Dim table As iTextSharp.text.pdf.PdfPTable
        table = New iTextSharp.text.pdf.PdfPTable(1)
        'table.Alignment = iTextSharp.text.Table.ALIGN_CENTER
        'table.BorderWidth = 0.5
        'table.BorderColor = iTextSharp.text.BaseColor.DARK_GRAY
        'table.Padding = 2.0F
        'table.Width = 90
        table.TotalWidth = 90

        _phrase = New iTextSharp.text.Phrase(testo, fontCell)
        Dim cell As New iTextSharp.text.pdf.PdfPCell(_phrase)
        'cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE

        table.AddCell(cell)

        Return table

    End Function

    Friend Function rotateRigheToColonne(ByVal dataTable As System.Data.DataTable) As System.Data.DataTable
        Dim i, j As Integer
        'Dim ds As New Data.DataSet
        Dim row As Data.DataRow
        Dim table As New Data.DataTable
        'tutte i valori (LABEL) delle righe diventano le mie colonne
        'mi scorro tutte le righe leggendo la prima colonna
        table.Columns.Add("LABEL")
        For i = 0 To dataTable.Rows.Count - 1
            table.Columns.Add(CStr(IIf(IsDBNull(dataTable.Rows(i)(0)), "", dataTable.Rows(i)(0).ToString)))
        Next

        For i = 1 To dataTable.Columns.Count - 1
            'mi scorro tutta la colonna...
            row = table.NewRow
            row(0) = dataTable.Columns(i).ColumnName
            For j = 0 To dataTable.Rows.Count - 1
                row(j + 1) = dataTable.Rows(j)(i)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function



    Public Function _updatePdfField(ByVal inputFile As IO.FileInfo, ByVal fieldValue As Hashtable) As Boolean
        Dim reader As New iTextSharp.text.pdf.PdfReader(inputFile.FullName)
        Return m_updatePDFField(reader, fieldValue)
    End Function

    Public Function _updatePdfField(ByVal inputFile As Byte(), ByVal fieldValue As Hashtable) As Boolean
        Dim reader As New iTextSharp.text.pdf.PdfReader(inputFile)
        Return m_updatePDFField(reader, fieldValue)
    End Function


    Public Function _updatePdfField(ByVal inputFile As IO.MemoryStream, ByVal fieldValue As Hashtable) As Boolean
        Dim reader As New iTextSharp.text.pdf.PdfReader(inputFile)
        Return m_updatePDFField(reader, fieldValue)
    End Function



    Private Function m_updatePDFField(ByVal reader As iTextSharp.text.pdf.PdfReader, ByVal fieldValue As Hashtable) As Boolean
        _risultatoMemoryStream.Close()
        _risultatoMemoryStream.Dispose()
        _risultatoMemoryStream = New IO.MemoryStream()
        Dim stamper As New iTextSharp.text.pdf.PdfStamper(reader, _risultatoMemoryStream)

        'Dim pdfField As iTextSharp.text.pdf.AcroFields.Item


        If stamper.AcroFields.Fields.Count <> fieldValue.Count Then
            Throw New MyManager.ManagerException("PDF update failed: number of fields is different")
        End If

        Dim myKey As String
        'Dim myValue As String


        'For Each myItem As Generic.KeyValuePair(Of String, iTextSharp.text.pdf.AcroFields.Item) In stamper.AcroFields.Fields
        ' Console.WriteLine(myItem.Key)
        'Next

        For Each myKey In fieldValue.Keys
            stamper.AcroFields.SetField(myKey, fieldValue(myKey))
        Next

        'This task is accomplished by setting the FormFlattening value to true or false.
        ' If the value is set to false, the resulting PDF will be available for edits, if the value is set to true, the PDF will be locked against further edits.
        stamper.FormFlattening = True


        stamper.Close()

        Return True


    End Function



End Class
