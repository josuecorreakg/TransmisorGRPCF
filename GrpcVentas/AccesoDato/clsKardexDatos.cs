using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsKardexDatos
    {
        public static DataResponseKardex InsertarKardexManual(List<protoErpCiKardex> lsCiKardex, List<protoErpCiKardexCompras> lskardexCompras, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso){
            DataResponseKardex objRespuesta = new DataResponseKardex();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Conversión de datos para erp_ci_kardex
                        var objKardex = clsGeneral.ConvertirListaAEntidad<protoErpCiKardex, ErpCiKardex>(lsCiKardex);
                        var objKardexCompras = clsGeneral.ConvertirListaAEntidad<protoErpCiKardexCompras, ErpCiKardexCompras>(lskardexCompras);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // --- 1. Inserción Masiva para erp_ci_kardex ---
                        if (objKardex.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO erp_ci_kardex (id_fran, clave, franquicia, id_producto, producto, fecha_operacion, precio_venta, precio_venta_sin_iva, precio_compra, inventario_inicial, entradas_traspaso, salidas_traspaso, entradas_ajustes, salidas_ajustes, entrada_compra_proveedores, entradas_compras, salidas_notas_credito, salidas_ventas, entrada_devoluciones, inventario_final, salidas_ajustes_degustacion_piezas, salidas_ajustes_siniestros_piezas) VALUES ");

                            var paramIndex = 0;
                            for (int i = 0; i < objKardex.Count; i++)
                            {
                                var item = objKardex[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}),");

                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Clave);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Franquicia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", int.Parse(item.IdProducto));
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Producto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PrecioVenta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PrecioVentaSinIva ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PrecioCompra ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.InventarioInicial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradasTraspaso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasTraspaso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradasAjustes);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustes);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.EntradaCompraProveedores ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradasCompras);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasNotasCredito);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasVentas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradaDevoluciones);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.InventarioFinal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustesDegustacionPiezas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustesSiniestrosPiezas);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // --- 2. Inserción Masiva para erp_ci_kardex_compras ---
                        if (objKardexCompras.Count > 0)
                        {
                            var cmdCompras = connection.CreateCommand();
                            cmdCompras.Transaction = mySqlTransaction;
                            cmdCompras.CommandTimeout = 180;
                            // El comando debe usar REPLACE INTO porque la tabla tiene clave compuesta (Id_Surtido, Idfran)
                            var sbCompras = new StringBuilder("REPLACE INTO erp_ci_kardex_compras (Id_Surtido, Idfran, FechaOperacion, FechaHora_Captura, FechaOperacion_Descarga, Factura_Fiscal, Total, Signo, TotalComprasSoloVenta) VALUES ");

                            var paramIndexCompras = 0;
                            for (int i = 0; i < objKardexCompras.Count; i++)
                            {
                                var item = objKardexCompras[i];
                                sbCompras.Append($"(@c{paramIndexCompras}, @c{paramIndexCompras + 1}, @c{paramIndexCompras + 2}, @c{paramIndexCompras + 3}, @c{paramIndexCompras + 4}, @c{paramIndexCompras + 5}, @c{paramIndexCompras + 6}, @c{paramIndexCompras + 7}, @c{paramIndexCompras + 8}),");

                                // Nota: Uso 'c' como prefijo para los parámetros de Compras para evitar colisiones con los parámetros de Kardex si se ejecutan de forma concurrente,
                                // aunque en este caso se está usando un nuevo 'cmdCompras' por lo que no es estrictamente necesario, pero es buena práctica.

                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", item.IdSurtido);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", item.Idfran);
                                // Conversión de string a DateTime para los campos de fecha. Se asume que el DTO maneja la conversión si la propiedad es DateTime.
                                // Si ErpCiKardexCompras tiene string, se usa el string. Si tiene DateTime, se debe parsear.
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.FechaOperacion ?? DBNull.Value); // Asumiendo que se debe convertir
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.FechaHoraCaptura ?? DBNull.Value);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.FechaOperacionDescarga ?? DBNull.Value);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.Factura_Fiscal ?? DBNull.Value);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.Total ?? DBNull.Value);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.Signo ?? DBNull.Value);
                                cmdCompras.Parameters.AddWithValue($"@c{paramIndexCompras++}", (object)item.TotalComprasSoloVenta ?? DBNull.Value);
                            }
                            cmdCompras.CommandText = sbCompras.ToString().TrimEnd(',') + ";";
                            cmdCompras.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATE corregido y seguro.
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 9";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtInicioProceso, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseKardex>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        // Si algo falla, la transacción debe hacer ROLLBACK automáticamente al salir del using(transaction),
                        // pero si se necesita un manejo explícito del error o rollback manual, se puede añadir aquí.
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Error clsKardexDatos-InsertarKardexManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }

        public static DataResponseKardex InsertarControlMensualManual(List<protoKardexControlMensualEntry> lskardexcontrolmensual, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseKardex objRespuesta = new DataResponseKardex();

            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. Conversión de Proto a Entidad de Base de Datos
                            //var lstEntidad = clsGeneral.ConvertirListaAEntidad<protoKardexControlMensualEntry, KardexControlMensual>(lskardexcontrolmensual);

                            var connection = context.Database.GetDbConnection() as MySqlConnection;
                            var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                            if (connection == null || mySqlTransaction == null)
                            {
                                throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                            }

                            if (lskardexcontrolmensual.Count > 0)
                            {
                                var cmd = connection.CreateCommand();
                                cmd.Transaction = mySqlTransaction;
                                cmd.CommandTimeout = 300; // Aumentado por el volumen de columnas

                                var sb = new StringBuilder(@"REPLACE INTO erp_ci_kardex_control_mensual_temp (
                            id_fran, clave, franquicia, id_producto, producto, mes, periodo, 
                            inventario_inicial_monto, inventario_inicial_piezas, inventario_inicial_costo_unitario, 
                            compras_similares_monto, compras_similares_piezas, compras_similares_costo_unitario, 
                            entrada_compra_proveedores_monto, entrada_compra_proveedores_piezas, 
                            entradas_traspaso_monto, entradas_traspaso_piezas, 
                            disponible_monto, disponible_piezas, disponible_costo_unitario, 
                            salidas_por_venta_monto, salidas_por_venta_piezas, 
                            devoluciones_por_venta_monto, devoluciones_por_venta_piezas, 
                            salidas_traspaso_monto, salidas_traspaso_piezas, 
                            entradas_ajustes_monto, entradas_ajustes_piezas, 
                            salidas_ajustes_monto, salidas_ajustes_piezas, 
                            salidas_ajustes_degustacion_monto, salidas_ajustes_degustacion_piezas, 
                            salidas_ajustes_siniestros_monto, salidas_ajustes_siniestros_piezas, 
                            devoluciones_similares_monto, devoluciones_similares_piezas, 
                            total_salida_monto, total_salida_piezas, 
                            total_entrada_monto, total_entrada_piezas, 
                            inventario_final_monto, inventario_final_piezas, inventario_final_costo_unitario
                        ) VALUES ");

                                var paramIndex = 0;
                                for (int i = 0; i < lskardexcontrolmensual.Count; i++)
                                {
                                    var item = lskardexcontrolmensual[i];
                                    // Generamos los 43 placeholders (@p0, @p1... @p42) para esta fila
                                    string placeholders = string.Join(", ", Enumerable.Range(paramIndex, 43).Select(n => $"@p{n}"));
                                    sb.Append($"({placeholders}),");

                                    // Mapeo de parámetros (Asegúrate que las propiedades de 'item' coincidan con tu Entidad)
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFran);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Clave);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Franquicia);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Producto);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Mes);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Periodo);

                                    // 1. Inventario Inicial
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0); // Monto (si no lo calculas)
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.InventarioInicialPiezas); // <-- CAMBIADO DE 0 A item.InvInicialPiezas
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0); // Costo Unitario

                                    // 2. Compras Similares
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ComprasSimilaresMonto);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ComprasSimilaresPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ComprasSimilaresCostoUnitario);

                                    // 3. Proveedores y Traspasos Entrada
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0); // entrada_compra_proveedores_monto
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradaCompraProveedoresPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0); // entradas_traspaso_monto
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradasTraspasoPiezas); // <-- EL DATO QUE TE FALTABA

                                    // 4. Disponible (Suma de Inicial + Entradas)
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);

                                    // 5. Ventas y Devoluciones
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasPorVentaPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.DevolucionesPorVentaPiezas);

                                    // 6. Traspasos Salida y Ajustes
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasTraspasoPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EntradasAjustesPiezas); // Entradas Ajuste
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustesPiezas);  // Salidas Ajuste

                                    // 7. Especiales
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustesDegustacionPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SalidasAjustesSiniestrosPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.DevolucionesSimilaresPiezas);

                                    // 8. Totales
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.TotalSalidaPiezas);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.TotalEntradaPiezas);

                                    // 9. Inventario Final
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.InventarioFinalPiezas); // <-- CAMBIADO DE 0 A item.InventarioFinalPiezas
                                    cmd.Parameters.AddWithValue($"@p{paramIndex++}", 0);
                                }

                                cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            // Actualizar tabla de control de envíos (Operación 9 asumiendo que es Control Mensual)
                            string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 9";
                            context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, DateTime.Now, objfranquicia.Idfran);

                            objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseKardex>("Control mensual guardado correctamente.");
                        }
                        catch (Exception ex)
                        {
                            objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Error en InsertarControlMensualManual: " + ex.Message);
                        }
                    }
                }

                return objRespuesta;
            }
            catch (Exception)
            {
                return objRespuesta;
            }
        }




    }
}
