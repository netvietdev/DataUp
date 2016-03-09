using Rabbit.DataUp.Domain;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rabbit.DataUp
{
    public class DataUpWorker
    {
        private readonly string[] _tags;
        private readonly Assembly[] _assemblies;
        private readonly DataUpContext _dbContext;
        private readonly IDataUpHandlerSearchWorker _dataUpHandlerSearchWorker;

        internal DataUpWorker(IDataUpHandlerSearchWorker dataUpHandlerSearchWorker, string[] tags, params Assembly[] assemblies)
        {
            _tags = tags;
            _assemblies = assemblies;
            _dataUpHandlerSearchWorker = dataUpHandlerSearchWorker;
            _dbContext = new DataUpContext();
        }

        public IList<string> PerformUpdate()
        {
            var revisions = _dataUpHandlerSearchWorker.GetRemainingRevisions(_dbContext.Revisions, _tags, _assemblies);
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
