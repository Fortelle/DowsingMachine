namespace PBT.DowsingMachine.Data;

[AttributeUsage(AttributeTargets.Field)]
public class AsEnumAttribute : Attribute
{
    public Type EnumType { get; set; }

    public AsEnumAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}
