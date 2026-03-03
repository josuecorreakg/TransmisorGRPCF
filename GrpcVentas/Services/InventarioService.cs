using Grpc.Core;
using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;
using System.Runtime.Intrinsics.X86;

namespace GrpcVentas.Services
{
    public class InventarioService:RespuestaInventarioProto.RespuestaInventarioProtoBase
    {
        public override async Task<DataResponseInventario> sayInventarioDataByte(InventarioDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseInventario objrespuesta = new DataResponseInventario();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var varInventarioData = InventarioDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(varInventarioData.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsInventario.GuardarInventarioManualBulk(varInventarioData, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + varInventarioData.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }

        public override async Task<DataResponseInventario> sayInventarioTiempoRealDataByte(InventarioDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseInventario objrespuesta = new DataResponseInventario();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var varInventarioData = InventarioTiempoRealRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(varInventarioData.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsInventario.GuardarInventarioTiempoRealBulk(varInventarioData, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + varInventarioData.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }


    }
}
