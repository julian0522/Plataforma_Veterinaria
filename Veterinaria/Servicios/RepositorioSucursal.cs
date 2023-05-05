using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioSucursal: IRepositorioSucursal
    {
        private readonly string connectionString;

        public RepositorioSucursal(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Sucursal sucursal) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Sucursal ( Nombre, Direccion, Telefono)
                                                                            VALUES (@Nombre, @Direccion, @Telefono);

                                                                            SELECT SCOPE_IDENTITY();", sucursal);
            sucursal.Id = id;
        }

        public async Task<IEnumerable<Sucursal>> ObtenerSucursales() 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Sucursal>(@"SELECT * FROM Sucursal");
        }

        public async Task<bool> Existe(string nombre) 
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                          FROM Sucursal
                                                                          WHERE Nombre = @Nombre", new {nombre});
            return existe == 1;
        }

        public async Task<Sucursal> ObtenerPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Sucursal>(@"SELECT Id, Nombre, Direccion, Telefono
                                                                         FROM Sucursal
                                                                         WHERE Id = @Id", new {id});
        }

        public async Task Actualizar(Sucursal sucursal) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Sucursal
                                            SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono
                                            WHERE Id = @Id", sucursal);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Sucursal WHERE Id = @Id", new {id});
        }
    }
}
