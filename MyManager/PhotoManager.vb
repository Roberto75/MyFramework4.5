Imports Microsoft.VisualBasic
Imports System.Drawing.Imaging

Public Class PhotoManager
    Inherits MyManager.Manager


    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Shared Function resizeImageWidth(ByVal imgToResize As System.Drawing.Image, ByVal newSizeWidth As Integer) As System.Drawing.Image
        Return MyManager.PhotoManager.resizeImage(imgToResize, newSizeWidth, -1)
    End Function

    Public Shared Function resizeImageHeight(ByVal imgToResize As System.Drawing.Image, ByVal newSizeHeight As Integer) As System.Drawing.Image
        Return MyManager.PhotoManager.resizeImage(imgToResize, -1, newSizeHeight)
    End Function
    Public Shared Function resizeImage(ByVal imgToResize As System.Drawing.Image, ByVal newSizeWidth As Integer, ByVal newSizeHeight As Integer) As System.Drawing.Image

        Dim sourceWidth As Integer = imgToResize.Width
        Dim sourceHeight As Integer = imgToResize.Height

        '    Dim nPercent As Double = 0
        Dim nPercentW As Double = 0
        Dim nPercentH As Double = 0

        nPercentW = newSizeWidth / sourceWidth
        nPercentH = newSizeHeight / sourceHeight

        If newSizeWidth = -1 Then
            nPercentW = nPercentH
        End If

        If newSizeHeight = -1 Then
            nPercentH = nPercentW
        End If



        '   If (nPercentH < nPercentW) Then
        'nPercent = nPercentH
        'Else
        'nPercent = nPercentW
        'End If


        Dim destWidth As Integer = sourceWidth * nPercentW
        Dim destHeight As Integer = sourceHeight * nPercentH

        Dim b As New System.Drawing.Bitmap(destWidth, destHeight)

        Dim g As System.Drawing.Graphics

        g = System.Drawing.Graphics.FromImage(b)
        g.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight)
        g.Dispose()

        Return b

    End Function

    Public Shared Function getThumbnailImage(ByVal oldImage As System.Drawing.Image, ByVal width As Int16, ByVal height As Int16) As System.Drawing.Image

        'Dim oldImage As New System.Drawing.Bitmap(absolutePathImage)
        'Dim newImage As New System.Drawing.Bitmap(width, height)

        'Dim gfx As System.Drawing.Graphics
        'gfx = System.Drawing.Graphics.FromImage(newImage)
        'gfx.DrawImage(oldImage, 0, 0, width, height)
        'gfx.Dispose()

        'newImage.Save("c:\a1.gif")

        'oldImage.Dispose()

        Return oldImage.GetThumbnailImage(width, height, Nothing, IntPtr.Zero)

    End Function

    'se viene specificata solo la larghezza allora ridimensiono l'immagine mantenendo le proporzioni
    Public Shared Function getThumbnailImageByWidth(ByVal absolutePathImage As String, ByVal width As Int16) As System.Drawing.Image
        Dim oldImage As System.Drawing.Image
        oldImage = System.Drawing.Image.FromFile(absolutePathImage)
        Return MyManager.PhotoManager.getThumbnailImageByWidth(oldImage, width)
    End Function


    Public Shared Function getThumbnailImageByWidth(ByVal oldImage As System.Drawing.Image, ByVal width As Int16) As System.Drawing.Image
        'calcolo le proporzioni
        Dim newImage As System.Drawing.Image
        Dim proporzioni As Double = (oldImage.Height * width) / oldImage.Width
        newImage = oldImage.GetThumbnailImage(width, proporzioni, Nothing, IntPtr.Zero)
        Return newImage
    End Function

    Public Shared Function getThumbnailImageByHeight(ByVal absolutePathImage As String, ByVal height As Int16) As System.Drawing.Image
        Dim oldImage As System.Drawing.Image
        oldImage = System.Drawing.Image.FromFile(absolutePathImage)
        Return MyManager.PhotoManager.getThumbnailImageByHeight(oldImage, height)
    End Function


    Public Shared Function getThumbnailImageByHeight(ByVal oldImage As System.Drawing.Image, ByVal height As Int16) As System.Drawing.Image
        'calcolo le proporzioni
        Dim newImage As System.Drawing.Image
        Dim proporzioni As Double = (oldImage.Width * height) / oldImage.Height

        newImage = oldImage.GetThumbnailImage(proporzioni, height, Nothing, IntPtr.Zero)
        Return newImage
    End Function

    'di default viene generata un Thumbnail di 100x100
    Public Shared Function getThumbnailImage(ByVal absolutePathImage As String) As System.Drawing.Image
        Dim oldImage As System.Drawing.Image
        oldImage = System.Drawing.Image.FromFile(absolutePathImage)

        Return getThumbnailImage(oldImage, 100, 100)
    End Function

    Public Shared Function getThumbnailImage(ByVal absolutePathImage As String, ByVal width As Int16, ByVal height As Int16) As System.Drawing.Image
        Dim oldImage As System.Drawing.Image
        oldImage = System.Drawing.Image.FromFile(absolutePathImage)

        Return getThumbnailImage(oldImage, width, height)
    End Function

    Public Shared Function getThumbnailImage(ByVal oldImage As System.Drawing.Image) As System.Drawing.Image
        Return getThumbnailImage(oldImage, 100, 100)
    End Function



    Public Function insertPhoto(ByVal absolutePath As String, ByVal description As String, ByVal externalId As Long, ByVal userId As Long) As Long
        Dim strSQL As String
        strSQL = "INSERT INTO PHOTO ( PATH, DESCRIPTION, FK_USER_ID, FK_EXTERNAL_ID) " & _
                   " VALUES ( @PATH , @DESCRIPTION , @FK_USER_ID ,  @FK_EXTERNAL_ID   ) "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        mAddParameter(command, "@PATH", absolutePath)
        mAddParameter(command, "@DESCRIPTION", description)
        mAddParameter(command, "@FK_USER_ID", userId)
        mAddParameter(command, "@FK_EXTERNAL_ID", externalId)

        command.CommandText = strSQL

        mExecuteNoQuery(command)

        'Return _getIdentity()
        Return 1
    End Function


    Public Function deletePhoto(ByVal photoId As Long) As Boolean
        Return deletePhoto(photoId, "")
    End Function


    Public Function deletePhoto(ByVal photoId As Long, ByVal absoluteServerPath As String) As Boolean

        Dim path As String
        mStrSQL = "SELECT PATH FROM PHOTO WHERE PHOTO_ID = " & photoId
        path = Me.mExecuteScalar(mStrSQL)


        If Not String.IsNullOrEmpty(path) AndAlso Not path.StartsWith("http") AndAlso Not String.IsNullOrEmpty(absoluteServerPath) Then

            If path.StartsWith("~") Then
                path = path.Substring(1)
            End If


            'cancellazione su file system della photo originale
            Dim f As New IO.FileInfo(absoluteServerPath & path)
            If f.Exists Then
                f.Delete()
            End If


            'cancellazione su file system della Thumbnail
            Dim temp As String
            temp = f.DirectoryName & "/thumbnail_" & f.Name
            Dim t As New IO.FileInfo(temp)
            If t.Exists Then
                t.Delete()
            End If


        End If

        mStrSQL = "DELETE * FROM PHOTO WHERE PHOTO_ID = " & photoId
        Me.mExecuteNoQuery(mStrSQL)

        Return True
    End Function


    Public Function getPhotosIsPlanimetria(ByVal externalId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM PHOTO WHERE  DATE_DELETED IS NULL AND DESCRIPTION = 'PLANIMETRIA' AND  FK_EXTERNAL_ID  = " & externalId
        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function getPhotosIsNotPlanimetria(ByVal externalId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM PHOTO WHERE  DATE_DELETED IS NULL AND DESCRIPTION <> 'PLANIMETRIA' AND  FK_EXTERNAL_ID  = " & externalId
        Return Me.mFillDataTable(mStrSQL)
    End Function

    Public Function getPhotos(ByVal externalId As Long) As Data.DataTable
        mStrSQL = "SELECT * FROM PHOTO WHERE  DATE_DELETED IS NULL AND  FK_EXTERNAL_ID  = " & externalId
        Return Me.mFillDataTable(mStrSQL)
    End Function


    Public Function getMyPhotos(ByVal externalId As Long) As List(Of MyPhoto)
        Dim dt As Data.DataTable
        dt = getPhotos(externalId)

        Dim result As New List(Of MyPhoto)
        Dim photo As MyPhoto

        For Each row As Data.DataRow In dt.Rows
            photo = New MyPhoto(row("photo_id"), row("path"), row("description"))
            result.Add(photo)
        Next

        Return result
    End Function

    Public Function getMyPhotosIsPlanimetria(ByVal externalId As Long) As List(Of MyPhoto)
        Dim dt As Data.DataTable
        dt = getPhotosIsPlanimetria(externalId)

        Dim result As New List(Of MyPhoto)
        Dim photo As MyPhoto
        Dim conta As Integer = 1

        For Each row As Data.DataRow In dt.Rows
            photo = New MyPhoto(row("photo_id"), row("path"), row("description") & " - " & conta)
            result.Add(photo)
            conta = conta + 1
        Next

        Return result
    End Function

    Public Function getMyPhotosIsNotPlanimetria(ByVal externalId As Long) As List(Of MyPhoto)
        Dim dt As Data.DataTable
        dt = getPhotosIsNotPlanimetria(externalId)

        Dim result As New List(Of MyPhoto)
        Dim photo As MyPhoto

        For Each row As Data.DataRow In dt.Rows
            photo = New MyPhoto(row("photo_id"), row("path"), row("description"))
            result.Add(photo)
        Next

        Return result
    End Function

End Class


Public Class MyPhoto

    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Long, ByVal path As String, ByVal description As String)
        _photoId = id
        _path = path
        _note = description
    End Sub


    Private _photoId As Long
    Public Property PhotoId() As Long
        Get
            Return _photoId
        End Get
        Set(ByVal value As Long)
            _photoId = value
        End Set
    End Property



    Private _path As String
    Public Property Path() As String
        Get
            Return _path
        End Get
        Set(ByVal value As String)
            _path = value
        End Set
    End Property



    Private _note As String
    Public Property Note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property


End Class
