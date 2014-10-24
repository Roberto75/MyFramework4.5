using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFormsLibrary
{
    public partial class FormAbout : Form
    {

        private bool isReleaseNote;


        public FormAbout()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        public bool _init(System.Drawing.Image logo)
        {
            this.label1.Text = Application.ProductName + " " + Application.ProductVersion;
                //this.GetType().Assembly.GetName().Version.ToString();


            if (this.Owner != null)
            {

                this.Icon = Owner.Icon;
            }


            tabPage1.Text = Application.ProductName;
            pictureBox1.Image = logo;

            isReleaseNote = System.IO.File.Exists(Application.StartupPath + "\\whatsnew.xml");
            if (!isReleaseNote)
            {
                tabControl1.TabPages.Remove(tabPage2);
            }
            else
            {
                fillReleaseNotes();
            }
            return true;
        }





        private bool fillReleaseNotes()
        {
            System.IO.MemoryStream risultatoMemoryStream = new System.IO.MemoryStream();
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();

            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new System.IO.StringReader(MyFormsLibrary.Properties.Resources.whatsnew));
            xslt.Load(reader);

            xslt.Transform(Application.StartupPath + "\\whatsnew.xml", null, risultatoMemoryStream);
            risultatoMemoryStream.Flush();

            string html;
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            html = encoding.GetString(risultatoMemoryStream.GetBuffer());
            html = "<h1>" + Application.ProductName + "</h1>" + html;

            webBrowser1.DocumentText = html;
            return true;
        }
    }
}
