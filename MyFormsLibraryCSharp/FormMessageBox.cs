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
    public partial class FormMessageBox : Form
    {
        public FormMessageBox()
        {
            InitializeComponent();
        }

        public enum MyButtons{
            Ok,
            OkCancel
        }

        public enum MyIcon{
            Info,
            Warning,
            Error
        }


        private MyButtons _buttons;


        public void _init(string title, string message, MyButtons buttons, MyIcon icon)
        {

            this.Text = title;
       
            _buttons = buttons;

            switch (buttons)
            {
                case MyButtons.Ok:
                    button1.Text = "OK";
                    button2.Visible = false;
                    break;
                case MyButtons.OkCancel:
                    button1.Text = "Cancel";
                    button2.Text = "OK";
                    break;
            }



            switch (icon)
            {
                case MyIcon.Error:
                    pictureBox1.Image = Properties.Resources.error;
                    break;
                case MyIcon.Info :
                    pictureBox1.Image = Properties.Resources.info;
                    break;
                case MyIcon.Warning :
                    pictureBox1.Image = Properties.Resources.warning;
                    break;
            }


            this.label1.Text = message;

            int modulo;
            modulo  = message.Length/ 150;

            int newSize;
            newSize = modulo * 50;

            if (newSize > this.Height)
            {
                this.Height = newSize;
            }

           
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
          


            if (this.Modal)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.Close();
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {



            if (this.Modal)
            {
                switch (_buttons)
                {
                    case MyButtons.Ok:
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                        break;
                    case MyButtons.OkCancel:
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        break;
                }
            }
            else
            {
                this.Close();
            }

           
        }


    }
}
