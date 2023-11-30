using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelLibro
{
    public class Libro
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Nombre { get; set; }
        public string Autor { get; set; }
        public string Imagen { get; set; }
        public string TipoLibro { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Anio { get; set; }
        public string EditorialNombre { get; set; }
        public int EditorialId { get; set; }
        public int ID_Administrador { get; set; }
        
        public string Descripcion { get; set; }
        public List<int> CategoriaId { get; set; } = new List<int>();
        public string CategoriaIdString { get; set; }
        public byte[] Byteimagen { get; set;}

    }
}