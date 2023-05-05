using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class Raza
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="EL campo {0} es requerido")]
        [StringLength(maximumLength:50)]
        [PrimeraLetraMayuscula]
        [Remote(action: "VerificarExisteRaza", controller: "Raza")]
        public string Nombre { get; set; }

        [Display(Name ="Especie")]
        public int EspecieId { get; set; }

        public string Especie { get; set; }
    }
}
