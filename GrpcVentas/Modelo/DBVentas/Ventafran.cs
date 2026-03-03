using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Ventafran
{
    public int Idfran { get; set; }

    public string Mes { get; set; } = null!;

    public string Ano { get; set; } = null!;

    public decimal Venta { get; set; }

    public decimal? VentaPe { get; set; }

    public decimal? VentaPn { get; set; }

    public decimal? Cepip { get; set; }

    public decimal? Vitaminas { get; set; }

    public decimal? Similares { get; set; }

    public decimal? Naturistas { get; set; }

    public decimal? Gravados { get; set; }

    public int? Clientes { get; set; }

    public decimal? Descuentos { get; set; }

    public decimal? Iva { get; set; }

    public decimal? ProdPremio { get; set; }

    public decimal? IvaSuple { get; set; }

    public decimal? IvaSupleT { get; set; }

    public int? Conmedm { get; set; }

    public int? Conmedv { get; set; }

    public int? Conmedn { get; set; }

    public int? Conmedx { get; set; }

    public decimal? PartVta { get; set; }

    public int? Idcat { get; set; }
}
