using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PBT.Workbench.Utilities
{
    public static class JsonUtil
    {
        public static void Serialize<T>(string path, T obj, JsonOptions opt = null)
        {
            var json = Serialize(obj, opt);
            if (json != null)
            {
                var folder = Path.GetDirectoryName(path);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllText(path, json);
            }
        }

        public static string Serialize<T>(T obj, JsonOptions opt = null)
        {
            var option = new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IgnoreReadOnlyProperties = true,
                IncludeFields = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            if(opt?.NamePolicy == JsonNamePolicy.Lower)
            {
                option.PropertyNamingPolicy = new LowerCaseNamingPolicy();
            }
            var text = JsonSerializer.Serialize(obj, option);
            text = text.Replace(@"\u3000", "\u3000");

            return text;
        }

        public static JsonNode Deserialize(string path)
        {
            var text = File.ReadAllText(path);
            var node = JsonNode.Parse(text);
            return node;
        }

        public static T Deserialize<T>(string path)
        {
            var text = File.ReadAllText(path);
            return Deserialize<T>(text);
        }


        public class ObjectToArrayConverter<T> : JsonConverter<T> where T : new()
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = typeof(T);
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();

                var obj = new T();
                foreach (var prop in properties)
                {
                    var v = reader.GetInt16();
                    prop.SetValue(obj, v);
                }

                return obj;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var type = typeof(T);
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();

                writer.WriteStartArray();
                foreach (var prop in properties)
                {
                    var v = prop.GetValue(value);
                    writer.WriteNumberValue(Convert.ToInt32(v));
                }
                writer.WriteEndArray();
            }
        }

    }

    public enum JsonNamePolicy
    {
        CamelCase,
        Lower,
    }

    public class JsonOptions
    {
        public JsonNamePolicy NamePolicy { get; set; }
    }

    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToUpper();
    }

    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}
