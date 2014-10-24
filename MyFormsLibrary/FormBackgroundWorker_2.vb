Public Class FormBackgroundWorker_2


    Private SLEEP_TIME_OUT As Integer = 300

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        If _worker Is Nothing Then
            Me._processInBackground(SLEEP_TIME_OUT)
        End If

    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click

        If Not _worker Is Nothing AndAlso Not _worker.CancellationPending Then
            _worker.CancelAsync()
        End If
    End Sub
End Class