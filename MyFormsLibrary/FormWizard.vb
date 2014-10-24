
Partial Public Class FormWizard

    Protected _currentStep As Integer
    Protected _connection As System.Data.Common.DbConnection
    Delegate Function Esegui(ByVal parametri() As Object) As Boolean
    Private m_eseguiAzione As MyFormsLibrary.FormWizard.Esegui
    Private colorSelNode As Drawing.Color = Color.FromArgb(165, 203, 247)

    Public Property EseguiAzione() As Esegui
        Get
            Return m_eseguiAzione
        End Get
        Set(ByVal value As Esegui)
            m_eseguiAzione = value
        End Set
    End Property


    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()

        Me._currentStep = 0
        ' AddHandler formc FormClosing, AddressOf Me.FormInsFormClosing

    End Sub

    'PIGLIACELLI: init vecchia *verificare su altri progetti se da rimuovere ()
    'Public Sub _init()

    '    'me.Mode = WorkMode.RunTime

    '    'format del treeview
    '    'dim myFont as New Font( FontFamily.GenericSansSerif, 9)

    '    'format tab controls

    '    For Each t As Windows.Forms.TreeNode In _treeView.Nodes
    '        t.BackColor = Drawing.SystemColors.Window
    '        t.ForeColor = Drawing.SystemColors.ControlText
    '        '		t.NodeFont = myFont
    '    Next
    '    _treeView.Nodes(0).BackColor = Drawing.Color.LightSkyBlue

    '    _ApplyGrant()

    'End Sub

    Public Overridable Function _init(ByRef connection As System.Data.Common.DbConnection, _
                                      ByVal titolo As String) As Boolean

        _tabControl.TabIndex = 0

        Me.Text = titolo
        Me.lblTitoloWizard.Text = titolo
        lblTitoloWizard.BackColor = colorSelNode


        If Not Me.Owner Is Nothing Then
            Me.Icon = Me.Owner.Icon
        End If
        Me._connection = connection

        For Each t As Windows.Forms.TreeNode In _treeView.Nodes
            t.BackColor = Drawing.SystemColors.Window
            t.ForeColor = Drawing.SystemColors.ControlText
            '		t.NodeFont = myFont
        Next
        _treeView.Nodes(0).BackColor = colorSelNode
        _currentStep = 0
        Me.btnPrevStep.Visible = False
        btnNextStep.Text = "Avanti"

        _ApplyGrant()

        Return True
    End Function



    Overridable Sub _GoToNextStep()
        Dim nextStep As Integer
        Dim MenuLeftIndex As Integer

        _currentStep += 1

        'navigazione normale ...
        If _currentStep < _treeView.Nodes.Count Then
            nextStep = _currentStep '+ 1
        Else
            nextStep = _currentStep - 1
        End If

        MenuLeftIndex = nextStep


        If _treeView.Nodes.Count > nextStep Then
            _treeView.Nodes(MenuLeftIndex - 1).BackColor = Drawing.SystemColors.Window
            _treeView.Nodes(MenuLeftIndex).BackColor = colorSelNode
        End If

       

        If _treeView.Nodes.Count > MenuLeftIndex Then
            label2.Text = _treeView.Nodes(MenuLeftIndex).Text
        End If


        'SE SALVATAGGIO-->!
        If _currentStep = _treeView.Nodes.Count Then

            If SaveAction() Then
                Windows.Forms.MessageBox.Show("Salvataggio avvenuto con successo", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
                'Dim formE As New MyFormsLibrary.FormMessageBox
                'formE._Init(MyFormsLibrary.FormMessageBox.Tipo.OK, "")
                Me.Close()
            Else
                'Dim formE As New MyFormsLibrary.FormMessageBox
                'formE._Init(MyFormsLibrary.FormMessageBox.Tipo.KO, "")
                Windows.Forms.MessageBox.Show("Salvataggio NON avvenuto con successo", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            End If

        End If


        'ALLA FINE, SETTO IL CURRENT
        _tabControl.SelectedIndex = nextStep
        _currentStep = nextStep

    End Sub

    Overridable Sub _GoToBackStep()
        Dim backStep As Integer
        Dim MenuLeftIndex As Integer

        backStep = _currentStep - 1
        MenuLeftIndex = backStep

        If _treeView.Nodes.Count > _currentStep Then
            _treeView.Nodes(_currentStep).BackColor = Drawing.SystemColors.Window
            _treeView.Nodes(MenuLeftIndex).BackColor = Me.colorSelNode
        End If

        _tabControl.SelectedIndex = backStep
        _currentStep = backStep

        label2.Text = _treeView.Nodes(MenuLeftIndex).Text

    End Sub


    Sub FormInsFormClosing(ByVal sender As Object, ByVal e As Windows.Forms.FormClosingEventArgs)
        e.Cancel = (System.Windows.Forms.MessageBox.Show("La chiusura del wizard comporterà la perdita dei dati inseriti. Si desidera chiudere?", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel)
    End Sub

    Public Overridable Sub BtnExitClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub


   

    Public Overridable Sub BtnNextStepClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextStep.Click
        _GoToNextStep()
    End Sub

    Public Overridable Sub BtnPrevStepClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrevStep.Click
        _GoToBackStep()
    End Sub

    Public Overridable Sub BtnLastStepClick(ByVal sender As Object, ByVal e As EventArgs)
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Sub

    Public Overridable Function SaveAction() As Boolean

        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Function


    Public Overridable Sub _tabControlSelecting(ByVal sender As Object, ByVal e As TabControlCancelEventArgs) Handles _tabControl.Selecting

        '		If Mode <>  WorkMode.Designer andalso sender.GetType.Name =  "TabControl" Then
        '			e.Cancel = True
        '			Exit Sub
        '		End If

        Dim myTab As Windows.Forms.TabControl = DirectCast(sender, Windows.Forms.TabControl)
        'Dim lastIndex As Int16  = myTab.TabCount
        Select Case e.TabPageIndex
            Case 0
                Me.btnPrevStep.Visible = False
                btnNextStep.Text = "Avanti"
            Case myTab.TabCount - 1 'LAST STEP
                Me.btnPrevStep.Visible = True
                btnNextStep.Text = "Fine"
            Case Else
                Me.btnPrevStep.Visible = True
                btnNextStep.Text = "Avanti"
        End Select

        'directcast (sender, System.Windows.Forms.TabControl ).Selecting(sender)

        DirectCast(sender, System.Windows.Forms.TabControl).TabIndex = e.TabPageIndex
    End Sub

    Friend Overridable Function _ApplyGrant() As Boolean
        Return True
    End Function

End Class
