using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class Franquicia
{
    public int Empresa { get; set; }

    public string Clave { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Regpatron { get; set; } = null!;

    public string Domicilio { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Delmun { get; set; } = null!;

    public string Colonia { get; set; } = null!;

    public string Cp { get; set; } = null!;

    public string Tel1 { get; set; } = null!;

    public string Tel2 { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public decimal Comision { get; set; }

    public decimal Bono { get; set; }

    public decimal Premiomv { get; set; }

    public string Zona { get; set; } = null!;

    public byte Diapedido { get; set; }

    public bool Transmite { get; set; }

    public string? Supervisor { get; set; }

    public decimal Iva { get; set; }

    public bool Activa { get; set; }

    public string? Segnegocio { get; set; }

    public short Idfran { get; set; }

    public byte? Diaemergente { get; set; }

    public string Correo { get; set; } = null!;

    public DateTime FechaApertura { get; set; }

    public int? IdfranAnt { get; set; }

    public DateTime? Sinoperacion { get; set; }

    public double? VersionSyncro2 { get; set; }

    public bool? VersionSyncro2Activa { get; set; }
}
