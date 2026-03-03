using Grpc.Core;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.General;

namespace GrpcVentas.Services
{
    public class UsuarioService:RespuestaUsuarioProto.RespuestaUsuarioProtoBase
    {
        public override async Task<DataResponseUsuario> sayUsuarioDataByte(UsuarioDataByteRequest request, ServerCallContext context)
        {
            DateTime dtInicioProceso = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);
            DataResponseUsuario objrespuesta = new DataResponseUsuario();
            try
            {
                byte[] originalData = clsGeneral.DecompressData(request.Compressdata.ToByteArray());
                var UsuarioDataRequest = lsUsuarioDataRequest.Parser.ParseFrom(originalData);

                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(UsuarioDataRequest.Nombrecorto);
                if (objCorporativo != null)
                {
                    objrespuesta = clsUsuario.GuardarUsuarioManualBulk(UsuarioDataRequest, objCorporativo, dtInicioProceso);
                }
                else
                {
                    objrespuesta.MensajeError = "Cliente no encontrado." + UsuarioDataRequest.Nombrecorto;
                    objrespuesta.EstatusCodigo = 104;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseUsuario>("Ocurrio el error 104 al procesar la información: " + ex);
                return objrespuesta;
            }
        }
    }
}
