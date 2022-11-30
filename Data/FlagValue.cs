using System.Runtime.InteropServices;

namespace PBT.DowsingMachine.Data;

[StructLayout(LayoutKind.Sequential)]
public struct FlagValue<TValue, TEnum>
    where TValue : struct
    where TEnum : Enum
{
    public TValue Value;

    public bool HasFlag(TEnum flag)
    {
        var value = Convert.ToUInt32(Value);
        var flagbit = (int)(object)flag;
        return (value & 1 << flagbit) != 0;
    }

    public bool[] GetBools()
    {
        var size = Marshal.SizeOf<TValue>();
        var length = size * 8;
        return GetBools(length);
    }

    public bool[] GetBools(int length)
    {
        var bools = new bool[length];
        var value = Convert.ToUInt32(Value);
        for (var i = 0; i < length; i++)
        {
            bools[i] = (value & (1 << i)) != 0;
        }
        return bools;
    }

    public TEnum[] GetFlags()
    {
        var bools = GetBools();
        var list = new List<TEnum>();
        for(var i = 0; i < bools.Length; i++)
        {
            if (bools[i])
            {
                list.Add((TEnum)(object)i);
            }
        }
        return list.ToArray();
    }

    public override string ToString()
    {
        return string.Join(", ", GetFlags());
    }
}
