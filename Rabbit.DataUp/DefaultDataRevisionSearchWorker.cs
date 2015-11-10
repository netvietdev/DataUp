using Rabbit.DataUp.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Rabbit.DataUp
{
    public class DefaultDataRevisionSearchWorker : IDataRevisionSearchWorker
    {
        public IEnumerable<IDataRevision> GetRemainingRevisions(IDbSet<Revision> systemRevisions, string tag, params Assembly[] assemblies)
        {
            var dataRevisions = from assembly in assemblies
                                from revisionType in assembly.GetTypes()
                                                             .Where(x => x.IsPublic &&
                                                                         !x.IsAbstract &&
                                                                         typeof(IDataRevision).IsAssignableFrom(x))
                                select (IDataRevision)Activator.CreateInstance(revisionType);

            // Filter revisions by tag
            dataRevisions = string.IsNullOrWhiteSpace(tag)
                ? dataRevisions.Where(x => !x.Tags.Any())
                : dataRevisions.Where(x => x.Tags.Contains(tag, StringComparer.InvariantCultureIgnoreCase));

            // Get remaining revisions
            var revisionList = dataRevisions.ToList();
            var executedRevisions = FindExecutedRevisions(systemRevisions, revisionList);
            var remainingRevisions = revisionList.Except(executedRevisions);

            return remainingRevisions;
        }

        private IEnumerable<IDataRevision> FindExecutedRevisions(IDbSet<Revision> systemRevisions, IEnumerable<IDataRevision> allRevisions)
        {
            return from dataRevision in allRevisions
                   let revisionType = dataRevision.GetType().FullName
                   where !systemRevisions.Any(x => revisionType.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                   select dataRevision;
        }
    }
}