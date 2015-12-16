Public Class ReportFlashManager
    Inherits Manager

    Public Enum Report
        DisplayLCD
        Tabella
        AngularGauge
        Area2D
        Column2D
        Column3D
        Line2D
        Bar2D
        Pie2D
    End Enum


    'serve al componente per creare il path assoluto per accedere ai file SWF.
    Public _contestoWEB As String

    Public _titolo As String = ""
    Public _sottoTitolo As String = ""
    Public _xAxisName As String = ""
    Public _yAxisName As String = ""
    'questa stringa viene posizionata prima del numero = numberPrefix
    Public _unitaDiMisura As String = ""
    'esempio Migliaia, Centinaia = numberSuffix="M"
    Public _scalaDiMisula As String = ""
    Public _numeroCifreDecimali As Int32 = 0
    Public _width As String = "400"
    Public _height As String = "300"

    'nome del foglio di stile utilizzato per la stampa della tabella
    Public _cssClassName As String = ""

    Public _RangeList As New Generic.List(Of Range)

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub



    Private Function isIndicatore(ByVal tipo As ReportFlashManager.Report) As Boolean

        If tipo = Report.AngularGauge Then
            Return True
        End If

        Return False

    End Function


    Public Function getChart(ByVal tipo As ReportFlashManager.Report, ByVal valore As Double) As String
        If Not isIndicatore(tipo) Then
            Throw New MyManager.ManagerException("Con questi parametri è possibile creare solo report di tipo indicatore")
        End If

        'se si tratta di indicatori che usano la V2
        If (Me._height.IndexOf("%") <> -1) Or (Me._width.IndexOf("%") <> -1) Then
            Throw New MyManager.ManagerException("Non è possibile usare dimensioni in %")
        End If

        Me._height = Me._height.Replace("px", "")
        Me._width = Me._width.Replace("px", "")


        Dim xmlData As String = ""
        xmlData &= "<chart"

        xmlData &= " lowerLimit= '" & CStr(Me._RangeList(0).minValue).Replace(",", ".") & "'"
        xmlData &= " upperLimit= '" & CStr(Me._RangeList(Me._RangeList.Count - 1).maxValue).Replace(",", ".") & "'"

        If Me._unitaDiMisura <> "" Then
            xmlData &= " numberPrefix= '" & Me._unitaDiMisura & "'"
        End If

        If Me._numeroCifreDecimali >= 0 Then
            xmlData &= " decimalPrecision= '" & Me._numeroCifreDecimali & "'"
        End If

        If Me._scalaDiMisula <> "" Then
            xmlData &= " numberSuffix= '" & Me._scalaDiMisula & "'"
        End If





        'If Me._titolo <> "" Then
        '    xmlData &= " caption= '" & Me._titolo & "'"
        'End If
        'If Me._sottoTitolo <> "" Then
        '    xmlData &= " subCaption= '" & Me._sottoTitolo & "'"
        'End If




        xmlData &= " >"


        'per gli indicatori mi aspetto :
        '- i range con i relativi colori 
        '- valore 

        xmlData &= "<colorRange>"
        Dim range As Range
        For Each range In Me._RangeList
            xmlData &= " <color minValue='" & CStr(range.minValue).Replace(",", ".") & "'  maxValue='" & CStr(range.maxValue).Replace(",", ".") & "' name='" & range.name & "' code='" & range.color & "' />"
        Next
        xmlData &= "</colorRange>"


        If tipo = Report.AngularGauge Then
            xmlData &= "<dials>"
            xmlData &= "<dial value='" & CStr(valore).Replace(",", ".") & "' />"
            xmlData &= "</dials>"
        Else
            xmlData &= "<value>" & CStr(valore).Replace(",", ".") & "</value>"
        End If



        xmlData &= "</chart>"


        Dim risposta As String = ""

        risposta = RenderChartHTML(getFileSWF(tipo), "", xmlData, "ChartId", _width, _height, False)

        Return risposta
    End Function



    Public Function getDisplayLCD(ByVal tipo As ReportFlashManager.Report, ByVal sqlQuery As String) As String
        If tipo <> Report.DisplayLCD Then
            Throw New MyManager.ManagerException("Con questo metodo è possibile generare solo report di tipo DisplayLCD")
        End If


        Dim risultato As String
        risultato = mExecuteScalar(sqlQuery)


        'risultato &= "<div style=""font-size:28:px; color:  > """

        risultato &= "<span   style ="" background-color:#DCDCDC; font-size :38px; font-family :Georgia; padding :10px;"">" & risultato & "</span>"


        Return risultato
    End Function


    Private Function getTableHTML(ByVal dataTable As System.Data.DataTable) As String
        Dim codiceHTML As String = ""
        Dim numeroDiColonne As Int32 = 0

        Dim i, j As Int32

        'Dim dt As System.Data.DataTable = dataSet.Tables(0)


        codiceHTML = "<table "

        If (_cssClassName <> "") Then
            codiceHTML &= " class=""" & Me._cssClassName & """ "
        End If

        codiceHTML &= " >"

        numeroDiColonne = dataTable.Columns.Count

        codiceHTML &= " <tr >"

        For i = 0 To numeroDiColonne - 1
            codiceHTML &= " <th> " & dataTable.Columns(i).ColumnName & " </th>"
        Next
        codiceHTML &= " </tr>"

        Dim row As System.Data.DataRow


        For i = 0 To dataTable.Rows.Count - 1
            row = dataTable.Rows(i)
            codiceHTML &= " <tr class=""tr_" & i Mod 2 & """ > "
            For j = 0 To numeroDiColonne - 1
                codiceHTML &= " <td>" & row.Item(j).ToString & " </td>"
            Next
            codiceHTML &= " </tr>"
        Next

        codiceHTML &= "</table> "


        Return codiceHTML
    End Function


    Public Function getChart(ByVal tipo As ReportFlashManager.Report, ByVal sqlQuery As String, _
                Optional ByVal rotateDataSet As Boolean = False, _
                Optional ByVal includeTable As Boolean = False) As String


        'eseguo la query
        Dim dataSet As System.Data.DataSet
        dataSet = _fillDataSet(sqlQuery)

        Return getChart(tipo, dataSet.Tables(0), rotateDataSet, includeTable)
    End Function


    Public Function getChart(ByVal tipo As ReportFlashManager.Report, ByVal dataTable As DataTable, _
                Optional ByVal rotateDataSet As Boolean = False, _
                Optional ByVal includeTable As Boolean = False) As String

        If isIndicatore(tipo) Then
            Throw New MyManager.ManagerException("Con questi parametri NON è possibile creare report di tipo indicatore")
        End If

        If (rotateDataSet) Then
            dataTable = rotateRigheToColonne(dataTable)
        End If

        Dim risposta As String = ""

        If (tipo = Report.Tabella) Then
            risposta = getTableHTML(dataTable)
        Else
            Dim xmlData As String = getXmlData(tipo, dataTable)

            risposta = RenderChartHTML(getFileSWF(tipo), "", xmlData, "ChartId", _width, _height, False)


            'lo swf per le Barre2D orizzontali non gestisce la multiserie con una serie sola!!!
            'Quindi cambio lo SWF...
            If tipo = Report.Bar2D And dataTable.Columns.Count = 2 Then
                'SINGOLA SERIE PER RIGHE
                'Nel formato LABEL, VALORE
                risposta = risposta.Replace("MSBar2D.swf", "Bar2D.swf")
            End If
            'la Multisere di una Bar2d non gestisce una singola serie



            'aggiungo la stampa dei dati in forma tabella alla fine del report
            If includeTable Then
                risposta &= getTableHTML(dataTable)
            End If
        End If

        Return risposta
    End Function



    Private Function getFileSWF(ByVal tipo As ReportFlashManager.Report) As String
        Dim risultato As String = ""


        '
        'If String.IsNullOrEmpty(_contestoWEB) Then
        '    risultato = "./Charts/"
        '    risultato = My.Application.Info.DirectoryPath & "\Charts\"
        'Else
        risultato = _contestoWEB & "/charts/"
        'End If


        Select Case tipo
            Case Report.AngularGauge
                risultato &= "FI2_Angular.swf"
            Case Report.Column3D
                risultato &= "MSColumn3D.swf"
            Case Report.Pie2D
                risultato &= "Pie2D.swf"
            Case Report.Column2D
                risultato &= "MSColumn2D.swf"
            Case Report.Line2D
                risultato &= "MSLine.swf"
            Case Report.Bar2D
                risultato &= "MSBar2D.swf"
            Case Report.Area2D
                risultato &= "MSArea.swf"
            Case Else
                Throw New MyManager.ManagerException("Attenzione non si riesce a risalire al file SWF")
        End Select
        Return risultato
    End Function



    ''' <summary>
    ''' Renders the HTML code for the chart. This
    ''' method does NOT embed the chart using JavaScript class. Instead, it uses
    ''' direct HTML embedding. So, if you see the charts on IE 6 (or above), you'll
    ''' see the "Click to activate..." message on the chart.
    ''' </summary>
    ''' <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
    ''' <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
    ''' <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
    ''' <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
    ''' <param name="chartWidth">Intended width for the chart (in pixels)</param>
    ''' <param name="chartHeight">Intended height for the chart (in pixels)</param>
    ''' <param name="debugMode">Whether to start the chart in debug mode</param>
    ''' <returns></returns>
    Public Shared Function RenderChartHTML(ByVal chartSWF As String, ByVal strURL As String, ByVal strXML As String, ByVal chartId As String, ByVal chartWidth As String, ByVal chartHeight As String, ByVal debugMode As Boolean) As String
        'Generate the FlashVars string based on whether dataURL has been provided
        'or dataXML.
        Dim strFlashVars As System.Text.StringBuilder = New System.Text.StringBuilder
        Dim flashVariables As String = String.Empty
        If (strXML.Length = 0) Then
            'DataURL Mode
            flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&dataURL={3}", chartWidth, chartHeight, boolToNum(debugMode), strURL)
        Else
            flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&dataXML={3}", chartWidth, chartHeight, boolToNum(debugMode), strXML)
        End If
        strFlashVars.AppendFormat(("<!-- START Code Block for Chart {0} -->" & Environment.NewLine), chartId)
        strFlashVars.AppendFormat(("<object classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"" codebase=""http://fpdownload.macromedi" & _
            "a.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"" width=""{0}"" height=""{1}"" name=""{2}" & _
            """>" & Environment.NewLine), chartWidth, chartHeight, chartId)
        strFlashVars.Append(("<param name=""allowScriptAccess"" value=""always"" />" & Environment.NewLine))
        strFlashVars.AppendFormat(("<param name=""movie"" value=""{0}""/>" & Environment.NewLine), chartSWF)
        strFlashVars.AppendFormat(("<param name=""FlashVars"" value=""{0}"" />" & Environment.NewLine), flashVariables)
        strFlashVars.Append(("<param name=""quality"" value=""high"" />" & Environment.NewLine))
        strFlashVars.AppendFormat(("<embed src=""{0}"" FlashVars=""{1}"" quality=""high"" width=""{2}"" height=""{3}"" name=""{4}""  allo" & _
            "wScriptAccess=""always"" type=""application/x-shockwave-flash"" pluginspage=""http://www.macromedia." & _
            "com/go/getflashplayer"" />" & Environment.NewLine), chartSWF, flashVariables, chartWidth, chartHeight, chartId)
        strFlashVars.Append(("</object>" & Environment.NewLine))
        strFlashVars.AppendFormat(("<!-- END Code Block for Chart {0} -->" & Environment.NewLine), chartId)
        Return strFlashVars.ToString()
    End Function


    ''' <summary>
    ''' Convert boolean value to integer value
    ''' </summary>
    ''' <param name="value">true/false value to be transformed</param>
    ''' <returns>1 if the value is true, 0 if the value is false</returns>
    Private Shared Function boolToNum(ByVal value As Boolean) As Integer
        If value Then
            Return 1
        Else
            Return 0
        End If
    End Function





    'Private Function getTagChart() As String
    '    Dim xmlData As String = ""
    '    xmlData &= "<chart"
    '    If Me._titolo <> "" Then
    '        xmlData &= " caption= '" & Me._titolo & "'"
    '    End If
    '    If Me._sottoTitolo <> "" Then
    '        xmlData &= " subCaption= '" & Me._sottoTitolo & "'"
    '    End If
    '    If Me._xAxisName <> "" Then
    '        xmlData &= " xAxisName= '" & Me._xAxisName & "'"
    '    End If
    '    If Me._yAxisName <> "" Then
    '        xmlData &= " yAxisName= '" & Me._yAxisName & "'"
    '    End If

    '    xmlData &= " >"
    '    Return xmlData
    'End Function




    Private Function decodeString(ByVal value As String) As String
        'gli apici creano problemi nella generazione del report 
        'Return value.Replace("'", "%26apos;").Replace("&", "").Replace("/", "").Replace(".", "").Replace("(", "").Replace(")", "")

        Return value.Replace("'", "%26apos;").Replace("&", "%26amp;")
    End Function



    ''' <summary>
    ''' Il DataSet di INPUT è del tipo:
    ''' LABEL, VALORE1, VALORE2, 
    ''' </summary>
    Private Function getXmlData(ByVal tipo As ReportFlashManager.Report, ByVal dataTable As System.Data.DataTable) As String
        Dim xmlData As String = ""


        xmlData &= "<chart"
        If Me._titolo <> "" Then
            xmlData &= " caption= '" & decodeString(Me._titolo) & "'"
        End If
        If Me._sottoTitolo <> "" Then
            xmlData &= " subCaption= '" & decodeString(Me._sottoTitolo) & "'"
        End If
        If Me._xAxisName <> "" Then
            xmlData &= " xAxisName= '" & decodeString(Me._xAxisName) & "'"
        End If
        If Me._yAxisName <> "" Then
            xmlData &= " yAxisName= '" & decodeString(Me._yAxisName) & "'"
        End If


        If Me._unitaDiMisura <> "" Then
            xmlData &= " numberPrefix= '" & decodeString(Me._unitaDiMisura) & "'"
        End If

        If Me._scalaDiMisula <> "" Then
            xmlData &= " numberSuffix= '" & decodeString(Me._scalaDiMisula) & "'"
        End If

        If Me._numeroCifreDecimali >= 0 Then
            xmlData &= " decimalPrecision= '" & Me._numeroCifreDecimali & "'"
        End If

        xmlData &= " >"





        Dim row As System.Data.DataRow
        Select Case tipo
            Case Report.Pie2D
                'SINGOLA SERIE PER RIGHE
                'Nel formato LABEL, VALORE
                If dataTable.Columns.Count <> 2 Then
                    Throw New MyManager.ManagerException("La singola serie necessita di DataSet composto da 2 colonne LABEL e VALORE")
                End If

                For Each row In dataTable.Rows
                    xmlData &= "<set label='" & decodeString(row(0).ToString) & "' value='" & row(1).ToString & "' />"
                Next
            Case Report.Column3D, Report.Line2D, Report.Column2D, Report.Area2D
                'MULTISERIE PER COLONNE
                xmlData &= Me.multiSeriePerColonne(dataTable)


            Case Report.Bar2D
                'la Multisere di una Bar2d non gestisce una singola serie

                'SINGOLA SERIE PER RIGHE
                'Nel formato LABEL, VALORE
                If dataTable.Columns.Count = 2 Then
                    For Each row In dataTable.Rows
                        xmlData &= "<set label='" & decodeString(row(0).ToString) & "' value='" & row(1).ToString & "' />"
                    Next
                Else
                    'MULTISERIE PER COLONNE
                    xmlData &= Me.multiSeriePerColonne(dataTable)
                End If


            Case Else
                Throw New MyManager.ManagerException("Attenzione questo tipo di report non è gestito correttamente")

        End Select


        xmlData &= "</chart>"

        Return xmlData.ToString
    End Function



    ''' <summary>
    '''  MULTISERIE PER COLONNE
    '''         ETICHETTA,ETICHETTA
    ''' SERIE1, VALORE1, VALORE2, ... VALOREN
    ''' SERIE2, VALORE1, VALORE2, ... VALOREN 
    ''' </summary>
    Private Function multiSeriePerColonne(ByVal dataTable As Data.DataTable) As String
        Dim i As Integer
        Dim numeroColonne As Integer
        Dim categories As String = ""
        Dim series As String = ""

        'Prelevo le etichette dal nome delle colonne del DataSet
        categories = "<categories>"
        'Salto la prima colonna.... parto da 1 e non da 0 
        numeroColonne = dataTable.Columns.Count
        For i = 1 To numeroColonne - 1
            categories &= "<category name='" & decodeString(dataTable.Columns(i).ColumnName) & "'/>"
        Next
        categories &= "</categories>"


        'Prelevo i valori di tutte le serie...
        Dim row As System.Data.DataRow
        For Each row In dataTable.Rows
            series &= " <dataset seriesName='" & decodeString(row(0).ToString) & "'>"
            For i = 1 To numeroColonne - 1
                series &= " <set value='" & CStr(IIf(IsDBNull(row(i)), "0", row(i).ToString.Replace(",", "."))) & "' />"
            Next
            series &= "</dataset>"
        Next


        Return categories & series
    End Function



    ''' <summary>
    ''' come INPUT ricevo un DataSet del tipo:
    ''' 12/10/2006
    ''' 
    '''         SERIE1   SERIE2       SERIEN
    ''' LABEL1, VALORE1, VALORE2, ... VALOREN
    ''' LABEL2, VALORE1, VALORE2, ... VALOREN
    ''' 
    ''' OUTPUT = 
    '''          LABEL1,LABEL2
    ''' SERIE1, VALORE1, VALORE2, ... VALOREN
    ''' SERIE2, VALORE1, VALORE2, ... VALOREN 
    ''' 
    ''' 
    ''' Ottengo una multiserie PER COLONNE
    ''' </summary>
    Private Function rotateRigheToColonne(ByVal dataSet As System.Data.DataSet) As System.Data.DataSet
        Dim ds As New Data.DataSet
        Dim table As Data.DataTable
        For Each dt As Data.DataTable In dataSet.Tables
            table = rotateRigheToColonne(dt)
            ds.Tables.Add(table)
        Next
        Return ds
    End Function


    Private Function rotateRigheToColonne(ByVal dataTable As System.Data.DataTable) As System.Data.DataTable
        Dim i, j As Integer
        'Dim ds As New Data.DataSet
        Dim row As Data.DataRow
        Dim table As New Data.DataTable
        Dim columnName As String

        'tutte i valori (LABEL) delle righe diventano le mie colonne
        'mi scorro tutte le righe leggendo la prima colonna
        table.Columns.Add("LABEL")
        For i = 0 To dataTable.Rows.Count - 1
            columnName = CStr(IIf(IsDBNull(dataTable.Rows(i)(0)), "", dataTable.Rows(i)(0).ToString))
            '      If Not table.Columns.Contains(columnName) Then
            table.Columns.Add(columnName)
            '  End If
        Next

        For i = 1 To dataTable.Columns.Count - 1
            'mi scorro tutta la colonna...
            row = table.NewRow
            row(0) = dataTable.Columns(i).ColumnName
            For j = 0 To dataTable.Rows.Count - 1
                row(j + 1) = dataTable.Rows(j)(i)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Class Range

        Public Sub New()

        End Sub

        Public Sub New(ByVal minValue As Double, ByVal maxValue As Double, ByVal name As String, ByVal color As String)
            Me._color = color
            Me._minValue = minValue
            Me._maxValue = maxValue
            Me._name = name
        End Sub



        Private _maxValue As Double
        Public Property maxValue() As Double
            Get
                Return _maxValue
            End Get
            Set(ByVal value As Double)
                _maxValue = value
            End Set
        End Property



        Private _name As String
        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property


        Private _color As String
        Public Property color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property




        Private _minValue As Double
        Public Property minValue() As Double
            Get
                Return _minValue
            End Get
            Set(ByVal value As Double)
                _minValue = value
            End Set
        End Property



    End Class





    Public Class TrendLine
        Private _value As Integer
        Public Property valore() As Integer
            Get
                Return _value
            End Get
            Set(ByVal value As Integer)
                _value = value
            End Set
        End Property


        Private _colore As Integer
        Public Property colore() As Integer
            Get
                Return _colore
            End Get
            Set(ByVal value As Integer)
                _colore = value
            End Set
        End Property

        Private _etichetta As String
        Public Property etichetta() As String
            Get
                Return _etichetta
            End Get
            Set(ByVal value As String)
                _etichetta = value
            End Set
        End Property






    End Class



End Class
