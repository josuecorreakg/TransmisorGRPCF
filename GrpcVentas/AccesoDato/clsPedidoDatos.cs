using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsPedidoDatos
    {

        public static DataResponsePedido InsertarPedidoManual(List<protoPedido> lspedido, List<protoPedidoDetalle> lspedidodetalle, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponsePedido objRespuesta = new DataResponsePedido();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objpedido = clsGeneral.ConvertirListaAEntidad<protoPedido, Pedido>(lspedido);
                        var objpedidodetalle = clsGeneral.ConvertirListaAEntidad<protoPedidoDetalle, PedidoDetalle>(lspedidodetalle);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. Inserción Masiva para pedido ----
                        if (objpedido.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO pedido (idfran, Id_Pedido, FechaOperacion, Id_Usuario, Semanal, FechaHora_Captura, Estatus, Observacion, Dias, Adicionales, FechaPedido, IncluirMenudeo, Definitivo, folioconfirmacion, foliopedido, sincref, estimado, pedidoemergente, id_almacen_surtido, id_financiamiento) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objpedido)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdPedido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Semanal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Dias);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Adicionales);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaPedido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IncluirMenudeo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Definitivo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Folioconfirmacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Foliopedido ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Sincref ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Estimado ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Pedidoemergente ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdAlmacenSurtido ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdFinanciamiento ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. Inserción Masiva para pedido_detalle ----
                        if (objpedidodetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO pedido_detalle (idfran, Id_Pedido, Id_Producto, UltimaVenta, Sugerencia, Pedido, ExistenciaTeorica, CostoUnitario) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objpedidodetalle)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdPedido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UltimaVenta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Sugerencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Pedido);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ExistenciaTeorica);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CostoUnitario);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATES corregidos y seguros
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 5";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponsePedido>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponsePedido>("Error clsPedidoDatos-InsertarPedidoManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
