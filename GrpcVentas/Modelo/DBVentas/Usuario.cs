using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Usuario
{
    public int Idfran { get; set; }

    public string IdUsuario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public short IdIdioma { get; set; }

    public string LlaveAcceso { get; set; } = null!;

    public decimal FechaUltimoCambio { get; set; }

    public bool CambiarLlaveAcceso { get; set; }

    public bool Temporal { get; set; }

    public bool EstatusRegistro { get; set; }
}
