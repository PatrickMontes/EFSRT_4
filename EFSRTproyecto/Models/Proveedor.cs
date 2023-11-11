using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFSRTproyecto.Models
{
    public class Proveedor
    {
        [Display(Name = "ID")]
        public int IdProveedor { get; set; }

        [Display(Name = "Proveedor")]
        public string NombreProveedor { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
    }
}