namespace PBT.DowsingMachine.Utilities.BinaryFinder;

public class StepFinder
{
    private readonly byte[][] Pattern;

    public int MinStep { get; set; } = 1;
    public int MaxStep { get; set; } = 1;

    public StepFinder(byte[] values)
    {
        Pattern = values.Select(x => new byte[] { x }).ToArray();
    }

    public StepFinder(short[] values)
    {
        Pattern = values.Select(x => BitConverter.GetBytes(x)).ToArray();
    }

    public StepFinder(ushort[] values)
    {
        Pattern = values.Select(x => BitConverter.GetBytes(x)).ToArray();
    }

    public StepFinder(int[] values)
    {
        Pattern = values.Select(x => BitConverter.GetBytes(x)).ToArray();
    }

    public StepFinder(uint[] values)
    {
        Pattern = values.Select(x => BitConverter.GetBytes(x)).ToArray();
    }

    public StepFinder(char[] values)
    {
        Pattern = values.Select(x => new[] { (byte)x }).ToArray();
    }

    public IEnumerable<(long Offset, int Step)> Find(string path)
    {
        var data = File.ReadAllBytes(path);
        return Find(data);
    }

    public IEnumerable<(long Offset, int Step)> Find(byte[] source)
    {
        for (var step = MinStep; step <= MaxStep; step++)
        {
            var query = FindData(source, Pattern, step);
            foreach (var offset in query)
            {
                yield return (offset, step);
            }
        }
    }

    private static IEnumerable<long> FindData(byte[] source, byte[][] pattern, int step)
    {
        long lastIndex = source.Length - pattern.Length * step;
        for (long i = 0; i <= lastIndex; i++)
        {
            var success = true;
            for (var j = 0; j < pattern.Length; j++)
            {
                if (!IsMatch(source, i + step * j, pattern[j]))
                {
                    success = false;
                    break;
                }
            }
            if (!success) continue;
            yield return i;
        }
    }

    private static bool IsMatch(byte[] data, long index, byte[] pattern)
    {
        for (var i = 0; i < pattern.Length; i++)
        {
            if (data[index + i] != pattern[i]) return false;
        }
        return true;
    }

}

