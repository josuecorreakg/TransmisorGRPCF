using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class TomaTemperatura
{
    public int Idfran { get; set; }

    public int IdToma { get; set; }

    public int? Temperatura { get; set; }

    public int? Humedad { get; set; }

    public DateTime? Fechaoperacion { get; set; }

    public string? HoraToma { get; set; }

    public string? UsuarioToma { get; set; }

    public string? NombreUsuario { get; set; }
}
