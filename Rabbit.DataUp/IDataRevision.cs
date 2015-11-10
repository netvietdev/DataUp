using System;

namespace Rabbit.DataUp
{
    public interface IDataRevision
    {
        bool Execute();

        Version VersionNumber { get; }
    }
}