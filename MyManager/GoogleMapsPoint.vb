Public Class GoogleMapsPoint
    Implements IComparer

    Private _occorrenze As Long = 0
    Public Property Occorrenze() As Long
        Get
            Return _occorrenze
        End Get
        Set(ByVal value As Long)
            _occorrenze = value
        End Set
    End Property

    Public Function _addOccorrenza() As Long
        _occorrenze = _occorrenze + 1
        Return _occorrenze
    End Function

    Public Function _addOccorrenze(ByVal value As Long) As Long
        _occorrenze = _occorrenze + value
        Return _occorrenze
    End Function



    Private _latitude As Double
    Public Property Latitude() As Object
        Get
            Return _latitude
        End Get
        Set(ByVal value As Object)
            If TypeOf value Is String Then
                _latitude = Double.Parse(value.ToString().Replace(".", ","))
            ElseIf TypeOf value Is Double Then
                _latitude = value
            ElseIf TypeOf value Is Decimal Then
                _latitude = value
            Else
                Throw New ManagerException("GoogleMapsPoint Latitude tipo non gestito")
            End If
        End Set
    End Property


    Private _longitude As Double
    Public Property Longitude() As Object
        Get
            Return _longitude
        End Get
        Set(ByVal value As Object)
            If TypeOf value Is String Then
                _longitude = Double.Parse(value.ToString().Replace(".", ","))
            ElseIf TypeOf value Is Double Then
                _longitude = value
            ElseIf TypeOf value Is Decimal Then
                _longitude = value
            Else
                Throw New ManagerException("GoogleMapsPoint Longitude tipo non gestito")
            End If
        End Set
    End Property


    Private _imageURL As String
    Public Property ImageURL() As String
        Get
            Return _imageURL
        End Get
        Set(ByVal value As String)
            _imageURL = value
        End Set
    End Property


    Private _tooltip As String
    Public Property Tooltip() As String
        Get
            Return _tooltip
        End Get
        Set(ByVal value As String)
            _tooltip = value
        End Set
    End Property


    Private _externalId As String = 0
    Public Property ExternalId() As String
        Get
            Return _externalId
        End Get
        Set(ByVal value As String)
            _externalId = value
        End Set
    End Property


    Private _infoWindow As String = 0
    Public Property InfoWindow() As String
        Get
            Return _infoWindow
        End Get
        Set(ByVal value As String)
            _infoWindow = value
        End Set
    End Property


    'Private _link As String = 0
    'Public Property Link() As String
    '    Get
    '        Return _link
    '    End Get
    '    Set(ByVal value As String)
    '        _link = value
    '    End Set
    'End Property

    Public Function CompareMapPoint(ByVal item1 As Object, ByVal item2 As Object) As Integer Implements IComparer.Compare
        Dim r As Integer

        r = item1.Latitude.CompareTo(item2.Latitude)
        If r <> 0 Then
            Return r
        End If

        Return item1.Longitude.CompareTo(item2.Longitude)
    End Function

End Class
