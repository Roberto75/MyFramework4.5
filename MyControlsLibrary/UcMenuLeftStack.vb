Public Class UcMenuLeftStack

    Public Event MyMenuLeftStackGroupOnClick(ByVal group As String)
    Public Event MyMenuLeftStackItemOnClick(ByVal group As String, ByVal item As String)

    Private _listBar As vbAccelerator.Components.ListBarControl.ListBar


    Public Function _init() As Boolean
        _listBar = New vbAccelerator.Components.ListBarControl.ListBar()
        _listBar.Dock = DockStyle.Fill
		_listBar.LargeImageList = imageList1

        AddHandler _listBar.ItemClicked, AddressOf Me.ListBar1ItemClicked

        AddHandler _listBar.GroupClicked, AddressOf Me.ListBar1GroupClicked



        Me.Controls.Add(_listBar)
        Return True
    End Function


    Public Function _init(ByVal imageList As Windows.Forms.ImageList) As Boolean
        _init()

        Me._listBar.LargeImageList = imageList
        Return True
    End Function



    Sub ListBar1ItemClicked(ByVal sender As Object, ByVal e As vbAccelerator.Components.ListBarControl.ItemClickedEventArgs)
        RaiseEvent MyMenuLeftStackItemOnClick("", e.Item.Caption)
    End Sub


    Sub ListBar1GroupClicked(ByVal sender As Object, ByVal e As vbAccelerator.Components.ListBarControl.GroupClickedEventArgs)
        RaiseEvent MyMenuLeftStackGroupOnClick(e.Group.Caption)
    End Sub

    Public Function _addGroup(ByVal name As String) As Boolean
        Return _addGroup(name, -1)
    End Function


    Public Function _addGroup(ByVal name As String, ByVal iconIndex As Integer) As Boolean

        Dim group As New vbAccelerator.Components.ListBarControl.ListBarGroup
        With group
            .Caption = name
            '.BackColor = SystemColors.Control
            .BackColor = Color.FromArgb(215, 227, 242)
            .ForeColor = SystemColors.ControlText
            .Key = name
        End With
        _listBar.Groups.Add(group)


        'AddHandler group..AfterCheck, AddressOf TreeView1AfterCheck

        If iconIndex >= 0 Then
            If _listBar.LargeImageList Is Nothing Then
                Throw New ApplicationException("Utilizzare il metodo _init(ImageList)")
                Return False
            End If
            Dim itemImage As New vbAccelerator.Components.ListBarControl.ListBarItem
            itemImage.IconIndex = iconIndex
            itemImage.Caption = "ddd"
            itemImage.ForeColor = System.Drawing.SystemColors.ControlText
            group.Items.Add(itemImage)
        End If


        Return True
    End Function

    Public Function _addItemToGroup(ByVal groupName As String, ByVal value As String) As Boolean

        Dim group As New vbAccelerator.Components.ListBarControl.ListBarGroup
        group = _containGroup(groupName)

        If group Is Nothing Then
            Throw New MyException("Group not found:" & groupName)
        End If


        Dim item As New vbAccelerator.Components.ListBarControl.ListBarItem
        item.Caption = value
        item.Font = New System.Drawing.Font("Tahoma", 10)
        item.ForeColor = System.Drawing.SystemColors.ControlText
        item.IconIndex = imageList1.Images.IndexOfKey("generic.ico")

        group.Items.Add(item)


        Return True
    End Function


    Private Function _containGroup(ByVal key As String) As vbAccelerator.Components.ListBarControl.ListBarGroup
        Dim en As System.Collections.IEnumerator
        en = _listBar.Groups.GetEnumerator()

        While en.MoveNext
            If en.Current.Key = key Then
                Return _listBar.Groups.Item(key)
            End If
        End While

        Return Nothing
    End Function

End Class
