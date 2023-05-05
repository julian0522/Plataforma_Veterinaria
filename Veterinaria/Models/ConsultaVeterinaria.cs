using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class ConsultaVeterinaria
    {
        public int Id { get; set; }

        [Display(Name = "Mascota")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una mascota")]
        public int MascotaId { get; set; }

        [Display(Name = "Nombre del Veterinario")]
        public int UsuarioId { get; set; }

        public int ClienteId { get; set; }

        [Display(Name = "Sucursal")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public int SucursalId { get; set; }

        [Required(ErrorMessage = "EL campo {0} es requerido")]
        [StringLength(maximumLength: 1000)]
        [PrimeraLetraMayuscula]
        public string MotivoConsulta { get; set; }

        [Required(ErrorMessage = "EL campo {0} es requerido")]
        [StringLength(maximumLength: 1000)]
        [PrimeraLetraMayuscula]
        public string Diagnostico { get; set; }

        [StringLength(maximumLength: 1000)]
        [PrimeraLetraMayuscula]
        public string RecetaMedica { get; set; }

        [Display(Name = "Fecha Consulta")]
        [DataType(DataType.DateTime)]
        public DateTime FechaConsulta { get; set; } = DateTime.Now;
        public decimal Peso { get; set; }



        public string NomMascota { get; set; }
        public string NomVeterinario { get; set; }
        public string NomCliente { get; set; }
        public string Sucursal { get; set; }
    }
}
