Public Class ProgettiManager
    Inherits Manager


    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Function lockPianificazione(ByVal pianificazioneId As Long, ByVal userId As Long) As Boolean
        Dim sqlQuery As String
        sqlQuery = "UPDATE PREVENTIVI SET [date_disabled] =  now(),  user_id_disabled = " & userId & _
                            " WHERE  preventivo_id = " & pianificazioneId
        Me.mExecuteNoQuery(sqlQuery)
        Return True
    End Function


    Public Function unlockPianificazione(ByVal pianificazioneId As Long, ByVal userId As Long) As Boolean
        Dim sqlQuery As String
        sqlQuery = "UPDATE PREVENTIVI SET [date_disabled] =  NULL,  user_id_disabled = " & userId & _
                            " WHERE  preventivo_id = " & pianificazioneId
        Me.mExecuteNoQuery(sqlQuery)
        Return True
    End Function



    Public Function pianificaDalAl(ByVal userId As Long, ByVal codiceCommessa As String, ByVal dataIniziale As DateTime, ByVal dataFinale As DateTime) As Integer

    End Function

    Public Function completaPianificazione(ByVal pianificazioneId As Long, ByVal codiceCommessa As String) As Int16
        Dim dataTable As System.Data.DataTable = getPreventivo(pianificazioneId)
        Dim _row As System.Data.DataRow = dataTable.Rows(0)

        Return completaPianificazione(pianificazioneId, _row("anno"), _row("mese"), codiceCommessa)
    End Function

    Public Function completaPianificazione(ByVal pianificazioneId As Long, ByVal anno As Int16, ByVal mese As Int16, ByVal codiceCommessa As String) As Int16
        'completo la pianificazione del mese inserendo la "Commessa" solo per i gioni in cui non ho 
        'pianificato nulla!
        Dim sqlQuery As String

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.Connection = Me.mConnection

        Me.mAddParameter(command, "@NEWCODICE ", codiceCommessa)

        Dim i As Int16
        Dim conta As Int16 = 0
        Dim flag As Boolean
        Dim myData As Date

        For i = 1 To 31
            flag = True
            Try
                myData = New Date(anno, mese, i)
            Catch ex As Exception
                flag = False
            End Try

            If flag Then
                If myData.DayOfWeek <> DayOfWeek.Sunday AndAlso _
                     myData.DayOfWeek <> DayOfWeek.Saturday AndAlso _
                    Not MyManager.DateTimeManager.IsHoliday(myData) Then

                    sqlQuery = "UPDATE PREVENTIVI SET [" & i & "M] = ?" & _
                          " WHERE ([" & i & "M] ='' or [" & i & "M] is NULL)   and preventivo_id =" & pianificazioneId
                    command.CommandText = sqlQuery
                    conta = conta + command.ExecuteNonQuery()

                    sqlQuery = "UPDATE PREVENTIVI SET [" & i & "P] = ?" & _
                          " WHERE ([" & i & "P] ='' or [" & i & "P] is NULL) and preventivo_id = " & pianificazioneId
                    command.CommandText = sqlQuery
                    conta = conta + command.ExecuteNonQuery()

                End If
            End If
        Next

        Return conta

    End Function



    Public Function insertPreventivoPerTutti(ByVal mese As Int16, ByVal anno As Int16) As Int16
        Dim strSql As String
        strSql = "SELECT user_id from UTENTI"

        Dim conta As Int16 = 0
        'prelevo tutti gli utenti!!
        Dim dataTable As System.Data.DataTable
        dataTable = Me._fillDataSet(strSql).Tables(0)

        Dim row As System.Data.DataRow

        For Each row In dataTable.Rows
            If Me.checkInsertNewPreventivo(anno, mese, row("user_id")) Then
                Me.insertPreventivo(row("user_id"), mese, anno)
                conta = conta + 1
            End If
        Next

        Return conta
    End Function




    Public Function copyPreventivo(ByVal userIdSRC As Long, ByVal meseSRC As Int16, ByVal annoSRC As Int16, _
                ByVal userIdDST As Long, ByVal meseDST As Int16, ByVal annoDST As Int16) As Long

        'leggo i valori della SOURCE
        Dim dataTableSRC As System.Data.DataTable
        dataTableSRC = Me.getPreventivo(userIdSRC, meseSRC, annoSRC)


        Dim strSql_1, strSql_2 As String
        strSql_1 = "INSERT INTO PREVENTIVI (MESE, ANNO, FK_USER_ID "

        strSql_2 = " VALUES (" & meseDST & "," & annoDST & ", " & userIdDST

        Dim i As Integer
        For i = 1 To 31
            strSql_1 &= ",[" & i & "M], [" & i & "P]"
            strSql_2 &= ",'" & dataTableSRC.Rows(0)(i & "M") & "','" & dataTableSRC.Rows(0)(i & "P") & "' "
        Next

        Dim strSQL As String = strSql_1 & " )" & strSql_2 & " )"
        Me.mExecuteNoQuery(strSQL)

        Return 0
    End Function



    Public Function insertPreventivo(ByVal userId As Long, ByVal mese As Int16, ByVal anno As Int16)
        Dim strSQL As String
        strSQL = "INSERT INTO PREVENTIVI (MESE, ANNO, FK_USER_ID ) " & _
                 " VALUES (" & mese & "," & anno & ", " & userId & ")"

        Me.mExecuteNoQuery(strSQL)
        Return True
    End Function


    Public Function insertConsuntivo(ByVal userId As Long, ByVal mese As Int16, ByVal anno As Int16, ByVal progettoId As Long) As Long
        Dim strSQL As String
        strSQL = "INSERT INTO CONSUNTIVI ( FK_USER_ID, MESE, ANNO, FK_PROGETTO_ID)" & _
            " VALUES (@FK_USER_ID ,@MESE ,@ANNO ,@FK_PROGETTO_ID )"

        '*** STATO = APERTO
        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me.mConnection

        Me.mAddParameter(command, "@FK_USER_ID", userId)
        Me.mAddParameter(command, "@MESE", mese)
        Me.mAddParameter(command, "@ANNO", anno)
        Me.mAddParameter(command, "@FK_PROGETTO_ID", progettoId)

        Me.mExecuteNoQuery(command)

        'Dim newKey As Long
        'newKey = Me._getIdentity()

        Return 0
    End Function

    Public Function deleteConsuntivo(ByVal consuntivoId As Long) As Integer
        Dim strSQL As String = "DELETE * FROM CONSUNTIVI WHERE CONSUNTIVO_ID=" & consuntivoId
        Return Me.mExecuteNoQuery(strSQL)
    End Function

    Public Function getPreventivo(ByVal preventivoId As Long) As DataTable
        Dim sqlQuery As String = "SELECT * FROM PREVENTIVI WHERE preventivo_id = " & preventivoId
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function


    Public Function getNoteByTag(ByVal preventivoId As Long, ByVal tag As String) As DataTable
        Dim sqlQuery As String = ""

        Dim i As Integer
        For i = 1 To 31
            sqlQuery &= ", nota" & i
        Next


        sqlQuery = "SELECT anno, mese " & sqlQuery & " FROM PREVENTIVI WHERE preventivo_id = " & preventivoId
        Dim dataTabe As System.Data.DataTable = _fillDataSet(sqlQuery).Tables(0)
        Dim row As System.Data.DataRow = dataTabe.Rows(0)

        Dim testo As String

        '<TAG\b[^>]*>(.*?)</TAG>
        Dim pattern As New System.Text.RegularExpressions.Regex("<" & tag & "\b[^>]*>(.*?)</" & tag & ">")

        For i = 1 To 31
            'cero il testo compreso tra <tag> e </tag>
            If Not IsDBNull(row("nota" & i)) Then
                testo = row("nota" & i)
                testo = pattern.Match(testo).Value
                If Not String.IsNullOrEmpty(testo) Then
                    testo = testo.Replace("<" & tag & ">", "")
                    testo = testo.Replace("</" & tag & ">", "")
                End If
                row("nota" & i) = testo
            End If

        Next

        Return dataTabe
    End Function



    Public Function getPreventivo(ByVal userId As Long, ByVal mese As Int16, ByVal anno As Int16) As DataTable
        Dim sqlQuery As String = "SELECT * FROM PREVENTIVI WHERE fk_user_id = " & userId & " and mese=" & mese & " and anno = " & anno
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function


    Public Function getCommessaCliente(ByVal codiceCommessa As String) As String
        Dim sqlQuery As String = "SELECT Cliente FROM V_ELENCO_Commesse WHERE ucase(NOME) = '" & codiceCommessa.ToUpper & "'"
        Return mExecuteScalar(sqlQuery)
    End Function


    Public Function getCommessaDescrizione(ByVal codiceCommessa As String) As String
        Dim sqlQuery As String = "SELECT DESCRIZIONE FROM Commesse WHERE ucase(NOME) = '" & codiceCommessa.ToUpper & "'"
        Return mExecuteScalar(sqlQuery)
    End Function

    Public Function getCliente(ByVal clienteId As Long) As DataTable
        Dim sqlQuery As String = "SELECT * FROM CLIENTI WHERE cliente_id = " & clienteId
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function

    Public Function getCommessa(ByVal commessaId As Long) As DataTable
        Dim sqlQuery As String = "SELECT * FROM Commesse WHERE commessa_id = " & commessaId
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function

    Public Function getCommesse() As DataTable
        Dim sqlQuery As String = "SELECT CLIENTI.nome as Cliente, commessa_id, COMMESSE.nome, COMMESSE.descrizione, background_color, fore_color , COMMESSE.fk_tipo_id FROM COMMESSE, CLIENTI WHERE  CLIENTI.cliente_id = COMMESSE.fk_cliente_id   "
        Return _fillDataSet(sqlQuery).Tables(0)
    End Function

    Public Function deletePreventivo(ByVal preventivoId As Long) As Integer
        Dim strSQL As String = "DELETE * FROM PREVENTIVI WHERE PREVENTIVO_ID=" & preventivoId
        Return Me.mExecuteNoQuery(strSQL)
    End Function

    Public Function checkInsertNewPreventivo(ByVal anno As Int16, ByVal mese As Int16, ByVal user_id As Long) As Boolean
        Dim sqlQuery As String = "SELECT count(*) from preventivi where anno = " & anno & " and mese = " & mese & " and fk_user_id = " & user_id
        Dim esito As Int16
        esito = Me.mExecuteScalar(sqlQuery)
        Return (esito = 0)
    End Function

    Public Function checkCliente(ByVal nome As String) As Boolean
        Dim sqlQuery As String = "SELECT count(*) from CLIENTI where ucase(nome) = ?"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = sqlQuery
        Me.mAddParameter(command, "@nome ", nome.ToUpper)

        Dim esito As Int16
        esito = Me.mExecuteScalar(command)
        Return esito = 0
    End Function

    'Public Function checkCopiaPreventivo(ByVal annoSRC As Int16, ByVal meseSRC As Int16, ByVal user_idSRC As Long) As Boolean
    '    Dim sqlQuery As String = "SELECT count(*) from preventivi where anno = " & anno & " and mese = " & mese & " and fk_user_id = " & user_id
    '    Dim esito As Int16
    '    esito = Me.mExecuteScalar(sqlQuery)
    '    Return esito = 0
    'End Function



    Public Function spostaCommessaPerUnUtente(ByVal userId As Long, ByVal oldName As String, ByVal newName As String) As Int16
        'la funzione consente di modificare le pianificazione di un utente... 
        'TUTTE le sue pianificazioni su una determinata commessa vengono cambiate con un codice nuovo
        Dim sqlQuery As String
        Dim conta As Int16 = 0

        'a questo punto devo rinominare TUTTI i preventivi che usano questa commessa

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.Connection = Me.mConnection

        Me.mAddParameter(command, "@NEWCODICE ", newName)
        Dim tempParameter As System.Data.Common.DbParameter
        tempParameter = Me.mAddParameter(command, "@OLDCODICE", oldName)


        Dim i As Int16
        For i = 1 To 31
            sqlQuery = "UPDATE PREVENTIVI SET [" & i & "M] = ?" & _
                  " WHERE [" & i & "M] = ?  AND FK_USER_ID =" & userId
            command.CommandText = sqlQuery
            conta = conta + command.ExecuteNonQuery()

            sqlQuery = "UPDATE PREVENTIVI SET [" & i & "P] = ?" & _
                  " WHERE [" & i & "P] = ? AND FK_USER_ID =" & userId
            command.CommandText = sqlQuery
            conta = conta + command.ExecuteNonQuery()
        Next

        Return conta
    End Function






    Public Function renameCommessa(ByVal commessaId As Long, ByVal newName As String) As Boolean
        Dim oldName As String
        Dim sqlQuery As String = "SELECT nome from COMMESSE where commessa_id = " & commessaId
        oldName = Me.mExecuteScalar(sqlQuery)

        'a questo punto devo rinominare TUTTI i preventivi che usano questa commessa

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.Connection = Me.mConnection
        command.CommandText = sqlQuery

        Me.mAddParameter(command, "@NEWCODICE", newName)
        Dim tempParameter As System.Data.Common.DbParameter
        tempParameter = Me.mAddParameter(command, "@OLDCODICE", oldName)


        Dim i As Int16
        For i = 1 To 31
            sqlQuery = "UPDATE PREVENTIVI SET [" & i & "M] = ?" & _
                  " WHERE [" & i & "M] = ? "
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()

            sqlQuery = "UPDATE PREVENTIVI SET [" & i & "P] = ?" & _
                  " WHERE [" & i & "P] = ? "
            command.CommandText = sqlQuery
            command.ExecuteNonQuery()
        Next


        sqlQuery = "UPDATE COMMESSE SET [NOME] = ? " & _
                " WHERE [commessa_id] =  " & commessaId
        command.CommandText = sqlQuery
        'rimuovo il parametro che non mi serve
        command.Parameters.Remove(tempParameter)
        Me.mExecuteNoQuery(command)

        Return True
    End Function



    Public Function checkCommessa(ByVal nome As String) As Boolean
        Dim sqlQuery As String = "SELECT count(*) from COMMESSE where ucase(nome) = ?"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = sqlQuery
        Me.mAddParameter(command, "@nome", nome.ToUpper)

        Dim esito As Int16
        esito = Me.mExecuteScalar(command)
        Return (esito = 0)
    End Function



    'sulla commessa non posso fare l'update del nome perchè viene 
    'utilizzato per fare i calcoli
    Public Function updateCommessa(ByVal commessaId As Long, _
        ByVal clienteId As Long, _
        ByVal tipoId As Long, _
        ByVal statoId As Long, _
        ByVal descrizione As String, _
        ByVal note As String, _
        ByVal backgroundColor As String, _
        ByVal businessUnit As Long, _
        ByVal capoProgetto As Long, _
        ByVal foreColor As String, ByVal orePreventivate As Int16) As Boolean

        Dim strSQL As String = "UPDATE COMMESSE SET DATE_MODIFIED = now " & _
                    ",FK_CLIENTE_ID = ? " & _
                    ",FK_TIPO_ID = ? " & _
                    ",FK_STATO_ID = ? " & _
                    ",DESCRIZIONE = ? " & _
                    ",NOTA = ? " & _
                    ",BACKGROUND_COLOR = ? " & _
                    ",FORE_COLOR = ? " & _
                    ",ORE_PREVENTIVATE = ? " & _
                    ",USER_ID = ? " & _
                    ",BUSINESS_UNIT = ? " & _
                    " WHERE COMMESSA_ID = ? "



        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()

        Me.mAddParameter(command, "@FK_CLIENTE_ID", clienteId)
        Me.mAddParameter(command, "@FK_TIPO_ID", tipoId)
        Me.mAddParameter(command, "@FK_STATO_ID", statoId)
        'Me.mAddParameter(command, "@NOME", nome)
        Me.mAddParameter(command, "@DESCRIZIONE", descrizione)
        Me.mAddParameter(command, "@NOTE", note)
        Me.mAddParameter(command, "@BACKGROUND_COLOR", backgroundColor)
        Me.mAddParameter(command, "@FORE_COLOR", foreColor)
        Me.mAddParameter(command, "@ORE_PREVENTIVATE", orePreventivate)

        If capoProgetto = -1 Then
            Me.mAddParameter(command, "@USER_ID", DBNull.Value)
        Else
            Me.mAddParameter(command, "@USER_ID", capoProgetto)
        End If


        If businessUnit = -1 Then
            Me.mAddParameter(command, "@BUSINESS_UNIT", DBNull.Value)
        Else
            Me.mAddParameter(command, "@BUSINESS_UNIT", businessUnit)
        End If



        Me.mAddParameter(command, "@COMMESSA_ID", commessaId)

        command.CommandText = strSQL
        Dim numeroRecord As Integer = command.ExecuteNonQuery()

        Return numeroRecord > 0
    End Function


    Public Function insertCliente(ByVal nome As String) As Long
        Dim strSQL As String
        strSQL = "INSERT INTO CLIENTI (NOME )" & _
                 " VALUES (@NOME )"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me.mConnection

        Me.mAddParameter(command, "@NOME", nome)
        Me.mExecuteNoQuery(command)

        Return Me._getIdentity()
    End Function






    Public Function insertCommessa(ByVal clienteId As Long, _
        ByVal tipoId As Long, _
        ByVal statoId As Long, _
        ByVal nome As String, _
        ByVal descrizione As String, _
        ByVal note As String, _
        ByVal backgroundColor As String, _
         ByVal businessUnit As Long, _
        ByVal capoProgetto As Long, _
        ByVal foreColor As String, ByVal orePreventivate As Int16) As Long

        Dim strSQL As String
        strSQL = "INSERT INTO COMMESSE (FK_CLIENTE_ID, FK_TIPO_ID, FK_STATO_ID , NOME, DESCRIZIONE, NOTA, BACKGROUND_COLOR, FORE_COLOR, ORE_PREVENTIVATE, USER_ID, BUSINESS_UNIT )" & _
            " VALUES (@FK_CLIENTE_ID ,@FK_TIPO_ID ,@FK_STATO_ID ,@NOME ,@DESCRIZIONE ,@NOTE ,@BACKGROUND_COLOR ,@FORE_COLOR ,@ORE_PREVENTIVATE ,@USER_ID ,@BUSINESS_UNIT )"

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = strSQL
        command.Connection = Me.mConnection

        Me.mAddParameter(command, "@FK_CLIENTE_ID ", clienteId)
        Me.mAddParameter(command, "@FK_TIPO_ID ", tipoId)
        Me.mAddParameter(command, "@FK_STATO_ID ", statoId)
        Me.mAddParameter(command, "@NOME ", nome)
        Me.mAddParameter(command, "@DESCRIZIONE", descrizione)
        Me.mAddParameter(command, "@NOTE", note)
        Me.mAddParameter(command, "@BACKGROUND_COLOR", backgroundColor)
        Me.mAddParameter(command, "@FORE_COLOR", foreColor)
        Me.mAddParameter(command, "@ORE_PREVENTIVATE", orePreventivate)


        If capoProgetto = -1 Then
            Me.mAddParameter(command, "@USER_ID", DBNull.Value)
        Else
            Me.mAddParameter(command, "@USER_ID", capoProgetto)
        End If

        If businessUnit = -1 Then
            Me.mAddParameter(command, "@BUSINESS_UNIT", DBNull.Value)
        Else
            Me.mAddParameter(command, "@BUSINESS_UNIT", businessUnit)
        End If



        Me.mExecuteNoQuery(command)

        Return Me._getIdentity()
    End Function

    Public Function deleteCommessa(ByVal commessaId As Long) As Integer
        Dim strSQL As String = "DELETE * FROM COMMESSE WHERE COMMESSA_ID=" & commessaId
        Return Me.mExecuteNoQuery(strSQL)
    End Function



    Public Function getStatinoInfo(ByVal preventivoId As Long) As System.Data.DataTable

        Dim sqlQuery As String = "SELECT * FROM PREVENTIVI WHERE preventivo_id = " & preventivoId
        Dim dataTable As System.Data.DataTable
        Dim row As System.Data.DataRow
        dataTable = _fillDataSet(sqlQuery).Tables(0)
        row = dataTable.Rows(0)

        Dim elencoCommesse As New Hashtable
        Dim i As Int16
        Dim key As String
        Dim ore As String

        For i = 1 To 31
            key = row(i & "M").ToString
            If Not String.IsNullOrEmpty(key) Then
                If elencoCommesse.ContainsKey(key) Then
                    ore = elencoCommesse(key)
                    elencoCommesse.Remove(key)
                    elencoCommesse.Add(key, ore + 1)
                Else
                    elencoCommesse.Add(key, 1)
                End If
            End If

            key = row(i & "P").ToString
            If Not String.IsNullOrEmpty(key) Then
                If elencoCommesse.ContainsKey(key) Then
                    ore = elencoCommesse(key)
                    elencoCommesse.Remove(key)
                    elencoCommesse.Add(key, ore + 1)
                Else
                    elencoCommesse.Add(key, 1)
                End If
            End If

        Next


        Dim risultato As New System.Data.DataTable
        risultato.Columns.Add("CODICE")
        risultato.Columns.Add("CLIENTE")
        risultato.Columns.Add("COMMESSA")
        risultato.Columns.Add("ORE")
        risultato.Columns.Add("GIORNI")

        Dim en As IDictionaryEnumerator
        en = elencoCommesse.GetEnumerator
        Dim newRow As System.Data.DataRow

        Dim totaleOre As Int16 = 0
        While en.MoveNext
            newRow = risultato.NewRow
            newRow("CODICE") = en.Key
            newRow("COMMESSA") = getCommessaDescrizione(en.Key)
            newRow("CLIENTE") = getCommessaCliente(en.Key)
            newRow("ORE") = en.Value * 4
            newRow("GIORNI") = newRow("ORE") / 8
            totaleOre = totaleOre + en.Value
            risultato.Rows.Add(newRow)

        End While

        If totaleOre > 0 Then
            newRow = risultato.NewRow
            newRow("COMMESSA") = "TOTALE"
            newRow("ORE") = totaleOre * 4
            newRow("GIORNI") = newRow("ORE") / 8
            risultato.Rows.Add(newRow)
        End If

        Return risultato
    End Function




    Public Function getCommessaInfo(ByVal commessa As String) As System.Data.DataTable
        Dim strSQL As String
        strSQL = "select  Utenti.nome, *  from  Utenti INNER JOIN preventivi ON Utenti.user_id = preventivi.fk_user_id WHERE "
        Dim i As Int16

        Dim strWhere As String = ""
        For i = 1 To 31
            strWhere &= "OR [" & i & "M]='" & commessa & "' OR [" & i & "P]='" & commessa & "' "
        Next

        strSQL = strSQL & strWhere.Substring(2) & " ORDER BY anno DESC, mese DESC , fk_user_id"

        Dim tempDataTable As System.Data.DataTable
        tempDataTable = Me._fillDataSet(strSQL).Tables(0)


        Dim risultato As New System.Data.DataTable
        risultato.Columns.Add("ANNO")
        risultato.Columns.Add("MESE")
        risultato.Columns.Add("USERS")
        risultato.Columns.Add("ORE")

        'ho l'elenco di tutti i preventivi che hanno scaricato su questa commessa
        Dim row As System.Data.DataRow
        Dim newRow As System.Data.DataRow
        Dim contaOreMensili As Int16 = 0
        Dim contaOreUtente As Int16 = 0

        Dim users As String = ""
        newRow = risultato.NewRow
        For Each row In tempDataTable.Rows
            If newRow.IsNull("ANNO") OrElse (newRow("ANNO") <> row("ANNO") Or newRow("MESE") <> row("MESE")) Then
                'rottura di codice!!!!
                If Not String.IsNullOrEmpty(users) Then
                    newRow("USERS") = users.Substring(1)
                    newRow("ORE") = contaOreMensili
                    risultato.Rows.Add(newRow)
                    users = ""
                    contaOreMensili = 0
                End If
                newRow = risultato.NewRow
                newRow("ANNO") = row("ANNO")
                newRow("MESE") = row("MESE")
                contaOreUtente = contaOre(row, commessa)
                contaOreMensili = contaOreMensili + contaOreUtente
                users &= ";" & row("fk_user_id") & "@" & row("NOME") & "@" & contaOreUtente
                'users &= makeHtmlRow(row)
            Else
                contaOreUtente = contaOre(row, commessa)
                contaOreMensili = contaOreMensili + contaOreUtente

                users &= ";" & row("fk_user_id") & "@" & row("NOME") & "@" & contaOreUtente
                ' users &= makeHtmlRow(row)
            End If
        Next

        If Not String.IsNullOrEmpty(users) Then
            newRow("USERS") = users.Substring(1)
            newRow("ORE") = contaOreMensili
            risultato.Rows.Add(newRow)
            users = ""
            contaOreMensili = 0
        End If

        Return risultato

    End Function

    'Public Shared Sub alertOre(ByVal oreTotali As Int16, ByRef oreConsumate As Int16, ByVal label As System.Web.UI.WebControls.Label)
    '    Dim differenza As Int16 = (oreTotali - oreConsumate)
    '    Dim percentuale As Double = (oreConsumate * 100) / oreTotali
    '    Dim messaggio As String
    '    messaggio = oreTotali & "h - " & oreConsumate & "h = " & differenza & "h "
    '    messaggio &= "<br />" & oreTotali / 8 & "g - " & oreConsumate / 8 & "g = " & differenza / 8 & "g"
    '    messaggio &= "<br /> <br />" & "(" & FormatNumber(percentuale, 0) & "% )"

    '    label.Text = messaggio
    '    label.Font.Size = 17
    '    'label.Height = 20

    '    If (percentuale) < 50 Then
    '        label.BackColor = Drawing.Color.Green
    '    ElseIf (percentuale < 75) Then
    '        label.BackColor = Drawing.Color.Orange
    '    Else
    '        label.BackColor = Drawing.Color.Red
    '    End If


    '    End Sub

    Private Function contaOre(ByVal row As System.Data.DataRow, ByVal commessa As String) As Int16
        Dim i As Int16
        Dim conta = 0
        Dim j As Int16 = row.Table.Columns.Count - 1
        For i = 0 To j
            If row(i).ToString = commessa Then
                conta = conta + 1
            End If
        Next

        Return conta * 4
    End Function



End Class
