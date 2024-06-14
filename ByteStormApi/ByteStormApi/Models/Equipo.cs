using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public enum Estado { Disponible, EnUso };
    public enum Tipo { Software, Hardware };
    public class Equipo
    {
        [Key]
        public long Id { get; set; }
        public Tipo? Tipo { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public string? Descripcion { get; set; }
        public Estado? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!


        public long? IdMision { get; set; }
        public virtual Mision? Mision { get; set; }
    }
}
