namespace PBT.DowsingMachine.Projects;

public class ArchiveReader<T> : DataReader<T> where T : IArchive, new()
{
    public ArchiveReader(string path) : base(path)
    {
    }

    protected override T Open()
    {
        var path = Project.GetPath(RelatedPath);
        var file = new T();
        file.Open(path);
        return file;
    }
}
