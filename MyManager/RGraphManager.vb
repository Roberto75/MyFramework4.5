Public Class RGraphManager
    Inherits Manager

    Public Enum Report
        DisplayLCD
        Tabella
        AngularGauge
        Area
        Column
        Line
        Bar
        HBar
        Pie
    End Enum


    Public Enum MyLabel
        None
        Label
        Valore
        LabelAndValore
    End Enum

    Public _returnTotale As Long = 0
    Public _dt As Data.DataTable


    Public _showLegend As Boolean = False
    Public _showLabels As MyLabel = MyLabel.None
    Public _showPercentuale As Boolean = True

    Public _orderColor As Boolean = True
    Public _titolo As String = ""

    Public _enableOnClick As Boolean = False




    Public _palette1 As New List(Of String)(New String() {"ffc363", "eb6033", "307ea9", "d1ccd1", "425c86" _
                                                         , "63a1fa", "92CD00", "2C6700", "ffe384", "ce694a" _
                                                         , "005dde", "6b7d94", "336699", "FFFFFF", "003366" _
                                                         , "996633", "666633", "E5E4D7", "CCCC99", "990033"})

    Public _palette2 As New List(Of String)(New String() {"1D8BD1", "F1683C", "2AD62A", "DBDC25", "8FBC8B", "D2B48C", "FAF0E6", "20B2AA", "B0C4DE", "DDA0DD", "9C9AFF", "9C3063", "FFFFCE" _
                                                          , "CEFFFF", "630063", "FF8284", "0065CE", "CECFFF", "000084", "FF00FF", "FFFF00", "00FFFF", "840084", "840000", "008284", "0000FF" _
                                                          , "00CFFF", "CEFFFF", "CEFFCE", "FFFF9C", "9CCFFF", "FF9ACE", "CE9AFF", "FFCF9C", "3165FF", "31CFCE", "9CCF00", "FFCF00", "FF9A00" _
                                                          , "FF6500"})


    Public _palette As List(Of String) = _palette1

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function getChart(name As String, ByVal tipo As RGraphManager.Report, ByVal sqlQuery As String, _
                                Optional ByVal rotateDataSet As Boolean = False, _
                                Optional ByVal includeTable As Boolean = False) As String


        'eseguo la query
        Dim dataSet As System.Data.DataSet
        dataSet = _fillDataSet(sqlQuery)

        Return getChart(name, tipo, dataSet.Tables(0), rotateDataSet, includeTable)
    End Function



    Public Function getChart(name As String, ByVal tipo As RGraphManager.Report, ByVal dataTable As DataTable, _
                                   Optional ByVal rotateDataSet As Boolean = False, _
                             Optional ByVal includeTable As Boolean = False) As String

        'If isIndicatore(tipo) Then
        '    Throw New MyManager.ManagerException("Con questi parametri NON è possibile creare report di tipo indicatore")
        'End If

        'If (rotateDataSet) Then
        '    dataTable = rotateRigheToColonne(dataTable)
        'End If


        _dt = dataTable


        Dim indexColor As Int16 = 0

        Dim strJavaScript As String = ""
        Dim strData As String = ""
        Dim strColors As String = ""
        Dim strLegend As String = ""
        Dim strLabel As String = ""

        Dim strMyKeys As String = ""


        Dim totale As Decimal = 0
        Dim paletteColors As Int16

        paletteColors = _palette.Count

        Select Case tipo
            Case Report.Pie
                strData = String.Format("var {0} = new RGraph.Pie('{0}', [", name)
            Case Report.HBar
                strData = String.Format("var {0} = new RGraph.HBar('{0}', [", name)


            Case Else
                Throw New MyManager.ManagerException("tipo di report non gestito: " & tipo.ToString)
        End Select


        strColors = String.Format("{0}.Set('chart.colors', [", name)
        strLegend = String.Format("{0}.Set('chart.key', [", name)
        strLabel = String.Format("{0}.Set('chart.labels', [", name)

        ' La varibile la dichiaro fuopri altrimenti non si vede
        'If dataTable.Columns.Contains("my_key") Then
        'strMyKeys = String.Format("var myKey{0} = new Array(", name)
        'End If


        Dim indice As Long = 0


        For Each row As Data.DataRow In dataTable.Rows
            strData &= row("valore") & ","

            strLegend &= String.Format("'{0}',", row("label").ToString().Replace("'", "\'"))

            Select Case _showLabels
                Case MyLabel.None
                Case MyLabel.Label
                    strLabel &= String.Format("'{0}',", row("label").ToString().Replace("'", "\'"))
                Case MyLabel.Valore
                    strLabel &= String.Format("'{0}',", row("valore").ToString().Replace("'", "\'"))
                Case MyLabel.LabelAndValore
                    strLabel &= String.Format("'{0}',", row("label").ToString().Replace("'", "\'") & " (" & row("valore").ToString().Replace("'", "\'") & ")")
            End Select


            If _orderColor Then
                strColors &= String.Format("'{0}',", row("label").ToString().Replace("'", "\'"))
            Else
                strColors &= String.Format("'#{0}'", _palette(indexColor Mod paletteColors))
                indexColor = indexColor + 1
            End If

            totale = totale + Decimal.Parse(row("valore"))


            If dataTable.Columns.Contains("my_key") Then
                'strMyKeys &= String.Format("'{0}',", row("my_key").ToString().Replace("'", "\'"))
                strMyKeys &= String.Format(" myKey{0}[{1}] = '{2}';", name, indice, row("my_key").ToString().Replace("'", "\'"))
                indice = indice + 1
            End If

        Next


        _returnTotale = totale


        If _orderColor Then
            dataTable.DefaultView.Sort = "valore desc"
            indexColor = 0

            For Each row As Data.DataRowView In dataTable.DefaultView
                strColors = strColors.Replace(String.Format("'{0}'", row("label").ToString().Replace("'", "\'")), String.Format("'{0}'", "#" & _palette(indexColor Mod paletteColors)))

                indexColor = indexColor + 1
            Next
        End If




        'cancello l'ultima ,
        strData = strData.Substring(0, strData.Length - 1)
        strData &= "]);"

        strColors = strColors.Substring(0, strColors.Length - 1)
        strColors &= "]);"

        strLegend = strLegend.Substring(0, strLegend.Length - 1)
        strLegend &= "]);"

        strLabel = strLabel.Substring(0, strLabel.Length - 1)
        strLabel &= "]);"

        strJavaScript = strData & vbCrLf & strColors


        If _showPercentuale Then
            For Each row As Data.DataRow In dataTable.Rows
                strLabel = strLabel.Replace(String.Format("'{0}'", row("label").ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row("label").ToString().Replace("'", "\'"), (row("valore") / totale) * 100))
                strLegend = strLegend.Replace(String.Format("'{0}'", row("label").ToString().Replace("'", "\'")), String.Format("'{0} {1:N2}%'", row("label").ToString().Replace("'", "\'"), (row("valore") / totale) * 100))
            Next
        End If



        If _showLegend Then
            strJavaScript &= vbCrLf & strLegend
            strJavaScript &= String.Format("{0}.Set('chart.key.background', 'white');", name)
        End If

        If _showLabels Then
            strJavaScript &= vbCrLf & strLabel
        End If



        ' strJavaScript &= String.Format("{0}.Set('chart.gutter.left', 400);", name)
        strJavaScript &= String.Format("{0}.Set('chart.text.size', 8);", name)
        'strJavaScript &= String.Format("{0}.Set('chart.colors.sequential', true);", name)


        'strJavaScript &= String.Format("{0}.Set('chart.labels.above', true);", name)

        'strJavaScript &= String.Format("{0}.Set('chart.zoom.factor',1.5);", name)
        strJavaScript &= String.Format("{0}.Set('chart.key.interactive', true);", name)


        If Not String.IsNullOrEmpty(_titolo) Then
            strJavaScript &= String.Format("{0}.Set('chart.title', '{1}');", name, _titolo.Replace("'", "\'"))
        End If



        If _enableOnClick Then
            strJavaScript &= String.Format("{0}.Set('chart.events.click', {1});", name, "myEventListener" & name)
            strJavaScript &= String.Format("{0}.Set('chart.events.mousemove', function (e, bar) {{e.target.style.cursor = 'pointer';}} );", name)
            'strJavaScript &= String.Format("{0}.Set('chart.events.mousemove', myMousemove );", name)
        End If

        'strJavaScript &= String.Format("{0}.Set('chart.key.position', 'gutter');", name)
        strJavaScript &= String.Format("{0}.Draw();", name) & vbCrLf


        If dataTable.Columns.Contains("my_key") Then
            ' strMyKeys = strMyKeys.Substring(0, strMyKeys.Length - 1)
            'strMyKeys &= ");"
            strJavaScript &= strMyKeys & vbCrLf
        End If

        Return strJavaScript
    End Function
End Class
