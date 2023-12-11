using System.Web;
using System.Web.Mvc;
using arriendojuegos.Filters;
namespace arriendojuegos
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CargarCategorias());
            filters.Add(new ActualizarEstadoAlquiler());
        }
    }
}
