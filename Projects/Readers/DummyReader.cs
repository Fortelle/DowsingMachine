using PBT.DowsingMachine.Projects;

namespace PBT.DowsingMachine.Pokemon.Games;

public class DummyReader : DataReader<string>
{
    public DummyReader() : base("")
    {
    }

    protected override string Open()
    {
        return null;
    }
}
