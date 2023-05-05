using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioUsuario: IRepositorioUsuario
    {
        private readonly string connectionString;
        public RepositorioUsuario(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearUsuario(Usuario usuario) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Usuario (Email,EmailNormalizado, PasswordHash, Nombre, Cedula, Telefono, Edad)
                                                            VALUES (@Email, @EmailNormalizado, @PasswordHash, @Nombre, @Cedula, @Telefono, @Edad);
                                                             SELECT SCOPE_IDENTITY();",
                                                            usuario);
            return id;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(@"SELECT * FROM Usuario WHERE EmailNormalizado = @emailNormalizado"
                                                                        , new {emailNormalizado});
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuarios() 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Usuario>(@"SELECT * FROM Usuario");
        }
    }
}
