using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Veterinaria.Interfaces;
using Veterinaria.Models;

namespace Veterinaria.Controllers
{
    [Authorize]
    public class EspecieController : Controller
    {
        private readonly IRepositorioEspecie repositorioEspecie;

        public EspecieController(IRepositorioEspecie repositorioEspecie)
        {
            this.repositorioEspecie = repositorioEspecie;
        }

        public async Task<IActionResult> Index()
        {
            var especies = await repositorioEspecie.Obtener();
            return View(especies);
        }


        /// <summary>
        /// Metodo que devuelve la vista crear de la especie
        /// </summary>
        /// <returns></returns>
        public IActionResult Crear()
        {
            return View();
        }


        /// <summary>
        /// Metodo que realiza la funcion de crear la especie en la base de datos
        /// </summary>
        /// <param name="especie"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Crear(Especie especie)
        {
            // con este if determinamos que si el estado del modelo no es valido
            if (!ModelState.IsValid)
            {
                // al mandarle la especie, vamos a poder volver a llenar el formulario
                // con la informacion ya diligenciada sin que se actualice y perder los datos ya digitados
                return View(especie);
            }

            var yaExisteEspecie = await repositorioEspecie.Existe(especie.Nombre);

            if (yaExisteEspecie) // Si la especie ya esxite agregamos un error que mostrara en la panatalla
            {
                ModelState.AddModelError(nameof(especie.Nombre),
                    $"La especie {especie.Nombre} ya existe");

                return View(especie);
            }

            await repositorioEspecie.Crear(especie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteEspecie(string nombre)
        {
            var yaexisteEspecie = await repositorioEspecie.Existe(nombre);

            if (yaexisteEspecie)
            {
                return Json($"La especie {nombre} ya existe");
            }

            return Json(true);
        }


        /// <summary>
        /// Metodo que permite cargar la pagina de editar el registro por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Editar(int id)
            {
            var especie = await repositorioEspecie.ObtenerPorId(id);

            if (especie is null)
            {
                return RedirectToAction("No encontrado", "Home");
            }

            return View(especie);
        }


        /// <summary>
        /// Metodo que realiza la accion de actualizar laa especie seleccionada
        /// </summary>
        /// <param name="especie"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditarEspecie(Especie especie) 
        {
            var especieExiste = await repositorioEspecie.ObtenerPorId(especie.Id);

            if (especieExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioEspecie.Actualizar(especie);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var especie = await repositorioEspecie.ObtenerPorId(id);

            if (especie is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(especie);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarEspecie(int id) 
        {
            var especie = await repositorioEspecie.ObtenerPorId(id);

            if (especie is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioEspecie.Borrar(id);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
            
            return RedirectToAction("Index");
        }

    }
}
