using PBT.DowsingMachine.Data;

namespace PBT.DowsingMachine.Utilities;

public static class FileUtil
{
    public static T Open<T>(string path)
    {
        var type = typeof(T);
        object ret;
        if (type == typeof(string))
        {
            ret = File.ReadAllText(path);
        }
        else if (type == typeof(byte[]))
        {
            ret = File.ReadAllBytes(path);
        }
        else if (type == typeof(Stream))
        {
            ret = File.OpenRead(path);
        }
        else if (type == typeof(BinaryReader))
        {
            var fs = File.OpenRead(path);
            ret = new BinaryReader(fs);
        }
        else if (type == typeof(byte[]))
        {
            ret = File.ReadAllBytes(path);
        }
        else if (typeof(IArchive).IsAssignableFrom(type))
        {
            ret = Activator.CreateInstance(type);
            ((IArchive)ret).Open(path);
        }
        else
        {
            throw new NotSupportedException();
        }

        return (T)ret;
    }

}
