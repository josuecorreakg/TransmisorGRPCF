using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class InventarioTraspaso
{
    public int Idfran { get; set; }

    public string IdFarmaciaEntrega { get; set; } = null!;

    public int IdTraspaso { get; set; }

    public int? IdfranPedido { get; set; }

    public string IdFarmaciaPedido { get; set; } = null!;

    public int IdConcepto { get; set; }

    public short IdMovimiento { get; set; }

    public string Documento { get; set; } = null!;

    public string Referencia { get; set; } = null!;

    public DateTime FechaOperacion { get; set; }

    public DateTime FechaHoraCaptura { get; set; }

    public DateTime FechaHoraAutorizacion { get; set; }

    /// <summary>
    /// La persona que captura la salida (tienda origen), Captura el traspaso (tienda destino)
    /// </summary>
    public string IdUsuarioCaptura { get; set; } = null!;

    /// <summary>
    /// La persona que autoriza o rechaza el traspaso
    /// </summary>
    public string IdUsuarioAutoriza { get; set; } = null!;

    /// <summary>
    /// Solicitud, Autorizado, Rechazado y Capturado
    /// </summary>
    public byte Estatus { get; set; }

    public string SincRef { get; set; } = null!;

    public decimal? Total { get; set; }
}
