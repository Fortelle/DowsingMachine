using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;

namespace PBT.DowsingMachine.Projects;

public class FilesReader<T> : DataReaderBase<IEnumerable<T>>, IDataReader<string[], IEnumerable<T>>, IDataReader<PathInfo[], IEnumerable<T>>
{

    public IEnumerable<T> Read(string[] filenames)
    {
        return filenames.Select(FileUtil.Open<T>);
    }

    public IEnumerable<T> Read(PathInfo[] files)
    {
        return files.Select(x => x.FullPath).Select(FileUtil.Open<T>);
    }

}
