using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class Especie
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50, ErrorMessage ="La cantidad de caracteres debe ser de maximo {1}")]
        [PrimeraLetraMayuscula]
        [Remote(action:"VerificarExisteEspecie", controller:"Especie")]
        public string Nombre { get; set; }
    }
}
