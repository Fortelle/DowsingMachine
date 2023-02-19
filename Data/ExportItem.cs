namespace PBT.DowsingMachine.Data;

public class ExportItem
{
    public string Relpath { get; set; }
    public object Data { get; set; }
    public ExportItem(string relpath, object data)
    {
        Relpath = relpath;
        Data = data;
    }
}