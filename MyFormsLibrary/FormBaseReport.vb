Public Class FormBaseReport
    Inherits MyFormsLibrary.FormBaseDetail_3

    Protected _myDataTable As DataTable


    Public Sub _hideButtonReset()
        Me.btnReset.Visible = False
    End Sub


    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.WaitCursor
        CType(Me.Owner, FormMaster).MyStatusBar.Text = "Elaborazione in corso ..."
        System.Windows.Forms.Application.DoEvents()

        _fillMyDataTable()

        If (Me._myDataTable Is Nothing) OrElse Me._myDataTable.Rows.Count = 0 Then
            System.Windows.Forms.MessageBox.Show("Attenzione il report non ha generato alcun risualto. Modificare i valori dei filtri.", My.Application.Info.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error)
        Else
            _ResetTabPageDetailStatus()
            'Me.UcPreviewDataTable1._setProgressBar(CType(Me.Owner, FormMaster).MyProgressBar)
            'Me.UcPreviewDataTable1._setStatusBar(CType(Me.Owner, FormMaster).MyStatusBar)
            Me.UcPreviewDataTable1._init(_myDataTable)


            If creaExel.Checked Then
                _MakeFileExcelFromMyDataTable()
            End If
            If creaXML.Checked Then
                Me.UcPreviewDataTable1._saveToXML()
            End If
            If creaCSV.Checked Then
                Me.UcPreviewDataTable1._saveToCSV()
            End If
        End If

        CType(Me.Owner, FormMaster).MyStatusBar.Text = ""
        CType(sender, System.Windows.Forms.Button).Cursor = Windows.Forms.Cursors.Default
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        _buttonResetOnClick()
    End Sub

    Overridable Function _buttonResetOnClick() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function

    Overridable Function _fillMyDataTable() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        _myDataTable = Nothing
        Return False
    End Function

    Overridable Function _MakeFileExcelFromMyDataTable() As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


End Class