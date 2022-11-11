using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PBT.DowsingMachine.Data;

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

    public static string Serialize<T>(T obj, JsonOptions? opt = null)
    {
        var defOpt = new JsonOptions();
        var option = new JsonSerializerOptions()
        {
            WriteIndented = opt?.WriteIndented ?? defOpt.WriteIndented,
            DefaultIgnoreCondition = opt?.DefaultIgnoreCondition ?? defOpt.DefaultIgnoreCondition,
            IgnoreReadOnlyProperties = opt?.IgnoreReadOnlyProperties ?? defOpt.IgnoreReadOnlyProperties,
            IncludeFields = opt?.IncludeFields ?? defOpt.IncludeFields,
            Encoder = opt?.Encoder ?? defOpt.Encoder,
            
        };
        if (opt?.NamePolicy == JsonNamePolicy.Lower)
        {
            option.PropertyNamingPolicy = new LowerCaseNamingPolicy();
        }
        if (opt?.ConvertEnum != false)
        {
            option.Converters.Add(new JsonStringEnumConverter());
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
    public bool ConvertEnum { get; set; } = true;

    public bool WriteIndented { get; set; } = true;
    public JsonIgnoreCondition DefaultIgnoreCondition { get; set; } = JsonIgnoreCondition.WhenWritingNull;
    public bool IgnoreReadOnlyProperties { get; set; } = true;
    public bool IncludeFields { get; set; } = true;
    public JavaScriptEncoder Encoder { get; set; } = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

}

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToUpper();
}

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLower();
}
