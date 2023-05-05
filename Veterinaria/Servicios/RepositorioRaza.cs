using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioRaza: IRepositorioRaza
    {
        private readonly string connectionString;
        public RepositorioRaza(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Raza raza) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Raza (Nombre, EspecieId)
                                                            VALUES (@Nombre, @EspecieId);
                                        
                                                            SELECT SCOPE_IDENTITY();", raza);
            raza.Id = id;
        }

        public async Task<bool> Existe(string nombre, int idEspecie) 
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                         FROM Raza
                                                                         WHERE Nombre = @Nombre AND EspecieId = @idEspecie;"
                                                                         , new {nombre, idEspecie });
            return existe == 1;
        }

        /// <summary>
        /// Metodo para obtener la lista de todas las razas existentes en la base de datos 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Raza>> ObtenerRaza() 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Raza>(@"SELECT Raza.Id, Raza.Nombre, ep.Nombre As Especie
                                                      FROM Raza
                                                      INNER JOIN Especie ep
                                                      ON ep.Id = Raza.EspecieId
                                                      ORDER BY ep.Nombre");
        }

        public async Task<IEnumerable<Raza>> ObtenerRazas(int especieId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Raza>("SELECT * FROM Raza WHERE EspecieId = @especieId",
                                                          new { especieId });
        }

        /// <summary>
        /// Metodo para obtener una raza por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Raza> ObtenerPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Raza>(@"SELECT Raza.Id, Raza.Nombre, Raza.EspecieId
                                                                    FROM Raza
                                                                    INNER JOIN Especie ep
                                                                    ON ep.Id = Raza.EspecieId
                                                                    WHERE Raza.Id = @Id", new {id});
        }

        public async Task Actualizar(RazaCreacionViewModel raza) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Raza
                                            SET Nombre = @Nombre, EspecieId = @EspecieId
                                            WHERE Id = @Id;", raza);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Raza WHERE Id = @Id", new { id});
        }
    }
}
