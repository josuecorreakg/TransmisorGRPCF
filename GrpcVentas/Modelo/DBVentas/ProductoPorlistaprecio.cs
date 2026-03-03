using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ProductoPorlistaprecio
{
    public int Idfran { get; set; }

    public sbyte IdListaprecio { get; set; }

    public string IdProducto { get; set; } = null!;

    public decimal? Precio { get; set; }

    public sbyte? Estatusregistro { get; set; }

    /// <summary>
    /// Fecha que actualiza syncro2
    /// </summary>
    public DateTime? Ultimaactualizacion { get; set; }
}
