Public Class GUIManager






    Public Shared Function getDate(ByVal value As Object) As Date

        If (value Is Nothing) Then
            Return ""
        End If

        If (value.Equals(System.DBNull.Value)) Then
            Return ""
        End If


        Dim result As Date
        result = Date.Parse(value)

        Return result
    End Function


    Public Shared Function getDateTime(ByVal value As String) As DateTime
        Dim result As DateTime
        result = DateTime.Parse(value)

        Return result
    End Function


    Public Shared Function isEmptyDate(ByVal value As DateTime) As Boolean
        '*** Attenzione con IsNothing non funziona!!!!!
        'If IsNothing(a) Then
        '    Console.WriteLine("La data è NULLA )")
        'Else
        '    Console.WriteLine("La data NON è NULLA (ERRORE!")
        'End If


        Return (value = Nothing)
    End Function


    Public Shared Function getEuro(ByVal value As Object) As String

        If (value Is Nothing) Then
            Return ""
        End If

        If (value.Equals(System.DBNull.Value)) Then
            Return ""
        End If

        Dim temp As Double = Double.Parse(value.ToString)

        Return FormatCurrency(temp, 2)
    End Function



    Public Shared Function getString(ByVal value As Object) As String

        If (value Is Nothing) Then
            Return ""
        End If

        If Not (value.Equals(System.DBNull.Value)) Then

            Return value.ToString
        Else
            Return ""

        End If


    End Function

    Public Shared Function getInteger(ByVal value As Object) As Integer

        If (value Is Nothing) Then
            Return 0
        End If

        If Not (value.Equals(System.DBNull.Value)) Then

            Return Integer.Parse(value)

        End If


    End Function

    Public Shared Function getLong(ByVal value As Object) As Long

        If (value Is Nothing OrElse value = Nothing) Then
            Return 0
        End If

        If Not (value.Equals(System.DBNull.Value) AndAlso value.ToString <> "") Then
            Return Long.Parse(value)
        End If
    End Function

    Public Shared Function getDouble(ByVal value As Object) As Double

        If (value Is Nothing OrElse value = Nothing) Then
            Return 0
        End If

        If Not (value.Equals(System.DBNull.Value) AndAlso value.ToString <> "") Then
            Return Double.Parse(value)
        End If
    End Function

    Public Shared Function getBoolean(ByVal value As Object) As Boolean

        If (value Is Nothing OrElse value = Nothing) Then
            Return Nothing
        End If

        If Not (value.Equals(System.DBNull.Value) AndAlso value.ToString <> "") Then

            Return Boolean.Parse(value)

        End If

    End Function




    Public Shared Function getMonth(ByVal numero As Int16) As String
        Select Case (numero)
            Case 1
                Return "Gennaio"
            Case 2
                Return "Febbraio"
            Case 3
                Return "Marzo"
            Case 4
                Return "Aprile"
            Case 5
                Return "Maggio"
            Case 6
                Return "Giugno"
            Case 7
                Return "Lugio"
            Case 8
                Return "Agosto"
            Case 9
                Return "Settembre"
            Case 10
                Return "Ottobre"
            Case 11
                Return "Novembre"
            Case 12
                Return "Dicembre"
        End Select
        Return numero
    End Function
End Class
