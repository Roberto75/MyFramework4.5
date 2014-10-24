
'http://www.fryan0911.com/2009/05/how-to-map-shared-network-folder-using.html

Imports System.Runtime.InteropServices

Public Class MapDriveManager
	
	Private Declare Function WNetAddConnection2 Lib "mpr.dll" Alias "WNetAddConnection2A" _
		(ByRef lpNetResource As NETRESOURCE, ByVal lpPassword As String, _
		ByVal lpUserName As String, ByVal dwFlags As Integer) As Integer
	
	Private  Declare Function WNetCancelConnection2 Lib "mpr" Alias "WNetCancelConnection2A" _
		(ByVal lpName As String, ByVal dwFlags As Integer, ByVal fForce As Integer) As Integer
	
	<StructLayout(LayoutKind.Sequential)> _
		Private Structure NETRESOURCE
		Public dwScope As Integer
		Public dwType As Integer
		Public dwDisplayType As Integer
		Public dwUsage As Integer
		Public lpLocalName As String
		Public lpRemoteName As String
		Public lpComment As String
		Public lpProvider As String
	End Structure
	
	
	Private Const ForceDisconnect As Integer = 1
	Private Const RESOURCETYPE_DISK As Long = &H1
	
	
	
	Public Shared Function MapDrive(ByVal DriveLetter As String, ByVal UNCPath As String, ByVal username As String, ByVal password As String) As Boolean
		
		If Not DriveLetter.EndsWith(":") Then
			DriveLetter = DriveLetter & ":"
		End If
		
		If String.IsNullOrEmpty(username) Then
			username = Nothing
		End If
		
		If String.IsNullOrEmpty(password) Then
			password = Nothing
		End If
		
		Dim nr As NETRESOURCE
		
		nr = New NETRESOURCE
		nr.lpRemoteName = UNCPath
		nr.lpLocalName = DriveLetter 
		nr.dwType = RESOURCETYPE_DISK
		
		Dim result As Integer
		result = WNetAddConnection2(nr,  password, username, 0)
		'The following topics provide lists of system error codes.
		'http://msdn.microsoft.com/en-us/library/ms681381(VS.85).aspx
		Select Case result
			Case 0
				Return True
			Case 1200
				Throw New ApplicationException("ERROR_BAD_DEVICE")
			Case 1326
				Throw new ApplicationException ("ERROR_LOGON_FAILURE: unknown user name or bad password")
			Case Else
				Return False
		End Select
		
	End Function
	
	
	
	
	Public Shared Function UnMapDrive(ByVal DriveLetter As String) As Boolean
		If Not DriveLetter.EndsWith(":") Then
			DriveLetter = DriveLetter & ":"
		End If
		
		Dim rc As Integer
		rc = WNetCancelConnection2(DriveLetter, 0, ForceDisconnect)
		
		If rc = 0 Then
			Return True
		Else
			Return False
		End If
		
	End Function
	
End Class
