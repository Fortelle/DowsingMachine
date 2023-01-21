using PBT.DowsingMachine.Data;
using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

public abstract class ExtendableProject : DataProject, IFolderProject
{
    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
    public string OriginalFolder { get; set; }

    [Option]
    [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
    public string? PatchFolder { get; set; }

    [Option]
    public Version Version { get; set; }


    public bool HasPatch => !string.IsNullOrEmpty(PatchFolder);

    public PatchReadMode ReadMode { get; set; }

    public string GetPath(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        if (!string.IsNullOrEmpty(PatchFolder))
        {
            var patchPath = Path.Combine(PatchFolder, relativePath);
            if (File.Exists(patchPath) || Directory.Exists(patchPath))
            {
                return patchPath;
            }
        }
        var basePath = Path.Combine(OriginalFolder, relativePath);
        return basePath;
    }

    public FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        return GetFiles(relativePath, searchPattern, ReadMode);
    }

    public FileEntry[] GetFiles(string relativePath, string searchPattern, PatchReadMode readMode)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var baseFiles = new List<FileEntry>();
        var patchFiles = new List<FileEntry>();

        if (!string.IsNullOrEmpty(OriginalFolder) && readMode != PatchReadMode.OnlyPatch)
        {
            var basePath = Path.Combine(OriginalFolder, relativePath);
            baseFiles.AddRange(DirectoryUtil.GetFiles(basePath, searchPattern));
        }
        if (!string.IsNullOrEmpty(PatchFolder) && readMode != PatchReadMode.OnlyBase)
        {
            var patchPath = Path.Combine(PatchFolder, relativePath);
            patchFiles.AddRange(DirectoryUtil.GetFiles(patchPath, searchPattern));
        }

        var unionFiles = new Dictionary<string, FileEntry>();
        foreach (var file in patchFiles)
        {
            unionFiles.Add(file.RelativePath, file);
        }
        foreach (var file in baseFiles)
        {
            if (!unionFiles.ContainsKey(file.RelativePath))
            {
                unionFiles.Add(file.RelativePath, file);
            }
        }

        return unionFiles.Values.ToArray();
    }

    public class PairedFilename
    {
        public string? BaseFilename { get; set; }
        public string? PatchFilename { get; set; }

        public PairedFilename(string? baseFilename, string? patchFilename)
        {
            BaseFilename = baseFilename;
            PatchFilename = patchFilename;
        }

        public string? Newer => PatchFilename ?? BaseFilename;
        public string? Older => BaseFilename ?? PatchFilename;
    }

    public PairedFilename GetPairedFile(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var basePath = string.IsNullOrEmpty(OriginalFolder) ? "" : Path.Combine(OriginalFolder, relativePath);
        var patchPath = string.IsNullOrEmpty(PatchFolder) ? "" : Path.Combine(PatchFolder, relativePath);

        var pf = new PairedFilename(
            ReadMode != PatchReadMode.OnlyPatch && File.Exists(basePath) ? basePath : null,
            ReadMode != PatchReadMode.OnlyBase && File.Exists(patchPath) ? patchPath : null
            );
        return pf;
    }
    
    public PairedFilename[] GetPairedFiles(string relativePath, string searchPattern)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var basePath = string.IsNullOrEmpty(OriginalFolder) ? "" : Path.Combine(OriginalFolder, relativePath);
        var patchPath = string.IsNullOrEmpty(PatchFolder) ? "" : Path.Combine(PatchFolder, relativePath);

        var baseFiles = (ReadMode == PatchReadMode.OnlyPatch || !Directory.Exists(basePath))
            ? Array.Empty<string>()
            : Directory.GetFiles(basePath, searchPattern);
        var patchFiles = (ReadMode == PatchReadMode.OnlyBase || !Directory.Exists(patchPath))
            ? Array.Empty<string>()
            : Directory.GetFiles(patchPath, searchPattern);

        var baseDict = baseFiles.ToDictionary(x => x.Replace(basePath, ""), x => x);
        var patchDict = patchFiles.ToDictionary(x => x.Replace(patchPath, ""), x => x);

        var unionFiles = baseDict.Keys.Union(patchDict.Keys)
            .Select(x => new PairedFilename(
                baseDict.ContainsKey(x) ? baseDict[x] : null,
                patchDict.ContainsKey(x) ? patchDict[x] : null
                ))
            .ToArray();

        return unionFiles;
    }

    public Entry<T>[] GetPatchedCollection<T>(string referenceName) where T : ICollectionArchive<T>, new()
    {
        var reference = GetReference(referenceName);
        var archiveType = reference.Reader.GetType().GenericTypeArguments[0];
        var pair = GetPairedFile(reference.Reader.RelatedPath);
        var list = new List<Entry<T>>();
        if (pair.PatchFilename != null)
        {
            var archive = new T();
            archive.Open(pair.PatchFilename);
            list.AddRange(archive.Entries);
        }
        if (pair.BaseFilename != null)
        {
            var archive = new T();
            archive.Open(pair.BaseFilename);
            foreach (var x in archive.Entries)
            {
                if (list.Any(y => y.Name == x.Name)) continue;
                list.Add(x);
            }
        }
        list.ForEach(x => x.Index = 0);
        return list.ToArray();
    }

    public override bool CheckValidity(out string error)
    {
        if (string.IsNullOrEmpty(OriginalFolder?.Trim()))
        {
            error = $"Property \"{nameof(OriginalFolder)}\" is not set.";
            return false;
        }

        if (!Directory.Exists(OriginalFolder))
        {
            error = $"Original directory \"{OriginalFolder}\" not exists.";
            return false;
        }

        return base.CheckValidity(out error);
    }

    [Action("Open original folder")]
    public void OpenOriginalFolder()
    {
        if (Directory.Exists(OriginalFolder))
        {
            ProcessUtil.OpenFolder(OriginalFolder);
        }
    }

    [Action("Open patch folder")]
    public void OpenPatchFolder()
    {
        if (Directory.Exists(PatchFolder))
        {
            ProcessUtil.OpenFolder(PatchFolder);
        }
    }

}
