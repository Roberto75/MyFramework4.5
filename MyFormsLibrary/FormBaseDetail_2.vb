Public Class FormBaseDetail_2
    Inherits FormBaseDetail_1

    Private Sub btnCollapse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCollapse.Click
        If SplitContainer1.Panel2Collapsed Then
            '   CType(sender, System.Windows.Forms.ToolStripButton).Text = "Nascondi"
            SplitContainer1.Panel2Collapsed = False
        Else
            '  CType(sender, System.Windows.Forms.ToolStripButton).Text = "Visualizza"
            SplitContainer1.Panel2Collapsed = True
        End If
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        If Not SplitContainer1.Panel1Collapsed Then
            SplitContainer1.Panel1Collapsed = True
            Me.btnTopDown.Visible = True
        End If
    End Sub

    Public Overrides Function _OnTopButtonDownClick() As Boolean
        If SplitContainer1.Panel1Collapsed Then
            SplitContainer1.Panel1Collapsed = False
            Me.btnTopDown.Visible = False
        End If
        Return True
    End Function

    Public Sub _collapseBottomPanel()
        SplitContainer1.Panel2Collapsed = True
    End Sub


    Public Sub _collapseTopPanel()
        SplitContainer1.Panel1Collapsed = True
    End Sub


    Public Sub _removeButtonToToolStrip(ByVal myName As String)
        MyToolStrip3.Items.RemoveByKey(myName)
    End Sub

    Public Sub _addSeparatorToToolStrip(ByVal myAlignement As System.Windows.Forms.ToolStripItemAlignment)
        Dim t As New System.Windows.Forms.ToolStripSeparator
        t.Alignment = myAlignement
        Me.MyToolStrip3.Items.Add(t)
    End Sub


    Public Sub _addButtonToToolStrip(ByVal myLabel As String _
                                     , ByVal myName As String _
                                     , ByVal myImage As System.Drawing.Image _
                                     , ByVal myAlignement As System.Windows.Forms.ToolStripItemAlignment)


        If MyToolStrip3.Items.Find(myName, False).Length > 0 Then
            Exit Sub
        End If

        If MyToolStrip3.Items.Count = 2 Then
            Dim t As New System.Windows.Forms.ToolStripSeparator
            t.Alignment = ToolStripItemAlignment.Left
            Me.MyToolStrip3.Items.Add(t)
        End If

        Dim myButton As New System.Windows.Forms.ToolStripButton
        myButton = New System.Windows.Forms.ToolStripButton
        myButton.Alignment = myAlignement

        If Not myImage Is Nothing Then
            myButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            'Me.myButton.Image = CType(Resources.GetObject("btnCollapse.Image"), System.Drawing.Image)
            myButton.Image = myImage
            myButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        End If

        myButton.Name = myName
        'myButton.Size = New System.Drawing.Size(23, 22)
        myButton.Size = myImage.Size
        myButton.Text = myLabel

        Me.MyToolStrip3.Items.Add(myButton)
    End Sub

    Private Sub MyToolStrip3_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MyToolStrip3.ItemClicked
        If e.ClickedItem.Name <> "btnUp" AndAlso _
            e.ClickedItem.Name <> "btnCollapse" AndAlso _
            e.ClickedItem.GetType.Name = "ToolStripButton" Then
            _toolStripButtonClicked(CType(e.ClickedItem, Windows.Forms.ToolStripButton))
        End If
    End Sub


    Public Overridable Function _toolStripButtonClicked(ByVal myButton As Windows.Forms.ToolStripButton) As Boolean
        Windows.Forms.MessageBox.Show("Function not available", My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        Return False
    End Function


End Class