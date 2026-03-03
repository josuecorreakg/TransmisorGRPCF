using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("erp_ci_costo_oportunidad")]
    public class CostoOp
    {
        [Key, Column(Order = 0)]
        public int IdFran { get; set; }

        public string? Clave { get; set; }

        [Key, Column(Order = 1)]
        public int IdProducto { get; set; }

        public string? Producto { get; set; }

        public DateTime FechaOperacion { get; set; }
        public DateTime FechaTransmision { get; set; }

        public decimal? PrecioVenta { get; set; }
        public int? InventarioActual { get; set; }
        public int? VentasDia1 { get; set; }
        public int? PromDia1 { get; set; }
        public int? VentasDia2 { get; set; }
        public int? PromDia2 { get; set; }
        public int? VentasDia3 { get; set; }
        public int? PromDia3 { get; set; }
        public int? VentasDia4 { get; set; }
        public int? PromDia4 { get; set; }
        public int? VentasDia5 { get; set; }
        public int? PromDia5 { get; set; }
        public int? VentasDia6 { get; set; }
        public int? PromDia6 { get; set; }
        public int? VentasDia7 { get; set; }
        public int? PromDia7 { get; set; }
    }
}
