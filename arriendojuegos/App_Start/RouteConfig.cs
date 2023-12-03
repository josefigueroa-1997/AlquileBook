using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace arriendojuegos
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteBook",
                url: "Libro/DeleteBook/{id}",
                defaults: new { controller = "Libro", action = "DeleteBook", id = UrlParameter.Optional }
                );
            routes.MapRoute(
            name: "LibroRoute",
            url: "Libro/Libros",
            defaults: new { controller = "Libro", action = "Libros" }
        );
        }
    }
}
