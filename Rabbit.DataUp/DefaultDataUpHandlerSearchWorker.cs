using Rabbit.DataUp.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Rabbit.DataUp
{
    internal class DefaultDataUpHandlerSearchWorker : IDataUpHandlerSearchWorker
    {
        public IEnumerable<IDataUpHandler> GetRemainingRevisions(IDbSet<Revision> systemRevisions, string[] tags, params Assembly[] assemblies)
        {
            var dataRevisions = from assembly in assemblies
                                from revisionType in assembly.GetTypes()
                                                             .Where(x => x.IsPublic &&
                                                                         !x.IsAbstract &&
                                                                         typeof(IDataUpHandler).IsAssignableFrom(x))
                                select (IDataUpHandler)Activator.CreateInstance(revisionType);

            // Filter revisions by tag
            dataRevisions = (tags.Length == 0)
                ? dataRevisions.Where(x => !x.Tags.Any())
                : dataRevisions.Where(x => !x.Tags.Any() || x.Tags.Intersect(tags, StringComparer.InvariantCultureIgnoreCase).Any());

            // Get remaining revisions
            var revisionList = dataRevisions.ToList();
            var executedRevisions = FindExecutedRevisions(systemRevisions, revisionList);
            var remainingRevisions = revisionList.Except(executedRevisions);

            return remainingRevisions;
        }

        private IEnumerable<IDataUpHandler> FindExecutedRevisions(IDbSet<Revision> systemRevisions, IEnumerable<IDataUpHandler> allRevisions)
        {
            return from dataRevision in allRevisions
                   let revisionType = dataRevision.GetType().FullName
                   where systemRevisions.Any(x => revisionType.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                   select dataRevision;
        }
    }
}
