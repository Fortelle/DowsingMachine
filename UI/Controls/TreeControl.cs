using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBT.DowsingMachine.UI.Controls;

public partial class TreeControl : UserControl
{
    public TreeControl()
    {
        InitializeComponent();
    }


    public class TreeNode
    {
        public bool HasChildren { get; set; }
        public string Name { get; set; }
        public string ImageKey { get; set; }
        public TreeNode[] Nodes { get; set; }
    }
}
