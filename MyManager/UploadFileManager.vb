'Imports System.Web.UI.WebControls

Public Class UploadFileManager

    'Public Shared Function save(ByVal file As System.Web.UI.WebControls.FileUpload, _
    '                                ByVal destinationFolder As String, _
    '                                Optional ByVal renameFile As String = Nothing) As Boolean

    '    If (file.HasFile) Then

    '        Dim path As String = System.AppDomain.CurrentDomain.BaseDirectory
    '        path &= destinationFolder.Replace("~", "")

    '        ' If Not myDirectory.Exists(Server.MapPath(path)) Then

    '        If Not System.IO.Directory.Exists(path) Then
    '            'La cartella non esiste creala
    '            Try
    '                System.IO.Directory.CreateDirectory(path)
    '            Catch ex As Exception
    '                'Non è stato possibile creare la cartella
    '                MyManager.MailManager.send(ex)
    '                Exit Function
    '            End Try
    '        End If


    '        'verifico se rinominare il file oppure no
    '        If renameFile <> "" Then
    '            path &= renameFile
    '        Else
    '            path &= file.FileName
    '        End If

    '        'salvo il file ...
    '        file.SaveAs(path)

    '    End If
    '    Return True
    'End Function


    'Public Shared Function saveImage(ByVal file As System.Web.UI.WebControls.FileUpload, _
    '                               ByVal destinationFolder As String, _
    '                               Optional ByVal renameFileWithoutExtension As String = Nothing, _
    '                                Optional ByVal width As Int32 = 0, _
    '                                Optional ByVal height As Int32 = 0) As Boolean

    '    If (file.HasFile) Then

    '        Dim path As String = System.AppDomain.CurrentDomain.BaseDirectory
    '        path &= destinationFolder.Replace("~/", "")

    '        ' If Not myDirectory.Exists(Server.MapPath(path)) Then

    '        If Not System.IO.Directory.Exists(path) Then
    '            'La cartella non esiste creala
    '            Try
    '                System.IO.Directory.CreateDirectory(path)
    '            Catch ex As Exception
    '                'Non è stato possibile creare la cartella
    '                MyManager.MailManager.send(ex)
    '                Exit Function
    '            End Try
    '        End If


    '        'verifico se rinominare il file oppure no
    '        If renameFileWithoutExtension <> "" Then

    '            'ATTENZIONE
    '            'l'estenzione del file di destinazione viene determinata dal file sorgenete
    '            'quindi cancello l'nevuale estenzione per evitare cose del tipo .gif.gif

    '            If renameFileWithoutExtension.ToLower.EndsWith(".gif") Then
    '                renameFileWithoutExtension = renameFileWithoutExtension.ToLower.Replace(".gif", "")
    '            ElseIf renameFileWithoutExtension.ToLower.EndsWith(".jpg") Then
    '                renameFileWithoutExtension = renameFileWithoutExtension.ToLower.Replace(".jpg", "")
    '            End If


    '            If file.FileName.ToLower.EndsWith(".gif") Then
    '                renameFileWithoutExtension &= ".gif"
    '            ElseIf file.FileName.ToLower.EndsWith(".jpg") Then
    '                renameFileWithoutExtension &= ".jpg"
    '            Else
    '                Throw New MyManager.ManagerException("Estenzione di un immagine non riconosciuta")
    '            End If

    '            path &= renameFileWithoutExtension
    '        Else
    '            path &= file.FileName
    '        End If
    '        'salvo l'immagine ...
    '        file.SaveAs(path)
    '        file.Dispose()



    '        If (width <> 0) And (height <> 0) Then
    '            '... poi la rileggo per sovrasciverla...
    '            Dim resizeImage As System.Drawing.Bitmap
    '            resizeImage = MyManager.PhotoManager.getThumbnailImage(path, width, height)
    '            resizeImage.Save(path)
    '        End If


    '         ' path = path.ToLower.Replace(".jpg", ".gif")
    '        'If path.EndsWith(".jpg") Then
    '        '    resizeImage.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg)
    '        'ElseIf path.EndsWith(".gif") Then
    '        '    path = "c:\a.gif"
    '        '    resizeImage.Save(path, System.Drawing.Imaging.ImageFormat.Gif)
    '        'End If

    '    End If
    '    Return True
    'End Function
End Class
