using System.Data.Entity;

namespace ByteStormApi.Models
{
    public class EquipoContext : DbContext
    {
        public DbSet<Equipo> Equipos { get; set; } = null!;
    }
}

