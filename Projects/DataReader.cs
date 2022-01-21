using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBT.DowsingMachine.Projects
{
    public abstract class DataReader<TOut> : IDataReader
    {
        public string Name { get; set; }
        public string RelatedPath { get; set; }
        public virtual DataProject Project { get; set; }
        public bool UseCache { get; set; }

        protected abstract TOut Read();

        public virtual object[] Open() => null;
        public virtual object GetContent() => Read();

        protected DataReader (string path)
        {
            RelatedPath = path;
        }

        protected object[] GetCache()
        {
            if(!UseCache)
            {
                var cache = Open();
                return cache;
            }

            if (!Project.Caches.ContainsKey(Name))
            {
                var cache = Open();
                Project.Caches.Add(Name, cache);
            }

            return Project.Caches[Name];
        }
    }

}
