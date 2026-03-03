using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentaProductoDesglose
{
    public int Idfran { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public string IdProducto { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal Precio { get; set; }

    public decimal Iva { get; set; }

    public decimal Importe { get; set; }
}
