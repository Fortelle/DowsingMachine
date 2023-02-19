using System.Collections;
using System.Data;
using System.Text;

namespace PBT.DowsingMachine.UI;

public partial class TextViewer : Form
{
    public TextViewer()
    {
        InitializeComponent();
    }

    public TextViewer(object data) : this()
    {
        switch (data)
        {
            case byte[][] x:
                Show(x);
                break;
            case byte[] x:
                Show(x);
                break;
            default:
                txtContent.Text = ToText(data);
                break;
        }
        txtHeader.Visible = txtHeader.TextLength > 0;
        txtContent.SelectionStart = 0;
        txtContent.SelectionLength = 0;
    }

    private static string ToHex(byte[] data)
    {
        return string.Join("  ", data.Chunk(16).Select(x => BitConverter.ToString(x).Replace("-", " "))); 
    }

    private static string ToText(object obj)
    {
        return obj switch
        {
            string x => x,
            IEnumerable<string> x => string.Join("\n", x),
            IEnumerable x => string.Join("\n", x.Cast<object>().Select(ToText)),
            null => "(null)",
            _ => obj.ToString()
        };
    }

    private void Show(byte[][] data)
    {
        var max = data.Max(x => x.Length);
        var header = ToHex(Enumerable.Range(0, max).Select(x => (byte)x).ToArray());
        txtHeader.Text = header;
        txtHeader.SelectionStart = 0;
        txtHeader.SelectionLength = 0;

        var sb = new StringBuilder();
        foreach (var r in data)
        {
            sb.AppendLine(ToHex(r));
        }
        txtContent.Text = sb.ToString();
    }

    private void Show(byte[] data)
    {
        var header = string.Join(" ", Enumerable.Range(0, 16).Select(x => $"{x:X2}"));
        txtHeader.Text = header;
        txtHeader.SelectionStart = 0;
        txtHeader.SelectionLength = 0;

        var sb = new StringBuilder();
        foreach (var r in data.Chunk(16))
        {
            sb.AppendLine(BitConverter.ToString(r).Replace("-", " "));
        }
        txtContent.Text = sb.ToString();
    }

    private void TextViewer_Load(object sender, EventArgs e)
    {

    }

    public static void Show(object data)
    {
        var frm = new TextViewer(data);
        frm.Show();
    }

}
