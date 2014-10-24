Public Class ZipManagerOLD
	
	
	Public Shared Function zipToFile(ByVal pathFileSource As String) As String
		'la funzione restituisce il path del file zippato
		Dim pathFileZippato As String = pathFileSource & ".zip"
		
		Dim fileZip As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(System.IO.File.Create(pathFileZippato))
		
		Dim fileSource As IO.FileStream = IO.File.OpenRead(pathFileSource)
		'buffer
		Dim buffer(CType(fileSource.Length - 1, Integer)) As Byte
		'leggo il file nel buffer
		fileSource.Read(buffer, 0, buffer.Length)
		
		fileSource.Close()
		
		
		
		'creo una entry nel file ZIP
		Dim entry As New ICSharpCode.SharpZipLib.Zip.ZipEntry(fileSource.Name)
		'aggiugno la entry al file ZIP
		fileZip.PutNextEntry(entry)
		'scrivo il file
		fileZip.Write(buffer, 0, buffer.Length)
		
		fileZip.Finish()
		fileZip.Close()
		
		
		
		Return pathFileZippato
	End Function
	
	Public Shared Function zipToMemory(ByVal pathFileSource As String) As System.IO.MemoryStream
		
		'Dim a As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream
		
		
		Dim myMemoryStream As New System.IO.MemoryStream()
		Dim fileZip As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(myMemoryStream)
		'This is required inorder to close the zip archive and leave the stream intact
		fileZip.IsStreamOwner = False
		' 0 - store only to 9 - means best compression
		fileZip.SetLevel(9)
		
		Dim fileSource As IO.FileStream = IO.File.OpenRead(pathFileSource)
		'buffer
		Dim buffer(fileSource.Length - 1) As Byte
		'leggo il file nel buffer
		fileSource.Read(buffer, 0, buffer.Length)
		fileSource.Close()
		
		Dim fileInfo As System.IO.FileInfo
		fileInfo = New System.IO.FileInfo(fileSource.Name)
		
		
		'creo una entry nel file ZIP
		Dim entry As New ICSharpCode.SharpZipLib.Zip.ZipEntry(fileInfo.Name)
		'aggiugno la entry al file ZIP
		fileZip.PutNextEntry(entry)
		'scrivo il file
		fileZip.Write(buffer, 0, buffer.Length)
		
		fileZip.Finish()
		fileZip.Close()
		
		
		
		Return myMemoryStream
	End Function
	
	
	
	
	
	Public Shared Sub zipFolder( zFile As String, sourceDir As String)
		Dim fileZip As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(System.IO.File.Create(zFile))
		'fileZip.IsStreamOwner = False
		' 0 - store only to 9 - means best compression
		fileZip.SetLevel(9)
		
		zipFolder (fileZip,sourceDir)
	End Sub
	
	
	
	Public Shared Sub zipFolder( byref zFile As ICSharpCode.SharpZipLib.Zip.ZipOutputStream, sourceDir As String)
		If Not IO.Directory.Exists(sourceDir) Then
			Throw New MyManager.ManagerException("Directory sorgente non valida")
			exit sub
		End If
		
		Dim fs As  New IO.DirectoryInfo (sourceDir)
		zipFolder (zFile,fs, sourceDir)
		
		zFile.Finish() 
		zFile.Close()
	End Sub
	
	
	Private Shared sub zipFolder( byref zFile As ICSharpCode.SharpZipLib.Zip.ZipOutputStream, sourceDir As IO.DirectoryInfo, pathIniziale as String)
		Dim entry As  ICSharpCode.SharpZipLib.Zip.ZipEntry
		
		Dim buffer () As Byte
		
		Dim fileList() As IO.FileInfo  = sourceDir.GetFiles()
		Dim myFileInfo As IO.FileInfo
		
		
		'Dim myCrc As New ICSharpCode.SharpZipLib.Checksums.Crc32 
		
		For Each myFileInfo In fileList
			
			Dim fileSource As IO.FileStream = IO.File.OpenRead(myFileInfo.FullName )
			'buffer
			ReDim buffer (fileSource.Length-1)
			
			'leggo il file nel buffer
			fileSource.Read(buffer, 0, buffer.Length)
			fileSource.Close()
			
						
			Dim fileInfo As System.IO.FileInfo
			fileInfo = New System.IO.FileInfo(fileSource.Name)
			
			
			'creo una entry nel file ZIP
			entry = New ICSharpCode.SharpZipLib.Zip.ZipEntry(fileInfo.FullName.Replace(pathIniziale,"")  )
			entry.Size = buffer.Length
			entry.DateTime = DateTime.Now 
			
			'myCrc.Reset()
			'myCrc.Update(buffer)
			'entry.Crc = myCrc.Value
			
						
			'aggiugno la entry al file ZIP
			zFile.PutNextEntry(entry)
			'scrivo il file
			zFile.Write(buffer, 0, buffer.Length)
			
			
			
			'			Dim strmFile As IO.FileStream = IO.File.OpenRead(strFile)
			'			Dim abyBuffer(strmFile.Length - 1) As Byte
			'
			'			strmFile.Read(abyBuffer, 0, abyBuffer.Length)
			'			Dim objZipEntry As ICSharpCode.SharpZipLib.Zip.ZipEntry = New ICSharpCode.SharpZipLib.Zip.ZipEntry(strFile)
			'
			'			objZipEntry.DateTime = DateTime.Now
			'			objZipEntry.Size = strmFile.Length
			'			strmFile.Close()
			
			'zFile.Add(myFileInfo.FullName )
		Next
		
		
		Dim myDirectoryInfo As IO.DirectoryInfo
		Dim directoryList() As IO.DirectoryInfo
		directoryList = sourceDir.GetDirectories()
		'Dim temp As String 
		
		For Each myDirectoryInfo In directoryList
			
			'Create an entry whose name has a trailing slash ('/') character and add it to the Zip file.
			
			entry = New ICSharpCode.SharpZipLib.Zip.ZipEntry(myDirectoryInfo.FullName.Replace(pathIniziale,"") & "/" )
			'entry.Size =0
			'entry.DateTime = DateTime.Now 
			
			zFile.PutNextEntry(entry)
						
			'Recursive
			ZipManager.zipFolder(zFile,myDirectoryInfo, pathIniziale )
		Next
	End Sub
	
	
	Public Shared Sub extractZip(zFile As String , targetDir As String)
		Dim zip As New ICSharpCode.SharpZipLib.Zip.ZipInputStream( _
			IO.File.OpenRead(zFile))
		
		extractZip(zip,targetDir )
	End Sub
	
	Public Shared Sub extractZip(zFile As ICSharpCode.SharpZipLib.Zip.ZipInputStream , targetDir As String)
		If Not IO.Directory.Exists(targetDir ) Then
			IO.Directory.CreateDirectory(targetDir)
		End If
		
		'Dim strDirectory As String
		'	Dim strFile As String
		Dim wFile As IO.FileStream
		
		Dim fInfo As IO.FileInfo 
		
		'	Dim size As Long
		Dim buffer() As Byte
		
		
		
		Dim entry As ICSharpCode.SharpZipLib.Zip.ZipEntry
		entry = zFile.GetNextEntry()
		
		While Not isNothing(entry)
			If entry.IsFile Then
				
				fInfo = New IO.FileInfo (targetDir &  entry.Name)
				If Not fInfo.Directory.Exists Then
					fInfo.Directory.Create()
				End If
				
				
				wFile = New IO.FileStream(  targetDir &  entry.Name , io.FileMode.Create, IO.FileAccess.Write )
				ReDim buffer (entry.Size -1 )
				'leggo il file nel buffer
				zFile.Read(buffer, 0, buffer.Length)
				wFile.Write(buffer, 0,buffer.Length )
				wFile.Close()
			End If
			
			If entry.IsDirectory Then
				IO.Directory.CreateDirectory(targetDir & entry.Name )
			End If
			
			
			entry = zFile.GetNextEntry()
		End While
		
		zFile.Close()
		
	End Sub
	
	
End Class
