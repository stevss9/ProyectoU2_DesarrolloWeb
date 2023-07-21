using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        public string email { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}