using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelUsuario;
using BCrypt.Net;
using arriendojuegos.Filters;

namespace arriendojuegos.Controllers
{
    [CargarCategorias]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Usuarios()
        {
            var obtenerusuarios = ObtenerDatos();
            return View(obtenerusuarios);
        }

        private List<Usuario> ObtenerDatos()
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                var datosusuario = dbcontext.Database.SqlQuery<Usuario>("EXEC OBTENERUSUARIOS").ToList();
                return datosusuario;
            }
        }


        //REGISTRAR USUARIO
        public ActionResult CrearUsuario()
        {
            List<Comuna> comunas = DropdownComunas();
            
            List<SelectListItem> items = comunas.ConvertAll(
                d =>
                {
                    return new SelectListItem()
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                }
            );
            ViewBag.datos = items;

            return View();
        }


        

        private List<Comuna> DropdownComunas()
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                var resultado = dbcontext.Database.SqlQuery<Comuna>("DESPLEGARCOMUNAS").ToList();
                return resultado;
            }
        }

        [HttpPost]
        public ActionResult RegistrarUsuario()
        {
          
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string encryptedpassword = BCrypt.Net.BCrypt.HashPassword(Request.Form["Contraseña"],salt);
                int telefono = int.Parse(Request.Form["telefono"]);
                string fechaNacimientoString = Request.Form["fechanacimiento"];
                string fechaFormateada = null;
                int idcomuna = int.Parse(Request.Form["ComunaId"]);
                if (DateTime.TryParse(fechaNacimientoString, out DateTime fechaNacimiento))
                {
                    
                    fechaFormateada = fechaNacimiento.ToString("yyyy-MM-dd");
                }
                    using (var dbcontext = new arriendojuegosEntities1())
                {
                    dbcontext.Database.ExecuteSqlCommand("CREARUSUARIO @NOMBRE, @CORREO, @CONTRASEÑA, @TELEFONO, @SEXO,@FECHANACIMIENTO,@COMUNA,@DIRECCION,@SALT",
                        new SqlParameter("@NOMBRE", Request.Form["Nombre"]),
                        new SqlParameter("@CORREO", Request.Form["Correo"]),
                        new SqlParameter("@CONTRASEÑA", encryptedpassword),
                        new SqlParameter("@TELEFONO", telefono),
                        new SqlParameter("@SEXO", Request.Form["SexoId"]),
                        new SqlParameter("@FECHANACIMIENTO", fechaFormateada),
                        new SqlParameter("@COMUNA", idcomuna),
                        new SqlParameter("@DIRECCION", Request.Form["Direccion"]),
                        new SqlParameter("@SALT",salt));
                        return RedirectToAction("Index","Home");
            }
            
            
            
            
        }

        private Boolean tieneMayusculas(string contraseña)
        {
            return contraseña.Any(char.IsUpper);
        }

        private bool tienecaracterespeciales(string contraseña)
        {
            string regex = "!@#$%^&*(),.?\":{}|<>";
            return contraseña.Any(caracteres=> regex.Contains(caracteres));
        }

        private Boolean validarcorreo(string correo)
        {
            string regex = "^[a-zA-Z0-9._%+-]+@gmail\\.com$";
            return correo.Any(caracteres => regex.Contains(caracteres));
        }

        private bool validarnumerotelefono(int? numero)
        {
            string regex = "^(?!.*\\+569)[98756]\\d{8}$";
            return numero.ToString().Any(caracter => regex.Contains(caracter));
        }

        //ACTUALIZAR USUARIO
        public ActionResult Update(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var usuario = Obtenerusuarioid(id);
                return View(usuario);
            }
            
        }

        private Usuario Obtenerusuarioid(int? id)
        {

            Usuario usuario = new Usuario();
            using (var dbcontext = new arriendojuegosEntities1())
            {
                var resultado = dbcontext.Database.SqlQuery<Usuario>("EXEC OBTENERUSUARIOID @ID",
                    new SqlParameter("@ID", id)).FirstOrDefault();

                usuario.nombre = resultado.nombre;
                usuario.correo = resultado.correo;
                usuario.contraseña = resultado.contraseña;
                usuario.telefono = resultado.telefono;
                usuario.direccion = resultado.direccion;
                usuario.comunanombre = resultado.comunanombre;
            }
            return usuario;
        }
        [HttpPost]
        public ActionResult Update(Usuario actualizar)
        {
            if (string.IsNullOrWhiteSpace(actualizar.correo) || !validarcorreo(actualizar.correo))
            {
                ModelState.AddModelError("correo", "El correo debe ser de dominio Gmail");
            }
            if (actualizar.telefono <= 0 || !validarnumerotelefono(actualizar.telefono))
            {
                ModelState.AddModelError("telefono", "El número no es valido");
            }
            if (ModelState.IsValid)
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    dbcontext.Database.ExecuteSqlCommand("UPDATEUSUARIO @ID, @NOMBRE, @CORREO,@TELEFONO,@DIRECCION,@COMUNA",
                        new SqlParameter("@ID", actualizar.Id),
                        new SqlParameter("@NOMBRE", actualizar.nombre),
                        new SqlParameter("@CORREO", actualizar.correo), 
                        new SqlParameter("@TELEFONO", actualizar.telefono),
                        new SqlParameter("@DIRECCION",actualizar.direccion),
                        new SqlParameter("@COMUNA", (object)actualizar.comunanombre ?? DBNull.Value)

                        );
                }
                var nombreact = obtenercredenciales(actualizar.correo);
                Session["nombre"] = nombreact.nombre;
                return RedirectToAction("Usuarios");
            }
            
            

            return View();
        }

        //ELIMINAR USUARIO
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbcontext = new arriendojuegosEntities1())
            {
                dbcontext.Database.ExecuteSqlCommand("DELETEUSUARIO @ID",
                    new SqlParameter("@ID", id));
            }
            return RedirectToAction("Usuarios");

        }

        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Login(string correo,string contraseña)
        {



            using (var dbcontext = new arriendojuegosEntities1())
            {
                Login credenciales = new Login();
                credenciales = obtenercredenciales(correo);
                if (credenciales!= null)
                {
                    bool validar = verificarcontraseñas(contraseña, credenciales.CONTRASEÑA);
                    if (validar)
                    {
                        int rol = RolUsuario(credenciales.Id);
                        
                                Session["rol"] = rol;
                                Session["id"] = credenciales.Id; 
                                Session["nombre"] = credenciales.nombre;
                                return RedirectToAction("Index","Home");

                    }
                    else
                    {
                        
                            ViewBag.Error = "Credenciales no válidas";
                            
                        
                    }
                }
                ViewBag.Error = "No existe un Usuario con ese Correo y Contraseña";
                
                return View();
            }
                

        }

        public ActionResult CerrarSesion()
        {
           
            Session.Abandon();

            
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            
            return RedirectToAction("Login", "Usuario");
        }




        private bool verificarcontraseñas(string passuser, string passbd)
        {
            bool verificar = false; 
            try
            {
                if (!string.IsNullOrEmpty(passuser) && (!string.IsNullOrEmpty(passbd)))
                {
                    

                    verificar = BCrypt.Net.BCrypt.Verify(passuser ,passbd);

                }
            }
            
            catch(Exception e)
            {
                Console.WriteLine("Error al obtener credenciales: " + e.Message);
            }
            return verificar;
        }


        private Login obtenercredenciales(string email)
        {
            Login login = null;

            try
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    var resultado = dbcontext.Database.SqlQuery<Login>("INICIOSESION @CORREO",
                    new SqlParameter("@CORREO", email)).FirstOrDefault();

                    if (resultado != null)
                    {
                        login = new Login
                        {
                            Id = resultado.Id,
                            CONTRASEÑA = resultado.CONTRASEÑA,
                            nombre = resultado.nombre,
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error al obtener credenciales: " + ex.Message);
            }

            return login;
        }


        private int RolUsuario(int id)
        {
            int rol = 0;
            using (var dbcontext = new arriendojuegosEntities1())
            {
                
                var resultado = dbcontext.Database.SqlQuery<int>("OBTENERROL @ID",
                    new SqlParameter("@ID",id)).FirstOrDefault();
                if (resultado !=0)
                {
                    rol = resultado;
                    return rol;
                }
            }
            return rol;
        }


    }
}


