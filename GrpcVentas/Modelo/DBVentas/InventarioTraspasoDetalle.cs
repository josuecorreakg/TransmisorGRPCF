using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioTraspasoDetalle
{
    public int Idfran { get; set; }

    public string IdFarmaciaEntrega { get; set; } = null!;

    public int IdTraspaso { get; set; }

    public string IdProducto { get; set; } = null!;

    public int Solicitud { get; set; }

    public int Autorizado { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Importe { get; set; }
}
