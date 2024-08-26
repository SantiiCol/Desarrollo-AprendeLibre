using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DesarrolloAprendeLibre.Models;

namespace DesarrolloAprendeLibre.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            // Verificar si la sesión contiene el usuario
            if (session.GetInt32("IdUsuario") == null)
            {
                // Redirigir al usuario a la página de inicio de sesión
                context.Result = new RedirectToActionResult("Index", "Acceso", null);
            }

            base.OnActionExecuting(context);
        }

    }
}
