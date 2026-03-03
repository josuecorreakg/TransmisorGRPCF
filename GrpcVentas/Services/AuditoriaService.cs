using Grpc.Core;
using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;

namespace GrpcVentas.Services
{
    public class AuditoriaService:RespuestaAuditoriaProto.RespuestaAuditoriaProtoBase
    {
        public override async Task<DataResponseAuditoria> sayEnvioAuditoriaDataByte(AuditoriaDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseAuditoria objrespuesta = new DataResponseAuditoria();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var AuditoriaDataRequest = AuditoriasDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(AuditoriaDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsAuditoria.GuardarAuditoriasManual(AuditoriaDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + AuditoriaDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;

            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseAuditoria>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponseAuditoria> sayCrearAuditoriaDataByte(AuditoriaDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseAuditoria objrespuesta = new DataResponseAuditoria();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var AuditoriaDataRequest = CrearAuditoriaDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(AuditoriaDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = await clsAuditoria.GenerarHasServerMejorado(AuditoriaDataRequest, objCorporativo);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + AuditoriaDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;

            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseAuditoria>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }


    }
}
