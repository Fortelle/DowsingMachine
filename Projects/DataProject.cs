using PBT.DowsingMachine.Data;
using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

public abstract class DataProject : IDataProject, IDisposable
{
    [Option]
    public string Name { get; set; }

    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))] 
    public string OutputFolder { get; set; }

    [Option]
    [ReadOnly(true)]
    public string Id { get; set; }

    public virtual string Description { get; }

    public ResourceManager Resources { get; } = new();
    protected ProjectCache Caches { get; } = new();
    protected bool IsWorking { get; set; }
    private bool IsConfigured { get; set; }

    public List<MethodInfo> ActionMethods { get; set; }
    public List<MethodInfo> TestMethods { get; set; }
    public List<MethodInfo> DataMethods { get; set; }

    public object GetData(string resName, GetDataOptions options = null)
    {
        var res = Resources.Get(resName);
        if (res == null)
        {
            throw new KeyNotFoundException();
        }

        if(options == null)
        {
            options = new();
            if (res.PreviewArguments?.Length > 0)
            {
                options.ReferenceArguments = res.PreviewArguments;
            }
        }

        if (string.IsNullOrEmpty(options.CacheKey))
        {
            options.CacheKey = $"resource_cache:{res.Key}";
        }

        return GetData(res, options);
    }

    public T GetData<T>(string resName, GetDataOptions options = null)
    {
        return (T)GetData(resName, options);
    }

    public T GetData<T>(Func<T> func)
    {
        var key = "method_cache:" + func.Method.Name;
        return GetOrCreateCache(key, func);
    }

    protected T GetOrCreateCache<T>(string key, Func<T> func)
    {
        if (Caches.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        else
        {
            var sw = new Stopwatch();
            sw.Start();
            var data = func();
            sw.Stop();
            if(key != "")
            {
                Caches.Add(key, data);
            }

            Debug.WriteLine($"Cached object: {key} ({sw.ElapsedMilliseconds}ms).");
            return data;
        }
    }

    protected T GetOrCreateCache<T>(Func<T> func, [CallerMemberName] string methodName = "")
    {
        var key = "method_cache:" + methodName;
        return GetOrCreateCache(key, func);
    }

    public object ReadReference(string resName, GetDataOptions args)
    {
        var res = Resources.Get(resName);
        return ReadReference(res.Reference, args);
    }

    protected virtual object ReadReference(IDataReference reference, GetDataOptions options)
    {
        switch (reference)
        {
            case null:
                return null;
            case ResRef rr:
                {
                    var res = GetData(rr.ResKey);
                    return res;
                }
        }

        throw new NotSupportedException("Not supported reference type.");
    }

    private object GetData(DataResource res, GetDataOptions options)
    {
        var cachekey = options.CacheKey;
        if (options.UseCache)
        {
            if( Caches.TryGetValue(cachekey, out var cache))
            {
                return cache;
            }
        }

        var data = ReadReference(res.Reference, options);
        if(data != null && options.UseCache)
        {
            cachekey = options.CacheKey;
            if (Caches.TryGetValue(cachekey, out var cache))
            {
                return cache;
            }
        }
        if (options.Step == 0)
        {
            return data;
        }

        if(res.Reader == null)
        {
            return data;
        }

        var getdata = () =>
        {
            data = res.Reader.Base.DoRead(data);
            if (options.Step == 1)
            {
                return data;
            }

            var parsers = res.Reader.Base.Parsers;
            if (parsers?.Count > 0)
            {
                for (var i = 0; i < parsers.Count; i++)
                {
                    var newData = i == 0 && parsers[0].Method.GetParameters().Length == 0
                        ? parsers[i].DynamicInvoke()
                        : parsers[i].DynamicInvoke(data)
                        ;
                    if (data is IDisposable dis && !Caches.ContainsValue(data))
                    {
                        dis.Dispose();
                    }
                    data = newData;
                    if (options.Step == i + 2)
                    {
                        return data;
                    }
                }
            }

            return data;
        };


        //var getdata = () =>
        //{
        //    if (res.Reader != null)
        //    {
        //        data = res.Reader.Read(data);
        //    }
        //    if (options.Step == 0)
        //    {
        //        return data;
        //    }

        //    if (res.Parsers?.Count > 0)
        //    {
        //        for (var i = 0; i < res.Parsers.Count; i++)
        //        {
        //            var newData = i == 0 && res.Parsers[0].Method.GetParameters().Length == 0
        //                ? res.Parsers[i].DynamicInvoke()
        //                : res.Parsers[i].DynamicInvoke(data)
        //                ;
        //            if (data is IDisposable dis && !Caches.ContainsValue(data))
        //            {
        //                dis.Dispose();
        //            }
        //            data = newData;
        //            if (options.Step == i + 1)
        //            {
        //                return data;
        //            }
        //        }
        //    }

        //    return data;
        //};

        if (options.UseCache)
        {
            GetOrCreateCache(cachekey, getdata);
        }
        else
        {
            getdata();
        }

        return data;
    }

    public virtual void Configure()
    {
    }

    public virtual bool CheckValidity(bool isNew, out string? error)
    {
        error = "";
        return true;
    }

    public virtual void BeginWork()
    {
        IsWorking = true;
    }

    public virtual void EndWork()
    {
        Caches.Release();
        IsWorking = false;
    }

    public void Active()
    {
        if (!IsConfigured)
        {
            Configure();
            IsConfigured = true;
        }

        LoadMethods();
    }

    public virtual void Dispose()
    {
        Caches?.Release();
    }


    private void LoadMethods()
    {
        ActionMethods = new();
        DataMethods = new();
        TestMethods = new();

        var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes().ToArray();
            if (!AuthorizeMethod(attributes)) continue;
            foreach (var attr in attributes)
            {
                switch (attr)
                {
                    case ActionAttribute:
                        ActionMethods.Add(method);
                        break;
                    case TestAttribute:
                        TestMethods.Add(method);
                        break;
                    case DataAttribute:
                        DataMethods.Add(method);
                        break;
                }
            }
        }
    }

    protected virtual bool AuthorizeMethod(Attribute[] attributes)
    {
        if(this is IMethodAuthorizable auth)
        {
            return auth.AuthorizeMethod(attributes);
        }

        return true;
    }

    public void Export(MethodInfo method)
    {
        if (!CheckOutputFolder()) return;

        var attr = method.GetCustomAttribute<DataAttribute>();
        if (attr == null || string.IsNullOrEmpty(attr.OutputPath)) return;

        var data = method.Invoke(this, Array.Empty<object>());
        if (data is null) return;

        var path = Path.Combine(OutputFolder, attr.OutputPath);
        Export(data, path);
    }

    protected virtual void Export(object data, string path)
    {
        switch (data)
        {
            case string str:
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, str);
                break;
            case string[] str:
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllLines(path, str);
                break;
            case StringBuilder sb:
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, sb.ToString());
                break;
            case IEnumerable<ExportItem> entries:
                foreach(var entry in entries)
                {
                    Export(entry.Data, Path.Combine(path, entry.Relpath));
                }
                break;
            case IExportable exportable:
                exportable.Export(path);
                break;
            default:
                JsonUtil.Serialize(path, data, new()
                {
                    IgnoreReadOnlyProperties = false,
                });
                break;
        }
    }


    public object Test(string methodName)
    {
        var method = GetType().GetMethod(methodName);
        var attr = method.GetCustomAttribute<TestAttribute>();
        var args = attr.Arguments;
        BeginWork();
        var data = method.Invoke(this, args);
        EndWork();
        return data;
    }

    public bool CheckOutputFolder()
    {
        if (string.IsNullOrEmpty(OutputFolder))
        {
            MessageBox.Show($"{nameof(OutputFolder)} is not set.");
            return false;
        }
        if(!Directory.Exists(OutputFolder))
        {
            MessageBox.Show($"Output folder {nameof(OutputFolder)} not exists.");
            return false;
        }
        return true;
    }

    #region "Parsers"

    //protected static TElement[] ReadArchive<TFormat, TElement>(string filepath) where TFormat : ICollectionArchive<TElement>, new()
    //{
    //    var file = new TFormat();
    //    file.Open(filepath);
    //    var data = file.GetData();
    //    if (file is IDisposable id) id.Dispose();
    //    return data;
    //}

    //protected static TElement[] ReadArchive<TFormat, TElement>(byte[] source) where TFormat : ICollectionArchive<TElement>, new()
    //{
    //    var file = new TFormat();
    //    file.Open(source);
    //    var data = file.GetData();
    //    if (file is IDisposable id) id.Dispose();
    //    return data;
    //}

    //protected static TElement ReadObjectArchive<TFormat, TElement>(string filepath) where TFormat : IObjectArchive<TElement>, new()
    //{
    //    var file = new TFormat();
    //    file.Open(filepath);
    //    if (file is IDisposable id) id.Dispose();
    //    return file.Data;
    //}

    //protected static TElement ReadObjectArchive<TFormat, TElement>(byte[] source) where TFormat : IObjectArchive<TElement>, new()
    //{
    //    var file = new TFormat();
    //    file.Open(source);
    //    if (file is IDisposable id) id.Dispose();
    //    return file.Data;
    //}

    //protected static TFormat ReadArchive<TFormat>(string filepath) where TFormat : IArchive, new()
    //{
    //    var file = new TFormat();
    //    file.Open(filepath);
    //    return file;
    //}

    //protected static TFormat ReadArchive<TFormat>(byte[] source) where TFormat : IArchive, new()
    //{
    //    var file = new TFormat();
    //    file.Open(source);
    //    return file;
    //}

    protected static T[] MarshalTo<T>(byte[] source) where T : struct
    {
        var size = Marshal.SizeOf<T>();
        var ret = new T[source.Length / size];
        Buffer.BlockCopy(source, 0, ret, 0, source.Length);
        return ret;
    }

    protected static T[] MarshalArray<T>(IEnumerable<byte[]> source) where T : new()
    {
        return source.Select(MarshalUtil.Deserialize<T>).ToArray();
    }

    protected static T[] MarshalArray<T>(IEnumerable<Entry<byte[]>> source) where T : new()
    {
        return source.Select(x => MarshalUtil.Deserialize<T>(x.Data)).ToArray();
    }

    protected static T[] ParseArray<T>(byte[][] source, Func<byte[], T> predicate)
    {
        return source.Select(predicate).ToArray();
    }

    protected static T[] ParseArray<T>(byte[][] source, Func<BinaryReader, T> predicate)
    {
        return ParseEnumerable(source, predicate);
    }

    protected static T[] ParseEnumerable<T>(IEnumerable<byte[]> source, Func<byte[], T> predicate)
    {
        return source.Select(predicate).ToArray();
    }

    protected static T[] ParseEnumerable<T>(IEnumerable<byte[]> source, Func<BinaryReader, T> predicate)
    {
        return source.Select(x =>
        {
            using var ms = new MemoryStream(x);
            using var br = new BinaryReader(ms);
            var data = predicate.Invoke(br);
            return data;
        }).ToArray();
    }

    //protected static Func<byte[][], T[]> ParseEnumerable<T>(Func<byte[], T> predicate)
    //{
    //    return (source) => source.Select(predicate).ToArray();
    //}

    protected static Func<IEnumerable<byte[]>, T[]> ParseEnumerable<T>(Func<byte[], T> predicate)
    {
        return (source) => source.Select(predicate).ToArray();
    }

    protected static Func<IEnumerable<byte[]>, T[]> ParseEnumerable<T>(Func<BinaryReader, T> predicate)
    {
        return (source) => source.Select(row => {
            using var ms = new MemoryStream(row);
            using var br = new BinaryReader(ms);
            var data = predicate.Invoke(br);
            return data;
        }).ToArray();
    }


    #endregion

    #region "Actions"

    [Action]
    public void OpenOutputFolder()
    {
        if (Directory.Exists(OutputFolder))
        {
            ProcessUtil.OpenFolder(OutputFolder);
        }
    }

    [Action]
    public void ExportAllData()
    {
        if (!CheckOutputFolder()) return;

        BeginWork();

        foreach (var method in DataMethods)
        {
            Export(method);
        }

        EndWork();

        OpenOutputFolder();
    }

    #endregion
}
