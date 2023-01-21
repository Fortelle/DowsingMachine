using PBT.DowsingMachine.FileFormats;

namespace PBT.DowsingMachine.Projects;

public class MatrixReader : DataReader<byte[][]>
{
    public int Offset { get; init; }
    public int ItemSize { get; init; }
    public int ItemCount { get; init; }

    public MatrixReader(string path, int length, int size = -1, int offset = 0) : base(path)
    {
        ItemSize = length;
        ItemCount = size;
        Offset = offset;
    }

    protected override byte[][] Open()
    {
        var path = Project.As<IFolderProject>().GetPath(RelatedPath);
        var bytes = File.ReadAllBytes(path);
        var pack = new MatrixPack(bytes, ItemSize, ItemCount, Offset);
        return pack.Entries.ToArray();
    }

}
