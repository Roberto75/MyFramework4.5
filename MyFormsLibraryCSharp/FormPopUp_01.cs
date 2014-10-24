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
    public partial class FormPopUp_01 : Form
    {
        public FormPopUp_01()
        {
            InitializeComponent();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            _buttonAnnullaOnClick();
        }

        public bool _buttonAnnullaOnClick()
        {
            if (this.Modal)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                this.Close();
            }
            
            return true;
        }

        private void FormPopUp_01_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (this.Owner != null)
            {

                this.Icon = Owner.Icon;
            }
        }

        private void btnConferma_Click(object sender, EventArgs e)
        {
            _buttonConfermaOnClick();
        }

        public bool _buttonConfermaOnClick()
        {
            if (!_checkValuesBeforeAction())
            {
                return false;
            }

            if (System.Windows.Forms.MessageBox.Show("Confermare l'operazione?", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
            {
                return false;
            }

            if (_executeAction())
            {
                System.Windows.Forms.MessageBox.Show("Operazione conclusa con successo", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
         
            return true;
        }

        virtual public bool _checkValuesBeforeAction()
        {
            return true;
        }

        virtual public bool _executeAction()
        {
            return true;
        }


    }
}
