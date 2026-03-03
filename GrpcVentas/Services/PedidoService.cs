using Grpc.Core;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;

namespace GrpcVentas.Services
{
    public class PedidoService:RespuestaPedidoProto.RespuestaPedidoProtoBase
    {
        public override async Task<DataResponsePedido> sayPedidoDataByte(PedidosDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponsePedido objrespuesta = new DataResponsePedido();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var pedidoDataRequest = lsPedidoDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(pedidoDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsPedido.GuardarPedidoManualBulk(pedidoDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + pedidoDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponsePedido>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }
    }
}
