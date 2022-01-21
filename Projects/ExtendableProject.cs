using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PBT.DowsingMachine.Projects
{
    public class ExtendableProject : DataProject
    {
        public string PatchFolder { get; set; }
        public Version Version { get; set; }
        public bool HasPatch => !string.IsNullOrEmpty(PatchFolder);

        public PatchReadMode ReadMode { get; set; }

        public ExtendableProject(string name, string version, string baseFolder) : base(name, baseFolder)
        {
            Version = new Version(version);
        }

        public ExtendableProject(string name, string version, string baseFolder, string patchFolder) : this(name, version, baseFolder)
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

        public override string[] GetFiles(string relativePath, string searchPattern)
        {
            return GetFiles(relativePath, searchPattern, ReadMode);
        }

        public string[] GetFiles(string relativePath, string searchPattern, PatchReadMode readMode)
        {
            relativePath = relativePath.TrimStart('/').TrimStart('\\');

            var patchPath = Path.Combine(PatchFolder, relativePath);
            var basePath = Path.Combine(Root, relativePath);

            var patchFiles = (string.IsNullOrEmpty(PatchFolder) || readMode == PatchReadMode.OnlyBase)
                ? Array.Empty<string>()
                : Directory.GetFiles(patchPath, searchPattern);

            var baseFiles = readMode == PatchReadMode.OnlyPatch
                ? Array.Empty<string>()
                : Directory.GetFiles(basePath, searchPattern);

            var unionFiles = new Dictionary<string, string>();
            foreach (var file in patchFiles)
            {
                var key = file.Replace(patchPath, "");
                unionFiles.Add(key, file);
            }
            foreach (var file in baseFiles)
            {
                var key = file.Replace(basePath, "");
                if (!unionFiles.ContainsKey(key))
                {
                    unionFiles.Add(key, file);
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
    }

}
