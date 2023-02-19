using PBT.DowsingMachine.Structures;

namespace PBT.DowsingMachine.Projects;

public class FilesRef : DataRef<PathInfo[]>
{
    public override string Description => Relpath;

    public string Relpath { get; set; }

    public FilesRef(string relpath)
    {
        Relpath = relpath;
    }

}
