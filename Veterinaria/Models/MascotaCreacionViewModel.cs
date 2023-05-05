using Microsoft.AspNetCore.Mvc.Rendering;

namespace Veterinaria.Models
{
    public class MascotaCreacionViewModel: Mascota
    {
        public IEnumerable<SelectListItem> ListaEspecies { get; set; }
        public IEnumerable<SelectListItem> ListaRazas { get; set; }
        public IEnumerable<SelectListItem> ListaClientes { get; set; }
        public IEnumerable<SelectListItem> ListaUsuarios { get; set; }
    }
}
