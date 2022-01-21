namespace PBT.DowsingMachine.FileFormats
{
    public abstract class LazyFile: IDisposable
    {
        protected FileStream Stream { get; init; }
        protected BinaryReader Reader { get; init; }

        protected LazyFile(string path)
        {
            Stream = File.OpenRead(path);
            Reader = new BinaryReader(Stream);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reader?.Close();
                Stream?.Close();
            }
        }
    }
}
