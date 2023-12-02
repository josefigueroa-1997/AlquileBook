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
namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    public class HomeController : Controller
    {
        private readonly Libroservice libroservice = new Libroservice();
        public ActionResult Index(int? id, string tipolibro, int? idcategoria, int? anio)
        {
            var libro = libroservice.ObtenerLibros(id,tipolibro,idcategoria,anio);
            return View(libro);
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