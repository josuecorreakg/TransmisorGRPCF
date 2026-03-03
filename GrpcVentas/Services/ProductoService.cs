using Grpc.Core;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;

namespace GrpcVentas.Services
{
    public class ProductoService:RespuestaProductoProto.RespuestaProductoProtoBase
    {
        public override async Task<DataResponseProducto> sayProductoDataByte(ProductoDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseProducto objrespuesta = new DataResponseProducto();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var varProductoData = lsProductoDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(varProductoData.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsProducto.GuardarProductoManualBulk(varProductoData, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + varProductoData.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseProducto>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }
    }
}
