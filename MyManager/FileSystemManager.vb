'
' Created by SharpDevelop.
' User: Roberto
' Date: 18/06/2009
' Time: 9.56
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class FileSystemManager

    Protected Shared _progressBar As Windows.Forms.ToolStripProgressBar
    Protected Shared _statusBar As Windows.Forms.ToolStripLabel

    Public Shared Sub _setProgressBar(ByRef value As Windows.Forms.ToolStripProgressBar)
        _progressBar = value
    End Sub

    Public Shared Sub _setStatusBar(ByRef value As Windows.Forms.ToolStripLabel)
        _statusBar = value
    End Sub



    Shared Sub copyFiles(ByVal sourceFileListPath As String(), ByVal destinationFolder As String)

        'If Not IO.Directory.Exists(destinationFolder) Then
        ' Throw New MyManager.ManagerException("Folder not exists")
        'Exit Sub
        'End If

        If Not destinationFolder.EndsWith("\") Then
            destinationFolder &= "\"
        End If

        If Not _progressBar Is Nothing Then
            _progressBar.Style = Windows.Forms.ProgressBarStyle.Blocks
            _progressBar.Maximum = sourceFileListPath.Length
            _progressBar.Value = 0
            _progressBar.Step = 1
        End If

        Dim fileSource As IO.FileInfo

        For i As Integer = 0 To sourceFileListPath.Length - 1
            fileSource = New IO.FileInfo(sourceFileListPath(i))
            fileSource.CopyTo(destinationFolder & fileSource.Name, True)

            If Not _progressBar Is Nothing Then
                _progressBar.PerformStep()
            End If
        Next

        If Not _progressBar Is Nothing Then
            _progressBar.Value = 0
        End If

    End Sub


    Shared Sub copyDirectory(ByVal SourcePath As String, ByVal DestPath As String, Optional ByVal Overwrite As Boolean = False)

        Dim SourceDir As IO.DirectoryInfo = New IO.DirectoryInfo(SourcePath)
        Dim DestDir As IO.DirectoryInfo = New IO.DirectoryInfo(DestPath)

        ' the source directory must exist, otherwise throw an exception
        If SourceDir.Exists Then
            ' if destination SubDir's parent SubDir does not exist throw an exception
            If Not DestDir.Parent.Exists Then
                Throw New IO.DirectoryNotFoundException _
                    ("Destination directory does not exist: " & DestDir.Parent.FullName)
            End If

            If Not DestDir.Exists Then
                DestDir.Create()
            End If

            ' copy all the files of the current directory
            Dim ChildFile As IO.FileInfo
            For Each ChildFile In SourceDir.GetFiles()
                If Overwrite Then
                    ChildFile.CopyTo(IO.Path.Combine(DestDir.FullName, ChildFile.Name), True)
                Else
                    ' if Overwrite = false, copy the file only if it does not exist
                    ' this is done to avoid an IOException if a file already exists
                    ' this way the other files can be copied anyway...
                    If Not IO.File.Exists(IO.Path.Combine(DestDir.FullName, ChildFile.Name)) Then
                        ChildFile.CopyTo(IO.Path.Combine(DestDir.FullName, ChildFile.Name), False)
                    End If
                End If
            Next

            ' copy all the sub-directories by recursively calling this same routine
            Dim SubDir As IO.DirectoryInfo
            For Each SubDir In SourceDir.GetDirectories()
                CopyDirectory(SubDir.FullName, IO.Path.Combine(DestDir.FullName, _
                    SubDir.Name), Overwrite)
            Next
        Else
            Throw New IO.DirectoryNotFoundException("Source directory does not exist: " & SourceDir.FullName)
        End If
    End Sub

End Class
