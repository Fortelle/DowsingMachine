namespace PBT.DowsingMachine.Projects;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public object[] Arguments { get; set; }

    public TestAttribute()
    {
    }
    
    public TestAttribute(object[] args)
    {
        Arguments = args;
    }
}
