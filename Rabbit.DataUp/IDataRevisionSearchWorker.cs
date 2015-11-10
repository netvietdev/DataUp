using Rabbit.DataUp.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Rabbit.DataUp
{
    public interface IDataRevisionSearchWorker
    {
        IEnumerable<IDataRevision> GetRemainingRevisions(IDbSet<Revision> systemRevisions, string tag, params Assembly[] assemblies);
    }
}