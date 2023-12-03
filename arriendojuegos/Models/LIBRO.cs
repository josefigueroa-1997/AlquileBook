//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace arriendojuegos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LIBRO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LIBRO()
        {
            this.LIBRO_CARRITO_USUARIO = new HashSet<LIBRO_CARRITO_USUARIO>();
            this.CATEGORIA = new HashSet<CATEGORIA>();
        }
    
        public int ID { get; set; }
        public string ISBN { get; set; }
        public string NOMBRE { get; set; }
        public string AUTOR { get; set; }
        public string TIPOLIBRO { get; set; }
        public decimal PRECIO { get; set; }
        public int STOCK { get; set; }
        public int ANIO { get; set; }
        public int EDITORIAL { get; set; }
        public Nullable<int> ID_ADMINISTRADOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string IMAGEN { get; set; }
    
        public virtual ADMINISTRADOR ADMINISTRADOR { get; set; }
        public virtual EDITORIAL EDITORIAL1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRO_CARRITO_USUARIO> LIBRO_CARRITO_USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CATEGORIA> CATEGORIA { get; set; }
    }
}
