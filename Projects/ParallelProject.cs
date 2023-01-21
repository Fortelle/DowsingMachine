using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

public abstract class ParallelProject : DataProject
{
    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
    public string SourceFolder { get; set; }

    public string[] Variations { get; set; }
    public string Variation { get; set; }

    public virtual bool Switch(string variation)
    {
        if (Variations.Contains(variation))
        {
            Variation = variation;
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetPath(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var root = string.Format(SourceFolder, Variation);
        var basePath = Path.Combine(root, relativePath);

        return basePath;
    }

    public FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        var path = GetPath(relativePath);
        var files = DirectoryUtil.GetFiles(path, searchPattern);

        return files;
    }

}
