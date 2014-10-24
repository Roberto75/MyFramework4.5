using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyControlsLibrary
{
    public partial class UcBrowseFileSystem : UserControl
    {
        public UcBrowseFileSystem()
        {
            InitializeComponent();
        }


        public bool init()
        {
            string[] drives;
            drives = System.Environment.GetLogicalDrives();

            System.Windows.Forms.TreeNode baseNode;

            foreach (string d in drives)
            {
                baseNode = MyTreeView.Nodes.Add(d);
                baseNode.Tag = d;
            }


            System.Windows.Forms.ColumnHeader h;
            h = listView1.Columns.Add("File name");
            h.Width = 300;
            
            return true;
        }
    }
}
