using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelCarritoCompra
{
    public class Boleta
    {
        public int IDBOLETA { get; set; }
        public int IDTRANSACCION { get; set; }
        public decimal TOTALTRANSACCION { get; set; }
        public int  IDUSUARIO { get; set; }
        public string NOMBREUSUARIO { get; set; }
        public DateTime FECHAEMISION { get; set; }
        public string NOMBRESLIBROS { get; set; }
        public string IDSLIBROS { get; set; }
    }
}