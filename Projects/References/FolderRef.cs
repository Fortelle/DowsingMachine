namespace PBT.DowsingMachine.Projects;

public class FolderRef : DataRef<string>
{
    public string Relpath { get; set; }

    public override string Description => Relpath;

    public FolderRef(string relpath)
    {
        Relpath = relpath;
    }
}
