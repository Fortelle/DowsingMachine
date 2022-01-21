namespace PBT.DowsingMachine.Projects
{
    public class SingleFileProject : DataProject
    {
        public SingleFileProject(string name, string filename) : base(name, filename)
        {
        }

        protected class SingleReader : DataReader<BinaryReader>
        {
            public int Offset { get; set; }

            public SingleReader(int offset) : base(null)
            {
                UseCache = true;
                Offset = offset;
            }

            public override object[] Open()
            {
                var fs = File.OpenRead(Project.Root);
                var br = new BinaryReader(fs);
                return new object[] {
                    fs,
                    br,
                };
            }

            protected override BinaryReader Read()
            {
                var cache = GetCache();
                var reader = (BinaryReader)cache[1];
                reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
                return reader;
            }
        }

    }

}
