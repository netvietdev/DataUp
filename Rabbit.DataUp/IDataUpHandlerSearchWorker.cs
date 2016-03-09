using Rabbit.DataUp.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Rabbit.DataUp
{
    public interface IDataUpHandlerSearchWorker
    {
        IEnumerable<IDataUpHandler> GetRemainingRevisions(IDbSet<Revision> systemRevisions, string[] tags, params Assembly[] assemblies);
    }
}