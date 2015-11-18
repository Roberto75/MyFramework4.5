Public Class FormMain
    Inherits MyFormsLibrary.FormMaster

    Private Sub FormMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MyLogEnabled Then
            Trace.WriteLine("Application shutdown")
        End If
    End Sub


    Private Sub FormMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If MyLogEnabled Then
            Trace.WriteLine("Application startup")
        End If

        builMenuLeftStack()

        buildMenuLeft()



    End Sub

    Private Function buildMenuLeft() As Boolean
        Dim myNode As System.Windows.Forms.TreeNode
        myNode = Me.UcMenuLeftTreeView1._addNodeLevel_1("nTest", "Test")
        Me.UcMenuLeftTreeView1._addNodeLevel_2("nTestExcel", "Excel", myNode)
        Me.UcMenuLeftTreeView1._addNodeLevel_2("nTestPDF", "PDF", myNode)

        Me.UcMenuLeftTreeView1._init()
        Return True
    End Function



    Private Function builMenuLeftStack() As Boolean
        Me.UcMenuLeftStack1._init()

        Me.UcMenuLeftStack1._addGroup("Group1")
        Me.UcMenuLeftStack1._addItemToGroup("Group1", "SubItem1")
		Me.UcMenuLeftStack1._addItemToGroup("Group1", "SubItem2")
		Me.UcMenuLeftStack1._addItemToGroup("Group1", "SubItem3")
		
        Me.UcMenuLeftStack1._addGroup("Group2")
		Me.UcMenuLeftStack1._addItemToGroup("Group2", "SubItem1")
		Me.UcMenuLeftStack1._addItemToGroup("Group2", "SubItem2")
		Me.UcMenuLeftStack1._addItemToGroup("Group2", "SubItem3")
		
		Me.UcMenuLeftStack1._addGroup("Group3")
		Me.UcMenuLeftStack1._addItemToGroup("Group3", "SubItem1")
		Me.UcMenuLeftStack1._addItemToGroup("Group3", "SubItem2")
		Me.UcMenuLeftStack1._addItemToGroup("Group3", "SubItem3")
    End Function



    Private Sub UcMenuLeftTreeView1_OnClickMenuLeft(ByVal node As System.Windows.Forms.TreeNode) Handles UcMenuLeftTreeView1.MyOnClickMenuLeft
        Select Case node.Name
            Case "nTestExcel"
                Dim f As New FormExcel()
                f.Owner = Me
                f._setTitolo(node.Text)
                f._init(Nothing, Me.MyStatusBar, Me.MyProgressBar)
                Me._addTabPage(f._getTabPage())

                'Case "nTestPDF"
                '    Dim f As New FormPDF()
                '    f.Owner = Me
                '    f._setTitolo(node.Text)
                '    f._init(Me.MyStatusBar, Me.MyProgressBar)
                '    Me._addTabPage(f._getTabPage())


        End Select

    End Sub



End Class