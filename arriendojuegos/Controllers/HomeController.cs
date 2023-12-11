using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelLibro;
using arriendojuegos.Filters;
using arriendojuegos.Services;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    [ActualizarEstadoAlquiler]
    public class HomeController : Controller
    {
        private readonly Libroservice libroservice = new Libroservice();
        public ActionResult Index(int? id, string tipolibro, int? idcategoria, int? anio,string nombre)
        {
            var libro = libroservice.ObtenerLibros(id,tipolibro,idcategoria,anio,nombre);
            var recomendaciones = RecomendacionesLibroUsuario(id);
            return View(libro);
        }

        private List<Libro> RecomendacionesLibroUsuario(int? id)
        {
            List<Libro> recomendaciones = null;
            using(var dbcontext = new arriendojuegosEntities1())
            {
                recomendaciones = dbcontext.Database.SqlQuery<Libro>("RECOMENDACIONESLIBROSPARATI @IDUSUARIO",
                    new SqlParameter("@IDUSUARIO",id)).ToList();
            }
            return recomendaciones;
        }
     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}