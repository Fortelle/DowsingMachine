namespace PBT.DowsingMachine.Projects
{
    public class IReadableCollection<T>
    {
        public T Read(string key)
        {
            throw new NotImplementedException();
        }

        public T Read(int index)
        {
            throw new NotImplementedException();
        }
    }

    public interface IFileReader
    {
        public void Load(string path);
        public void Load(byte[] data);
    }

    public interface IFileReader<T>
    {
        public T Get();
    }

    public interface IExtractable
    {
        public void Extract(string path);
    }

}
