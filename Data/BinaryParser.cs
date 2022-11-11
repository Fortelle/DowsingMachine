using System;
using System.Linq;

namespace PBT.DowsingMachine.Data;

public abstract class BinaryParser
{
    protected static int[] Read(ulong value, params int[] binLength)
    {
        var sum = binLength.Sum();
        if (sum < 32)
        {
            binLength = binLength.Concat(new int[] { 32 - sum }).ToArray();
        }
        var results = new int[binLength.Length];

        for (var i = 0; i < binLength.Length; i++)
        {
            if (i > 0)
            {
                value >>= binLength[i - 1];
            }
            var a = (1 << binLength[i]) - 1;
            var r = (long)value & a;
            //if (binLength[i] > 1 && r == a) r = -1;
            results[i] = (int)r;
        }
        return results;
    }

    protected static int[] Read(byte[] values, params int[] binLength)
    {
        var results = new int[binLength.Length + (binLength.Sum() < values.Length * 8 ? 1 : 0)];
        var bin = string.Join("", values.Reverse().Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
        var sum = 0;
        for (var i = 0; i < binLength.Length; i++)
        {
            sum += binLength[i];
            var b = bin.Substring(bin.Length - sum, binLength[i]);
            results[i] = Convert.ToInt32(b, 2);
        }
        if (results.Length > binLength.Length)
        {
            var b = bin[..^sum];
            results[^1] = Convert.ToInt32(b, 2);
        }

        return results;
    }

    protected static int Overflow(int value, uint bound)
    {
        return value == bound ? (int)(bound - value - 1) : (int)value;
    }

    protected static int Sign(int value, uint bound)
    {
        return value <= (bound >> 1) ? value : (int)(value - bound - 1);
    }

    protected static string ToBin(int value, int length)
    {
        return "0b " + Convert.ToString(value, 2).PadLeft(length, '0');
    }
}
