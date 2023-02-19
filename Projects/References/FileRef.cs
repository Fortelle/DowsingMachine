namespace PBT.DowsingMachine.Projects;

public class FileRef : DataRef<string>
{
    public string Relpath { get; set; }

    public override string Description => Relpath;

    public FileRef(string relpath)
    {
        Relpath = relpath;
    }
}
