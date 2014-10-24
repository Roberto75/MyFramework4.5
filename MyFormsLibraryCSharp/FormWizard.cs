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
    public partial class FormWizard : Form
    {

        protected  int _currentStep = 0;


        public FormWizard()
        {
            InitializeComponent();
        }


        public void _hideButtonIndietro()
        {
            btnIndietro.Visible = false;
        }


        public virtual bool _init(string title)
        {
            if (this.Owner != null && this.Owner.Icon != null)
            {
                this.Icon = this.Owner.Icon;
            }

            this.myTabControl1._initTabPageStatus();
            this.myTabControl1.Selecting += new TabControlCancelEventHandler(myTabControl_Selecting);

            if (myTabControl1.TabCount == 1)
            {
                btnAvanti.Text = "Fine";
              //  btnIndietro.Visible = false;
            }

            this.label1.Text = title;

            this.Text = this.GetType().Assembly.GetName().Name + " " + this.GetType().Assembly.GetName().Version.ToString() + " - " + title;
        

            btnIndietro.Visible = false;
            return true;
        }

        void myTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == myTabControl1.TabCount - 1)
            {
                btnAvanti.Text = "Fine";
            }
            else
            {
                btnAvanti.Text = "Avanti >>";
            }

           //base.OnSelecting(e);
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }




        private void btnAvanti_Click(object sender, EventArgs e)
        {
            if (_currentStep == myTabControl1.TabPages.Count -1)
            {
                if (_checkBeforeSaveAction() == true)
                {
                    if (System.Windows.Forms.MessageBox.Show("Confermare l'operazione?", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        _saveAction();
                    }
                }
            }
            else
            {
                // navigazione normale
                _goToNextStep();
            }
        }



        public virtual bool _goToNextStep()
        {
            if (! _checkBeforeNextStep (_currentStep )) {
                return false;
            }
            _currentStep = _currentStep + 1;
            btnIndietro.Visible = true;
            myTabControl1.SelectedIndex = _currentStep;
            return true;
        }



        public void _goToStep(int stepNumber)
        {
            _currentStep = stepNumber;
            myTabControl1.SelectedIndex = _currentStep;
        }



        public virtual void _goToBackStep()
        {
            _currentStep = _currentStep - 1;
            myTabControl1.SelectedIndex = _currentStep;
        }

        public virtual bool _saveAction()
        {
            System.Windows.Forms.MessageBox.Show("Function not available", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            return false;
        }


        public virtual bool _checkBeforeSaveAction()
        {
            return true;
        }



        public virtual bool _checkBeforeNextStep(int stepIndex )
        {
            return true;
        }



        private void btnIndietro_Click(object sender, EventArgs e)
        {
            if (_currentStep == 0) {
                return;
            }

            if (_currentStep == 1)
            {
                btnIndietro.Visible = false;
            }
            _goToBackStep();
        }







    }
}
