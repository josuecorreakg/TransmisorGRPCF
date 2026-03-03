using Grpc.Core;
using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo;
using GrpcVentas.Modelo.DBCorporativo;

namespace GrpcVentas.Services
{
    public class VentasService : RespuestaVentasProto.RespuestaVentasProtoBase
    {
        public override async Task<DataResponse> sayVentasDataByte(VentasDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponse objrespuesta = new DataResponse();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var ventasDataRequest = VentasDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(ventasDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsVentas.GuardarVentasManualBulk(ventasDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + ventasDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;

            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponse> sayHistoricosDataByte(VentasDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponse objrespuesta = new DataResponse();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var historicoDataRequest = HistoricosDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(historicoDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsVentas.GuardarHistoricosManualBulk(historicoDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + historicoDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponse> sayCostoOpDataByte(VentasDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponse objrespuesta = new DataResponse();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var CostoDataRequest = CostoOpDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(CostoDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsVentas.GuardarCostoOpBulk(CostoDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + CostoDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponse> sayPrueba(VentasPrueba request, ServerCallContext context)
        {
            DataResponse objrespuesta = new DataResponse();
            try
            {
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn("Alpacel");
                if (objCorporativo != null)
                {
                    objrespuesta.MensajeRespuesta = objCorporativo.NombreCorto;
                    objrespuesta.EstatusCodigo = StatusCodes.Status200OK;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta.EstatusCodigo = StatusCodes.Status500InternalServerError;
                objrespuesta.MensajeError = ex.ToString();
                return objrespuesta;
            }
        }

    }
}
