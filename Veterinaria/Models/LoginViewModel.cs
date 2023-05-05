using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El cmapo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Recuerdame { get; set; }
    }
}
