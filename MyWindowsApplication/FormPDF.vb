Public Class FormPDF

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim pdf As New PdfDocument()

        Dim myMemoryStream As System.IO.MemoryStream
        myMemoryStream = pdf.makePDF()
        pdf._saveToFile(New IO.FileInfo("C:\temp\myPdf.pdf"))



        Dim valoriToPDF As New Hashtable()

        valoriToPDF.Add("numeroRate", "Ciao emiliano ")

        pdf._updatePdfField(New IO.FileInfo("c:\template_preventivo.pdf"), valoriToPDF)
        pdf._saveToFile(New IO.FileInfo("C:\temp\myPdf.pdf"))
        
        Me.WebBrowser1.Navigate("C:\temp\myPdf.pdf")
   


       

    End Sub
End Class