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
using System.Diagnostics;

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
            if (Session["id"] != null)
            {
                var recomendaciones = RecomendacionesLibroUsuario((int)Session["id"]);
                if (recomendaciones != null)
                {
                    ViewBag.Recomendaciones = recomendaciones;
                }
            }
            var populares = LibrosMasAlquilados();
            ViewBag.Populares = populares;
            return View(libro);
        }

        private List<Libro> RecomendacionesLibroUsuario(int id)
        {
            List<Libro> recomendaciones = null;
            using(var dbcontext = new arriendojuegosEntities1())
            {
                recomendaciones = dbcontext.Database.SqlQuery<Libro>("RECOMENDACIONESLIBROSPARATI @IDUSUARIO",
                    new SqlParameter("@IDUSUARIO",id)).ToList();
            }
            return recomendaciones;
        }

        private List<Libro> LibrosMasAlquilados()
        {
            List<Libro> librospopulares = null;
            
            using (var dbcontext = new arriendojuegosEntities1())
            {
                librospopulares = dbcontext.Database.SqlQuery<Libro>("LIBROSMASALQUILADOS").ToList();
            }
            return librospopulares;
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