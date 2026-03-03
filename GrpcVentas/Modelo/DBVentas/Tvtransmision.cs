using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("tv_transmision")]
    public class TvTransmision
    {
        [Key, Column("idfran", Order = 0)]
        public int IdFran { get; set; }

        [Key, Column("idOperacion", Order = 1)]
        public int IdOperacion { get; set; }

        [Required]
        [Column("fechainicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("fechafin")]
        public DateTime FechaFin { get; set; }

        [Column("status")]
        public sbyte Status { get; set; } // tinyint en MySQL mapea a sbyte o byte
    }
}
