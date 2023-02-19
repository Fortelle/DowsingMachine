namespace PBT.DowsingMachine.Projects;

public abstract class DataReaderBase<T> : IDataReaderBase, IDataReaderWrapper<T>
{
    public List<Delegate> Parsers { get; } = new();

    public IDataReaderBase Base => this;
    public Type GenericType => typeof(T);

    public Delegate Debugger { get; set; }

    public IDataReaderWrapper<TOut> Then<TOut>(Func<T, TOut> func)
    {
        Parsers.Add(func);
        return new DataReaderWrapper<TOut>(Base);
    }

    public IDataReaderWrapper<T> Debug<TOut>(Func<T, TOut> func)
    {
        Debugger = func;
        return this;
    }

    public object DoRead(object input)
    {
        var inputType = input.GetType();
        var genericType = typeof(IDataReader<,>);
        var interfaces = GetType()
            .GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDataReader<,>))
            .ToArray();
        foreach (var inputInterface in interfaces)
        {
            if (inputInterface.GetGenericArguments()[0] == inputType)
            {
                return inputInterface.GetMethod("Read")?.Invoke(this, new[] { input });
            }
        }

        return null;
    }

}
