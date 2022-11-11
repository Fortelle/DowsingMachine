using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;

namespace PBT.DowsingMachine.Projects;

public class ParallelProject : DataProject
{
    public string[] Variations { get; set; }
    public string Variation { get; set; }

    public ParallelProject(string name, string baseFolder, string variation) : base(name, baseFolder)
    {
        Variations = new string[] { variation };
        Variation = variation;
    }

    public ParallelProject(string name, string baseFolder, string[] variations) : base(name, baseFolder)
    {
        Variations = variations;
        Variation = variations[0];
    }

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

    public override string GetPath(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var root = string.Format(Root, Variation);
        var basePath = Path.Combine(root, relativePath);

        return basePath;
    }

    public override FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        var path = GetPath(relativePath);
        var files = DirectoryUtil.GetFiles(path, searchPattern);

        return files;
    }

}
