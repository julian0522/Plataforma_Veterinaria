    using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioEspecie: IRepositorioEspecie
    {
        private readonly string connectionString;

        public RepositorioEspecie(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Metodo para crear una especie
        /// </summary>
        /// <param name="especie"></param>
        /// <returns></returns>
        public async Task Crear(Especie especie) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Especie (Nombre)
                                                             VALUES (@Nombre);
                                                            SELECT SCOPE_IDENTITY();", especie);
            especie.Id = id;
        }

        /// <summary>
        /// Metodo para saber si la especie que se desee registrar ya se encuentra registrada en la base de datos
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public async Task<bool> Existe(string nombre) 
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                         FROM Especie
                                                                         WHERE Nombre = @Nombre;
                                                                         ", new { nombre });
            return existe == 1;
        }

        /// <summary>
        /// Metodo para listar totas las especies 
        /// </summary>
        /// <returns> Devuelve una lista de todas las especies existentes</returns>
        public async Task<IEnumerable<Especie>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Especie>("SELECT * FROM Especie");
        }

        public async Task Actualizar(Especie especie)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Especie
                                           SET Nombre = @Nombre
                                           WHERE Id = @Id", especie);
        }

        /// <summary>
        /// Metodo para obtener el id de la especie que se desea borrar ya que en la pagina index se encuentran listadas 
        /// todas las especies creadas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Especie> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Especie>(@"SELECT Id, Nombre
                                                                       FROM Especie
                                                                       WHERE Id = @id", new { id});
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Especie WHERE Id = @Id", new {id});
        }
    }
}
