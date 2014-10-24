Public Class FormBaseSearch_0
    Inherits MyFormsLibrary.FormBaseDetail_2


    Public Sub _showButtonAdd()
        Me.btnAdd.Visible = True
    End Sub

    Public Sub _hideButtonAdd()
        Me.btnAdd.Visible = False
    End Sub


   

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor
        _buttonSearchOnClick()
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor
        _buttonResetOnClick()
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        _buttonAddOnClick()
    End Sub


    Public Overridable Function _buttonResetOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Public Overridable Function _buttonAddOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


    Public Overridable Function _buttonSearchOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function
End Class