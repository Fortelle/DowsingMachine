namespace PBT.DowsingMachine.Projects;

public class ProjectCache : Dictionary<string, object>
{
    public void Release()
    {
        foreach (var (key, cache) in this)
        {
            if (cache is IDisposable dis) { 
                dis.Dispose(); 
            }
        }
        Clear();
    }
}
