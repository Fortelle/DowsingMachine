using System;

namespace PBT.DowsingMachine.Projects;

public class DataReference
{
    public string Name { get; set; }
    public IDataReader Reader { get; set; }
    public Delegate[] Parsers { get; set; }
    public Dictionary<string, string?> Attributes { get; set; }

    public DataReference(string name, IDataReader reader, params Delegate[] parsers)
    {
        Name = ModifyName(name);
        Reader = reader;
        Parsers = parsers;
    }

    public DataReference AddAttribute(string name, string? value)
    {
        Attributes ??= new();
        Attributes.Add(name, value);
        return this;
    }

    public static string ModifyName(string name)
    {
        return name.Replace("_", "").ToLower();
    }
}
