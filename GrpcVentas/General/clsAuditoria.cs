using Azure.Core;
using Google.Protobuf;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;

namespace GrpcVentas.General
{
    public class clsAuditoria
    {
        /// Metodo del procesado de auditorias tv_envioauditoria
        public static DataResponseAuditoria GuardarAuditoriasManual(AuditoriasDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseAuditoria objrespuesta = new DataResponseAuditoria();

            try
            {
                //Se acomodan los datos
                string Sclave = request.Clave;
                string stAnio = request.Anio;
                string stMes = request.Mes;
                int iIdoperacion = request.IdOperacion;
                string stDia = request.Dia;

                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        objrespuesta = clsAuditoriaDatos.InsertarAuditoriasManual(objCorporativo, objfranquicia, dtInicioProceso, stAnio, stMes, iIdoperacion, stDia);
                    }
                    else
                    {
                        objrespuesta.MensajeRespuesta = "Error 304, Problemas de información de sucursal. clsAuditoria - GuardarAuditoriasManual";
                        objrespuesta.MensajeError = "Sucursal" + Sclave;
                        objrespuesta.EstatusCodigo = 304;
                    }
                }
                else
                {
                    objrespuesta.MensajeError = "Clave no encontrada." + Sclave;
                    objrespuesta.EstatusCodigo = 304;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseAuditoria>("Error clsAuditoria-GuardarAuditoriasManual " + ex);
                return objrespuesta;
            }
        }


        //Generar HashAuditoria

        public static async Task<DataResponseAuditoria> GenerarHasServerMejorado(CrearAuditoriaDataRequest objdatos ,DatosCorporativo objCorporativo){
            var objRespuesta = new DataResponseAuditoria();
            string idfran = "";
            Dictionary<string, TvHashauditoria> dicHashesGenerados = new Dictionary<string, TvHashauditoria>();

            try
            {
                // 1. Obtener Franquicia y datos iniciales
                Franquicia Franquicia = clsGeneralDatos.GetFranquicia(objdatos.Clave, objCorporativo);
                idfran = Franquicia.Idfran.ToString();
                DateTime FechaOperacion = DateTime.Now.AddDays(-1);
                byte bIdOperacion = (byte)objdatos.IdOperacion;

                // Obtener la cantidad de días a auditar. Se mantiene el uso de ClsFuncionesBD
                int Dias = clsGeneralDatos.IntQuery("select diasAuditar from desk_catalogo_operacion where idOperacion=" + bIdOperacion, objCorporativo);

                using (var context = new VentasContext(objCorporativo))
                {
                    for (int i = 0; i <= Dias; i++) // Recorrido por días a realizar la auditoría
                    {
                        DateTime FechaAuditoria = FechaOperacion.AddDays(-i);
                        int dia = FechaAuditoria.Day;
                        byte Mes = (byte)FechaAuditoria.Month;
                        int anio = FechaAuditoria.Year;
                        

                        // --- Generación de Hashes (Manteniendo tu lógica original) ---
                        // Nota: Idealmente, estas consultas a las tablas 'venta', 'Venta_Producto', etc., 
                        // también deberían refactorizarse a LINQ con EF Core para mayor seguridad y consistencia.

                        // **2. Generación de Hashes Centralizada**
                        // Se llama a una nueva función que encapsula la lógica de Hash de Venta O Factura.
                        string sHash = GenerarHashParaOperacion(objCorporativo, idfran, FechaAuditoria, bIdOperacion);


                        // 3. Insertar o Actualizar el registro base (UPSERT) en TvHashauditoria
                        await clsAuditoriaDatos.InsertarOActualizarHashBase(context, idfran, anio, Mes, bIdOperacion);

                        // 4. Actualizar dinámicamente la columna 'hashN' para el día específico (EF Core)
                        TvHashauditoria hashActualizado = await clsAuditoriaDatos.ActualizarHashDiaYRetornar(context, idfran, anio, Mes, bIdOperacion, dia, sHash);

                        if (hashActualizado != null)
                        {
                            // Crear una clave única que identifica el registro mensual (la llave primaria)
                            string claveAuditoria = $"{idfran}_{anio}_{Mes}_{bIdOperacion}";

                            // Si la clave (el mes) no está en el diccionario, la agregamos.
                            if (!dicHashesGenerados.ContainsKey(claveAuditoria))
                            {
                                // Añadir el registro completo del mes.
                                dicHashesGenerados.Add(claveAuditoria, hashActualizado);
                            }
                            // Si ya existe, no hacemos nada. El objeto ya está en el diccionario
                            // y sus referencias internas se actualizarán si TvHashauditoria es una clase.
                        }


                    }
                }

                List<TvHashauditoria> listaHashesGenerados = dicHashesGenerados.Values.ToList();
                // 1. Serializar la lista de objetos C# a una cadena JSON (texto)
                // Se utiliza Newtonsoft.Json (JsonConvert) para esta tarea.
                string jsonAuditoria = JsonConvert.SerializeObject(listaHashesGenerados, Formatting.None);

                // 2. Convertir la cadena JSON (texto) a un array de bytes (byte[])
                // Se recomienda usar la codificación UTF-8.
                byte[] requestData = Encoding.UTF8.GetBytes(jsonAuditoria);

                // 3. Comprimir el array de bytes resultante.
                // Aquí se llama a tu función de compresión, pasándole los bytes del JSON.
                byte[] DataGzip = clsGeneral.CompressData(requestData);

                // Ahora 'DataGzip' contiene los datos de auditoría serializados y comprimidos.
                // Estos son los bytes que luego puedes convertir a Base64 para el envío.

                // El MensajeRespuesta ahora puede ser un simple mensaje de estado
                objRespuesta.MensajeRespuesta = "Auditoría de Hash generada y comprimida.";
                objRespuesta.EstatusCodigo = StatusCodes.Status200OK;
                objRespuesta.Compressdata = ByteString.CopyFrom(DataGzip);

                return objRespuesta;
            }
            catch (Exception ex)
            {
                // Manejo de errores simplificado
                objRespuesta.EstatusCodigo = StatusCodes.Status500InternalServerError;
                objRespuesta.MensajeError = $"Error ClsAuditoria-GenerarHasServer: {ex.Message}";

                // En entornos de producción, se recomienda loggear el error completo (ex.ToString())
                return objRespuesta;
            }
        }

        public static string GenerarHashParaOperacion(DatosCorporativo objCorporativo, string idfran, DateTime FechaAuditoria, byte idOperacion)
        {
            string sHash = "";

            if (idOperacion == 1) // Lógica para Auditoría de Venta
            {
                string Hashventa = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "venta");
                string HashventaProducto = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Venta_Producto");
                string HashVentaPago = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Venta_Pago");

                sHash = Hashventa + HashventaProducto + HashVentaPago;
            }
            else if (idOperacion == 2) // Lógica para Auditoría de Factura
            {
                // Tu lógica de Factura refactorizada en funciones.
                string HashFactuFact = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Facturacion_Factura");
                string HashFactref = clsAuditoriaDatos. GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Facturacion_Referencia");

                sHash = HashFactuFact + HashFactref;
            }
            else if (idOperacion == 2) // Lógica para Auditoría de Factura
            {
                // Tu lógica de Factura refactorizada en funciones.
                string HashFactuFact = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Facturacion_Factura");
                string HashFactref = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Facturacion_Referencia");

                sHash = HashFactuFact + HashFactref;
            }
            else if (idOperacion == 3) // **Lógica para Auditoría de Inventario (NUEVO)**
            {
                // Generar los hashes específicos para Inventario
                string HashInventarioFisicoCompleto = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Inventario_Fisico_Completo");
                string HashInventarioOtros = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Inventario_Otros");
                string HashInventarioSurtido = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Inventario_Surtido");
                string HashInventarioTraspaso = clsAuditoriaDatos.GenerarHashTabla(objCorporativo, idfran, FechaAuditoria, "Inventario_Traspaso");

                // Concatenar los hashes de Inventario
                sHash = HashInventarioFisicoCompleto + HashInventarioOtros + HashInventarioSurtido + HashInventarioTraspaso;
            }

            return sHash;
        }



    }
}
