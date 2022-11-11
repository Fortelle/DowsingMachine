namespace PBT.DowsingMachine.Utilities.Codecs;

// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
public static class FnvHash
{
    private const uint FnvPrime32 = 0x01000193;
    private const uint OffsetBasis32 = 0x811C9DC5;

    private const ulong FnvPrime64 = 0x00000100_000001b3;
    private const ulong OffsetBasis64 = 0xCBF29CE4_84222645;

    private static ulong Fnv1(byte[] data, ulong prime, ulong offset)
    {
        var hash = offset;
        foreach (var b in data)
        {
            hash *= prime;
            hash ^= b;
        }
        return hash;
    }

    private static ulong Fnv1a(byte[] data, ulong prime, ulong offset)
    {
        var hash = offset;
        foreach (var b in data)
        {
            hash ^= b;
            hash *= prime;
        }
        return hash;
    }

    private static uint Fnv1(byte[] data, uint prime, uint offset)
    {
        var hash = offset;
        foreach (var b in data)
        {
            hash *= prime;
            hash ^= b;
        }
        return hash;
    }

    private static uint Fnv1a(byte[] data, uint prime, uint offset)
    {
        var hash = offset;
        foreach (var b in data)
        {
            hash ^= b;
            hash *= prime;
        }
        return hash;
    }
    public static ulong Fnv1_64(string text)
    {
        var bytes = text.Select(x => (byte)x).ToArray();
        return Fnv1(bytes, FnvPrime64, OffsetBasis64);
    }

    public static ulong Fnv1_64(byte[] data)
    {
        return Fnv1(data, FnvPrime64, OffsetBasis64);
    }

    public static ulong Fnv1a_64(string text)
    {
        var bytes = text.Select(x => (byte)x).ToArray();
        return Fnv1a(bytes, FnvPrime64, OffsetBasis64);
    }

    public static ulong Fnv1a_64(byte[] data)
    {
        return Fnv1a(data, FnvPrime64, OffsetBasis64);
    }
}
