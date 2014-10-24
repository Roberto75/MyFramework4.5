Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Security.Cryptography

Public Class SecurityManager
    Private Const chiaveDES As String = "_C1t1_0#"
    Private Const vettoreDES As String = "#_S3rv13"

    Private Shared Function convert2Byte(ByVal stringa As String) As Byte()
        Dim i As Integer
        Dim arrayChar As Char()

        arrayChar = stringa.ToCharArray()
        Dim arrayByte(arrayChar.Length - 1) As Byte
        For i = 0 To arrayByte.Length - 1
            arrayByte(i) = Convert.ToByte(arrayChar(i))
        Next
        Return arrayByte
    End Function

	Public Shared Function getMD5HashFile(pathFile As String)	As String
		If Not IO.File.Exists (pathFile) Then
			return "File not found"
		End If
		
		Dim fs as New IO.FileStream(pathFile , IO.FileMode.Open, IO.FileAccess.Read  )
		Dim result As String
		Try
			result = getMD5HashFile(fs )
		Finally
			fs.Close()
		End Try
		
		Return result
	End Function


    Public Shared Function getMD5HashFile(input As IO.FileStream)	As String
	 	' Create a new instance of the MD5 object.
        Dim md5Hasher As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(input)

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New System.Text.StringBuilder()

        ' Loop through each byte of the hashed data
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i


		input.Close()
        ' Return the hexadecimal string.
        Return sBuilder.ToString().ToUpper()
	 End Function

	

    Public Shared Function getMD5Hash(ByVal input As String) As String
        ' Create a new instance of the MD5 object.
        Dim md5Hasher As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New System.Text.StringBuilder()

        ' Loop through each byte of the hashed data
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString().ToUpper()
    End Function

    Public Shared Function criptaDES(ByVal stringa As String) As String
        Dim arrayChiaveDES As Byte()
        Dim arrayVettoreDES As Byte()
        Dim strInput As Byte()
        Dim objDES As DESCryptoServiceProvider

        Try
            arrayChiaveDES = convert2Byte(chiaveDES)
            arrayVettoreDES = convert2Byte(vettoreDES)
            strInput = convert2Byte(stringa)
            objDES = New DESCryptoServiceProvider
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, objDES.CreateEncryptor(arrayChiaveDES, arrayVettoreDES), CryptoStreamMode.Write)
            cs.Write(strInput, 0, strInput.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            Return "ERRORE: " & ex.Message
        End Try
    End Function

    Public Shared Function decriptaDES(ByVal stringa As String) As String
        Dim arrayChiaveDES As Byte()
        Dim arrayVettoreDES As Byte()
        Dim strInput As Byte()
        Dim objDES As DESCryptoServiceProvider

        Try
            arrayChiaveDES = convert2Byte(chiaveDES)
            arrayVettoreDES = convert2Byte(vettoreDES)
            strInput = convert2Byte(stringa)
            objDES = New DESCryptoServiceProvider
            strInput = Convert.FromBase64String(stringa)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, objDES.CreateDecryptor(arrayChiaveDES, arrayVettoreDES), CryptoStreamMode.Write)

            cs.Write(strInput, 0, strInput.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

        Catch ex As Exception
            Return "ERRORE: " & ex.Message
        End Try
    End Function


End Class
