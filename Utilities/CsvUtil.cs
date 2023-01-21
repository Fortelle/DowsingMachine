using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace PBT.DowsingMachine.Utilities;

public static class CsvUtil
{
    public static void Serialize(string path, object[][] obj)
    {
        var csv = Serialize(obj);

        var folder = Path.GetDirectoryName(path);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        File.WriteAllText(path, csv, Encoding.Unicode);
    }

    public static string Serialize(object[][] obj)
    {
        var sw = new StringWriter();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
        };
        var cw = new CsvWriter(sw, config);

        foreach (var row in obj)
        {
            foreach (var cell in row)
            {
                cw.WriteField(cell);
            }
            cw.NextRecord();
        }
        return sw.ToString();
    }
}
