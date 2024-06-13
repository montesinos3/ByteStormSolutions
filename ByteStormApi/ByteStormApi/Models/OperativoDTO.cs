using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public class OperativoDTO {
        [Key]
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public string? Rol { get; set; }

        public virtual List<long>? Misiones { get; set; }
    }
}
