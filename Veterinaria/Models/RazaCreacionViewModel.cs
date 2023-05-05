using Microsoft.AspNetCore.Mvc.Rendering;

namespace Veterinaria.Models
{
    public class RazaCreacionViewModel: Raza
    {
        // Esta clase va a ser el modelo de la vista que va a contener el formulario de creacion de la raza

        public IEnumerable<SelectListItem> ListaEspecies { get; set; }
    }
}
