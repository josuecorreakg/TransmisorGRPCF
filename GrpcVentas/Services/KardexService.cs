using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.General;
using Grpc.Core;

namespace GrpcVentas.Services
{
    public class KardexService:RespuestaKardexProto.RespuestaKardexProtoBase
    {
        public override async Task<DataResponseKardex> sayKardexDataByte(KardexDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseKardex objrespuesta = new DataResponseKardex();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var varkardexDataRequest = KardexDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(varkardexDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsKardex.GuardarKardexManualBulk(varkardexDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + varkardexDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponseKardex> sayKardexPrecioPromedioDataByte(KardexDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseKardex objrespuesta = new DataResponseKardex();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var varkardexDataRequest = KardexPrecioPromedioDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(varkardexDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsKardex.GuardarKardexControlMensual(varkardexDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + varkardexDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }


    }
}
