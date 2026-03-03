using Grpc.Core;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;

namespace GrpcVentas.Services
{
    public class OperacionService:RespuestaOperacionProto.RespuestaOperacionProtoBase
    {
        public override async Task<DataResponseOperacion> sayOperacionDataByte(OperacionDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseOperacion objrespuesta = new DataResponseOperacion();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var OperacionDataRequest = lsOperacionDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(OperacionDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsOperacion.GuardarOperacionManualBulk(OperacionDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + OperacionDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponseOperacion> sayTemperaturaDataByte(OperacionDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseOperacion objrespuesta = new DataResponseOperacion();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var OperacionDataRequest = lsTemperaturaRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(OperacionDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsOperacion.GuardarTemperaturaManualBulk(OperacionDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + OperacionDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponseOperacion> sayOperacionAuditoriaDataByte(OperacionDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseOperacion objrespuesta = new DataResponseOperacion();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var OperacionDataRequest = lsOperacionAuditoriaDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(OperacionDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsOperacion.GuardarOperacionAuditoriaManualBulk(OperacionDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + OperacionDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

    }
}
