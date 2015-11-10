using Rabbit.DataUp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rabbit.DataUp
{
    public class DataUpWorker
    {
        private readonly string _tag;
        private readonly Assembly[] _assemblies;
        private readonly DataUpContext _dbContext;

        internal DataUpWorker(string tag, params Assembly[] assemblies)
        {
            _tag = tag;
            _assemblies = assemblies;
            _dbContext = new DataUpContext();
        }

        public IList<string> PerformUpdate()
        {
            var revisions = GetDataRevisions();

            var executedTypes = new List<string>();

            foreach (var dataRevision in revisions)
            {
                var revisionType = dataRevision.GetType().FullName;

                if (_dbContext.Revisions.Any(x => x.Type.Equals(revisionType)))
                {
                    continue;
                }

                dataRevision.Execute();

                _dbContext.Revisions.Add(new Revision()
                    {
                        IntegratedDate = DateTime.Now,
                        Type = revisionType
                    });

                executedTypes.Add(revisionType);
            }

            _dbContext.SaveChanges();

            return executedTypes;
        }

        private IEnumerable<IDataRevision> GetDataRevisions()
        {
            var dataRevisions = from assembly in _assemblies
                                from revisionType in assembly.GetTypes()
                                                             .Where(x => x.IsPublic &&
                                                                         !x.IsAbstract &&
                                                                         typeof(IDataRevision).IsAssignableFrom(x))
                                select (IDataRevision)Activator.CreateInstance(revisionType);

            if (!string.IsNullOrWhiteSpace(_tag))
            {
                dataRevisions =
                    dataRevisions.Where(x => x.Tags.Contains(_tag, StringComparer.InvariantCultureIgnoreCase));
            }

            return dataRevisions.OrderBy(x => x.VersionNumber);
        }
    }
}
