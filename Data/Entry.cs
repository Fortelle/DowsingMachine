namespace PBT.DowsingMachine.Data;

public class Entry<T>
{
    public int? Index { get; set; }
    public string? Name { get; set; }
    public T Data { get; set; }
    public string[] Parents { get; set; }

    public Entry(T data)
    {
        Data = data;
    }

    public Entry(T data, string name)
    {
        Data = data;
        Name = name;
    }

    public Entry(T data, int index)
    {
        Data = data;
        Index = index;
    }

    public Entry(T data, string? name, int? index)
    {
        Data = data;
        Name = name;
        Index = index;
    }

    public Entry<TNew> New<TNew>(TNew data)
    {
        return new Entry<TNew>(data, Name, Index);
    }
}
