namespace PBT.DowsingMachine.Projects;

public class DataReader<T> : DataReaderBase<T>, IDataReader<T, T>
{
    public T Read(T input)
    {
        return input;
    }
}
