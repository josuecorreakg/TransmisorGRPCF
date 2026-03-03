using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentaProducto
{
    public int Idfran { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public string IdProducto { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal Precio { get; set; }

    public decimal Iva { get; set; }

    public decimal Descuento { get; set; }

    public decimal DescuentoPorciento { get; set; }

    public short Puntos { get; set; }

    public decimal IvaPorciento { get; set; }

    public decimal IvaImporte { get; set; }

    public short Posicion { get; set; }

    public bool? Premio { get; set; }
}
