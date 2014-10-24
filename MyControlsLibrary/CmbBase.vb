'
' Created by SharpDevelop.
' User: Roberto
' Date: 04/11/2008
' Time: 12.43
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class CmbBase
	Inherits System.Windows.Forms.ComboBox
	

	
	Public Enum  WorkMode 
		Insert
		Update
	End Enum
	
	Protected  _mode as WorkMode 
	Public Property	Mode As WorkMode 
		Get 
			return _mode
		End Get
		Set( ByVal value As WorkMode )
			_mode = value
		End Set
	End Property
		
	
	Public Sub New()
		Me.FlatStyle = System.Windows.Forms.FlatStyle.Popup
		Me.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems 
		Me.AutoCompleteMode =  System.Windows.Forms.AutoCompleteMode.None  
		me.Size = new  System.Drawing.Size(195, 21)
	End Sub

    Overridable Function _init() As Boolean
        Return True
    End Function

End Class
