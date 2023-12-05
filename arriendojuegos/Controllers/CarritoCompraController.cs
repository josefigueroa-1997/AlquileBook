using arriendojuegos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace arriendojuegos.Controllers
{
    public class CarritoCompraController : Controller
    {
        // GET: CarritoCompra
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AgregarCarrito(int Idlibro, int idusuario, decimal totalproducto)
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                // Ejecutar el procedimiento almacenado y obtener el resultado escalar
                int resultado = dbcontext.Database.SqlQuery<int>(
                    "AGREGAR_CARRITO @ID_LIBRO, @ID_USUARIO, @TOTAL_PRODUCTOS",
                    new SqlParameter("@ID_LIBRO", Idlibro),
                    new SqlParameter("@ID_USUARIO", idusuario),
                    new SqlParameter("@TOTAL_PRODUCTOS", totalproducto)
                ).FirstOrDefault();
                Debug.WriteLine(resultado);
                // Verificar el resultado obtenido
                if (resultado == 1)
                {
                    return Json(new { success = false, message = "Solamente puede agregar un libro" });
                }
                else 
                {
                    return Json(new { success = true, message = "Se agregó con éxito el producto al carrito" });
                }
               
            }
        }

    }
}