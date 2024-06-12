using System.Data.Entity;

namespace ByteStormApi.Models
{
    public class MisionContext : DbContext
    {
        public DbSet<Mision> Misiones { get; set; } = null!;
    }
}

