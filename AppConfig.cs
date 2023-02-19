using PBT.DowsingMachine.Utilities;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace PBT.DowsingMachine;

public class AppConfig
{
    private Dictionary<string, JsonValue> Configs { get; set; }

    public void Set(string key, object value)
    {
        Configs[key] = JsonValue.Create(value);
    }

    public T Get<T>(string key, T defaultValue = default) 
    {
        if (Configs.ContainsKey(key))
        {
            return Configs[key].GetValue<T>();
        }
        else
        {
            return defaultValue;
        }
    }

    public void Load(string path)
    {
        if (!File.Exists(path)) return;

        try
        {
            Configs = JsonUtil.Deserialize<Dictionary<string, JsonValue>>(path);
        }
        catch (Exception)
        {

        }
        Configs ??= new();
    }

    public void Save(string path)
    {
        JsonUtil.Serialize(path, Configs, new JsonOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });
    }

}
