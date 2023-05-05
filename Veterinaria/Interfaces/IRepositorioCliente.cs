using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioCliente
    {
        Task Actualizar(Cliente cliente);
        Task Borrar(int id);
        Task Crear(Cliente cliente);
        Task<bool> Existe(int idCedula, string nombre);
        Task<IEnumerable<Cliente>> ObtenerClientes();
        Task<Cliente> ObtenerPorId(int idCedula);
    }
}
