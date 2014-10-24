Public Class FormBaseDetailWithLog

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


    Public Overrides Function _checkFieldBeforeSaveDetailOnClick(ByVal tab As System.Windows.Forms.TabPage) As Boolean
        If tab.Name = "TabPage1" Then
            Return True
        End If
        Return MyBase._checkFieldBeforeSaveDetailOnClick(tab)
    End Function




    Protected Overrides Function _btnSaveDetailOnClick(ByVal tab As System.Windows.Forms.TabPage) As Boolean
        If tab.Name = "TabPage1" Then
            Dim SaveFileDialog1 As New Windows.Forms.SaveFileDialog
            SaveFileDialog1.Filter = "Text files (*.txt)|*.txt"
            'setting filters so that Text files and All Files choice appears in the Files of Type box
            'in the dialog
            Dim dr As System.Windows.Forms.DialogResult
            dr = SaveFileDialog1.ShowDialog()

            If dr <> System.Windows.Forms.DialogResult.OK Then
                Return False
            End If

            'showDialog method makes the dialog box visible at run time
            Dim outputFileName As String
            outputFileName = SaveFileDialog1.FileName

            IO.File.WriteAllText(outputFileName, Me.txtLogging.Text)
            Return True
        End If

        Return MyBase._btnSaveDetailOnClick(tab)
    End Function

End Class