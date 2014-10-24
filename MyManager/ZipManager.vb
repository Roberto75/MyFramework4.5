'
' Created by SharpDevelop.
' User: Roberto
' Date: 29/06/2009
' Time: 16.50
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class ZipManager
	
	
	'*** http://dotnetzip.codeplex.com/ ***


    Public Shared Function zipToFile(ByVal pathFileSource As String) As String
        'al fil di destinazione ci aggiungo l'estenzione .ZIP
        Return zipToFile(pathFileSource, pathFileSource & ".zip")
    End Function


    Public Shared Function zipToFile(ByVal pathFileSource As String, ByVal pathFileDestination As String) As String
        Dim fileZip As New Ionic.Zip.ZipFile()
        fileZip.AddFile(pathFileSource)

        fileZip.Save(pathFileDestination)

        Return pathFileDestination
    End Function
	

    Public Shared Function zipToMemory(ByVal file As IO.FileInfo) As System.IO.MemoryStream
        Dim myMemoryStream As New System.IO.MemoryStream()
        Dim fileZip As New Ionic.Zip.ZipFile()

        fileZip.AddFile(file.FullName, "")
        fileZip.Save(myMemoryStream)

        ' move the stream position to the beginning
        myMemoryStream.Seek(0, IO.SeekOrigin.Begin)
        Return myMemoryStream
    End Function


	Public Shared Function zipToMemory(ByVal fileOrDirectoty As String) As System.IO.MemoryStream
		Dim myMemoryStream As New System.IO.MemoryStream()
		Dim fileZip As New Ionic.Zip.ZipFile()
		
        fileZip.AddItem(fileOrDirectoty)
		fileZip.Save(myMemoryStream)
		
		return myMemoryStream
	End Function
	
	
	
	Public Shared Sub zipFolder( zFile As String, sourceDir As String)
		If Not IO.Directory.Exists(sourceDir) Then
			Throw New MyManager.ManagerException("Directory sorgente non valida")
			exit sub
		End If
		
		Dim fileZip As New Ionic.Zip.ZipFile(zFile)
		fileZip.CompressionLevel = 9
		
		fileZip.AddDirectory (sourceDir, "" )
		fileZip.Save()
	End Sub
	
	
	Public Shared Sub extractAllZip(input As IO.MemoryStream , targetDir As String)
		If Not IO.Directory.Exists(targetDir ) Then
			IO.Directory.CreateDirectory(targetDir)
		End If
		
		Dim fileZip As  Ionic.Zip.ZipFile
		fileZip = Ionic.Zip.ZipFile.Read(input)
		
		fileZip.ExtractAll (targetDir, True)
	End Sub
	
	
	Public Shared Sub extractAllZip(zFile As String , targetDir As String)
		If Not IO.Directory.Exists(targetDir ) Then
			IO.Directory.CreateDirectory(targetDir)
		End If
		
		Dim fileZip As New Ionic.Zip.ZipFile(zFile)
		fileZip.ExtractAll (targetDir, True)
	End Sub
	
	
	Public Shared Function  extractZipEntry(input As IO.MemoryStream, entry As String ,  targetDir As String) As String
		'restituisce il path del file estratto
		
		If targetDir.EndsWith ("\") Then
			If Not IO.Directory.Exists(targetDir ) Then
				IO.Directory.CreateDirectory(targetDir)
			End If
		Else
			Throw New MyManager.ManagerException("ZipManager unzip: directory di destinazione non corretta: " & targetDir)
			Return ""
		End If
		
		'lettura da memoria
		Dim zFile As Ionic.Zip.ZipFile
		zFile = Ionic.Zip.ZipFile.Read(input)
	
		If Not zFile.EntryFileNames.Contains (entry) Then
			Throw New MyManager.ManagerException("ZipManager unzip entry file not found: " & entry)
		End If
	
		For Each z As Ionic.Zip.ZipEntry In zFile
			If z.FileName = entry Then
				z.Extract( targetDir,True)
				Return targetDir & entry
			End If
		Next
		
		Return ""
	End Function
	
	
	Public Shared Function  extractZipEntryIntoMemory(input As IO.MemoryStream, entry As String ) As IO.MemoryStream 
		'lettura da memoria
		Dim zFile As Ionic.Zip.ZipFile
		zFile = Ionic.Zip.ZipFile.Read(input)
	
		If Not zFile.EntryFileNames.Contains (entry) Then
			Throw New Exception("ZipManager unzip entry file not found: " & entry)
		End If
	
		Dim result As New IO.MemoryStream 
		
		For Each z As Ionic.Zip.ZipEntry In zFile
			If z.FileName = entry Then
				z.Extract(result)
				Return result 
			End If
		Next
		
		Return Nothing
	End Function
	
	
End Class
