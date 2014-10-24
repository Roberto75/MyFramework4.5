Imports System.Drawing

Public Class Captcha

    Protected RandomValue As New Random

    Protected _captchaImage As Bitmap
    Protected _captchaHeight As Integer
    Protected _captchaWidth As Integer
    Protected _captchaFont As Font
    Protected _captchaCode As String

    ''' <summary>
    ''' Useful for subclassing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Loads properties and generates an initial captcha value and graphic
    ''' </summary>
    ''' <param name="imageWidth">The Width of the Captcha Graphic</param>
    ''' <param name="imageHeight">The Height of the Captcha Graphic</param>
    ''' <param name="imageFont">The Font of the Captcha Graphic</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal imageWidth As Integer, ByVal imageHeight As Integer, ByVal imageFont As Font, ByVal stringa As String)
        _captchaCode = stringa
        _captchaHeight = imageHeight
        _captchaWidth = imageWidth
        _captchaFont = imageFont
        GenerateNewCaptcha()
    End Sub

    ''' <summary>
    ''' Generates a new captcha value and graphic for the class
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GenerateNewCaptcha()
        If _captchaFont Is Nothing OrElse _captchaHeight = 0 OrElse _captchaWidth = 0 Then
            Exit Sub
        End If
        ' Geneara il codice Random
        ' _captchaCode = RandomValue.Next(10000, 100000).ToString
        _captchaImage = New Bitmap(_captchaWidth, _captchaHeight)
        Using CaptchaGraphics As Graphics = Graphics.FromImage(CaptchaImage)
            Using BackgroundBrush As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Max, Color.LightBlue, Color.DarkTurquoise)
                CaptchaGraphics.FillRectangle(BackgroundBrush, 0, 0, _captchaWidth, _captchaHeight)
            End Using
            Dim CharacterSpacing As Integer = (_captchaWidth \ 5) - 1
            Dim HorizontalPosition As Integer
            Dim MaxVerticalPosition As Integer
            For Each CharValue As Char In _captchaCode.ToCharArray
                MaxVerticalPosition = _captchaHeight - Convert.ToInt32(CaptchaGraphics.MeasureString(CharValue, _captchaFont).Height)
                CaptchaGraphics.DrawString(CharValue, _captchaFont, Brushes.Blue, HorizontalPosition, RandomValue.Next(0, MaxVerticalPosition))
                HorizontalPosition += CharacterSpacing + RandomValue.Next(-1, 1)
            Next
            For Counter As Integer = 0 To 24
                CaptchaGraphics.FillEllipse(Brushes.DimGray, RandomValue.Next(1, _captchaWidth), RandomValue.Next(1, _captchaHeight), RandomValue.Next(1, 4), RandomValue.Next(1, 4))
            Next
            For Counter As Integer = 0 To 24
                CaptchaGraphics.FillEllipse(Brushes.WhiteSmoke, RandomValue.Next(1, _captchaWidth), RandomValue.Next(1, _captchaHeight), RandomValue.Next(1, 4), RandomValue.Next(1, 4))
            Next
        End Using
    End Sub

    ''' <summary>
    ''' The captcha bitmap image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaptchaImage() As Bitmap
        Get
            Return _captchaImage
        End Get
        Set(ByVal value As Bitmap)
            _captchaImage = value
        End Set
    End Property

    ''' <summary>
    ''' The value of the captcha
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaptchaCode() As String
        Get
            Return _captchaCode
        End Get
        Set(ByVal value As String)
            _captchaCode = value
        End Set
    End Property

    ''' <summary>
    ''' The Height of the Captcha Graphic
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaptchaHeight() As Integer
        Get
            Return _captchaHeight
        End Get
        Set(ByVal value As Integer)
            _captchaHeight = value
        End Set
    End Property

    ''' <summary>
    ''' The Width of the Captcha Graphic
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaptchaWidth() As Integer
        Get
            Return _captchaWidth
        End Get
        Set(ByVal value As Integer)
            _captchaWidth = value
        End Set
    End Property

    ''' <summary>
    ''' The Font of the Captcha Graphic
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaptchaFont() As Font
        Get
            Return _captchaFont
        End Get
        Set(ByVal value As Font)
            _captchaFont = value
        End Set
    End Property

End Class
