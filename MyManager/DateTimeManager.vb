Public Class DateTimeManager

    ''' <summary>
    ''' inhibits any instance of the class
    ''' </summary>
    Private Sub New()
        '
    End Sub

    ''' <summary>
    ''' Finds the previous specified day of week before the specified date
    ''' </summary>
    ''' <param name="dow">The day of the week to search for</param>
    ''' <param name="data">The starting date. Use the current date, if none was specified</param>
    Public Shared Function PreviousDow(ByVal dow As DayOfWeek, ByVal data As DateTime) As DateTime
        Dim dt As DateTime = data.AddDays(dow - data.DayOfWeek)
        If dt >= data Then dt = dt.AddDays(-7)
        Return dt
    End Function

    Public Shared Function PreviousDow(ByVal dow As DayOfWeek) As DateTime
        Return PreviousDow(dow, DateTime.Today)
    End Function


    ''' <summary>
    ''' Finds the next specified day of week after the specified date
    ''' </summary>
    ''' <param name="dow">The day of the week to search for</param>
    ''' <param name="data">The starting date. Use the current date, if none was specified</param>
    Public Shared Function NextDow(ByVal dow As DayOfWeek, ByVal data As DateTime) As DateTime
        Dim dt As DateTime = data.AddDays(dow - data.DayOfWeek)
        If dt <= data Then dt = dt.AddDays(7)
        Return dt
    End Function

    Public Shared Function NextDow(ByVal dow As DayOfWeek) As DateTime
        Return NextDow(dow, DateTime.Today)
    End Function
    ''' <summary>
    ''' Finds the date of the nth specified day of week after the specified date 
    ''' or in the month containing the specified date
    ''' </summary>
    ''' <param name="startDate">the specified date, to start from or in the month to search</param>
    ''' <param name="n">number of the specified day of week</param>
    ''' <param name="dow">the specified day of week</param>
    ''' <param name="fromMonthFirst">flag to start from the first day of the month (default) or from the specified date</param>
    Public Shared Function NthDow(ByVal startDate As DateTime, ByVal n As Integer, ByVal dow As DayOfWeek, _
                    Optional ByVal fromMonthFirst As Boolean = True) As DateTime
        Dim dt As DateTime = startDate
        If fromMonthFirst Then dt = FirstDayInMonth(dt)
        For i As Integer = 1 To n
            dt = NextDow(dow, dt)
        Next
        Return dt
    End Function

    ''' <summary>
    ''' Calculates the number of specified day of week in the month containing the specified date
    ''' </summary>
    ''' <param name="dow">the specified day of week</param>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function CountDowInMonth(ByVal dow As DayOfWeek, ByVal data As DateTime) As Integer
        Dim cd As Integer
        Dim dt As DateTime
        dt = FirstDayInMonth(data)
        Do While dt.Month = data.Month
            dt = NextDow(dow, dt)
            If dt.Month = data.Month Then cd += 1
        Loop
        Return cd
    End Function

    Public Shared Function CountDowInMonth(ByVal dow As DayOfWeek) As Integer
        Return CountDowInMonth(dow, DateTime.Today)
    End Function
    ''' <summary>
    ''' Finds the next anniversary of the specified date
    ''' </summary>
    ''' <param name="data">A date representing a birthdate or anniversary</param>
    Public Shared Function NextAnniversary(ByVal data As DateTime) As DateTime
        Dim dt As New DateTime(DateTime.Today.Year, data.Month, data.Day)
        If dt < DateTime.Today Then dt = dt.AddYears(1)
        Return dt
    End Function

    ''' <summary>
    ''' Returns the first day in the month of the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function FirstDayInMonth(ByVal data As DateTime) As DateTime
        Return New DateTime(data.Year, data.Month, 1)
    End Function

    Public Shared Function FirstDayInMonth() As DateTime
        Return FirstDayInMonth(DateTime.Today)
    End Function
    ''' <summary>
    ''' Return the last day in the month of the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Use the current date, if none was specified</param>
    Public Shared Function LastDayInMonth(ByVal data As DateTime) As DateTime
        Return New DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month))
    End Function

    Public Shared Function LastDayInMonth() As DateTime
        Return LastDayInMonth(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the first day in the week containing the specified date
    ''' </summary>
    ''' <remarks>Uses localized settings for the first day of the week</remarks>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function FirstDayInWeek(ByVal data As DateTime) As DateTime
        Return data.AddDays(1 - data.DayOfWeek)
    End Function

    Public Shared Function FirstDayInWeek() As DateTime
        Return FirstDayInWeek(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the first day in the specified week of the specified year
    ''' </summary>
    ''' <remarks>Uses localized settings for the first day of the week</remarks>
    ''' <param name="week">the specified week</param>
    ''' <param name="year">the specified year. Uses the current year, if none was specified</param>
    Public Shared Function FirstDayInWeek(ByVal week As Integer, Optional ByVal year As Integer = 0) As DateTime
        Dim dt As DateTime, d As Integer
        If year = 0 Then year = DateTime.Today.Year
        dt = New DateTime(year, 1, 1)
        d = dt.DayOfWeek
        ' per determinare in che direzione cercare il primo lunedì
        ' si determina se il primo gennaio cade in un giorno prima o dopo giovedì
        ' poiché DayOfWeek.Sunday = 0 bisogna fare in modo che sembri 7
        If d = 0 Then d = 7
        If d < DayOfWeek.Thursday Then
            dt = dt.AddDays(7)
        Else
            week -= 1
        End If
        dt = FirstDayInWeek(dt)
        dt = dt.AddDays(7 * week)
        Return dt
    End Function

    ''' <summary>
    ''' Returns the last day in the week containing the specified date
    ''' </summary>
    ''' <remarks>Uses localized settings for the first day of the week</remarks>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function LastDayInWeek(ByVal data As DateTime) As DateTime
        Return data.AddDays(7 - data.DayOfWeek)
    End Function

    Public Shared Function LastDayInWeek() As DateTime
        Return LastDayInWeek(DateTime.Today)
    End Function

    ''' <summary>
    ''' Returns the first day in the quarter containing the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function FirstDayInQuarter(ByVal data As DateTime) As DateTime
        Return New DateTime(data.Year, ((data.Month - 1) \ 3) * 3 + 1, 1)
    End Function

    Public Shared Function FirstDayInQuarter() As DateTime
        Return FirstDayInQuarter(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the last day in the quarter containing the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Use the current date, if none was specified</param>
    Public Shared Function LastDayInQuarter(ByVal data As DateTime) As DateTime
        Return LastDayInMonth(FirstDayInQuarter(data).AddMonths(2))
    End Function
    Public Shared Function LastDayInQuarter() As DateTime
        Return LastDayInQuarter(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the Easter date in the specified year
    ''' </summary>
    ''' <param name="year">the specified year. Uses the current year if none was specified</param>
    Public Shared Function EasterDate(Optional ByVal year As Integer = 0) As DateTime
        Static dt As DateTime
        Dim G, C, H, i, j, L As Integer
        Dim m, d As Integer

        If year = 0 Then year = DateTime.Today.Year
        If dt.Year <> year Then
            G = year Mod 19
            C = year \ 100
            H = ((C - (C \ 4) - ((8 * C + 13) \ 25) + (19 * G) + 15) Mod 30)
            i = H - ((H \ 28) * (1 - (H \ 28) * (29 \ (H + 1)) * ((21 - G) \ 11)))
            j = ((year + (year \ 4) + i + 2 - C + (C \ 4)) Mod 7)
            L = i - j

            m = 3 + ((L + 40) \ 44)
            d = L + 28 - (31 * (m \ 4))
            dt = New DateTime(year, m, d)
        End If
        Return dt
    End Function

    ''' <summary>
    ''' Returns True if the specified date is an Italian holiday
    ''' </summary>
    ''' <param name="data">The specified date</param>
    Public Shared Function IsHoliday(ByVal data As DateTime) As Boolean
        Dim y As Integer = data.Year
        Dim m As Integer = data.Month
        Dim d As Integer = data.Day

        If m = 1 And d = 1 Or m = 1 And d = 6 Or m = 4 And d = 25 Or _
           m = 5 And d = 1 Or m = 6 And d = 2 Or m = 8 And d = 15 Or _
           m = 11 And d = 1 Or m = 12 And d = 8 Or m = 12 And d = 25 Or _
           m = 12 And d = 26 Or data.Equals(EasterDate(y).AddDays(1)) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns True if the specified date is Saturday or Sunday
    ''' </summary>
    ''' <param name="data">The specified date</param>
    Public Shared Function IsWeekend(ByVal data As DateTime) As Boolean
        If data.DayOfWeek = DayOfWeek.Saturday Or data.DayOfWeek = DayOfWeek.Sunday Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Skips weekend days, and holidays
    ''' </summary>
    ''' <param name="data">the date to start the skip</param>
    ''' <param name="forward">the direction to skip: forward if true (default), backward if false</param>
    ''' <returns>the first workday near the date specified (after or before, depending on forward); 
    ''' if the date is a workday, it is returned</returns>
    Public Shared Function SkipHolidays(ByVal data As DateTime, Optional ByVal forward As Boolean = True) As DateTime
        Dim d As Integer = 1
        If Not forward Then d = -1
        Do While IsHoliday(data) Or IsWeekend(data)
            data = data.AddDays(d)
        Loop
        Return data
    End Function

    ''' <summary>
    ''' Returns the first working day in the month of the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function FirstWorkdayInMonth(ByVal data As DateTime) As DateTime
        Return SkipHolidays(FirstDayInMonth(data), True)
    End Function
    Public Shared Function FirstWorkdayInMonth() As DateTime
        Return FirstWorkdayInMonth(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the last working day in the month containing the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Use the current date, if none was specified</param>
    Public Shared Function LastWorkdayInMonth(ByVal data As DateTime) As DateTime
        Return SkipHolidays(LastDayInMonth(data), False)
    End Function
    Public Shared Function LastWorkdayInMonth() As DateTime
        Return LastWorkdayInMonth(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the next working day after the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Use the current date, if none was specified</param>
    Public Shared Function NextWorkday(ByVal data As DateTime) As DateTime
        Return SkipHolidays(data.AddDays(1), True)
    End Function
    Public Shared Function NextWorkday() As DateTime
        Return NextWorkday(DateTime.Today)
    End Function
    ''' <summary>
    ''' Returns the previous working day before the specified date
    ''' </summary>
    ''' <param name="data">The specified date. Use the current date, if none was specified</param>
    Public Shared Function PreviousWorkday(ByVal data As DateTime) As DateTime
        Return SkipHolidays(data.AddDays(-1), False)
    End Function
    Public Shared Function PreviousWorkday() As DateTime
        Return PreviousWorkday(DateTime.Today)
    End Function
    ''' <summary>
    ''' Counts the business days (not counting weekends/holidays) in a given date range
    ''' </summary>
    ''' <param name="startDate">Date specifying the start of the range, must be lesser than the endDate</param>
    ''' <param name="endDate">Date specifying the end of the range, must be greater than startDate</param>
    Public Shared Function CountWorkdays(ByVal startDate As DateTime, ByVal endDate As DateTime) As Integer
        Dim wd As Integer
        Do
            startDate = SkipHolidays(startDate, True)
            If startDate <= endDate Then wd += 1
            startDate = startDate.AddDays(1)
        Loop Until (startDate > endDate)
        Return wd
    End Function

    ''' <summary>
    ''' Counts holidays in a given date range
    ''' </summary>
    ''' <param name="startDate">Date specifying the start of the range, must be lesser than the endDate</param>
    ''' <param name="endDate">Date specifying the end of the range, must be greater than startDate</param>
    Public Shared Function CountHolidays(ByVal startDate As DateTime, ByVal endDate As DateTime) As Integer
        Return endDate.Subtract(startDate).Days + 1 - CountWorkdays(startDate, endDate)
    End Function

    ''' <summary>
    ''' Returns the age of a person, at the specified date
    ''' </summary>
    ''' <param name="birthday">The birthday date of the person, must be lesser than the atDate</param>
    ''' <param name="atDate">The specified date. Uses the current date, if none was specified</param>
    Public Shared Function Age(ByVal birthday As DateTime, ByVal atDate As DateTime) As Integer
        Return (atDate.Subtract(birthday.AddDays(-1)).Days \ 365)
    End Function
    Public Shared Function Age(ByVal birthday As DateTime) As Integer
        Return Age(birthday, DateTime.Today)
    End Function


    'Roberto Rutigliano 15/03/2011
    Public Shared Function decodeEnglishMonth(ByVal value As Int16) As String
        Dim valore As String

        Select Case value
            Case 1
                valore = "January"
            Case 2
                valore = "February"
            Case 3
                valore = "March"
            Case 4
                valore = "April"
            Case 5
                valore = "May"
            Case 6
                valore = "June"
            Case 7
                valore = "July"
            Case 8
                valore = "August"
            Case 9
                valore = "September"
            Case 10
                valore = "October"
            Case 11
                valore = "November"
            Case 12
                valore = "December"
            Case Else
                Throw New MyManager.ManagerException("Mese non gestito: " & value)
        End Select
        Return valore
    End Function

End Class
