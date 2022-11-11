using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBT.DowsingMachine.UI;

public partial class ImageViewer : Form
{
    public ImageViewer()
    {
        InitializeComponent();
    }

    private void ImageViewer_Load(object sender, EventArgs e)
    {

    }

    public ImageViewer(Bitmap image) : this()
    {
        pictureBox1.Image = image;
        this.Width = image.Width;
        this.Height = image.Height;
    }
}
