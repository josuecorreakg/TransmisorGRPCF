using Grpc.Core;
using GrpcVentas;
using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;

namespace GrpcVentas.Services
{
    public class ConfiguracionService : RespuestaConfiguracionProto.RespuestaConfiguracionProtoBase
    {
        public override async Task<DataResponseConfiguracion> saySETConfiguracionDataByte(ConfiguracionDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseConfiguracion objrespuesta = new DataResponseConfiguracion();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var ConfiguracionDataRequest = protodataConfiguracion.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(ConfiguracionDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsConfiguracion.GuardarYObtenerConfiguracion(ConfiguracionDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + ConfiguracionDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;

            }
            catch (Exception ex)
            {
                //objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<protoRespuestaLiberaciones> sayGETLiberacionesPendientes(protodataConfiguracion request, ServerCallContext context)
        {
            protoRespuestaLiberaciones objrespuesta = new protoRespuestaLiberaciones();
            try
            {
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(request.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsLiberacion.ObtenerLiberacionesPendientes(request, objCorporativo);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + request.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception)
            {
                return objrespuesta;
            }
        }

        public override async Task<DataResponseConfiguracion> saySETEstatusLiberacion(protoLiberacionEjecutable request, ServerCallContext context)
        {
            DataResponseConfiguracion objrespuesta = new DataResponseConfiguracion();
            try
            {
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(request.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsLiberacion.ActualizarEstatusLiberacion(request, objCorporativo);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + request.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception)
            {
                return objrespuesta;
            }
        }

    }
}
