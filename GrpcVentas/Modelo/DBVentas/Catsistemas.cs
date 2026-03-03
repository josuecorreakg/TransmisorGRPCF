using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("cat_sistemas")]
    public class Catsistemas
    {
        [Key]
        [Column("idSistema")]
        public int IdSistema { get; set; }

        [Required]
        [MaxLength(45)]
        [Column("nombreSistema")]
        public string NombreSistema { get; set; } = null!;

        [Required]
        [Column("versionGral")]
        public double VersionGral { get; set; }
    }
}
