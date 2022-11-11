namespace PBT.DowsingMachine.Projects;

public class DataInfo : IDataReader
{
    public string Name { get; set; }
    public string RelatedPath { get; set; }
    public virtual DataProject Project { get; set; }

    public DataInfo(string path)
    {
        RelatedPath = path;
    }

    object IDataReader.Open() => this;
    object IDataReader.Read(object cache) => cache;

    public string Path => Project.GetPath(RelatedPath);
}
