'
' Created by SharpDevelop.
' User: Roberto
' Date: 29/01/2009
' Time: 16.33
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class MyEventArgs
	Inherits System.EventArgs
	
	Private _label As String
	Private _value As String
	
	Public Sub New (label As String, value As String)
		Me._label = label
		Me._value = value
	End Sub
	
	
	
	
End Class
