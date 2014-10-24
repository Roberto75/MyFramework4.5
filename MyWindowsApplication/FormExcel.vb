Public Class FormExcel
    Inherits MyFormsLibrary.FormBaseDetail_3

    Private _manager As MyApplicationManager
    Private _excel As MyManager.ExcelManager

	
	Public Sub New()
		Me.InitializeComponent()		
	End Sub

    Public Overrides Function _OnTabClosing() As Boolean
        _excel._closeExcel()
        Return MyBase._OnTabClosing()
    End Function

    Private Sub MyButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyButton1.Click
        _manager = New MyApplicationManager(CType(Me.Owner, FormMain).MyConnection)

        Dim dt As DataTable
        dt = _manager._fillDataTable("Select * from employee")


        If _excel Is Nothing Then
            _excel = New MyManager.ExcelManager(CType(Me.Owner, FormMain).MyConnection)
            _excel._setProgressBar(CType(Me.Owner, FormMain).MyProgressBar)
            _excel._openExcel()
        End If

        If Me.chkExcel.Checked Then
            If _excel._fill(dt) Then
                _excel._showExcel()
            End If
        End If

        UcPreviewDataTable1._init(dt)




        '  Dim wsheet As Microsoft.Office.Interop.Excel.Worksheet = CType(exl.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)
        ' wsheet.Activate()


        'Dim cn As New System.Data.OleDb.OleDbConnection
        'cn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\vb_net\MyWindowsApplication\database\db1.mdb;Persist Security Info=True"
        'Dim str As String = "Select * from employee"
        'cn.Open()
        'Dim da As New System.Data.OleDb.OleDbDataAdapter(str, cn)
        'Dim ds As New DataSet
        'da.Fill(ds, "Employee")
        'Dim dt As DataTable = ds.Tables("Employee")
        'cn.Close()
    End Sub
End Class