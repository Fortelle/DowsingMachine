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
    public partial class GridViewer : Form
    {
        public GridViewer()
        {
            InitializeComponent();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        public GridViewer(IEnumerable data) : this()
        {
            dataGridView1.SuspendLayout();

            switch (data.GetType().GetElementType())
            {
                case var t when t == typeof(IDictionary):
                    ShowDictionary(data);
                    break;
                case var t when t == typeof(JsonNode):
                    ShowNodes((IEnumerable<JsonNode>)data);
                    break;
                case var t when t == typeof(byte[]):
                    ShowBytes((IEnumerable<byte[]>)data);
                    break;
                case var t when t == typeof(byte):
                    ShowByte((byte[])data);
                    break;
                default:
                    var type = data.GetType()
                        .GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .GetGenericArguments()
                        .FirstOrDefault();

                    if (type == null || type.IsArray || type.IsEnum || type.IsPrimitive || type == typeof(string))
                    {
                        ShowList(data);
                    }
                    else
                    {
                        ShowProperties(data, type);
                    }
                    break;
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;

            dataGridView1.ResumeLayout();
        }

        private void GridViewer_Load(object sender, EventArgs e)
        {

        }

        private void ShowDictionary(IEnumerable list)
        {
            var data = new List<Dictionary<string, string>>();
            foreach(IDictionary entry in list)
            {
                var dict = new Dictionary<string, string>();
                foreach (DictionaryEntry item in entry)
                {
                    var key = ToLine(item.Key);
                    var value = ToLine(item.Value);
                    dict.Add(key, value);
                };
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
                    dataGridView1.Rows[row].Cells[key].Value = data[i].Values;
                }
                dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
            }
        }

        private static string ToLine(object obj)
        {
            return obj switch
            {
                byte[] b => "0x" + BitConverter.ToString(b).Replace("-", ""),
                //bool[] b => "[ " + string.Join("", b.Select(x => x ? "t" : "f")) + " ]",
                string s => s,
                _ => ToString(obj)
            };
        }

        private static IEnumerable<DictionaryEntry> CastDE(IDictionary dict)
        {
            foreach (DictionaryEntry item in dict) yield return item;
        }

        public static string ToString(object obj)
        {
            return obj switch
            {
                string s => s,
                IDictionary d => "{ " + string.Join(", ", CastDE(d).Select(x => ToLine(x.Key) + ": " + ToLine(x.Value))) + " }",
                IEnumerable i => "[ " + string.Join(", ", i.Cast<object>().Select(ToLine)) + " ]",
                _ => obj.ToString()
            };
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

            var i = 0;
            foreach (var item in list)
            {
                var row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    var text = ToLine(value);
                    dataGridView1.Rows[row].Cells[prop.Name].Value = text;
                }
                foreach (var field in fields)
                {
                    var value = field.GetValue(item);
                    var text = ToLine(value);
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
                var text = ToLine(item);
                var row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].HeaderCell.Value = $"{i}";
                dataGridView1.Rows[row].Cells[0].Value = text;
                i++;
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
                    dataGridView1.Rows[row].Cells[kv.Key].Value = ToLine(kv.Value);
                }
                i++;
            }
        }

    }
}
