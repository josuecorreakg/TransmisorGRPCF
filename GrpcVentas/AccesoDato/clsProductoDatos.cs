using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsProductoDatos
    {
        public static DataResponseProducto InsertarProductoManual(List<protoProductoFranquicia> lsProductoFranquicia, List<protoProductoSuplementoFranquicia> lsProductoSuplementoFranquicia, List<protoProductoPorListaPrecio> lsProductoPorListaPrecio, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseProducto objRespuesta = new DataResponseProducto();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objproductofranquicia = clsGeneral.ConvertirListaAEntidad<protoProductoFranquicia, Productofranquicium>(lsProductoFranquicia);
                        var objproductosuplementofranquicia = clsGeneral.ConvertirListaAEntidad<protoProductoSuplementoFranquicia, ProductoSuplementoFranquicium>(lsProductoSuplementoFranquicia);
                        var objproductoporlistaprecio = clsGeneral.ConvertirListaAEntidad<protoProductoPorListaPrecio, ProductoPorlistaprecio>(lsProductoPorListaPrecio);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. Inserción Masiva para productofranquicia ----
                        if (objproductofranquicia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO productofranquicia (idfran, Id_Producto, Id_Nivel1, Id_Nivel2, Id_Nivel3, Id_Articulo, Id_Presentacion, Nombre, MarcaEconomica, PrecioCompra, Precio, UltimoCosto, IVA, Inventario, InventarioDiario, Combo, OTC, Venta, Servicio, Premio, EstructuraNegocio, AplicaCaducidad, AplicaDescuento, ProductoBasico, AsignaPuntos, PrecioPuntos, ProductoGondola, EstatusRegistro, Controlado, Descripcion_Corta, FueradeCatalogo, NoPonderado, CantidadPresentacion, FechaInclusion, id_productosat, ieps, Ultima_Actualizacion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objproductofranquicia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}, @p{paramIndex + 28}, @p{paramIndex + 29}, @p{paramIndex + 30}, @p{paramIndex + 31}, @p{paramIndex + 32}, @p{paramIndex + 33}, @p{paramIndex + 34}, @p{paramIndex + 35}, @p{paramIndex + 36}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdNivel1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdNivel2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdNivel3);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdArticulo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdPresentacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.MarcaEconomica);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.PrecioCompra);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Precio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UltimoCosto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Iva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Inventario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.InventarioDiario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Combo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Otc);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Venta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Servicio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Premio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstructuraNegocio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.AplicaCaducidad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.AplicaDescuento);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ProductoBasico);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.AsignaPuntos);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.PrecioPuntos);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.ProductoGondola);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Controlado ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.DescripcionCorta ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FueradeCatalogo ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NoPonderado ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.CantidadPresentacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.FechaInclusion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdProductosat ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Ieps ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UltimaActualizacion);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. Inserción Masiva para producto_suplemento_franquicia ----
                        if (objproductosuplementofranquicia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO producto_suplemento_franquicia (id_Fran, id_Producto, Ultima_Actualizacion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objproductosuplementofranquicia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UltimaActualizacion);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. Inserción Masiva para producto_porlistaprecio ----
                        if (objproductoporlistaprecio.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("REPLACE INTO producto_porlistaprecio (idfran, id_listaprecio, id_producto, precio, estatusregistro, ultimaactualizacion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objproductoporlistaprecio)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdListaprecio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Precio ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Estatusregistro ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Ultimaactualizacion ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATES corregidos y seguros
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 6";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseProducto>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseProducto>("Error clsProductoDatos-InsertarProductoManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
