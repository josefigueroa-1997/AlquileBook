using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelLibro;
using arriendojuegos.Controllers;
using arriendojuegos.Filters;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices.ComTypes;

namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    
    public class LibroController : Controller
    {
        
        // GET: Libro
        public ActionResult Libros(int?id)
        {
            if (Session["nombre"]!= null)
            {
                var libros = ObtenerLibros(id);
                ViewBag.Libros = libros;
                return View();
            }
            else
            {
                return RedirectToAction("Login","Usuario");
            }
            
        }

        
        public ActionResult Detalle(int? id)
        {
            if (id == null || id<0)
            {
                return RedirectToAction("Libros");
            } 
            var libro = ObtenerLibros(id);
            var editorial = obtenereditorial();
            var cateogiras = obtenercategorias();
            ViewBag.Editoriales = editorial;
            ViewBag.Categorias = cateogiras;
            return View(libro);
        }


        public ActionResult Registrar()
        {
            if (Session["nombre"] != null)
            {
                var editorial = obtenereditorial();
                var cateogiras = obtenercategorias();
                ViewBag.Editoriales = editorial;
                ViewBag.Categorias = cateogiras;
                return View();

            }
            else
            {
                return RedirectToAction("Login","Usuario");
            }
            
        }

        [HttpPost]
        public ActionResult RegistrarLibro()
        {
            try
            {
                 using (var dbcontext = new arriendojuegosEntities1())
                 {
                    string imagen = obtenerimagen(Request.Files["imagen"]);
                     var idlibro = new SqlParameter("@IDLIBRO", SqlDbType.Int);
                     idlibro.Direction = ParameterDirection.Output;
                     int idadmin = int.Parse(Session["id"].ToString());
                     int stock = int.Parse(Request.Form["Stock"]);
                     decimal precio = decimal.Parse(Request.Form["Precio"]);
                     int anio = int.Parse(Request.Form["Anio"]);
                     int editorial = int.Parse(Request.Form["Editorial"]);
                     string[] categoriaseleccionadas = Request.Form.GetValues("Categorias[]");
                     string categoriaidlist = categoriaseleccionadas != null ? string.Join(",", categoriaseleccionadas) : "";
                     var categoriaIds = categoriaseleccionadas?.Select(int.Parse).ToList();
                 
                    if (categoriaIds != null && categoriaIds.Any())
                     {
                         
                         var categoriasExistentes = dbcontext.CATEGORIA.Count(c => categoriaIds.Contains(c.ID));

                         if (categoriasExistentes != categoriaIds.Count)
                         {
                             return RedirectToAction("Error", "Shared");
                         }
                     }
                     var parameters = new List<SqlParameter>
                     {
                        new SqlParameter("@ISBN", Request.Form["ISBN"]),
                        new SqlParameter("@NOMBRE", Request.Form["Nombre"]),
                        new SqlParameter("@AUTOR", Request.Form["Autor"]),
                        new SqlParameter("@TIPOLIBRO", Request.Form["Tipolibro"]),
                        new SqlParameter("@PRECIO", precio),
                         new SqlParameter("@STOCK", stock),
                         new SqlParameter("@ANIO", anio),
                         new SqlParameter("@EDITORIAL", editorial),
                        new SqlParameter("@ID_ADMINISTRADOR", idadmin),
                        new SqlParameter("@DESCRIPCION", Request.Form["descripcion"]),
                         idlibro,
                         new SqlParameter("@CATEGORIAS_ID_LIST", categoriaidlist),
                        new SqlParameter("@IMAGEN", imagen)
                     };

                     var parametersArray = parameters.ToArray();
                     var resultado = dbcontext.Database.ExecuteSqlCommand("REGISTRARLIBRO @ISBN, @NOMBRE, @AUTOR, @TIPOLIBRO, @PRECIO, @STOCK, @ANIO, @EDITORIAL, @ID_ADMINISTRADOR, @DESCRIPCION, @IDLIBRO OUTPUT, @CATEGORIAS_ID_LIST, @IMAGEN", parametersArray);

                     int idlibrosalida = idlibro.Value != DBNull.Value ? (int)idlibro.Value : 0;
                     return RedirectToAction("Libros");

                 }
             }
             catch (SqlException e)
             {
                Debug.WriteLine($"Error SQL: {e.Number} - {e.Message}");
                Debug.WriteLine($"Error SQL: {e.Message}");

                 return RedirectToAction("Error", "Shared");
             }
                
        }


        [HttpPost]
        public ActionResult Actualizarlibro()
        {
           
            try
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    string imagen = obtenerimagen(Request.Files["imagen"]);
                    int idadmin = int.Parse(Session["id"].ToString());
                    int stock = int.Parse(Request.Form["Stock"]);
                    decimal precio = decimal.Parse(Request.Form["Precio"]);
                    int anio = int.Parse(Request.Form["Anio"]);
                  int editorial = int.Parse(Request.Form["Editorial"]);
                    int idlibro = int.Parse(Request.Form["idlibro"]);
                    
                    string[] categoriaseleccionadas = Request.Form.GetValues("Categorias[]");
                    string categoriaidlist = categoriaseleccionadas != null ? string.Join(",", categoriaseleccionadas) : "";
                    var resultado = dbcontext.Database.ExecuteSqlCommand("ACTUALIZARLIBRO @ISBN, @NOMBRE, @AUTOR,@TIPOLIBRO,@PRECIO,@STOCK,@ANIO,@EDITORIAL ,@ID_ADMINISTRADOR,@DESCRIPCION,@IDLIBRO ,@CATEGORIAS_ID_LIST, @IMAGEN",
                        new SqlParameter("@ISBN", Request.Form["ISBN"]),
                        new SqlParameter("@NOMBRE", Request.Form["Nombre"]),
                        new SqlParameter("@AUTOR", Request.Form["Autor"]),                
                        new SqlParameter("@TIPOLIBRO", Request.Form["Tipolibro"]),
                        new SqlParameter("@PRECIO", precio),
                        new SqlParameter("@STOCK", stock),
                        new SqlParameter("@ANIO", anio),
                        new SqlParameter("@EDITORIAL", editorial),
                        new SqlParameter("@ID_ADMINISTRADOR", idadmin),
                        new SqlParameter("@DESCRIPCION", Request.Form["descripcion"]),
                        new SqlParameter("@IDLIBRO", idlibro),
                        new SqlParameter("@CATEGORIAS_ID_LIST", categoriaidlist),
                        new SqlParameter("@IMAGEN", imagen != null ? (object)imagen : DBNull.Value));
                    if (resultado == 0)
                    {
                        Console.WriteLine("El procedimiento se ejecutó correctamente, pero no se realizaron cambios en los datos.");
                    }   
                        
                        if (resultado < 0)
                    {
                        // Puedes imprimir el mensaje de error o tomar alguna acción específica.
                        Console.WriteLine("Error al ejecutar el procedimiento almacenado.");
                    }
                    return RedirectToAction("Libros");
                }
                
            }
            catch (SqlException e)
            {
                Debug.WriteLine($"Error SQL: {e.Message}");

                

               
                
                return RedirectToAction("Error", "Home");
            }


        }

        private string obtenerimagen(HttpPostedFileBase file)
        {
            byte[] bytesImagen = null;
            string base64String = null;
            if (file != null && file.ContentLength > 0)
            {

                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytesImagen = br.ReadBytes(file.ContentLength);
                }


                base64String = Convert.ToBase64String(bytesImagen);


            }
            return base64String;
        }
        
        private List<Libro> ObtenerLibros(int? id)
        {
            
            using (var dbcontext = new arriendojuegosEntities1())
            {
                object idparameter = (object)id ?? DBNull.Value;
                var resultado = dbcontext.Database.SqlQuery<Libro>("OBTENERLIBRO @ID",
                    new SqlParameter("@ID", idparameter)).ToList();
                foreach (var libro in resultado)
                {
                    libro.CategoriaIdString = libro.CategoriaIdString.Trim();
                    if (!string.IsNullOrEmpty(libro.CategoriaIdString))
                    {
                        libro.CategoriaId = libro.CategoriaIdString.Split(',').Select(int.Parse).ToList();
                    }
                    if (!string.IsNullOrEmpty(libro.Imagen))
                    {
                        libro.Byteimagen = Convert.FromBase64String(libro.Imagen);
                    }

                }
                return resultado;
            }
        }

        private List<Editorial> obtenereditorial()
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                var resultado = dbcontext.Database.SqlQuery<Editorial>("OBTENEREDITORIAL").ToList();
                return resultado;
            }
        }

        private List<Categoria> obtenercategorias()
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                var resultado = dbcontext.Database.SqlQuery<Categoria>("OBTENERCATEGORIAS").ToList();
                return resultado;
            }
        }

        
    }
}