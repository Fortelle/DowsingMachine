namespace PBT.DowsingMachine.Projects;

public class DataResource
{
    public string Key { get; set; }

    public IDataReference Reference { get; set; }
    public IDataReaderWrapper? Reader { get; set; }
    public Delegate Exporter { get; set; }

    public object[] PreviewArguments { get; init; }

    public bool Enable { get; init; } = true;
    public bool Browsable { get; set; } = true;
    public bool Previewable { get; set; } = true;


    public DataResource()
    {
    }

    public DataResource(string key)
    {
        Key = key;
    }

    public Type OutputType
    {
        get
        {
            if (Reader is not null && Reader.Base is not null)
            {
                if (Reader.Base.Parsers?.Count > 0)
                {
                    return Reader.Base.Parsers.Last().Method.ReturnType;
                }

                return Reader.Base.GenericType;
            }
            else if (Reference is not null)
            {
                return Reference.GenericType;
            }

            return null;
        }
    }

}
