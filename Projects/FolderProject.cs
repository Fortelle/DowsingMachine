using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

public abstract class FolderProject : DataProject
{
    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
    public string SourceFolder { get; set; }

    public FolderProject() : base()
    {

    }

    protected override object ReadReference(IDataReference reference, GetDataOptions options)
    {
        switch (reference)
        {
            case FileRef fr when options?.ReferenceArguments?.Length > 0:
                {
                    var relpath = string.Format(fr.Relpath, options.ReferenceArguments);
                    var path = Path.Combine(SourceFolder, relpath);
                    options.CacheKey = path;
                    return path;
                }
            case FileRef fr:
                {
                    var path = Path.Combine(SourceFolder, fr.Relpath);
                    return path;
                }
            case FilesRef fr:
                {
                    var path = Path.Combine(SourceFolder, fr.Relpath);
                    var files = DirectoryUtil.GetFiles(path);
                    return files
                        .OrderBy(x => x.SortIndex)
                        .ThenBy(x => x.RelativePart)
                        .ThenBy(x => x.FullPath)
                        .ToArray();
                }
        }

        return base.ReadReference(reference, options);
    }


    [Action]
    public void OpenSourceFolder()
    {
        if (Directory.Exists(SourceFolder))
        {
            ProcessUtil.OpenFolder(SourceFolder);
        }
    }

}
