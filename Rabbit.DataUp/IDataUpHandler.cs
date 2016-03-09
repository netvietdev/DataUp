using System;
using System.Collections.Generic;

namespace Rabbit.DataUp
{
    public interface IDataUpHandler
    {
        bool Execute();

        Version VersionNumber { get; }

        IEnumerable<string> Tags { get; }
    }
}