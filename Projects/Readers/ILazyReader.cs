namespace PBT.DowsingMachine.Projects;

public interface ILazyReader : IDisposable
{
    public void Open(string path);
    public void Open(byte[] data);
}

