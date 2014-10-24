Public Class FormBaseSearch_2
    Inherits MyFormsLibrary.FormBaseSearch_0

    Public _mySearchDataTable As Data.DataTable

    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        Me.UcPreviewDataTable1.Visible = True
        Return MyBase._init(connection, statusBar, progressBar)
    End Function

    Public Overrides Function _buttonSearchOnClick() As Boolean
        If Not _doSearch() Then
            If Not _statusBar Is Nothing Then
                _statusBar.Text = String.Empty
            End If
            Return False
        End If

        Me.UcPreviewDataTable1._clear()

        If Me._mySearchDataTable.Rows.Count = 0 Then
            System.Windows.Forms.MessageBox.Show("Per i criteri di ricerca impostati non è stato trovato nessun risualto. Modificare i valori dei filtri.", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
            If Not _statusBar Is Nothing Then
                _statusBar.Text = String.Empty
            End If
            Return False
        End If

        Me.UcPreviewDataTable1._init(Me._mySearchDataTable)
        If Not _statusBar Is Nothing Then
            _statusBar.Text = String.Empty
        End If
        Return True
    End Function

    Protected Overridable Function _doSearch() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Public Sub _setCursor(ByVal cur As System.Windows.Forms.Cursor)
        Me.FindForm().Cursor = cur
        Me.Cursor = cur
        Me.UcPreviewDataTable1.Cursor = cur
        Me.panel1.Cursor = cur
        Me.Panel2.Cursor = cur
        Me.tabPageMain.Cursor = cur
        Me.TabControl1.Cursor = cur
        Application.DoEvents()
    End Sub
End Class