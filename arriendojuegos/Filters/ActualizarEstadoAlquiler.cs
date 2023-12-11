using arriendojuegos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace arriendojuegos.Filters
{
    public class ActualizarEstadoAlquiler : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
            ActualizarEstado();
            base.OnActionExecuted(filterContext);
        }

        public void ActualizarEstado()
        {
            using(var dbcontext = new arriendojuegosEntities1())
            {
                var resultado = dbcontext.ACTUALIZARESTADOALQUILER();
            }
        }



    }
}