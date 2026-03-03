using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentaRecetaControlado
{
    public int Idfran { get; set; }

    public string? IdReceta { get; set; }

    public int IdVenta { get; set; }

    public ulong? Retenerreceta { get; set; }

    public DateTime? Fechacaptura { get; set; }

    public int? IdMedico { get; set; }

    public string? Nombre { get; set; }

    public string? Cedula { get; set; }

    public string? Domicilio { get; set; }

    public string? Tipo { get; set; }
}
