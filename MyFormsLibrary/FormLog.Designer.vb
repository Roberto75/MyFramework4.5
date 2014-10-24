<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLog
    Inherits System.Windows.Forms.Form

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
    	Me.Panel1 = New System.Windows.Forms.Panel
    	Me.Button1 = New System.Windows.Forms.Button
    	Me.ListView1 = New System.Windows.Forms.ListView
    	Me.Panel1.SuspendLayout
    	Me.SuspendLayout
    	'
    	'Panel1
    	'
    	Me.Panel1.Controls.Add(Me.Button1)
    	Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
    	Me.Panel1.Location = New System.Drawing.Point(0, 225)
    	Me.Panel1.Name = "Panel1"
    	Me.Panel1.Size = New System.Drawing.Size(444, 41)
    	Me.Panel1.TabIndex = 0
    	'
    	'Button1
    	'
    	Me.Button1.Location = New System.Drawing.Point(357, 8)
    	Me.Button1.Name = "Button1"
    	Me.Button1.Size = New System.Drawing.Size(75, 23)
    	Me.Button1.TabIndex = 0
    	Me.Button1.Text = "OK"
    	Me.Button1.UseVisualStyleBackColor = true
    	'
    	'ListView1
    	'
    	Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.ListView1.Location = New System.Drawing.Point(0, 0)
    	Me.ListView1.Name = "ListView1"
    	Me.ListView1.Size = New System.Drawing.Size(444, 225)
    	Me.ListView1.TabIndex = 1
    	Me.ListView1.UseCompatibleStateImageBehavior = false
    	Me.ListView1.View = System.Windows.Forms.View.List
    	'
    	'FormLog
    	'
    	Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    	Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    	Me.ClientSize = New System.Drawing.Size(444, 266)
    	Me.Controls.Add(Me.ListView1)
    	Me.Controls.Add(Me.Panel1)
    	Me.Name = "FormLog"
    	Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    	Me.Text = "FormLog"
    	Me.Panel1.ResumeLayout(false)
    	Me.ResumeLayout(false)
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
End Class
