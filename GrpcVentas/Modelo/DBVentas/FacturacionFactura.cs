using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class FacturacionFactura
{
    public int Idfran { get; set; }

    public string Serie { get; set; } = null!;

    public int Folio { get; set; }

    public string IdCliente { get; set; } = null!;

    public bool Global { get; set; }

    public byte Estatus { get; set; }

    public DateTime FechaOperacion { get; set; }

    public DateTime FechaFacturacion { get; set; }

    public DateTime FechaCancelacion { get; set; }

    public bool Electronica { get; set; }

    public byte Reimpresiones { get; set; }

    public bool NotaCredito { get; set; }

    public bool Fundacion { get; set; }

    public bool Franquicia { get; set; }

    public bool Correo { get; set; }

    public int ContadorItems { get; set; }

    public sbyte? SifeEstatus { get; set; }
}
