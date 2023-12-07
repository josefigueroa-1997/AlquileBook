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
                    return Json(new { success = true, message = "Se agregó con éxito el producto al carrito" });
                }
               
            }
        }


        public ActionResult CargarCarrito(int id)
        {
            Session["carrito"] = Obtenercarrito(id); ;
           //var carrito = 
            
            if (Session["carrito"] != null)
            {
                
                return Json(new { success = true,carrito = Session["carrito"] },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {success = false, message = "Error al cargar el carrito"},JsonRequestBehavior.AllowGet);
            }
            
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

        private List<Carrito> Obtenercarrito(int id)
        {
            try 
            { 
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    var resultado = dbcontext.Database.SqlQuery<Carrito>("OBTENERCARRITO @IDUSUARIO",
                        new SqlParameter("@IDUSUARIO",id)).ToList();
                    foreach(var libro in resultado)
                    {
                        if (!string.IsNullOrEmpty(libro.IMAGENLIBRO))
                        {
                            libro.imagen = Convert.FromBase64String(libro.IMAGENLIBRO);
                        }
                    }
                    
                    return resultado;
                }
            
            
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }


        }

        

    }
}