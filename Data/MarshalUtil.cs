using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PBT.DowsingMachine.Data;

// very very low performance
public class MarshalUtil
{

    public static byte[] GetBytes(object value)
    {
        return value switch
        {
            byte value2 => new byte[] { value2 },
            bool value2 => new byte[] { (byte)(value2 ? 0x01 : 0x00) },
            Half value2 => BitConverter.GetBytes(value2),
            float value2 => BitConverter.GetBytes(value2),
            double value2 => BitConverter.GetBytes(value2),

            short value2 => BitConverter.GetBytes(value2),
            ushort value2 => BitConverter.GetBytes(value2),
            int value2 => BitConverter.GetBytes(value2),
            uint value2 => BitConverter.GetBytes(value2),
            long value2 => BitConverter.GetBytes(value2),
            ulong value2 => BitConverter.GetBytes(value2),
        };
    }

    public static byte[] Serialize<T>(T obj)
    {
        var type = typeof(T);
        var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
        var data = new List<byte>();

        for (var i = 0; i < properties.Length; i++)
        {
            var value = properties[i].GetValue(obj);
            var bit = properties[i].GetCustomAttribute<BitFieldAttribute>();
            if (bit == null)
            {
                var bytes = GetBytes(value);
                data.AddRange(bytes);
            }
            else
            {
                var fieldSize = Marshal.SizeOf(value.GetType());
                var offset = 0;

                var buffer = Convert.ToUInt64(value);
                offset += bit.Length;

                for (var j = i + 1; j < properties.Length; j++)
                {
                    var bit2 = properties[j].GetCustomAttribute<BitFieldAttribute>();
                    if (bit2 == null) break;
                    var length2 = bit.Length;
                    if (length2 == 0) length2 = fieldSize * 8 - offset;
                    var value2 = properties[j].GetValue(obj);
                    buffer |= Convert.ToUInt64(value2) << offset; // check overflow
                    offset += length2;
                    i = j;
                    if (offset >= fieldSize * 8) break;
                }

                var bytes = GetBytes(buffer);
                data.AddRange(bytes.Take(fieldSize));
            }
        }

        return data.ToArray();
    }

    public static T Deserialize<T>(byte[] data) where T : new()
    {
        var x = 0;
        return (T)Deserialize(typeof(T), data, ref x, false, 32);
    }

    public static object Deserialize(Type type, byte[] data)
    {
        var x = 0;
        return Deserialize(type, data, ref x, false, 32);
    }

    public static T DeserializeBigEndian<T>(byte[] data) where T : new()
    {
        var x = 0;
        return (T)Deserialize(typeof(T), data, ref x, true, 32);
    }

    private static object GetPrimitiveValue(Type type, byte[] data, ref int ptr, bool bigEndian, int pack)
    {
        var reqSize = Marshal.SizeOf(type);
        Debug.Assert(reqSize <= pack);
        var leftSize = pack - (ptr % pack);
        if (leftSize < reqSize)
        {
            ptr += leftSize;
        }

        var data2 = bigEndian ? data[ptr..(ptr + reqSize)].Reverse().ToArray() : data;
        var ptr2 = bigEndian ? 0 : ptr;

        object value = type switch
        {
            var t when t == typeof(byte) => data2[ptr2],
            var t when t == typeof(sbyte) => unchecked((sbyte)data2[ptr2]),
            var t when t == typeof(bool) => data2[ptr2],
            var t when t == typeof(Half) => BitConverter.ToHalf(data2, ptr2),
            var t when t == typeof(float) => BitConverter.ToSingle(data2, ptr2),
            var t when t == typeof(double) => BitConverter.ToDouble(data2, ptr2),
            var t when t == typeof(short) => BitConverter.ToInt16(data2, ptr2),
            var t when t == typeof(ushort) => BitConverter.ToUInt16(data2, ptr2),
            var t when t == typeof(int) => BitConverter.ToInt32(data2, ptr2),
            var t when t == typeof(uint) => BitConverter.ToUInt32(data2, ptr2),
            var t when t == typeof(long) => BitConverter.ToInt64(data2, ptr2),
            var t when t == typeof(ulong) => BitConverter.ToUInt64(data2, ptr2),
            var t when t == typeof(char) => (char)data2[ptr2],
        };
        ptr += reqSize;
        return value;
    }

    private static int GetStructSize(Type type)
    {
        if (type.IsPrimitive)
        {
            return Marshal.SizeOf(type);
        }
        else
        {
            var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
            var size = properties.Max(x =>
            {
                if (x.FieldType.IsArray)
                {
                    var s = GetStructSize(x.FieldType.GetElementType());
                    var asa = x.GetCustomAttribute<ArraySizeAttribute>();
                    Debug.Assert(asa != null);
                    return asa.Size * s;
                }
                else
                {
                    return GetStructSize(x.FieldType);
                }
            });
            return size;
        }
    }

    public static object Deserialize(Type type, byte[] data, ref int ptr, bool bigEndian, int pack)
    {
        if (type.IsPrimitive)
        {
            var value = GetPrimitiveValue(type, data, ref ptr, bigEndian, pack);
            return value;
        }

        var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
        var obj = Activator.CreateInstance(type);

        if (ptr > 0)
        {
            var maxSize = GetStructSize(type);
            var leftSize = pack - (ptr % pack);
            if (leftSize < maxSize)
            {
                ptr += leftSize;
            }
        }
        //var ptr = offset;
        for (var i = 0; i < properties.Length; i++)
        {
            var fieldType = properties[i].FieldType;

            if (fieldType.IsArray)
            {
                var asa = properties[i].GetCustomAttribute<ArraySizeAttribute>();
                Debug.Assert(asa != null);
                var length = asa.Size;
                var elementType = fieldType.GetElementType();
                Array array = Array.CreateInstance(elementType, length);
                for (var j = 0; j < length; j++)
                {
                    var element = Deserialize(elementType, data, ref ptr, bigEndian, pack);
                    array.SetValue(element, j);
                }
                var toArray = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(elementType);
                var dest = toArray.Invoke(null, new object[] { array });
                properties[i].SetValue(obj, dest);
            }
            else if (fieldType.IsPrimitive)
            {
                var value = GetPrimitiveValue(fieldType, data, ref ptr, bigEndian, pack);
                var bfa = properties[i].GetCustomAttribute<BitFieldAttribute>();
                if (bfa == null)
                {
                    var value2 = Convert.ChangeType(value, properties[i].FieldType);
                    properties[i].SetValue(obj, value2);
                }
                else
                {
                    var buffer = Convert.ToUInt64(value);
                    var fieldSize = Marshal.SizeOf(value);
                    var o = 0;
                    for (var j = i; j < properties.Length; j++)
                    {
                        var bit2 = j == i ? bfa : properties[j].GetCustomAttribute<BitFieldAttribute>();
                        if (bit2 == null) break;
                        var l = bit2.Length;
                        if (l == 0) l = fieldSize * 8 - o;
                        var mask = (1 << l) - 1;
                        var value2 = (buffer >> o) & (ulong)mask;
                        var value3 = Convert.ChangeType(value2, properties[j].FieldType);
                        properties[j].SetValue(obj, value3);
                        o += l;
                        i = j;
                        if (o >= fieldSize * 8) break;
                    }
                }
            }
            else
            {
                var value = Deserialize(fieldType, data, ref ptr, bigEndian, pack);
                properties[i].SetValue(obj, value);
            }

            if(ptr >= data.Length) //todo: config
            {
                break;
            }
        }

        return obj;
    }

}
