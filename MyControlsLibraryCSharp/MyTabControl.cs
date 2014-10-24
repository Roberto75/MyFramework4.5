using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyControlsLibrary
{
    public class MyTabControl : TabControl
    {

        public delegate void TabPageStatusEventHandler(object sender, MyTabPageEventArgs e);
        public event TabPageStatusEventHandler MyTabPageIsNotLoaded;

        public enum MyStatus
        {
            Undefined,
            Changed,
            Loaded
        }

        private List<bool> _tabPageStatus = new List<bool>() ;

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            if (this.DesignMode) return;



            //DIASBILITO 

            //if (_tabPageStatus[e.TabPageIndex] == true)
            //{
            //    base.OnSelecting(e);
            //    return;
            //}

            // Rilancio un evento per notificare che il TabPage NON è stato ancora caricato

            MyTabPageEventArgs ev = new MyTabPageEventArgs();
            ev.TabPageIndex = e.TabPageIndex;
            ev.TabPage = e.TabPage;
            ev.Status = MyStatus.Undefined;


            if (MyTabPageIsNotLoaded != null)
            {
                MyTabPageIsNotLoaded(this, ev);
            }

            // 11/04/2012
            // dopo aver rilanciato l'evento gli cambio stato
            // se tutto procede come dovrebbe, al prossimo passaggio il tab deve risultare caricato

            //DISABILITO
            //_tabPageStatus[e.TabPageIndex] = true;

            base.OnSelecting(e);
        }

        public void _resetTabPageStatus(TabPage tabPage)
        {
            int i ;
            i = this.TabPages.IndexOf(tabPage);
            _tabPageStatus.Insert(i, false);
        }

        public void _resetTabPageStatus()
        {
            _initTabPageStatus();
        }

        public void _refreshTabPage(TabPage tabPage)
        {
            int i;
            i = this.TabPages.IndexOf(tabPage);

            MyTabPageEventArgs ev = new MyTabPageEventArgs();
            ev.TabPageIndex = i;
            ev.TabPage = tabPage;
            ev.Status = MyStatus.Undefined;

            tabPage.Invalidate();

            if (MyTabPageIsNotLoaded != null)
            {
                MyTabPageIsNotLoaded(this, ev);
            }

            // 11/04/2012
            // dopo aver rilanciato l'evento gli cambio stato
            // se tutto procede come dovrebbe, al prossimo passaggio il tab deve risultare caricato
            _tabPageStatus[i] = true;

        }

        public void _initTabPageStatus()
        {
            _tabPageStatus.Clear();
            for (int i = 0; i < this.TabCount ; i++)
            {
                _tabPageStatus.Insert(i, false);
            }
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode)
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }

    }

}
