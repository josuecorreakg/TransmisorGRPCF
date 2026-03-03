using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class PedidoDetalle
{
    public int Idfran { get; set; }

    public int IdPedido { get; set; }

    public string IdProducto { get; set; } = null!;

    public DateTime UltimaVenta { get; set; }

    public int Sugerencia { get; set; }

    public int Pedido { get; set; }

    public int ExistenciaTeorica { get; set; }

    public decimal CostoUnitario { get; set; }
}
