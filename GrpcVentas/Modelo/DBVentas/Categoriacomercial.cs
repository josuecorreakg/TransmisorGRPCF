using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Categoriacomercial
{
    public int IdCategoriaComercial { get; set; }

    public string Nombre { get; set; } = null!;

    public bool EstatusRegistro { get; set; }

    public string Grupo { get; set; } = null!;

    public int Orden { get; set; }
}
