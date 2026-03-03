using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ConsultaRespuestum
{
    public int Idfran { get; set; }

    public int IdConsultorioTurno { get; set; }

    public int IdConsultaPregunta { get; set; }

    public string Respuesta { get; set; } = null!;
}
