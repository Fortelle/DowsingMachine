using PBT.DowsingMachine.Structures;

namespace PBT.DowsingMachine.UI;

public partial class ProgressForm : Form
{
    public ITask Task { get; set; }
    public int Count { get; set; }

    public ProgressForm()
    {
        InitializeComponent();

        progressBar1.Value = 0;
        progressBar1.Maximum = 1;
    }


    public ProgressForm(ITask task) : this()
    {
        Task = task;
    }

    private void ProgressForm_Load(object sender, EventArgs e)
    {

    }
}
