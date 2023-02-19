namespace PBT.DowsingMachine.Projects;

public interface IDataReaderWrapper
{
    public IDataReaderBase Base { get; }
}

public interface IDataReaderWrapper<T> : IDataReaderWrapper
{
    public IDataReaderWrapper<TOut> Then<TOut>(Func<T, TOut> func)
    {
        Base.Parsers.Add(func);
        return new DataReaderWrapper<TOut>(Base);
    }

    public IDataReaderWrapper<T> Debug<TOut>(Func<T, TOut> func)
    {
        Base.Debugger = func;
        return this;
    }

}
