using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelCarritoCompra
{
    public class Carrito
    {
        public int Id_Libro { get; set; }
        public int Id_Usuario { get; set; }
        public decimal Total_Productos { get; set; }
    }
}