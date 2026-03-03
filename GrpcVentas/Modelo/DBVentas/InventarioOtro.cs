using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioOtro
{
    public int Idfran { get; set; }

    public int IdRegistro { get; set; }

    public int IdMovimiento { get; set; }

    public string Documento { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public string Signo { get; set; } = null!;

    public DateTime FechaOperacion { get; set; }

    public DateTime FechaHoraCaptura { get; set; }

    public string IdUsuario { get; set; } = null!;

    public string Observacion { get; set; } = null!;

    public string SincRef { get; set; } = null!;

    public int IdTipo { get; set; }
}
