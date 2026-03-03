using GrpcVentas.Modelo.DBCorporativo;
//using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;

namespace GrpcVentas.AccesoDato
{
    public class clsOperacionDatos
    {

        public static DataResponseOperacion InsertarOperacionManual(List<protoConsultaRespuesta> lsConsultaRespuesta, List<protoConfiguracionFarmaciaOperacion> lsconfiguracionfarmacia, List<protoOperacion> lsoperacion, List<protoUsuario> lsusuario, List<protoCategoriaComercial> lscategoriacomercial, List<protoCategoriaComercialProducto> lscategoriacomercialproducto, List<protoConsultaConsultorioTurnoDetalle> lsconsultaconsultorioturnodetalle, List<protoOperacionGlobal> lsglobal, List<protoTomaTemperatura> lsTemperatura, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseOperacion objRespuesta = new DataResponseOperacion();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objConsultaRespuesta = clsGeneral.ConvertirListaAEntidad<protoConsultaRespuesta, ConsultaRespuestum>(lsConsultaRespuesta);
                        var objConfiguracionFarmacia = clsGeneral.ConvertirListaAEntidad<protoConfiguracionFarmaciaOperacion, ConfiguracionFarmacium>(lsconfiguracionfarmacia);
                        var objOperacion = clsGeneral.ConvertirListaAEntidad<protoOperacion, Operacion>(lsoperacion);
                        var objUsuario = clsGeneral.ConvertirListaAEntidad<protoUsuario, Usuario>(lsusuario);
                        var objCategoriaComercial = clsGeneral.ConvertirListaAEntidad<protoCategoriaComercial, Categoriacomercial>(lscategoriacomercial);
                        var objCategoriaComercialProducto = clsGeneral.ConvertirListaAEntidad<protoCategoriaComercialProducto, Categoriacomercialproducto>(lscategoriacomercialproducto);
                        var objConsultaConsultorioTurnoDetalle = clsGeneral.ConvertirListaAEntidad<protoConsultaConsultorioTurnoDetalle, ConsultaConsultorioturnodetalle>(lsconsultaconsultorioturnodetalle);
                        var objOperacionGlobal = clsGeneral.ConvertirListaAEntidad<protoOperacionGlobal, Operacionglobal>(lsglobal);
                        var objTemperatura = clsGeneral.ConvertirListaAEntidad<protoTomaTemperatura, TomaTemperatura>(lsTemperatura);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. consulta_respuesta ----
                        if (objConsultaRespuesta.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO consulta_respuesta (idfran, Id_ConsultorioTurno, Id_ConsultaPregunta, Respuesta) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConsultaRespuesta)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorioTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultaPregunta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Respuesta);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. configuracion_farmacia ----
                        if (objConfiguracionFarmacia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO configuracion_farmacia (idfran, fran, id_configuracion, id_farmacia, id_division, nombre, id_pais, pais, estado, municipio, ciudad, codigopostal, colonia, calle, noexterior, nointerior, domicilio, razonsocial, domiciliofiscal1, domiciliofiscal2, rfc1, rfc2, rfc3, fechaapertura, iva, transaccionenlinea, horacierre, facturacionelectronica, ruta_rdis, id_negocio_franquicia, requerirasistencia, webservice_sincroniza, tienegondolas, usuariocontivity, passwordcontivity, ipcontivity, modificado, modificadoiva, id_usuario, fecha_captura, id_almacen, webservice_ventas, regimenfiscal, afiliacionbancaria, leyendafiscal, surtidodirecto, correoavisoprivacidad, id_categoriaventa) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConfiguracionFarmacia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}, @p{paramIndex + 28}, @p{paramIndex + 29}, @p{paramIndex + 30}, @p{paramIndex + 31}, @p{paramIndex + 32}, @p{paramIndex + 33}, @p{paramIndex + 34}, @p{paramIndex + 35}, @p{paramIndex + 36}, @p{paramIndex + 37}, @p{paramIndex + 38}, @p{paramIndex + 39}, @p{paramIndex + 40}, @p{paramIndex + 41}, @p{paramIndex + 42}, @p{paramIndex + 43}, @p{paramIndex + 44}, @p{paramIndex + 45}, @p{paramIndex + 46}, @p{paramIndex + 47}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Fran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConfiguracion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmacia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdDivision);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdPais);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Pais);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Municipio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ciudad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Codigopostal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Colonia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Calle);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Noexterior);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nointerior);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domicilio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Razonsocial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domiciliofiscal1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domiciliofiscal2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc3);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Fechaapertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Iva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Transaccionenlinea);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Horacierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Facturacionelectronica);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.RutaRdis);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdNegocioFranquicia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Requerirasistencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.WebserviceSincroniza);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Tienegondolas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Usuariocontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Passwordcontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ipcontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Modificado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Modificadoiva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdAlmacen);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.WebserviceVentas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Regimenfiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Afiliacionbancaria);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Leyendafiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Surtidodirecto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Correoavisoprivacidad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaventa);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. operacion ----
                        if (objOperacion.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO operacion (idfran, FechaOperacion, FechaHora_Apertura, Id_Usuario_Apertura, FechaHora_Cierre, Id_Usuario_Cierre, Id_Usuario_Vendedor, Id_Usuario_Cajero, Estatus, GranTotalApertura, GranTotalCierre, EstatusPV, EstatusCierre, EnvioCierre, FechaHora_Envio) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objOperacion)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraApertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioApertura ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioCierre ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioVendedor ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioCajero ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.GranTotalApertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.GranTotalCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusPv);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EnvioCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraEnvio);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 4. usuario ----
                        if (objUsuario.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO usuario (idfran, Id_Usuario, Nombre, Id_Idioma, LlaveAcceso, FechaUltimoCambio, CambiarLlaveAcceso, Temporal, EstatusRegistro) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objUsuario)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdIdioma);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.LlaveAcceso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaUltimoCambio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CambiarLlaveAcceso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Temporal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 5. categoriacomercial ----
                        if (objCategoriaComercial.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO categoriacomercial (Id_CategoriaComercial, Nombre, EstatusRegistro, grupo, orden) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objCategoriaComercial)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaComercial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Grupo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Orden);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 6. categoriacomercialproducto ----
                        if (objCategoriaComercialProducto.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO categoriacomercialproducto (Id_CategoriaComercial, Id_Producto, EstatusRegistro) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objCategoriaComercialProducto)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaComercial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 7. consulta_consultorioturnodetalle ----
                        if (objConsultaConsultorioTurnoDetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO consulta_consultorioturnodetalle (idfran, Id_ConsultorioTurno, Id_Consultorio, Id_Turno, Id_MotivoSinConsultas, CorreccionCaptura, Observaciones, FechaCaptura, UsuarioCaptura, FechaModificacion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConsultaConsultorioTurnoDetalle)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorioTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMotivoSinConsultas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CorreccionCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observaciones);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UsuarioCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaModificacion);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 8. operacionglobal ----
                        if (objOperacionGlobal.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO operacionglobal (idfran, versiondb) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objOperacionGlobal)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Versiondb ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 9. toma_temperatura ----
                        if (objTemperatura.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO toma_temperatura (idfran, id_toma, temperatura, humedad, fechaoperacion, hora_toma, usuario_toma, nombre_usuario) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objTemperatura)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdToma);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Temperatura ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Humedad ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Fechaoperacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.HoraToma ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.UsuarioToma ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NombreUsuario ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 4";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtInicioProceso, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseOperacion>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error clsOperacionDatos-InsertarOperacionManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }

        public static DataResponseOperacion InsertarTemperaturaManual(List<protoTomaTemperatura> lsTomaTemperatura, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseOperacion objRespuesta = new DataResponseOperacion();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objTomaTemperatura = clsGeneral.ConvertirListaAEntidad<protoTomaTemperatura, TomaTemperatura>(lsTomaTemperatura);

                        DateTime dtfechaInicioinsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- Inserción Masiva para toma_temperatura ----
                        if (objTomaTemperatura.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO toma_temperatura (idfran, id_toma, temperatura, humedad, fechaoperacion, hora_toma, usuario_toma, nombre_usuario) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objTomaTemperatura)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdToma);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Temperatura ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Humedad ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Fechaoperacion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.HoraToma ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.UsuarioToma ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.NombreUsuario ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        // UPDATES corregidos y seguros
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 8";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtInicioProceso, objfranquicia.Idfran);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseOperacion>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error clsTomaTemperaturaDatos-InsertarTemperaturaManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }

        public static DataResponseOperacion InsertarOperacionAuditoriaManual(List<protoConsultaRespuesta> lsConsultaRespuesta, List<protoConfiguracionFarmaciaOperacion> lsconfiguracionfarmacia, List<protoOperacion> lsoperacion, List<protoUsuario> lsusuario, List<protoCategoriaComercial> lscategoriacomercial, List<protoCategoriaComercialProducto> lscategoriacomercialproducto, List<protoConsultaConsultorioTurnoDetalle> lsconsultaconsultorioturnodetalle, List<protoOperacionGlobal> lsglobal, DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso)
        {
            DataResponseOperacion objRespuesta = new DataResponseOperacion();
            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var objConsultaRespuesta = clsGeneral.ConvertirListaAEntidad<protoConsultaRespuesta, ConsultaRespuestum>(lsConsultaRespuesta);
                        var objConfiguracionFarmacia = clsGeneral.ConvertirListaAEntidad<protoConfiguracionFarmaciaOperacion, ConfiguracionFarmacium>(lsconfiguracionfarmacia);
                        var objOperacion = clsGeneral.ConvertirListaAEntidad<protoOperacion, Operacion>(lsoperacion);
                        var objUsuario = clsGeneral.ConvertirListaAEntidad<protoUsuario, Usuario>(lsusuario);
                        var objCategoriaComercial = clsGeneral.ConvertirListaAEntidad<protoCategoriaComercial, Categoriacomercial>(lscategoriacomercial);
                        var objCategoriaComercialProducto = clsGeneral.ConvertirListaAEntidad<protoCategoriaComercialProducto, Categoriacomercialproducto>(lscategoriacomercialproducto);
                        var objConsultaConsultorioTurnoDetalle = clsGeneral.ConvertirListaAEntidad<protoConsultaConsultorioTurnoDetalle, ConsultaConsultorioturnodetalle>(lsconsultaconsultorioturnodetalle);
                        var objOperacionGlobal = clsGeneral.ConvertirListaAEntidad<protoOperacionGlobal, Operacionglobal>(lsglobal);

                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        // ---- 1. consulta_respuesta ----
                        if (objConsultaRespuesta.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO consulta_respuesta (idfran, Id_ConsultorioTurno, Id_ConsultaPregunta, Respuesta) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConsultaRespuesta)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorioTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultaPregunta);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Respuesta);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 2. configuracion_farmacia ----
                        if (objConfiguracionFarmacia.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO configuracion_farmacia (idfran, fran, id_configuracion, id_farmacia, id_division, nombre, id_pais, pais, estado, municipio, ciudad, codigopostal, colonia, calle, noexterior, nointerior, domicilio, razonsocial, domiciliofiscal1, domiciliofiscal2, rfc1, rfc2, rfc3, fechaapertura, iva, transaccionenlinea, horacierre, facturacionelectronica, ruta_rdis, id_negocio_franquicia, requerirasistencia, webservice_sincroniza, tienegondolas, usuariocontivity, passwordcontivity, ipcontivity, modificado, modificadoiva, id_usuario, fecha_captura, id_almacen, webservice_ventas, regimenfiscal, afiliacionbancaria, leyendafiscal, surtidodirecto, correoavisoprivacidad, id_categoriaventa) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConfiguracionFarmacia)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}, @p{paramIndex + 15}, @p{paramIndex + 16}, @p{paramIndex + 17}, @p{paramIndex + 18}, @p{paramIndex + 19}, @p{paramIndex + 20}, @p{paramIndex + 21}, @p{paramIndex + 22}, @p{paramIndex + 23}, @p{paramIndex + 24}, @p{paramIndex + 25}, @p{paramIndex + 26}, @p{paramIndex + 27}, @p{paramIndex + 28}, @p{paramIndex + 29}, @p{paramIndex + 30}, @p{paramIndex + 31}, @p{paramIndex + 32}, @p{paramIndex + 33}, @p{paramIndex + 34}, @p{paramIndex + 35}, @p{paramIndex + 36}, @p{paramIndex + 37}, @p{paramIndex + 38}, @p{paramIndex + 39}, @p{paramIndex + 40}, @p{paramIndex + 41}, @p{paramIndex + 42}, @p{paramIndex + 43}, @p{paramIndex + 44}, @p{paramIndex + 45}, @p{paramIndex + 46}, @p{paramIndex + 47}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Fran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConfiguracion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdFarmacia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdDivision);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdPais);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Pais);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Municipio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ciudad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Codigopostal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Colonia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Calle);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Noexterior);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nointerior);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domicilio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Razonsocial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domiciliofiscal1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Domiciliofiscal2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc1);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc2);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Rfc3);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Fechaapertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Iva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Transaccionenlinea);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Horacierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Facturacionelectronica);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.RutaRdis);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdNegocioFranquicia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Requerirasistencia);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.WebserviceSincroniza);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Tienegondolas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Usuariocontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Passwordcontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Ipcontivity);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Modificado);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Modificadoiva);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdAlmacen);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.WebserviceVentas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Regimenfiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Afiliacionbancaria);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Leyendafiscal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Surtidodirecto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Correoavisoprivacidad);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaventa);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 3. operacion ----
                        if (objOperacion.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO operacion (idfran, FechaOperacion, FechaHora_Apertura, Id_Usuario_Apertura, FechaHora_Cierre, Id_Usuario_Cierre, Id_Usuario_Vendedor, Id_Usuario_Cajero, Estatus, GranTotalApertura, GranTotalCierre, EstatusPV, EstatusCierre, EnvioCierre, FechaHora_Envio) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objOperacion)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}, @p{paramIndex + 10}, @p{paramIndex + 11}, @p{paramIndex + 12}, @p{paramIndex + 13}, @p{paramIndex + 14}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaOperacion);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraApertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioApertura ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioCierre ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioVendedor ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.IdUsuarioCajero ?? DBNull.Value);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Estatus);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.GranTotalApertura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.GranTotalCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusPv);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EnvioCierre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaHoraEnvio);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 4. usuario ----
                        if (objUsuario.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO usuario (idfran, Id_Usuario, Nombre, Id_Idioma, LlaveAcceso, FechaUltimoCambio, CambiarLlaveAcceso, Temporal, EstatusRegistro) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objUsuario)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdUsuario);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdIdioma);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.LlaveAcceso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaUltimoCambio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CambiarLlaveAcceso);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Temporal);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 5. categoriacomercial ----
                        if (objCategoriaComercial.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO categoriacomercial (Id_CategoriaComercial, Nombre, EstatusRegistro, grupo, orden) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objCategoriaComercial)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaComercial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Nombre);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Grupo);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Orden);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 6. categoriacomercialproducto ----
                        if (objCategoriaComercialProducto.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO categoriacomercialproducto (Id_CategoriaComercial, Id_Producto, EstatusRegistro) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objCategoriaComercialProducto)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdCategoriaComercial);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdProducto);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.EstatusRegistro);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 7. consulta_consultorioturnodetalle ----
                        if (objConsultaConsultorioTurnoDetalle.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO consulta_consultorioturnodetalle (idfran, Id_ConsultorioTurno, Id_Consultorio, Id_Turno, Id_MotivoSinConsultas, CorreccionCaptura, Observaciones, FechaCaptura, UsuarioCaptura, FechaModificacion) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objConsultaConsultorioTurnoDetalle)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}, @p{paramIndex + 2}, @p{paramIndex + 3}, @p{paramIndex + 4}, @p{paramIndex + 5}, @p{paramIndex + 6}, @p{paramIndex + 7}, @p{paramIndex + 8}, @p{paramIndex + 9}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorioTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdConsultorio);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdTurno);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.IdMotivoSinConsultas);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.CorreccionCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Observaciones);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.UsuarioCaptura);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.FechaModificacion);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        // ---- 8. operacionglobal ----
                        if (objOperacionGlobal.Count > 0)
                        {
                            var cmd = connection.CreateCommand();
                            cmd.Transaction = mySqlTransaction;
                            cmd.CommandTimeout = 180;
                            var sb = new StringBuilder("INSERT IGNORE INTO operacionglobal (idfran, versiondb) VALUES ");
                            var paramIndex = 0;
                            foreach (var item in objOperacionGlobal)
                            {
                                sb.Append($"(@p{paramIndex}, @p{paramIndex + 1}),");
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", item.Idfran);
                                cmd.Parameters.AddWithValue($"@p{paramIndex++}", (object)item.Versiondb ?? DBNull.Value);
                            }
                            cmd.CommandText = sb.ToString().TrimEnd(',') + ";";
                            cmd.ExecuteNonQuery();
                        }

                        DateTime dtFechaFin = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
                        string sQuery = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = 4";
                        context.Database.ExecuteSqlRaw(sQuery, dtInicioProceso, dtFechaFin, objfranquicia.Idfran);

                        transaction.Commit();
                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseOperacion>("La información se ha guardado correctamente");
                    }
                    catch (Exception ex)
                    {
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error clsOperacionDatos-InsertarOperacionAuditoriaManual " + ex.ToString());
                    }
                }
            }
            return objRespuesta;
        }

    }


}
