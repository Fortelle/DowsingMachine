using PBT.DowsingMachine.Projects;
using PBT.DowsingMachine.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PBT.DowsingMachine;

public static class DowsingMachineApp
{
    public static List<DataProject> Projects { get; set; }

    public static AppConfig Config { get; set; }

    public static readonly string RoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);

    private static readonly string ConfigPath = Path.Combine(RoamingPath, "App.config");

    public static Type[] SupportedProjectTypes { get; set; }

    public static void Initialize()
    {
        SupportedProjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.FullName.StartsWith("System.") & !x.FullName.StartsWith("Microsoft."))
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(DataProject)) && !t.IsAbstract)
            .ToArray();

        if (File.Exists(ConfigPath))
        {
            try
            {
                Config = JsonUtil.Deserialize<AppConfig>(ConfigPath);
            }
            catch (Exception)
            {

            }
        }
        Config ??= new();

        Projects ??= new();
        var optionAttrType = typeof(OptionAttribute);
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
        };
        foreach (var projectOptions in Config.ProjectOptions)
        {
            try
            {
                var type = SupportedProjectTypes.FirstOrDefault(x => x.FullName == projectOptions["$Type"].ToString());
                if (type is null) continue;
                var project = (DataProject)Activator.CreateInstance(type);
                foreach (var (key, jsonElement) in projectOptions)
                {
                    if (jsonElement is null) continue;
                    var property = type.GetProperty(key);
                    if (property is null) continue;
                    try
                    {
                        var value = JsonSerializer.Deserialize(((JsonElement)jsonElement).GetRawText(), property.PropertyType, jsonOptions);
                        property.SetValue(project, value);
                    }
                    catch
                    {
                    }
                }

                AddProject(project);
            } catch (Exception ex) 
            {
            }
        }
    }

    public static void Finalize()
    {
        var projectOptions = new List<Dictionary<string, object>>();
        var optionAttrType = typeof(OptionAttribute);
        foreach (var project in Projects)
        {
            var dict = new Dictionary<string, object>();
            var type = project.GetType();
            dict.Add("$Type", type.FullName);

            var options = type.GetProperties().Where(x => x.GetCustomAttributes(optionAttrType, false).Any());
            foreach(var option in options)
            {
                dict.Add(option.Name, option.GetValue(project));
            }

            projectOptions.Add(dict);
        }
        Config.ProjectOptions = projectOptions;

        try
        {
            JsonUtil.Serialize(ConfigPath, Config, new JsonOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            });
        }
        catch
        {

        }

    }

    public static DataProject AddProject(DataProject project)
    {
        if (string.IsNullOrEmpty(project.Id))
        {
            project.Id = Guid.NewGuid().ToString();
        }

        project.Configure();

        Projects.Add(project);

        return project;
    }

    public static void RemoveProject(DataProject project)
    {
        Projects.Remove(project);
    }

    public static DataProject GetProject(string id)
    {
        return Projects.FirstOrDefault(x => x.Id == id);
    }

}
