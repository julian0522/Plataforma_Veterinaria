using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioCliente: IRepositorioCliente
    {
        private readonly string connectionString;

        public RepositorioCliente(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cliente cliente) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.QuerySingleAsync(@"INSERT INTO Cliente (Id, Nombre, Email, Telefono, Direccion)
                                                             VALUES (@Id, @Nombre, @Email, @Telefono, @Direccion);
                                            
                                                             SELECT SCOPE_IDENTITY();",cliente);
         
        }

        public async Task<bool> Existe(int id, string nombre) 
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                         FROM Cliente
                                                                         WHERE Id = @Id AND Nombre = @Nombre;"
                                                                         , new {id, nombre });
            return existe == 1;
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientes() 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cliente>("SELECT * FROM Cliente");
        }

        public async Task<Cliente> ObtenerPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cliente>(@"SELECT Id, Nombre, Email, Telefono, Direccion
                                                                        FROM Cliente
                                                                        WHERE Id = @Id", new { id});
        }

        public async Task Actualizar(Cliente cliente) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Cliente
                                            SET Email = @Email, Telefono = @Telefono, Direccion = @Direccion
                                            WHERE Id = @Id", cliente);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Cliente WHERE Id = @Id", new {id});
        }

        
    }
}
