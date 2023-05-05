using Veterinaria.Models;

namespace Veterinaria.Interfaces
{
    public interface IRepositorioUsuario
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
        Task<IEnumerable<Usuario>> ObtenerUsuarios();
    }
}
