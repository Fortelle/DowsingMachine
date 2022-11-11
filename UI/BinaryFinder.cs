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
        var files = Directory.GetFiles(@"E:\Pokemon\Resources\Unpacked\NS\Legend\romfs\", "*bin", SearchOption.AllDirectories);
        
        var bf = new BmhFinder(new byte[] {
            0x1F, 00, 0x28, 00, 0x3C, 00, 0x70, 00
        });

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
}