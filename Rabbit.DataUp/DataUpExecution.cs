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
            return new DataUpWorker(assemblies);
        }
    }
}