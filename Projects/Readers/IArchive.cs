namespace PBT.DowsingMachine.Projects;

public interface IArchive
{
    public void Open(string path);
    public void Open(byte[] data);
    public void Open(Stream stream);
}
