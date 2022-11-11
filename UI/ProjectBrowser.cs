using PBT.DowsingMachine.Projects;
using PBT.DowsingMachine.Structures;
using System.Collections;
using System.Text;

namespace PBT.DowsingMachine.UI;

public partial class ProjectBrowser : Form
{
    public DataProject SelectedProject { get; set; }
    public Func<object, bool> ExtendedPreview { get; set; }

    public T RomAs<T>() where T : DataProject => (T)lvwProjects.Tag;

    public ProjectBrowser()
    {
        InitializeComponent();

    }

    private void ProjectBrowser_Load(object sender, EventArgs e)
    {

    }

    public void LoadProject(DataProject project)
    {
        splitContainer1.Panel1Collapsed = true;
        lvwProjects.Items.Clear();

        SelectProject(project);
    }

    public void LoadProjects(DataProject[] projects)
    {
        lvwProjects.BeginUpdate();
        lvwProjects.Items.Clear();
        foreach (var project in projects)
        {
            var row = lvwProjects.Items.Add(project.Name);
            row.Name = project.Key;
            row.Tag = project;
            row.SubItems.Add(project is ExtendableProject ep ? ep.Version.ToString() : "");
        }
        lvwProjects.EndUpdate();
    }

    public void SelectProject(DataProject project, bool updateList = false)
    {
        SelectedProject = project;
        this.Text = String.Format("{0} - ProjectBrowser", project.Name);

        if (updateList)
        {
            var rows = lvwProjects.Items.Find(project.Key, false);
            if (rows.Length > 0)
            {
                lvwProjects.EnsureVisible(rows[0].Index);
            }
        }

        foreach (ListViewItem row in lvwProjects.Items)
        {
            row.Font = new Font(row.Font, (DataProject)row.Tag == project ? FontStyle.Bold : FontStyle.Regular);
        }

        lstData.Items.Clear();
        foreach (var r in project.References)
        {
            lstData.Items.Add(r.Name);
        }

        lstDump.Items.Clear();
        foreach (var action in project.GetMethods<DumpAttribute>())
        {
            lstDump.Items.Add(action.Name);
        }

        lstExtract.Items.Clear();
        foreach (var action in project.GetMethods<ExtractionAttribute>())
        {
            lstExtract.Items.Add(action.Name);
        }

        lstTest.Items.Clear();
        foreach (var action in project.GetMethods<TestAttribute>())
        {
            lstTest.Items.Add(action.Name);
        }
    }

    private void Preview(object? data)
    {
        if(data is null) {
            return;
        }

        if (ExtendedPreview?.Invoke(data) == true)
        {
            return;
        }

        switch (data)
        {
            case string or StringBuilder:
                {
                    TextViewer.Show(data);
                }
                break;
            case byte[] bytes:
                {
                    TextViewer.Show(bytes);
                }
                break;
            case byte[][] bytes:
                {
                    TextViewer.Show(bytes);
                }
                break;
            case IEnumerable ie:
                {
                    var frm = new GridViewer(ie);
                    if (SelectedProject is IPreviewString ps)
                    {
                        frm.GetString = ps.GetPreviewString;
                    }
                    frm.Show();
                }
                break;
            case Bitmap bmp:
                {
                    var frm = new ImageViewer(bmp);
                    frm.Show();
                }
                break;
            default:
                {
                    var text = Data.JsonUtil.Serialize(data);
                    var frm = new UI.TextViewer(text);
                    frm.Show();
                }
                break;
        }

    }

    private Task DoWork(Action action)
    {
        toolStripProgressBar1.Visible = true;
        var task = new Task(action);
        task.ContinueWith(t => {
            toolStripProgressBar1.Visible = false;
        });
        task.Start();
        return task;
    }

    private void lvwProjects_ItemActivate(object sender, EventArgs e)
    {
        if (lvwProjects.SelectedItems.Count == 0) return;
        var project = (DataProject)lvwProjects.SelectedItems[0].Tag;
        SelectProject(project, false);
    }

    private void lstData_DoubleClick(object sender, EventArgs e)
    {
        var refName = lstData.SelectedItem as string;
        if (string.IsNullOrEmpty(refName)) return;
        SelectedProject.BeginWork();
        var data = SelectedProject.GetData(refName!);
        SelectedProject.EndWork();
        Preview(data);
    }

    private void lstData_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && lstData.SelectedItems.Count > 0)
        {
            var refName = lstData.SelectedItem as string;
            var reference = SelectedProject.GetReference(refName!);
            var count = 1 + reference.Parsers.Length;

            foreach (var m in contextMenuStrip1.Items.OfType<ToolStripItem>().ToArray())
            {
                m.Dispose();
            }
            contextMenuStrip1.Items.Clear();
            for (var i = 0; i < count; i++)
            {
                var tsi = new ToolStripMenuItem();
                tsi.Text = i == 0 ? "Raw" : $"Step {i}";
                var x = i;
                tsi.Click += (_, _) => {
                    SelectedProject.BeginWork();
                    var data = reference.Reader.Open();
                    for (var j = 1; j <= x; j++)
                    {
                        data = reference.Parsers[j - 1].DynamicInvoke(data);
                    }
                    SelectedProject.EndWork();

                    Preview(data);
                };
                contextMenuStrip1.Items.Add(tsi);
            }
            contextMenuStrip1.Show(lstData, e.Location);
        }
    }

    private void lstData_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void lvwProjects_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void toolStripMenuItem2_Click(object sender, EventArgs e)
    {
    }

    private void lstTest_DoubleClick(object sender, EventArgs e)
    {
        var action = lstTest.SelectedItem as string;
        if (string.IsNullOrEmpty(action)) return;

        SelectedProject.BeginWork();
        var data = SelectedProject.Test(action!);
        SelectedProject.EndWork();

        Preview(data);
    }

    private void lstDump_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void lstTest_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void lstDump_DoubleClick(object sender, EventArgs e)
    {
        var action = lstDump.SelectedItem as string;
        if (string.IsNullOrEmpty(action)) return;

        SelectedProject.BeginWork();
        var query = SelectedProject.Extract(action);
        var wp = new WorkProcesser();
        wp.Set(query);
        wp.ShowDialog(true);
        SelectedProject.EndWork();
    }

    private void lvwProjects_MouseDown(object sender, MouseEventArgs e)
    {
        var hit = lvwProjects.HitTest(e.X, e.Y);
        if (hit.Item == null) return;

        var project = (DataProject)hit.Item.Tag;

        if (e.Button == MouseButtons.Right)
        {
            tsmiProjectVersions.DropDownItems.Clear();
            switch (project)
            {
                case ParallelProject pp:
                    {
                        foreach(var v in pp.Variations)
                        {
                            var tsmi = tsmiProjectVersions.DropDownItems.Add(v);
                            tsmi.Tag = v;
                            tsmi.Click += (_, _) =>
                            {
                                pp.Switch(v);
                                hit.Item.SubItems[1].Text = v;
                                SelectProject(pp);
                            };
                        }
                    }
                    break;
            }
            cmsProject.Show(lvwProjects, e.Location);
        }
    }

    private void lstExtract_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void lstExtract_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        var action = lstExtract.SelectedItem as string;
        if (string.IsNullOrEmpty(action)) return;

        SelectedProject.BeginWork();
        var query = SelectedProject.Extract(action);
        var wp = new WorkProcesser();
        wp.Set(query);
        wp.ShowDialog(true);
        SelectedProject.EndWork();
    }

    private void devToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }
}
