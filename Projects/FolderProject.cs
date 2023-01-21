using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

public abstract class FolderProject : DataProject, IFolderProject
{
    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
    public string SourceFolder { get; set; }

    public FolderProject() : base()
    {

    }

    public string GetPath(string relatedPath)
    {
        relatedPath = relatedPath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(SourceFolder, relatedPath);
        return path;
    }

    public FileEntry[] GetFiles(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(SourceFolder, relativePath);
        var files = DirectoryUtil.GetFiles(path, "*");
        return files;
    }

    public FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');
        var path = Path.Combine(SourceFolder, relativePath);
        var files = DirectoryUtil.GetFiles(path, searchPattern);
        return files;
    }


    [Action("Open source folder")]
    public void OpenSourceFolder()
    {
        if (Directory.Exists(SourceFolder))
        {
            ProcessUtil.OpenFolder(SourceFolder);
        }
    }

}

public interface IFolderProject
{
    string GetPath(string relativePath);

}
