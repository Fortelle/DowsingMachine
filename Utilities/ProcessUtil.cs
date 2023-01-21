using System.Diagnostics;

namespace PBT.DowsingMachine.Utilities;

public static class ProcessUtil
{

    public static void OpenFolder(string path)
    {
        Process.Start("explorer.exe", path);
    }

    public static void OpenContainingFolder(string path)
    {
        Process.Start("explorer.exe", $"/select,\"{path}\"");
    }

    public static void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true,
        });
    }

}