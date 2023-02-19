using PBT.DowsingMachine.Structures;
using System.Text.RegularExpressions;

namespace PBT.DowsingMachine.Utilities;

public static class DirectoryUtil
{
    public static PathInfo[] GetFiles(string path, string searchPattern)
    {
        var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
        return files.Select(x => new PathInfo(x, path)).ToArray();
    }

    public static PathInfo[] GetFiles(string path)
    {
        var folder = path;
        var pattern = "*";

        if (path.Contains("*"))
        {
            var match = Regex.Match(path, @"[\\\/][^\\\/]*?\*");
            var index = match.Index;
            folder = new string(path.Substring(0, index));
            pattern = new string(path.Substring(index + 1));
        }

        var files = Directory.GetFiles(folder, pattern, SearchOption.AllDirectories);
        return files.Select(x => new PathInfo(x, folder)).ToArray();
    }

}
