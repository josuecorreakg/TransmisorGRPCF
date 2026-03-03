using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentaDescuento
{
    public int Idfran { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public int IdDescuento { get; set; }

    public decimal Descuento { get; set; }

    public byte ProductoBasico { get; set; }

    public string Tarjeta { get; set; } = null!;

    public string ClienteNombre { get; set; } = null!;

    public string DoctorNombre { get; set; } = null!;

    public string DoctorCedula { get; set; } = null!;

    public string Receta { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public decimal? DescuentoPorciento { get; set; }
}
