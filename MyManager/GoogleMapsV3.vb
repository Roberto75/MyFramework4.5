Public Class GoogleMapsV3

    Public Function _getNewFileFromTemplate(ByVal teplateName As String) As String
        If Not IO.File.Exists(Windows.Forms.Application.StartupPath & "\" & teplateName) Then
            Throw New MyManager.ManagerException("Template not found")
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


    Public Function _updateHTML_Marker(ByVal sourceHTML As String, ByVal point As MyManager.GoogleMapsPoint) As String
        Dim temp As New List(Of MyManager.GoogleMapsPoint)
        temp.Add(point)
        Return _updateHTML_Marker(sourceHTML, temp)
    End Function

    Public Function _updateHTML_Marker(ByVal sourceHTML As String, ByVal points As List(Of MyManager.GoogleMapsPoint)) As String
        Dim HTML As String = ""

        For Each p As MyManager.GoogleMapsPoint In points
            HTML = HTML & String.Format("  var marker = new google.maps.Marker( {{  position: new google.maps.LatLng( {0},  {1}) ,   map: map, title:""{2}""  }});", p.Latitude.ToString().Replace(",", "."), p.Longitude.ToString().Replace(",", "."), p.Tooltip) & vbCrLf
        Next

        Dim x As Integer
        x = sourceHTML.IndexOf("<!-- MyMarkers -->")
        sourceHTML = sourceHTML.Substring(0, x) & vbCrLf & HTML & vbCrLf & sourceHTML.Substring(x)
        Return sourceHTML
    End Function


    Public Function _updateHTML_CenterMap(ByVal sourceHTML As String, ByVal point As MyManager.GoogleMapsPoint) As String
        Return _updateHTML_CenterMap(sourceHTML, point.Latitude, point.Longitude)
    End Function

    Public Function _updateHTML_CenterMap(ByVal sourceHTML As String, ByVal latitute As Double, ByVal longitute As Double) As String
        Dim HTML As String

        HTML = String.Format("var latlng = new google.maps.LatLng({0}, {1});", latitute.ToString().Replace(",", "."), longitute.ToString().Replace(",", "."))

        Dim x As Integer
        x = sourceHTML.IndexOf(" <!--MyCenterPoint-->")
        sourceHTML = sourceHTML.Substring(0, x) & vbCrLf & HTML & vbCrLf & sourceHTML.Substring(x)
        Return sourceHTML

    End Function


End Class
