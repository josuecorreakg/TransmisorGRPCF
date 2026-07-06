using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("tv_liberaciones")]
    public class TvLiberaciones
    {
        [Key, Column("idfran", Order = 0)]
        public int IdFran { get; set; }

        [Key, Column("clave", Order = 1)]
        public string Clave { get; set; }

        [Key, Column("nombre_sistema", Order = 2)]
        public string NombreSistema { get; set; }

        [Column("fecha_liberacion")]
        public DateTime? FechaLiberacion { get; set; }

        [Column("fecha_instalacion")]
        public DateTime? FechaInstalacion { get; set; }

        [Column("estatus")]
        public sbyte? Estatus { get; set; }

        [Column("ruta_evaluacion")]
        public string RutaEvaluacion { get; set; }
    }
}
