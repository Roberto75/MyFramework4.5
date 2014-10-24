Public Class UcGiornoMeseAnno
    Public Enum WorkMode
        Giorno
        Mese
        Anno
    End Enum

    Private _workMode As WorkMode = WorkMode.Anno
    Public ReadOnly Property MyWorkMode() As WorkMode
        Get
            Return _workMode
        End Get
    End Property



    Private _hideGiorno As Boolean = False
    Public Property MyHideGiorno() As Boolean
        Get
            Return _hideGiorno
        End Get
        Set(ByVal value As Boolean)
            _hideGiorno = value

            If _hideGiorno = True Then
                Me.giorno.Visible = False
                Me.lblGiorno.Visible = False
            Else
                Me.giorno.Visible = True
                Me.lblGiorno.Visible = True
            End If
        End Set
    End Property


    Private _hideAnno As Boolean = False
    Public Property MyHideAnno() As Boolean
        Get
            Return _hideAnno
        End Get
        Set(ByVal value As Boolean)
            _hideAnno = value

            If _hideAnno = True Then
                Me.anno.Visible = False
                Me.lblAnno.Visible = False
            Else
                Me.anno.Visible = True
                Me.lblAnno.Visible = True
            End If
        End Set
    End Property





    Private _enableClickOnLabel As Boolean

    Public Function _init(ByVal mode As WorkMode, ByVal enabeClickOnLabel As Boolean) As Boolean
        _workMode = mode
        _enableClickOnLabel = enabeClickOnLabel
        _hideGiorno = False
        Me.giorno.Value = Now
        Me.anno.Text = Now.ToString("yyyy")
        Me.mese.SelectedItem = Now.ToString("MM")
        initControls()
        Return True
    End Function

    Private Sub initControls()
        Dim f As System.Drawing.Font
        f = GroupBox1.Font

        Select Case _workMode
            Case WorkMode.Giorno
                Me.giorno.Enabled = True
                Me.giorno.Checked = True
                Me.mese.Enabled = False
                Me.anno.Enabled = False

                If _enableClickOnLabel Then
                    Me.lblGiorno.Font = New Font(f, FontStyle.Bold Or FontStyle.Underline)
                Else
                    Me.lblGiorno.Font = New Font(f, FontStyle.Bold Or FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.labelMese.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.labelMese.Font = New Font(f, FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.lblAnno.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.lblAnno.Font = New Font(f, FontStyle.Regular)
                End If

            Case WorkMode.Mese
                Me.giorno.Enabled = False
                Me.mese.Enabled = True
                Me.anno.Enabled = True

                If _enableClickOnLabel Then
                    Me.lblGiorno.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.lblGiorno.Font = New Font(f, FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.labelMese.Font = New Font(f, FontStyle.Bold Or FontStyle.Underline)
                Else
                    Me.labelMese.Font = New Font(f, FontStyle.Bold Or FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.lblAnno.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.lblAnno.Font = New Font(f, FontStyle.Regular)
                End If


            Case WorkMode.Anno
                Me.giorno.Enabled = False
                Me.mese.Enabled = False
                Me.anno.Enabled = True

                If _enableClickOnLabel Then
                    Me.lblGiorno.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.lblGiorno.Font = New Font(f, FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.labelMese.Font = New Font(f, FontStyle.Underline)
                Else
                    Me.labelMese.Font = New Font(f, FontStyle.Regular)
                End If

                If _enableClickOnLabel Then
                    Me.lblAnno.Font = New Font(f, FontStyle.Bold Or FontStyle.Underline)
                Else
                    Me.lblAnno.Font = New Font(f, FontStyle.Bold Or FontStyle.Regular)
                End If
        End Select

        If _enableClickOnLabel Then
            Me.lblGiorno.Cursor = Cursors.Hand
            Me.labelMese.Cursor = Cursors.Hand
            Me.lblAnno.Cursor = Cursors.Hand
        Else
            Me.lblGiorno.Cursor = Cursors.Default
            Me.labelMese.Cursor = Cursors.Default
            Me.lblAnno.Cursor = Cursors.Default
        End If

    End Sub


    Public Function _getAnno() As Integer
        Return Me.anno.Value
    End Function

    Public Function _getMese() As Integer
        Return Me.mese.SelectedItem
    End Function

    Public Function _getGiorno() As Date
        Return Me.giorno.Value
    End Function

    Public Sub _setGoupBoxLabel(ByVal value As String)
        Me.GroupBox1.Text = value
    End Sub



    Public Sub _setToolTipLabel(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.lblGiorno, value)
        Me.ToolTip1.SetToolTip(Me.labelMese, value)
        Me.ToolTip1.SetToolTip(Me.lblAnno, value)
    End Sub


    Public Sub _setToolTipLabelGiorno(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.lblGiorno, value)
    End Sub

    Public Sub _setToolTipLabelAnno(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.lblAnno, value)
    End Sub

    Public Sub _setToolTipLabelMese(ByVal value As String)
        Me.ToolTip1.SetToolTip(Me.labelMese, value)
    End Sub


    Public Shared Function _decodeMese(ByVal value As Integer) As String
        Select Case value
            Case 1
                Return "Gennaio"
            Case 2
                Return "Febbraio"
            Case 3
                Return "Marzo"
            Case 4
                Return "Aprile"
            Case 5
                Return "Maggio"
            Case 6
                Return "Giugno"
            Case 7
                Return "Luglio"
            Case 8
                Return "Agosto"
            Case 9
                Return "Settembre"
            Case 10
                Return "Ottobre"
            Case 11
                Return "Novembre"
            Case 12
                Return "Dicembre"
            Case Else
                Throw New ApplicationException("_decode mese non valido: " & value)
        End Select
    End Function


    Private Sub lblAnno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAnno.Click
        If Not _enableClickOnLabel Then
            Exit Sub
        End If
        Me._workMode = WorkMode.Anno
        Me.initControls()
    End Sub

    Private Sub labelMese_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelMese.Click
        If Not _enableClickOnLabel Then
            Exit Sub
        End If
        Me._workMode = WorkMode.Mese
        Me.initControls()
    End Sub

    Private Sub lblGiorno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGiorno.Click
        If Not _enableClickOnLabel Then
            Exit Sub
        End If
        Me._workMode = WorkMode.Giorno
        Me.initControls()
    End Sub
End Class
