using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ErpSposSeguridadRol
{
    public int Idfran { get; set; }

    public int IdRol { get; set; }

    public int IdCategoria { get; set; }

    public sbyte? NivelSeguridad { get; set; }
}
