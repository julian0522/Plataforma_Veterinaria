using System.ComponentModel.DataAnnotations;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class Sucursal
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 150, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string  Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "El numero movil debe ser de 10 numeros")]
        public string Telefono { get; set; }
    }
}
