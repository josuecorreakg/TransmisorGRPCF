using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Ventum
{
    public int Idfran { get; set; }

    public int IdVenta { get; set; }

    public int IdVentaLocal { get; set; }

    public short IdVentaConsecutivo { get; set; }

    public int? IdMovimiento { get; set; }

    public int IdVentaRegistradora { get; set; }

    public byte IdRegistradoraVenta { get; set; }

    public byte IdRegistradoraCobro { get; set; }

    public string IdUsuarioVenta { get; set; } = null!;

    public string IdUsuarioCobro { get; set; } = null!;

    public string IdUsuarioCancelacion { get; set; } = null!;

    public DateTime FechaHoraVenta { get; set; }

    public DateTime FechaHoraCobro { get; set; }

    public DateTime FechaHoraCancelacion { get; set; }

    public DateTime FechaOperacion { get; set; }

    public byte TipoVenta { get; set; }

    public byte TipoOperacion { get; set; }

    public string IdVentaReferencia { get; set; } = null!;

    public bool Receta { get; set; }

    public bool AntesTotal { get; set; }

    public byte Estatus { get; set; }

    public int? Historico { get; set; }

    public byte? Sincroniza { get; set; }

    public int? IdCliente { get; set; }

    public short? PuntosIniciales { get; set; }

    public short? PuntosFinales { get; set; }

    public short? PuntosAcumulados { get; set; }

    public byte? Restriccion { get; set; }
}
