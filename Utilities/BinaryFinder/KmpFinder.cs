namespace PBT.DowsingMachine.Utilities.BinaryFinder;

// https://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm
public class KmpFinder
{
    private byte[] Pattern { get; set; }
    private int[] Failure { get; set; }

    public KmpFinder(byte[] pattern)
    {
        Pattern = pattern;
        Failure = CreateFailureTable(pattern);
    }



    public IEnumerable<long> Find(string path)
    {
        var data = File.ReadAllBytes(path);
        return Find(data);
    }

    public IEnumerable<long> Find(byte[] source)
    {
        var pattern = Pattern.ToArray();
        var failure = Failure.ToArray();
        int sourceLength = source.Length;
        int patternLength = pattern.Length;

        int i = 0, j = 0;

        while (i < sourceLength)
        {
            if (source[i] == pattern[j])
            {
                i++;
                j++;
            }

            if (j == patternLength)
            {
                yield return i - j;

                j = failure[j - 1];
            }
            else if (i < sourceLength && source[i] != pattern[j])
            {
                if (j != 0)
                {
                    j = failure[j - 1];
                }
                else
                {
                    i++;
                }
            }
        }

        //while (i < sourceLength && j < patternLength)
        //{
        //    if (source[i] == pattern[j])
        //    {
        //        i++;
        //        j++;
        //    }
        //    else if (j == 0)
        //    {
        //        i++;
        //    }
        //    else
        //    {
        //        j = failure[j - 1] + 1;
        //    }
        //}

        //return (j == sourceLength) ? (i - patternLength) : -1;
    }

    private static int[] CreateFailureTable(byte[] pattern)
    {
        var failure = new int[pattern.Length];
        failure[0] = -1;
        int i = 0, j = -1;
        while (i < pattern.Length - 1)
        {
            if (j == -1 || pattern[i] == pattern[j])
            {
                i++;
                j++;
                failure[i] = pattern[i] != pattern[j] ? j : failure[j];
            }
            else
            {
                j = failure[j];
            }
        }
        return failure;
    }

}
