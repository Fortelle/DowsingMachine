using PBT.DowsingMachine.Data;
using PBT.DowsingMachine.Structures;
using PBT.DowsingMachine.Utilities;

namespace PBT.DowsingMachine.Projects;

public class ExtendableProject : DataProject
{
    public string? PatchFolder { get; set; }
    public Version Version { get; set; }
    public bool HasPatch => !string.IsNullOrEmpty(PatchFolder);
    public override string Key => string.Format("{0}_{1}", Name, Version);

    public PatchReadMode ReadMode { get; set; }

    public ExtendableProject(string name, string version, string baseFolder) : base(name, baseFolder)
    {
        Version = new Version(version);
    }

    public ExtendableProject(string name, string version, string baseFolder, string? patchFolder) : this(name, version, baseFolder)
    {
        PatchFolder = patchFolder;
    }

    public override string GetPath(string relativePath)
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
        var basePath = Path.Combine(Root, relativePath);
        return basePath;
    }

    public override FileEntry[] GetFiles(string relativePath, string searchPattern)
    {
        return GetFiles(relativePath, searchPattern, ReadMode);
    }

    public FileEntry[] GetFiles(string relativePath, string searchPattern, PatchReadMode readMode)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var baseFiles = new List<FileEntry>();
        var patchFiles = new List<FileEntry>();

        var basePath = Path.Combine(Root, relativePath);
        var patchPath = Path.Combine(PatchFolder, relativePath);
        if (!string.IsNullOrEmpty(basePath) && readMode != PatchReadMode.OnlyPatch)
        {
            baseFiles.AddRange(DirectoryUtil.GetFiles(basePath, searchPattern));
        }
        if (!string.IsNullOrEmpty(PatchFolder) && readMode != PatchReadMode.OnlyBase)
        {
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

    public record PairedFilename(string? BaseFilename, string? PatchFilename);

    public PairedFilename GetPairedFile(string relativePath)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var basePath = Path.Combine(Root, relativePath);
        var patchPath = Path.Combine(PatchFolder, relativePath);
        var pf = new PairedFilename(
            ReadMode != PatchReadMode.OnlyPatch && File.Exists(basePath) ? basePath : null,
            ReadMode != PatchReadMode.OnlyBase && File.Exists(patchPath) ? patchPath : null
            );
        return pf;
    }
    
    public PairedFilename[] GetPairedFiles(string relativePath, string searchPattern)
    {
        relativePath = relativePath.TrimStart('/').TrimStart('\\');

        var basePath = Path.Combine(Root, relativePath);
        var patchPath = Path.Combine(PatchFolder, relativePath);

        var baseFiles = ReadMode == PatchReadMode.OnlyPatch
            ? Array.Empty<string>()
            : Directory.GetFiles(basePath, searchPattern);
        var patchFiles = ReadMode == PatchReadMode.OnlyBase
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
}
