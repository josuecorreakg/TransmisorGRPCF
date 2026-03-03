using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentaPago
{
    public int Idfran { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public string IdFormaPago { get; set; } = null!;

    public decimal Importe { get; set; }

    public decimal TipoCambio { get; set; }
}
