using Grpc.Core;
using GrpcVentas.AccesoDato;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;

namespace GrpcVentas.Services
{
    public class FacturaService : RespuestaFacturasProto.RespuestaFacturasProtoBase
    {
        public override async Task<DataResponseFacturas> sayFacturaDataByte(FacturasDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

            DataResponseFacturas objrespuesta = new DataResponseFacturas();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var facturasDataRequest = lsFacturaDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(facturasDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsFactura.GuardarFacturasManualBulk(facturasDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + facturasDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseFacturas>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }
    }
}
