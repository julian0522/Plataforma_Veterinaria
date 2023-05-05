using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioRaza
    {
        Task Actualizar(RazaCreacionViewModel raza);
        Task Borrar(int id);
        Task Crear(Raza raza);
        Task<bool> Existe(string nombre, int idEspecie);
        Task<Raza> ObtenerPorId(int id);
        Task<IEnumerable<Raza>> ObtenerRaza();
        Task<IEnumerable<Raza>> ObtenerRazas(int especieId);
    }
}
