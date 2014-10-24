Public Class MyContatto


    Public ReadOnly Property MyKey() As String
        Get
            Dim fullName As String

            If (String.IsNullOrEmpty(Nome)) Then
                fullName = Cognome
            Else
                fullName = Nome & " " & Cognome
            End If

            If (String.IsNullOrEmpty(fullName)) Then
                fullName = Societa
            End If

            Return fullName
        End Get
    End Property


    Private _outlookId As String
    Public Property OutlookId() As String
        Get
            Return _outlookId
        End Get
        Set(ByVal value As String)
            _outlookId = value
        End Set
    End Property



    



    Private _nome As String
    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property


    Private _cognome As String
    Public Property Cognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal value As String)
            _cognome = value
        End Set
    End Property

    Private _societa As String
    Public Property Societa() As String
        Get
            Return _societa
        End Get
        Set(ByVal value As String)
            _societa = value
        End Set
    End Property

    Private _note As String
    Public Property Note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property

    Private _compreanno As DateTime
    Public Property Compleanno() As DateTime
        Get
            Return _compreanno
        End Get
        Set(ByVal value As DateTime)
            _compreanno = value
        End Set
    End Property

    Private _cellulare As String
    Public Property Cellulare() As String
        Get
            Return _cellulare
        End Get
        Set(ByVal value As String)
            _cellulare = value
        End Set
    End Property

    Private _telefonoCasa As String
    Public Property TelefonoCasa() As String
        Get
            Return _telefonoCasa
        End Get
        Set(ByVal value As String)
            _telefonoCasa = value
        End Set
    End Property

    Private _telfonoUfficio As String
    Public Property TelefonoUfficio() As String
        Get
            Return _telfonoUfficio
        End Get
        Set(ByVal value As String)
            _telfonoUfficio = value
        End Set
    End Property

    Private _emailPersonale As String
    Public Property EmailPersonale() As String
        Get
            Return _emailPersonale
        End Get
        Set(ByVal value As String)
            _emailPersonale = value
        End Set
    End Property

    Private _emailUfficio As String
    Public Property EmailUfficio() As String
        Get
            Return _emailUfficio
        End Get
        Set(ByVal value As String)
            _emailUfficio = value
        End Set
    End Property




    Public Class CompareByMyKey : Implements IComparer(Of MyContatto)

        Public Function Compare1(x As MyContatto, y As MyContatto) As Integer Implements System.Collections.Generic.IComparer(Of MyContatto).Compare
            If (x.MyKey Is Nothing) Then
                Return 1
            End If

            If (y.MyKey Is Nothing) Then
                Return -1
            End If

            Return x.MyKey.CompareTo(y.MyKey)
        End Function
    End Class

            

End Class
