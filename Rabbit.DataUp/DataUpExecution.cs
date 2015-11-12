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
            return Initialize(string.Empty, assemblies);
        }

        /// <summary>
        /// Initialize all data revisions from specified assemblies and tag
        /// </summary>
        public static DataUpWorker Initialize(string tag, params Assembly[] assemblies)
        {
            return new DataUpWorker(tag, new DefaultDataRevisionSearchWorker(), assemblies);
        }

        /// <summary>
        /// Initialize all data revisions from specified assemblies, tag
        /// </summary>
        public static DataUpWorker Initialize(string tag, IDataRevisionSearchWorker dataRevisionSearchWorker, params Assembly[] assemblies)
        {
            return new DataUpWorker(tag, dataRevisionSearchWorker, assemblies);
        }
    }
}
