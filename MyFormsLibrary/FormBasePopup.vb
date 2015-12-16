Public Class FormBasePopup

    Protected mConnection As Data.Common.DbConnection

    Public ReadOnly Property MyStatusBar() As Windows.Forms.ToolStripStatusLabel
        Get
            Return Me.tsStatusBar
        End Get
    End Property

    Public ReadOnly Property MyProgressBar() As Windows.Forms.ToolStripProgressBar
        Get
            Return Me.tsProgressBar
        End Get
    End Property



    Public Overridable Function _init(ByRef connection As System.Data.Common.DbConnection, ByVal titolo As String) As Boolean
        Me.Text = titolo
        If Not Me.Owner Is Nothing Then
            Me.Icon = Me.Owner.Icon
        End If
        Me.mConnection = connection
        Return True
    End Function

    Public Function _getButtonConferma() As Windows.Forms.Button
        Return btnConferma
    End Function

    Public Sub _progressBarSetValue(ByVal value As Integer)
        Me.tsProgressBar.Value = value
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _progressBarSetMaximumValue(ByVal value As Integer)
        Me.tsProgressBar.Maximum = value
    End Sub

    Public Sub _progressBarPerformStep()
        Me.tsProgressBar.PerformStep()
        Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub _statusBarUpdate(ByVal value As String)
        Me.tsStatusBar.Text = value
        Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub FormBasePopup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tsStatusBar.Text = ""
    End Sub

    Private Sub btnAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
        _buttonAnnullaOnClick()
    End Sub

    Public Overridable Function _buttonAnnullaOnClick() As Boolean
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Function

    Private Sub btnConferma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConferma.Click
        _buttonSaveOnClick()
    End Sub

    Public Overridable Function _buttonSaveOnClick() As Boolean
        If Not _checkValuesBeforeSaveAction() Then
            Return False
        End If

        If System.Windows.Forms.MessageBox.Show("Confermare l'operazione di salvataggio", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) <> Windows.Forms.DialogResult.OK Then
            Return False
        End If

        If (_saveAction()) Then
            System.Windows.Forms.MessageBox.Show("Salvataggio concluso con successo", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If

        Return True
    End Function

    Public Overridable Function _saveAction() As Boolean
        Return False
    End Function

    Public Overridable Function _checkValuesBeforeSaveAction() As Boolean
        Return True
    End Function
End Class