using System.Xml.Serialization;

namespace PBT.DowsingMachine.Data;

public static class XmlUtil
{
    public static void XmlSerialize<T>(string path, T obj, params Type[] types)
    {
        var folder = Path.GetDirectoryName(path);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var tempPath = path + ".temp";
        var xs = new XmlSerializer(obj.GetType(), types);
        using (var sw = new StreamWriter(tempPath))
        {
            xs.Serialize(sw, obj);
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.Move(tempPath, path);
    }

    public static void XmlDeserialize<T>(string path, out T obj, params Type[] types)
    {
        var xs = new XmlSerializer(typeof(T), types);
        using var sr = new StreamReader(path);
        obj = (T)xs.Deserialize(sr);
    }
}
