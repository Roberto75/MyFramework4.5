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
    public partial class FormLogin : Form
    {

        public delegate void btnLoginEventHandler(object sender, EventArgs e);
        public event btnLoginEventHandler ButtonLoginOnClick;


        public FormLogin()
        {
            InitializeComponent();
        }



        public virtual bool _init(string title)
        {
            this.Text = title;
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ButtonLoginOnClick != null)
            {
                ButtonLoginOnClick(this, e);
            }
        }
    }
}
