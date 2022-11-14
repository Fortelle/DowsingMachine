using PBT.DowsingMachine.Data;
using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;
using System.Reflection;

namespace PBT.DowsingMachine.Projects;

public class DataProject
{
    public string Name { get; set; }
    public string Root { get; set; }
    public string OutputPath { get; set; }

    public List<DataReference> References { get; set; }
    public Dictionary<string, object> Caches { get; set; }
    public bool UseCache { get; set; }

    public virtual string Key => Name;

    public event Action<object> OnDataGet;

    public DataProject()
    {
        References = new();
        Caches = new();
    }

    public DataProject(string name, string path) : this()
    {
        Name = name;
        Root = path;
    }

    public DataReference AddReference(DataReference reference)
    {
        if(reference.Reader != null)
        {
            reference.Reader.Name = Name;
            reference.Reader.Project = this;
        }
        References.Add(reference);
        return reference;
    }

    protected DataReference AddReference(string name, IDataReader reader)
    {
        var reference = new DataReference(name, reader);
        return AddReference(reference);
    }

    protected DataReference AddReference<T>(string name, Func<T> parser)
    {
        var reference = new DataReference(name, null, parser);
        return AddReference(reference);
    }

    protected DataReference AddReference<T0, T1, T2>(string name, DataReader<T0, T1> reader,
        Func<T1, T2> parser1)
    {
        var reference = new DataReference(name, reader, parser1);
        return AddReference(reference);
    }

    protected DataReference AddReference<T0, T1, T2, T3>(string name, DataReader<T0, T1> reader,
        Func<T1, T2> parser1,
        Func<T2, T3> parser2)
    {
        var reference = new DataReference(name, reader, parser1, parser2);
        return AddReference(reference);
    }

    protected DataReference AddReference<T0, T1, T2, T3, T4>(string name, DataReader<T0, T1> reader,
        Func<T1, T2> parser1,
        Func<T2, T3> parser2,
        Func<T3, T4> parser3)
    {
        var reference = new DataReference(name, reader, parser1, parser2, parser3);
        return AddReference(reference);
    }

    public DataReference GetReference(string name)
    {
        name = DataReference.ModifyName(name);
        return References.First(x => x.Name == name);
    }

    protected object GetData(DataReference reference, int step)
    {
        object? data;

        if (!UseCache)
        {
            reference.TryOpen(out data);
        }
        else
        {
            if (!Caches.ContainsKey(reference.Name))
            {
                reference.TryOpen(out data);
                Caches.Add(reference.Name, data);
            }
            else
            {
                data = Caches[reference.Name];
            }
        }

        if (step == -1) step = reference.Parsers.Length;
        if (step > 0)
        {
            foreach (var del in reference.Parsers)
            {
                var newData = data is null ? del.DynamicInvoke() : del.DynamicInvoke(data);
                if(data is IDisposable dis) { dis.Dispose(); }
                data = newData;
            }
        }

        OnDataGet?.Invoke(data);

        return data;
    }

    public object GetData(string refName, int step = -1)
    {
        var reference = GetReference(refName);
        return GetData(reference, step);
    }

    public T GetData<T>(string refName, int step)
    {
        return (T)GetData(refName, step);
    }

    public T GetData<T>(string refName)
    {
        return (T)GetData(refName, -1);
    }

    public virtual void BeginWork()
    {
        UseCache = true;
    }

    public virtual void EndWork()
    {
        foreach(var (refName, cache) in Caches)
        {
            //var reference = GetReference(refName);
            if(cache is IDisposable dis) { dis.Dispose(); }
            Caches.Remove(refName);
        }
        UseCache = false;
    }

    public IEnumerable<MethodInfo> GetMethods<T>() where T : Attribute
    {
        var type = typeof(T);
        return GetType()
            .GetMethods()
            .Where(x => x.GetCustomAttributes(type, false).Any())
            ;
    }

    public object Test(string name)
    {
        var method = GetType().GetMethod(name);
        var ta = method.GetCustomAttribute<TestAttribute>();
        return method.Invoke(this, ta.Arguments);
    }

    public IEnumerable<string> Extract(string name)
    {
        var method = GetType().GetMethod(name);
        var result = method.Invoke(this, null);
        return result switch
        {
            string path => new[] { path },
            IEnumerable<string> paths => paths,
            null => new[] { $"{name} is done." },
            _ => throw new NotImplementedException(),
        };
    }

    public virtual string GetPath(string relatedPath)
    {
        relatedPath = relatedPath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(Root, relatedPath);
        return path;
    }

    public virtual FileEntry[] GetFiles(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(Root, relativePath);
        var files = DirectoryUtil.GetFiles(path, "*");
        return files;
    }

    public virtual FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(Root, relativePath);
        var files = DirectoryUtil.GetFiles(path, searchPattern);
        return files;
    }

    public string GetOutputFile(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(OutputPath, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        return path;
    }

    public static T[] MarshalArray<T>(IEnumerable<byte[]> source) where T : new()
    {
        return source.Select(MarshalUtil.Deserialize<T>).ToArray();
    }

    public static T[] MarshalArray<T>(IEnumerable<Entry<byte[]>> source) where T : new()
    {
        return source.Select(x => MarshalUtil.Deserialize<T>(x.Data)).ToArray();
    }

    public static T[] ParseArray<T>(byte[][] source, Func<byte[], T> predicate)
    {
        return source.Select(predicate).ToArray();
    }

    public static T[] ParseArray<T>(byte[][] source, Func<BinaryReader, T> predicate)
    {
        return ParseEnumerable(source, predicate);
    }

    public static T[] ParseEnumerable<T>(IEnumerable<byte[]> source, Func<byte[], T> predicate)
    {
        return source.Select(predicate).ToArray();
    }

    public static T[] ParseEnumerable<T>(IEnumerable<byte[]> source, Func<BinaryReader, T> predicate)
    {
        return source.Select(x=> {
            using var ms = new MemoryStream(x);
            using var br = new BinaryReader(ms);
            var data = predicate.Invoke(br);
            return data;
        }).ToArray();
    }
}
