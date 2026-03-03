using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioFisicoCompleto
{
    public int Idfran { get; set; }

    public DateTime FechaOperacion { get; set; }

    public string IdProducto { get; set; } = null!;

    public bool Contado { get; set; }

    public int ExistenciaInicial { get; set; }

    public int Entradas { get; set; }

    public int Salidas { get; set; }

    public int ExistenciaFinal { get; set; }

    public decimal Costo { get; set; }
}
