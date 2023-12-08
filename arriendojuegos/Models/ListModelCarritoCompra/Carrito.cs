using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelCarritoCompra
{
    public class Carrito
    {
        public int Id_Usuario { get; set; }
        public int Id_Libro { get; set; }
        public string NOMBRELIBRO { get; set; }
        public string AUTOR { get; set; }
        public string IMAGENLIBRO { get; set; }
        public decimal PRECIOLIBRO { get; set; }
        public string TIPOLIBRO { get; set; }
        public string DIRECCION { get; set; }
        public string COMUNA { get; set; }
        public byte[] imagen { get; set; }
    }
}