namespace PBT.DowsingMachine.Projects;

[AttributeUsage(AttributeTargets.Method)]
public class ActionAttribute : Attribute
{
    public string Name { get; init; }

    public ActionAttribute(string name)
    {
        Name = name;
    }

}
