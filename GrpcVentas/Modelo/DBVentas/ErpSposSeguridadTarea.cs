using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class ErpSposSeguridadTarea
{
    public int Idfran { get; set; }

    public int IdTarea { get; set; }

    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public sbyte? Orden { get; set; }

    public sbyte? Validar { get; set; }

    public sbyte? Permitir { get; set; }

    public sbyte? Consultar { get; set; }

    public sbyte? Crear { get; set; }

    public sbyte? Modificar { get; set; }

    public sbyte? Eliminar { get; set; }

    public sbyte? Imprimir { get; set; }

    public sbyte? Auditoria { get; set; }
}
