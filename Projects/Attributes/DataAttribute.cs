namespace PBT.DowsingMachine.Projects;

[AttributeUsage(AttributeTargets.Method)]
public class DataAttribute : Attribute
{
    public string OutputPath { get; set; }

    public DataAttribute()
    {
    }

    public DataAttribute(string output)
    {
        OutputPath = output;
    }
}
