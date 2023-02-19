namespace PBT.DowsingMachine.Projects;

public interface IDataReaderBase
{
    public object DoRead(object input);
    public List<Delegate> Parsers { get; }
    public Type GenericType { get; }
    public Delegate Debugger { get; set; }
}
