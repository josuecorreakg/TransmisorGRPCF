using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsProducto
    {
        /// <summary>
        /// Metodo de productos
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseProducto GuardarProductoManualBulk(lsProductoDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseProducto objrespuesta = new DataResponseProducto();
            DateTime fechaNow = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionProducto> lsconfiguracion = request.LspConfiguracionProducto.ToList();
                List<protoProductoFranquicia> lsproductoFranquicia = request.LspProductoFranquicia.ToList();
                List<protoProductoSuplementoFranquicia> lsProductoSuplementoFranquicia = request.LspProductoSuplementoFranquicia.ToList();
                List<protoProductoPorListaPrecio> lsProductoPorListaPrecio = request.LspProductoListaPrecio.ToList();
                List<protoProductoClasificacion> lsProductoClasificacion = request.LspProductoClasificacion.ToList();

                ////Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null){
                        lsproductoFranquicia = clsGeneral.ActualizarIdFranProducto(lsproductoFranquicia, objfranquicia.Idfran);
                        lsProductoSuplementoFranquicia = clsGeneral.ActualizarIdFranSuplemento(lsProductoSuplementoFranquicia, objfranquicia.Idfran);
                        lsProductoPorListaPrecio = clsGeneral.ActualizarIdFranProducto(lsProductoPorListaPrecio, objfranquicia.Idfran);

                        lsProductoSuplementoFranquicia = clsGeneral.ActualizarFecha(lsProductoSuplementoFranquicia, fechaNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        lsProductoPorListaPrecio = clsGeneral.ActualizarFecha(lsProductoPorListaPrecio, fechaNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        lsproductoFranquicia = clsGeneral.ActualizarFecha(lsproductoFranquicia, fechaNow.ToString("yyyy-MM-dd HH:mm:ss"));

                        objrespuesta = clsProductoDatos.InsertarProductoManual(lsproductoFranquicia, lsProductoSuplementoFranquicia, lsProductoPorListaPrecio, lsProductoClasificacion, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseProducto>("Error 304 clsProducto-GuardarProductoManualBulk " + ex);
                return objrespuesta;
            }
        }
    }
}
