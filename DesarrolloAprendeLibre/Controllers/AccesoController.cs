// Librerias para las verificaciones de usuario y el manejo de procedimientos almacenados
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using DesarrolloAprendeLibre.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

// Librerias para el manejo de las cookies
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DesarrolloAprendeLibre.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AplDbContext _context;

        public AccesoController(AplDbContext context)
        {
            _context = context;
        }

        // accion para mostrar la vista del inicio de sesion
        public IActionResult Login()
        {
            return View();
        }

        // Acción para mostrar la vista de registro de usuario con los roles disponibles
        public IActionResult Register()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View();
        }

        // Acción HTTP POST para registrar un nuevo usuario
        [HttpPost]
        public IActionResult Register(Usuario _usuario)
        {
            // Verifica si las contraseñas coinciden
            if (_usuario.Clave != _usuario.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
                return View();
            }

            // Encripta la contraseña usando SHA-256
            _usuario.Clave = ConvertirSha256(_usuario.Clave);

            bool registrado;
            string mensaje;

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection cn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("Nombres", _usuario.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", _usuario.Apellidos);
                cmd.Parameters.AddWithValue("Correo", _usuario.Correo);
                cmd.Parameters.AddWithValue("Clave", _usuario.Clave);
                cmd.Parameters.AddWithValue("IdRol", _usuario.IdRol);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
                return View(_usuario);
            }
        }

        // Acción HTTP POST para iniciar sesión
        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            // Verifica si el correo y la clave no son nulos
            if (string.IsNullOrEmpty(_usuario.Correo) || string.IsNullOrEmpty(_usuario.Clave))
            {
                ViewData["Mensaje"] = "Correo y Clave son requeridos";
                return View();
            }

            // Encripta la clave antes de hacer la consulta
            _usuario.Clave = ConvertirSha256(_usuario.Clave);

            // Conexión a la base de datos y ejecución del procedimiento almacenado para validar el usuario
            using (SqlConnection cn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("Correo", _usuario.Correo);
                cmd.Parameters.AddWithValue("Clave", _usuario.Clave);

                cn.Open();

                // Obtiene el ID del usuario si la validación es exitosa
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int userId))
                {
                    _usuario.IdUsuario = userId;
                }
                else
                {
                    _usuario.IdUsuario = 0;
                }

                // Si el usuario es válido, obtiene la información completa del usuario, incluyendo el rol
                if (_usuario.IdUsuario != 0)
                {
                    var usuarioConRol = _context.Usuarios
                        .Include(u => u.IdRolNavigation)
                        .FirstOrDefault(u => u.IdUsuario == _usuario.IdUsuario);

                    if (usuarioConRol != null)
                    {
                        _usuario = usuarioConRol;

                        // Crea los claims de identidad del usuario
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _usuario.Nombres),
                        new Claim("Correo", _usuario.Correo),
                    };

                        if (_usuario.IdRolNavigation != null && !string.IsNullOrEmpty(_usuario.IdRolNavigation.NombreRol))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, _usuario.IdRolNavigation.NombreRol));
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Inicia sesión estableciendo los claims
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        // Establece la sesión del usuario
                        HttpContext.Session.SetInt32("IdUsuario", _usuario.IdUsuario);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario no encontrado";
                    return View();
                }
            }
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        // Método para encriptar la clave usando SHA-256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
