using PBT.DowsingMachine.Data;
using PBT.DowsingMachine.Projects;
using PBT.DowsingMachine.Utilities;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace PBT.DowsingMachine.UI;

public partial class GridViewer : Form
{
    public Func<object[], string> GetString { get; set; }

    public GridViewer()
    {
        InitializeComponent();

        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
    }

    public GridViewer(IEnumerable data) : this()
    {
        //var asrm = dataGridView1.AutoSizeRowsMode;
        //var ascm = dataGridView1.AutoSizeColumnsMode;
        //var chhs = dataGridView1.ColumnHeadersHeightSizeMode;

        //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        //dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        dataGridView1.Tag = data;
    }

    private void GridViewer_Load(object sender, EventArgs e)
    {
        var data = (IEnumerable)dataGridView1.Tag;
        dataGridView1.SuspendLayout();

        //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        //dataGridView1.AutoSizeColumnsMode = ascm;
        //dataGridView1.ColumnHeadersHeightSizeMode = chhs;
        //dataGridView1.ResumeLayout();
        switch (data)
        {
            case IDictionary dict:
                ShowDictionary(dict);
                break;
            case byte[] x:
                ShowByte(x);
                break;
            case IEnumerable<byte[]> x:
                ShowBytes(x);
                break;
            case IEnumerable<JsonNode> x:
                ShowNodes(x);
                break;
            case IEnumerable:
                var type = data.GetType();
                switch (type.GetElementType())
                {
                    case null:
                        var objarray = data.Cast<object>();
                        var first = objarray.First();
                        if (first is ITuple)
                        {
                            ShowProperties(data, first.GetType());
                        }
                        else
                        {
                            ShowList(data);
                        }
                        break;
                    case var t when t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>):
                        ShowDictionaryCollection(data);
                        break;
                    default:
                        var elementType = data.GetType()
                            .GetInterfaces()
                            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            .GetGenericArguments()
                            .FirstOrDefault();

                        if (elementType == null || elementType.IsEnum || elementType.IsPrimitive || elementType == typeof(string))
                        {
                            ShowList(data);
                        }
                        else if (elementType.IsArray)
                        {
                            ShowMatrix(data);
                        }
                        else
                        {
                            ShowProperties(data, elementType);
                        }
                        break;
                }

                break;
        }
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

        dataGridView1.ResumeLayout();
    }

    private void ShowDictionaryCollection(IEnumerable list)
    {
        var data = new List<Dictionary<string, string>>();
        foreach(IDictionary entry in list)
        {
            var dict = new Dictionary<string, string>();
            foreach (DictionaryEntry item in entry)
            {
                var key = StringUtil.ToLine(item.Key);
                var value = StringUtil.ToLine(item.Value);
                dict.Add(key, value);
            };
            data.Add(dict);
        }

        var keys = data.SelectMany(x => x.Keys).Distinct().ToArray();
        if (keys.Length == 0) keys = new[] { "*(none)" };
        foreach (var key in keys)
        {
            var col = dataGridView1.Columns.Add(key, key);
            dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        for (var i = 0; i < data.Count; i++)
        {
            var row = dataGridView1.Rows.Add();
            foreach (var key in keys)
            {
                dataGridView1.Rows[row].Cells[key].Value = data[i][key];
            }
            dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
        }
    }

    private void ShowProperties(IEnumerable list, Type type)
    {
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();
        foreach (var prop in properties)
        {
            var col = dataGridView1.Columns.Add(prop.Name, prop.Name);
            dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
        foreach (var field in fields)
        {
            var col = dataGridView1.Columns.Add(field.Name, field.Name);
            dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        var entryStringRef = type.GetCustomAttribute<StringReferenceAttribute>();

        var i = 0;
        foreach (var item in list)
        {
            var row = dataGridView1.Rows.Add();

            if (GetString != null && entryStringRef != null)
            {
                var args = entryStringRef.Arguments.Append(i).ToArray();
                var text = GetString(args);
                dataGridView1.Rows[row].HeaderCell.Value = $"{i} ({text})";
            }
            else
            {
                dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
            }


            foreach (var prop in properties)
            {
                var value = prop.GetValue(item);
                var text = StringUtil.ToLine(value);
                if(GetString != null)
                {
                    var sr = prop.GetCustomAttribute<StringReferenceAttribute>();
                    if (sr != null)
                    {
                        var args = sr.Arguments.Append(value).ToArray();
                        text += " (" + GetString(args) + ")";
                    }
                }

                dataGridView1.Rows[row].Cells[prop.Name].Value = text;
            }
            foreach (var field in fields)
            {
                var value = field.GetValue(item);
                var text = StringUtil.ToLine(value);
                if (GetString != null)
                {
                    var sr = field.GetCustomAttribute<StringReferenceAttribute>();
                    if (sr != null)
                    {
                        var args = sr.Arguments.Append(value).ToArray();
                        text += " (" + GetString(args) + ")";
                    }
                }
                else
                {
                    var sr = field.GetCustomAttribute<AsEnumAttribute>();
                    if (sr != null && Enum.TryParse(sr.EnumType, value.ToString(), out var enumvalue))
                    {
                        text += $" ({enumvalue})";
                    }
                }

                dataGridView1.Rows[row].Cells[field.Name].Value = text;
            }
            i++;
        }
    }

    private void ShowList(IEnumerable list)
    {
        var col = dataGridView1.Columns.Add("Items", "Items");
        dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        var i = 0;
        foreach (var item in list.Cast<object>())
        {
            var text = StringUtil.ToLine(item);
            var row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
            dataGridView1.Rows[row].Cells[0].Value = text;
            i++;
        }
    }

    private void ShowDictionary(IDictionary dict)
    {
        var col = dataGridView1.Columns.Add("Key", "Key");
        dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        col = dataGridView1.Columns.Add("Value", "Value");
        dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

        var i = 0;
        foreach (DictionaryEntry kv in dict)
        {
            var text = StringUtil.ToLine(kv.Value);
            var row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
            dataGridView1.Rows[row].Cells[0].Value = kv.Key.ToString();
            dataGridView1.Rows[row].Cells[1].Value = text;
            i++;
        }
    }

    private void ShowMatrix(IEnumerable data)
    {
        var col = 0;
        foreach(IEnumerable obj in data)
        {
            var c = 0;
            foreach (var x in obj)
            {
                c++;
            }
            col = Math.Max(col, c);
        }

        for (var i = 0; i < col; i++)
        {
            var key = i.ToString();
            dataGridView1.Columns[dataGridView1.Columns.Add(key, key)].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        var j = 0;
        foreach (IEnumerable obj in data)
        {
            var row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].HeaderCell.Value = $"{j++}";
            var k = 0;
            foreach (var x in obj)
            {
                dataGridView1.Rows[row].Cells[k].Value = StringUtil.ToLine(x);
                k++;
            }
        }
    }

    private void ShowByte(byte[] data)
    {
        for (var i = 0; i < 16; i++)
        {
            var key = i.ToString("X2");
            dataGridView1.Columns[dataGridView1.Columns.Add(key, key)].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        var table = data.Chunk(16);
        var rows = Math.Ceiling((double)data.Length / 16);
        var j = 0;
        foreach (var x in table)
        {
            var row = new DataGridViewRow();
            row.HeaderCell.Value = $"{j}";
            var y = x.Select(z => z.ToString("X2")).ToArray();
            dataGridView1.Rows.Add(y);
            j++;
        }
    }

    private void ShowBytes(IEnumerable<byte[]> data)
    {
        var col = data.Max(x => x.Length);
        for (var i = 0; i < col; i++)
        {
            var key = i.ToString("X2");
            dataGridView1.Columns[dataGridView1.Columns.Add(key, key)].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        var j = 0;
        foreach (var bytes in data)
        {
            var row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].HeaderCell.Value = $"{j}";
            for (var k = 0; k < bytes.Length; k++)
            {
                dataGridView1.Rows[row].Cells[k].Value = bytes[k].ToString("X2"); ;
            }
            j++;
        }
    }

    private void ShowNodes(IEnumerable<JsonNode> nodes)
    {
        var keys = nodes.SelectMany(x => x.AsObject().Select(y => y.Key)).Distinct().ToArray();
        if (keys.Length == 0) keys = new[] { "*(none)" };

        foreach (var key in keys)
        {
            var col = dataGridView1.Columns.Add(key, key);
            dataGridView1.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        var i = 0;
        foreach (var node in nodes)
        {
            var row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
            foreach (var kv in node.AsObject())
            {
                dataGridView1.Rows[row].Cells[kv.Key].Value = StringUtil.ToLine(kv.Value);
            }
            i++;
        }
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void jsonToolStripMenuItem_Click(object sender, EventArgs e)
    {
        saveFileDialog1.FileName = Path.ChangeExtension(saveFileDialog1.FileName, "*.json");
        if(saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            JsonUtil.Serialize(saveFileDialog1.FileName, dataGridView1.Tag);
        }
    }

    private void csvToolStripMenuItem_Click(object sender, EventArgs e)
    {
        saveFileDialog1.FileName = Path.ChangeExtension(saveFileDialog1.FileName, "*.csv");
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            var data = new List<object[]>();
            {
                var x = new List<object>();
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    x.Add(col.HeaderText);
                }
                data.Add(x.ToArray());
            }

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                var x = new List<object>();
                foreach(DataGridViewCell cell in row.Cells)
                {
                    x.Add(cell.Value);
                }
                data.Add(x.ToArray());
            }
            CsvUtil.Serialize(saveFileDialog1.FileName, data.ToArray());
        }
    }
}
