using System.Text.RegularExpressions;

namespace PBT.DowsingMachine.Utilities;

internal static class TypeExtensions
{

    public static string GetGenericName(this Type type)
    {
        if (type.IsGenericType)
        {
            return Regex.Replace(type.Name, @"`\d+$", "<" + string.Join(",", type.GetGenericArguments().Select(GetGenericName)) + ">");
        }
        else if (type.IsArray && type.GetElementType().IsGenericType)
        {
            return type.GetElementType().GetGenericName() + "[]";
        }
        else
        {
            return type.Name;
        }
    }

}
