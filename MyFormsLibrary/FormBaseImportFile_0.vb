Public Class FormBaseImportFile_0

    Public _pathInputFile As String = ""

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim OpenFileDialog1 As New Windows.Forms.OpenFileDialog
        OpenFileDialog1.Filter = "Excel (*.xls;*.xlsx))|*.xls;*.xlsx"

        OpenFileDialog1.InitialDirectory = Application.ExecutablePath
        OpenFileDialog1.Title = "Caricamento File"
        'OpenFileDialog1.FileName = "source.xls"

        If (OpenFileDialog1.ShowDialog(tabPageMain) = Windows.Forms.DialogResult.OK) Then

            'If Windows.Forms.MessageBox.Show("Procedere con il caricamento del file?", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OKCancel, Windows.Forms.MessageBoxIcon.Question) <> Windows.Forms.DialogResult.OK Then
            '         Exit Sub
            '    End If
            Me.txtPathSource.Text = OpenFileDialog1.FileName
            Me._pathInputFile = OpenFileDialog1.FileName

            Application.DoEvents()

            _importFileSource(OpenFileDialog1.FileName)

        End If
    End Sub


    Overridable Function _importFileSource(ByVal pathInputFile As String) As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

End Class