using System.ComponentModel.DataAnnotations;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        [StringLength(maximumLength: 150, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "El numero movil debe ser de 10 numeros")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        public int Edad { get; set; }
    }
}
