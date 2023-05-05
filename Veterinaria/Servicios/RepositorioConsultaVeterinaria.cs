using Dapper;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Servicios
{
    public class RepositorioConsultaVeterinaria: IRepositorioConsultaVeterinaria
    {
        private readonly string connectionString;

        public RepositorioConsultaVeterinaria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(ConsultaVeterinaria consultaVeterinaria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT
                                                            INTO ConsultaVeterinaria (MascotaId,UsuarioId,SucursalId,MotivoConsulta,Diagnostico,RecetaMedica,FechaConsulta,Peso,ClienteId)
                                                            VALUES (@MascotaId,@UsuarioId, @SucursalId, @MotivoConsulta, @Diagnostico, @RecetaMedica, @FechaConsulta, @Peso, @ClienteId);

                                                            SELECT SCOPE_IDENTITY();", consultaVeterinaria);
            consultaVeterinaria.Id = id;
        }

        public async Task<IEnumerable<ConsultaVeterinaria>> Obtener(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ConsultaVeterinaria>(@"SELECT ConsultaVeterinaria.*, ma.Nombre AS NomMascota, us.Nombre AS NomVeterinario, cl.Nombre AS NomCliente, su.Nombre AS Sucursal
                                                                    FROM ConsultaVeterinaria
                                                                    INNER JOIN Mascota ma
                                                                    ON ma.Id = ConsultaVeterinaria.MascotaId
                                                                    INNER JOIN Usuario us
                                                                    ON us.Id = ConsultaVeterinaria.UsuarioId
                                                                    INNER JOIN Cliente cl
                                                                    ON cl.Id = ConsultaVeterinaria.ClienteId
                                                                    INNER JOIN Sucursal su
                                                                    ON su.Id = ConsultaVeterinaria.SucursalId
                                                                    WHERE ma.Id = @Id", new {id});
        }

        public async Task<ConsultaVeterinaria> ObtenerPorId(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<ConsultaVeterinaria>(@"SELECT ConsultaVeterinaria.*, ma.Nombre AS NomMascota, us.Nombre AS NomVeterinario, cl.Nombre AS NomCliente, su.Nombre AS Sucursal
                                                                    FROM ConsultaVeterinaria
                                                                    INNER JOIN Mascota ma
                                                                    ON ma.Id = ConsultaVeterinaria.MascotaId
                                                                    INNER JOIN Usuario us
                                                                    ON us.Id = ConsultaVeterinaria.UsuarioId
                                                                    INNER JOIN Cliente cl
                                                                    ON cl.Id = ConsultaVeterinaria.ClienteId
                                                                    INNER JOIN Sucursal su
                                                                    ON su.Id = ConsultaVeterinaria.SucursalId
                                                                    WHERE ConsultaVeterinaria.Id = @Id", new { id });
        }

    }
}
