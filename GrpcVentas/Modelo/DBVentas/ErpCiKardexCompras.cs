using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    /// <summary>
    /// Modelo de entidad para la tabla 'erp_ci_kardex_compras'.
    /// </summary>
    [Table("erp_ci_kardex_compras")]
    public class ErpCiKardexCompras
    {
        [Key]
        [Column("Id_Surtido")]
        public int IdSurtido { get; set; }
        [Key]
        [Column("Idfran")]
        public int Idfran { get; set; }
        [Column("FechaOperacion")]
        public DateTime FechaOperacion { get; set; }
        [Column("FechaHora_Captura")]
        public DateTime FechaHoraCaptura { get; set; }
        [Column("FechaOperacion_Descarga")]
        public DateTime FechaOperacionDescarga { get; set; }
        [StringLength(45)]
        [Column("Factura_Fiscal")]
        public string Factura_Fiscal { get; set; }
        [Column("Total", TypeName = "decimal(19,2)")]
        public decimal Total { get; set; }
        [StringLength(2)]
        [Column("Signo")]
        public string Signo { get; set; }
        [Column("TotalComprasSoloVenta", TypeName = "decimal(19,2)")]
        public decimal TotalComprasSoloVenta { get; set; }
    }


}
