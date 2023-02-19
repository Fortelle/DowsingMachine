namespace PBT.DowsingMachine.Projects;

public abstract class DataRef<T> : IDataReference
{
    public virtual string Description { get; set; }

    public Type GenericType => typeof(T);

    public DataRef()
    {
    }

}
