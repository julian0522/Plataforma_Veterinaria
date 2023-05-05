using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioSucursal
    {
        Task Actualizar(Sucursal sucursal);
        Task Borrar(int id);
        Task Crear(Sucursal sucursal);
        Task<bool> Existe(string nombre);
        Task<Sucursal> ObtenerPorId(int id);
        Task<IEnumerable<Sucursal>> ObtenerSucursales();
    }
}
