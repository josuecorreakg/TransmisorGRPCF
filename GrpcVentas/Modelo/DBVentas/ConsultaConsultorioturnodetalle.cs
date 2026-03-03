using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ConsultaConsultorioturnodetalle
{
    public int Idfran { get; set; }

    public int IdConsultorioTurno { get; set; }

    public string IdConsultorio { get; set; } = null!;

    public string IdTurno { get; set; } = null!;

    public int IdMotivoSinConsultas { get; set; }

    public bool CorreccionCaptura { get; set; }

    public string Observaciones { get; set; } = null!;

    public DateTime FechaCaptura { get; set; }

    public string UsuarioCaptura { get; set; } = null!;

    public DateTime FechaModificacion { get; set; }
}
