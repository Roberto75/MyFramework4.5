Imports System.ComponentModel
Public Class FormAbout
    Private _link As String
    Private _releaseNote As Boolean
    Private _dataDirectory As String

   



    Public Function _init(ByVal logo As System.Drawing.Image _
                          , ByVal link As String _
                         , ByVal folderData As String) As Boolean



        Me.lblProduct.Text = My.Application.Info.ProductName
        Me.lblVersion.Text = My.Application.Info.Version.ToString()

        Me._link = link

        Me.LinkLabel1.Text = link

        If Not Me.Owner Is Nothing Then
            Me.Icon = Owner.Icon
        End If

        If logo Is Nothing Then
            Me.PictureBox1.Visible = False
        Else
            Me.PictureBox1.Image = logo
        End If

        _releaseNote = IO.File.Exists(Application.StartupPath & "\whatsnew.xml")
        If Not _releaseNote Then
            Me.TabControl1.TabPages.Remove(TabPage4)
        End If


        _dataDirectory = folderData
        If String.IsNullOrEmpty(folderData) Then
            Me.labelDataDirectory.Visible = False
            Me.Label3.Visible = False
        Else
            Me.labelDataDirectory.Text = folderData
        End If

        Return True
    End Function


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start(_link)
    End Sub


    Private Sub lblProduct_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblProduct.LinkClicked
        System.Diagnostics.Process.Start(Application.StartupPath)
    End Sub


    Private Sub labelDataDirectory_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles labelDataDirectory.LinkClicked
        System.Diagnostics.Process.Start(_dataDirectory)
    End Sub


    Private Sub TabControl1_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Selecting
        If e.TabPage Is Nothing Then Exit Sub

        Select Case e.TabPage.Text
            Case "Release notes"
                fillReleaseNote()
            Case "DataBase"
                fillConnnections()
            Case "System"
                fillSystemDLL()

        End Select
    End Sub

    Private Sub fillSystemDLL()

        ListView1.Items.Clear()
        Dim fileEntries As String()
        Dim fileName As String
        Dim myFileVersionInfo As FileVersionInfo

        fileEntries = IO.Directory.GetFiles(Application.StartupPath, "*.dll")

        For Each fileName In fileEntries
            myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName)
            Dim stFile As String = myFileVersionInfo.FileName.Replace(Application.StartupPath & "\", "") & ";" & myFileVersionInfo.FileVersion
            Dim lst1 As New ListViewItem(Split(stFile, ";"))
            ListView1.Items.Add(lst1)
        Next


    End Sub


    Private Sub fillReleaseNote()
        'http://tomcat.apache.org/tomcat-6.0-doc/changelog.html

        'Read file IO.File.Exists(Application.StartupPath & "\whatsnew.xml")

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
        html = "<h1>" & My.Application.Info.ProductName & "</h1>" & html
        WebBrowser1.DocumentText = html

    End Sub

    Public Function getReleaseNote() As String
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
        html = html
        Return html

    End Function


    Private Sub fillConnnections()
        'carico la listbox di sinistra, ed al click carico la propertygrid di destra
        ListBox1.Items.Clear()

        Dim tmp As New ArrayList
        Dim Conf1 As Configuration.ConnectionStringSettingsCollection = System.Configuration.ConfigurationManager.ConnectionStrings

        ListBox1.DisplayMember = "name"

        For Each e1 As System.Configuration.ConnectionStringSettings In Conf1
            Dim stCN As String = e1.ToString
            'Dim Confx As Configuration.ConnectionStringSettings = e1
            'add oggetto ConnectionString solo se non empty
            If Not String.IsNullOrEmpty(Trim(stCN)) Then ListBox1.Items.Add(e1)

        Next


    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim Confx As Configuration.ConnectionStringSettings = CType(ListBox1.SelectedItem, System.Configuration.ConnectionStringSettings)

        Dim pgItemDBConn1 As New pgItemDBConn(Confx.Name, _
                                              Confx.ProviderName, _
                                              Confx.ConnectionString, _
                                              Confx.ElementInformation.Source, _
                                              Confx.LockItem)
        Dim tmp As New ArrayList
        tmp.Add(pgItemDBConn1)
        pg_Dett.SelectedObjects = tmp.ToArray

    End Sub


    Private Sub GetAssembliesVersion()
        listView1.Items.Clear()
        Dim fileEntries As String()
        Dim fileName As String
        Dim myFileVersionInfo As FileVersionInfo
        Dim conta As Int16 = 1

        fileEntries = IO.Directory.GetFiles(Application.StartupPath, "*.dll")
        For Each fileName In fileEntries
            myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName)
            'AddModuleItem(myFileVersionInfo.OriginalFilename.Replace(".dll", ""), myFileVersionInfo.FileVersion, Application.StartupPath)
        Next

        fileEntries = IO.Directory.GetFiles(Application.StartupPath & "\Plugins", "*.dll")
        For Each fileName In fileEntries
            myFileVersionInfo = FileVersionInfo.GetVersionInfo(fileName)
            'AddModuleItem(myFileVersionInfo.OriginalFilename.Replace(".dll", ""), myFileVersionInfo.FileVersion, Application.StartupPath & "\Plugins")
        Next
    End Sub










    Private Class pgItemDBConn
        Private _connectionLock As Boolean
        Private _connectionSource As String
        Private _connectionString As String
        Private _connectionName As String
        Private _provider As String

        Public Sub New()

        End Sub
        Public Sub New(ByVal ConnectionNamein As String, _
                       ByVal Providerin As String, _
                        ByVal ConnectionStringIN As String, _
                        ByVal ConnectionSourceIN As String, _
                        ByVal ConnectionLockIN As Boolean)


            ConnectionName = ConnectionNamein
            Provider = Providerin
            ConnectionString = ConnectionStringIN
            ConnectionLock = ConnectionLockIN
            ConnectionSource = ConnectionSourceIN


        End Sub

        <Description("Indicatore dello stato della connessione al database corrente"), Category("Connessione"), ReadOnlyAttribute(True)> _
                Public Property ConnectionLock() As Boolean
            Get
                Return _connectionLock
            End Get
            Set(ByVal Value As Boolean)
                _connectionLock = Value
            End Set
        End Property
        <Description("Origine della connessione al database corrente"), Category("Connessione"), ReadOnlyAttribute(True)> _
                Public Property ConnectionSource() As String
            Get
                Return _connectionSource
            End Get
            Set(ByVal Value As String)
                _connectionSource = Value
            End Set
        End Property
        <Description("Stringa di connessione al database corrente"), Category("Connessione"), ReadOnlyAttribute(True)> _
                Public Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
            Set(ByVal Value As String)
                _connectionString = Value
            End Set
        End Property
        <Description("Nome della connessione al database corrente"), Category("Connessione"), ReadOnlyAttribute(True)> _
        Public Property ConnectionName() As String
            Get
                Return _connectionName
            End Get
            Set(ByVal Value As String)
                _connectionName = Value
            End Set
        End Property
        <Description("Nome del Provider della connessione al Database corrente"), Category("Connessione"), ReadOnlyAttribute(True)> _
        Public Property Provider() As String
            Get
                Return _provider
            End Get
            Set(ByVal Value As String)
                _provider = Value
            End Set
        End Property
    End Class








  
End Class