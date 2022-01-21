using PBT.DowsingMachine.Utilities;
using System.Reflection;

namespace PBT.DowsingMachine.Projects
{
    public class DataProject
    {
        public string Name { get; set; }
        public string Root { get; set; }
        public string OutputPath { get; set; }

        public List<DataReference> References { get; set; }
        public Dictionary<string, object[]> Caches { get; set; }

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

        public void AddReference(DataReference reference)
        {
            reference.Reader.Name = Name;
            reference.Reader.Project = this;
            References.Add(reference);
        }

        protected void AddReference<T1>(string name, DataReader<T1> reader)
        {
            var reference = new DataReference(name, reader);
            References.Add(reference);
        }

        protected void AddReference<T1, T2>(string name, DataReader<T1> reader,
            Func<T1, T2> parser1)
        {
            var reference = new DataReference(name, reader, parser1);
            References.Add(reference);
        }

        protected void AddReference<T1, T2, T3>(string name, DataReader<T1> reader,
            Func<T1, T2> parser1,
            Func<T2, T3> parser2)
        {
            var reference = new DataReference(name, reader, parser1, parser2);
            References.Add(reference);
        }

        protected void AddReference<T1, T2, T3, T4>(string name, DataReader<T1> reader,
            Func<T1, T2> parser1,
            Func<T2, T3> parser2,
            Func<T3, T4> parser3)
        {
            var reference = new DataReference(name, reader, parser1, parser2, parser3);
            References.Add(reference);
        }

        public DataReference GetReference(string name)
        {
            return References.FirstOrDefault(x => x.Name == name);
        }

        public object GetData(string refName)
        {
            var reference = GetReference(refName);
            var content = reference.Reader.GetContent();
            var data = content;
            foreach(var del in reference.Parsers)
            {
                data = del.DynamicInvoke(data);
            }
            return data;
        }

        public object GetRawData(string refName)
        {
            var reference = GetReference(refName);
            var content = reference.Reader.GetContent();
            return content;
        }

        public T GetData<T>(string refName) where T : class
        {
            return (T)GetData(refName);
        }

        public virtual void BeginWork()
        {

        }

        public virtual void EndWork()
        {
            foreach(var (refName, caches) in Caches)
            {
                foreach (var cache in caches)
                {
                    if (cache is IDisposable dis) dis.Dispose();
                }
                Caches.Remove(refName);
            }
        }

        public string[] GetExtractions()
        {
            return GetMethods<ExtractionAttribute>().Select(x => x.Name).ToArray();
        }

        public string[] GetTest()
        {
            return GetMethods<TestAttribute>().Select(x => x.Name).ToArray();
        }

        private IEnumerable<MethodInfo> GetMethods<T>() where T : Attribute
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
            return method.Invoke(this, null);
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

        public IEnumerable<(string, IEnumerable<string>)> ExtractAll()
        {
            var methods = GetExtractions();
            foreach (var method in methods)
            {
                yield return (method, Extract(method));
            }
        }

        public virtual string GetPath(string relatedPath)
        {
            relatedPath = relatedPath.TrimStart('/').TrimStart('\\');
            var path = Path.Combine(Root, relatedPath);
            return path;
        }

        public virtual string[] GetFiles(string relativePath)
        {
            relativePath = relativePath.TrimStart('/').TrimStart('\\');
            var path = Path.Combine(Root, relativePath);
            var files = Directory.GetFiles(path);
            return files;
        }

        public virtual string[] GetFiles(string relativePath, string searchPattern)
        {
            relativePath = relativePath.TrimStart('/').TrimStart('\\');
            var path = Path.Combine(Root, relativePath);
            var files = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
             
            return files;
        }

        public static T[] MarshalArray<T>(IEnumerable<byte[]> source) where T : new()
        {
            return source.Select(x => MarshalUtil.Deserialize<T>(x)).ToArray();
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
}
