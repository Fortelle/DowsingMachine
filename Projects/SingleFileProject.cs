namespace PBT.DowsingMachine.Projects;

// for gb - gba
public class SingleFileProject : DataProject
{
    public SingleFileProject(string name, string filename) : base(name, filename)
    {
    }

    protected class SingleReader : StreamBinaryReader
    {
        public SingleReader(int offset) : base("", offset)
        {
            RelatedPath = Project.Root;
        }
    }

}
