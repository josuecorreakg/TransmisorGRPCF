using Google.Protobuf;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore;
using Respuestacorporativo;
using System.IO.Compression;
using System.Text.Json;

namespace GrpcVentas.General
{
    public class clsSoporte
    {
        public static SoporteDataByteRequest GetFranquicias(DatosCorporativo objCorporativo)
        {
            DataResponseSoporte objrespuesta = new DataResponseSoporte();
            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    try
                    {
                        // Obtiene sucursales
                        var listaSucursales = context.Database
                                            .SqlQueryRaw<ConnectionClientResult>("SELECT IFNULL(Cliente, '') AS Cliente, IFNULL(Clave, '') AS Clave, IFNULL(Nombre, '') AS Nombre, IFNULL(ConnectionId, '') AS ConnectionId, FechaConnection, Id_Operacion, IFNULL(Componentes, '') AS Componentes, IFNULL(Internet, '') AS Internet FROM connectionclient WHERE Id_Operacion=1;")
                                            .ToList();

                        List<protoConnectionClient> lsFranquiciaClients = new List<protoConnectionClient>();

                        foreach (var item in listaSucursales)
                        {
                            var protoFranquiciaClient = new protoConnectionClient
                            {
                                Cliente = Convert.ToString(item.Cliente),
                                Clave = Convert.ToString(item.Clave),
                                Nombre = Convert.ToString(item.Nombre),
                                ConnectionId = Convert.ToString(item.ConnectionId),
                                IdOperacion = item.Id_Operacion, // Es int, así que no hay problema
                                FechaConnection = item.FechaConnection?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                                Internet = Convert.ToString(item.Internet)
                            };

                            lsFranquiciaClients.Add(protoFranquiciaClient);
                        }


                        // Obtiene datos de envio
                        var listaTvMonitoreo = context.Database
                                               .SqlQueryRaw<EnvioControlResult>("SELECT A.idfran, A.idOperacion, B.clave AS 'Clave', A.fechaInicio, A.fechaFin, A.statusSesion, A.activo, A.ultimoEnvio, A.version FROM tv_enviocontrol A inner join franquicias B on (A.idfran=B.idfran) WHERE B.activa=1;")
                                               .ToList();

                        List<protoEnvioControl> lsEnvioControl = new List<protoEnvioControl>();

                        foreach (var item in listaTvMonitoreo)
                        {
                            var protoEnviocontrol = new protoEnvioControl
                            {
                                IdFran = item.IdFran,
                                IdOperacion = item.IdOperacion,
                                Clave = item.Clave,
                                FechaInicio = item.FechaInicio?.ToString() ?? "",
                                FechaFin = item.FechaFin?.ToString() ?? "",
                                StatusSesion = item.StatusSesion ?? 0,
                                Activo = item.Activo ?? 0,
                                UltimoEnvio = item.UltimoEnvio ?? "",
                                Version = item.Version ?? 0
                            };

                            lsEnvioControl.Add(protoEnviocontrol);
                        }

                        objrespuesta.EstatusCodigo = 200;
                        objrespuesta.MensajeRespuesta = "Consulta realizada con éxito";
                        objrespuesta.MensajeError = "";

                        objrespuesta.LsProtoFranquicias.AddRange(lsFranquiciaClients);
                        objrespuesta.LsProtoEnvioControl.AddRange(lsEnvioControl);

                        return CrearRespuestaComprimida(objrespuesta);
                    }
                    catch (Exception ex)
                    {
                        objrespuesta.EstatusCodigo = 500;
                        objrespuesta.MensajeError = "No se pudo procesar la consulta: " + ex.Message + "Cliente:" + objCorporativo.NombreCorto + " Base:" + objCorporativo.DbSyncro2 + " Host:" + objCorporativo.Hst + " Usuario:" + objCorporativo.UsrSyncro2 + " Pass:" + objCorporativo.PssSyncro2;

                        return CrearRespuestaComprimida(objrespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                objrespuesta.EstatusCodigo = 500;
                objrespuesta.MensajeError = "Error al conectar con la base de datos: " + ex.Message;

                return CrearRespuestaComprimida(objrespuesta);
            }
        }

        public class ConnectionClientResult
        {
            public string Cliente { get; set; }
            public string Clave { get; set; }
            public string Nombre { get; set; }
            public string ConnectionId { get; set; }
            public int Id_Operacion { get; set; }
            public DateTime? FechaConnection { get; set; } // DateTime porque viene de MySQL
            public string Internet { get; set; }
        }

        public class EnvioControlResult
        {
            public int IdFran { get; set; }
            public byte IdOperacion { get; set; }
            public DateTime? FechaInicio { get; set; } 
            public DateTime? FechaFin { get; set; } 
            public byte? StatusSesion { get; set; } 
            public byte? Activo { get; set; } 
            public string? UltimoEnvio { get; set; } 
            public double? Version { get; set; }
            public string? Clave { get; set; }
        }

        public static SoporteDataByteRequest ConvertirADatosComprimidos(DataResponseSoporte data)
        {
            var jsonString = JsonSerializer.Serialize(data);

            byte[] compressedData;

            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                using (var writer = new StreamWriter(gzipStream))
                {
                    writer.Write(jsonString);
                }
                compressedData = outputStream.ToArray();
            }

            return new SoporteDataByteRequest
            {
                Compressdata = ByteString.CopyFrom(compressedData)
            };
        }

        public static SoporteDataByteRequest CrearRespuestaComprimida(DataResponseSoporte dataResponseSoporte)
        {
            // Serializar el objeto a JSON
            var jsonString = JsonSerializer.Serialize(dataResponseSoporte);

            // Comprimir el JSON usando GZip
            byte[] compressedData;
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                using (var writer = new StreamWriter(gzipStream))
                {
                    writer.Write(jsonString);
                }
                compressedData = outputStream.ToArray();
            }

            // Crear la respuesta en formato de bytes comprimidos
            return new SoporteDataByteRequest
            {
                Compressdata = ByteString.CopyFrom(compressedData)
            };
        }

    }
}
