'
' Created by SharpDevelop.
' User: Roberto
' Date: 10/12/2008
' Time: 14.08
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class MyException
	Inherits System.Exception
	
	Public Enum ErrorNumber
		Undefined = 0
		FunctionNotAvailabe = 1
	End Enum
	
	
	'Public _messageColor As System.ConsoleColor
	Public ErrorCode As MyException.ErrorNumber
	
	Public Sub New()
		MyBase.New()
		Me.ErrorCode = ErrorNumber.Undefined
	End Sub
	
	REM Crea un eccezione caratterizzata dall'avere come massaggio una stringa
	Public Sub New(ByVal message As String)
		MyBase.New(message)
		Me.ErrorCode = ErrorNumber.Undefined
	End Sub
	
	Public Sub New(ByVal message As String, ByVal innerException As Exception)
		MyBase.New(message, innerException)
		Me.ErrorCode = ErrorNumber.Undefined
	End Sub
	
	Public Sub New(ByVal errorNumber As ErrorNumber)
		MyBase.New("")
		Me.ErrorCode = errorNumber
	End Sub
	
	
	
	Public Sub New(ByVal errorNumber As ErrorNumber, ByVal message As String)
		MyBase.New(message)
		Me.ErrorCode = errorNumber
	End Sub
	
	
	
	Public Overrides ReadOnly Property Message() As String
		Get
			Dim messaggio As String = ""
			If  Me.ErrorCode = ErrorNumber.Undefined Then
				messaggio = MyBase.Message
			Else
				Select Case ErrorCode
					Case ErrorNumber.FunctionNotAvailabe 
						messaggio = "La funzione richiesta non è stata implementata."
					Case Else
						messaggio = "ATTENZIONE, codice di errore non gestito."
					
				End Select
				
				If Not String.IsNullOrEmpty(MyBase.Message) Then
					messaggio &= vbCrLf & MyBase.Message
				End If
				
			End If
			Return messaggio
		End Get
	End Property
	
	
End Class
