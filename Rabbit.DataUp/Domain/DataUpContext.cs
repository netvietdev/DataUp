using System.Configuration;
using System.Data.Entity;

namespace Rabbit.DataUp.Domain
{
    public class DataUpContext : DbContext
    {
        public DataUpContext()
            : base(ConfigurationManager.ConnectionStrings["DataUp"].ConnectionString)
        {
        }

        public IDbSet<Revision> Revisions { get; set; }
    }
}