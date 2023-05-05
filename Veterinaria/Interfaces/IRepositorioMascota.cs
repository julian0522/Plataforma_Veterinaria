using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioMascota
    {
        Task Actualizar(MascotaCreacionViewModel mascota);
        Task Borrar(int id);
        Task Crear(Mascota mascota);
        Task<IEnumerable<Mascota>> Obtener();
        Task<IEnumerable<Mascota>> ObtenerMascotas(int clienteId);
        Task<Mascota> ObtenerPorId(int id);
    }
}
