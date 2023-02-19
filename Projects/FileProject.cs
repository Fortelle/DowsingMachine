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

    [Action]
    public void OpenContainingFolder()
    {
        if (File.Exists(SourceFile))
        {
            ProcessUtil.OpenContainingFolder(SourceFile);
        }
    }

    public override bool CheckValidity(bool isNew, out string error)
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

        return base.CheckValidity(isNew, out error);
    }

    protected override object ReadReference(IDataReference reference, GetDataOptions options)
    {
        switch (reference)
        {
            case PosRef pr:
                var br = GetData(GetReader);
                br.BaseStream.Position = pr.Position;
                return br;
        }

        return base.ReadReference(reference, options);
    }

    private BinaryReader GetReader()
    {
        var ms = File.OpenRead(SourceFile);
        return new BinaryReader(ms);
    }

    protected class PosRef : DataRef<BinaryReader>
    {
        public uint Position { get; set; }

        public override string Description => $"0x{Position:X8}";

        public PosRef(uint position)
        {
            Position = position;
        }
    }

}
