using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public enum EstadoM { Planificada, Activa, Completada };
    public class Mision
    {
        [Key]
        public long Id { get; set; }
        public string? Descripcion { get; set; }
        public EstadoM? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!

        public virtual List<Equipo>? Equipos { get; set; }

        public long? IdOperativo { get; set; }
        public virtual Operativo? Operativo { get; set; }    
    }
}
