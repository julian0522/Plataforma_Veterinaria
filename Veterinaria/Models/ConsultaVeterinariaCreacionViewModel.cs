using Microsoft.AspNetCore.Mvc.Rendering;

namespace Veterinaria.Models
{
    public class ConsultaVeterinariaCreacionViewModel: ConsultaVeterinaria
    {
        public IEnumerable<SelectListItem> ListaEspecies { get; set; }
        public IEnumerable<SelectListItem> ListaSucursales { get; set; }
        public IEnumerable<SelectListItem> ListaClientes { get; set; }
        public IEnumerable<SelectListItem> ListaUsuarios { get; set; }
        public IEnumerable<SelectListItem> ListaMascotas { get; set; }
    }
}
