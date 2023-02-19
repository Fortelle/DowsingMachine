namespace PBT.DowsingMachine.Data;

public static class BinaryReaderExtensions
{

    public static byte[][] ReadByteMatrix(this BinaryReader br, int itemSize, int itemCount = -1, int offset = 0)
    {
        if (offset > 0)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
        }
        if (itemCount == -1)
        {
            itemCount = (int)((br.BaseStream.Length - offset) / itemSize);
        }

        var result = new byte[itemCount][];
        for (int i = 0; i < itemCount; i++)
        {
            result[i] = br.ReadBytes(itemSize);
        }
        return result;
    }

    public static IEnumerable<BinaryReader> AsEnumerableMatrix(this BinaryReader br, int itemSize, int itemCount = -1, int offset = 0)
    {
        if (offset > 0)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
        }
        if (itemCount == -1)
        {
            itemCount = (int)((br.BaseStream.Length - offset) / itemSize);
        }

        var alignment = offset;
        for (int i = 0; i < itemCount; i++)
        {
            if(br.BaseStream.Position < alignment)
            {
                br.BaseStream.Position = alignment;
            }
            yield return br;
            alignment += itemSize;
        }
    }

    public static short[] ReadShorts(this BinaryReader br, int count)
    {
        return Enumerable.Range(0, count).Select(_ => br.ReadInt16()).ToArray();
    }

    public static ushort[] ReadUShorts(this BinaryReader br, int count)
    {
        return Enumerable.Range(0, count).Select(_ => br.ReadUInt16()).ToArray();
    }

    public static int[] ReadIntegers(this BinaryReader br, int count)
    {
        return Enumerable.Range(0, count).Select(_ => br.ReadInt32()).ToArray();
    }

    public static uint[] ReadUIntegers(this BinaryReader br, int count)
    {
        return Enumerable.Range(0, count).Select(_ => br.ReadUInt32()).ToArray();
    }

    public static int[] ToIntegers(this byte[] bytes)
    {
        return bytes.Select(x => (int)x).ToArray();
    }

}
