using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using arriendojuegos.Models;
using arriendojuegos.Models.ListModelLibro;
namespace arriendojuegos.Services
{
    public class Libroservice
    {
        public List<Libro> ObtenerLibros(int? id,string tipolibro,int? idcategoria,int? anio)
        {

            using (var dbcontext = new arriendojuegosEntities1())
            {
                object idparameter = (object)id ?? DBNull.Value;
                object tipolibroparameter = (object)tipolibro ?? DBNull.Value;
                object idcategoriaparameter = (object)idcategoria ?? DBNull.Value;
                object anioparameter = (object)anio ?? DBNull.Value;
                var resultado = dbcontext.Database.SqlQuery<Libro>("OBTENERLIBRO @ID,@TIPO_LIBRO,@CATEGORIA_ID,@ANIO",
                    new SqlParameter("@ID", idparameter),
                    new SqlParameter("@TIPO_LIBRO", tipolibroparameter),
                    new SqlParameter("@CATEGORIA_ID", idcategoriaparameter),
                    new SqlParameter("@ANIO", anioparameter)).ToList();
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
    }
}