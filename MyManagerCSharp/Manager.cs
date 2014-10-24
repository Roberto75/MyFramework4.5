using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    
    public class Manager
    {

        #region "Progress Bar e Statu Bar"
            
        protected System.Windows.Forms.ToolStripProgressBar _progressBar;
    
        protected System.Windows.Forms.ToolStripLabel _statusBar;

    
        public void _setProgressBar(System.Windows.Forms.ToolStripProgressBar value)
        {
            _progressBar = value;
        }

        public void _setStatusBar(System.Windows.Forms.ToolStripLabel value)
        {
            _statusBar = value;
        }

        public void _statusBarUpdate(string value)
        {
            if (_statusBar == null)
            {
                return;
            }
            _statusBar.Text = value;
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void _progressBarPerformStep()
        {
            if (_progressBar == null)
            {
                return;
            }
            _progressBar.PerformStep();
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void _progressBarSetValue(int value)
        {
            if (_progressBar == null)
            {
                return;
            }
            _progressBar.Value = value;
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void _progressBarSetMaximumValue(int value)
        {
            if (_progressBar == null)
            {
                return;
            }
            _progressBar.Maximum = value;
        }

        #endregion


         

    }
}
