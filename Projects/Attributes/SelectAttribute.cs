namespace PBT.DowsingMachine.Projects;

[AttributeUsage(AttributeTargets.Property)]
public class SelectAttribute : Attribute
{
    public object[] Options { get; set; }

    public SelectAttribute(params object[] options)
    {
        Options = options;
    }

}
