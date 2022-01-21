using PBT.DowsingMachine.UI;
using System.Collections;

namespace PBT.DowsingMachine.Structures
{
    public class WorkProcesser
    {
        private string Name { get; init; }

        public int Size;

        public IEnumerable Queue { get; set; }

        public WorkProcesser()
        {
        }

        public WorkProcesser(string name) : this()
        {
            Name = name;
        }

        public void ShowDialog(bool autoStart = false)
        {
            var form = new ProgressDialog(this, autoStart);
            form.ShowDialog();
        }

        public void Set<T>(IEnumerable<T> source)
        {
            Queue = source;
            Size = source.TryGetNonEnumeratedCount(out int size) ? size : 0;
        }

        public void Set<T>(IEnumerable<T> source, int size)
        {
            Queue = source;
            Size = size;
        }

        public void Set<T>(T[] source, Action<T> action)
        {
            Queue = source.Select(x =>
            {
                action(x);
                return x;
            });
            Size = source.Length;
        }

        public void Set(params Func<Task>[] tasks)
        {
            Queue = tasks;
            Size = tasks.Length;
        }

        public void Test(int count = 1)
        {
            var i = 0;
            foreach(var q in Queue)
            {
                i++;
                if (i == count) break;
            }
        }

    }
}
