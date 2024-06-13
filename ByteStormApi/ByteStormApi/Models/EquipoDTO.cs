﻿using System.ComponentModel.DataAnnotations;

namespace ByteStormApi.Models
{
    public class EquipoDTO
    {
        [Key]
        public int Id { get; set; }
        public string? Tipo{ get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public string? Descripcion { get; set; }
        public string? Estado { get; set; } //cambiar por enum !!!!!!!!!!!!!!!!!
        public long? IdMision { get; set; }
    }
}
