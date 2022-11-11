namespace PBT.DowsingMachine.Utilities.BinaryFinder;

// https://en.wikipedia.org/wiki/Boyer%E2%80%93Moore_string-search_algorithm
// https://stackoverflow.com/questions/16252518/boyer-moore-horspool-algorithm-for-all-matches-find-byte-array-inside-byte-arra
public class BmhFinder
{
    private byte[] Pattern { get; set; }
    private int[] Failure { get; set; }

    public BmhFinder(byte[] pattern)
    {
        Pattern = pattern;
        Failure = CreateFailureTable(pattern);
    }

    public IEnumerable<long> Find(string path)
    {
        using var fs = File.OpenRead(path);
        return Find(fs);
    }

    public IEnumerable<long> Find(byte[] data)
    {
        using var ms = new MemoryStream(data);
        foreach(var x in Find(ms))
        {
            yield return x;
        }
        //return Find(ms);
    }

    public IEnumerable<long> Find(Stream stream)
    {
        var pattern = Pattern.ToArray();
        var failure = Failure.ToArray();

        var buffer = new byte[Math.Max(2 * pattern.Length, 4096)];
        long offset = 0;
        while (true)
        {
            int dataLength;
            if (offset == 0)
            {
                dataLength = stream.Read(buffer, 0, buffer.Length);
            }
            else
            {
                Array.Copy(buffer, buffer.Length - pattern.Length, buffer, 0, pattern.Length);
                dataLength = stream.Read(buffer, pattern.Length, buffer.Length - pattern.Length) + pattern.Length;
            }

            var index = IndexOf(buffer, dataLength, pattern, failure);
            if (index >= 0)
            {
                yield return offset + index; // found!
            }

            if (dataLength < buffer.Length)
            {
                break;
            }

            offset += dataLength - pattern.Length;
        }
    }

    private static int IndexOf(byte[] value, int valueLength, byte[] pattern, int[] failure)
    {
        int index = 0;

        while (index <= valueLength - pattern.Length)
        {
            for (var i = pattern.Length - 1; value[index + i] == pattern[i]; i--)
            {
                if (i == 0) return index;
            }

            index += failure[value[index + pattern.Length - 1]];
        }

        return -1;
    }

    private static int[] CreateFailureTable(byte[] pattern)
    {
        var failure = Enumerable.Repeat(pattern.Length, byte.MaxValue + 1).ToArray();

        for (var i = 0; i < pattern.Length - 1; i++)
        {
            failure[pattern[i]] = pattern.Length - 1 - i;
        }

        return failure;
    }

}
