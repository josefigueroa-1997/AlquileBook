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
    
    public partial class ADMINISTRADOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADMINISTRADOR()
        {
            this.LIBRO = new HashSet<LIBRO>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string CORREO { get; set; }
        public string CONTRASEÑA { get; set; }
        public string SALT { get; set; }
        public string ROL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRO> LIBRO { get; set; }
    }
}