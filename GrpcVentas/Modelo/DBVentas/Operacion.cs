using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Operacion
{
    public int Idfran { get; set; }

    public DateTime FechaOperacion { get; set; }

    public DateTime FechaHoraApertura { get; set; }

    public string? IdUsuarioApertura { get; set; }

    public DateTime FechaHoraCierre { get; set; }

    public string? IdUsuarioCierre { get; set; }

    public string? IdUsuarioVendedor { get; set; }

    public string? IdUsuarioCajero { get; set; }

    public bool Estatus { get; set; }

    public decimal GranTotalApertura { get; set; }

    public decimal GranTotalCierre { get; set; }

    public bool EstatusPv { get; set; }

    public byte EstatusCierre { get; set; }

    public bool EnvioCierre { get; set; }

    public DateTime FechaHoraEnvio { get; set; }
}
