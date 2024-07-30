using System;
using System.Collections.Generic;

namespace DesarrolloAprendeLibre.Models;

public partial class Clase
{
    public int IdClase { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Materia { get; set; } = null!;

    public string? SubirArchivo { get; set; }

    public string? Imagen { get; set; }
}
