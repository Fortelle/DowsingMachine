using PBT.DowsingMachine.Projects;
using System.Collections.Generic;
using System.ComponentModel;

namespace PBT.DowsingMachine.UI;

public partial class AddProjectForm : Form
{
    public DataProject Project { get; set; }

    private bool AddMode { get; set; }

    private bool HasChanged { get; set; }

    public AddProjectForm()
    {
        InitializeComponent();

        btnOK.Enabled = false;
        cmbProject.Items.AddRange(DowsingMachineApp.SupportedProjectTypes);
        propertyGrid1.BrowsableAttributes = new AttributeCollection(new OptionAttribute());

        AddMode = true;
        HasChanged = true;
    }

    public AddProjectForm(DataProject project) : this()
    {
        AddMode = false;
        HasChanged = false;

        cmbProject.Enabled = false;
        cmbProject.SelectedItem = project.GetType();

        Project = project;
        propertyGrid1.SelectedObject = project;

        btnOK.Enabled = true;
        btnCancel.Visible = false;
    }

    private void AddProjectForm_Load(object sender, EventArgs e)
    {

    }

    private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!AddMode) return;

        var projectType = cmbProject.SelectedItem as Type;
        Project = Activator.CreateInstance(projectType) as DataProject;
        Project.Name = projectType.Name;

        propertyGrid1.SelectedObject = Project;
        btnOK.Enabled = true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (!Project.CheckValidity(AddMode, out var error))
        {
            DialogResult = DialogResult.None;
            lblError.Text = error;
            return;
        }

        if (!HasChanged)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }
    }

    private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
        HasChanged = true;
    }
}
