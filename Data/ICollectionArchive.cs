namespace PBT.DowsingMachine.Data;

public interface ICollectionArchive<T> : IArchive
{
    public int Count { get; }
    public T this[int index] { get; }
    public IEnumerable<Entry<T>> AsEnumerable();
    public IEnumerable<T> Values => AsEnumerable().Select(x => x.Data);
}

public interface ICollectionArchive<TKey, TValue> : IArchive
{
    public int Count { get; }
    public TValue this[int index] { get; }
    public TValue this[TKey key] { get; }

    public IEnumerable<Entry<TValue>> AsEnumerable();
    public TKey[] Keys { get; }
    public virtual IEnumerable<TValue> Values => AsEnumerable().Select(x => x.Data);
    
    public void Extract(string folder)
    {
        var dsc = Path.DirectorySeparatorChar;
        Directory.CreateDirectory(folder);

        foreach (var entry in AsEnumerable())
        {
            var path = entry.Name ?? $"{entry.Index}";
            if (entry.Directories?.Length > 0)
            {
                path = string.Join(dsc, entry.Directories) + dsc + path;
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
