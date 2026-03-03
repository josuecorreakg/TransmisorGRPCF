using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.AccesoDato;

namespace GrpcVentas.General
{
    public class clsPedido
    {
        /// <summary>
        /// Metodo del procesado de pedido
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponsePedido GuardarPedidoManualBulk(lsPedidoDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponsePedido objrespuesta = new DataResponsePedido();

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionPedido> lsconfiguracion = request.Lspconfiguracionpedido.ToList();
                List<protoPedido> lspedido = request.LspPedido.ToList();
                List<protoPedidoDetalle> lspedidodetalle = request.LspPedidoDetalle.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null) {
                        lspedido = clsGeneral.ActualizarIdFran(lspedido, objfranquicia.Idfran);
                        lspedidodetalle = clsGeneral.ActualizarIdFran(lspedidodetalle, objfranquicia.Idfran);

                        objrespuesta = clsPedidoDatos.InsertarPedidoManual(lspedido, lspedidodetalle, objCorporativo, objfranquicia, dtInicioProceso);
                    }   
                }
                else
                {
                    objrespuesta.MensajeError = "Clave no encontrada." + Sclave;
                    objrespuesta.EstatusCodigo = 304;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponsePedido>("Error 304 clsPedido-GuardarPedidoManualBulk " + ex);
                return objrespuesta;
            }
        }
    }
}
