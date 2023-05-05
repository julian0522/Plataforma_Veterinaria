using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Controllers
{
    public class ConsultaVeterinariaController: Controller
    {
        private readonly IRepositorioConsultaVeterinaria repositorioConsultaVeterinaria;
        private readonly IRepositorioMascota repositorioMascota;
        private readonly IRepositorioEspecie repositorioEspecie;
        private readonly IRepositorioRaza repositorioRaza;
        private readonly IRepositorioCliente repositorioCliente;
        private readonly IRepositorioSucursal repositorioSucursal;
        private readonly IServicioUsuario servicioUsuario;
        private readonly IMapper mapper;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ConsultaVeterinariaController(IRepositorioConsultaVeterinaria repositorioConsultaVeterinaria,IRepositorioMascota repositorioMascota, IRepositorioEspecie repositorioEspecie, IRepositorioRaza repositorioRaza,
            IRepositorioCliente repositorioCliente, IRepositorioSucursal repositorioSucursal, IServicioUsuario servicioUsuario, IMapper mapper, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioConsultaVeterinaria = repositorioConsultaVeterinaria;
            this.repositorioMascota = repositorioMascota;
            this.repositorioEspecie = repositorioEspecie;
            this.repositorioRaza = repositorioRaza;
            this.repositorioCliente = repositorioCliente;
            this.repositorioSucursal = repositorioSucursal;
            this.servicioUsuario = servicioUsuario;
            this.mapper = mapper;
            this.repositorioUsuario = repositorioUsuario;
        }

        /// <summary>
        /// En este metodo utilizamos rotativa para generar un pdf del detade de cada consulta medica que se necesite
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Imprimir(int id) 
        {
            var consultaVeterinaria = await repositorioConsultaVeterinaria.ObtenerPorId(id);

            if (consultaVeterinaria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            
            return new Rotativa.AspNetCore.ViewAsPdf("DetalleImprimir",consultaVeterinaria)
            {
                FileName = "Consulta Veterinaria.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4

            };
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id) 
        {
            var consultaVeterinariaCreada = await repositorioConsultaVeterinaria.Obtener(id);
            var modelo = consultaVeterinariaCreada.Select(x=> new ConsultaVeterinaria
            {
                Id = x.Id,
                NomMascota = x.NomMascota,
                NomVeterinario = x.NomVeterinario,
                NomCliente = x.NomCliente,
                Sucursal = x.Sucursal,
                FechaConsulta = x.FechaConsulta
            });
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var modelo = new ConsultaVeterinariaCreacionViewModel();
            modelo.ListaClientes = await ObtenerClientes();
            modelo.ListaMascotas = await ObtenerListaMascotas(modelo.ClienteId);
            modelo.ListaSucursales = await ObtenerSucursales();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ConsultaVeterinariaCreacionViewModel modelo) 
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.ListaClientes = await ObtenerClientes();
                modelo.ListaMascotas = await ObtenerListaMascotas(modelo.ClienteId);
                modelo.ListaSucursales = await ObtenerSucursales();

                return View(modelo);
            }

            var cliente = await repositorioCliente.ObtenerPorId(modelo.ClienteId);

            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var mascota = await repositorioMascota.ObtenerPorId(modelo.MascotaId);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var sucursal = await repositorioSucursal.ObtenerPorId(modelo.SucursalId);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            modelo.UsuarioId = usuarioId;

            await repositorioConsultaVeterinaria.Crear(modelo);
            return RedirectToAction("Index", "Mascota");
        }


        public async Task<IActionResult> Detalle(int id) 
        {
            var consultaVeterinaria = await repositorioConsultaVeterinaria.ObtenerPorId(id);

            if (consultaVeterinaria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(consultaVeterinaria);
        }


        public async Task<IEnumerable<SelectListItem>> ObtenerClientes()
        {
            var listaClientes = await repositorioCliente.ObtenerClientes();
            return listaClientes.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
        public async Task<IEnumerable<SelectListItem>> ObtenerListaMascotas(int clienteId)
        {
            var listaMascotas = await repositorioMascota.ObtenerMascotas(clienteId);
            return listaMascotas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerUsuarios()
        {
            var listaUsuarios = await repositorioUsuario.ObtenerUsuarios();
            return listaUsuarios.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerSucursales()
        {
            var listaSucursales = await repositorioSucursal.ObtenerSucursales();
            return listaSucursales.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerMascotas([FromBody] int cliente)
        {
            var mascotas = await ObtenerListaMascotas(cliente);
            return Ok(mascotas);
        }
    }
}
