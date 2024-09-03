using DesarrolloAprendeLibre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DesarrolloAprendeLibre.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {

        private readonly AplDbContext _context;

        public AdministradorController(AplDbContext context)
        {
            _context = context;
        }

        public IActionResult PanelAdministrador()
        {
            return View(); // Vista donde se muestran opciones exclusivas para el administrador
        }
        public IActionResult CrearModerador()
        {
            return View();
        }

        // Acción HTTP POST para crear un nuevo moderador
        [HttpPost]
        public IActionResult CrearModerador(Moderador moderador)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Encriptar la contraseña
                    moderador.Clave = AccesoController.ConvertirSha256(moderador.Clave);

                    // Guardar el nuevo moderador en la base de datos
                    _context.Moderadors.Add(moderador);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Moderador creado exitosamente.";
                    return RedirectToAction("PanelModerador");
                }
                catch (SqlException ex)
                {
                    // Manejar errores de SQL, por ejemplo, problemas de conexión o restricciones de base de datos
                    TempData["ErrorMessage"] = "Ocurrió un problema con la base de datos: " + ex.Message;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier otro tipo de excepción
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado: " + ex.Message;
                }
            }

            // Si el modelo no es válido o se captura una excepción, vuelve a la vista con el modelo actual
            return View(moderador);
        }
    }
}
