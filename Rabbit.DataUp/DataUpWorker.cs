using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rabbit.DataUp.Domain;

namespace Rabbit.DataUp
{
    public class DataUpWorker
    {
        private readonly Assembly[] _assemblies;
        private readonly DataUpContext _dbContext;

        internal DataUpWorker(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
            _dbContext = new DataUpContext();
        }

        public IList<string> PerformUpdate()
        {
            var executedTypes = new List<string>();

            var revisions = GetRemainingDataRevisions();

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

        private IEnumerable<IDataRevision> GetRemainingDataRevisions()
        {
            return from assembly in _assemblies
                   from revisionType in assembly.GetTypes()
                                                .Where(x => x.IsPublic &&
                                                            !x.IsAbstract &&
                                                            typeof(IDataRevision).IsAssignableFrom(x))
                   select (IDataRevision)Activator.CreateInstance(revisionType);
        }
    }
}