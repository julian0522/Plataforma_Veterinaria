using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Veterinaria.Models;

namespace Veterinaria.Controllers
{
    public class UsuarioController: Controller
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Registro() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo) 
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new Usuario() { Email = modelo.Email,
                                        Nombre = modelo.Nombre,
                                        Telefono = modelo.Telefono,
                                        Cedula = modelo.Cedula,
                                        Edad = modelo.Edad };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Cliente");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }

                return View(modelo);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index","Cliente");
        }

        [HttpGet]
        public IActionResult Login() 
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo) 
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            // El lockoutOnFailure hace referencia a las veces que el usuario puede errar la autenticacion en el login
            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password,
                                                                    modelo.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index","Cliente");
            }
            else
            {

               ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(modelo);
            }
        }
    }
}
