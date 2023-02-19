namespace PBT.DowsingMachine.Projects;

public class GetDataOptions
{
    public string CacheKey { get; set; }
    public int Step { get; set; } = -1;
    public object[] ReferenceArguments { get; set; }
    public bool UseCache { get; set; } = true;
}
