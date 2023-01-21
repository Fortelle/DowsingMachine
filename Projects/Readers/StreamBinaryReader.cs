using PBT.DowsingMachine.Projects;

namespace PBT.DowsingMachine.Projects;

public class StreamBinaryReader : DataReader<StreamBinaryReader.Cache, BinaryReader>
{
    public int Offset;

    public StreamBinaryReader(string path) : base(path)
    {
    }

    public StreamBinaryReader(string path, int offset) : this(path)
    {
        Offset = offset;
    }

    protected override Cache Open()
    {
        var path = Project.As<IFolderProject>().GetPath(RelatedPath);
        var ms = File.OpenRead(path);
        var br = new BinaryReader(ms);
        return new Cache(ms, br);
    }

    protected override BinaryReader Read(Cache cache)
    {
        var br = cache.Reader;
        if (Offset > 0)
        {
            br.BaseStream.Seek(Offset, SeekOrigin.Begin);
        }
        return br;
    }

    //protected override void Release((Stream, BinaryReader) cache)
    //{
    //    cache.Item1.Dispose();
    //    cache.Item2.Dispose();
    //}

    public class Cache : IDisposable
    {
        public Stream Stream;
        public BinaryReader Reader;

        public Cache(Stream stream, BinaryReader reader)
        {
            Stream = stream;
            Reader = reader;
        }

        public void Dispose()
        {
            Stream?.Dispose();
            Stream?.Dispose();
        }
    }
}
