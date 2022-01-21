using System;

namespace PBT.DowsingMachine.Projects
{
    public class DataReference
    {
        public string Name { get; set; }
        public IDataReader Reader { get; set; }
        public Delegate[] Parsers { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();

        public DataReference(string name, IDataReader reader, params Delegate[] parsers)
        {
            Name = name;
            Reader = reader;
            Parsers = parsers;
        }
    }
}
