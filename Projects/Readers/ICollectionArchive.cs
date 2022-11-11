using PBT.DowsingMachine.Data;

namespace PBT.DowsingMachine.Projects;

public interface ICollectionArchive<T> : IArchive
{
    public T this[int index] { get; }
    public T this[string name] { get; }
    public IEnumerable<Entry<T>> Entries { get; }

    public T[] GetData()
    {
        return Entries.Select(x => x.Data).ToArray();
    }

    public void Extract(string folder)
    {
        var dsc = Path.DirectorySeparatorChar;
        Directory.CreateDirectory(folder);

        foreach (var entry in Entries)
        {
            var path = entry.Name ?? $"{entry.Index}";
            if(entry.Parents?.Length > 0)
            {
                path = string.Join(dsc, entry.Parents) + dsc + path;
            }
            path = path.TrimStart(dsc);
            var hasSubfolder = path.Contains(dsc);
            path = Path.Combine(folder, path);

            if (hasSubfolder)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            switch (entry.Data)
            {
                case byte[] bytes:
                    File.WriteAllBytes(path, bytes);
                    break;
                case string text:
                    File.WriteAllText(path, text);
                    break;
            }
        }
    }
}
