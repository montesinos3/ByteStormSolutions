using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ByteStormApi.Models
{
    public class ByteStormContext : DbContext
    {
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Mision> Misiones { get; set; }
        public DbSet<Operativo> Operativos { get; set; }

        private IConfiguration _configuration;
        public ByteStormContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("ApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mision>()
            .HasOne<Operativo>(m => m.Operativo)
            .WithMany(o => o.Misiones)
            .HasForeignKey(m => m.IdOperativo);

            modelBuilder.Entity<Equipo>()
            .HasOne<Mision>(e => e.Mision)
            .WithMany(m => m.Equipos)
            .HasForeignKey(e => e.IdMision);
        }
    }
}

