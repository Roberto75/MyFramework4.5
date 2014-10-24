'
' Created by SharpDevelop.
' User: Roberto
' Date: 17/12/2008
' Time: 12.56
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class CmbItem
	
	Private _label As String
	Public Property Label as String
		Get
			Return _label
		End Get
		Set(ByVal value As String)
			_label = value
		End Set
	End Property
	
	Private _value As String
	Public Property Value as String
		Get
			Return _value
		End Get
		Set(ByVal inValue As String)
			_value = inValue
		End Set
	End Property
	
	
	Public Sub New ()
		Me._label = ""
		Me._value = ""
	End Sub
	
	Public Sub New (label As String, value As String)
		Me._label = label
		Me._value = value
	End Sub
End Class
