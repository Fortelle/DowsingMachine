using System.Collections;
using System.Reflection;

namespace PBT.DowsingMachine.Utilities;

public static class StringUtil
{

    public static string ToLine(object obj)
    {
        switch (obj)
        {
            case null:
                return "(null)";
            case byte[] b:
                return "0x" + BitConverter.ToString(b).Replace("-", "");
            case long l:
                return $"0x{l:X16}";
            case ulong l:
                return $"0x{l:X16}";
            case string s:
                return s;
            case IDictionary d:
                return "{ " + string.Join(", ", CastDE(d).Select(x => ToLine(x.Key) + ": " + ToLine(x.Value))) + " }";
            case IEnumerable i:
                return "[ " + string.Join(", ", i.Cast<object>().Select(ToLine)) + " ]";
            case var x when x.GetType().IsPrimitive:
                return obj.ToString();
            default:
                var type = obj.GetType();
                if (type.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, Array.Empty<Type>()) is not null)
                {
                    return obj.ToString();
                }
                return JsonUtil.Serialize(obj, new JsonOptions() { WriteIndented = false, ConvertEnum = true });
        }
    }

    private static IEnumerable<DictionaryEntry> CastDE(IDictionary dict)
    {
        foreach (DictionaryEntry item in dict) yield return item;
    }


    public static string GetMostMatchedLangcode(string[] langcodes, string langcode)
    {
        if (langcodes.Contains(langcode))
        {
            return langcode;
        }
        
        if (langcode.Contains('-'))
        {
            var prime = langcode.Split('-')[0];
            if (langcodes.Contains(prime))
            {
                return prime;
            }
            else
            {
                langcode = prime;
            }
        }

        var index = Array.FindIndex(langcodes, x => x.StartsWith(langcode + '-'));
        if (index >= 0)
        {
            return langcodes[index];
        }

        throw new KeyNotFoundException();
    }
}
