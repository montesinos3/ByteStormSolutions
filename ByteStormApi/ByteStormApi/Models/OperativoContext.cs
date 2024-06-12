using System.Data.Entity;

namespace ByteStormApi.Models
{
    public class OperativoContext : DbContext
    {
        public DbSet<Operativo> Operativos { get; set; } = null!;
    }
}

