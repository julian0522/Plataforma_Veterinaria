using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Interfaces;
using Veterinaria.Models;
using Veterinaria.Servicios;

namespace Veterinaria.Controllers
{
    [Authorize]
    public class ClienteController: Controller
    {
        private readonly IRepositorioCliente repositorioCliente;

        public ClienteController(IRepositorioCliente repositorioCliente)
        {
            this.repositorioCliente = repositorioCliente;
        }

        public async Task<IActionResult> Index() 
        {
            var cliente = await repositorioCliente.ObtenerClientes();
            return View(cliente);
        }

        public IActionResult Crear() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Cliente cliente) 
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var yaExisteCliente = await repositorioCliente.Existe(cliente.Id, cliente.Nombre);

            if (yaExisteCliente)
            {
                ModelState.AddModelError(nameof(cliente.Id),
                            $"El Id {cliente.Id} ya existe");

                ModelState.AddModelError(nameof(cliente.Nombre),
                           $"El Cliente {cliente.Nombre} ya existe en la base de datos");
                return View(cliente);
            }

            await repositorioCliente.Crear(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var cliente = await repositorioCliente.ObtenerPorId(id);

            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(Cliente cliente) 
        {   
            var clienteExiste = await repositorioCliente.ObtenerPorId(cliente.Id);

            if (clienteExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCliente.Actualizar(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var cliente = await repositorioCliente.ObtenerPorId(id);

            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");

            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCliente(int id) 
        {
            var cliente = await repositorioCliente.ObtenerPorId(id);

            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioCliente.Borrar(id);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }

            
            return RedirectToAction("Index");
        }

    }
}
