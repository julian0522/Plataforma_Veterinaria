using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioEspecie
    {
        Task Actualizar(Especie especie);
        Task Borrar(int id);
        Task Crear(Especie especie);
        Task<bool> Existe(string nombre);
        Task<IEnumerable<Especie>> Obtener();
        Task<Especie> ObtenerPorId(int id);
    }
}
