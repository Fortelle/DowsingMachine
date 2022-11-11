namespace PBT.DowsingMachine.Data;

[AttributeUsage(AttributeTargets.Field)]
public class ArraySizeAttribute : Attribute
{
    public int Size { get; set; }

    public ArraySizeAttribute(int size)
    {
        Size = size;
    }
}
