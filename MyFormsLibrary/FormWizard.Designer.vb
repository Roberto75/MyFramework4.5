'
' Created by SharpDevelop.
' User: Roberto
' Date: 05/12/2008
' Time: 12.31
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'

Partial Class FormWizard
    Inherits System.Windows.Forms.Form
    ''' <summary>
    ''' Designer variable used to keep track of non-visual components.
    ''' </summary>
    Private components As System.ComponentModel.IContainer


    ''' <summary>
    ''' This method is required for Windows Forms designer support.
    ''' Do not change the method contents inside the source code editor. The Forms designer might
    ''' not be able to load this method if it was changed manually.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormWizard))
        Me.pictureBoxTitle = New System.Windows.Forms.PictureBox
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer
        Me._treeView = New System.Windows.Forms.TreeView
        Me.splitContainer2 = New System.Windows.Forms.SplitContainer
        Me.label2 = New System.Windows.Forms.Label
        Me._tabControl = New System.Windows.Forms.TabControl
        Me.tabPage1 = New System.Windows.Forms.TabPage
        Me.btnNextStep = New MyControlsLibrary.MyButton
        Me.btnPrevStep = New MyControlsLibrary.MyButton
        Me._ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblTitoloWizard = New System.Windows.Forms.Label
        CType(Me.pictureBoxTitle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.splitContainer2.Panel1.SuspendLayout()
        Me.splitContainer2.Panel2.SuspendLayout()
        Me.splitContainer2.SuspendLayout()
        Me._tabControl.SuspendLayout()
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictureBoxTitle
        '
        Me.pictureBoxTitle.BackColor = System.Drawing.Color.White
        Me.pictureBoxTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pictureBoxTitle.Image = CType(resources.GetObject("pictureBoxTitle.Image"), System.Drawing.Image)
        Me.pictureBoxTitle.Location = New System.Drawing.Point(0, 0)
        Me.pictureBoxTitle.Name = "pictureBoxTitle"
        Me.pictureBoxTitle.Size = New System.Drawing.Size(896, 67)
        Me.pictureBoxTitle.TabIndex = 0
        Me.pictureBoxTitle.TabStop = False
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 67)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me._treeView)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.splitContainer2)
        Me.splitContainer1.Size = New System.Drawing.Size(896, 409)
        Me.splitContainer1.SplitterDistance = 173
        Me.splitContainer1.TabIndex = 2
        '
        '_treeView
        '
        Me._treeView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._treeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me._treeView.Enabled = False
        Me._treeView.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._treeView.FullRowSelect = True
        Me._treeView.ItemHeight = 32
        Me._treeView.Location = New System.Drawing.Point(0, 0)
        Me._treeView.Name = "_treeView"
        Me._treeView.ShowLines = False
        Me._treeView.ShowPlusMinus = False
        Me._treeView.ShowRootLines = False
        Me._treeView.Size = New System.Drawing.Size(173, 409)
        Me._treeView.TabIndex = 0
        '
        'splitContainer2
        '
        Me.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.splitContainer2.Name = "splitContainer2"
        Me.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainer2.Panel1
        '
        Me.splitContainer2.Panel1.Controls.Add(Me.label2)
        Me.splitContainer2.Panel1.Controls.Add(Me._tabControl)
        '
        'splitContainer2.Panel2
        '
        Me.splitContainer2.Panel2.Controls.Add(Me.btnNextStep)
        Me.splitContainer2.Panel2.Controls.Add(Me.btnPrevStep)
        Me.splitContainer2.Size = New System.Drawing.Size(719, 409)
        Me.splitContainer2.SplitterDistance = 365
        Me.splitContainer2.TabIndex = 0
        '
        'label2
        '
        Me.label2.BackColor = System.Drawing.SystemColors.Control
        Me.label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.Location = New System.Drawing.Point(0, 0)
        Me.label2.Margin = New System.Windows.Forms.Padding(3)
        Me.label2.Name = "label2"
        Me.label2.Padding = New System.Windows.Forms.Padding(4, 0, 0, 0)
        Me.label2.Size = New System.Drawing.Size(719, 22)
        Me.label2.TabIndex = 4
        Me.label2.Text = "Label intestazione"
        Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_tabControl
        '
        Me._tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me._tabControl.Controls.Add(Me.tabPage1)
        Me._tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me._tabControl.Location = New System.Drawing.Point(0, 0)
        Me._tabControl.Name = "_tabControl"
        Me._tabControl.SelectedIndex = 0
        Me._tabControl.Size = New System.Drawing.Size(719, 365)
        Me._tabControl.TabIndex = 0
        '
        'tabPage1
        '
        Me.tabPage1.BackColor = System.Drawing.SystemColors.Window
        Me.tabPage1.Location = New System.Drawing.Point(4, 25)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Size = New System.Drawing.Size(711, 336)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "Step 1"
        '
        'btnNextStep
        '
        Me.btnNextStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextStep.FlatAppearance.BorderSize = 0
        Me.btnNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNextStep.Image = CType(resources.GetObject("btnNextStep.Image"), System.Drawing.Image)
        Me.btnNextStep.Location = New System.Drawing.Point(577, 4)
        Me.btnNextStep.MySize = MyControlsLibrary.MyButton.ButtonSize.px122
        Me.btnNextStep.MyType = MyControlsLibrary.MyButton.ButtonType.btnNext
        Me.btnNextStep.Name = "btnNextStep"
        Me.btnNextStep.Padding = New System.Windows.Forms.Padding(0, 0, 0, 5)
        Me.btnNextStep.Size = New System.Drawing.Size(130, 30)
        Me.btnNextStep.TabIndex = 0
        Me.btnNextStep.Text = "Avanti"
        Me.btnNextStep.UseVisualStyleBackColor = True
        '
        'btnPrevStep
        '
        Me.btnPrevStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrevStep.FlatAppearance.BorderSize = 0
        Me.btnPrevStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrevStep.Image = CType(resources.GetObject("btnPrevStep.Image"), System.Drawing.Image)
        Me.btnPrevStep.Location = New System.Drawing.Point(14, 4)
        Me.btnPrevStep.MySize = MyControlsLibrary.MyButton.ButtonSize.px122
        Me.btnPrevStep.MyType = MyControlsLibrary.MyButton.ButtonType.btnPrev
        Me.btnPrevStep.Name = "btnPrevStep"
        Me.btnPrevStep.Padding = New System.Windows.Forms.Padding(0, 0, 0, 5)
        Me.btnPrevStep.Size = New System.Drawing.Size(130, 30)
        Me.btnPrevStep.TabIndex = 1
        Me.btnPrevStep.Text = "Indietro"
        Me.btnPrevStep.UseVisualStyleBackColor = True
        Me.btnPrevStep.Visible = False
        '
        '_ErrorProvider
        '
        Me._ErrorProvider.ContainerControl = Me
        '
        'lblTitoloWizard
        '
        Me.lblTitoloWizard.AutoSize = True
        Me.lblTitoloWizard.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.lblTitoloWizard.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitoloWizard.ForeColor = System.Drawing.Color.White
        Me.lblTitoloWizard.Location = New System.Drawing.Point(138, 9)
        Me.lblTitoloWizard.Name = "lblTitoloWizard"
        Me.lblTitoloWizard.Size = New System.Drawing.Size(275, 39)
        Me.lblTitoloWizard.TabIndex = 3
        Me.lblTitoloWizard.Text = "Titolo del Wizard"
        '
        'FormWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(896, 476)
        Me.Controls.Add(Me.lblTitoloWizard)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.pictureBoxTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "FormWizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FormWzInsert"
        CType(Me.pictureBoxTitle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.ResumeLayout(False)
        Me.splitContainer2.Panel1.ResumeLayout(False)
        Me.splitContainer2.Panel2.ResumeLayout(False)
        Me.splitContainer2.ResumeLayout(False)
        Me._tabControl.ResumeLayout(False)
        CType(Me._ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected label2 As System.Windows.Forms.Label
    Protected tabPage1 As System.Windows.Forms.TabPage
    Protected _treeView As System.Windows.Forms.TreeView
    Protected WithEvents _tabControl As System.Windows.Forms.TabControl
    Protected _initTab As System.Collections.Hashtable = New System.Collections.Hashtable
    Protected _ErrorProvider As System.Windows.Forms.ErrorProvider
    Private splitContainer2 As System.Windows.Forms.SplitContainer
    Private splitContainer1 As System.Windows.Forms.SplitContainer




    Protected pictureBoxTitle As System.Windows.Forms.PictureBox
    Protected WithEvents btnNextStep As MyControlsLibrary.MyButton
    Protected WithEvents btnPrevStep As MyControlsLibrary.MyButton
    Friend WithEvents lblTitoloWizard As System.Windows.Forms.Label
End Class
