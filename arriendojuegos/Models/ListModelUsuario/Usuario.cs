using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace arriendojuegos.Models.ListModelUsuario
{
    public class Usuario
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public int telefono { get; set; }
        public string direccion { get; set; }
        public string comunanombre { get; set; }


    }
}