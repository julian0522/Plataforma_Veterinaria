using AutoMapper;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Raza, RazaCreacionViewModel>();
            CreateMap<Mascota, MascotaCreacionViewModel>();
            CreateMap<ConsultaVeterinaria, ConsultaVeterinariaCreacionViewModel>();
        }
    }
}
