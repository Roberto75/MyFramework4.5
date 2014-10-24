'
' Created by SharpDevelop.
' User: Roberto
' Date: 12/09/2008
' Time: 17.36
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class MyTextBox
    Inherits System.Windows.Forms.TextBox

    Public Event InvalidEmail()
    Public Event InvalidPercentage()
    Public Event ValueIsOK()

    Public Enum WorkMode
        Interger
        Float
        Float4
        Email
        Percentage
        Currency
        Standard
    End Enum

    '	Private _greaterThanZero As Boolean
    '	<System.ComponentModel.DefaultValueAttribute(False)> _
    '	Public Property GreaterThanZero as Boolean
    '		Get
    '			return _greaterThanZero
    '		End Get
    '		Set( ByVal value As Boolean )
    '			Me._greaterThanZero = value
    '		End Set
    '	End Property

    Private _mode As WorkMode = WorkMode.Standard
    <System.ComponentModel.DefaultValueAttribute(GetType(WorkMode), "Standard")> _
     Public Property Mode() As WorkMode
        Get
            Return _mode
        End Get
        Set(ByVal value As WorkMode)
            _mode = value

            Select Case _mode
                Case WorkMode.Interger, WorkMode.Currency, WorkMode.Float, WorkMode.Float4
                    Me.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
                    Me.Width = 100
                Case WorkMode.Percentage
                    Me.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
                    Me.Width = 50
                Case Else
                    Me.Width = 200
                    Me.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
            End Select


            MyBase.Text = ""

            'Select Case _mode
            '    Case WorkMode.Email, WorkMode.Standard
            '        MyBase.Text = ""
            '    Case WorkMode.Currency
            '        MyBase.Text = String.Format("{0:C}", 0)
            '    Case WorkMode.Float
            '        MyBase.Text = String.Format("{0:N2}", 0)
            '    Case WorkMode.Interger
            '        MyBase.Text = String.Format("{0:N0}", 0)
            '    Case WorkMode.Percentage
            '        MyBase.Text = String.Format("{0:N2}%", 0)
            'End Select
        End Set
    End Property

    Public Sub New()
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Width = 200
    End Sub

    <System.ComponentModel.EditorBrowsableAttribute()> _
     Protected Overloads Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Select Case _mode
            Case WorkMode.Interger
                If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
                    MyBase.OnKeyPress(e)
                Else
                    ' come se non premessi il tasto
                    e.Handled = True
                End If
            Case WorkMode.Float, WorkMode.Currency, WorkMode.Percentage, WorkMode.Float4
                If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) _
                    OrElse (e.KeyChar = "-") _
                    OrElse (e.KeyChar = "," AndAlso MyBase.Text.IndexOf(",") = -1) Then
                    'consento di digitare una sola ,
                    MyBase.OnKeyPress(e)
                Else
                    ' come se non premessi il tasto
                    e.Handled = True
                End If
            Case Else
                MyBase.OnKeyPress(e)
        End Select
    End Sub


    '<System.ComponentModel.EditorBrowsableAttribute()> _
    ' Protected Overloads Overrides Sub OnLeave(ByVal e As EventArgs)
    '    Select Case _mode
    '        Case WorkMode.Email
    '            If isValidEmail(MyBase.Text) Then
    '                MyBase.OnLeave(e)
    '                RaiseEvent ValueIsOK()
    '            Else
    '                RaiseEvent InvalidEmail()
    '            End If
    '        Case WorkMode.Currency
    '            If String.IsNullOrEmpty(MyBase.Text) Then
    '                MyBase.Text = String.Format("{0:C}", 0)
    '            Else
    '                'if _greaterThanZero andalso  then
    '                MyBase.Text = String.Format("{0:C}", Decimal.Parse(Me._Value))
    '            End If
    '        Case WorkMode.Float
    '            If String.IsNullOrEmpty(MyBase.Text) Then
    '                MyBase.Text = String.Format("{0:N2}", 0)
    '            Else
    '                MyBase.Text = String.Format("{0:N2}", Decimal.Parse(MyBase.Text))
    '            End If
    '        Case WorkMode.Interger
    '            If String.IsNullOrEmpty(MyBase.Text) Then
    '                MyBase.Text = String.Format("{0:N0}", 0)
    '            Else
    '                MyBase.Text = String.Format("{0:N0}", Integer.Parse(MyBase.Text.Replace(".", "")))
    '            End If
    '        Case WorkMode.Percentage
    '            If String.IsNullOrEmpty(MyBase.Text) Then
    '                MyBase.Text = String.Format("{0:N2}%", 0)
    '            Else
    '                Dim d As Decimal
    '                d = Decimal.Parse(Me._Value)
    '                MyBase.Text = String.Format("{0:N2}%", d)
    '                'controllo .... se il chiamnte gestisce questo tipo di evento ...
    '                If d < 0 OrElse d > 100 Then
    '                    RaiseEvent InvalidPercentage()
    '                Else
    '                    RaiseEvent ValueIsOK()
    '                End If
    '            End If
    '        Case Else
    '            MyBase.OnLeave(e)
    '    End Select
    'End Sub



    <System.ComponentModel.EditorBrowsableAttribute()> _
 Protected Overrides Sub OnValidated(ByVal e As System.EventArgs)
        Select Case _mode
            Case WorkMode.Email
                If isValidEmail(MyBase.Text) Then
                    ' MyBase.OnValidated(e)
                    RaiseEvent ValueIsOK()
                Else
                    RaiseEvent InvalidEmail()
                End If
            Case WorkMode.Currency
                If String.IsNullOrEmpty(MyBase.Text) Then
                    MyBase.Text = String.Format("{0:C}", 0)
                Else
                    'if _greaterThanZero andalso  then
                    MyBase.Text = String.Format("{0:C}", Decimal.Parse(Me._Value))
                End If
            Case WorkMode.Float
                If String.IsNullOrEmpty(MyBase.Text) Then
                    MyBase.Text = String.Format("{0:N2}", 0)
                Else
                    MyBase.Text = String.Format("{0:N2}", Decimal.Parse(MyBase.Text))
                End If
            Case WorkMode.Float4
                If String.IsNullOrEmpty(MyBase.Text) Then
                    MyBase.Text = String.Format("{0:N4}", 0)
                Else
                    MyBase.Text = String.Format("{0:N4}", Decimal.Parse(MyBase.Text))
                End If
            Case WorkMode.Interger
                If String.IsNullOrEmpty(MyBase.Text) Then
                    MyBase.Text = String.Format("{0:N0}", 0)
                Else
                    MyBase.Text = String.Format("{0:N0}", Long.Parse(MyBase.Text.Replace(".", "")))
                End If
            Case WorkMode.Percentage
                If String.IsNullOrEmpty(MyBase.Text) Then
                    MyBase.Text = String.Format("{0:N2}%", 0)
                Else
                    Dim d As Decimal
                    d = Decimal.Parse(Me._Value)
                    MyBase.Text = String.Format("{0:N2}%", d)
                    'controllo .... se il chiamnte gestisce questo tipo di evento ...
                    If d < 0 OrElse d > 100 Then
                        RaiseEvent InvalidPercentage()
                    Else
                        RaiseEvent ValueIsOK()
                    End If
                End If
                'Case Else
                '    MyBase.OnValidated(e)
        End Select

        '09/03/2010
        MyBase.OnValidated(e)
    End Sub



    Private Function cleanNumberValue(ByVal value As String) As String
        Return value.Replace(" ", "").Replace(".", "")
    End Function

    Public Shared Function isValidEmail(ByVal value As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(value, "\S+@\S+\.\S{2,3}")
    End Function


    Public Property _Value() As String
        Get
            Dim temp As String
            Select Case _mode
                Case WorkMode.Currency
                    temp = cleanNumberValue(MyBase.Text)
                    temp = temp.Replace("€", "").Replace("$", "")
                    Return temp
                Case WorkMode.Percentage
                    temp = cleanNumberValue(MyBase.Text)
                    temp = temp.Replace("%", "")
                    Return temp
                Case WorkMode.Interger, WorkMode.Float, WorkMode.Float4
                    temp = cleanNumberValue(MyBase.Text)
                    If String.IsNullOrEmpty(temp) Then
                        Return 0
                    End If
                    Return temp

                Case Else
                    Return MyBase.Text
            End Select
        End Get
        Set(ByVal value As String)
            Dim temp As String
            Select Case _mode
                'Case WorkMode.Currency
                '   temp = cleanNumberValue(MyBase.Text)
                '  temp = temp.Replace("€", "").Replace("$", "")

                Case WorkMode.Percentage
                    temp = cleanNumberValue(value)
                    temp = temp.Replace("%", "")
                    MyBase.Text = String.Format("{0:N2}%", Decimal.Parse(temp))
                Case Else
                    MyBase.Text = value
            End Select
        End Set
    End Property





End Class
