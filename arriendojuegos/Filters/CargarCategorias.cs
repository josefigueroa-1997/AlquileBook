using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelLibro;

namespace arriendojuegos.Filters
{
    public class CargarCategorias : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var categorias = ObtenerCategorias();
            filterContext.Controller.ViewBag.Categorias = categorias;
        }

        public List<Categoria> ObtenerCategorias()
        {
            using (var dbContext = new arriendojuegosEntities1())
            {
                try
                {
                    var categorias = dbContext.Database.SqlQuery<Categoria>("OBTENERCATEGORIAS").ToList();
                    return categorias;
                }
                catch (Exception ex)
                {
                    
                    System.Diagnostics.Debug.WriteLine(ex);
                    return null;
                }
            }
        }

    }
}