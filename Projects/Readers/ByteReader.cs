using PBT.DowsingMachine.Projects;

namespace PBT.DowsingMachine.Pokemon.Games;

public class ByteReader : DataReader<byte[]>
{
    public ByteReader(string path) : base(path)
    {
    }

    protected override byte[] Open()
    {
        var path = Project.GetPath(RelatedPath);
        var data = File.ReadAllBytes(path);
        return data;
    }
}
