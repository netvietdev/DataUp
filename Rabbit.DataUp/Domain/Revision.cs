using System;

namespace Rabbit.DataUp.Domain
{
    public class Revision
    {
        public Revision()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public string Type { get; set; }

        public DateTime IntegratedDate { get; set; }
    }
}
