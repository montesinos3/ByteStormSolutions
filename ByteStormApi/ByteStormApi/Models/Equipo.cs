namespace ByteStormApi.Models
{
    public class Equipo
    {
        public string? Tipo{ get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public string? Descripcion { get; set; }
        public string? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!


        public long IdMision { get; set; }
        public virtual Mision Mision{ get; set; }
    }
}
