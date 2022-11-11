using System.Collections;

namespace PBT.DowsingMachine.Structures;

public interface ITask
{
    public void Run();
    public int? Size { get; set; }
    public int Degree { get; set; }
    public IEnumerable Queue { get; }
    public Delegate Action { get; }
}
