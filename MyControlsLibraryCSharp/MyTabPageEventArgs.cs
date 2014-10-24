using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyControlsLibrary
{
    public class MyTabPageEventArgs : EventArgs
    {

        private MyTabControl.MyStatus _status;
        public MyTabControl.MyStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int TabPageIndex { get; set; }
        public System.Windows.Forms.TabPage   TabPage { get; set; }

        public MyTabPageEventArgs(MyTabControl.MyStatus status, int tabPageIndex, ref System.Windows.Forms.TabPage tabPage)
        {
            this._status = status;
            this.TabPageIndex = tabPageIndex;
            this.TabPage = tabPage;
        }

        public MyTabPageEventArgs()
        {
        }
    }
}
