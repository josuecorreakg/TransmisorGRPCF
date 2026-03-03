using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("control_version")]
    public class ControlVersion
    {
        [Key]
        [Column("idSistema", Order = 1)]
        public int IdSistema { get; set; }

        [Key]
        [Column("idFran", Order = 2)]
        public int IdFran { get; set; }

        [Required]
        [Column("version")]
        public double Version { get; set; }

        [Column("fechaLiberacion")]
        public DateTime? FechaLiberacion { get; set; }
    }
}
