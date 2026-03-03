using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsInventariosDatos
    {


        public static DataResponseInventario InsertarInventarioManual(List<protoInventarioFisicoCompleto> lsinventariofisicocompleto, List<protoInventarioOtro> lsinventariootros, List<protoInventarioOtrosDetalle> lsinventariootrosdetalle, List<protoInventarioSurtido> lsinventariosurtido, List<protoInventarioSurtidoDetalle> lsinventariosurtidodetalle, List<protoInventarioSurtidoFranquicium> lsinventariosurtidofranquicia, List<protoInventarioTraspaso> lsinventariotraspaso, List<protoInventarioTraspasoDetalle> lsinventariotraspasodetalle, List<protoInventario> lsinventario, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseInventario objRespuesta = new DataResponseInventario();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Conversión de todas las listas
                        var objInventarioFisicoCompleto = clsGeneral.ConvertirListaAEntidad<protoInventarioFisicoCompleto, InventarioFisicoCompleto>(lsinventariofisicocompleto);
                        var objInventarioOtros = clsGeneral.ConvertirListaAEntidad<protoInventarioOtro, InventarioOtro>(lsinventariootros);
                        var objInventarioOtrosDetalle = clsGeneral.ConvertirListaAEntidad<protoInventarioOtrosDetalle, InventarioOtrosDetalle>(lsinventariootrosdetalle);
                        var objInventarioSurtido = clsGeneral.ConvertirListaAEntidad<protoInventarioSurtido, InventarioSurtido>(lsinventariosurtido);
                        var objInventarioSurtidoDetalle = clsGeneral.ConvertirListaAEntidad<protoInventarioSurtidoDetalle, InventarioSurtidoDetalle>(lsinventariosurtidodetalle);
                        var objInventarioSurtidofranquicia = clsGeneral.ConvertirListaAEntidad<protoInventarioSurtidoFranquicium, InventarioSurtidoFranquicium>(lsinventariosurtidofranquicia);
                        var objInventarioTraspaso = clsGeneral.ConvertirListaAEntidad<protoInventarioTraspaso, InventarioTraspaso>(lsinventariotraspaso);
                        var objInventarioTraspasoDetalle = clsGeneral.ConvertirListaAEntidad<protoInventarioTraspasoDetalle, InventarioTraspasoDetalle>(lsinventariotraspasodetalle);
                        var objInventario = clsGeneral.ConvertirListaAEntidad<protoInventario, Inventario>(lsinventario);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. inventario_fisico_completo ----
                        if (objInventarioFisicoCompleto.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_fisico_completo (idfran, FechaOperacion, Id_Producto, Contado, ExistenciaInicial, Entradas, Salidas, ExistenciaFinal, Costo) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioFisicoCompleto.Count; i++)
                            {
                                var item = objInventarioFisicoCompleto[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Contado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ExistenciaInicial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Entradas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Salidas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ExistenciaFinal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Costo);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. inventario_otros ----
                        if (objInventarioOtros.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_otros (idfran, Id_Registro, Id_Movimiento, Documento, Referencia, Signo, FechaOperacion, FechaHora_Captura, Id_Usuario, Observacion, SincRef, id_tipo) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioOtros.Count; i++)
                            {
                                var item = objInventarioOtros[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRegistro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMovimiento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Documento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Referencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Signo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SincRef);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTipo);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. inventario_otros_detalle ----
                        if (objInventarioOtrosDetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_otros_detalle (idfran, Id_Registro, Id_Producto, Cantidad) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioOtrosDetalle.Count; i++)
                            {
                                var item = objInventarioOtrosDetalle[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRegistro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Cantidad);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 4. inventario_surtido ----
                        if (objInventarioSurtido.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_surtido (idfran, Id_Surtido, Id_Surtido_Local, Id_Movimiento, Documento, Referencia, FechaOperacion, FechaHora_Captura, FechaOperacion_Descarga, Fecha_Facturacion, Id_Usuario, Factura, Factura_Fiscal, SurtidoElectronico, Id_Proveedor, Id_FarmaciaSurtido, Estatus, Observacion, Subtotal, Descuento, Impuesto, Total, Respaldo, Conteo, SincRef, Signo, fechavencimiento, factura_fiscal_ref) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioSurtido.Count; i++)
                            {
                                var item = objInventarioSurtido[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtidoLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMovimiento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Documento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Referencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacionDescarga);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaFacturacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Factura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FacturaFiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SurtidoElectronico);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProveedor);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmaciaSurtido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Subtotal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Descuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Impuesto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Total);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Respaldo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Conteo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SincRef);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Signo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Fechavencimiento ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FacturaFiscalRef ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 5. inventario_surtido_detalle ----
                        if (objInventarioSurtidoDetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO inventario_surtido_detalle (idfran, Id_Surtido, Id_Surtido_Local, Id_Producto, Remision, Conteo1, Conteo2, MalEstado, CostoUnitario, Descuento, Impuesto, Total, subtotal, ivaporciento, descuentoporciento) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioSurtidoDetalle.Count; i++)
                            {
                                var item = objInventarioSurtidoDetalle[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtidoLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Remision);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Conteo1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Conteo2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.MalEstado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CostoUnitario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Descuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Impuesto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Total);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Subtotal ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Ivaporciento ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Descuentoporciento ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 6. inventario_surtido_franquicia ----
                        if (objInventarioSurtidofranquicia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO inventario_surtido_franquicia (idfran, Id_Surtido, Id_Surtido_Local, Id_Movimiento, Documento, Referencia, FechaOperacion, FechaHora_Captura, FechaOperacion_Descarga, Fecha_Facturacion, Id_Usuario, Factura, Factura_Fiscal, SurtidoElectronico, Id_Proveedor, Id_FarmaciaSurtido, Estatus, Observacion, Subtotal, Descuento, Impuesto, Total, Respaldo, Conteo, SincRef, Signo, fechavencimiento, factura_fiscal_ref, ultimaActualizacion) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioSurtidofranquicia.Count; i++)
                            {
                                var item = objInventarioSurtidofranquicia[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}, @p{paramIndex + 28}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdSurtidoLocal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMovimiento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Documento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Referencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacionDescarga);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaFacturacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Factura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FacturaFiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SurtidoElectronico);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProveedor);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmaciaSurtido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Subtotal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Descuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Impuesto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Total);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Respaldo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Conteo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SincRef);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Signo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Fechavencimiento ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FacturaFiscalRef ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.UltimaActualizacion ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 7. inventario_traspaso ----
                        if (objInventarioTraspaso.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_traspaso (idfran, Id_Farmacia_Entrega, Id_Traspaso, idfran_pedido, Id_Farmacia_Pedido, Id_Concepto, Id_Movimiento, Documento, Referencia, FechaOperacion, FechaHora_Captura, FechaHora_Autorizacion, Id_Usuario_Captura, Id_Usuario_Autoriza, Estatus, SincRef, total) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioTraspaso.Count; i++)
                            {
                                var item = objInventarioTraspaso[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmaciaEntrega);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTraspaso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdfranPedido ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmaciaPedido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConcepto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMovimiento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Documento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Referencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraAutorizacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuarioCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuarioAutoriza);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.SincRef);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Total ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 8. inventario_traspaso_detalle ----
                        if (objInventarioTraspasoDetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_traspaso_detalle (idfran, Id_Farmacia_Entrega, Id_Traspaso, Id_Producto, Solicitud, Autorizado, precio, importe) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioTraspasoDetalle.Count; i++)
                            {
                                var item = objInventarioTraspasoDetalle[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmaciaEntrega);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTraspaso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Solicitud);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Autorizado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Precio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Importe ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 9. inventario ----
                        if (objInventario.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO inventario (idfran, id_Producto, Existencia, NoDisponible, Fecha_Modificacion, UltimaActualizacion) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventario.Count; i++)
                            {
                                var item = objInventario[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Existencia ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NoDisponible ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FechaModificacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.UltimaActualizacion ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 3";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseInventario>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Error clsInventarioDatos-InsertarInventarioManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


        public static DataResponseInventario InsertarInventarioTiempoRealManual(List<protoInventarioFisicoCompleto> lsinventariofisicocompleto, List<protoInventario> lsinventario, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseInventario objRespuesta = new DataResponseInventario();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objInventarioFisicoCompleto = clsGeneral.ConvertirListaAEntidad<protoInventarioFisicoCompleto, InventarioFisicoCompleto>(lsinventariofisicocompleto);
                        var objInventario = clsGeneral.ConvertirListaAEntidad<protoInventario, Inventario>(lsinventario);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. inventario_fisico_completo ----
                        if (objInventarioFisicoCompleto.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario_fisico_completo (idfran, FechaOperacion, Id_Producto, Contado, ExistenciaInicial, Entradas, Salidas, ExistenciaFinal, Costo) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventarioFisicoCompleto.Count; i++)
                            {
                                var item = objInventarioFisicoCompleto[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Contado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ExistenciaInicial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Entradas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Salidas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ExistenciaFinal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Costo);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. inventario ----
                        if (objInventario.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO inventario (idfran, id_Producto, Existencia, NoDisponible, Fecha_Modificacion, UltimaActualizacion) VALUES ");
                            var paramIndex = 0;
                            for (int i = 0; i < objInventario.Count; i++)
                            {
                                var item = objInventario[i];
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Existencia ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NoDisponible ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FechaModificacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.UltimaActualizacion ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 3";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseInventario>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Error clsInventarioDatos-InsertarInventarioTiempoRealManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
