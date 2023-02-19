namespace PBT.DowsingMachine.Data;

public interface IArchive
{
    public void Open(string path);
    public void Open(byte[] data);
}
