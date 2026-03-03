using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo;
using MySqlConnector;
using System.Data;
using System.Text;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace GrpcVentas.AccesoDato
{
    public class clsVentasDatos
    {
        public static DataResponse InsertarVentasManual(List<protoVentaDescuento> lsventaDescuento, List<protoVentaProducto> lsventaProdu, List<protoVentaPago> lsventaPagos, List<protoVentum> lsventa, List<protoVentafrandium> lsventafrandia, List<protoVentaProductoDesglose> lsventaproductoDesgloce, List<protoVentaRecetaControlado> lsventaRecetaControlado, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso, int idfran)
        {
            DataResponse objRespuesta = new DataResponse();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objVentaDescuento = clsGeneral.ConvertirListaAEntidad<protoVentaDescuento, VentaDescuento>(lsventaDescuento);
                        var objProducto = clsGeneral.ConvertirListaAEntidad<protoVentaProducto, VentaProducto>(lsventaProdu);
                        var objPagos = clsGeneral.ConvertirListaAEntidad<protoVentaPago, VentaPago>(lsventaPagos);
                        var objVenta = clsGeneral.ConvertirListaAEntidad<protoVentum, Ventum>(lsventa);
                        var objVentaFrandia = clsGeneral.ConvertirListaAEntidad<protoVentafrandium, Ventafrandium>(lsventafrandia);
                        var objVentaProductoDesgloce = clsGeneral.ConvertirListaAEntidad<protoVentaProductoDesglose, VentaProductoDesglose>(lsventaproductoDesgloce);
                        var objVentaRecetaControlado = clsGeneral.ConvertirListaAEntidad<protoVentaRecetaControlado, VentaRecetaControlado>(lsventaRecetaControlado);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. venta_descuento ----
                        if (objVentaDescuento.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta_descuento (idfran, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_Descuento, Descuento, ProductoBasico, Tarjeta, Cliente_Nombre, Doctor_Nombre, Doctor_Cedula, Receta, Referencia, DescuentoPorciento) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaDescuento)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaConsecutivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdDescuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Descuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ProductoBasico);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Tarjeta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ClienteNombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.DoctorNombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.DoctorCedula);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Receta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Referencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.DescuentoPorciento ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. venta_producto ----
                        if (objProducto.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta_producto (idfran, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_Producto, Cantidad, Precio, IVA, Descuento, DescuentoPorciento, Puntos, IVA_Porciento, IVA_Importe, Posicion, Premio) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objProducto)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaConsecutivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Cantidad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Precio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Iva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Descuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.DescuentoPorciento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Puntos);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IvaPorciento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IvaImporte);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Posicion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Premio ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. venta_pago ----
                        if (objPagos.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta_pago (idfran, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_FormaPago, Importe, TipoCambio) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objPagos)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaConsecutivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFormaPago);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Importe);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.TipoCambio);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 4. venta ----
                        if (objVenta.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta (idfran, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_Movimiento, Id_Venta_Registradora, Id_Registradora_Venta, Id_Registradora_Cobro, Id_Usuario_Venta, Id_Usuario_Cobro, Id_Usuario_Cancelacion, FechaHoraVenta, FechaHoraCobro, FechaHoraCancelacion, FechaOperacion, TipoVenta, TipoOperacion, Id_Venta_Referencia, Receta, AntesTotal, Estatus, Historico, Sincroniza, Id_Cliente, PuntosIniciales, PuntosFinales, PuntosAcumulados, Restriccion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVenta)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaConsecutivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdMovimiento ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaRegistradora);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRegistradoraVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRegistradoraCobro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuarioVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuarioCobro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuarioCancelacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCobro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCancelacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.TipoVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.TipoOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaReferencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Receta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.AntesTotal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Historico ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Sincroniza ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdCliente ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PuntosIniciales ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PuntosFinales ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PuntosAcumulados ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Restriccion ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 5. ventafrandia ----
                        if (objVentaFrandia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO ventafrandia (idfran, mes, ano, dia, venta, naturistas, inventario, inventarioCompra, FechaCalculo) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaFrandia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Mes);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ano);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Dia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Venta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Naturistas ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Inventario ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.InventarioCompra ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FechaCalculo ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 6. venta_producto_desglose ----
                        if (objVentaProductoDesgloce.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta_producto_desglose (idfran, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_Producto, Cantidad, Precio, IVA, Importe) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaProductoDesgloce)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVentaConsecutivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Cantidad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Precio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Iva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Importe);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 7. venta_receta_controlados ----
                        if (objVentaRecetaControlado.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO venta_receta_controlados (idfran, id_receta, id_venta, retenerreceta, fechacaptura, id_medico, nombre, cedula, domicilio, tipo) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaRecetaControlado)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdReceta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Retenerreceta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Fechacaptura ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdMedico ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Nombre ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Cedula ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Domicilio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Tipo ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATES corregidos y seguros
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 1";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponse>("La información se ha guardado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error clsVentas-InsertarVentasManual " + ex.ToString() + ":" + idfran);
                    }
                }
            }
            return objRespuesta;
        }

        public static DataResponse InsertarHistoricosManual(List<protoVentafran> lsVentaFran, List<protoVentafrandium> lsVentaFranDia, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponse objRespuesta = new DataResponse();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objVentaFran = clsGeneral.ConvertirListaAEntidad<protoVentafran, Ventafran>(lsVentaFran);
                        var objVentaFranDia = clsGeneral.ConvertirListaAEntidad<protoVentafrandium, Ventafrandium>(lsVentaFranDia);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. Inserción Masiva para ventafran ----
                        if (objVentaFran.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO ventafran (idfran, mes, ano, venta, ventaPE, ventaPN, cepip, vitaminas, similares, naturistas, gravados, clientes, descuentos, iva, prodPremio, iva_suple, iva_suple_t, conmedm, conmedv, conmedn, conmedx, part_vta, idcat) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaFran)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Mes);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ano);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Venta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentaPe ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentaPn ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Cepip ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Vitaminas ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Similares ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Naturistas ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Gravados ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Clientes ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Descuentos ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Iva ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.ProdPremio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IvaSuple ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IvaSupleT ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Conmedm ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Conmedv ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Conmedn ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Conmedx ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PartVta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Idcat ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. Inserción Masiva para ventafrandia ----
                        if (objVentaFranDia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO ventafrandia (idfran, mes, ano, dia, venta, naturistas, inventario, inventarioCompra, FechaCalculo) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objVentaFranDia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Mes);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ano);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Dia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Venta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Naturistas ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Inventario ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.InventarioCompra ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FechaCalculo ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATES corregidos y seguros
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 7";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtInicioProceso, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponse>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error clsHistoricosDatos-InsertarHistoricosManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }

        public static DataResponse InsertarCostoOp(List<protoCostoOp> lsCostoOp, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponse objRespuesta = new DataResponse();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objCostoOp = clsGeneral.ConvertirListaAEntidad<protoCostoOp, CostoOp>(lsCostoOp);

                        // Asigna FechaTransmision a cada elemento con DateTime.Now
                        foreach (var costoOpItem in objCostoOp)
                        {
                            costoOpItem.FechaTransmision = DateTime.Now;
                        }

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- Inserción Masiva para erp_ci_costo_oportunidad ----
                        if (objCostoOp.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO erp_ci_costo_oportunidad (Id_fran, Clave, Id_Producto, Producto, Fecha_operacion, FechaTransmision, PrecioVenta, InventarioActual, VentasDia1, PromDia1, VentasDia2, PromDia2, VentasDia3, PromDia3, VentasDia4, PromDia4, VentasDia5, PromDia5, VentasDia6, PromDia6, VentasDia7, PromDia7) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objCostoOp)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Clave ?? DBNull.Value);
                                // NOTA: Conversión de int a string para coincidir con la base de datos
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto.ToString());
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Producto ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaTransmision);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PrecioVenta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.InventarioActual ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia2 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia2 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia3 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia3 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia4 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia4 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia5 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia5 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia6 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia6 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.VentasDia7 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.PromDia7 ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponse>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error clsCostoOpDatos-InsertarCostoOp " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
