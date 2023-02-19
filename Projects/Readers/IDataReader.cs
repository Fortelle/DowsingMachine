namespace PBT.DowsingMachine.Projects;

public interface IDataReader<TIn, TOut>
{
    public TOut Read(TIn input);
}
