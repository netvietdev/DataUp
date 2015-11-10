using Rabbit.DataUp.Domain;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rabbit.DataUp
{
    public class DataUpWorker
    {
        private readonly string _tag;
        private readonly Assembly[] _assemblies;
        private readonly DataUpContext _dbContext;
        private readonly IDataRevisionSearchWorker _dataRevisionSearchWorker;

        internal DataUpWorker(string tag, params Assembly[] assemblies)
        {
            _tag = tag;
            _assemblies = assemblies;
            _dbContext = new DataUpContext();
            _dataRevisionSearchWorker = new DefaultDataRevisionSearchWorker();
        }

        public IList<string> PerformUpdate()
        {
            var revisions = _dataRevisionSearchWorker.GetRemainingRevisions(_dbContext.Revisions, _tag, _assemblies);
            var executedTypes = new List<string>();

            foreach (var dataRevision in revisions)
            {
                dataRevision.Execute();

                var revisionType = dataRevision.GetType().FullName;

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
    }
}
