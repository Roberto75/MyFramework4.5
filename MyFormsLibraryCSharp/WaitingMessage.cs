using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFormsLibrary
{
   public static  class WaitingMessage
    {


        static private System.Windows.Forms.Form _formToDisplay;
        static private System.Windows.Forms.Form _owner;

        static public void Show(System.Windows.Forms.Form owner)
        {
            FormAbout f = new FormAbout();
            Show(f, owner);
        }

        static public void Show(System.Windows.Forms.Form formToDisplay, System.Windows.Forms.Form owner)
        {

            _formToDisplay = formToDisplay;

            _owner = owner;


            System.ComponentModel.BackgroundWorker worker;

            worker = new System.ComponentModel.BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(workerStart);
        }


    static   public void Hide(){

           _formToDisplay.Close(); 
       }


     static   private void workerStart(Object sender, System.ComponentModel.DoWorkEventArgs arg)
        {
            _formToDisplay.ShowDialog(_owner );
        }
    }
}
