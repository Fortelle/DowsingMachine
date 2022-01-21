using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBT.DowsingMachine.UI
{
    public partial class TreeViewer : Form
    {
        public TreeViewer()
        {
            InitializeComponent();
        }

        public TreeViewer(object data) : this()
        {
            treeView1.SuspendLayout();

            treeView1.ResumeLayout();
        }

        private void DataViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
