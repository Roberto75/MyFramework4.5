Public Class UcDataInizioDataFine

    Public Enum ControlloLarghezza
        Esteso
        Corto
    End Enum

    Public Sub _showCheckBox()
        dtA.ShowCheckBox = True
        dtDa.ShowCheckBox = True
    End Sub


    Public Sub _init(Optional ByVal formato As DateTimePickerFormat = DateTimePickerFormat.Long, _
                     Optional ByVal larghezza As ControlloLarghezza = ControlloLarghezza.Esteso)


        'Per poter inpostare i vlaori ai controlli devono essere ceccati!!
        dtDa.Checked = True
        dtA.Checked = True

        Label1.Text = "Data inizio:"
        Label2.Text = "Data fine:"

        dtDa.Format = formato
        dtA.Format = formato

        dtA.Value = Date.Now
        '        dtA.Refresh()


        dtDa.Value = New Date(Now.Year, Now.Month, 1)
        '       dtDa.Refresh()


        dtA.ShowCheckBox = False
        dtDa.ShowCheckBox = False


        Select Case larghezza
            Case ControlloLarghezza.Corto
                dtA.Width = 100
                dtDa.Width = 100
            Case ControlloLarghezza.Esteso
                dtA.Width = 183
                dtDa.Width = 183
        End Select

        Me.Invalidate()
    End Sub


    Public Function _getDataDa() As Date
        If dtDa.ShowCheckBox AndAlso dtDa.Checked = False Then
            Return Date.MinValue
        End If
        Return dtDa.Value
    End Function

    Public Function _getDataA() As Date
        If dtA.ShowCheckBox AndAlso dtA.Checked = False Then
            Return Date.MinValue
        End If
        Return dtA.Value
    End Function


    Public Sub _setDataDa(ByVal dataValue As Date, ByVal checkboxValue As Boolean)
        'dtDa.ShowCheckBox = s
        dtDa.Checked = checkboxValue
        dtDa.Value = dataValue
    End Sub

    Public Sub _setDataA(ByVal dataValue As Date, ByVal checkboxValue As Boolean)
        'dtA.ShowCheckBox = showCheckbox
        dtA.Checked = checkboxValue
        dtA.Value = dataValue
    End Sub


    Public Function _checkGreatAndEqual() As Boolean
        '-------------------------------------------
        'Tipo di valore   |  Condizione            |
        '-------------------------------------------
        'Minore di zero   |  t1 è precedente a t2. |
        'Zero             |  t1 è uguale a t2.     |
        'Maggiore di zero |  t1 è successivo a t2. |
        '-------------------------------------------

        Me.ErrorProvider1.SetError(Me.dtA, "")
        Me.ErrorProvider1.SetError(Me.dtDa, "")

        If _getDataDa() = Date.MinValue Then
            Me.ErrorProvider1.SetError(Me.dtDa, "Valorizzare la data inizio")
            Return False
        End If


        If _getDataA() = Date.MinValue Then
            Return True
        End If


        If Date.Compare(_getDataDa(), _getDataA()) > 0 Then
            Me.ErrorProvider1.SetError(Me.dtA, "Data fine deve essere maggiore o uguale della data inizio")
            Return False
        End If

        Return True

    End Function

    Public Function _checkIsCheckedA() As Boolean
        Me.ErrorProvider1.SetError(Me.dtA, "")
        If dtA.Checked = False Then
            Me.ErrorProvider1.SetError(Me.dtA, "Valorizzare la data fine")
            Return False
        End If
        Return True
    End Function




    Public Function _checkGreat() As Boolean
        Me.ErrorProvider1.SetError(Me.dtA, "")
        Me.ErrorProvider1.SetError(Me.dtDa, "")

        If _getDataDa() = Date.MinValue Then
            Me.ErrorProvider1.SetError(Me.dtDa, "Valorizzare la data inizio")
            Return False
        End If


        If _getDataA() = Date.MinValue Then
            Return True
        End If


        If Date.Compare(_getDataDa(), _getDataA()) >= 0 Then
            Me.ErrorProvider1.SetError(Me.dtA, "Data fine deve essere maggiore della data inizio")
            Return False
        End If

        Return True

    End Function


    Public Function _getIntervallo(ByVal TipoIntervallo As DateInterval) As Long
        Return DateDiff(TipoIntervallo, dtDa.Value, dtA.Value)
    End Function


    Public Function _isCheckedDataDa() As Boolean
        Return dtDa.Checked
    End Function

    Public Function _isCheckedDataA() As Boolean
        Return dtA.Checked
    End Function

End Class
