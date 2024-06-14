using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public class EquipoDTO
    {
        [Key]
        public long Id { get; set; }
        public Tipo? Tipo{ get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public string? Descripcion { get; set; }
        public Estado? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public long? IdMision { get; set; }
    }
}
