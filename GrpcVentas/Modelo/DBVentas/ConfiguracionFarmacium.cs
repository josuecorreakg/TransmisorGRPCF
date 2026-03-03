using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ConfiguracionFarmacium
{
    public int Idfran { get; set; }

    public string Fran { get; set; } = null!;

    public sbyte IdConfiguracion { get; set; }

    public string IdFarmacia { get; set; } = null!;

    public string IdDivision { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string IdPais { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Municipio { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public int Codigopostal { get; set; }

    public string Colonia { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public string Noexterior { get; set; } = null!;

    public string Nointerior { get; set; } = null!;

    public string Domicilio { get; set; } = null!;

    public string Razonsocial { get; set; } = null!;

    public string Domiciliofiscal1 { get; set; } = null!;

    public string Domiciliofiscal2 { get; set; } = null!;

    public string Rfc1 { get; set; } = null!;

    public string Rfc2 { get; set; } = null!;

    public string Rfc3 { get; set; } = null!;

    public DateTime Fechaapertura { get; set; }

    public decimal Iva { get; set; }

    public bool Transaccionenlinea { get; set; }

    public string Horacierre { get; set; } = null!;

    public bool Facturacionelectronica { get; set; }

    public string RutaRdis { get; set; } = null!;

    public int IdNegocioFranquicia { get; set; }

    public bool Requerirasistencia { get; set; }

    public string WebserviceSincroniza { get; set; } = null!;

    public bool Tienegondolas { get; set; }

    public string Usuariocontivity { get; set; } = null!;

    public string Passwordcontivity { get; set; } = null!;

    public string Ipcontivity { get; set; } = null!;

    public bool Modificado { get; set; }

    public bool Modificadoiva { get; set; }

    public string IdUsuario { get; set; } = null!;

    public DateTime FechaCaptura { get; set; }

    public string IdAlmacen { get; set; } = null!;

    public string WebserviceVentas { get; set; } = null!;

    public string Regimenfiscal { get; set; } = null!;

    public string Afiliacionbancaria { get; set; } = null!;

    public string Leyendafiscal { get; set; } = null!;

    public bool Surtidodirecto { get; set; }

    public string Correoavisoprivacidad { get; set; } = null!;

    public int IdCategoriaventa { get; set; }

    //public string? HoraActualizacion { get; set; }

    //public sbyte? ActivoSap { get; set; }

    //public sbyte? Iepspos { get; set; }

    //public int? IdHorario { get; set; }
}
