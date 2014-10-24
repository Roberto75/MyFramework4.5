Public Class GoogleMarkerManager
    Inherits MyManager.Manager

    Public Sub New()
        ' MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    Public Function _openTemplate(ByVal teplateName As String) As String
        If Not IO.File.Exists(Windows.Forms.Application.StartupPath & "\" & teplateName) Then
            Throw New MyManager.ManagerException("Template non presente nella cartella")
            Return ""
        End If

        Dim path As String = Windows.Forms.Application.StartupPath & "\temp\"
        If Not System.IO.Directory.Exists(path) Then
            System.IO.Directory.CreateDirectory(path)
        End If

        path = path & teplateName.Replace(".html", "") & "_" & Now.Year.ToString & String.Format("{0:00}", Now.Month) & String.Format("{0:00}", Now.Day) & "_" & String.Format("{0:00}", Now.Hour) & String.Format("{0:00}", Now.Minute) & String.Format("{0:00}", Now.Second) & ".html"
        System.IO.File.Copy(Windows.Forms.Application.StartupPath & "\" & teplateName, path, True)

        Return path
    End Function

    Public Function _getFromLogSession() As Data.DataTable
        Dim strSQL As String
        strSQL = "select count(*) as TOT,  latitude, longitude  from LOG_SESSION where (latitude is not null) or (longitude is not null) group by latitude, longitude"
        Return _fillDataTable(strSQL)
    End Function

    Public Function getHTML(ByVal groupName As String, ByVal poits As List(Of MyManager.GoogleMapsPoint)) As String
        Dim HTML As String = ""

        HTML = HTML & String.Format(" var name = ""{0}"";", groupName) & vbCrLf
        HTML = HTML & " var myIcon = new GIcon(G_DEFAULT_ICON);" & vbCrLf
        HTML = HTML & " myIcon.image = ""../images/"" + name + "".png"";" & vbCrLf
        HTML = HTML & " myIcon.iconSize = new GSize(20,20);" & vbCrLf

        ' HTML = HTML & " markerOptions = { icon:myIcon,   };" & vbCrLf
        HTML = HTML & " var markers = [];" & vbCrLf

        For Each p As MyManager.GoogleMapsPoint In poits
            HTML = HTML & String.Format("  var marker = new GMarker(new GLatLng( {0},  {1}) , {{  icon:myIcon, clickable:false, title:""{2}""  }});", p.Latitude.ToString().Replace(",", "."), p.Longitude.ToString().Replace(",", "."), p.Tooltip) & vbCrLf
            HTML = HTML & " markers.push(marker);" & vbCrLf
        Next

        HTML = HTML & " var opt = new Object();" & vbCrLf
        HTML = HTML & " var styles_ = [];" & vbCrLf
        '// QUI SI METTONO LE DIMENSIONI PROGRESSIVE DEI 5 SIMBOLI
        HTML = HTML & " var sizes = [45, 55, 65, 75, 90];" & vbCrLf
        HTML = HTML & "  for (i = 1; i <= 5; ++i) {" & vbCrLf
        HTML = HTML & "  styles_.push({" & vbCrLf
        HTML = HTML & "  'url': ""../images/"" + name + i + "".png""," & vbCrLf
        HTML = HTML & "  'height': sizes[i - 1], " & vbCrLf
        HTML = HTML & "  'width': sizes[i - 1]" & vbCrLf
        HTML = HTML & "    });" & vbCrLf
        HTML = HTML & "}" & vbCrLf
        HTML = HTML & " opt.styles = [];" & vbCrLf
        HTML = HTML & " opt.styles = styles_;" & vbCrLf
        HTML = HTML & " var markerCluster = new MarkerClusterer(map, markers,opt);" & vbCrLf
        HTML = HTML & vbCrLf & vbCrLf
        Return HTML
    End Function



    Public Function getHTML_V3(ByVal groupName As String, ByVal poits As List(Of MyManager.GoogleMapsPoint)) As String
        Dim HTML As String = ""
        Dim info As String = ""

        HTML = HTML & String.Format(" var name = ""{0}"";", groupName) & vbCrLf

        ' HTML = HTML & " var myIcon = new google.maps.MarkerImage ({ url:'images/casa.gif' });" & vbCrLf
        'HTML = HTML & " myIcon.image = ""../images/"" + name + "".png"";" & vbCrLf
        ' HTML = HTML & " myIcon.iconSize = new GSize(20,20);" & vbCrLf

        '' HTML = HTML & " markerOptions = { icon:myIcon,   };" & vbCrLf
        HTML = HTML & " var markers = [];" & vbCrLf

        HTML &= "var infowindow = new google.maps.InfoWindow({  content: 'dummy' });"

        Dim i As Long = 1
        For Each p As MyManager.GoogleMapsPoint In poits
            ' HTML = HTML & String.Format("  var marker = new google.maps.Marker ( new google.maps.LatLng( {0},  {1}) , {{  icon:myIcon, clickable:false, title:""{2}""  }});", p.Latitude.ToString().Replace(",", "."), p.Longitude.ToString().Replace(",", "."), p.Tooltip) & vbCrLf
            HTML = HTML & String.Format(" var latLng = new google.maps.LatLng({0},  {1}); ", p.Latitude.ToString().Replace(",", "."), p.Longitude.ToString().Replace(",", ".")) & vbCrLf

            info = String.Format("{0} <br />", p.InfoWindow)

            HTML = HTML & String.Format(" var marker = new google.maps.Marker ( {{ position: latLng , icon: 'images/casa.gif' , title:'{0}', html:'{1}' }}) ;", p.Tooltip, info) & vbCrLf
            HTML = HTML & " markers.push(marker);" & vbCrLf

          
            HTML &= "google.maps.event.addListener(marker, 'click', function() {  infowindow.setContent(this.html); infowindow.open(map, this) });"
            i = i + 1
        Next

        'HTML = HTML & " var opt = new Object();" & vbCrLf
        'HTML = HTML & " var styles_ = [];" & vbCrLf
        ''// QUI SI METTONO LE DIMENSIONI PROGRESSIVE DEI 5 SIMBOLI
        'HTML = HTML & " var sizes = [45, 55, 65, 75, 90];" & vbCrLf
        'HTML = HTML & "  for (i = 1; i <= 5; ++i) {" & vbCrLf
        'HTML = HTML & "  styles_.push({" & vbCrLf
        'HTML = HTML & "  'url': ""../images/"" + name + i + "".png""," & vbCrLf
        'HTML = HTML & "  'height': sizes[i - 1], " & vbCrLf
        'HTML = HTML & "  'width': sizes[i - 1]" & vbCrLf
        'HTML = HTML & "    });" & vbCrLf
        'HTML = HTML & "}" & vbCrLf
        'HTML = HTML & " opt.styles = [];" & vbCrLf
        'HTML = HTML & " opt.styles = styles_;" & vbCrLf
        'HTML = HTML & " var markerCluster = new MarkerClusterer(map, markers,opt);" & vbCrLf
        HTML = HTML & " var markerCluster = new MarkerClusterer(map, markers);" & vbCrLf
        HTML = HTML & vbCrLf & vbCrLf
        Return HTML
    End Function

    Public Function getHTML(ByVal points As List(Of MyManager.GoogleMapsPoint)) As String
        Dim HTML As String = ""
        HTML = HTML & " var markers = [];" & vbCrLf

        For Each p As MyManager.GoogleMapsPoint In points
            HTML = HTML & String.Format("  var marker = new GMarker(new GLatLng( {0},  {1}));", p.Latitude.ToString().Replace(",", "."), p.Longitude.ToString().Replace(",", ".")) & vbCrLf
            HTML = HTML & " markers.push(marker);" & vbCrLf
        Next


        HTML = HTML & " var opt = new Object();" & vbCrLf
        HTML = HTML & " var styles_ = [];" & vbCrLf
        '// QUI SI METTONO LE DIMENSIONI PROGRESSIVE DEI 5 SIMBOLI
        HTML = HTML & " var sizes = [53, 56, 66, 78, 90];" & vbCrLf
        HTML = HTML & "  for (i = 1; i <= 5; ++i) {" & vbCrLf
        HTML = HTML & "  styles_.push({" & vbCrLf
        HTML = HTML & "  'url': ""http://gmaps-utility-library.googlecode.com/svn/trunk/markerclusterer/images/m"" + i + "".png""," & vbCrLf
        HTML = HTML & "  'height': sizes[i - 1], " & vbCrLf
        HTML = HTML & "  'width': sizes[i - 1]" & vbCrLf
        HTML = HTML & "    });" & vbCrLf
        HTML = HTML & "}" & vbCrLf
        HTML = HTML & " opt.styles = [];" & vbCrLf
        HTML = HTML & " opt.styles = styles_;" & vbCrLf
        HTML = HTML & " var markerCluster = new MarkerClusterer(map, markers,opt);" & vbCrLf
        HTML = HTML & vbCrLf & vbCrLf
        Return HTML
    End Function

    Public Function mergePointSameLatLng(ByVal values As List(Of MyManager.GoogleMapsPoint)) As List(Of MyManager.GoogleMapsPoint)
        Dim risultato As New List(Of MyManager.GoogleMapsPoint)

        '  Dim t As MyManager.GoogleMapsPoint
        'Dim index As Integer


        ' Dim match As Predicate(Of MyManager.GoogleMapsPoint)

        For Each p As MyManager.GoogleMapsPoint In values

            ' index = risultato.( (AddressOf CompareMapPoint())

            'i = risultato.Find(
            'If i <> 0 Then

            '    t = r
            'Else
            '    risultato.Add(p)
            'End If

        Next

        Return risultato
    End Function



    '  Public Function FindPoint(ByVal match As Predicate(Of MyManager.GoogleMapsPoint)) As MyManager.GoogleMapsPoint

    ' End Function



    Shared Function CompareMapPoint(ByVal item1 As MyManager.GoogleMapsPoint, ByVal item2 As MyManager.GoogleMapsPoint) As Integer
        Dim r As Integer

        r = item1.Latitude.CompareTo(item2.Latitude)
        If r <> 0 Then
            Return r
        End If

        Return item1.Longitude.CompareTo(item2.Longitude)
    End Function



    Public Function searchIpAddressInTxtFile(ByVal f As IO.FileInfo) As Boolean

    End Function



End Class
