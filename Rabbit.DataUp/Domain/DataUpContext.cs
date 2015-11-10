using System.Data.Entity;

namespace Rabbit.DataUp.Domain
{
    public class DataUpContext : DbContext
    {
        public DataUpContext()
            : base("DataUp")
        {
        }

        public IDbSet<Revision> Revisions { get; set; }
    }
}