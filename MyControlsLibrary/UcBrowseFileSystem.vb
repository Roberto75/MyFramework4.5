'
' Created by SharpDevelop.
' User: Roberto
' Date: 16/06/2009
' Time: 17.56
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Partial Class UcBrowseFileSystem
	Inherits System.Windows.Forms.UserControl
	
    Private _folderRoot As String

    Public Sub New()
        Me.InitializeComponent()
    End Sub


    Public Function _init() As Boolean

        'I dirver!

        Dim drive As String()
        drive = System.Environment.GetLogicalDrives()
        Dim basenode As System.Windows.Forms.TreeNode

        For Each d As String In drive
            basenode = MyTreeView.Nodes.Add(d)
            basenode.Tag = d
            LoadDir(d, basenode)
        Next


        'Me._folderRoot = pathFolder
        'LoadFolderTree(pathFolder)

        Dim h As System.Windows.Forms.ColumnHeader

        h = listView1.Columns.Add("File name")
        h.Width = 300

        Return True
    End Function


    Public Sub LoadFolderTree(ByVal path As String)
        Dim basenode As System.Windows.Forms.TreeNode
        If IO.Directory.Exists(path) Then
            If path.Length <= 3 Then
                basenode = MyTreeView.Nodes.Add(path)
            Else
                basenode = MyTreeView.Nodes.Add(My.Computer.FileSystem.GetName(path))
            End If
            basenode.Tag = path
            LoadDir(path, basenode)
        Else
            Throw New System.IO.DirectoryNotFoundException()
        End If
    End Sub

    Private Sub MyTreeView_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles MyTreeView.AfterExpand
        Dim n As System.Windows.Forms.TreeNode
        For Each n In e.Node.Nodes
            LoadDir(n.Tag, n)
        Next
    End Sub

    Public Sub LoadDir(ByVal DirPath As String, ByVal Node As Windows.Forms.TreeNode)
        On Error Resume Next
        Dim Dir As String
        Dim Index As Integer
        If Node.Nodes.Count = 0 Then
            For Each Dir In My.Computer.FileSystem.GetDirectories(DirPath)
                Index = Dir.LastIndexOf("\")
                Node.Nodes.Add(Dir.Substring(Index + 1, Dir.Length - Index - 1))
                Node.LastNode.Tag = Dir
                Node.LastNode.ImageIndex = 0
            Next
        End If
    End Sub

    Public Function fillFiles(ByVal folder As String) As Boolean

        Dim d As New IO.DirectoryInfo(folder)

        Dim fl As IO.FileSystemInfo()
        fl = d.GetFileSystemInfos()

        listView1.Items.Clear()

        Dim newItem As System.Windows.Forms.ListViewItem

        For Each f As IO.FileSystemInfo In fl
            If String.IsNullOrEmpty(f.Extension) Then

            Else
                newItem = listView1.Items.Add(f.Name)
                newItem.Tag = f.FullName
            End If
        Next

        Return True
    End Function

    Public Function _getFolder() As String
        Return MyTreeView.SelectedNode().FullPath & "\"

    End Function


    Public Function _getListFiles() As List(Of IO.FileInfo)
        Return _getListFiles("")
    End Function

    Public Function _getListFiles(ByVal filter As String) As List(Of IO.FileInfo)

        Dim risultato As New List(Of IO.FileInfo)
        Dim f As IO.FileInfo

        filter = filter.ToLower

        For Each i As System.Windows.Forms.ListViewItem In listView1.Items

            f = New IO.FileInfo(i.Tag)

            If String.IsNullOrEmpty(filter) Then
                risultato.Add(f)
            Else
                'If f.Extension.ToLower = filter Then
                '    risultato.Add(f)
                'End If

                '2012/02/18
                If System.Text.RegularExpressions.Regex.IsMatch(f.Extension.ToLower, filter) Then
                    risultato.Add(f)
                End If
            End If

        Next
        Return risultato
    End Function


    Private Sub MyTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles MyTreeView.AfterSelect
        fillFiles(e.Node.Tag)
    End Sub
End Class
