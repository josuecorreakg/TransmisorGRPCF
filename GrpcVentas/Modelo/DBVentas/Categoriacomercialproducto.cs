using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Categoriacomercialproducto
{
    public int IdCategoriaComercial { get; set; }

    public string IdProducto { get; set; } = null!;

    public bool EstatusRegistro { get; set; }
}
