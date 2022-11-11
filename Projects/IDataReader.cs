namespace PBT.DowsingMachine.Projects;

public interface IDataReader
{
    public string Name { get; set; }
    public string RelatedPath { get; set; }
    public DataProject Project { get; set; }

    public object Open();
    object Read(object _);
    //public void Release(object cache);
}
