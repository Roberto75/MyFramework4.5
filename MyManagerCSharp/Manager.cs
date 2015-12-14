using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    
    public class Manager
    {

        #region "Progress Bar e Statu Bar"
            
        protected System.Windows.Forms.ToolStripProgressBar m_progressBar;
        protected System.Windows.Forms.ToolStripLabel m_statusBar;

    
        public void SetProgressBar(System.Windows.Forms.ToolStripProgressBar value)
        {
            m_progressBar = value;
        }

        public void SetStatusBar(System.Windows.Forms.ToolStripLabel value)
        {
           m_statusBar = value;
        }

        public void StatusBarUpdate(string value)
        {
            if (m_statusBar == null)
            {
                return;
            }
            m_statusBar.Text = value;
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void ProgressBarPerformStep()
        {
            if (m_progressBar == null)
            {
                return;
            }
            m_progressBar.PerformStep();
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void ProgressBarSetValue(int value)
        {
            if (m_progressBar == null)
            {
                return;
            }
            m_progressBar.Value = value;
            System.Windows.Forms.Application.DoEvents();
        }
        
        public void ProgressBarSetMaximumValue(int value)
        {
            if (m_progressBar == null)
            {
                return;
            }
            m_progressBar.Maximum = value;
        }

        #endregion

                

    }
}
