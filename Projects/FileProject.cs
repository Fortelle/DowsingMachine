using PBT.DowsingMachine.Utilities;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PBT.DowsingMachine.Projects;

// for gb - gba
public abstract class FileProject : DataProject
{
    [Option]
    [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
    public string SourceFile { get; set; }

    public FileProject() : base()
    {

    }

    protected class SingleReader : StreamBinaryReader
    {
        public SingleReader(int offset) : base("", offset)
        {
        }

        protected override Cache Open()
        {
            var path = ((FileProject)Project).SourceFile;
            var ms = File.OpenRead(path);
            var br = new BinaryReader(ms);
            return new Cache(ms, br);
        }
    }

    public override bool CheckValidity(out string error)
    {
        if (string.IsNullOrEmpty(SourceFile?.Trim()))
        {
            error = $"Property \"{nameof(SourceFile)}\" is not set.";
            return false;
        }

        if (!File.Exists(SourceFile))
        {
            error = $"Source file \"{SourceFile}\" not exists.";
            return false;
        }

        return base.CheckValidity(out error);
    }

    [Action("Open containing folder")]
    public void OpenContainingFolder()
    {
        if (File.Exists(SourceFile))
        {
            ProcessUtil.OpenContainingFolder(SourceFile);
        }
    }

}
