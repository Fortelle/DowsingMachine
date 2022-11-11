using PBT.DowsingMachine.Structures;

namespace PBT.DowsingMachine.Utilities;

public static class DirectoryUtil
{
    public static FileEntry[] GetFiles(string path, string searchPattern)
    {
        var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
        var entries = files.Select(x => new FileEntry()
        {
            Path = x,
            //Filename = Path.GetFileName(x),
            RelativePath = Path.GetRelativePath(path, x),
        });
        return entries.ToArray();
    }

}
