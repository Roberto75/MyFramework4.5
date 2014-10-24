Public Class FormLog

    Public Function _init(ByVal file As IO.FileInfo) As Boolean
        Trace.Close()
        Dim lines() As String = System.IO.File.ReadAllLines(file.FullName)

        For Each log As String In lines
            ListView1.Items.Add(log)
        Next
    End Function





    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class