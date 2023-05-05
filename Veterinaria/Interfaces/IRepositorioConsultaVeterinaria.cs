using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioConsultaVeterinaria
    {
        Task Crear(ConsultaVeterinaria consultaVeterinaria);
        Task<IEnumerable<ConsultaVeterinaria>> Obtener(int id);
        Task<ConsultaVeterinaria> ObtenerPorId(int id);
    }
}
