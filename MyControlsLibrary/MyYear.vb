Public Class MyYear
    Inherits Windows.Forms.NumericUpDown

    Public Sub New()
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Width = 70

        Me.Minimum = Date.MinValue.Year
        Me.Maximum = Date.MaxValue.Year
        Me.Value = Now.Year
    End Sub

End Class
