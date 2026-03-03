using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text;
using GrpcVentas.Modelo;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace GrpcVentas.AccesoDato
{
    public class clsAuditoriaDatos
    {
        //Insertar registros de seguimiento
        public static DataResponseAuditoria InsertarAuditoriasManual(DatosCorporativo objCorporativo, Franquicia objfranquicia, DateTime dtInicioProceso, string stAnio, string stMes, int iIdoperacion, string stDia)
        {
            DataResponseAuditoria objRespuesta = new DataResponseAuditoria();

            string nombreColumnaDia = $"dia{stDia}";

            string sQuery = $@"INSERT INTO tv_envioauditoria 
                                (idfran, anio, mes, idOperacion, {nombreColumnaDia})
                                VALUES (@pIdfran, @pAnio, @pMes, @pIdOperacion, 1)
                                ON DUPLICATE KEY UPDATE
                                {nombreColumnaDia} = 1;";

            using (var context = new VentasContext(objCorporativo))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var connection = context.Database.GetDbConnection() as MySqlConnection;
                        var mySqlTransaction = transaction.GetDbTransaction() as MySqlTransaction;

                        if (connection == null || mySqlTransaction == null)
                        {
                            throw new InvalidOperationException("Esta operación requiere una conexión a MySQL.");
                        }

                        var commandAuditoria = connection.CreateCommand();
                        commandAuditoria.Transaction = mySqlTransaction;
                        commandAuditoria.CommandTimeout = 60;

                        commandAuditoria.CommandText = sQuery;

                        commandAuditoria.Parameters.AddWithValue("@pIdfran", objfranquicia.Idfran);
                        commandAuditoria.Parameters.AddWithValue("@pAnio", stAnio);
                        commandAuditoria.Parameters.AddWithValue("@pMes", stMes);
                        commandAuditoria.Parameters.AddWithValue("@pIdOperacion", iIdoperacion);

                        int rowsAffected = commandAuditoria.ExecuteNonQuery();

                        transaction.Commit();

                        string mensaje = (rowsAffected > 0)
                            ? "Registro de auditoria procesado correctamente (INSERT o UPDATE)."
                            : "Advertencia: La operación no afectó filas. Revisar.";


                        DateTime dtfechaFininsert = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

                        string sQueryTV = "UPDATE tv_enviocontrol SET FechaInicio = @p0, fechaFin = @p1, version = 1 WHERE idFran = @p2 AND idOperacion = @p3";
                        context.Database.ExecuteSqlRaw(sQueryTV, dtInicioProceso, dtfechaFininsert, objfranquicia.Idfran, iIdoperacion);

                        objRespuesta = clsRespuestaFactory.CrearRespuestaExito<DataResponseAuditoria>(mensaje);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        objRespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseAuditoria>("Error al insertar y/o actualizar información en clsAuditoriaDatos - InsertarAuditoriasManual: " + ex.Message);
                    }
                }
            }
            return objRespuesta;
        }

        public static string GenerarHashTabla(DatosCorporativo objCorporativo, string idfran, DateTime FechaAuditoria, string nombreTabla)
        {
            string sQuery = "";
            string Fecha = FechaAuditoria.ToString("yyyy-MM-dd");

            switch (nombreTabla)
            {
                case "venta":
                    sQuery = $@"SELECT Id_Venta, Id_Venta_Local, Id_Venta_Consecutivo, Id_Venta_Registradora, Id_Usuario_Venta, Id_Usuario_Cobro, TipoVenta, TipoOperacion, Estatus
                         FROM venta
                         WHERE id_venta <> 0 AND estatus IN(0, 16) AND idfran = {idfran} AND FechaOperacion='{Fecha}'
                         ORDER BY id_venta";
                    break;

                case "Venta_Producto":
                    sQuery = $@"SELECT vp.Id_Venta, vp.Id_Venta_Local, vp.Id_Venta_Consecutivo, vp.Id_Producto
                         FROM Venta v INNER JOIN Venta_Producto vp ON(v.idfran = vp.idfran AND v.id_venta = vp.Id_venta)
                         WHERE v.id_venta <> 0 AND v.estatus IN(0, 16) AND v.idfran = {idfran} AND v.FechaOperacion = '{Fecha}'
                         ORDER BY vp.id_venta, vp.Id_Venta_Local, vp.Id_Venta_Consecutivo, vp.Id_Producto";
                    break;

                case "Venta_Pago":
                    sQuery = $@"SELECT vp.Id_Venta, vp.Id_Venta_Local, vp.Id_Venta_Consecutivo, vp.Id_FormaPago, vp.Importe
                         FROM Venta v INNER JOIN Venta_Pago vp ON(v.idfran = vp.idfran AND v.id_venta = vp.Id_venta)
                         WHERE v.id_venta <> 0 AND v.estatus IN(0, 16) AND v.idfran = {idfran} AND v.FechaOperacion = '{Fecha}'
                         ORDER BY vp.id_venta, vp.Id_Venta_Local, vp.Id_Venta_Consecutivo, vp.Id_FormaPago";
                    break;
                case "Facturacion_Factura":
                    sQuery = $@"SELECT serie 'Serie', folio 'Folio'
                         FROM facturacion_factura
                         WHERE idfran={idfran} AND estatus = 1 AND fechaoperacion ='{Fecha}'
                         ORDER BY serie, folio";
                    break;

                case "Facturacion_Referencia":
                    sQuery = $@"SELECT a.Serie, a.Folio, a.id_venta, a.id_venta_local, a.id_venta_consecutivo
                         FROM facturacion_referencia a 
                         INNER JOIN Facturacion_Factura b ON (a.idfran=b.idfran and a.serie=b.serie and a.folio=b.folio)
                         WHERE a.idfran={idfran} AND b.FechaOperacion ='{Fecha}'
                         ORDER BY a.serie, a.folio, a.id_venta, a.id_venta_local, a.id_venta_consecutivo";
                    break;
                case "Inventario_Fisico_Completo":
                    sQuery = $@"SELECT FechaOperacion, Id_Producto, ExistenciaInicial, ExistenciaFinal
                         FROM Inventario_Fisico_Completo
                         WHERE FechaOperacion = '{Fecha}' AND idFran={idfran}";
                    break;

                case "Inventario_Otros":
                    sQuery = $@"SELECT Id_Registro, Id_Movimiento, Documento, Referencia, Signo, Id_Usuario, Observacion, SincRef, Id_Tipo
                         FROM Inventario_Otros
                         WHERE fechaoperacion = '{Fecha}' AND idFran={idfran}";
                    break;

                case "Inventario_Surtido":
                    sQuery = $@"SELECT Id_Surtido, Id_Surtido_Local, Id_Movimiento, Documento, Referencia, Id_Usuario, Factura, Factura_Fiscal, SurtidoElectronico, Id_Proveedor
                         FROM Inventario_Surtido
                         WHERE fechaoperacion = '{Fecha}' AND idFran={idfran}";
                    break;

                case "Inventario_Traspaso":
                    sQuery = $@"SELECT Id_Farmacia_Entrega, Id_Traspaso, Id_Farmacia_Pedido, Id_Concepto, Id_Movimiento
                         FROM Inventario_Traspaso
                         WHERE fechaoperacion = '{Fecha}' AND idFran={idfran}";
                    break;

                default:
                    // Si el nombre de la tabla no es reconocido, retorna una consulta vacía.
                    return "";
            }

            // El resto de la lógica de conexión y hash se mantiene:
            var dtVenta = GetDataTable(sQuery, objCorporativo);

            if (dtVenta.Rows.Count == 0) return "";

            string cadenaEnvio = JsonConvert.SerializeObject(dtVenta, Formatting.None);
            // Asume que clsGeneral.MD5Hash está disponible
            return clsGeneral.MD5Hash(cadenaEnvio);
        }

        public static DataTable GetDataTable(string queryString, DatosCorporativo objCorporativo)
        {
            string sCn = "server = " + objCorporativo.Hst + "; user = " + objCorporativo.UsrSyncro2 + "; password = " + objCorporativo.PssSyncro2 + "; database = " + objCorporativo.DbSyncro2;

            DataTable dt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(sCn))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = new MySqlCommand(queryString, connection);
                    adapter.Fill(dt);

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    connection.Close();
                }
                return dt;
            }
        }

        public static async Task InsertarOActualizarHashBase(VentasContext context,string idfran,int anio,byte mes,byte idOperacion)
        {
            // Buscar si el registro existe por la clave primaria
            var existingHash = await context.TvHashauditoria
                .FirstOrDefaultAsync(h => h.idfran == idfran && h.anio == anio && h.mes == mes && h.idOperacion == idOperacion);

            if (existingHash == null)
            {
                // El registro no existe, creamos el objeto con los valores por defecto (hashes en "0")
                var newHashEntry = new TvHashauditoria
                {
                    idfran = idfran,
                    anio = anio,
                    mes = mes,
                    idOperacion = idOperacion
                    // Las propiedades hash1...hash31 se inicializan a "0" por el constructor/inicializador del modelo
                };

                context.TvHashauditoria.Add(newHashEntry);
                await context.SaveChangesAsync();
            }
            // Si existe, no hacemos nada porque los campos base ya están (esto simula la primera parte de tu código original)
        }

        public static async Task<TvHashauditoria> ActualizarHashDiaYRetornar(VentasContext context, string idfran, int anio, byte mes, byte idOperacion, int dia, string sHash)
        {
            string nombreColumnaHash = $"hash{dia}";

            // 1. OBTENER LA ENTIDAD RASTREADA O DE LA BASE DE DATOS.
            var hashToUpdate = await context.TvHashauditoria
                .FirstOrDefaultAsync(h => h.idfran == idfran && h.anio == anio && h.mes == mes && h.idOperacion == idOperacion);

            if (hashToUpdate == null)
            {
                // Manejar o registrar error si el registro no se encontró
                return null;
            }

            // 2. Usar Reflection para obtener la propiedad de la columna que queremos actualizar.
            PropertyInfo propertyInfo = typeof(TvHashauditoria).GetProperty(nombreColumnaHash);

            if (propertyInfo != null)
            {
                // 3. Asignar el nuevo valor y guardar (EF Core detecta el cambio).
                propertyInfo.SetValue(hashToUpdate, sHash);
                await context.SaveChangesAsync();

                // 4. Retornar el objeto que acaba de ser actualizado.
                return hashToUpdate;
            }
            else
            {
                throw new ArgumentException($"La columna {nombreColumnaHash} no existe en TvHashauditoria.");
            }
        }

    }
}
