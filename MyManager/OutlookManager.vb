Public Class OutlookManager
    Inherits MyManager.Manager

#Const isActive = False


#If isActive Then

    Private _objOutlook As Microsoft.Office.Interop.Outlook.Application

    Private _contactFolder As Microsoft.Office.Interop.Outlook.MAPIFolder
    Private _items As Microsoft.Office.Interop.Outlook.Items

    Public _contactItem As Microsoft.Office.Interop.Outlook.ContactItem
    '  Private _item As Microsoft.Office.Interop.Outlook.Items






    Public Sub New()
    End Sub

    Public Sub New(ByVal _obj As Microsoft.Office.Interop.Outlook.Application)
        Me._objOutlook = _obj
    End Sub



    'Public Function openFile(ByVal fullPathPST As String) As Boolean
    '    If _objOutlook Is Nothing Then
    '        _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
    '    End If



    'End Function




    Public Function getAllContacts() As List(Of MyContatto)

        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        Dim risultato As New List(Of MyContatto)
        Dim contatto As MyContatto



        Dim conta As Long = 1

        For i As Integer = 1 To _items.Count

            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try


            Debug.WriteLine(conta & ": " & _contactItem.FullName & " -> " & _contactItem.Subject)

            contatto = New MyContatto()
            contatto.Nome = _contactItem.FirstName
            contatto.Cognome = _contactItem.LastName
            contatto.Note = _contactItem.Body
            contatto.OutlookId = _contactItem.EntryID
            contatto.Societa = _contactItem.CompanyName


            '   risultato = risultato &  & ";@@;" &  & ";@@;" & _& ";@@;" & _contactItem.CompanyName & ";@@;" &  & ";@@;"

           
            risultato.Add(contatto)

            conta = conta + 1
        Next

        Return risultato
    End Function


    Public Function getAllContact() As String

        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        Dim risultato As String = ""
        Dim conta As Long = 1

        For i As Integer = 1 To _items.Count

            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try


            Debug.WriteLine(conta & ": " & _contactItem.FullName)
            risultato = risultato & _contactItem.Subject & ";@@;" & _contactItem.FirstName & ";@@;" & _contactItem.LastName & ";@@;" & _contactItem.CompanyName & ";@@;" & _contactItem.Body & ";@@;"

            If (Not _statusBar Is Nothing) Then
                _statusBarUpdate(String.Format("Export {0:N0}/{1:N0}", conta, _items.Count))
            End If


            conta = conta + 1
        Next

        Return risultato
    End Function




    Public Function searchContactWithBodysize(ByVal limit As Integer) As Boolean
        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        For i As Integer = 1 To _items.Count
            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try


            If _contactItem.Subject.Length >= limit Then
                Return True
            End If

            _statusBarUpdate(String.Format("Check body size {0:N0}/{1:N0}", i, _items.Count))

        Next

        Return False

    End Function



    Public Function formatMobileNumber() As Boolean
        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If

        Dim telephone As String


        For i As Integer = 1 To _items.Count
            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try

            telephone = _contactItem.MobileTelephoneNumber
            If Not String.IsNullOrEmpty(telephone) Then

                telephone = telephone.Trim

                If telephone.Contains(".") Then
                    telephone = telephone.Replace(".", "")
                End If

                If telephone.StartsWith("+39") Then
                    If Not telephone.StartsWith("+39 ") Then
                        telephone = "+39 " & telephone.Replace("+39", "").Replace(" ", "")
                    End If
                End If




                If telephone.StartsWith("335") _
                    OrElse telephone.StartsWith("340") _
                    OrElse telephone.StartsWith("346") _
                    OrElse telephone.StartsWith("347") _
                    OrElse telephone.StartsWith("348") _
                    OrElse telephone.StartsWith("349") _
                    OrElse telephone.StartsWith("338") _
                    OrElse telephone.StartsWith("334") _
                    OrElse telephone.StartsWith("339") _
                    OrElse telephone.StartsWith("333") _
                    OrElse telephone.StartsWith("320") _
                    OrElse telephone.StartsWith("329") _
                    OrElse telephone.StartsWith("328") Then

                    telephone = "+39 " & telephone
                End If


                If telephone <> _contactItem.MobileTelephoneNumber Then
                    _contactItem.MobileTelephoneNumber = telephone
                    _contactItem.Save()
                End If


                If Not telephone.StartsWith("+") Then
                    Return False
                End If
            End If

            _statusBarUpdate(String.Format("Check mobile number {0:N0}/{1:N0}", i, _items.Count))

        Next

        Return True

    End Function


    Public Function searchContact(ByVal subject As String) As Boolean

        'ricerco un contatto con un determianto subject!
        'Se lo trovo lo memorizzo nella variabile pubbica _contactItem

        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        For i As Integer = 1 To _items.Count
            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try

            If _contactItem.Subject = subject Then
                Return True
            End If

            _statusBarUpdate(String.Format("Search {2}: {0:N0}/{1:N0}", i, _items.Count, subject))
        Next
        Return False
    End Function


    Public Function searchContact(ByVal nome As String, ByVal cognome As String) As Boolean

        'ricerco un contatto con un determianto subject!
        'Se lo trovo lo memorizzo nella variabile pubbica _contactItem

        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        For i As Integer = 1 To _items.Count
            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try

            If _contactItem.FirstName = nome AndAlso _contactItem.LastName = cognome Then
                Return True
            End If
            _statusBarUpdate(String.Format("Search {2}: {0:N0}/{1:N0}", i, _items.Count, nome & " " & cognome))
        Next
        Return False
    End Function



    Public Function searchContactBySocieta(ByVal societa As String) As Boolean

        'ricerco un contatto con un determianto subject!
        'Se lo trovo lo memorizzo nella variabile pubbica _contactItem

        If _objOutlook Is Nothing Then
            _objOutlook = New Microsoft.Office.Interop.Outlook.Application()
        End If

        If _items Is Nothing Then
            Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace = _objOutlook.GetNamespace("MAPI")
            _contactFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts)
            _items = _contactFolder.Items
        End If


        For i As Integer = 1 To _items.Count
            Try
                _contactItem = _items(i)
            Catch ex As Exception
                'iggloro ... sarà una lista di distribuzione
                Debug.WriteLine("Errore di casting:")
            End Try

            If _contactItem.CompanyName = societa Then
                Return True
            End If
            _statusBarUpdate(String.Format("Search {2}: {0:N0}/{1:N0}", i, _items.Count, societa))
        Next
        Return False
    End Function


#End If
End Class
