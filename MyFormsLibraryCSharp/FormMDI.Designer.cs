namespace MyFormsLibrary
{
    public partial class FormMDI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MenuStripTop = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 389);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(924, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MenuStripTop
            // 
            this.MenuStripTop.Location = new System.Drawing.Point(0, 0);
            this.MenuStripTop.Name = "MenuStripTop";
            this.MenuStripTop.Size = new System.Drawing.Size(924, 24);
            this.MenuStripTop.TabIndex = 2;
            this.MenuStripTop.Text = "menuStrip1";
            // 
            // FormMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 411);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MenuStripTop);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuStripTop;
            this.Name = "FormMDI";
            this.Text = "MDIForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        protected System.Windows.Forms.MenuStrip MenuStripTop;
    }
}