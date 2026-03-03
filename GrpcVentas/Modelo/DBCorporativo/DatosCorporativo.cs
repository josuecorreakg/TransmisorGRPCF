using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrpcVentas.Modelo.DBCorporativo;

public partial class DatosCorporativo
{
    public int Corporativo { get; set; }
    [Key]
    public string? Dominio { get; set; } = null!;

    public string? DmnNcptd { get; set; }

    public string? Cnxn { get; set; }

    public string? Nombre { get; set; }

    public string? NombreCorto { get; set; }

    public string? Hst { get; set; }

    public string? ErpUsr { get; set; }

    public string? ErpPss { get; set; }

    public string? ErpDb { get; set; }

    public string? PresupuestosHost { get; set; }

    public string? PresupuestosUsr { get; set; }

    public string? PresupuestosPss { get; set; }

    public string? PresupuestosDb { get; set; }

    /// <summary>
    /// U
    /// </summary>
    public string? Dsn { get; set; }

    /// <summary>
    /// FE
    /// </summary>
    public string? Dsn1 { get; set; }

    /// <summary>
    /// Conexión para sistema de registro de asistencia.
    /// </summary>
    public string? Dsnasistencia { get; set; }

    public string? Dsntr { get; set; }

    public string? UsrSyncro2 { get; set; }

    public string? PssSyncro2 { get; set; }

    public short? MaxCnnSyncro2 { get; set; }

    public string? DbSyncro2 { get; set; }

    public string? DiasAudit { get; set; }

    public int? DiasEvaluar { get; set; }

    public string? HoraAudit { get; set; }

    public string? UsrAsis { get; set; }

    public string? PssAsis { get; set; }

    public sbyte? AplicaMonitor { get; set; }

    public sbyte? AplicaDocker { get; set; }

    public sbyte? PonderacionYdesabasto { get; set; }

    public sbyte? AplicaFacturacionEnLinea { get; set; }

    public sbyte? AplicaBitacora { get; set; }

    public sbyte? FacturacionEnAzure { get; set; }

    public sbyte? ActualizaDescuentosFacturacion { get; set; }

    public string? Razonsocial { get; set; }

    public string? Rfc { get; set; }

    public string? Uuid { get; set; } = null!;
}
