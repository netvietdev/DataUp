using System.Linq;
using System.Reflection;

namespace Rabbit.DataUp
{
    public static class DataUpExecution
    {
        /// <summary>
        /// Initialize all data revisions from specified assemblies
        /// </summary>
        public static DataUpWorker Initialize(params Assembly[] assemblies)
        {
            return Initialize(Enumerable.Empty<string>().ToArray(), assemblies);
        }

        /// <summary>
        /// Initialize all data revisions from specified assemblies and tag
        /// </summary>
        public static DataUpWorker Initialize(string[] tags, params Assembly[] assemblies)
        {
            return new DataUpWorker(new DefaultDataUpHandlerSearchWorker(), tags, assemblies);
        }

        /// <summary>
        /// Initialize all data revisions from specified assemblies, tag
        /// </summary>
        public static DataUpWorker Initialize(IDataUpHandlerSearchWorker dataUpHandlerSearchWorker, string[] tags, params Assembly[] assemblies)
        {
            return new DataUpWorker(dataUpHandlerSearchWorker, tags, assemblies);
        }
    }
}
