Public Class ConsoleManager
    Inherits Manager
    'Classe base per la gestione di applicazioni di tipo Console




    Public Shared Function calcolaTempoDiElaborazione(ByVal dataIniziale As DateTime, ByVal dataFinale As DateTime) As String

        Dim risultato As String = ""
        'Dim secondi As Long
        'Dim minuti As Long
        'Dim ore As Long
        'Dim giorni As Long



        Dim ts As TimeSpan = dataFinale.Subtract(dataIniziale)

        If ts = TimeSpan.Zero Then
            Return "0"
        End If

        If ts.Days > 0 Then
            risultato = String.Concat(ts.Days, "g ")
        End If

        If ts.Hours > 0 Then
            risultato = String.Concat(risultato, ts.Hours, "h ")
        End If

        If ts.Minutes > 0 Then
            risultato = String.Concat(risultato, ts.Minutes, "' ")
        Else
            If Not String.IsNullOrEmpty(risultato) AndAlso ts.Seconds > 0 Then
                risultato = String.Concat(risultato, "0' ")
            End If
        End If


        If ts.Seconds > 0 Then
            risultato = String.Concat(risultato, ts.Seconds)
            If ts.Milliseconds > 0 Then
                risultato = String.Concat(risultato, ",", ts.Milliseconds)
            End If
            risultato = String.Concat(risultato, "''")
        Else
            If ts.Milliseconds > 0 Then
                risultato = String.Concat(risultato, "0,", ts.Milliseconds, "''")
            End If
        End If

        Return risultato






        'secondi = DateDiff(DateInterval.Second, dataIniziale, dataFinale)
        'If (secondi >= 60) Then
        '    minuti = (secondi / 60)
        '    ' risultato = (secondi Mod 60) & "''"
        '    risultato = dataFinale.Subtract(dataIniziale).Seconds & "''"
        'Else
        '    Return dataFinale.Subtract(dataIniziale).TotalSeconds & "''"
        'End If

        'If (minuti > 60) Then
        '    ore = minuti / 60
        '    risultato = (minuti Mod 60) & "' " & risultato
        'Else
        '    Return minuti & "' " & risultato
        'End If

        'If (ore > 24) Then
        '    giorni = ore / 24
        '    risultato = (ore Mod 24) & "' " & risultato
        'Else
        '    Return ore & "h " & risultato
        'End If


        'Return giorni & "g " & risultato

    End Function


    'inserisco gli argomenti in un hashtable
    Public Shared Function ParseArguments(ByVal args() As String) As Hashtable
        Dim arg As String
        Dim valore As String
        Dim conta As Integer

        Dim _argsHashtable As New Hashtable

        For conta = 0 To args.Length - 1
            arg = args(conta).Trim.ToUpper

            ' Trace.WriteLine("arg: " & arg)
            'il primo argomento deve essere un parametro -XXX
            If (arg.StartsWith("-")) Then
                'leggo il suo valore... 
                If (conta + 1 < args.Length) Then
                    'controllo sul valore successivo...
                    If (args(conta + 1) Is Nothing) Then
                        valore = ""
                    ElseIf (args(conta + 1).StartsWith("-")) Then
                        valore = ""
                    Else
                        valore = args(conta + 1).Trim.ToUpper
                        'incremento il contatore di 2
                        conta = conta + 1
                    End If
                Else
                    valore = ""
                End If

                'If (valore.StartsWith("-")) Then
                'Return False
                'End If
                'aggiungo il parametro nell'hashtable...
                _argsHashtable.Add(arg, valore)

            End If
        Next
        Return _argsHashtable
    End Function


End Class
