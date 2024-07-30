using System;
using System.Collections.Generic;

namespace DesarrolloAprendeLibre.Models;

public partial class Recurso
{
    public int IdRecurso { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Materia { get; set; } = null!;

    public string? Imagen { get; set; }
}
