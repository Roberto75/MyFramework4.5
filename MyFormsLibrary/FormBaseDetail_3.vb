Public Class FormBaseDetail_3
    Inherits FormBaseDetail_2

    Friend _TabPageStatus As New List(Of Boolean)
    Friend _isInitialized As Boolean = False


    Public Overrides Function _init(ByRef connection As System.Data.Common.DbConnection, ByRef statusBar As System.Windows.Forms.ToolStripStatusLabel, ByRef progressBar As System.Windows.Forms.ToolStripProgressBar) As Boolean
        MyBase._init(connection, statusBar, progressBar)
        _ResetTabPageDetailStatus()
        _isInitialized = True
        Return True
    End Function


    Private Sub TabControlDetail_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControlDetail.Selecting
        If e.TabPage Is Nothing Then Exit Sub
        CType(sender, Windows.Forms.TabControl).Cursor = Windows.Forms.Cursors.WaitCursor
        If _isInitialized Then
            If _TabDetailOnClick(_TabPageStatus(e.TabPageIndex), e) Then
                _TabPageStatus(e.TabPageIndex) = True
            End If
        End If
        CType(sender, Windows.Forms.TabControl).Cursor = Windows.Forms.Cursors.Default
    End Sub


    Overridable Function _TabDetailOnClick(ByVal isLoad As Boolean, ByVal e As TabControlCancelEventArgs) As Boolean
        Select Case e.TabPage.Name
            Case "tpFIN"
                'If Not isLoad Then 	Fill_FinanziamentoTab()
            Case "tpIND"
                'If not isLoad Then 	Fill_Indirizzi()
            Case "tpRC"
                '	If Not isLoad Then 	Fill_RiferimentiTab()
            Case Else
                Windows.Forms.MessageBox.Show("TabDetailOnClick not implement: " & e.TabPage.Name, My.Application.Info.ProductName, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Select

        Return False
    End Function

    Public Function _TabDetailReload(ByVal tab As TabPage) As Boolean
        Return _TabDetailOnClick(False, New System.Windows.Forms.TabControlCancelEventArgs(tab, 0, False, TabControlAction.Selected))
    End Function




    Sub _ResetTabPageDetailStatus()
        Dim i As Integer = 0
        _TabPageStatus.Clear()
        For i = 0 To tabControlDetail.TabCount - 1
            _TabPageStatus.Insert(i, False)
        Next
    End Sub

    Public Sub _removeAllTabPages()
        For i As Integer = Me.TabControlDetail.TabPages.Count - 1 To 1 Step -1
            Me.TabControlDetail.TabPages.RemoveAt(i)
        Next
    End Sub

    Public Sub _addTabPageWithTextArea(ByVal name As String, ByVal testo As String)

        Dim tb As New System.Windows.Forms.TabPage()
        tb.Text = name

        Dim textArea As New System.Windows.Forms.TextBox()
        With textArea
            .AcceptsReturn = True
            .AcceptsTab = True
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Location = New System.Drawing.Point(3, 3)
            .Multiline = True
            .ScrollBars = System.Windows.Forms.ScrollBars.Both
            .Size = New System.Drawing.Size(675, 191)
            .TabIndex = 0
            .Text = testo
        End With

        tb.Controls.Add(textArea)

        Me.TabControlDetail.TabPages.Add(tb)
        Me._ResetTabPageDetailStatus()
    End Sub


    Public Sub _addTabPageWithGridView(ByVal name As String, ByVal dt As Data.DataTable)

        Dim tb As New System.Windows.Forms.TabPage()
        tb.Text = name

        Dim grid As New MyControlsLibrary.UcPreviewDataTable
        grid.Dock = DockStyle.Fill

        grid._init(dt)
        tb.Controls.Add(grid)

        Me.TabControlDetail.TabPages.Add(tb)
        Me._ResetTabPageDetailStatus()
    End Sub

End Class