using PBT.DowsingMachine.Utilities;
using System.Text;

namespace PBT.DowsingMachine.Data;

public class DictionaryComparer<TKey>
{
    public Func<TKey, string> KeyToString { get; set; }

    public string[] IgnoreProperties { get; set; }

    public string Compare<TValue>(Dictionary<TKey, TValue> oldDictionary, Dictionary<TKey, TValue> newDictionary)
    {
        var type = typeof(TValue);
        var fields = type.GetFields();
        var properties = type.GetProperties();
        var lstAdded = new List<TKey>();
        var lstRemoved = new List<TKey>();
        var sb = new StringBuilder();
        if(IgnoreProperties?.Length > 0)
        {
            fields = fields.Where(x => !IgnoreProperties.Contains(x.Name)).ToArray();
            properties = properties.Where(x => !IgnoreProperties.Contains(x.Name)).ToArray();
        }
        foreach (var (key, object1) in oldDictionary)
        {
            if (!newDictionary.TryGetValue(key, out var object2))
            {
                lstRemoved.Add(key);
                continue;
            }
            var sb2 = new StringBuilder();
            foreach (var field in fields)
            {
                var value1 = StringUtil.ToLine(field.GetValue(object1));
                var value2 = StringUtil.ToLine(field.GetValue(object2));
                if (value1 != value2)
                {
                    sb2.AppendLine($"    {field.Name}: {value1} => {value2}");
                }
            }
            foreach (var prop in properties)
            {
                var value1 = StringUtil.ToLine(prop.GetValue(object1));
                var value2 = StringUtil.ToLine(prop.GetValue(object2));
                if (value1 != value2)
                {
                    sb2.AppendLine($"    {prop.Name}: {value1} => {value2}");
                }
            }
            if (sb2.Length > 0)
            {
                sb.AppendLine($"{GetKeyText(key)}");
                sb.AppendLine(sb2.ToString());
            }
        }
        foreach (var (key, _) in newDictionary)
        {
            if (!oldDictionary.ContainsKey(key))
            {
                lstAdded.Add(key);
                continue;
            }
        }
        sb.AppendLine();

        if (lstRemoved.Count > 0)
        {
            sb.AppendLine("Removed:");
            lstRemoved.ForEach(x => sb.AppendLine($"    {GetKeyText(x)}"));
            sb.AppendLine();
        }

        if (lstAdded.Count > 0)
        {
            sb.AppendLine("Added:");
            lstAdded.ForEach(x => sb.AppendLine($"    {GetKeyText(x)}"));
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public string Compare<TValue>((TKey, TValue)[] oldArray, (TKey, TValue)[] newArray)
    {
        var oldDictionary = oldArray.DistinctBy(x => x.Item1).ToDictionary(x => x.Item1, x => x.Item2);
        var newDictionary = newArray.DistinctBy(x => x.Item1).ToDictionary(x => x.Item1, x => x.Item2);
        return Compare(oldDictionary, newDictionary);
    }

    private string GetKeyText(TKey key)
    {
        return KeyToString?.Invoke(key) ?? key.ToString();
    }
}


