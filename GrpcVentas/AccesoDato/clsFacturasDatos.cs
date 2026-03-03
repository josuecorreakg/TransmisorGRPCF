using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsFacturasDatos
    {

        public static DataResponseFacturas InsertarFacturasManual(List<protoFacturacionFactura> lsfacturacionfactura, List<protoFacturacionReferencium> lsfacturacionreferencia, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseFacturas objRespuesta = new DataResponseFacturas();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objfacturacionfactura = clsGeneral.ConvertirListaAEntidad<protoFacturacionFactura, FacturacionFactura>(lsfacturacionfactura);
                        var objfacturacionreferencia = clsGeneral.ConvertirListaAEntidad<protoFacturacionReferencium, FacturacionReferencium>(lsfacturacionreferencia);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- Parte 1: Insertar FacturacionFactura ----
                        if (objfacturacionfactura.Count > 0)
                        {
                            var commandFacturas = connection.CreateCommand();
                            commandFacturas.Transaction = mySqlTransaction;
                            commandFacturas.CommandTimeout = 180;

                            var sCommand = new StringBuilder("INSERT IGNORE INTO facturacion_factura (Idfran, Serie, Folio, Global, Estatus, FechaOperacion, FechaFacturacion, FechaCancelacion, Electronica, Reimpresiones, NotaCredito, Fundacion, Franquicia, Correo, ContadorItems) VALUES ");

                            var parameterIndex = 0;
                            for (int i = 0; i < objfacturacionfactura.Count; i++)
                            {
                                var factura = objfacturacionfactura[i];
                                sCommand.Append($"(@p{parameterIndex}, @p{parameterIndex + 1}, @p{parameterIndex + 2}, @p{parameterIndex + 3}, @p{parameterIndex + 4}, @p{parameterIndex + 5}, @p{parameterIndex + 6}, @p{parameterIndex + 7}, @p{parameterIndex + 8}, @p{parameterIndex + 9}, @p{parameterIndex + 10}, @p{parameterIndex + 11}, @p{parameterIndex + 12}, @p{parameterIndex + 13}, @p{parameterIndex + 14}),");

                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Idfran);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Serie);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Folio);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Global);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Estatus);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.FechaOperacion);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.FechaFacturacion);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.FechaCancelacion);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Electronica);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Reimpresiones);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.NotaCredito); // CORREGIDO
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Fundacion);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Franquicia);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.Correo);
                                commandFacturas.Parameters.AddWithValue($"@p{parameterIndex++}", factura.ContadorItems);
                            }
                            commandFacturas.CommandText = sCommand.ToString().TrimEnd(',') + ";";
                            commandFacturas.ExecuteNonQuery();
                        }

                        // ---- Parte 2: Insertar FacturacionReferencium ----
                        if (objfacturacionreferencia.Count > 0)
                        {
                            var commandReferencias = connection.CreateCommand();
                            commandReferencias.Transaction = mySqlTransaction;
                            commandReferencias.CommandTimeout = 180;

                            var sCommand = new StringBuilder("INSERT IGNORE INTO facturacion_referencia (Idfran, Serie, Folio, Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, ReferenciaTicket, SubTotalNeto, Descuento, TotalNeto, Iva, GranTotal) VALUES ");

                            var parameterIndex = 0;
                            for (int i = 0; i < objfacturacionreferencia.Count; i++)
                            {
                                var referencia = objfacturacionreferencia[i];
                                sCommand.Append($"(@p{parameterIndex}, @p{parameterIndex + 1}, @p{parameterIndex + 2}, @p{parameterIndex + 3}, @p{parameterIndex + 4}, @p{parameterIndex + 5}, @p{parameterIndex + 6}, @p{parameterIndex + 7}, @p{parameterIndex + 8}, @p{parameterIndex + 9}, @p{parameterIndex + 10}, @p{parameterIndex + 11}),");

                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.Idfran);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.Serie);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.Folio);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.IdVenta);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.IdVentaLocal);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.IdVentaConsecutivo);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.ReferenciaTicket);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.SubTotalNeto);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.Descuento);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.TotalNeto);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.Iva);
                                commandReferencias.Parameters.AddWithValue($"@p{parameterIndex++}", referencia.GranTotal);
                            }
                            commandReferencias.CommandText = sCommand.ToString().TrimEnd(',') + ";";
                            commandReferencias.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 2";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);


                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseFacturas>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseFacturas>("Error clsFacturasDatos-InsertarFacturasManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
