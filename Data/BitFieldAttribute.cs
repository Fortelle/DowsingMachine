namespace PBT.DowsingMachine.Data;

[AttributeUsage(AttributeTargets.Field)]
public class BitFieldAttribute : Attribute
{
    public int Length { get; set; }

    public BitFieldAttribute(int length)
    {
        Length = length;
    }
}
