using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ErpSposUsuarioRol
{
    public int Idfran { get; set; }

    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string? EstatusRegistro { get; set; }
}
