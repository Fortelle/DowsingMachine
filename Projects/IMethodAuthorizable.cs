namespace PBT.DowsingMachine.Projects;

public interface IMethodAuthorizable
{
    public bool AuthorizeMethod(Attribute[] attributes);
}
