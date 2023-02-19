namespace PBT.DowsingMachine.Data;

public interface IObjectArchive<T> : IArchive
{
    public T Data { get; }
}
