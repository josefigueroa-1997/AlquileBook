using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace arriendojuegos.Models.ListModelUsuario
{
    public class CrearUsuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "El campo Correo no tiene un formato de correo electrónico válido.")]
        [Display(Name = "Correo")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "El correo electrónico debe ser de Gmail")]
        public string Correo { get; set; }
        [Required]
        [StringLength(200)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\"":{}|<>])(.{8,})$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula y un carácter especial.")]
        public string Contraseña { get; set; }
        [Required]
        [RegularExpression(@"^(?!.*\+569)[98756]\d{8}$", ErrorMessage = "Número de telefóno no valido")]
        public int? telefono { get; set; }
        [Required]
        [Display(Name ="Selecciona un sexo")]
        public int SexoId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name ="Ingrese su fecha de Nacimiento")]
        public DateTime fechanacimiento { get; set; }
        [Required]
        [Display(Name = "Ingrese su Dirección")]
        public string Direccion { get; set; }
        [Required]
        [Display(Name ="Seleccione una comuna")]
        public int ComunaId { get; set; }

        public List<SelectListItem> Comunas { get; set; }
        
    }
}