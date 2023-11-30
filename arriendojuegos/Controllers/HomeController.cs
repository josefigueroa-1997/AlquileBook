using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelLibro;
using arriendojuegos.Filters;
namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            
            return View();
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