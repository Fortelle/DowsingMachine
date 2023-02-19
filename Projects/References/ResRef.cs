namespace PBT.DowsingMachine.Projects;

public class ResRef : DataRef<string>
{
    public string ResKey { get; set; }

    public override string Description => ResKey;

    public ResRef(string reskey)
    {
        ResKey = reskey;
    }
}
