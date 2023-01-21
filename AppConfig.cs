using System.Security.Cryptography.X509Certificates;

namespace PBT.DowsingMachine;

public class AppConfig
{
    public List<Dictionary<string, object>> ProjectOptions { get; set; }

    public string LastSelectedProjectId { get; set; }

    public string LastSelectedProjectItem { get; set; }
}
