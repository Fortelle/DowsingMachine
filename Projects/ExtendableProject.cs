using PBT.DowsingMachine.Structures;

namespace PBT.DowsingMachine.Projects;

public abstract class ExtendableProject : FolderProject
{
    [Option]
    public Version Version { get; set; }

    [Option]
    public bool IsExtendable { get; set; }

    public override string Description => Version?.ToString();

    public override void Configure()
    {
    }

    private ExtendableProject? _originalProject;
    protected ExtendableProject? OriginalProject
    {
        get
        {
            if (!IsExtendable) return null;
            if (!IsWorking) return null;
            if(_originalProject == null)
            {
                _originalProject = DowsingMachineApp.Projects
                    .OfType<ExtendableProject>()
                    .Where(y => y != this)
                    .Where(y => y.GetType() == GetType())
                    .Where(y => y.IsExtendable == false)
                    .Where(y => y.Version < Version)
                    .MaxBy(y => y.Version)
                    ;
                if (_originalProject is null)
                {
                    throw new ProjectNotFoundException();
                }
                _originalProject.Active();
                _originalProject.BeginWork();
            }
            return _originalProject;
        }
    }

    public override void EndWork()
    {
        base.EndWork();

        _originalProject?.EndWork();
        _originalProject = null;
    }

    public override bool CheckValidity(bool isNew, out string error)
    {
        return base.CheckValidity(isNew, out error);
    }


    protected T FindPreviousVersion<T>() where T : ExtendableProject
    {
        var project = DowsingMachineApp.Projects
            .Where(x => x.GetType() == GetType())
            .Cast<T>()
            .Where(x => x.Version < Version)
            .MaxBy(x => x.Version)
            ;
        if (project is null)
        {
            throw new ProjectNotFoundException();
        }
        return (T)project;
    }

    protected override object ReadReference(IDataReference reference, GetDataOptions args)
    {
        switch (reference)
        {
            case FileRef when IsExtendable:
                {
                    var ret = base.ReadReference(reference, args);
                    if (ret == null)
                    {
                        ret = OriginalProject.ReadReference(reference, args);
                    }
                    return ret;
                }
            case FilesRef filesRef when IsExtendable:
                {
                    var list = new List<PathInfo>();
                    var patchFiles = (PathInfo[])base.ReadReference(reference, args);
                    if (patchFiles != null)
                    {
                        list.AddRange(patchFiles);
                    }

                    var originFiles = (PathInfo[])OriginalProject.ReadReference(reference, args);
                    if (originFiles != null)
                    {
                        foreach (var originFile in originFiles)
                        {
                            if (!list.Any(x => x.RelativePart == originFile.RelativePart))
                            {
                                list.Add(originFile);
                            }
                        }
                    }

                    return list
                        .OrderBy(x => x.SortIndex)
                        .ThenBy(x => x.RelativePart)
                        .ThenBy(x => x.FullPath)
                        .ToArray();
                }
        }

        return base.ReadReference(reference, args);
    }

}
