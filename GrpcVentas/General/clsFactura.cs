using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsFactura
    {
        /// <summary>
        /// Metodo del procesado de las facturas
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseFacturas GuardarFacturasManualBulk(lsFacturaDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseFacturas objrespuesta = new DataResponseFacturas();

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionFacturas> lsconfiguracionfactura = request.LspFacturaConfiguracion.ToList();
                List<protoFacturacionFactura> lsfacturacionFactura = request.LspFacturacionfactura.ToList();
                List<protoFacturacionReferencium> lsFacturacionReferencia = request.LspFacturacionreferencium.ToList();

                ////Se actualiza el id de la sucursal
                string Sclave = lsconfiguracionfactura[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null){
                        lsfacturacionFactura = clsGeneral.ActualizarIdFran(lsfacturacionFactura, objfranquicia.Idfran);
                        lsFacturacionReferencia = clsGeneral.ActualizarIdFran(lsFacturacionReferencia, objfranquicia.Idfran);

                        objrespuesta = clsFacturasDatos.InsertarFacturasManual(lsfacturacionFactura, lsFacturacionReferencia, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseFacturas>("Error clsFactura-GuardarFacturasBulk " + ex);
                return objrespuesta;
            }
        }
    }
}
