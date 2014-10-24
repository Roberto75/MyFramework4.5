Public Class UcTextBoxChooser

    Private dt As Data.DataTable

    Public Sub _Init(ByVal dataTable As Data.DataTable, _
                     ByVal displayValue As String, _
                     ByVal valueMember As String)

        dt = dataTable

        TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource
        TextBox1.AutoCompleteMode = AutoCompleteMode.Suggest

        Dim atc As New AutoCompleteStringCollection()
        Dim dt1 As Data.DataTable = dt

        Label2.Text = dt1.Rows.Count & " elementi trovati"

        For Each dtr As Data.DataRow In dt1.Rows
            Try
                atc.Add(dtr(displayValue).ToString)
            Catch
                'utilizzo il try catch perchè è più veloce che
                'testare il dbnull su ogni riga.
                'quindi se la riga và in errore la salto
            End Try
        Next

        TextBox1.AutoCompleteCustomSource = atc

        Dim arrAlfabeto() As String = Split("A, B, C, D, E, F, G, H, I, L, M, N, O, P, Q, R, S, T, U, V, Z, Y, W, J, K, X", ",")

        For Each s1 As String In arrAlfabeto
            If dt1.Select(displayValue & " like '" & s1.Trim & "%'").Length > 0 Then
                ContextMenuStrip1.Items.Add(s1)
            End If
        Next



        '

    End Sub


    Public Sub _setLabelDesc(ByVal st As String)
        Label1.Text = st
    End Sub


    Private Sub UcTextBoxChooser_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics

        Dim pen As New Pen(Color.CornflowerBlue, 2.0)

        g.DrawRectangle(pen, New Rectangle(TextBox1.Location, TextBox1.Size))

        pen.Dispose()
    End Sub

   


    Private Sub RectangleShape1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

   

    Private Sub ContextMenuStrip1_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ContextMenuStrip1.ItemClicked

        TextBox1.Text = e.ClickedItem.Text.Trim

    End Sub
End Class
