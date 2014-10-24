Public Class FormClickOnce
    Inherits FormBaseDetail_3

    Public Enum Framework
        Framework35
        Framework40
    End Enum

    Protected _sourceFolder As String
    Protected _deployFolder As String
    Protected _applicationName As String
    Protected _applicationNameTrimmed As String
    Protected _applicationEXE As String
    Protected _pathCertificato As String
    Protected _passwordCertificato As String
    Protected _cmdMage As String
    Protected _deployVersion As String
    Public _serverToDeploy As List(Of MyControlsLibrary.CmbItem)
    Public _pathOnServerToDeploy As String
    Protected _urlHTTP As String

    Public Overloads Function _init(ByRef statusBar As Windows.Forms.ToolStripStatusLabel, _
                                    ByVal applicationName As String, _
                                    ByVal applicationEXE As String, _
                                    ByVal pathCertificatoPFX As String, _
                                    ByVal password As String, _
                                    ByVal urlHTTP As String, _
                                    ByVal remotePath As String, _
                                    myFramework As Framework) As Boolean

        MyBase._init(Nothing, statusBar, Nothing)

        Dim d As IO.DirectoryInfo
        Me._applicationName = applicationName
        Me._applicationEXE = applicationEXE
        Me._applicationNameTrimmed = applicationName.Replace(" ", "")
        Me._urlHTTP = urlHTTP
        Me._pathOnServerToDeploy = remotePath

        _sourceFolder = Application.StartupPath & "\src_" & _applicationNameTrimmed & "\"
        lblSourceFolder.Text = _sourceFolder
        d = New IO.DirectoryInfo(_sourceFolder)
        If Not d.Exists Then
            System.Windows.Forms.MessageBox.Show("La cartella sorgente è inesistente " & vbCrLf & _sourceFolder, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        _deployFolder = Application.StartupPath & "\deploy_" & _applicationNameTrimmed & "\"
        d = New IO.DirectoryInfo(_deployFolder)
        'cancello il contenuto della vecchia cartella!
        If d.Exists Then
            d.Delete(True)
        End If
        d.Create()

        _pathCertificato = pathCertificatoPFX
        _passwordCertificato = password

        If Not IO.File.Exists(_pathCertificato) Then
            System.Windows.Forms.MessageBox.Show("Il path del certifcato non è corretto:" & vbCrLf & _pathCertificato, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If


        '_cmdMage = """" & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft SDKs\Windows\v6.1\Bin\mage"" "

        '_cmdMage = """" & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft SDKs\Windows\v7.0\Bin\mage"" "

        '_cmdMage = """" & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\mage"" "

        Select Case myFramework
            Case Framework.Framework35
                _cmdMage = Application.StartupPath & "\Mage\3.5\mage.exe"
            Case Framework.Framework40
                _cmdMage = Application.StartupPath & "\Mage\4.0\mage.exe"
            Case Else
                System.Windows.Forms.MessageBox.Show("Versione del framework non gestita", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                Return False
        End Select

        If Not IO.File.Exists(_cmdMage) Then
            System.Windows.Forms.MessageBox.Show("Il comando mage.exe non è stato trovato" & _cmdMage, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        _cmdMage = """" & _cmdMage & """"

        If String.IsNullOrEmpty(_applicationEXE) OrElse (Not _applicationEXE.ToUpper.EndsWith(".EXE")) Then
            System.Windows.Forms.MessageBox.Show("Il nome del file .exe dell'applicazione non è corretto:" & _applicationEXE, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        testContentSorceFolderBase()


        Return True
    End Function

    Protected Overridable Function _getServersToDeploy() As Boolean
        Return True
    End Function

    Private Sub MyButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyButton1.Click
        _getServersToDeploy()
        _makeDeploy()
    End Sub


    Protected Overridable Function _makeDeploy() As Boolean
        If _serverToDeploy.Count = 0 Then
            System.Windows.Forms.MessageBox.Show("Selezionare almeno un server su cui eseguire la procedura di aggiornamento", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        If Not _testContentSourceFolder() Then
            System.Windows.Forms.MessageBox.Show("Il contenuto della cartella sorgente non è corretto", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        If Not String.IsNullOrEmpty(txtDeployVersion.Text) Then
            If System.Windows.Forms.MessageBox.Show("Forzare la versione di deploy a: " & Me.txtDeployVersion.Text, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) <> Windows.Forms.DialogResult.OK Then
                txtDeployVersion.Text = ""
                Return False
            End If
            'lblDeployVer.Text = txtDeployVersion.Text
            _deployVersion = txtDeployVersion.Text
        End If


        Dim temp As String
        Dim esito As Boolean = True

        For Each server As MyControlsLibrary.CmbItem In _serverToDeploy
            Me._statusBarUpdate("Aggiornamento del file di configurazione per " & server.Label)
            If _updateFileConfig(server) Then
                Me._statusBarUpdate(String.Format("Aggiornamento del file di configurazione {0}.config avvenuta con successo", Me._applicationEXE))

                temp = deployWithClickOnce(server.Label)
                If String.IsNullOrEmpty(temp) Then
                    System.Windows.Forms.MessageBox.Show("Errore durante il processo deploy", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                    esito = False
                Else
                    Me._statusBarUpdate("Deploy per " & server.Label & " conclusa con successo")

                    If Not String.IsNullOrEmpty(_pathOnServerToDeploy) _
                            AndAlso System.Windows.Forms.MessageBox.Show("Proseguire con la copia di " & server.Label & " sul server " & _urlHTTP, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                        temp = _copyOnServerMicrosoft(server)

                        If String.IsNullOrEmpty(temp) Then
                            Me._statusBarUpdate("ERROR on server updated")
                            System.Windows.Forms.MessageBox.Show("Errore durante la copia della cartella di deploy sul server " & server.Label, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                            esito = False
                        Else
                            'apro la cartella in cui è presente il file aggiornato
                            System.Diagnostics.Process.Start(temp)
                            Me._statusBarUpdate("Server updated")
                        End If
                    End If
                End If
            Else
                System.Windows.Forms.MessageBox.Show(String.Format("Errore aggiornamento file {0}.config", Me._applicationEXE), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
                esito = False
            End If
        Next
        If esito Then
            System.Windows.Forms.MessageBox.Show("Deploy terminato con successo", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        Else
            System.Windows.Forms.MessageBox.Show("Attenzione, deploy terminato con errori", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning)
        End If

        Return esito
    End Function

    Overridable Function _testContentSourceFolder() As Boolean
        Return True
    End Function

    Private Function testContentSorceFolderBase() As Boolean
        Dim d As New IO.DirectoryInfo(_sourceFolder & "CVS")
        If d.Exists Then
            d.Delete(True)
        End If

        If Not IO.File.Exists(_sourceFolder & _applicationEXE) Then
            System.Windows.Forms.MessageBox.Show(String.Format("Il file {0} non è presente nella cartella Source", _applicationEXE), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If

        If Not IO.File.Exists(_sourceFolder & _applicationEXE & ".config") Then
            System.Windows.Forms.MessageBox.Show(String.Format("Il file {0} non è presente nella cartella Source", _applicationEXE & ".config"), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If


        Dim checkFiles As String()
        checkFiles = IO.Directory.GetFiles(_sourceFolder, "Thumbs.db", IO.SearchOption.AllDirectories)

        For Each f As String In checkFiles
            IO.File.Delete(f)
        Next

        Dim myFileVersionInfo As FileVersionInfo
        myFileVersionInfo = FileVersionInfo.GetVersionInfo(_sourceFolder & _applicationEXE)
        Me.lblDeployVer.Text = myFileVersionInfo.FileVersion
        _deployVersion = myFileVersionInfo.FileVersion

        Return _testContentSourceFolder()
    End Function

    Private Function deployWithClickOnce(ByVal key As String) As String
        Dim deployRoot As New IO.DirectoryInfo(_deployFolder)
        'cancello vecchio deploy
        If deployRoot.Exists Then
            deployRoot.Delete(True)
        End If
        deployRoot.Create()

        Me.TextBox1.Text = ""

        Dim label As String
        If String.IsNullOrEmpty(key) Then
            label = _applicationName
        Else
            label = _applicationName & " - " & key
        End If

        Dim deployFolder As String = deployRoot.FullName & "deploy_" & _deployVersion.Replace(".", "_") & "\"
        If Not IO.Directory.Exists(deployFolder) Then
            IO.Directory.CreateDirectory(deployFolder)
        End If


        Dim providerURL As String
        If String.IsNullOrEmpty(key) Then
            providerURL = String.Format("{0}{1}/{1}.application", _urlHTTP, _applicationNameTrimmed)
        Else
            providerURL = String.Format("{0}{2}/{1}/{2}.application", _urlHTTP, key, _applicationNameTrimmed)
        End If


        'ICONA
        If Not IO.File.Exists(_sourceFolder & "app.ico") Then
            System.Windows.Forms.MessageBox.Show(String.Format("Il file app.ico non è presente nella cartella {0}", _sourceFolder), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return ""
        End If
        IO.File.Copy(_sourceFolder & "app.ico", deployFolder & "app.ico")

        'Copio i files sorgenti
        FileSystemManager.copyDirectory(_sourceFolder, deployFolder)

        Dim command As String
        command = _cmdMage & String.Format(" -New Application -ToFile {0}{2}.manifest -Name ""{3}"" -Version {1} -Publisher ""Techub"" -FromDirectory {0} -um true -IconFile ""app.ico"" ", deployFolder, _deployVersion, _applicationEXE, label)
        Me.TextBox1.Text &= command & vbCrLf & vbCrLf

        Shell(command, AppWinStyle.Hide, True)
        System.Threading.Thread.Sleep(1000)
        If Not IO.File.Exists(deployFolder & _applicationEXE & ".manifest") Then
            System.Windows.Forms.MessageBox.Show("Errore durante la creazione del file " & _applicationEXE & ".manifest", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return ""
        End If

        command = _cmdMage & String.Format(" -Sign {0}{3}.manifest -CertFile {1} -Password {2}", deployFolder, _pathCertificato, _passwordCertificato, _applicationEXE)
        Me.TextBox1.Text &= command & vbCrLf & vbCrLf
        Shell(command, AppWinStyle.Hide, True)
        System.Threading.Thread.Sleep(1000)

        command = _cmdMage & String.Format(" -New Deployment -ToFile {0}\" & _applicationNameTrimmed & ".application -Name ""{4}"" -Version {1} -Publisher ""Techub"" -AppManifest {2}" & _applicationEXE & ".manifest -providerUrl {3} -Install true -UseManifestForTrust true ", deployRoot.FullName, _deployVersion, deployFolder, providerURL, label)
        Me.TextBox1.Text &= command & vbCrLf & vbCrLf
        Shell(command, AppWinStyle.Hide, True)
        System.Threading.Thread.Sleep(1000)
        If Not IO.File.Exists(String.Format("{0}\{1}.application", deployRoot.FullName, _applicationNameTrimmed)) Then
            System.Windows.Forms.MessageBox.Show(String.Format("Errore durante la creazione del file " & _applicationNameTrimmed & ".application"), My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return ""
        End If


        'FIX minVersion
        command = _cmdMage & String.Format(" -Update {0}{2}.application -MinVersion {1}", deployRoot.FullName, _deployVersion, _applicationNameTrimmed)
        Me.TextBox1.Text &= command & vbCrLf & vbCrLf
        Shell(command, AppWinStyle.Hide, True)
        System.Threading.Thread.Sleep(1000)

        'FIX UPDATE beforeApplicationStartup
        If (True = True) Then
            Dim fileTXT As String
            fileTXT = IO.File.ReadAllText(String.Format("{0}{1}.application", deployRoot.FullName, _applicationNameTrimmed))
            Dim x, y As Integer
            Dim replaced As String

            x = fileTXT.IndexOf("<subscription>")
            y = fileTXT.IndexOf("</subscription>")
            replaced = fileTXT.Substring(x, (y + 15) - x)
            fileTXT = fileTXT.Replace(replaced, "<subscription><update><beforeApplicationStartup /></update></subscription>")

            IO.File.WriteAllText(String.Format("{0}{1}.application", deployRoot.FullName, _applicationNameTrimmed), fileTXT)
        End If





        ''PREREQUISITI
        'If (True = False) Then
        '    Dim fileTXT As String
        '    fileTXT = IO.File.ReadAllText(String.Format("{0}{1}.application", deployRoot.FullName, _applicationNameTrimmed))
        '    Dim x, y As Integer
        '    Dim newFile As String

        '    x = fileTXT.IndexOf("</deployment>")

        '    newFile = fileTXT.Substring(0, x + 14)

        '    newFile &= "<dependency>" & _
        '        "<dependentAssembly dependencyType=""preRequisite"" allowDelayedBinding=""true"" supportUrl=""http://msdn.microsoft.com/en-us/netframework/aa569263"">" & _
        '        "<assemblyIdentity name=""Microsoft.Windows.CommonLanguageRuntime"" version=""4.0.30319.0""/> " & _
        '        "</dependentAssembly>" & _
        '        "  </dependency>"

        '    newFile &= fileTXT.Substring(x + 15)


        '    IO.File.WriteAllText(String.Format("{0}{1}.application", deployRoot.FullName, _applicationNameTrimmed), newFile)
        'End If




        command = _cmdMage & String.Format(" -Sign {0}\{3}.application -CertFile {1} -Password {2}", deployRoot.FullName, _pathCertificato, _passwordCertificato, _applicationNameTrimmed)
        Me.TextBox1.Text &= command & vbCrLf & vbCrLf
        Shell(command, AppWinStyle.Hide, True)
        System.Threading.Thread.Sleep(1000)

        Return deployRoot.FullName
    End Function

    Overridable Function _updateFileConfig(ByVal item As MyControlsLibrary.CmbItem) As Boolean
        Return True
    End Function

    Overridable Function _copyOnServerMicrosoft(ByVal server As MyControlsLibrary.CmbItem) As String
        If String.IsNullOrEmpty(_pathOnServerToDeploy) Then
            System.Windows.Forms.MessageBox.Show("Specificare il path del server sul cui effettuare il deploy", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return ""
        End If


        Me._statusBarUpdate("Connecting on " & _urlHTTP & " ...")

        If Not IO.Directory.Exists(_pathOnServerToDeploy) Then
            System.Windows.Forms.MessageBox.Show("Errore di connesione verso il server " & server.Label, My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            Return ""
        End If

        Dim destinationPath As String = _pathOnServerToDeploy & _applicationNameTrimmed

        If Not IO.Directory.Exists(destinationPath) Then
            IO.Directory.CreateDirectory(destinationPath)
        End If

        destinationPath = destinationPath & "\" & server.Label

        If Not IO.Directory.Exists(destinationPath) Then
            IO.Directory.CreateDirectory(destinationPath)
        End If

        Dim checkFolder As String
        checkFolder = destinationPath & "\deploy_" & _deployVersion.Replace(".", "_")

        If IO.Directory.Exists(checkFolder) Then
            If System.Windows.Forms.MessageBox.Show("Sul server esite già una deploy con questa versione:" & _deployVersion & vbCrLf & "Sovrascivere i files e proseguire con la copia?", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) <> Windows.Forms.DialogResult.OK Then
                Return ""
            End If
            IO.Directory.Delete(checkFolder, True)
        End If

        Me._statusBarUpdate("Copy deploy on " & _urlHTTP & " ...")

        FileSystemManager.copyDirectory(_deployFolder, destinationPath, True)

        Me._statusBarUpdate("Server " & _urlHTTP & " updated")

        Return destinationPath
    End Function



End Class