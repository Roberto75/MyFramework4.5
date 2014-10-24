Public Class FormBaseSearch_3
    Inherits MyFormsLibrary.FormBaseDetail_3


    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        'Me.UcPreviewDataTable1.Visible = False
        Return MyBase._init(connection, statusBar, progressBar)
    End Function


    Public Sub _hideButtonReset()
        Me.btnReset.Visible = False
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor
        _btnResetOnClick()
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerca.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor

        Me.UcPreviewDataTable1._clear()

        If _doSearch() Then
            Me.UcPreviewDataTable1.Visible = True
        End If
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default

    End Sub

    Protected Overridable Function _doSearch() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Protected Overridable Function _btnResetOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Public Function _btnSearchOnClick() As Boolean
        btnCerca_Click(Me.btnCerca, Nothing)
        Return True
    End Function



End Class