namespace PBT.DowsingMachine.Projects
{
    public class MultiFileReader<T> : DataReader<IEnumerable<T>>
    {
        private readonly string Pattern;

        public MultiFileReader(string path, string pattern) : base(path)
        {
            Pattern = pattern;
        }

        protected override IEnumerable<T> Read()
        {
            var path = Project.GetPath(RelatedPath);
            var files = Directory.GetFiles(path, Pattern, SearchOption.AllDirectories);

            switch (typeof(T))
            {
                case var type when type == typeof(string):
                    foreach (var f in files)
                    {
                        var text = File.ReadAllText(f);
                        yield return (T)(object)text;
                    }
                    break;
                case var type when type == typeof(byte[]):
                    foreach (var f in files)
                    {
                        var data = File.ReadAllBytes(f);
                        yield return (T)(object)data;
                    }
                    break;
                case var type when type == typeof(BinaryReader):
                    foreach (var f in files)
                    {
                        var fs = File.OpenRead(f); // ???
                        var br = new BinaryReader(fs);
                        yield return (T)(object)br;
                    }
                    break;
            }

            yield break;
        }
    }

}
