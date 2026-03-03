using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioSurtidoDetalle
{
    public int Idfran { get; set; }

    public int IdSurtido { get; set; }

    public int IdSurtidoLocal { get; set; }

    public string IdProducto { get; set; } = null!;

    public int Remision { get; set; }

    public int Conteo1 { get; set; }

    public int Conteo2 { get; set; }

    public int MalEstado { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Descuento { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Ivaporciento { get; set; }

    public decimal? Descuentoporciento { get; set; }
}
