using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Veterinaria.Interfaces;
using Veterinaria.Models;
using Veterinaria.Servicios;

namespace Veterinaria.Controllers
{
    [Authorize]
    public class RazaController: Controller
    {
        private readonly IRepositorioEspecie repositorioEspecie;
        private readonly IRepositorioRaza repositorioRaza;
        private readonly IMapper mapper;

        public RazaController(IRepositorioEspecie repositorioEspecie, IRepositorioRaza repositorioRaza, IMapper mapper)
        {
            this.repositorioEspecie = repositorioEspecie;
            this.repositorioRaza = repositorioRaza;
            this.mapper = mapper;
        }

        /// <summary>
        /// Metodo para retornar la vista index, en este metodo se desea mostrar la lista de todas las razas
        /// y sus respectivas especies
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index() 
        {
            var razasConEspecie = await repositorioRaza.ObtenerRaza();

            // En esta parte la variable modelo va a recibir la lista de todas las razas y las va a agrupar por 
            // el nombre de la especie a la que pertenece y va a listar las razas por su tipo de especie
            var modelo = razasConEspecie
                .GroupBy(x => x.Especie)
                .Select(grupo => new IndiceRazaViewModel
                {
                    Especie = grupo.Key,
                    Razas = grupo.AsEnumerable()
                }).ToList();

            return View(modelo);
        }

        /// <summary>
        /// Metodo para enviar la lista de todas las Especies a la vista de la creacion de razas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            var especies = await repositorioEspecie.Obtener();
            var modelo = new RazaCreacionViewModel();
            modelo.ListaEspecies = await ObtenerEspecies();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(RazaCreacionViewModel raza) 
        {
            var especie = await repositorioEspecie.ObtenerPorId(raza.EspecieId);

            if (especie is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                raza.ListaEspecies = await ObtenerEspecies();
                return View(raza);
            }

            var yaExisteRaza = await repositorioRaza.Existe(raza.Nombre, raza.EspecieId);

            if (yaExisteRaza)
            {
                ModelState.AddModelError(nameof(raza.Nombre),
                            $"La raza {raza.Nombre} ya existe");

                return View(raza);
            }

            await repositorioRaza.Crear(raza);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteRaza(string nombre, int idEspecie) 
        {
            var yaExisteRaza = await repositorioRaza.Existe(nombre, idEspecie);

            if (yaExisteRaza)
            {
                return Json($"La raza {nombre} ya existe");
            }
            return Json(true);
        }


        public async Task<IEnumerable<SelectListItem>> ObtenerEspecies() 
        {
            var listaEspecies = await repositorioEspecie.Obtener();
            return listaEspecies.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        public async Task<IActionResult> Editar(int id) 
        {
            var raza = await repositorioRaza.ObtenerPorId(id);

            if (raza is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<RazaCreacionViewModel>(raza); // En esta linea de codigo mapeamos desde raza hacia RazaCreacionViewModel
            
           /* var modelo = new RazaCreacionViewModel()    Forma de mapear de una clase a otra de forma manual
            {
                Id = raza.Id,
                Nombre = raza.Nombre,
                EspecieId = raza.EspecieId
            };
           */

            modelo.ListaEspecies = await ObtenerEspecies();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EditarRaza(RazaCreacionViewModel razaEditar) 
        {
            var raza = await repositorioRaza.ObtenerPorId(razaEditar.Id);

            if ( raza is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var especie = await repositorioEspecie.ObtenerPorId(razaEditar.EspecieId);

            if (especie is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioRaza.Actualizar(razaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var raza = await repositorioRaza.ObtenerPorId(id);

            if (raza is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(raza);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarRaza(int id) 
        {
            var raza = await repositorioRaza.ObtenerPorId(id);

            if (raza is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioRaza.Borrar(id);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}
