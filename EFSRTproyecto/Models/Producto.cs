using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EFSRTproyecto.Models
{
    public class Producto
    {
        public int id { get; set; }

        [Display(Name = " Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Display(Name = "Categoría")]
        public Categoria categoria { get; set; } = new Categoria();

        [Display(Name = "Proveedor")]
        public Proveedor proveedor { get; set; } = new Proveedor();

    }
}