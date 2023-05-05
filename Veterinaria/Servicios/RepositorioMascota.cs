using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioMascota: IRepositorioMascota
    {
        private readonly string connectionString;

        public RepositorioMascota(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Mascota mascota) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT
                                                            INTO Mascota (Nombre,SexoId,ClienteId,UsuarioId,EspecieId,RazaId,FechaRegistro,Edad,Descripcion)
                                                            VALUES (@Nombre,@SexoId, @ClienteId, @UsuarioId, @EspecieId, @RazaId, @FechaRegistro, @Edad, @Descripcion);

                                                            SELECT SCOPE_IDENTITY();", mascota);
            mascota.Id = id;
        }

        public async Task<IEnumerable<Mascota>> Obtener() 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Mascota>(@"SELECT Mascota.*, ep.Nombre As Especie , cl.Nombre as NombreDueño, us.Nombre as NombreUsuario, rz.Nombre as NombreRaza
                                                        FROM Mascota
                                                        INNER JOIN Especie ep
                                                        ON  ep.Id = Mascota.EspecieId
                                                        INNER JOIN Raza rz
                                                        ON  rz.Id = Mascota.RazaId
                                                        INNER JOIN Cliente cl
                                                        ON  cl.Id = Mascota.ClienteId
                                                        INNER JOIN Usuario us
                                                        ON  us.Id = Mascota.UsuarioId
                                                        ORDER BY Mascota.Nombre");
        }

        public async Task<Mascota> ObtenerPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Mascota>(@"SELECT Mascota.*
                                                        FROM Mascota
                                                        INNER JOIN Especie ep
                                                        ON  ep.Id = Mascota.EspecieId
                                                        INNER JOIN Raza rz
                                                        ON  rz.Id = Mascota.RazaId
                                                        INNER JOIN Cliente cl
                                                        ON  cl.Id = Mascota.ClienteId
                                                        INNER JOIN Usuario us
                                                        ON  us.Id = Mascota.UsuarioId
                                                        WHERE Mascota.Id = @Id", new {id});
        }

        public async Task Actualizar(MascotaCreacionViewModel mascota) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Mascota
                                            SET SexoId = @SexoId, ClienteId = @ClienteId, EspecieId = @EspecieId,
                                                        RazaId = @RazaId, Edad = @Edad, Descripcion = @Descripcion
                                            WHERE Id = @Id;", mascota);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Mascota WHERE Id = @Id", new {id});
        }

        public async Task<IEnumerable<Mascota>> ObtenerMascotas(int clienteId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Mascota>("SELECT * FROM Mascota WHERE ClienteId = @clienteId",
                                                          new { clienteId });
        }

    }
}
