using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Pedido
{
    public int Idfran { get; set; }

    public int IdPedido { get; set; }

    public DateTime FechaOperacion { get; set; }

    public string IdUsuario { get; set; } = null!;

    /// <summary>
    /// 1 Semanal, 0 Resurtido Emergente
    /// </summary>
    public bool Semanal { get; set; }

    public DateTime FechaHoraCaptura { get; set; }

    public byte Estatus { get; set; }

    public string Observacion { get; set; } = null!;

    public byte Dias { get; set; }

    public byte Adicionales { get; set; }

    public DateTime FechaPedido { get; set; }

    public bool IncluirMenudeo { get; set; }

    public bool Definitivo { get; set; }

    public string? Folioconfirmacion { get; set; }

    public string? Foliopedido { get; set; }

    public string? Sincref { get; set; }

    public decimal? Estimado { get; set; }

    public bool? Pedidoemergente { get; set; }

    public string? IdAlmacenSurtido { get; set; }

    public int? IdFinanciamiento { get; set; }
}
