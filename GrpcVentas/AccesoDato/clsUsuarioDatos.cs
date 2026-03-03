using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsUsuarioDatos
    {

        public static DataResponseUsuario InsertarUsuariosManual(List<protoSeguridadCategoria> lsSeguridadCategoria, List<protoSeguridadRol> lsSeguridadRol, List<protoSeguridadTarea> lsSeguridadTarea, List<protoUsuarioRol> lsUsuarioRol, List<protoUsuarioPorRol> lsUsuarioPorRol, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseUsuario objRespuesta = new DataResponseUsuario();

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objSeguridadCategoria = clsGeneral.ConvertirListaAEntidad<protoSeguridadCategoria, ErpSposSeguridadCategorium>(lsSeguridadCategoria);
                        var objSeguridadRol = clsGeneral.ConvertirListaAEntidad<protoSeguridadRol, ErpSposSeguridadRol>(lsSeguridadRol);
                        var objSeguridadTarea = clsGeneral.ConvertirListaAEntidad<protoSeguridadTarea, ErpSposSeguridadTarea>(lsSeguridadTarea);
                        var objUsuarioRol = clsGeneral.ConvertirListaAEntidad<protoUsuarioRol, ErpSposUsuarioRol>(lsUsuarioRol);
                        var objUsuarioPorRol = clsGeneral.ConvertirListaAEntidad<protoUsuarioPorRol, ErpSposUsuarioporrol>(lsUsuarioPorRol);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. erp_spos_seguridad_categoria ----
                        if (objSeguridadCategoria.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO erp_spos_seguridad_categoria (Idfran, Id_categoria, Nombre, Orden, Reporte) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objSeguridadCategoria)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoria);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Nombre ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Orden ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Reporte ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. erp_spos_seguridad_rol ----
                        if (objSeguridadRol.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO erp_spos_seguridad_rol (Idfran, Id_Rol, Id_Categoria, NivelSeguridad) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objSeguridadRol)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRol);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoria);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NivelSeguridad ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. erp_spos_seguridad_tarea ----
                        if (objSeguridadTarea.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO erp_spos_seguridad_tarea (Idfran, Id_tarea, Id_Categoria, Nombre, Orden, Validar, Permitir, Consultar, Crear, Modificar, Eliminar, Imprimir, Auditoria) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objSeguridadTarea)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTarea);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoria);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Nombre ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Orden ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Validar ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Permitir ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Consultar ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Crear ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Modificar ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Eliminar ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Imprimir ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Auditoria ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 4. erp_spos_usuario_rol ----
                        if (objUsuarioRol.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO erp_spos_usuario_rol (Idfran, Id_Rol, Nombre, EstatusRegistro) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objUsuarioRol)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRol);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.EstatusRegistro ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 5. erp_spos_usuarioporrol ----
                        if (objUsuarioPorRol.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO erp_spos_usuarioporrol (Idfran, Id_Usuario, Id_Rol) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objUsuarioPorRol)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdRol);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATE corregido y seguro
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 10";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtInicioProceso, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseUsuario>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseUsuario>("Error clsUsuarioDatos-InsertarUsuariosManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }


    }
}
