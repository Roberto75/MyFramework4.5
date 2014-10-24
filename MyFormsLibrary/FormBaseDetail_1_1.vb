Public Class FormBaseDetail_1_1
    Inherits MyFormsLibrary.FormBaseDetail_1

    Public Overridable Function _checkFieldBeforeSaveOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function checkFieldBeforeSaveOnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Function


    Public Overridable Function _buttonSaveOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function buttonSaveOnClick not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not _checkFieldBeforeSaveOnClick() Then
            Exit Sub
        End If

        If System.Windows.Forms.MessageBox.Show("Confermare il salvataggio delle modifiche?", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If

        Me._cursorWait()

        If _buttonSaveOnClick() Then
            Me._cursorDefault()
            System.Windows.Forms.MessageBox.Show("Salvataggio concluso con successo.", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        Else
            Me._cursorDefault()
        End If
    End Sub


End Class