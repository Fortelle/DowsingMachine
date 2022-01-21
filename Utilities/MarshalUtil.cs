using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PBT.DowsingMachine.Utilities
{
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
                        var value2 = properties[j].GetValue(obj);
                        buffer |= Convert.ToUInt64(value2) << offset; // check overflow
                        offset += bit2.Length;
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

            var type = typeof(T);
            var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
            var obj = (object)new T(); // boxing
            var ptr = 0;
            for (var i = 0; i < properties.Length; i++)
            {
                var bit = properties[i].GetCustomAttribute<BitFieldAttribute>();
                var size = properties[i].GetCustomAttribute<ArraySizeAttribute>();

                var fieldType = properties[i].FieldType;
                if (fieldType.IsArray)
                {
                    var list = new List<object>();

                }

                var fieldSize = Marshal.SizeOf(properties[i].FieldType);
                var leftSize = 4 - (ptr % 4);
                if (leftSize < fieldSize)
                {
                    ptr += leftSize;
                }
                object value = properties[i].GetValue(obj) switch
                {
                    byte => data[ptr],
                    bool => data[ptr],
                    Half => BitConverter.ToHalf(data, ptr),
                    float => BitConverter.ToSingle(data, ptr),
                    double => BitConverter.ToDouble(data, ptr),
                    short => BitConverter.ToInt16(data, ptr),
                    ushort => BitConverter.ToUInt16(data, ptr),
                    int => BitConverter.ToInt32(data, ptr),
                    uint => BitConverter.ToUInt32(data, ptr),
                    long => BitConverter.ToInt64(data, ptr),
                    ulong => BitConverter.ToUInt64(data, ptr),
                };

                properties[i].SetValue(obj, value);

                if (bit != null)
                {
                    var buffer = Convert.ToUInt64(value);
                    var offset = 0;

                    for (var j = i; j < properties.Length; j++)
                    {
                        var bit2 = j == i ? bit : properties[j].GetCustomAttribute<BitFieldAttribute>();
                        if (bit2 == null) break;
                        var l = bit2.Length;
                        if (l == 0) l = fieldSize * 8 - offset;
                        var mask = (1 << l) - 1;
                        var value2 = (buffer >> offset) & (ulong)mask;
                        var value3 = Convert.ChangeType(value2, properties[j].FieldType);
                        properties[j].SetValue(obj, value3);
                        offset += l;
                        i = j;
                        if (offset >= fieldSize * 8) break;
                    }
                }
                ptr += fieldSize;
            }

            return (T)obj;
        }

        public static T DeserializeBigEndian<T>(byte[] data) where T : new()
        {
            var x = 0;
            return (T)Deserialize(typeof(T), data, ref x, true, 32);
        }

        private static object GetPrimitiveValue(Type type, byte[] data, ref int ptr, bool bigEndian, int bit)
        {
            var reqSize = Marshal.SizeOf(type);
            Debug.Assert(reqSize <= (bit / 8));
            var leftSize = (bit / 8) - (ptr % (bit / 8));
            if (leftSize < reqSize)
            {
                ptr += leftSize;
            }

            var data2 = bigEndian ? data[ptr..(ptr + reqSize)].Reverse().ToArray() : data;
            var ptr2 = bigEndian ? 0 : ptr;

            object value = type switch
            {
                var t when t == typeof(byte) => data2[ptr2],
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
            var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
            var size = properties.Max(x => GetStructSize(x.FieldType));
            return size;
        }

        public static object Deserialize(Type type, byte[] data, ref int ptr, bool bigEndian, int bit)
        {
            if (type.IsPrimitive)
            {
                var value = GetPrimitiveValue(type, data, ref ptr, bigEndian, bit);
                return value;
            }

            var properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();
            var obj = Activator.CreateInstance(type);

            if (ptr > 0)
            {
                var maxSize = GetStructSize(type);
                var leftSize = (bit / 8) - (ptr % (bit / 8));
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
                        var element = Deserialize(elementType, data, ref ptr, bigEndian, bit);
                        array.SetValue(element, j);
                    }

                    var toArray = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(elementType);
                    var dest = toArray.Invoke(null, new object[] { array });

                    properties[i].SetValue(obj, dest);
                }
                else if (fieldType.IsPrimitive)
                {
                    var value = GetPrimitiveValue(fieldType, data, ref ptr, bigEndian, bit);
                    var bfa = properties[i].GetCustomAttribute<BitFieldAttribute>();
                    if (bfa == null)
                    {
                        properties[i].SetValue(obj, value);
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
                    var value = Deserialize(type, data, ref ptr, bigEndian, bit);
                    properties[i].SetValue(obj, value);
                }
            }

            return obj;
        }

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class BitFieldAttribute : Attribute
    {
        public int Length { get; set; }

        public BitFieldAttribute(int length)
        {
            Length = length;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ArraySizeAttribute : Attribute
    {
        public int Size { get; set; }

        public ArraySizeAttribute(int size)
        {
            Size = size;
        }
    }

}
