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

    }
}
