Public Class PdfDocument
    Inherits MyManager.iTextDocumentBase


    Public Sub New()
        MyBase.New()
    End Sub


    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub




    Public Function makePDF() As System.IO.MemoryStream
        Dim temp As String = ""
        Dim conta As Integer = 1
        Try

            My.Resources.logo.Save(_tempMemoryStream, System.Drawing.Imaging.ImageFormat.Bmp)
            _image = iTextSharp.text.Image.GetInstance(_tempMemoryStream.GetBuffer)
            _image.Alignment = iTextSharp.text.Element.ALIGN_CENTER
            _document.Add(_image)

            temp = "Titolo Report"
            Dim titolo As New iTextSharp.text.Paragraph(temp, _fontTitolo0)
            titolo.Alignment = iTextSharp.text.Element.ALIGN_CENTER
            _document.Add(titolo)

            'CAPITOLO N: tabella delle baseline
            _capitolo = New iTextSharp.text.Chapter(New iTextSharp.text.Paragraph("Summary by Baseline", _fontTitolo1), conta + 1)
            ' addLineaDa2()
            '_sqlQuery = "SELECT USER_ID, MY_LOGIN, NOME, COGNOME  FROM UTENTI "
            ''_table = getPdfTable(_sqlQuery)
            'Dim param As New Hashtable
            'param.Add("Ragione Sociale", "Techub")
            'param.Add("Provincia", "Roma")
            '_table = getPdfFromHasTable(param)
            '_capitolo.Add(_table)
            _capitolo.Add(getDidascalia("Didascalia"))
            _document.Add(_capitolo)



        Finally
            If _document.IsOpen Then
                _document.Close()
            End If
        End Try
        Return _resultMemoryStream
    End Function
End Class
