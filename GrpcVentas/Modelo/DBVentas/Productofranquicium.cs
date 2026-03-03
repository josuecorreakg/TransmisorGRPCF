using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Productofranquicium
{
    public int Idfran { get; set; }

    public string IdProducto { get; set; } = null!;

    public string IdNivel1 { get; set; } = null!;

    public string IdNivel2 { get; set; } = null!;

    public string IdNivel3 { get; set; } = null!;

    public int IdArticulo { get; set; }

    public short IdPresentacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string MarcaEconomica { get; set; } = null!;

    public decimal PrecioCompra { get; set; }

    public decimal Precio { get; set; }

    public decimal UltimoCosto { get; set; }

    public decimal Iva { get; set; }

    public bool Inventario { get; set; }

    public bool InventarioDiario { get; set; }

    public bool Combo { get; set; }

    public bool Otc { get; set; }

    public bool Venta { get; set; }

    public bool Servicio { get; set; }

    public bool Premio { get; set; }

    public int EstructuraNegocio { get; set; }

    public bool AplicaCaducidad { get; set; }

    public bool AplicaDescuento { get; set; }

    public bool ProductoBasico { get; set; }

    public short AsignaPuntos { get; set; }

    public short PrecioPuntos { get; set; }

    public bool ProductoGondola { get; set; }

    public bool EstatusRegistro { get; set; }

    public bool? Controlado { get; set; }

    public string? DescripcionCorta { get; set; }

    public bool? FueradeCatalogo { get; set; }

    public bool? NoPonderado { get; set; }

    public int? CantidadPresentacion { get; set; }

    public DateTime? FechaInclusion { get; set; }

    public string? IdProductosat { get; set; }

    public decimal? Ieps { get; set; }

    public DateTime UltimaActualizacion { get; set; }
}
