using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelCarritoCompra
{
    public class Alquiler
    {
        public int ID_TRANSACCION { get; set; }
        public DateTime FECHAALQUILER { get; set; }
        public int ID_USUARIO { get; set; }
        public string NOMBREUSUARIO { get; set; }
        public string NOMBRESLIBROS { get; set; }
        public DateTime FECHATERMINO { get; set; }
        public decimal TOTALPAGO { get; set; }
        public string ESTADO { get; set; }
        public string IMAGENLIBRO { get; set; }
    }
}