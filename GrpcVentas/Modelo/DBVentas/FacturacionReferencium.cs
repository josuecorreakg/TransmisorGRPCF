using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class FacturacionReferencium
{
    public int Idfran { get; set; }

    public string Serie { get; set; } = null!;

    public int Folio { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public string ReferenciaTicket { get; set; } = null!;

    public decimal SubTotalNeto { get; set; }

    public decimal Descuento { get; set; }

    public decimal TotalNeto { get; set; }

    public decimal Iva { get; set; }

    public decimal GranTotal { get; set; }
}
