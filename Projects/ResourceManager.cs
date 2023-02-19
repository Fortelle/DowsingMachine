namespace PBT.DowsingMachine.Projects;

public class ResourceManager
{
    public List<DataResource> Resources { get; } = new();

    public DataResource Get(string key)
    {
        var res = Resources.FirstOrDefault(x => x.Key == key);
        return res;
    }

    public DataResource Add(DataResource resource)
    {
        if (resource.Enable)
        {
            Resources.Add(resource);
            return resource;
        }
        else
        {
            return null;
        }
    }

    public void AddRange(params DataResource[] resources)
    {
        Resources.AddRange(resources);
    }

}
