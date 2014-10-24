Public Class FormMessageBox

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Enum Tipo
        OK
        KO
        WARNING
        DOMANDA
        PERSONALIZZATO
    End Enum


    Public Sub _Init(ByVal tipoMsg As Tipo, _
                     Optional ByVal messaggio_aggiuntivo As String = "", Optional ByVal titolo As String = "")

        If String.IsNullOrEmpty(messaggio_aggiuntivo) Then
            messaggio_aggiuntivo = "Nessuna informazione aggiuntiva" & _
                    " presente per questa operazione." & vbCrLf & vbCrLf & _
                    "messaggio delle ore " & Now.ToShortTimeString
        End If

        messaggio_aggiuntivo = My.Application.Info.ProductName & _
                               vbCrLf & _
                               messaggio_aggiuntivo

        lblInfoAdd.Text = messaggio_aggiuntivo

        Select Case tipoMsg
            Case Tipo.OK
                picTipo.Image = ImageList1.Images("Ok")
                lblTitolo.Text = "Operazione conclusa con successo"

            Case Tipo.KO
                picTipo.Image = ImageList1.Images("Error")
                lblTitolo.Text = "Operazione NON conclusa con successo"

            Case Tipo.PERSONALIZZATO
                picTipo.Image = ImageList1.Images("Info")
                lblTitolo.Text = titolo

            Case Tipo.WARNING
                picTipo.Image = ImageList1.Images("Warning")
                lblTitolo.Text = titolo

            Case Tipo.DOMANDA
                picTipo.Image = ImageList1.Images("Faq")
                lblTitolo.Text = titolo
        End Select

        Me.ShowDialog()


    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Me.Close()
    End Sub
End Class