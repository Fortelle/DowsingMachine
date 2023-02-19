using PBT.DowsingMachine.Projects;
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
        lvwProjects.Groups.Clear();
        lvwProjects.Items.Clear();
        var groups = DowsingMachineApp.Projects.GroupBy(x => x.GetType());
        foreach (var group in groups)
        {
            var lvg = new ListViewGroup(group.Key.GetGenericName());
            lvwProjects.Groups.Add(lvg);
            foreach (var project in group)
            {
                var row = lvwProjects.Items.Add(project.Name);
                row.Name = project.Id;
                row.Tag = project;
                row.SubItems.Add(project.Description);
                lvg.Items.Add(row);
            }
        }
        lvwProjects.EndUpdate();
    }

    public void SelectProject(DataProject project, bool updateList = false)
    {
        SelectedProject = project;
        lvwReferences.Items.Clear();
        lvwExports.Groups.Clear();
        lvwExports.Items.Clear();
        lvwTests.Groups.Clear();
        lvwTests.Items.Clear();
        pnlActions.Controls.Clear();

        if (project is null)
        {
            this.Text = "ProjectBrowser";
        }
        else
        {
            project.Active();

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

            foreach (var reference in project.Resources.Resources)
            {
                if (!reference.Browsable) continue;
                var row = lvwReferences.Items.Add(reference.Key);
                row.Name = reference.Key;
                row.SubItems.Add(reference.Reference?.Description ?? "");
                row.SubItems.Add(reference.OutputType?.GetGenericName() ?? "");
            }

            var dataGroups = project.DataMethods.GroupBy(m => m.DeclaringType).Reverse();
            foreach (var group in dataGroups)
            {
                var lvg = new ListViewGroup(group.Key.GetGenericName());
                lvwExports.Groups.Add(lvg);
                foreach (var method in group)
                {
                    var attr = method.GetCustomAttribute<DataAttribute>()!;
                    var row = lvwExports.Items.Add(CamelToSentence(method.Name));
                    row.Name = method.Name;
                    row.Tag = method.Name;
                    row.SubItems.Add(method.ReturnType.GetGenericName() ?? "");
                    row.SubItems.Add(attr.OutputPath ?? "");
                    lvg.Items.Add(row);
                }
            }

            var actionGroups = project.ActionMethods.GroupBy(m => m.DeclaringType).Reverse();
            var controls = new List<Control>();
            foreach (var group in actionGroups)
            {
                controls.Add(new Label()
                {
                    Text = "",
                    Dock = DockStyle.Top,
                    Height = 8,
                });
                controls.Add(new Label()
                {
                    Text = group.Key.GetGenericName(),
                    Dock = DockStyle.Top,
                });
                foreach (var method in group)
                {
                    var btn = new Button()
                    {
                        Text = CamelToSentence(method.Name),
                        Dock = DockStyle.Top,
                        Height = 32,
                    };
                    btn.Click += (_, _) =>
                    {
                        SelectedProject.BeginWork();
                        var data = method.Invoke(project, Array.Empty<object>());
                        SelectedProject.EndWork();
                        Preview(data);
                    };
                    controls.Add(btn);
                }
            }
            controls.Reverse();
            pnlActions.Controls.AddRange(controls.ToArray());

            var testGroups = project.TestMethods.GroupBy(m => m.DeclaringType).Reverse();
            foreach (var group in testGroups)
            {
                var lvg = new ListViewGroup(group.Key.GetGenericName());
                lvwTests.Groups.Add(lvg);
                foreach (var method in group)
                {
                    var row = lvwTests.Items.Add(method.Name);
                    row.Name = method.Name;
                    row.Tag = method;
                    row.SubItems.Add(method.ReturnType.GetGenericName() ?? "");
                    lvg.Items.Add(row);
                }
            }

        }
    }

    private static string CamelToSentence(string word)
    {
        return Regex.Replace(word, "[a-z][A-Z]", m => $"{m.Value[0]} {(((m.Index + 3 < word.Length) && char.IsLower(word[m.Index + 2])) ? char.ToLower(m.Value[1]) : m.Value[1])}");
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

    private void lvwReferences_ItemActivate(object sender, EventArgs e)
    {
        if (lvwReferences.SelectedItems.Count == 0) return;

        tsmiReferencesRead.PerformClick();
    }

    private void lvwExports_ItemActivate(object sender, EventArgs e)
    {
        if (lvwExports.SelectedItems.Count == 0) return;

        var methodName = lvwExports.SelectedItems[0].Name;
        var method = SelectedProject.DataMethods.First(x => x.Name == methodName);

        object data = null;
        SelectedProject.BeginWork();
        data = method.Invoke(SelectedProject, Array.Empty<object>());

        SelectedProject.EndWork();

        Preview(data);
    }











    private void lvwProjects_MouseDown(object sender, MouseEventArgs e)
    {
        var hit = lvwProjects.HitTest(e.X, e.Y);
        if (hit.Item == null) return;

        if (e.Button == MouseButtons.Right)
        {
            var project = (DataProject)hit.Item.Tag;

            if (cmsProject.Tag != project)
            {
                cmsProject.Tag = project;
            }

            tsmiProjectVersions.DropDownItems.Clear();
            tsmiProjectVersions.Visible = false;
            cmsProject.Show(lvwProjects, e.Location);
        }
    }

    private void tsmiProjectOptions_Click(object sender, EventArgs e)
    {
        var project = (DataProject)cmsProject.Tag;

        using var frm = new AddProjectForm(project);
        var result = frm.ShowDialog();

        if(result == DialogResult.OK)
        {
            LoadProjects();
            SelectProject(project);
            DowsingMachineApp.SaveProjectConfig();
        }
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

    private void tsmiReferencesRead_Click(object sender, EventArgs e)
    {
        if (lvwReferences.SelectedItems.Count == 0) return;

        var resName = lvwReferences.SelectedItems[0].Name;
        var res = SelectedProject.Resources.Get(resName);
        if (res.Previewable == false) 
        {
            MessageBox.Show("This item does not support previewing.");
            return;
        }
        
        SelectedProject.BeginWork();
        var data = SelectedProject.GetData(resName);
        SelectedProject.EndWork();

        Preview(data);
    }

    private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
    {

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

    private void lvwExports_MouseDown(object sender, MouseEventArgs e)
    {
    }

    private void tsmiExportSelected_Click(object sender, EventArgs e)
    {
        if (SelectedProject is null) return;
        if (lvwExports.SelectedItems.Count == 0) return;

        SelectedProject.BeginWork();

        var methodNames = lvwExports.SelectedItems.Cast<ListViewItem>().Select(x => x.Name).ToArray();
        var methods = SelectedProject.GetType().GetMethods().Where(x => methodNames.Contains(x.Name)).ToArray();
        foreach (var method in methods)
        {
            SelectedProject.Export(method);
        }

        SelectedProject.EndWork();

        SelectedProject.OpenOutputFolder();
    }

    private void tsmiReload_Click(object sender, EventArgs e)
    {
        var project = (DataProject)cmsProject.Tag;

        var newProject = DowsingMachineApp.ReloadProject(project);
        LoadProjects();
        SelectProject(newProject);
    }

    private void lvwTests_ItemActivate(object sender, EventArgs e)
    {
        if (lvwTests.SelectedItems.Count == 0) return;

        var methodName = lvwTests.SelectedItems[0].Name;
        var method = SelectedProject.TestMethods.First(x => x.Name == methodName);

        object data = null;
        SelectedProject.BeginWork();
        data = method.Invoke(SelectedProject, Array.Empty<object>());
        SelectedProject.EndWork();

        Preview(data);
    }

    private void lvwReferences_MouseDown(object sender, MouseEventArgs e)
    {
        var hit = lvwReferences.HitTest(e.X, e.Y);
        if (hit.Item == null) return;

        if (e.Button == MouseButtons.Right)
        {
            var refName = hit.Item.Name;
            var res = SelectedProject.Resources.Get(refName);

            tsmiReferenceSteps.DropDownItems.Clear();
            int index = 0;
            if (res.Reference != null)
            {
                var tsmi = tsmiReferenceSteps.DropDownItems.Add($"Reference: " + res.Reference.GenericType.GetGenericName());
                tsmi.Click += (_, _) =>
                {
                    SelectedProject.BeginWork();
                    var data = SelectedProject.GetData(refName, new()
                    {
                        Step = 0,
                        UseCache = false,
                    });
                    SelectedProject.EndWork();
                    Preview(data);
                };
            }
            if (res.Reader?.Base != null)
            {
                var tsmi = tsmiReferenceSteps.DropDownItems.Add($"Reader: " + res.Reader.Base.GenericType.GetGenericName());
                tsmi.Click += (_, _) =>
                {
                    SelectedProject.BeginWork();
                    var data = SelectedProject.GetData(refName, new()
                    {
                        Step = 1,
                        UseCache = false,
                    });
                    SelectedProject.EndWork();
                    Preview(data);
                };
            }
            if(res.Reader.Base.Parsers?.Count > 0)
            {
                for (var i = 0; i < res.Reader.Base.Parsers.Count; i++)
                {
                    var tsmi = tsmiReferenceSteps.DropDownItems.Add($"Parser{i + 1}: " + res.Reader.Base.Parsers[i].Method.ReturnType.GetGenericName());
                    var j = i + 2;
                    tsmi.Click += (_, _) =>
                    {
                        SelectedProject.BeginWork();
                        var data = SelectedProject.GetData(refName, new()
                        {
                            Step = j,
                            UseCache = false,
                        });
                        SelectedProject.EndWork();
                        Preview(data);
                    };
                }
            }

            tsmiReferencesRead.Enabled = res.Previewable;
            tsmiReferenceSteps.Visible = tsmiReferenceSteps.DropDownItems.Count > 0;
            tsmiReferencesDebug.Visible = res.Reader.Base.Debugger != null;
            cmsReferences.Show(lvwReferences, e.Location);
        }
    }

    private void tsmiReferenceExport_Click(object sender, EventArgs e)
    {

    }

    private void tsmiReferencesDebug_Click(object sender, EventArgs e)
    {
        if (lvwReferences.SelectedItems.Count == 0) return;

        var refName = lvwReferences.SelectedItems[0].Name;
        var res = SelectedProject.Resources.Get(refName);
        if (res.Reader.Base.Debugger == null) return;

        SelectedProject.BeginWork();
        var data = SelectedProject.GetData(refName);
        var debug = res.Reader.Base.Debugger.DynamicInvoke(data);
        SelectedProject.EndWork();

        Preview(debug);
    }
}
