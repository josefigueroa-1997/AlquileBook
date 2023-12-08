using arriendojuegos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using arriendojuegos.Models.ListModelCarritoCompra;
using System.Web.Services.Description;
using arriendojuegos.Models.ListModelLibro;
using arriendojuegos.Filters;
using Newtonsoft.Json;

namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    public class CarritoCompraController : Controller
    {
        // GET: CarritoCompra
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AgregarCarrito(int Idlibro, int idusuario)
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
               
                int resultado = dbcontext.Database.SqlQuery<int>(
                    "AGREGAR_CARRITO @ID_LIBRO, @ID_USUARIO",
                    new SqlParameter("@ID_LIBRO", Idlibro),
                    new SqlParameter("@ID_USUARIO", idusuario)
                    
                ).FirstOrDefault();
                
                if (resultado == 1)
                {
                    return Json(new { success = false, message = "Solamente puede agregar una copia de este producto al carrito" });
                }
                else 
                {

                    var carrito = Obtenercarrito(idusuario);
                    AlmacenarCarritoEnCookies(carrito);
                    return Json(new { success = true, message = "Se agregó con éxito el producto al carrito" });
                }
               
            }
        }


        public ActionResult CargarCarrito(int id)
        {
           var carrito = Obtenercarrito(id); ;
            
            
            if (carrito != null)
            {
                AlmacenarCarritoEnCookies(carrito);
                return Json(new { success = true,carrito = carrito },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {success = false, message = "Error al cargar el carrito"},JsonRequestBehavior.AllowGet);
            }
            
        }
        private List<Carrito> Obtenercarrito(int id)
        {
            try
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    var resultado = dbcontext.Database.SqlQuery<Carrito>("OBTENERCARRITO @IDUSUARIO",
                        new SqlParameter("@IDUSUARIO", id)).ToList();
                    foreach (var libro in resultado)
                    {
                        if (!string.IsNullOrEmpty(libro.IMAGENLIBRO))
                        {
                            libro.imagen = Convert.FromBase64String(libro.IMAGENLIBRO);
                        }
                    }

                    return resultado;
                }


            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }


        }
        public void AlmacenarCarritoEnCookies(List<Carrito> carrito)
        {
            // Convertir el carrito a una cadena JSON y almacenarlo en una cookie
            var carritoJson = JsonConvert.SerializeObject(carrito);
            Response.Cookies["Carrito"].Value = carritoJson;
        }

        // Obtener el carrito desde cookies
        public List<Carrito> ObtenerCarritoDesdeCookies()
        {
            var carritoJson = Request.Cookies["Carrito"]?.Value;

            if (!string.IsNullOrEmpty(carritoJson))
            {
                return JsonConvert.DeserializeObject<List<Carrito>>(carritoJson);
            }

            return new List<Carrito>();
        }
        [HttpPost]
        public ActionResult EliminarLibroCarrito(int idlibro, int idusuario)
        {
            try
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    var resultado = dbcontext.Database.ExecuteSqlCommand("DELETELIBROCARRITO @IDLIBRO,@IDUSUARIO",
                        new SqlParameter("@IDLIBRO",idlibro),
                        new SqlParameter("@IDUSUARIO",idusuario));
                    return Json(new {success = true});
                }
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Error", "Shared");
            }
        }

        public ActionResult ResumenAlquiler(int idusuario)
        {
            if (Session["id"]!=null )
            {
                var carrito = Obtenercarrito(idusuario);
                ViewBag.Carrito = carrito;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        [HttpPost]
        public ActionResult EfectuarAlquiler()
        {
            try
            {
                using(var dbcontext = new arriendojuegosEntities1())
                {

                    int iduser = int.Parse(Session["id"].ToString());
                    decimal total = decimal.Parse(Request.Form["total"]);
                    string[] idlibros = Request.Form.AllKeys.Where(key => key.StartsWith("idlibros[")).Select(key => Request.Form[key]).ToArray();
                    string librosid = string.Join(",",idlibros);
                    int semanasSeleccionadas = int.Parse(Request.Form["fechaalquiler"]);
                    DateTime fechaAlquiler = DateTime.Now;

                    for (int i = 0; i < semanasSeleccionadas; i++)
                    {
                        fechaAlquiler = fechaAlquiler.AddDays(5); 
                        if (fechaAlquiler.DayOfWeek == DayOfWeek.Friday)
                        {
                            fechaAlquiler = fechaAlquiler.AddDays(3); 
                        }
                    }
                    var resultado = dbcontext.Database.ExecuteSqlCommand("EFECTUARALQUILER @ID_USUARIO,@LIBROS_ID,@TOTAL,@INFORMACION,@FECHATERMINO",
                        new SqlParameter("@ID_USUARIO",iduser),
                        new SqlParameter("@LIBROS_ID",librosid),
                        new SqlParameter("@TOTAL",total),
                        new SqlParameter("@INFORMACION","Se realizó un alquiler de manera exitosa"),
                        new SqlParameter("@FECHATERMINO",fechaAlquiler));
                }
                return RedirectToAction("Success","CarritoCompra");
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Error", "Shared");
            }

        }

        


    }
}