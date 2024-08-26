using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesarrolloAprendeLibre.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    [NotMapped]
    public string? ConfirmarClave { get; set; }

    public int IdRol { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;
}
