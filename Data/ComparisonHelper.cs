using System.Runtime.CompilerServices;
using System.Text;

namespace PBT.DowsingMachine.Data;

public class ComparisonHelper
{
    private StringBuilder _stringBuilder { get; }

    public ComparisonHelper(StringBuilder sb)
    {
        _stringBuilder = sb;
    }

    public bool Compare(object value1, object value2, [CallerArgumentExpression("value1")] string name1 = "")
    {
        if (value1 == value2)
        {
            return true;
        }
        else
        {
            _stringBuilder.AppendLine($"- {name1}: {value1} => {value2}");
            return false;
        }
    }

    public void CompareObjects<T>(T[] array1, T[] array2)
    {
        var type = typeof(T);
        var fields = type.GetFields();
        var count = Math.Min(array1.Length, array2.Length);

        for(var i = 0; i < count; i++)
        {
            var sb = new StringBuilder();
            foreach(var field in fields)
            {
                var value1 = field.GetValue(array1[i]).ToString();
                var value2 = field.GetValue(array2[i]).ToString();
                if(value1 != value2)
                {
                    sb.AppendLine($"- {field.Name}: {value1} => {value2}");
                }
            }
            if (sb.Length > 0)
            {
                _stringBuilder.AppendLine($"#{i}");
                _stringBuilder.AppendLine(sb.ToString());
            }
        }
    }

    public void CompareObjects<TKey, TValue>(Dictionary<TKey, TValue> array1, Dictionary<TKey, TValue> array2)
    {
        var type = typeof(TValue);
        var fields = type.GetFields();
        var lstAdded = new List<TKey>();
        var lstRemoved = new List<TKey>();
        foreach (var (key, object1) in array1)
        {
            if (!array2.TryGetValue(key, out var object2))
            {
                lstRemoved.Add(key);
                continue;
            }
            var sb = new StringBuilder();
            foreach (var field in fields)
            {
                var value1 = field.GetValue(object1).ToString();
                var value2 = field.GetValue(object2).ToString();
                if (value1 != value2)
                {
                    sb.AppendLine($"- {field.Name}: {value1} => {value2}");
                }
            }
            if (sb.Length > 0)
            {
                _stringBuilder.AppendLine($"#{key}");
                _stringBuilder.AppendLine(sb.ToString());
            }
        }
        foreach(var (key, _) in array2)
        {
            if (!array1.ContainsKey(key))
            {
                lstAdded.Add(key);
                continue;
            }
        }
        _stringBuilder.AppendLine();

        if (lstRemoved.Count > 0)
        {
            _stringBuilder.AppendLine("Removed:");
            lstRemoved.ForEach(x => _stringBuilder.AppendLine($"- {x}"));
            _stringBuilder.AppendLine();
        }

        if (lstAdded.Count > 0)
        {
            _stringBuilder.AppendLine("Added:");
            lstAdded.ForEach(x => _stringBuilder.AppendLine($"- {x}"));
            _stringBuilder.AppendLine();
        }
    }
}
