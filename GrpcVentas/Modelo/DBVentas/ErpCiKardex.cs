using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ErpCiKardex
{
    public int IdFran { get; set; }

    public string Clave { get; set; } = null!;

    public string Franquicia { get; set; } = null!;

    public string IdProducto { get; set; }

    public string Producto { get; set; } = null!;

    public DateTime FechaOperacion { get; set; }

    public decimal? PrecioVenta { get; set; }

    public decimal? PrecioVentaSinIva { get; set; }

    public decimal? PrecioCompra { get; set; }

    public int InventarioInicial { get; set; }

    public int EntradasTraspaso { get; set; }

    public int SalidasTraspaso { get; set; }

    public int EntradasAjustes { get; set; }

    public int SalidasAjustes { get; set; }

    public int? EntradaCompraProveedores { get; set; }

    public int EntradasCompras { get; set; }

    public int SalidasNotasCredito { get; set; }

    public int SalidasVentas { get; set; }

    public int EntradaDevoluciones { get; set; }

    public int InventarioFinal { get; set; }
    public int SalidasAjustesDegustacionPiezas { get; set; }
    public int SalidasAjustesSiniestrosPiezas { get; set; }
}
