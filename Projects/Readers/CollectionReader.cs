namespace PBT.DowsingMachine.Projects;

public class CollectionReader<T> : DataReader<T> where T : IArchive, new()
{
    public CollectionReader(string path) : base(path)
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
