using PBT.DowsingMachine.Projects;
using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

    public void LoadProjects()
    {
        lvwProjects.BeginUpdate();
        lvwProjects.Items.Clear();
        foreach (var project in DowsingMachineApp.Projects)
        {
            var row = lvwProjects.Items.Add(project.Name);
            row.Name = project.Id;
            row.Tag = project;
            row.SubItems.Add(project is ExtendableProject ep ? $"{ep.Version}" : "");
        }
        lvwProjects.EndUpdate();
    }

    public void SelectProject(DataProject project, bool updateList = false)
    {
        SelectedProject = project;
        lstData.Items.Clear();
        lstDump.Items.Clear();
        lstExtract.Items.Clear();
        lstTest.Items.Clear();

        if(project is null)
        {
            this.Text = "ProjectBrowser";
            return;
        }

        this.Text = string.Format("{0} - ProjectBrowser", project.Name);

        if (updateList)
        {
            var rows = lvwProjects.Items.Find(project.Id, false);
            if (rows.Length > 0)
            {
                lvwProjects.EnsureVisible(rows[0].Index);
            }
        }

        foreach (ListViewItem row in lvwProjects.Items)
        {
            row.Font = new Font(row.Font, (DataProject)row.Tag == project ? FontStyle.Bold : FontStyle.Regular);
        }

        foreach (var r in project.References)
        {
            lstData.Items.Add(r.Name);
        }

        foreach (var action in project.GetMethods<DumpAttribute>())
        {
            lstDump.Items.Add(action.Name);
        }

        foreach (var action in project.GetMethods<ExtractionAttribute>())
        {
            lstExtract.Items.Add(action.Name);
        }

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
                    var text = JsonUtil.Serialize(data);
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
        if(!SelectedProject.CheckValidity(out var err))
        {
            MessageBox.Show(err, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

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

    private void lstTest_DoubleClick(object sender, EventArgs e)
    {
        var action = lstTest.SelectedItem as string;
        if (string.IsNullOrEmpty(action)) return;

        SelectedProject.BeginWork();
        var data = SelectedProject.Test(action!);
        SelectedProject.EndWork();

        Preview(data);
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
            if(cmsProject.Tag != project)
            {
                cmsProject.Items
                    .OfType<ToolStripMenuItem>()
                    .Where(x => x.Tag is string s && s.StartsWith("action:"))
                    .ToList()
                    .ForEach(x => cmsProject.Items.Remove(x))
                    ;

                foreach (var method in project.GetMethods<ActionAttribute>())
                {
                    var attr = method.GetCustomAttribute<ActionAttribute>();
                    var tsmi = cmsProject.Items.Add(attr.Name);
                    tsmi.Tag = $"action:{method.Name}";
                    tsmi.Click += (_, _) =>
                    {
                        method.Invoke(project, null);
                    };
                }

                tsmiProjectVersions.DropDownItems.Clear();
                tsmiProjectVersions.Visible = false;
                switch (project)
                {
                    case ParallelProject pp:
                        {
                            foreach (var v in pp.Variations)
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
                            tsmiProjectVersions.Visible = true;
                        }
                        break;
                }
                cmsProject.Tag = project;
            }

            cmsProject.Show(lvwProjects, e.Location);
        }
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

    private void tsmiAddProject_Click(object sender, EventArgs e)
    {
        using var frm = new AddProjectForm();
        if (frm.ShowDialog() != DialogResult.OK) return;
        if (frm.Project is null) return;

        var project = frm.Project;

        DowsingMachineApp.AddProject(project);
        LoadProjects();
        SelectProject(project);
    }

    private void tsmiProjectOptions_Click(object sender, EventArgs e)
    {
        var project = (DataProject)cmsProject.Tag;

        using var frm = new AddProjectForm(project);
        frm.ShowDialog();

        LoadProjects();
        SelectProject(project);
    }

    private void tsmiProjectRemove_Click(object sender, EventArgs e)
    {
        var project = (DataProject)cmsProject.Tag;

        if(MessageBox.Show($"Are you sure you want to remove project \"{project.Name}\" from the list?", null, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        DowsingMachineApp.RemoveProject(project);
        LoadProjects();
        SelectProject(null);
    }

    private void tsmiHelpAppData_Click(object sender, EventArgs e)
    {
        if (Directory.Exists(DowsingMachineApp.RoamingPath))
        {
            Process.Start("explorer.exe", DowsingMachineApp.RoamingPath);
        }
    }

    private void tsmiHelpGitHub_Click(object sender, EventArgs e)
    {
        using var process = new Process()
        {
            StartInfo =
            {
                FileName = "git",
                Arguments = "config --get remote.origin.url",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }
        };

        process.Start();
        var url = process.StandardOutput.ReadToEnd().Trim();

        if (process.ExitCode != 0)
        {
            return;
        }

        if (Regex.IsMatch(url, @"^https://.+\.git$"))
        {
            url = Regex.Replace(url, @"\.git$", "");

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true,
            });
        }
    }

}
