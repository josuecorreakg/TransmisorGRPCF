using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioSurtido
{
    public int Idfran { get; set; }

    public int IdSurtido { get; set; }

    public int IdSurtidoLocal { get; set; }

    public short IdMovimiento { get; set; }

    public string Documento { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public DateTime FechaOperacion { get; set; }

    public DateTime FechaHoraCaptura { get; set; }

    public DateTime FechaOperacionDescarga { get; set; }

    public DateTime FechaFacturacion { get; set; }

    public string IdUsuario { get; set; } = null!;

    /// <summary>
    /// para buscar el documento en el websevice
    /// </summary>
    public string Factura { get; set; } = null!;

    /// <summary>
    /// para buscar el documento en el websevice
    /// </summary>
    public string FacturaFiscal { get; set; } = null!;

    public bool SurtidoElectronico { get; set; }

    public int IdProveedor { get; set; }

    public string IdFarmaciaSurtido { get; set; } = null!;

    /// <summary>
    /// 1:Pendiente, 2:Capturada, 3:?
    /// </summary>
    public byte Estatus { get; set; }

    public string Observacion { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public decimal Descuento { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public string Respaldo { get; set; } = null!;

    public byte Conteo { get; set; }

    public string SincRef { get; set; } = null!;

    public string? Signo { get; set; }

    public DateTime? Fechavencimiento { get; set; }

    public string? FacturaFiscalRef { get; set; }
}
