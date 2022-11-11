namespace PBT.DowsingMachine.Structures;

public struct FileEntry
{
    public string Path;
    public string RelativePath;

    public string Filename => System.IO.Path.GetFileName(Path);
    public string FileNameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(Path);
    public string DirectoryName => System.IO.Path.GetDirectoryName(Path);
    public string RelativeDirectoryName => System.IO.Path.GetDirectoryName(RelativePath);
}
