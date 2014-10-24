Public Class CatalogoManager
    Inherits Manager
    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub

    ' Public Sub New(ByVal connectionString As String)
    '     MyBase.New(connectionString)
    ' End Sub

    Public Sub New()
        MyBase.New("catalogo")
    End Sub

    Public _isPubblico As Boolean = False
    Public _isUsato As Boolean = False
    Public _descrizione As String
    Public _nome As String
    Public _codice As String
    Public _scheda As String
    Public _tipoDiOggetto As Integer
    Public _prezzo As Double
    Public _prezzoAlPubblico As Double
    Public _prezzoSpeciale As Double
    Public _photo1 As String
    Public _photo2 As String
    Public _photo3 As String
    Public _photo4 As String


    Public Sub addClick(ByVal oggettoId As Long)
        Dim sqlQuery As String = "UPDATE OGGETTO SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 " & _
                                            " WHERE OGGETTO_ID=" & oggettoId
        Me._executeNoQuery(sqlQuery)
    End Sub



    Public Function getTipoDiOggetto() As Data.DataSet
        Dim sqlQuery As String
        sqlQuery = "SELECT TIPO_ID, NOME, DESCRIZIONE FROM  TIPOOGGETTO"

        Return _fillDataSet(sqlQuery)
    End Function

    Public Function insert() As Long
        Dim strSQL As String
        Dim strSQLParametri As String

        strSQL = "INSERT INTO OGGETTO (DATE_ADDED, DATE_MODIFIED "
        strSQLParametri = " VALUES ( Now(), Now() "

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)



        strSQL &= ",ISPUBLIC "
        strSQLParametri &= ",@ISPUBLIC "
        oleDbCommand.Parameters.Add("@ISPUBLIC", OleDb.OleDbType.Boolean).Value = _isPubblico

        strSQL &= ",ISUSATO "
        strSQLParametri &= ",@ISUSATO "
        oleDbCommand.Parameters.Add("@ISUSATO", OleDb.OleDbType.Boolean).Value = _isUsato


        If _descrizione <> "" Then
            strSQL &= ",DESCRIZIONE "
            strSQLParametri &= ",@DESCRIZIONE "
            oleDbCommand.Parameters.Add("@DESCRIZIONE", OleDb.OleDbType.VarChar).Value = _descrizione
        End If

        If _nome <> "" Then
            strSQL &= ",NOME "
            strSQLParametri &= ",@NOME "
            oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = _nome
        End If

        If _scheda <> "" Then
            strSQL &= ",SCHEDA "
            strSQLParametri &= ",@SCHEDA "
            oleDbCommand.Parameters.Add("@SCHEDA", OleDb.OleDbType.VarChar).Value = _scheda
        End If

        If _codice <> "" Then
            strSQL &= ",CODICE_PRODOTTO "
            strSQLParametri &= ",@CODICE_PRODOTTO "
            oleDbCommand.Parameters.Add("@CODICE_PRODOTTO", OleDb.OleDbType.VarChar).Value = _codice
        End If


        If _tipoDiOggetto > 0 Then
            strSQL &= ",TIPO_ID "
            strSQLParametri &= ",@TIPO_ID "
            oleDbCommand.Parameters.Add("@TIPO_ID", OleDb.OleDbType.Numeric).Value = _tipoDiOggetto
        End If

        If _prezzo > 0 Then
            strSQL &= ",PREZZO "
            strSQLParametri &= ",@PREZZO "
            oleDbCommand.Parameters.Add("@PREZZO", OleDb.OleDbType.Numeric).Value = _prezzo
        End If

        If _prezzoAlPubblico > 0 Then
            strSQL &= ",PREZZOALPUBBLICO "
            strSQLParametri &= ",@PREZZOALPUBBLICO "
            oleDbCommand.Parameters.Add("@PREZZOALPUBBLICO", OleDb.OleDbType.Numeric).Value = _prezzoAlPubblico
        End If

        If _prezzoSpeciale > 0 Then
            strSQL &= ",PREZZO_SPECIALE "
            strSQLParametri &= ",@PREZZO_SPECIALE "
            oleDbCommand.Parameters.Add("@PREZZO_SPECIALE", OleDb.OleDbType.Numeric).Value = _prezzoSpeciale
        End If


        If _photo1 <> "" Then
            strSQL &= ",PHOTO1 "
            strSQLParametri &= ",@PHOTO1 "
            oleDbCommand.Parameters.Add("@PHOTO1", OleDb.OleDbType.VarChar).Value = _photo1
        End If

        If _photo2 <> "" Then
            strSQL &= ",PHOTO2 "
            strSQLParametri &= ",@PHOTO2 "
            oleDbCommand.Parameters.Add("@PHOTO2", OleDb.OleDbType.VarChar).Value = _photo2
        End If

        If _photo3 <> "" Then
            strSQL &= ",PHOTO3 "
            strSQLParametri &= ",@PHOTO3 "
            oleDbCommand.Parameters.Add("@PHOTO3", OleDb.OleDbType.VarChar).Value = _photo3
        End If

        If _photo4 <> "" Then
            strSQL &= ",PHOTO4 "
            strSQLParametri &= ",@PHOTO4 "
            oleDbCommand.Parameters.Add("@PHOTO4", OleDb.OleDbType.VarChar).Value = _photo4
        End If




        oleDbCommand.CommandText = strSQL & " ) " & strSQLParametri & " )"

        oleDbCommand.ExecuteNonQuery()





        'Ottengo in codice id del nuovo elemento appena inserito...

        Return _getIdentity()
    End Function


    Public Sub delete(ByVal oggettoId As Long)
        Dim strSQL As String = "DELETE * FROM OGGETTO WHERE OGGETTO_ID=" & oggettoId
        Dim oleDbCommand As New System.Data.OleDb.OleDbCommand(strSQL, _connection)
        oleDbCommand.ExecuteNonQuery()
    End Sub



    Public Function getPhotoFileName(ByVal oggettoId As Long, ByVal numeroPhoto As Integer) As String
        Dim sqlQuery As String
        sqlQuery = "SELECT PHOTO" & numeroPhoto & " FROM OGGETTO WHERE OGGETTO_ID=" & oggettoId

        Return _executeScalar(sqlQuery)
    End Function

    Public Sub deletePhoto(ByVal oggettoId As Long, ByVal numeroPhoto As Integer)


        Dim fileName As String
        fileName = Me.getPhotoFileName(oggettoId, numeroPhoto)

        'Cancellazione da FILE SYSTEM
        Dim folder As String
        folder = System.Configuration.ConfigurationManager.AppSettings("catalogo.images.folder")
        folder = System.AppDomain.CurrentDomain.BaseDirectory & folder.Replace("~/", "") & oggettoId & "_" & numeroPhoto & "_" & fileName

        Dim file As New System.IO.FileInfo(folder)
        If file.Exists Then
            file.Delete()
        End If



        Dim strSQL As String = "UPDATE OGGETTO SET DATE_MODIFIED = NOW ,PHOTO" & numeroPhoto & " = NULL " & _
                                " WHERE OGGETTO_ID=" & oggettoId

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        oleDbCommand.ExecuteNonQuery()



    End Sub







    Public Function update(ByVal oggettoId As Long) As Boolean
        Dim esito As Boolean = False

        Dim strSQL As String = "UPDATE OGGETTO SET DATE_MODIFIED = NOW "

        Dim oleDbCommand As System.Data.OleDb.OleDbCommand
        oleDbCommand = New System.Data.OleDb.OleDbCommand(strSQL, _connection)

        strSQL &= ",ISPUBLIC = @ISPUBLIC "
        oleDbCommand.Parameters.Add("@ISPUBLIC", OleDb.OleDbType.Boolean).Value = _isPubblico

        strSQL &= ",ISUSATO = @ISUSATO"
        oleDbCommand.Parameters.Add("@ISUSATO", OleDb.OleDbType.Boolean).Value = _isUsato

        strSQL &= ",NOME = @NOME "
        oleDbCommand.Parameters.Add("@NOME", OleDb.OleDbType.VarChar).Value = _nome

        strSQL &= ",CODICE_PRODOTTO = @CODICE_PRODOTTO "
        oleDbCommand.Parameters.Add("@CODICE_PRODOTTO", OleDb.OleDbType.VarChar).Value = _codice

        strSQL &= ",DESCRIZIONE = @DESCRIZIONE "
        oleDbCommand.Parameters.Add("@DESCRIZIONE", OleDb.OleDbType.VarChar).Value = _descrizione

        strSQL &= ",SCHEDA = @SCHEDA "
        oleDbCommand.Parameters.Add("@SCHEDA", OleDb.OleDbType.VarChar).Value = _scheda

        strSQL &= ",TIPO_ID = @TIPO_ID "
        oleDbCommand.Parameters.Add("@TIPO_ID", OleDb.OleDbType.Numeric).Value = _tipoDiOggetto

        strSQL &= ",PREZZO = @PREZZO "
        oleDbCommand.Parameters.Add("@PREZZO", OleDb.OleDbType.Numeric).Value = _prezzo

        strSQL &= ",PREZZOALPUBBLICO = @PREZZOALPUBBLICO "
        oleDbCommand.Parameters.Add("@PREZZOALPUBBLICO", OleDb.OleDbType.Numeric).Value = _prezzoAlPubblico


        strSQL &= ",PREZZO_SPECIALE = @PREZZO_SPECIALE "
        oleDbCommand.Parameters.Add("@PREZZO_SPECIALE", OleDb.OleDbType.Numeric).Value = _prezzoSpeciale

        If _photo1 <> "" Then
            strSQL &= ",PHOTO1 = @PHOTO1 "
            oleDbCommand.Parameters.Add("@PHOTO1", OleDb.OleDbType.VarChar).Value = _photo1
        End If

        If _photo2 <> "" Then
            strSQL &= ",PHOTO2 = @PHOTO2 "
            oleDbCommand.Parameters.Add("@PHOTO2", OleDb.OleDbType.VarChar).Value = _photo2
        End If

        If _photo3 <> "" Then
            strSQL &= ",PHOTO3 = @PHOTO3 "
            oleDbCommand.Parameters.Add("@PHOTO3", OleDb.OleDbType.VarChar).Value = _photo3
        End If

        If _photo4 <> "" Then
            strSQL &= ",PHOTO4 = @PHOTO4 "
            oleDbCommand.Parameters.Add("@PHOTO4", OleDb.OleDbType.VarChar).Value = _photo4
        End If


        strSQL &= " WHERE OGGETTO_ID=" & oggettoId



        oleDbCommand.CommandText = strSQL

        oleDbCommand.ExecuteNonQuery()

        Return True
    End Function





    Public Function getElencoProdottiPubbicati(Optional ByVal tipoId As Long = -1) As System.Data.DataSet
        Return getElencoProdotti(True, tipoId)
    End Function

    Public Function getElencoProdotti() As System.Data.DataSet
        Return getElencoProdotti(False)
    End Function


    Private Function getElencoProdotti(ByVal onlyPublic As Boolean, Optional ByVal tipoId As Long = -1) As System.Data.DataSet
        'Dim sqlQuery As String = "SELECT OGGETTO.* , TIPOOGGETTO.NOME as TIPO " & _
        '    " FROM OGGETTO, TIPOOGGETTO" & _
        '    " WHERE OGGETTO.tipo_id= TIPOOGGETTO.tipo_id"
        Dim sqlQuery As String = "SELECT  oggetto.oggetto_id, oggetto.nome, Mid (oggetto.descrizione,1,200) &  '...' as descrizione ,PREZZO, PREZZO_SPECIALE, PREZZOALPUBBLICO, TIPOOGGETTO.NOME as TIPO, IsPUBLIC, IsUsato, CODICE_PRODOTTO , PHOTO1" & _
                  " FROM OGGETTO, TIPOOGGETTO" & _
                  " WHERE OGGETTO.tipo_id= TIPOOGGETTO.tipo_id " & _
                  ""

        If tipoId <> -1 Then
            sqlQuery &= " AND OGGETTO.tipo_id = " & tipoId
        End If

        If onlyPublic Then
            sqlQuery &= " AND OGGETTO.ISPUBLIC = true"
        End If

        sqlQuery &= "  ORDER BY DATE_ADDED DESC"


        Return _fillDataSet(sqlQuery)
    End Function


    Public Function copyProdotto(ByVal oggettoId As Long) As Long
        Dim sqlQuery As String
        sqlQuery = "select * from OGGETTO WHERE OGGETTO_id = " & oggettoId

        Dim dataSet As System.Data.DataSet = _fillDataSet(sqlQuery)
        Dim row As System.Data.DataRow
        row = dataSet.Tables(0).Rows(0)


        Me._descrizione = MyManager.GUIManager.getString(row("DESCRIZIONE"))
        Me._isPubblico = False
        Me._isUsato = MyManager.GUIManager.getBoolean(row("ISUSATO"))
        Me._nome = "Copia di " & MyManager.GUIManager.getString(row("NOME"))
        Me._codice = MyManager.GUIManager.getString(row("CODICE_PRODOTTO"))
        Me._photo1 = MyManager.GUIManager.getString(row("PHOTO1"))
        Me._photo2 = MyManager.GUIManager.getString(row("PHOTO2"))
        Me._photo3 = MyManager.GUIManager.getString(row("PHOTO3"))
        Me._photo4 = MyManager.GUIManager.getString(row("PHOTO4"))
        Me._prezzo = MyManager.GUIManager.getString(row("PREZZO"))
        Me._prezzoAlPubblico = MyManager.GUIManager.getString(row("PREZZOALPUBBLICO"))
        Me._prezzoSpeciale = MyManager.GUIManager.getString(row("PREZZO_SPECIALE"))
        Me._scheda = MyManager.GUIManager.getString(row("SCHEDA"))
        Me._tipoDiOggetto = MyManager.GUIManager.getString(row("TIPO_ID"))


        Return (Me.insert())
    End Function

    Public Function getProdotto(ByVal prodottoId As Long) As System.Data.DataSet

        Dim sqlQuery As String = "SELECT OGGETTO.* , TIPOOGGETTO.NOME as TIPO " & _
                   " FROM OGGETTO, TIPOOGGETTO" & _
                   " WHERE OGGETTO.tipo_id= TIPOOGGETTO.tipo_id AND OGGETTO_id = " & prodottoId

        Return _fillDataSet(sqlQuery)
    End Function



End Class
