using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Controllers
{
    [Authorize]
    public class SucursalController: Controller
    {
        private readonly IRepositorioSucursal repositorioSucursal;

        public SucursalController(IRepositorioSucursal repositorioSucursal)
        {
            this.repositorioSucursal = repositorioSucursal;
        }

        public async Task<IActionResult> Index() 
        {
            var sucursales = await repositorioSucursal.ObtenerSucursales();
            return View(sucursales);
        }

        public IActionResult Crear() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Sucursal sucursal) 
        {
            if (!ModelState.IsValid)
            {
                return View(sucursal);
            }

            var yaExisteSucursal = await repositorioSucursal.Existe(sucursal.Nombre);

            if (yaExisteSucursal)
            {
                ModelState.AddModelError(nameof(sucursal.Nombre),
                            $"La sucursal {sucursal.Nombre} ya existe");
                return View(sucursal);
            }

            await repositorioSucursal.Crear(sucursal);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteSucursal(string nombre) 
        {
            var yaExisteSucursal = await repositorioSucursal.Existe(nombre);

            if (yaExisteSucursal)
            {
                return Json($"La sucursal {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var sucursal = await repositorioSucursal.ObtenerPorId(id);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(sucursal);
        }

        [HttpPost]
        public async Task<IActionResult> EditarSucursal(Sucursal sucursal) 
        {
            var sucursalExiste = await repositorioSucursal.ObtenerPorId(sucursal.Id);

            if (sucursalExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioSucursal.Actualizar(sucursal);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var sucursal = await repositorioSucursal.ObtenerPorId(id);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(sucursal);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarSucursal(int id) 
        {
            var sucursal = await repositorioSucursal.ObtenerPorId(id);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioSucursal.Borrar(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            

            return RedirectToAction("Index");
        }
    }
}
