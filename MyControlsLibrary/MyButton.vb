'
' Created by SharpDevelop.
' User: Roberto
' Date: 26/09/2008
' Time: 15.06
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Partial Class MyButton
	Inherits System.Windows.Forms.Button
	
    Public Enum ButtonType
        btnNext
        btnPrev
        btnLast
        btnFind
        btnExit
        btnNew
        btnSave
        btnReset
        btnExecute
        btnEdit
        btnRefresh
        btnDelete
        btnPrint
        btnOpen
        btnPreview
        btnReport
        btnExport
        Standard
    End Enum
	
	Public Enum ButtonSize
	px150
	px122
	End Enum
	
	Private resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MyButton))
	
	Private _type As ButtonType = ButtonType.Standard
	<System.ComponentModel.DefaultValueAttribute(GetType(ButtonType), "Standard")> _
		Public Property MyType() As ButtonType
		Get
			Return _type
		End Get
		Set(ByVal value As ButtonType)
			_type = value
			changeImage()
		End Set
	End Property
	
	
	Private _size As ButtonSize  = ButtonSize.px150
	<System.ComponentModel.DefaultValueAttribute(GetType(ButtonSize), "px150")> _
		Public Property MySize() As ButtonSize
		Get
			Return _size
		End Get
		Set(ByVal value As ButtonSize)
			_size = value
			changeImage()
		End Set
	End Property
	
	
	
	Private Sub changeImage()
		If _size = ButtonSize.px150 Then
			Select Case _type
				Case ButtonType.btnNew
					MyBase.Image = CType(resources.GetObject("btnNew150.Image"),System.Drawing.Image)
					MyBase.Text = "Nuovo ..."
				Case ButtonType.btnExecute
					MyBase.Image = CType(resources.GetObject("btnExecute150.Image"),System.Drawing.Image)
					MyBase.Text = "Esegui"
				Case ButtonType.btnSave
					MyBase.Image = CType(resources.GetObject("btnSave150.Image"),System.Drawing.Image)
					MyBase.Text = "Salva"
				Case ButtonType.btnReset
					MyBase.Image = CType(resources.GetObject("btnReset150.Image"),System.Drawing.Image)
					MyBase.Text = "Azzera campi"
				Case ButtonType.btnFind
					MyBase.Image = CType(resources.GetObject("btnFind150.Image"),System.Drawing.Image)
					MyBase.Text = "Cerca"
				Case ButtonType.btnEdit
					MyBase.Image = CType(resources.GetObject("btnEdit150.Image"),System.Drawing.Image)
					MyBase.Text = "Modifica"
				Case ButtonType.btnDelete
					MyBase.Image = CType(resources.GetObject("btnDelete150.Image"),System.Drawing.Image)
                    MyBase.Text = "Elimina"
                Case ButtonType.btnExit
                    MyBase.Image = CType(resources.GetObject("btnDelete150.Image"), System.Drawing.Image)
                    MyBase.Text = "Esci"
				Case ButtonType.btnRefresh 
					MyBase.Image = CType(resources.GetObject("btnRefresh150.Image"),System.Drawing.Image)
                    MyBase.Text = "Aggiorna"
                Case ButtonType.btnPrint
                    MyBase.Image = CType(resources.GetObject("btnPrint150.Image"), System.Drawing.Image)
                    MyBase.Text = "Stampa"
                Case ButtonType.btnOpen
                    MyBase.Image = CType(resources.GetObject("btnOpen150.Image"), System.Drawing.Image)
                    MyBase.Text = "Apri"
                Case ButtonType.btnPreview
                    MyBase.Image = CType(resources.GetObject("btnPreview150.Image"), System.Drawing.Image)
                    MyBase.Text = "Preview"
                Case ButtonType.btnReport
                    MyBase.Image = CType(resources.GetObject("btnReport150.Image"), System.Drawing.Image)
                    MyBase.Text = "Report"
                Case ButtonType.btnExport
                    MyBase.Image = CType(resources.GetObject("btnExport150.Image"), System.Drawing.Image)
                    MyBase.Text = "Export"
				Case Else
					MyBase.Image = CType(resources.GetObject("NotAvailable.Image"),System.Drawing.Image)
					MyBase.Text = ""
			End Select
			me.Width = 156
		ElseIf  _size = ButtonSize.px122  Then
			Select Case _type
				Case ButtonType.btnNext
					MyBase.Image = CType(resources.GetObject("btnNext122.Image"),System.Drawing.Image)
					MyBase.Text = "Avanti"
				Case ButtonType.btnPrev
					MyBase.Image = CType(resources.GetObject("btnPrev122.Image"),System.Drawing.Image)
					MyBase.Text = "Indietro"
				Case ButtonType.btnLast
					MyBase.Image = CType(resources.GetObject("btnNext122.Image"),System.Drawing.Image)
					MyBase.Text = "Finito"
				Case ButtonType.btnExit
					MyBase.Image = CType(resources.GetObject("btnExit122.Image"),System.Drawing.Image)
					MyBase.Text = "Esci"
				Case ButtonType.btnRefresh 
					MyBase.Image = CType(resources.GetObject("btnRefresh122.Image"),System.Drawing.Image)
                    MyBase.Text = "Aggiorna"
                Case ButtonType.btnOpen
                    MyBase.Image = CType(resources.GetObject("btnOpen122.Image"), System.Drawing.Image)
                    MyBase.Text = "Apri"
                Case ButtonType.btnPreview
                    MyBase.Image = CType(resources.GetObject("btnPreview122.Image"), System.Drawing.Image)
                    MyBase.Text = "Preview"
                Case ButtonType.btnReport
                    MyBase.Image = CType(resources.GetObject("btnReport122.Image"), System.Drawing.Image)
                    MyBase.Text = "Report"
				Case Else
					MyBase.Image = CType(resources.GetObject("NotAvailable.Image"),System.Drawing.Image)
					MyBase.Text = ""
			End Select
			me.Width = 130
		End If
	End Sub
	
	
	Public Sub New()
        Me.FlatAppearance.BorderSize = 0
        Me.FlatAppearance.MouseDownBackColor = Color.Transparent
        Me.FlatAppearance.MouseOverBackColor = Color.Transparent

        Me.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.Width = 156
		Me.Height  = 30
        'Me.UseVisualStyleBackColor = true
        Me.BackColor = Color.Transparent

	End Sub
	
	
	
End Class
