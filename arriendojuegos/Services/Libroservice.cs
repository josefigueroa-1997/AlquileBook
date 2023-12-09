using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelCarritoCompra;
using arriendojuegos.Models.ListModelLibro;
namespace arriendojuegos.Services
{
    public class Libroservice
    {
        public List<Libro> ObtenerLibros(int? id,string tipolibro,int? idcategoria,int? anio, string nombre)
        {

            using (var dbcontext = new arriendojuegosEntities1())
            {
                object idparameter = (object)id ?? DBNull.Value;
                object tipolibroparameter = (object)tipolibro ?? DBNull.Value;
                object idcategoriaparameter = (object)idcategoria ?? DBNull.Value;
                object anioparameter = (object)anio ?? DBNull.Value;
                object nombreparameter = (object)nombre ?? DBNull.Value;
                var resultado = dbcontext.Database.SqlQuery<Libro>("OBTENERLIBRO @ID,@TIPO_LIBRO,@CATEGORIA_ID,@ANIO,@NOMBRE",
                    new SqlParameter("@ID", idparameter),
                    new SqlParameter("@TIPO_LIBRO", tipolibroparameter),
                    new SqlParameter("@CATEGORIA_ID", idcategoriaparameter),
                    new SqlParameter("@ANIO", anioparameter),
                    new SqlParameter("@NOMBRE",nombreparameter)).ToList();
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

        public Boleta DetalleBoleta(int? id)
        {
            Boleta boleta = null;
            try
            {
                using (var dbcontext = new arriendojuegosEntities1())
                {
                    object idboleta = (object)id ?? DBNull.Value;
                    var resultado = dbcontext.Database.SqlQuery<Boleta>("DETALLEBOLETA @ID",
                        new SqlParameter("@ID", idboleta)).FirstOrDefault();
                    if (resultado != null)
                    {
                        boleta = new Boleta
                        {
                            IDBOLETA = resultado.IDBOLETA,
                            IDTRANSACCION = resultado.IDTRANSACCION,
                            TOTALTRANSACCION = resultado.TOTALTRANSACCION,
                            IDUSUARIO = resultado.IDUSUARIO,
                            NOMBREUSUARIO = resultado.NOMBREUSUARIO,
                            FECHAEMISION = resultado.FECHAEMISION,
                            NOMBRESLIBROS = resultado.NOMBRESLIBROS,
                            IDSLIBROS = resultado.IDSLIBROS,

                        };

                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            
            
            return boleta;

        }
    }
}