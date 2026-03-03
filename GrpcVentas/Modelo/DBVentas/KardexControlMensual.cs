using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcVentas.Modelo.DBVentas
{
    public class KardexControlMensual
    {
        [Key, Column("id_fran", Order = 0)]
        public int IdFran { get; set; }

        [Required, MaxLength(6), Column("clave")]
        public string Clave { get; set; }

        [Required, MaxLength(50), Column("franquicia")]
        public string Franquicia { get; set; }

        [Key, Required, MaxLength(10), Column("id_producto", Order = 1)]
        public string IdProducto { get; set; }

        [Required, MaxLength(100), Column("producto")]
        public string Producto { get; set; }

        [Key, Required, MaxLength(2), Column("mes", Order = 2)]
        public string Mes { get; set; }

        [Key, Required, MaxLength(4), Column("periodo", Order = 3)]
        public string Periodo { get; set; }

        // --- INVENTARIO INICIAL ---
        [Column("inventario_inicial_monto", TypeName = "decimal(19,2)")]
        public decimal InventarioInicialMonto { get; set; }

        [Column("inventario_inicial_piezas")]
        public int InventarioInicialPiezas { get; set; }

        [Column("inventario_inicial_costo_unitario", TypeName = "decimal(19,2)")]
        public decimal InventarioInicialCostoUnitario { get; set; }

        // --- COMPRAS Y ENTRADAS ---
        [Column("compras_similares_monto", TypeName = "decimal(19,2)")]
        public decimal ComprasSimilaresMonto { get; set; }

        [Column("compras_similares_piezas")]
        public int ComprasSimilaresPiezas { get; set; }

        [Column("compras_similares_costo_unitario", TypeName = "decimal(19,2)")]
        public decimal ComprasSimilaresCostoUnitario { get; set; }

        [Column("entrada_compra_proveedores_monto", TypeName = "decimal(19,2)")]
        public decimal EntradaCompraProveedoresMonto { get; set; }

        [Column("entrada_compra_proveedores_piezas")]
        public int EntradaCompraProveedoresPiezas { get; set; }

        [Column("entradas_traspaso_monto", TypeName = "decimal(19,2)")]
        public decimal EntradasTraspasoMonto { get; set; }

        [Column("entradas_traspaso_piezas")]
        public int EntradasTraspasoPiezas { get; set; }

        // --- DISPONIBLE ---
        [Column("disponible_monto", TypeName = "decimal(19,2)")]
        public decimal DisponibleMonto { get; set; }

        [Column("disponible_piezas")]
        public int DisponiblePiezas { get; set; }

        [Column("disponible_costo_unitario", TypeName = "decimal(19,2)")]
        public decimal DisponibleCostoUnitario { get; set; }

        // --- SALIDAS Y DEVOLUCIONES ---
        [Column("salidas_por_venta_monto", TypeName = "decimal(19,2)")]
        public decimal SalidasPorVentaMonto { get; set; }

        [Column("salidas_por_venta_piezas")]
        public int SalidasPorVentaPiezas { get; set; }

        [Column("devoluciones_por_venta_monto", TypeName = "decimal(19,2)")]
        public decimal DevolucionesPorVentaMonto { get; set; }

        [Column("devoluciones_por_venta_piezas")]
        public int DevolucionesPorVentaPiezas { get; set; }

        [Column("salidas_traspaso_monto", TypeName = "decimal(19,2)")]
        public decimal SalidasTraspasoMonto { get; set; }

        [Column("salidas_traspaso_piezas")]
        public int SalidasTraspasoPiezas { get; set; }

        // --- AJUSTES ---
        [Column("entradas_ajustes_monto", TypeName = "decimal(19,2)")]
        public decimal EntradasAjustesMonto { get; set; }

        [Column("entradas_ajustes_piezas")]
        public int EntradasAjustesPiezas { get; set; }

        [Column("salidas_ajustes_monto", TypeName = "decimal(19,2)")]
        public decimal SalidasAjustesMonto { get; set; }

        [Column("salidas_ajustes_piezas")]
        public int SalidasAjustesPiezas { get; set; }

        [Column("salidas_ajustes_degustacion_monto", TypeName = "decimal(19,2)")]
        public decimal SalidasAjustesDegustacionMonto { get; set; }

        [Column("salidas_ajustes_degustacion_piezas")]
        public int SalidasAjustesDegustacionPiezas { get; set; }

        [Column("salidas_ajustes_siniestros_monto", TypeName = "decimal(19,2)")]
        public decimal SalidasAjustesSiniestrosMonto { get; set; }

        [Column("salidas_ajustes_siniestros_piezas")]
        public int SalidasAjustesSiniestrosPiezas { get; set; }

        [Column("devoluciones_similares_monto", TypeName = "decimal(19,2)")]
        public decimal DevolucionesSimilaresMonto { get; set; }

        [Column("devoluciones_similares_piezas")]
        public int DevolucionesSimilaresPiezas { get; set; }

        // --- TOTALES Y FINAL ---
        [Column("total_salida_monto", TypeName = "decimal(19,2)")]
        public decimal TotalSalidaMonto { get; set; }

        [Column("total_salida_piezas")]
        public int TotalSalidaPiezas { get; set; }

        [Column("total_entrada_monto", TypeName = "decimal(19,2)")]
        public decimal TotalEntradaMonto { get; set; }

        [Column("total_entrada_piezas")]
        public int TotalEntradaPiezas { get; set; }

        [Column("inventario_final_monto", TypeName = "decimal(19,2)")]
        public decimal InventarioFinalMonto { get; set; }

        [Column("inventario_final_piezas")]
        public int InventarioFinalPiezas { get; set; }

        [Column("inventario_final_costo_unitario", TypeName = "decimal(19,2)")]
        public decimal InventarioFinalCostoUnitario { get; set; }
    }
}
