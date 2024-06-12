namespace ByteStormApi.Models
{
    public class Operativo { 
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public bool Rol { get; set; }

        public virtual List<Mision?> Misiones { get; set; }
    }
}
