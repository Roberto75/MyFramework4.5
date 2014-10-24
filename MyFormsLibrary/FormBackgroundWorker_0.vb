Public Class FormBackgroundWorker_0

    Friend _worker As System.ComponentModel.BackgroundWorker
    Friend _dateStart As Date
    Friend _dateEnd As Date
    

    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        MyBase._init(connection, statusBar, progressBar)

        _addButtonToToolStrip("Status", "Status", MyControlsLibrary.My.Resources.MyResource.connecting_16x16, ToolStripItemAlignment.Right)
        Me.MyToolStrip3.Items("Status").Visible = False
        Return True
    End Function
   


   




#Region "Thread"
    Friend Overridable Sub workerStart(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        'lettura parametri
        Dim sleepTimeTotal As Long
        sleepTimeTotal = CLng(e.Argument)

        Dim oneSecond As Integer = 0
        Dim conta As Integer = 0

        Dim localWorker As System.ComponentModel.BackgroundWorker
        localWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        _dateStart = Now
        _consoleWriteLine("Start: " & _dateStart)

        'Dim dateStartTask As Date
        'Dim dateEndTask As Date

        While (True)
            If localWorker.CancellationPending Then
                e.Cancel = True
                Exit Sub
            Else

                If (oneSecond Mod sleepTimeTotal) = 0 Then
                    Me.MyToolStrip3.Items("Status").Visible = True
                    conta = conta + 1
                    Me._statusBarUpdate(String.Format("Execute task n° {0:N0}", conta))


                    _consoleWriteLine(String.Format(vbCrLf & "Start task: n° {0:N0}", conta) & vbTab & Now.ToString)
                    'eseguo il codice ....
                    _executeTask()
                    _consoleWriteLine(String.Format("End task: n° {0:N0}", conta) & vbTab & Now.ToString)

                    oneSecond = 0
                    Me.MyToolStrip3.Items("Status").Visible = False
                End If

                'un secondo alla volta 
                System.Threading.Thread.Sleep(1000)

                oneSecond = oneSecond + 1
                Me._statusBarUpdate(String.Format("Waiting ...  {0:N0}/{1:N0}", oneSecond, sleepTimeTotal))
            End If
        End While

        e.Result = "OK"

    End Sub

    Friend Overridable Sub workerFinisched(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Dim localWorker As System.ComponentModel.BackgroundWorker
        localWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        _dateEnd = Now

        _consoleWriteLine("")
        _consoleWriteLine("End: " & _dateEnd)
        '_consoleWriteLine("Stop: " & )

        If e.Cancelled Then
            ' The user canceled the operation.
            MessageBox.Show("Operation was canceled", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        ElseIf (e.Error IsNot Nothing) Then
            ' There was an error during the operation.
            Dim msg As String = String.Format("An error occurred: {0}", e.Error.Message)
            MessageBox.Show(msg, My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            ' The operation completed normally.

        End If

        _worker = Nothing
        Me._statusBarUpdate("")
        '  Me.MyToolStrip3.Items("Play").Image = MyControlsLibrary.My.Resources.MyResource.stop_16x16
    End Sub

    Friend Overridable Sub workerProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        Dim localWorker As System.ComponentModel.BackgroundWorker
        localWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        '        Me.btnAutoCheck.Text = String.Format("AUTO ({0:N0})", e.ProgressPercentage)
    End Sub

    Public Sub _processInBackground(ByVal sleepTimeInSeconds As Long)
        If _worker Is Nothing Then
            _worker = New System.ComponentModel.BackgroundWorker

            AddHandler _worker.DoWork, AddressOf Me.workerStart
            AddHandler _worker.RunWorkerCompleted, AddressOf Me.workerFinisched
            AddHandler _worker.ProgressChanged, AddressOf Me.workerProgressChanged

            _worker.WorkerSupportsCancellation = True
            _worker.WorkerReportsProgress = True

            _worker.RunWorkerAsync(sleepTimeInSeconds)
        Else
            Throw New ApplicationException("Worker Auto Check process just running")
            'If Not _workerAutoCheck.CancellationPending Then
            'Me.btnAutoCheck.BackColor = Color.Gray
            '_workerAutoCheck.CancelAsync()
            'End If
        End If
    End Sub




    Overridable Sub _executeTask()
        System.Threading.Thread.Sleep(5000)
    End Sub

    Sub _cancelTask()
        If Not _worker Is Nothing AndAlso Not _worker.CancellationPending Then
            _worker.CancelAsync()
        End If
    End Sub


#End Region



    Private Delegate Sub delegateAddLogInformation(ByVal value As String)
    Public Sub _consoleWriteLine(ByVal value As String)
        If Me.Owner.InvokeRequired Then
            Dim theDelegate As New delegateAddLogInformation(AddressOf Me._consoleWriteLine)
            Me.Owner.Invoke(theDelegate, New Object() {value})
        Else
            txtLogging.Text &= value & vbCrLf
            txtLogging.Select(txtLogging.Text.Length, 0)
            txtLogging.ScrollToCaret()
        End If
    End Sub

    Public Sub _consoleWrite(ByVal value As String)
        If Me.Owner.InvokeRequired Then
            Dim theDelegate As New delegateAddLogInformation(AddressOf Me._consoleWrite)
            Me.Owner.Invoke(theDelegate, New Object() {value})
        Else
            txtLogging.Text &= value
            txtLogging.Select(txtLogging.Text.Length, 0)
            txtLogging.ScrollToCaret()
        End If
    End Sub

End Class