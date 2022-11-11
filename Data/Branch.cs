namespace PBT.DowsingMachine.Data;

public class Branch<T>
{
    public string Name { get; set; }
    public Branch<T>[] Folders { get; set; }
    public Entry<T>[] Files { get; set; }
}
