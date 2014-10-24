Public Class FormBaseDetail_1_2
    Inherits MyFormsLibrary.FormBaseDetail_1_1

    'ATTENZIONE
    'Se ci aggoungo il datagrid ... non riesco a mofificarlo nella form che lo erdita


    Public Overridable Function _buttonNuovoOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function buttonNuovoOnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Function
    

    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        _buttonNuovoOnClick()
    End Sub
End Class