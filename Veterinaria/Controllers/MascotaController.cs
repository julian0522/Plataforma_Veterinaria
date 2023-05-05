using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Veterinaria.Interfaces;
using Veterinaria.Models;
using Veterinaria.Servicios;

namespace Veterinaria.Controllers
{
    [Authorize]
    public class MascotaController: Controller
    {
        private readonly IRepositorioMascota repositorioMascota;
        private readonly IRepositorioEspecie repositorioEspecie;
        private readonly IRepositorioRaza repositorioRaza;
        private readonly IRepositorioCliente repositorioCliente;
        private readonly IRepositorioSucursal repositorioSucursal;
        private readonly IServicioUsuario servicioUsuario;
        private readonly IMapper mapper;
        private readonly IRepositorioUsuario repositorioUsuario;

        public MascotaController(IRepositorioMascota repositorioMascota, IRepositorioEspecie repositorioEspecie, IRepositorioRaza repositorioRaza,
            IRepositorioCliente repositorioCliente, IRepositorioSucursal repositorioSucursal, IServicioUsuario servicioUsuario, IMapper mapper, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioMascota = repositorioMascota;
            this.repositorioEspecie = repositorioEspecie;
            this.repositorioRaza = repositorioRaza;
            this.repositorioCliente = repositorioCliente;
            this.repositorioSucursal = repositorioSucursal;
            this.servicioUsuario = servicioUsuario;
            this.mapper = mapper;
            this.repositorioUsuario = repositorioUsuario;
        }

        public ActionResult Imprimir()
        {
            return new Rotativa.AspNetCore.ViewAsPdf("Detalle");
        }

        public async Task<IActionResult> Index() 
        {
            var mascotasCreadas = await repositorioMascota.Obtener();
            var modelo = mascotasCreadas.Select(x=> new Mascota 
            {
                Id = x.Id,
                Nombre = x.Nombre,
                NombreDueño = x.NombreDueño,
                NombreUsuario = x.NombreUsuario,
                Especie = x.EspecieId.ToString(),
                NombreRaza = x.NombreRaza,
                FechaRegistro = x.FechaRegistro
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            var modelo = new MascotaCreacionViewModel();
            modelo.ListaEspecies = await ObtenerEspecies();
            modelo.ListaRazas = await ObtenerListaRazas(modelo.EspecieId);
            modelo.ListaClientes = await ObtenerClientes();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(MascotaCreacionViewModel modelo) 
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.ListaEspecies = await ObtenerEspecies();
                modelo.ListaRazas = await ObtenerListaRazas(modelo.EspecieId);
                modelo.ListaClientes = await ObtenerClientes();

                return View(modelo);
            }

            var especie = await repositorioEspecie.ObtenerPorId(modelo.EspecieId);

            if (especie is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var raza = await repositorioRaza.ObtenerPorId(modelo.RazaId);

            if (raza is null)
            {
               return RedirectToAction("NoEncontrado", "Home");
            }

            var cliente = await repositorioCliente.ObtenerPorId(modelo.ClienteId);

            if (cliente is null)
            {
               return RedirectToAction("NoEncontrado", "Home");
            }

            modelo.UsuarioId = usuarioId;

            await repositorioMascota.Crear(modelo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var mascota = await repositorioMascota.ObtenerPorId(id);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<MascotaCreacionViewModel>(mascota);

            modelo.ListaEspecies = await ObtenerEspecies();
            modelo.ListaRazas = await ObtenerListaRazas(modelo.EspecieId);
            modelo.ListaClientes = await ObtenerClientes();

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> EditarMascota(MascotaCreacionViewModel mascotaEditar) 
        {
            var mascota = await repositorioMascota.ObtenerPorId(mascotaEditar.Id);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var especie = await repositorioEspecie.ObtenerPorId(mascotaEditar.EspecieId);

            if (especie is null)
            {
               return RedirectToAction("NoEncontrado", "Home");
            }

            var raza = await repositorioRaza.ObtenerPorId(mascotaEditar.RazaId);

            if (raza is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioMascota.Actualizar(mascotaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var mascota = await repositorioMascota.ObtenerPorId(id);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(mascota);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarMascota(int id) 
        {
            var mascota = await repositorioMascota.ObtenerPorId(id);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioMascota.Borrar(id);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detalle(int id) 
        {
            var mascota = await repositorioMascota.ObtenerPorId(id);

            if (mascota is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<MascotaCreacionViewModel>(mascota);

            modelo.ListaEspecies = await ObtenerEspecies();
            modelo.ListaRazas = await ObtenerListaRazas(modelo.EspecieId);
            modelo.ListaClientes = await ObtenerClientes();
            modelo.ListaUsuarios = await ObtenerUsuarios();

            return View(modelo);

        }


        public async Task<IEnumerable<SelectListItem>> ObtenerClientes()
        {
            var listaClientes = await repositorioCliente.ObtenerClientes();
            return listaClientes.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
        public async Task<IEnumerable<SelectListItem>> ObtenerListaRazas(int especieId)
        {
            var listaRazas = await repositorioRaza.ObtenerRazas(especieId);
            return listaRazas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
        public async Task<IEnumerable<SelectListItem>> ObtenerEspecies()
        {
            var listaEspecies = await repositorioEspecie.Obtener();
            return listaEspecies.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerUsuarios()
        {
            var listaUsuarios = await repositorioUsuario.ObtenerUsuarios();
            return listaUsuarios.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerRazas([FromBody] int especie)
        {
            var razas = await ObtenerListaRazas(especie);
            return Ok(razas);
        }
    }
}
