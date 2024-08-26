using DesarrolloAprendeLibre.Models;
using DesarrolloAprendeLibre.Permisos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DesarrolloAprendeLibre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CerrarSesion()
        {
            // Eliminar todas las variables de sesi�n
            HttpContext.Session.Clear();

            // Redirigir al usuario a la p�gina de inicio de sesi�n
            return RedirectToAction("Index", "Acceso");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
