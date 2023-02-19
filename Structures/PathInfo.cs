namespace PBT.DowsingMachine.Structures;

public struct PathInfo
{
    public string FullPath;
    public string FolderPart;
    public string RelativePart;
    public int SortIndex;

    public PathInfo(string path)
    {
        FullPath = path;
        FolderPart = "";
        RelativePart = "";
        SortIndex = 0;
    }

    public PathInfo(string path, string folder)
    {
        FullPath = path;
        FolderPart = folder;
        RelativePart = path.Replace(folder, "").TrimStart('/', '\\');
        SortIndex = 0;
    }

    public string Filename => Path.GetFileName(RelativePart);
    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(RelativePart);
    public string DirectoryName => Path.GetDirectoryName(FullPath);

}
