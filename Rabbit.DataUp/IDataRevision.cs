using System;
using System.Collections.Generic;

namespace Rabbit.DataUp
{
    public interface IDataRevision
    {
        bool Execute();

        Version VersionNumber { get; }

        IEnumerable<string> Tags { get; }
    }
}