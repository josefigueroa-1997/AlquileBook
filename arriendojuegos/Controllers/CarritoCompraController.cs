﻿using arriendojuegos.Models;
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
using System.Net;
using System.Net.Mail;
using arriendojuegos.Services;

namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    public class CarritoCompraController : Controller
    {
        private readonly Libroservice libroservice = new Libroservice();
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
                    DateTime fechaalquiler = calcularfechatermino(Request.Form["fechaalquiler"]);
                    var resultado = dbcontext.Database.ExecuteSqlCommand("EFECTUARALQUILER @ID_USUARIO,@LIBROS_ID,@TOTAL,@INFORMACION,@FECHATERMINO",
                        new SqlParameter("@ID_USUARIO",iduser),
                        new SqlParameter("@LIBROS_ID",librosid),
                        new SqlParameter("@TOTAL",total),
                        new SqlParameter("@INFORMACION","Se realizó un alquiler de manera exitosa"),
                        new SqlParameter("@FECHATERMINO",fechaalquiler));
                }

                string email = Session["email"].ToString();
                Boleta boleta = new Boleta();
                boleta = libroservice.DetalleBoleta();
                if(!string.IsNullOrEmpty(email) && boleta != null)
                {
                    EnviarCorreo(email, boleta);
                }
                else
                {
                    Debug.WriteLine("El correo o idboleta es vacio o no  existe");
                }
                return RedirectToAction("Success","CarritoCompra");
            }
            catch(SqlException e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Error", "Shared");
            }

        }

        public ActionResult Success()
        {
            return View();
        }
        private DateTime calcularfechatermino(string parametrofecha)
        {
            int semanasseleccionadas = int.Parse(parametrofecha);
            if(semanasseleccionadas == 3)
            {
                semanasseleccionadas++;
            }
            DateTime fechatermino = DateTime.Now;
            Debug.WriteLine(fechatermino);
            for(int i = 1; i <= semanasseleccionadas; i++)
            {
                fechatermino = fechatermino.AddDays(7);
                if(fechatermino.DayOfWeek == DayOfWeek.Friday)
                {
                    fechatermino = fechatermino.AddDays(3);
                }
                if (fechatermino.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechatermino = fechatermino.AddDays(2);
                }
                if (fechatermino.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechatermino = fechatermino.AddDays(1);
                }

            }
            return fechatermino;
        }

        private void EnviarCorreo(string email,Boleta boleta)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress("alquilerbooks@outlook.com");
            mensaje.To.Add(email);
            mensaje.Subject = "Su Alquiler fue exitoso";
            mensaje.Headers.Add("Content-Type", "text/html; charset=utf-8");
            mensaje.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("Gracias por alquilar libros con nosotros\n" +
                                                                        "Sus libros físicos serán despachados a la brevedad\n" +
                                                                        $"ID de Boleta: '{boleta.IDBOLETA}'\n" +
                                                                        $"ID de Transacción: '{boleta.IDTRANSACCION}'\n" +
                                                                        $"ID de Usuario: '{boleta.IDUSUARIO}'\n" +
                                                                        $"Nombre de Usuario: '{boleta.NOMBREUSUARIO}'\n" +
                                                                        $"Fecha de Emisión de Boleta: '{boleta.FECHAEMISION}'\n" +
                                                                        $"ID de Libros Alquilados: '{boleta.IDSLIBROS}'\n" +
                                                                        $"Nombre de Libros: '{boleta.NOMBRESLIBROS}'\n" +
                                                                        $"Total Transacción: '{boleta.TOTALTRANSACCION}'", null, "text/plain"));
            
            mensaje.Body = "<html><body style='font-family: Arial, sans-serif; font-size: 16px; text-align: center;'>"; 
            mensaje.Body += "<p style='font-weight: bold;'>Gracias por alquilar libros con nosotros</p>";
            mensaje.Body += "<p>Sus libros físicos serán despachados a la brevedad</p>";
            mensaje.Body += "<p>ID de Boleta: '" + boleta.IDBOLETA + "'</p>";
            mensaje.Body += "<p>ID de Transacción: '" + boleta.IDTRANSACCION + "'</p>";
            mensaje.Body += "<p>ID de Usuario: '" + boleta.IDUSUARIO + "'</p>";
            mensaje.Body += "<p>Nombre de Usuario: '" + boleta.NOMBREUSUARIO + "'</p>";
            mensaje.Body += "<p>Fecha de Emisión de Boleta: '" + boleta.FECHAEMISION + "'</p>";
            mensaje.Body += "<p>ID de Libros Alquilados: '" + boleta.IDSLIBROS + "'</p>";
            mensaje.Body += "<p>Nombre de Libros: '" + boleta.NOMBRESLIBROS + "'</p>";
            mensaje.Body += "<p>Total Transacción: '" + boleta.TOTALTRANSACCION + "'</p>";
            mensaje.Body += "</body></html>";

            // Añadir una imagen al correo electrónico (cambia la URL de la imagen según tu caso)
            string imagenUrl = "https://ejemplo.com/imagen.jpg";
            mensaje.Body += $"<p><img src='{imagenUrl}' alt='Imagen de ejemplo'></p>";
           
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("alquilerbooks@outlook.com", "pepe2023");
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mensaje);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        



    }
}