using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public class Mision
    {
        [Key]
        public long Id { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!

        public virtual List<Equipo>? Equipos { get; set; }

        public long? IdOperativo { get; set; }
        public virtual Operativo? Operativo { get; set; }    
    }
}
