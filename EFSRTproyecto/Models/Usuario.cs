using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EFSRTproyecto.Models
{
    public class Usuario
    {
        public int id { get;set; }
        public string Nombres { get; set; }
        public string Apellidos { get;set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Dni { get; set; }

        [Required()]public Rol IdRol { get; set; }
    }
}