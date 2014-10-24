Public Class DotNetChartingWinFormsManager
    Inherits Manager

#Const Activate = True


    Private _titolo As String
    Public Property MyTitolo() As String
        Get
            Return _titolo
        End Get
        Set(ByVal value As String)
            _titolo = value
        End Set
    End Property



    Private _width As Integer = 200
    Public Property MyWidth() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property



    Private _height As Integer = 200
    Public Property MyHeight() As Integer
        Get
            Return _height
        End Get
        Set(ByVal value As Integer)
            _height = value
        End Set
    End Property





#If Activate Then

    Public Sub New()

    End Sub

    Public Sub New(ByVal providerName As String, ByVal connectionString As String, ByVal connection As Data.Common.DbConnection)
        _connection = connection

      
        Me._dataEngine = New dotnetCHARTING.WinForms.DataEngine()

        Select Case providerName
            Case "System.Data.Odbc"
                _dataEngine.DataProviderType = dotnetCHARTING.WinForms.DataProviderType.Odbc
            Case "System.Data.SqlClient"
                _dataEngine.DataProviderType = dotnetCHARTING.WinForms.DataProviderType.MsSql
            Case "Npgsql"
                _dataEngine.DataProviderType = dotnetCHARTING.WinForms.DataProviderType.Odbc
                'cambio la connessione!
                connectionString = MyManager.Manager.getOdbcConnectionFromNpgsql(connectionString)
                '_dataEngine.DataProviderType=
            Case Else
                Throw New ManagerException("Tipo di provider non supportato: " & providerName)
        End Select

        _connectionString = connectionString
        _provider = providerName
        Me._dataEngine.ConnectionString = _connectionString


    End Sub






    Public Sub New(ByVal providerName As String, ByVal connectionString As String)
        _connectionString = connectionString
        _provider = providerName
        Me._dataEngine = New dotnetCHARTING.WinForms.DataEngine()
        Me._dataEngine.ConnectionString = _connectionString

        Select Case _provider
            Case "System.Data.Odbc"
                _dataEngine.DataProviderType = dotnetCHARTING.WinForms.DataProviderType.Odbc
            Case Else
                Throw New ManagerException("Tipo di provider non supportato: " & _provider)
        End Select

        '_factory = System.Data.Common.DbProviderFactories.GetFactory(_provider)
        '_connection = _factory.CreateConnection()
        '_connection.ConnectionString = _connectionString

    End Sub





    Private _connectionString As String
    Private _dataEngine As dotnetCHARTING.WinForms.DataEngine

    Private _chartType As dotnetCHARTING.WinForms.ChartType = dotnetCHARTING.WinForms.ChartType.ComboHorizontal
    Public Property ChartType() As dotnetCHARTING.WinForms.ChartType
        Get
            Return _chartType
        End Get
        Set(ByVal value As dotnetCHARTING.WinForms.ChartType)
            _chartType = value
        End Set
    End Property


    Private _use3D As Boolean
    Public Property MyUse3D() As Boolean
        Get
            Return _use3D
        End Get
        Set(ByVal value As Boolean)
            _use3D = value
        End Set
    End Property



    Private _dataFields As String
    Public Property MyDataFields() As String
        Get
            Return _dataFields
        End Get
        Set(ByVal value As String)
            _dataFields = value
        End Set
    End Property


    Private _stacked As Boolean = False
    Public Property MyStacked() As Boolean
        Get
            Return _stacked
        End Get
        Set(ByVal value As Boolean)
            _stacked = value
        End Set
    End Property



    Public Sub getChart(ByRef myChart As dotnetCHARTING.WinForms.Chart, ByVal sqlQuery As String)
        ' Dim chart As New dotnetCHARTING.WinForms.Chart
        myChart.Type = _chartType
        myChart.TempDirectory = "temp"

        myChart.Debug = True
        myChart.Title = _Titolo

        ' Make element labels show up.
        myChart.DefaultElement.ShowValue = True


        myChart.Use3D = _use3D
        myChart.ShadingEffect = True
        '  myChart.ShadingEffectMode = dotnetCHARTING.WinForms.ShadingEffectMode.One


        If _stacked Then
            myChart.YAxis.Scale = dotnetCHARTING.WinForms.Scale.Stacked
        End If
        'chart.ImageFormat = dotnetCHARTING.WinForms.ImageFormat.Jpg
        '       chart.Dpi = 300
        '        chart.Series.Name = "ss"


        Dim sc As dotnetCHARTING.WinForms.SeriesCollection




        'chart.Series.Data = dataTable





        ' sqlQuery = "SELECT LOCATION, passed, unknow, failed FROM V_LOCATION_STATUS where date_evaluation ='29/10/2007 10:51:12' order by location"

        '_dataEngine.StartDate = New DateTime(2002, 1, 1, 8, 0, 0)     ' Specify the start date
        '_dataEngine.EndDate = New DateTime(2009, 1, 1, 23, 59, 59)
        _dataEngine.SqlStatement = sqlQuery


        If String.IsNullOrEmpty(_dataFields) Then
            _dataEngine.DataFields = String.Empty
        Else
            _dataEngine.DataFields = _dataFields
        End If

        '_dataEngine.DateGrouping = dotnetCHARTING.WinForms.TimeInterval.Months
        '_dataEngine.SplitByLimit



        '_dataEngine.SqlStatement = "SELECT myDate, GroupA, GroupB, GroupC FROM Stats"
        ' de.SqlStatement = "SELECT myDate, GroupA, GroupB, GroupC FROM Stats"
        'de.DataFields = "XValue=myDate,YValue=GroupA=Group Alpha,YValue=GroupB=Group Beta,YValue=GroupC=Group Gamma"


        '_dataEngine.DataFields = "XValue=Location,YValue=passed=Passed,YValue=unknow=UNKNOW,YValue=Failed=Group Gamma"

        sc = _dataEngine.GetSeries()


        If Not String.IsNullOrEmpty(_dataEngine.ErrorMessage) Then
            Throw New ManagerException(_dataEngine.ErrorMessage)
        End If



        'Dim dataTable As System.Data.DataTable
        'dataTable = _fillDataSet(sqlQuery).Tables(0)
        'sc = serieSemplice(dataTable)


        ' sc = getRandomData()
        myChart.SeriesCollection.Add(sc)





        'Enable streaming
        'chart.UseFile = False



        'Change the shading mode
        'chart.ShadingEffect = True
        'chart.ShadingEffectMode = dotnetCHARTING.ShadingEffectMode.Two


        'Set the x axis label
        'chart.ChartArea.XAxis.Label.Text = "X Axis Label"
        'chart.XAxis.Scale = dotnetCHARTING.WinForms.Scale.FullStacked


        'Set the y axis label
        'chart.ChartArea.YAxis.Label.Text = "Y Axis Label"

        'Set he chart size.
        'chart.  .wWidth = New System.Web.UI.WebControls.Unit(_width, Web.UI.WebControls.UnitType.Pixel)
        'chart.Height = _myHeight

        '  Return chart
    End Sub


    Public Sub getChart(ByRef myChart As dotnetCHARTING.WinForms.Chart, ByVal dataTable As DataTable)
        'ATTENZIONE!! da gestire se il datatable è singolaserie o multiserie
        'possibile soluzione: Singolaserie è sottinsieme di multiserie, indipendente quindi

        ' Dim chart As New dotnetCHARTING.WinForms.Chart
        myChart.Type = _chartType
        myChart.TempDirectory = "temp"
        myChart.Debug = True
        myChart.Title = _Titolo
        myChart.Use3D = _use3D

        ' Make element labels show up.
        myChart.DefaultElement.ShowValue = True
        myChart.SeriesCollection.Clear()


        Dim sc As dotnetCHARTING.WinForms.SeriesCollection

        If dataTable.Columns.Count > 2 Then
            sc = multiSeriePerColonne(dataTable)
        Else
            sc = serieSemplice(dataTable)
        End If


        myChart.SeriesCollection.Add(sc)

        myChart.RefreshChart()
    End Sub


    Public Function getChartStream(ByVal sqlQuery As String) As System.IO.MemoryStream
        Dim chart As dotnetCHARTING.WinForms.Chart = Nothing

        getChart(chart, sqlQuery)
        Return chart.GetChartStream
    End Function

    'LABEL, VALORE1
    Private Function serieSemplice(ByVal dataTable As Data.DataTable) As dotnetCHARTING.WinForms.SeriesCollection

        Dim s As New dotnetCHARTING.WinForms.Series()
        Dim e As dotnetCHARTING.WinForms.Element

        Dim row As System.Data.DataRow
        For Each row In dataTable.Rows

            If Not IsDBNull(row(0)) Then

                e = New dotnetCHARTING.WinForms.Element()
                e.Name = row(0)
                If IsDBNull(row(1)) Then
                    e.YValue = 0
                Else
                    e.YValue = row(1)
                End If


                s.Elements.Add(e)
            End If
        Next
        Return New dotnetCHARTING.WinForms.SeriesCollection(s)
    End Function



    ''' <summary>
    '''  MULTISERIE PER COLONNE
    '''         ETICHETTA,ETICHETTA
    ''' SERIE1, VALORE1, VALORE2, ... VALOREN
    ''' SERIE2, VALORE1, VALORE2, ... VALOREN 
    ''' </summary>
    Private Function multiSeriePerColonne(ByVal dataTable As Data.DataTable) As dotnetCHARTING.WinForms.SeriesCollection


        Dim sc As New dotnetCHARTING.WinForms.SeriesCollection()

        Dim numeroColonne As Integer
        numeroColonne = dataTable.Columns.Count

        Dim s As dotnetCHARTING.WinForms.Series
        Dim e As dotnetCHARTING.WinForms.Element
        Dim i As Integer
        'For i = 1 To numeroColonne - 1
        '    s = New dotnetCHARTING.WinForms.Series()
        '    s.Name = dataTable.Columns(i).ColumnName

        '    sc.Add(s)
        'Next




        'Prelevo i valori di tutte le serie...
        For Each row As System.Data.DataRow In dataTable.Rows
            s = New dotnetCHARTING.WinForms.Series()
            s.Name = row(0).ToString
            For i = 1 To numeroColonne - 1
                e = New dotnetCHARTING.WinForms.Element()
                e.Name = dataTable.Columns(i).Caption

                If IsDBNull(row(i)) Then
                    e.YValue = 0
                Else
                    'e.YValue = row(i).ToString.Replace(",", ".")
                    e.YValue = row(i)
                End If

                s.Elements.Add(e)
            Next
            sc.Add(s)
        Next




        Return sc


    End Function



    Public Function rotateRigheToColonne(ByVal dataTable As System.Data.DataTable) As System.Data.DataTable
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




    Function getRandomData() As dotnetCHARTING.WinForms.SeriesCollection
        Dim SC As New dotnetCHARTING.WinForms.SeriesCollection()
        Dim myR As New Random()
        Dim a As Integer
        For a = 1 To 4
            Dim s As New dotnetCHARTING.WinForms.Series()
            s.Name = "Series " + a.ToString()
            Dim b As Integer
            For b = 1 To 5
                Dim e As New dotnetCHARTING.WinForms.Element()
                e.Name = "E " + b.ToString()
                e.YValue = myR.Next(50)
                s.Elements.Add(e)
            Next b
            SC.Add(s)
        Next a

        ' Set Different Colors for our Series
        SC(0).DefaultElement.Color = System.Drawing.Color.FromArgb(49, 255, 49)
        SC(1).DefaultElement.Color = System.Drawing.Color.FromArgb(255, 255, 0)
        SC(2).DefaultElement.Color = System.Drawing.Color.FromArgb(255, 99, 49)
        SC(3).DefaultElement.Color = System.Drawing.Color.FromArgb(0, 156, 255)
        Return SC
    End Function 'getRandomData
#End If

End Class
