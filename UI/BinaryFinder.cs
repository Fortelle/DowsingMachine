using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities.BinaryFinder;
using System.Data;
using System.Text;

namespace PBT.DowsingMachine.UI;

public partial class BinaryFinder : Form
{
    public BinaryFinder()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();

        var folder = folderBrowserDialog1.SelectedPath;
        if (!Directory.Exists(folder)) return;
        var files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
        
        var bf = new BmhFinder(new byte[] {});

        var sb = new StringBuilder();
        var job = files.Select(x =>
        {
            //var data = File.ReadAllBytes(x);
            foreach (var result in bf.Find(x))
            {
                sb.AppendLine($"{x}, 0x{result:X8}, {1}");
            }
            return x;
        });

        var wp = new WorkProcesser();
        wp.Set(job);
        wp.ShowDialog(true);

        new TextViewer(sb).ShowDialog();
    }

    private void BinaryFinder_Load(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
        txtFolder.Text = folderBrowserDialog1.SelectedPath;
    }

}