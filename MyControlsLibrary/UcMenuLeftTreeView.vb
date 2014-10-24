Public Class UcMenuLeftTreeView

    Public Event MyOnClickMenuLeft(ByVal node As System.Windows.Forms.TreeNode)


    Public Function _init() As Boolean
        Me.treeViewLeft.ExpandAll()
    End Function


    'Function addRootNode(ByVal name As String, ByVal text As String) As System.Windows.Forms.TreeNode
    '    treeViewLeft.
    'End Function


    Function _addNodeLevel_1(ByVal name As String, ByVal text As String) As System.Windows.Forms.TreeNode
        Return _addNodeLevel_1(name, text, 0)
    End Function

    Function _addNodeLevel_1(ByVal name As String, ByVal text As String, ByVal imageIndex As Int16) As System.Windows.Forms.TreeNode
        Dim n As New System.Windows.Forms.TreeNode
        n.Name = name
        n.Text = text
        n.ImageIndex = imageIndex
        n.SelectedImageIndex = imageIndex
        treeViewLeft.Nodes.Add(n)
        Return n
    End Function

    Public Sub _clear()
        treeViewLeft.Nodes.Clear()
    End Sub

    Function _addNodeLevel_2(ByVal name As String, ByVal text As String, ByVal parent As System.Windows.Forms.TreeNode) As System.Windows.Forms.TreeNode
        Return _addNodeLevel_2(name, text, parent, 3)
    End Function

    Function _addNodeLevel_2(ByVal name As String, ByVal text As String, ByVal parent As System.Windows.Forms.TreeNode, ByVal imageIndex As Int16) As System.Windows.Forms.TreeNode
        Dim n As New System.Windows.Forms.TreeNode
        n.Name = name
        n.Text = text
        n.ImageIndex = imageIndex
        n.SelectedImageIndex = imageIndex
        n.ForeColor = Color.SlateGray
        parent.Nodes.Add(n)
        Return n
    End Function


    Private Sub treeViewLeft_NodeMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles treeViewLeft.NodeMouseClick
        RaiseEvent MyOnClickMenuLeft(e.Node)
    End Sub
End Class
