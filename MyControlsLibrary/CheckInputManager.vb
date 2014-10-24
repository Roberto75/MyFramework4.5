'
' Created by SharpDevelop.
' User: Roberto
' Date: 18/03/2009
' Time: 16.57
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'

Public Class CheckInputManager
	
	Private _refErrorProvider As Windows.Forms.ErrorProvider
	
	ReadOnly Property _ErrorProvider  as Windows.Forms.ErrorProvider
		Get
			Return _refErrorProvider
		End Get
	End Property
	
	
	Sub New(ByRef errProvider As  Windows.Forms.ErrorProvider )
		me._refErrorProvider = errProvider
	End sub
	
	
	Function isMandatory(byref  ctrl As Windows.Forms.Control) As Boolean
		If TypeOf ctrl Is MyTextBox Then
			If String.IsNullOrEmpty (Ctype(ctrl, MyTextBox)._Value ) Then
				Me._refErrorProvider.SetError(ctrl, "Specificare un valore")
				ctrl.focus()
				Return True
			End If
			Me._refErrorProvider.SetError(ctrl,"")
			Return False
		End If
		Return True
	End Function
	
	
	Function Limit(byref ctrl As Windows.Forms.Control, lower as Integer , higher  as Integer  ) as Boolean
		If TypeOf ctrl Is MyTextBox Then
			Dim valore As Decimal
			valore = Decimal.Parse ( CType(ctrl, MyTextBox )._value)
			
			If valore < lower Then
				_refErrorProvider.SetError(ctrl, "In valore deve essere maggiore di " & lower)
				ctrl.Focus()
				Return true
			End If
			
			If valore > higher Then
				_refErrorProvider.SetError(ctrl, "In valore deve essere inferiore a " & higher)
				ctrl.Focus()
				Return true
			End If
			
			_refErrorProvider.SetError(ctrl, "")
			Return False
			
		End If
		
		Return True
	End Function
	
	
	Function GreaterZero( byref ctrl As Windows.Forms.Control ) as Boolean
		If TypeOf ctrl Is MyTextBox Then
			Dim valore As Decimal
			valore = Decimal.Parse ( CType(ctrl, MyTextBox )._value)
			
			If valore <= 0 Then
				_refErrorProvider.SetError(ctrl, "In valore deve essere maggiore di 0")
				ctrl.Focus()
				Return True
			End If
			
			_refErrorProvider.SetError(ctrl, "")
			Return False
		End If
		
		Return True
	End Function
	
	Function GreaterEqualZero( byref ctrl As Windows.Forms.Control ) as Boolean
		If TypeOf ctrl Is MyTextBox Then
			Dim valore As Decimal
			valore = Decimal.Parse ( CType(ctrl, MyTextBox )._value)
			
			If valore < 0 Then
				_refErrorProvider.SetError(ctrl, "In valore deve essere maggiore di 0")
				ctrl.Focus()
				Return True
			End If
			
			_refErrorProvider.SetError(ctrl, "")
			Return False
		End If
		
		Return True
	End Function
	
	
End Class
