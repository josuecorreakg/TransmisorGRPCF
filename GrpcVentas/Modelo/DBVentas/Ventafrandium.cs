using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Ventafrandium
{
    public int Idfran { get; set; }

    public string Mes { get; set; } = null!;

    public string Ano { get; set; } = null!;

    public int Dia { get; set; }

    public decimal? Venta { get; set; }

    public decimal? Naturistas { get; set; }

    public decimal? Inventario { get; set; }

    public decimal? InventarioCompra { get; set; }

    public DateTime? FechaCalculo { get; set; }
}
