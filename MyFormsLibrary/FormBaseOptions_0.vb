Imports System.Xml
Imports System.Configuration
Imports System.Text

Public Class FormBaseOptions_0

  
    Private _documentXML As XmlDocument
    Private _filePath As String


    Public Sub _init()

        _filePath = Application.StartupPath & "/" & My.Application.Info.AssemblyName & ".exe.config"
        _documentXML = New System.Xml.XmlDocument
        _documentXML.Load(_filePath)

        Dim ds As DataSet = _CreaDsOpzioni()
        dtgOptions.DataSource = ds.Tables(0)

    End Sub

#Region " Finestra Opzioni "

    Private Function _CreaDsOpzioni() As DataSet

        Dim writer As New System.Xml.XmlTextWriter("registro.xml", Encoding.UTF8)
        writer.WriteStartDocument(True)
        writer.Indentation = 2
        writer.WriteStartElement("Table")

        ' converto la lista in XML
        'For Each elm In _lstRegistry
        '    createNode(elm.Key, elm.Value, writer)
        'Next

        ' leggo i valori del file di config e li converto in una lista XML, poi carico in griglia
        Dim nodeListKey As XmlNodeList = _documentXML.SelectNodes("/configuration/userSettings/" & My.Application.Info.AssemblyName & ".My.MySettings/setting")
          Dim key As String
        Dim val As String

        For Each node As Xml.XmlNode In nodeListKey

            key = node.Attributes("name").Value
            If key.StartsWith("_") Then
                key = key.Remove(0, 1)
                If node.FirstChild.FirstChild Is Nothing Then
                    val = ""
                Else
                    val = node.FirstChild.FirstChild.Value
                End If

                createNode(key, val, writer)
            End If

        Next

        writer.WriteEndElement()
        writer.WriteEndDocument()
        writer.Close()

        Dim xmlFile As System.Xml.XmlReader
        xmlFile = System.Xml.XmlReader.Create("registro.xml", New XmlReaderSettings())

        ' converto il file XML in un dataset
        Dim ds As New DataSet
        ds.ReadXml(xmlFile)

        xmlFile.Close()

        Return ds
    End Function

    Private Sub createNode(ByVal pKeyName As String, ByVal pKeyVal As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement("row")
        writer.WriteStartElement("Key")
        writer.WriteString(pKeyName)
        writer.WriteEndElement()
        writer.WriteStartElement("Value")
        writer.WriteString(pKeyVal)
        writer.WriteEndElement()
        writer.WriteEndElement()
    End Sub

   

#End Region


    Private Sub btnAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim myNode As XmlNode
        Dim pKeyName As String
        Dim pValue As String

        For Each rw As DataGridViewRow In dtgOptions.Rows
            '  SaveRegistryKey("_" + rw.Cells("Key").Value.ToString(), rw.Cells("Value").Value.ToString())
            pKeyName = "_" + rw.Cells("Key").Value.ToString()
            myNode = _documentXML.SelectSingleNode("/configuration/userSettings/" & My.Application.Info.AssemblyName & ".My.MySettings/setting[@name='" & pKeyName & "']/value")

            pValue = rw.Cells("Value").Value.ToString().Trim
            myNode.InnerText = pValue

        Next
        _documentXML.Save(_filePath)

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class