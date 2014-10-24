'
' Created by SharpDevelop.
' User: Roberto
' Date: 16/06/2009
' Time: 17.56
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class UcBrowseFileSystem
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the control.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.MyTreeView = New System.Windows.Forms.TreeView()
        Me.listView1 = New System.Windows.Forms.ListView()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.MyTreeView)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.listView1)
        Me.splitContainer1.Size = New System.Drawing.Size(478, 282)
        Me.splitContainer1.SplitterDistance = 159
        Me.splitContainer1.TabIndex = 0
        '
        'MyTreeView
        '
        Me.MyTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MyTreeView.Location = New System.Drawing.Point(0, 0)
        Me.MyTreeView.Name = "MyTreeView"
        Me.MyTreeView.Size = New System.Drawing.Size(159, 282)
        Me.MyTreeView.TabIndex = 0
        '
        'listView1
        '
        Me.listView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listView1.Location = New System.Drawing.Point(0, 0)
        Me.listView1.Name = "listView1"
        Me.listView1.Size = New System.Drawing.Size(315, 282)
        Me.listView1.TabIndex = 0
        Me.listView1.UseCompatibleStateImageBehavior = False
        Me.listView1.View = System.Windows.Forms.View.Details
        '
        'UcBrowseFileSystem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.splitContainer1)
        Me.Name = "UcBrowseFileSystem"
        Me.Size = New System.Drawing.Size(478, 282)
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private listView1 As System.Windows.Forms.ListView
    Private WithEvents MyTreeView As System.Windows.Forms.TreeView
	Private splitContainer1 As System.Windows.Forms.SplitContainer
End Class
