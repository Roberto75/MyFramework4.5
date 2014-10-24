'
' Created by SharpDevelop.
' User: Roberto
' Date: 22/09/2008
' Time: 11.51
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Partial Class MyDateTimePicker
	Inherits System.Windows.Forms.DateTimePicker 
	
	 Public Enum WorkMode
        Birthday
        Standard
    End Enum

	Private _mode As WorkMode = WorkMode.Standard
    <System.ComponentModel.DefaultValueAttribute(GetType(WorkMode), "Standard")> _
    Public Property Mode() As WorkMode
        Get
            Return _mode
        End Get
        Set(ByVal value As WorkMode)
            _mode = value
            
            Select Case _mode
            	Case WorkMode.Birthday
            		Me.MaxDate = Now()
            	Case Else
            		me.MaxDate = DateTime.MaxValue
            End Select
        End Set
    End Property
	
	
	Public Sub New()
		Me.ShowCheckBox = True
		Me.Checked = False
		me.Width = 195
		me.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
	End Sub
	
	 
	 
	 
	
	
End Class
