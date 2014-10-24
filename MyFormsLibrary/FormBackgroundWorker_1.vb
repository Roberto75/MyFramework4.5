Public Class FormBackgroundWorker_1



    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        MyBase._init(connection, statusBar, progressBar)
        _addButtonToToolStrip("Play", "Play", MyControlsLibrary.My.Resources.MyResource.next_16x16, ToolStripItemAlignment.Left)
        _addButtonToToolStrip("Status", "Status", MyControlsLibrary.My.Resources.MyResource.connecting_16x16, ToolStripItemAlignment.Right)
        Me.MyToolStrip3.Items("Status").Visible = False
        Return True
    End Function

    Public Overrides Function _toolStripButtonClicked(ByVal myButton As System.Windows.Forms.ToolStripButton) As Boolean
        Select Case myButton.Name
            Case "Play"
                If _worker Is Nothing Then
                    myButton.Image = MyControlsLibrary.My.Resources.MyResource.stop_16x16

                    _buttonPlayBeforeStart()
                Else
                    If Not _worker Is Nothing AndAlso Not _worker.CancellationPending Then
                        _worker.CancelAsync()
                    End If
                End If
        End Select
        Return True
    End Function



    Overridable Sub _buttonPlayBeforeStart()
        'passo il tempo espresso in secondi tra un'attività e l'altra
        _processInBackground(7)
    End Sub


End Class