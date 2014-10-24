
Imports System.ComponentModel
Public Class FormHome





    Public Overloads Function _init(ByVal logo As System.Drawing.Image _
                                  , ByVal link As String) As Boolean


        Me._hideButtonClose()


        lblNome.Text = My.Application.Info.ProductName
        Label2.Text = My.Application.Info.Version.ToString()

        LinkLabel1.Text = link
        PictureBox1.Image = logo

        Dim risultatoMemoryStream As New System.IO.MemoryStream

        Dim xslt As New System.Xml.Xsl.XslCompiledTransform()
        'xslt.Load(Application.StartupPath & "\whatsnew.xsl")
        Dim reader As Xml.XmlReader = Xml.XmlReader.Create(New System.IO.StringReader(My.Resources.whatsnew))

        xslt.Load(reader)
        '.Create(New System.IO.StreamReader(My.Resources.whatsnew)))
        xslt.Transform(Application.StartupPath & "\whatsnew.xml", Nothing, risultatoMemoryStream)
        risultatoMemoryStream.Flush()


        Dim html As String
        Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
        html = encoding.GetString(risultatoMemoryStream.GetBuffer())
        WebBrowser1.DocumentText = "<font color=MidnightBlue>" & html & "</font>"



    End Function

   
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start(LinkLabel1.Text)
    End Sub
End Class