namespace PBT.DowsingMachine.Projects;

public abstract class DataReader<TOut> : DataReader<TOut, TOut>
{

    protected DataReader(string path) : base(path)
    {
    }

    protected override TOut Read(TOut cache) => cache;
}

public abstract class DataReader<TCache, TOut> : IDataReader
{
    public string Name { get; set; }
    public string RelatedPath { get; set; }
    public virtual DataProject Project { get; set; }

    protected DataReader(string path)
    {
        RelatedPath = path;
    }

    protected abstract TCache Open();

    protected abstract TOut Read(TCache cache);

    //protected virtual void Release(TCache cache)
    //{
    //    if (cache is IDisposable dis) dis.Dispose();
    //}

    object IDataReader.Open() => Open();
    object IDataReader.Read(object cache) => Read((TCache)cache);
    //void IDataReader.Release(object cache) => Release((TCache)cache);
}
