namespace PBT.DowsingMachine.Projects
{
    public class FileReader<T> : DataReader<T> where T : IFileReader, new()
    {
        public FileReader(string path) : base(path)
        {
        }

        public override object[] Open()
        {
            var path = Project.GetPath(RelatedPath);
            var file = new T();
            file.Load(path);
            return new object[] { file };
        }

        protected override T Read()
        {
            var cache = GetCache();
            return (T)cache[0];
        }

    }

}