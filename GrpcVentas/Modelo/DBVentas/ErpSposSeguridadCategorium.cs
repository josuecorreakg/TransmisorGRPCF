using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ErpSposSeguridadCategorium
{
    public int Idfran { get; set; }

    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public sbyte? Orden { get; set; }

    public sbyte? Reporte { get; set; }
}
