namespace PBT.DowsingMachine.Projects;

public class DataReaderWrapper<T> : IDataReaderWrapper<T>
{
    public IDataReaderBase Base { get; init; }

    public DataReaderWrapper(IDataReaderBase @base)
    {
        Base = @base;
    }

}
