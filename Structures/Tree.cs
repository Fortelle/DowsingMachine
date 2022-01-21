namespace PBT.DowsingMachine.Structures
{
    public class Tree<T>
    {
        public List<Tree<T>> Folders;
        public List<Entry<T>> Files;

        public Tree()
        {
            Folders = new();
            Files = new();
        }
    }
}
