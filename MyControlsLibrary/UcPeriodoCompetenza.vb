Public Class UcPeriodoCompetenza


    Public Enum WorkMode
        Undefined
        IntervalloDaA
        NumeroGiorni
        Mese
    End Enum

    Public Sub New()
        InitializeComponent()
        Me._workMode = WorkMode.Undefined
    End Sub

    Private _workMode As WorkMode
    Public ReadOnly Property MyWorkMode() As WorkMode
        Get
            Return _workMode
        End Get
        '    Set(ByVal value As WorkMode)
        '        _workMode = value
        '  End Set
    End Property


    Public Function _init(ByVal mode As WorkMode) As Boolean

        Me.dtA.Value = Now
        Me.dtDa.Value = New Date(Now.Year, Now.Month, 1)

        Me.anno.Text = Now.ToString("yyyy")

        Me.cmbMese.SelectedItem = Now.ToString("MM")

        initControls(mode)
    End Function

    Private Sub initControls(ByVal mode As WorkMode)

        If mode = _workMode Then
            Exit Sub
        End If

        Dim font As Font
        font = labelPeriodo.Font

        Select Case mode
            Case WorkMode.IntervalloDaA
                dtA.Checked = True
                dtDa.Checked = True
                dtA.Enabled = True
                dtDa.Enabled = True
                upDownNumGiorni.Enabled = False
                cmbMese.Enabled = False
                anno.Enabled = False

                labelPeriodo.Font = New Font(font, FontStyle.Underline Or FontStyle.Bold)
                labelGiorni.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)
                labelMese.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)

            Case WorkMode.Mese
                dtA.Checked = False
                dtDa.Checked = False
                dtA.Enabled = False
                dtDa.Enabled = False
                upDownNumGiorni.Enabled = False
                cmbMese.Enabled = True
                anno.Enabled = True

                labelPeriodo.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)
                labelGiorni.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)
                labelMese.Font = New Font(font, FontStyle.Underline Or FontStyle.Bold)

            Case WorkMode.NumeroGiorni
                dtA.Checked = False
                dtDa.Checked = False
                dtA.Enabled = False
                dtDa.Enabled = False
                upDownNumGiorni.Enabled = True
                cmbMese.Enabled = False
                anno.Enabled = False

                labelPeriodo.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)
                labelGiorni.Font = New Font(font, FontStyle.Underline Or FontStyle.Bold)
                labelMese.Font = New Font(font, FontStyle.Underline Or FontStyle.Regular)

        End Select

        Me._workMode = mode
    End Sub

    Public Function _Da() As Date
        Select Case _workMode
            Case WorkMode.IntervalloDaA
                If dtDa.Checked = False Then
                    Return Date.MinValue
                End If
                Return Me.dtDa.Value
            Case WorkMode.NumeroGiorni
                Return DateAdd(DateInterval.Day, -Me.upDownNumGiorni.Value, Now)
            Case WorkMode.Mese
                Return New Date(Integer.Parse(anno.Text), Integer.Parse(Me.cmbMese.Text), 1)
            Case WorkMode.Undefined
                Throw New MyException("WorkMode.Undefined")
        End Select
    End Function

    Public Function _A() As Date
        Select Case _workMode
            Case WorkMode.IntervalloDaA
                If dtA.Checked = False Then
                    Return Date.MinValue
                End If
                Return Me.dtA.Value
            Case WorkMode.NumeroGiorni
                Return DateAdd(DateInterval.Day, Me.upDownNumGiorni.Value, Now)
            Case WorkMode.Mese
                Dim t As Date
                t = New Date(Integer.Parse(anno.Text), Integer.Parse(Me.cmbMese.Text), 1)
                'mese successivo
                t = DateAdd(DateInterval.Month, 1, t)
                Return DateAdd(DateInterval.Day, -1, t)
            Case WorkMode.Undefined
                Throw New MyException("WorkMode.Undefined")
        End Select
    End Function



    Private Sub upDownNumGiorni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles upDownNumGiorni.Click
        initControls(WorkMode.NumeroGiorni)
    End Sub

    Private Sub cmbMese_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMese.Click
        initControls(WorkMode.Mese)
    End Sub

    Private Sub anno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles anno.Click
        initControls(WorkMode.Mese)
    End Sub

    Private Sub dtDa_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtDa.MouseUp
        If dtDa.Enabled Then
            initControls(WorkMode.IntervalloDaA)
        End If

    End Sub

    Private Sub dtA_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtA.MouseUp
        If dtA.Enabled Then
            initControls(WorkMode.IntervalloDaA)
        End If
    End Sub

    Private Sub anno_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles anno.Leave
        Dim currentYear = Date.Now.Year
        ErrorProvider1.SetIconAlignment(anno, ErrorIconAlignment.TopLeft)
        ErrorProvider1.SetError(anno, "")

        If (Integer.Parse(anno.Text) < Date.MinValue.Year) OrElse (Integer.Parse(anno.Text) > Date.MaxValue.Year) Then
            ErrorProvider1.SetError(anno, "Anno non valido")
        End If
    End Sub

    Private Sub labelMese_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelMese.Click
        initControls(WorkMode.Mese)
    End Sub

    Private Sub labelGiorni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelGiorni.Click
        initControls(WorkMode.NumeroGiorni)
    End Sub

    Private Sub labelPeriodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPeriodo.Click
        initControls(WorkMode.IntervalloDaA)
    End Sub


    Public Sub _setToolTipLabel(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.labelPeriodo, value)
        Me.ToolTip1.SetToolTip(Me.labelGiorni, value)
        Me.ToolTip1.SetToolTip(Me.labelMese, value)
    End Sub


    Public Sub _setToolTipLabelPeriodo(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.labelPeriodo, value)
    End Sub

    Public Sub _setToolTipLabelNumeroGiorni(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.labelGiorni, value)
    End Sub

    Public Sub _setToolTipLabelMeseCompetenza(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.labelMese, value)
    End Sub


End Class
