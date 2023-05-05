using System.ComponentModel.DataAnnotations;
using Veterinaria.Validaciones;

namespace Veterinaria.Models
{
    public class Mascota
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "EL campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar el sexo")]
        public int SexoId { get; set; }

        [Display(Name = "Cedula del dueño")]
        public int ClienteId { get; set; }

        [Display(Name = "Nombre del Veterinario")]
        public int UsuarioId { get; set; }

        [Display(Name = "Especie")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public int EspecieId { get; set; }

        [Display(Name = "Raza")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        public int RazaId { get; set; }

        [Display(Name = "Fecha Registro")]
        [DataType(DataType.DateTime)] // aca podemos definir si queremos obtener la fecha actual con la hora o si no queremos solo mostrar la fecha
        // Lo que hacemos en esta propiedad de la fecha es que por medio del DateTime.Parse podemos seleccionar como queremos que aparezca la fecha actual con su hora y asi filtrar
        // lo que nos interesa de la fecha actual
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int Edad { get; set; }

        [StringLength(maximumLength: 500)]
        [PrimeraLetraMayuscula]
        public string Descripcion { get; set; }

        public string Especie { get; set; }
        public string NombreDueño { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreRaza { get; set; }
    }
}
