using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ProductoSuplementoFranquicium
{
    public int IdFran { get; set; }

    public string IdProducto { get; set; } = null!;

    public DateTime UltimaActualizacion { get; set; }
}
