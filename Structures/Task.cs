using PBT.DowsingMachine.UI;
using System.Collections;

namespace PBT.DowsingMachine.Structures;

public class Task<T> : ITask
{
    public string Message { get; init; }

    public int? Size { get; set; }
    public int Degree { get; set; } // -1 for max
    public IEnumerable<T> Queue { get; set; }
    public Action<T>? Action { get; set; }

    IEnumerable ITask.Queue => Queue;
    Delegate ITask.Action => Action;

    public Task()
    {
    }

    public Task(string name) : this()
    {
        Message = name;
    }

    public void Test(int count = 1)
    {
        var i = 0;
        foreach (var q in Queue)
        {
            i++;
            if (i == count) break;
        }
    }

    public void Run()
    {
        var form = new ProgressForm(this);
        form.ShowDialog();
    }
}
