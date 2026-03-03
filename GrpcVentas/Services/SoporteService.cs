using Azure;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Notificaciones;
using Microsoft.AspNetCore.SignalR;
using Respuestacorporativo;
using System.IO.Compression;
using System.Text.Json;
using static GrpcVentas.Notificaciones.clsHub;

namespace GrpcVentas.Services
{
    public class SoporteService:RespuestaCorporativoProto.RespuestaCorporativoProtoBase
    {
        private readonly IHubContext<clsHub> _hubContext;

        public SoporteService(IHubContext<clsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public override async Task<DataCorporativoResponse> sayGetCorporativoDataByte(Empty request, ServerCallContext context)
        {
            DataCorporativoResponse objrespuesta = new DataCorporativoResponse();
            try
            {
                var listaCorporativo = clsGeneralDatos.GetDatosCorporativo();
                objrespuesta.MensajeError = listaCorporativo.Error;
                foreach (var datos in listaCorporativo.Datos)
                {
                    var item = new protoSoporteDatosCorporativo
                    {
                        Corporativo = datos.Corporativo,
                        Dominio = datos.Dominio ?? "",
                        DmnNCPTD = datos.DmnNcptd ?? "",
                        Cnxn = datos.Cnxn ?? "",
                        Nombre = datos.Nombre ?? "",
                        NombreCorto = datos.NombreCorto ?? "",
                        Hst = datos.Hst ?? "",
                        ErpUsr = datos.ErpUsr ?? "",
                        ErpPss = datos.ErpPss ?? "",
                        ErpDb = datos.ErpDb ?? "",
                        PresupuestosHost = datos.PresupuestosHost ?? "",
                        PresupuestosUsr = datos.PresupuestosUsr ?? "",
                        PresupuestosPss = datos.PresupuestosPss ?? "",
                        PresupuestosDb = datos.PresupuestosDb ?? "",
                        Dsn = datos.Dsn ?? "",
                        Dsn1 = datos.Dsn1 ?? "",
                        Dsnasistencia = datos.Dsnasistencia ?? "",
                        Dsntr = datos.Dsntr ?? "",
                        UsrSyncro2 = datos.UsrSyncro2 ?? "",
                        PssSyncro2 = datos.PssSyncro2 ?? "",
                        MaxCnnSyncro2 = datos.MaxCnnSyncro2 ?? 0,
                        DbSyncro2 = datos.DbSyncro2 ?? "",
                        DiasAudit = datos.DiasAudit ?? "",
                        DiasEvaluar = datos.DiasEvaluar ?? 0,
                        HoraAudit = datos.HoraAudit ?? "",
                        UsrAsis = datos.UsrAsis ?? "",
                        PssAsis = datos.PssAsis ?? "",
                        AplicaMonitor = (datos.AplicaMonitor ?? 0) == 1,
                        AplicaDocker = (datos.AplicaDocker ?? 0) == 1,
                        PonderacionYDesabasto = (datos.PonderacionYdesabasto ?? 0) == 1,
                        AplicaFacturacionEnLinea = (datos.AplicaFacturacionEnLinea ?? 0) == 1,
                        AplicaBitacora = (datos.AplicaBitacora ?? 0) == 1,
                        FacturacionEnAzure = (datos.FacturacionEnAzure ?? 0) == 1,
                        ActualizaDescuentosFacturacion = (datos.ActualizaDescuentosFacturacion ?? 0) == 1,
                        Razonsocial = datos.Razonsocial ?? "",
                        Rfc = datos.Rfc ?? "",
                        UUID = datos.Uuid ?? ""
                    };
                    objrespuesta.LsProtoDatosCorporativos.Add(item);
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataCorporativoResponse>("Ocurrio un error al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<SoporteDataByteRequest> sayGetFarmaciasDataByte(FranquiciasDataByteRequest request, ServerCallContext context)
        {
            SoporteDataByteRequest objrespuesta = new SoporteDataByteRequest();

            try
            {
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(request.StNombreCliente);

                if (objCorporativo != null)
                {
                    // Si el cliente existe, procesar normalmente
                    objrespuesta = clsSoporte.GetFranquicias(objCorporativo);
                }
                else
                {
                    var dataResponseSoporte = new DataResponseSoporte
                    {
                        MensajeError = "Cliente no encontrado: " + request.StNombreCliente,
                        EstatusCodigo = 404,
                        MensajeRespuesta = "",
                        LsProtoFranquicias = { },
                        LsProtoEnvioControl = { }
                    };
                    var response = clsSoporte.CrearRespuestaComprimida(dataResponseSoporte);
                    return response;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                var errorResponse = new DataResponseSoporte
                {
                    MensajeError = "Ocurrió un error: " + ex.Message,
                    EstatusCodigo = 500,
                    MensajeRespuesta = "Error interno del servidor",
                    LsProtoFranquicias = { },
                    LsProtoEnvioControl = { }
                };
                return clsSoporte.CrearRespuestaComprimida(errorResponse);
            }
        }

        public override async Task<NotificacionResponse> EnviarNotificacion(NotificacionRequest request, ServerCallContext context)
        {
            var notificacion = new Notificacion(request.Operacion, request.FechaInicio, request.FechaFin);

            try
            {
                await _hubContext.Clients.Client(request.ConnectionId).SendAsync("SendNotificaciones", notificacion);
                return new NotificacionResponse
                {
                    Status = $"✅ Notificación enviada a {request.ConnectionId}"
                };
            }
            catch (Exception ex)
            {
                return new NotificacionResponse
                {
                    Status = $"❌ Error al enviar a {request.ConnectionId}: {ex.Message}"
                };
            }

        }

        public override async Task<JoinResponse> JoinClientConnection(JoinRequest request, ServerCallContext context)
        {
            JoinResponse objrespuesta = new JoinResponse();
            bool response = false;

            try
            {
                response = ConnectionData.JoinConnection(request.IdconnectClient, request.Cliente, request.Clave, request.Nombre, request.Operacion);
                objrespuesta = new JoinResponse
                {
                    Success = response
                };
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = new JoinResponse
                {
                    Success = false
                };
                return objrespuesta;
            }
        }

        public override async Task<JoinResponse> ExitClientConnection(JoinRequest request, ServerCallContext context)
        {
            JoinResponse objrespuesta = new JoinResponse();
            bool response = false;

            try
            {
                response = ConnectionData.ExitConnection(request.IdconnectClient, request.Cliente);
                objrespuesta = new JoinResponse
                {
                    Success = response
                };
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = new JoinResponse
                {
                    Success = false
                };
                return objrespuesta;
            }
        }


    }
}
