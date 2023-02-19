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
    private static readonly string ProjectConfigPath = Path.Combine(RoamingPath, "Projects.config");

    public static Type[] SupportedProjectTypes { get; set; }

    public static void Initialize()
    {
        SupportedProjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.FullName.StartsWith("System.") & !x.FullName.StartsWith("Microsoft."))
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(DataProject)) && !t.IsAbstract)
            .ToArray();

        Config = new();
        Config.Load(ConfigPath);
        LoadProjectConfig();
    }

    public static void Finalize()
    {
        SaveProjectConfig();
        Config.Save(ConfigPath);
    }

    public static void LoadProjectConfig()
    {
        Projects ??= new();
        if (!File.Exists(ProjectConfigPath)) return;

        var projectOptions = JsonUtil.Deserialize<Dictionary<string, object>[]>(ProjectConfigPath);
        foreach (var options in projectOptions)
        {
            var project = CreateProject(options);
            if (project != null)
            {
                AddProject(project);
            }
        }
    }

    public static void SaveProjectConfig()
    {
        var projectOptions = new List<Dictionary<string, object>>();
        foreach (var project in Projects)
        {
            var options = GetProjectOptions(project);
            projectOptions.Add(options);
        }

        JsonUtil.Serialize(ProjectConfigPath, projectOptions, new JsonOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });
    }

    private static DataProject? CreateProject(Dictionary<string, object> options)
    {
        var type = SupportedProjectTypes.FirstOrDefault(x => x.FullName == options["$Type"].ToString());
        if (type is null) return null;
        var project = (DataProject)Activator.CreateInstance(type);
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
        };
        foreach (var (key, valueobject) in options)
        {
            if (valueobject is null) continue;
            var property = type.GetProperty(key);
            if (property is null) continue;
            try
            {
                if(valueobject is JsonElement je)
                {
                    var value = JsonSerializer.Deserialize(je.GetRawText(), property.PropertyType, jsonOptions);
                    property.SetValue(project, value);
                }
                else
                {
                    property.SetValue(project, valueobject);
                }
            }
            catch
            {
            }
        }
        return project;
    }

    private static Dictionary<string, object> GetProjectOptions(DataProject project)
    {
        var optionAttrType = typeof(OptionAttribute);
        var dict = new Dictionary<string, object>();
        var type = project.GetType();
        dict.Add("$Type", type.FullName);

        var options = type.GetProperties().Where(x => x.GetCustomAttributes(optionAttrType, false).Any());
        foreach (var option in options)
        {
            dict.Add(option.Name, option.GetValue(project));
        }
        return dict;
    }

    public static DataProject AddProject(DataProject project)
    {
        if (string.IsNullOrEmpty(project.Id))
        {
            project.Id = Guid.NewGuid().ToString();
        }

        Projects.Add(project);

        return project;
    }

    public static void RemoveProject(DataProject project)
    {
        Projects.Remove(project);
    }

    public static DataProject ReloadProject(DataProject project)
    {
        var index = Projects.IndexOf(project);
        var options = GetProjectOptions(project);

        var newProject = CreateProject(options);
        Projects[index] = newProject!;

        project.Dispose();
        return newProject;
    }

    public static DataProject? GetProject(string id)
    {
        return Projects.FirstOrDefault(x => x.Id == id);
    }

    public static DataProject? GetProject(Func<DataProject, bool> predicate)
    {
        return Projects.FirstOrDefault(predicate);
    }

    public static T FindProject<T>(Func<IEnumerable<T>, T> predicate, bool active = true) where T : DataProject
    {
        var result = predicate(Projects.OfType<T>());
        if (result is null)
        {
            MessageBox.Show("No suitable project found.");
            return null;
        }

        if (active)
        {
            result.Active();
        }

        return (T)result;
    }

    public static string GetLangcode(string[] langcodes)
    {
        var langcode = DowsingMachineApp.Config.Get<string>("PreviewLanguage");
        if (string.IsNullOrEmpty(langcode)) langcode = "ja";
        langcode = StringUtil.GetMostMatchedLangcode(langcodes, langcode);
        if (string.IsNullOrEmpty(langcode))
        {
            langcode = StringUtil.GetMostMatchedLangcode(langcodes, "ja");
        }
        return langcode;
    }

}
