using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioOtrosDetalle
{
    public int Idfran { get; set; }

    public int IdRegistro { get; set; }

    public string IdProducto { get; set; } = null!;

    public int Cantidad { get; set; }
}
