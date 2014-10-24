<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBaseSearch_0
    Inherits MyFormsLibrary.FormBaseDetail_2

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBaseSearch_0))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnAdd = New MyControlsLibrary.MyButton
        Me.btnReset = New MyControlsLibrary.MyButton
        Me.btnSearch = New MyControlsLibrary.MyButton
        Me.tabControlSearchFilter = New System.Windows.Forms.TabControl
        Me.TabPageRicercaBase = New System.Windows.Forms.TabPage
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageMain.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabControlSearchFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabControlSearchFilter)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel2)
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnAdd)
        Me.Panel2.Controls.Add(Me.btnReset)
        Me.Panel2.Controls.Add(Me.btnSearch)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 184)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(706, 39)
        Me.Panel2.TabIndex = 1
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(545, 7)
        Me.btnAdd.MyType = MyControlsLibrary.MyButton.ButtonType.btnNew
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(156, 30)
        Me.btnAdd.TabIndex = 4
        Me.btnAdd.Text = "Nuovo ..."
        Me.btnAdd.UseVisualStyleBackColor = True
        Me.btnAdd.Visible = False
        '
        'btnReset
        '
        Me.btnReset.FlatAppearance.BorderSize = 0
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Image = CType(resources.GetObject("btnReset.Image"), System.Drawing.Image)
        Me.btnReset.Location = New System.Drawing.Point(5, 7)
        Me.btnReset.MyType = MyControlsLibrary.MyButton.ButtonType.btnReset
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(156, 30)
        Me.btnReset.TabIndex = 3
        Me.btnReset.Text = "Azzera campi"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.FlatAppearance.BorderSize = 0
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(201, 7)
        Me.btnSearch.MyType = MyControlsLibrary.MyButton.ButtonType.btnFind
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(156, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Cerca"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'tabControlSearchFilter
        '
        Me.tabControlSearchFilter.Controls.Add(Me.TabPageRicercaBase)
        Me.tabControlSearchFilter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControlSearchFilter.Location = New System.Drawing.Point(0, 0)
        Me.tabControlSearchFilter.Name = "tabControlSearchFilter"
        Me.tabControlSearchFilter.SelectedIndex = 0
        Me.tabControlSearchFilter.Size = New System.Drawing.Size(706, 184)
        Me.tabControlSearchFilter.TabIndex = 2
        '
        'TabPageRicercaBase
        '
        Me.TabPageRicercaBase.Location = New System.Drawing.Point(4, 22)
        Me.TabPageRicercaBase.Name = "TabPageRicercaBase"
        Me.TabPageRicercaBase.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageRicercaBase.Size = New System.Drawing.Size(698, 158)
        Me.TabPageRicercaBase.TabIndex = 0
        Me.TabPageRicercaBase.Text = "Ricerca"
        Me.TabPageRicercaBase.UseVisualStyleBackColor = True
        '
        'FormBaseSearch_0
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 452)
        Me.Name = "FormBaseSearch_0"
        Me.Text = "FormBaseSearch_0"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageMain.ResumeLayout(False)
        Me.tabPageMain.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.tabControlSearchFilter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnAdd As MyControlsLibrary.MyButton
    Friend WithEvents btnReset As MyControlsLibrary.MyButton
    Friend WithEvents btnSearch As MyControlsLibrary.MyButton
    Friend WithEvents tabControlSearchFilter As System.Windows.Forms.TabControl
    Protected WithEvents TabPageRicercaBase As System.Windows.Forms.TabPage
End Class
