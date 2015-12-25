using System;

namespace Rabbit.DataUp.Domain
{
    public class Release
    {
        public Release()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// PK field
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Fullname of IDataRevision class
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The date that this release is planned
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The date inwhich the revision has been executed
        /// </summary>
        public DateTime IntegratedDate { get; set; }
    }
}