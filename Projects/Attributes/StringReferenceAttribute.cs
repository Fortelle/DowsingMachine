namespace PBT.DowsingMachine.Projects;

[AttributeUsage(AttributeTargets.Property)]
public class StringReferenceAttribute : Attribute
{
    public object[] Arguments { get; set; }

    public StringReferenceAttribute(params object[] args)
    {
        Arguments = args;
    }
}
