using System;
using System.Collections.Generic;

namespace DesarrolloAprendeLibre.Models;

public partial class Libro
{
    public int IdLibro { get; set; }

    public string Autor { get; set; } = null!;

    public string NombrePortada { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
}
