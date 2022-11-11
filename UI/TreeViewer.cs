using System.Collections;
using System.Reflection;

namespace PBT.DowsingMachine.UI;

public partial class TreeViewer : Form
{
    public TreeViewer()
    {
        InitializeComponent();

    }

    public TreeViewer(object data) : this()
    {
        treeView1.SuspendLayout();
        treeView1.Nodes.Clear();

        AddNode(data);

        treeView1.ResumeLayout();
    }

    private void DataViewer_Load(object sender, EventArgs e)
    {

    }

    private void AddNode(object data, TreeNode? parentNode = null)
    {
        var parentNodes = parentNode?.Nodes ?? treeView1.Nodes;

        switch (data)
        {
            case null:
                {
                    var text = $"(null)";
                    var node = parentNodes.Add(text);
                }
                break;
            case IEnumerable x:
                {
                    foreach (var y in x)
                    {
                        var node = parentNodes.Add(x.ToString());
                        AddNode(y, node);
                    }
                    if (parentNode is not null)
                    {
                        parentNode.Tag = x;
                        parentNode.Text += " ...";
                    }
                }
                break;
            case object x when parentNode is null:
                {
                    var type = x.GetType();
                    var rootNode = parentNodes.Add(type.Name);
                    AddNode(x, rootNode);
                }
                break;
            case object x:
                {
                    var type = x.GetType();
                    var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();
                    foreach (var prop in properties)
                    {
                        if (prop.PropertyType.IsArray)
                        {
                            var node = parentNodes.Add(prop.Name);
                            var value = prop.GetValue(x);
                            AddNode(value!, node);
                        }
                    }

                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
                    foreach (var field in fields)
                    {
                        if (field.FieldType.IsArray)
                        {
                            var node = parentNodes.Add(field.Name);
                            var value = field.GetValue(x);
                            AddNode(value!, node);
                        }
                    }

                    parentNode.Tag = data;
                }
                break;
        }
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {

    }

    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
    }

    private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if(e.Button == MouseButtons.Right)
        {
            if (e.Node.Tag is IEnumerable x)
            {
                using var frm = new GridViewer(x);
                frm.ShowDialog();
            }
            else if (e.Node.Tag is not null)
            {
                var json = Data.JsonUtil.Serialize(e.Node.Tag);
                using var frm = new TextViewer(json);
                frm.ShowDialog();
            }
        }
    }
}
