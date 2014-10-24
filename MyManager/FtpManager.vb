Public Class FtpManager
    Private _ftpRequest As System.Net.FtpWebRequest
    Private _ftpResponse As System.Net.FtpWebResponse


    Sub New()

    End Sub


    Public Function upload(ByVal localFile As IO.FileInfo) As Boolean

    End Function


    Public Function download(ByVal remoteFile As String, ByVal localFile As IO.FileInfo) As Boolean


    End Function

End Class
