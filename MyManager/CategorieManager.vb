Public Class CategorieManager
    Inherits Manager

    Public Enum SubCategorie
        TestiScolastici = 1130000
        TestiUniversitari = 1140000
    End Enum


    Public Sub New(ByVal connectionName As String)
        MyBase.New(connectionName)
    End Sub

    Public Sub New(ByVal connection As System.Data.Common.DbConnection)
        MyBase.New(connection)
    End Sub


    Public Sub addClick(ByVal categoria_id As Long)
        mStrSQL = "UPDATE CATEGORIE SET DATE_LAST_CLICK = NOW , COUNT_CLICK = COUNT_CLICK +1 " & _
                                            " WHERE CATEGORIA_ID=" & categoria_id
        Me.mExecuteNoQuery(mStrSQL)
    End Sub




    'elenco delle categorie di primo livello 
    Public Function getComboCategoriaRoot() As System.Data.DataSet
        mStrSQL = "select A.nome, A.categoria_id ,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
                    "from CATEGORIE A WHERE FK_PADRE_ID is NULL  AND HIDE = False" & _
                    " ORDER BY nome"

        Return _fillDataSet(mStrSQL)

    End Function

    'elenco delle sotto-categorie
    Public Function getComboCategoria(ByVal categoria_id As String) As System.Data.DataSet

        'Select Case categoria_id
        '    Case CategorieManager.SubCategorie.TestiScolastici
        '        mStrSQL = "select 'Testi Scolastici - ' & A.nome, A.categoria_id,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
        '            " from CATEGORIE A WHERE FK_PADRE_ID = " & categoria_id & _
        '            " ORDER BY nome"

        '    Case CategorieManager.SubCategorie.TestiUniversitari
        '        mStrSQL = "select 'Testi Universitari - ' & A.nome, A.categoria_id,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
        '            " from CATEGORIE A WHERE FK_PADRE_ID = " & categoria_id & _
        '            " ORDER BY nome"
        '    Case Else
        '        mStrSQL = "select A.nome, A.categoria_id,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
        '               " from CATEGORIE A WHERE FK_PADRE_ID = " & categoria_id & _
        '               " ORDER BY nome"
        'End Select

        mStrSQL = "select A.nome, A.categoria_id,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
                       " from CATEGORIE A WHERE FK_PADRE_ID = " & categoria_id & _
                       " ORDER BY nome"

        Return _fillDataSet(mStrSQL)

    End Function


    Public Function getCategoriaFromAnnuncio(ByVal annuncioId As Long) As System.Data.DataSet
        mStrSQL = "select  CATEGORIE.* FROM CATEGORIE LEFT JOIN ANNUNCIO ON ANNUNCIO.FK_CATEGORIA_ID = CATEGORIE.CATEGORIA_ID WHERE ANNUNCIO_ID =" & annuncioId
        Return _fillDataSet(mStrSQL)
    End Function



    Private Function getCategorieRootLevel_XML() As System.Xml.XmlDocument

        Dim document As New System.Xml.XmlDocument

        Dim dataSet As System.Data.DataSet


        mStrSQL = "select A.*,  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  " & _
                    ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID = a.CATEGORIA_ID AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI " & _
                    " from CATEGORIE A WHERE FK_PADRE_ID is NULL and HIDE = false" & _
                    " ORDER by nome"

        dataSet = _fillDataSet(mStrSQL, "Categorie", "Categoria")

        document.LoadXml(dataSet.GetXml)

        Return document
    End Function


    Public Function getCategorie_XML() As System.Xml.XmlDocument

        Dim document As New System.Xml.XmlDocument

        document = getCategorieRootLevel_XML()

        Dim nodeList As System.Xml.XmlNodeList
        Dim node As System.Xml.XmlNode
        Dim temp As String
        Dim nodeImported As System.Xml.XmlNode


        nodeList = document.SelectNodes("/Categorie/Categoria[childnodecount > 0]")

        For Each node In nodeList
            temp = node.SelectSingleNode("categoria_id").InnerText
            nodeImported = getCategorie_XML(temp)

            If (nodeImported IsNot Nothing) Then
                nodeImported = document.ImportNode(nodeImported.SelectSingleNode("/Categorie"), True)
                node.AppendChild(nodeImported)
            End If

        Next
        Return document

    End Function


    Private Function getCategorie_XML(ByVal categoria_id As Long) As System.Xml.XmlDocument
        Dim document As New System.Xml.XmlDocument
        Dim dataSet As System.Data.DataSet

        Dim sqlQuery As String = ""

        'testi universitari
        'testi scolastici


        sqlQuery = "select A.* " & _
         ",  (select count (*) from  CATEGORIE where FK_PADRE_ID = a.CATEGORIA_ID  ) as childnodecount  "

        'If categoria_id = MyManager.CategorieManager.SubCategorie.TestiScolastici _
        '        OrElse categoria_id = MyManager.CategorieManager.SubCategorie.TestiUniversitari Then
        '    sqlQuery = sqlQuery & ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID >= a.CATEGORIA_ID  AND FK_CATEGORIA_ID <= (a.CATEGORIA_ID + 9999)  AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI "
        'Else
        '    sqlQuery = sqlQuery & ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID = a.CATEGORIA_ID AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI "
        'End If

        sqlQuery = sqlQuery & ", (select count (*) from  ANNUNCIO where FK_CATEGORIA_ID = a.CATEGORIA_ID AND DATE_DELETED IS NULL  ) as COUNT_ANNUNCI "



        sqlQuery = sqlQuery & " from CATEGORIE A WHERE FK_PADRE_ID =" & categoria_id & _
                " ORDER BY nome"



        dataSet = _fillDataSet(sqlQuery, "Categorie", "Categoria")
        document.LoadXml(dataSet.GetXml)


        Dim nodeList As System.Xml.XmlNodeList
        Dim node As System.Xml.XmlNode
        Dim temp As String
        Dim nodeImported As System.Xml.XmlNode


        nodeList = document.SelectNodes("/Categorie/Categoria[childnodecount > 0]")

        For Each node In nodeList
            temp = node.SelectSingleNode("categoria_id").InnerText
            nodeImported = getCategorie_XML(temp)

            If (nodeImported IsNot Nothing) Then
                nodeImported = document.ImportNode(nodeImported.SelectSingleNode("/Categorie"), True)
                node.AppendChild(nodeImported)
            End If

        Next
        Return document
    End Function



    'ad ogni categoria è associato un tipo di oggetto
    Public Function getTipoDiOggetto(ByVal categoria_id As String) As System.Data.DataSet
        Dim sqlQuery As String = ""
        sqlQuery = "SELECT tipo_oggetto.*" & _
                        " FROM tipo_oggetto, categorie " & _
                        " WHERE tipo_oggetto.tipo_oggetto_id = categorie.fk_tipo_oggetto_id " & _
                        "       AND categorie.categoria_id = " & categoria_id

        Return _fillDataSet(sqlQuery)

    End Function


    'Dettaglio di una Categoria
    Public Function getCategoria(ByVal categoria_id As String) As System.Data.DataSet
        mStrSQL = "SELECT categorie.*" & _
                        " FROM  categorie " & _
                        " WHERE categorie.categoria_id = " & categoria_id

        Return _fillDataSet(mStrSQL)
    End Function


    Public Function decodeCategoria(ByVal value As String) As Long
        Dim categoria As String
        Dim subCategoria As String
        Dim indice As Integer
        Dim temp As String

        indice = value.IndexOf(" - ")

        If indice = -1 Then
            categoria = value.Trim
            subCategoria = ""

            If categoria.IndexOf("Scienza") <> -1 Then
                Dim debugg As Boolean
                debugg = True
            End If
        Else
            categoria = value.Substring(0, indice).Trim
            subCategoria = value.Substring(indice + 3).Trim
        End If

        mStrSQL = "SELECT categoria_id " & _
            " FROM  categorie " & _
            " WHERE UCASE(nome) = @NOME and FK_PADRE_ID = 1000000 "

        Dim command As System.Data.Common.DbCommand
        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@NOME", categoria.ToUpper)

        temp = mExecuteScalar(command)

        If String.IsNullOrEmpty(temp) Then
            Return -1
        End If


        If String.IsNullOrEmpty(subCategoria) Then
            Return Long.Parse(temp)
        End If

        mStrSQL = "SELECT categoria_id " & _
            " FROM  categorie " & _
            " WHERE UCASE(nome) = @NOME and FK_PADRE_ID =" & temp

        command = mConnection.CreateCommand()
        command.CommandText = mStrSQL

        Me.mAddParameter(command, "@NOME", subCategoria.ToUpper)
        temp = mExecuteScalar(command)

        If String.IsNullOrEmpty(temp) Then
            Return -1
        End If

        Return Long.Parse(temp)
    End Function

End Class
