using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;

namespace PBT.DowsingMachine.Projects;

public class FileReader<T> : DataReaderBase<T>, IDataReader<string, T>, IDataReader<PathInfo, T>
{
    public virtual T Read(string filename)
    {
        return FileUtil.Open<T>(filename);
    }

    public virtual T Read(PathInfo file)
    {
        return Read(file.FullPath);
    }
}

public class FileReader : FileReader<BinaryReader>
{
    public int Position { get; set; }

    public FileReader()
    {
    }

    public FileReader(int position)
    {
        Position = position;
    }

    public override BinaryReader Read(string filename)
    {
        var br = base.Read(filename);
        if(Position > 0)
        {
            br.BaseStream.Seek(Position, SeekOrigin.Begin);
        }
        return br;
    }

}
