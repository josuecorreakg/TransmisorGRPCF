using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Inventario
{
    public int Idfran { get; set; }

    public string IdProducto { get; set; } = null!;

    public int? Existencia { get; set; }

    public int? NoDisponible { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }
}
