using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsKardex
    {
        /// <summary>
        /// Metodo del procesado de kardex
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <returns>
        /// </returns>
        public static DataResponseKardex GuardarKardexManualBulk(KardexDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseKardex objrespuesta = new DataResponseKardex();

            try
            {
                ////Se acomodan las listas recibidas
                List<protoconfiguracionKardex> lsconfiguracion = request.LspconfiguracionKardex.ToList();
                List<protoErpCiKardex> lsCiKardex = request.LspCiKardex.ToList();
                List<protoErpCiKardexCompras> lsCiKardexCompras = request.LspCiKardexCompras.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null){
                        lsCiKardex = clsGeneral.ActualizarIdFranKardex(lsCiKardex, objfranquicia.Idfran);
                        lsCiKardexCompras = clsGeneral.ActualizarIdFranProducto(lsCiKardexCompras, objfranquicia.Idfran);

                        objrespuesta = clsKardexDatos.InsertarKardexManual(lsCiKardex, lsCiKardexCompras, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Error 304 clskardex-GuardarKardexManualBulk " + ex);
                return objrespuesta;
            }
        }

        public static DataResponseKardex GuardarKardexControlMensual(KardexPrecioPromedioDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseKardex objrespuesta = new DataResponseKardex();

            try
            {
                ////Se acomodan las listas recibidas
                List<protoKardexControlMensualEntry> lsKardexControlMensual = request.LspKardexControlMensual.ToList();


                //Se actualiza el id de la sucursal
                string Sclave = lsKardexControlMensual[0].Clave.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        lsKardexControlMensual = clsGeneral.ActualizarIdFranKardex(lsKardexControlMensual, objfranquicia.Idfran);

                        objrespuesta = clsKardexDatos.InsertarControlMensualManual(lsKardexControlMensual, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseKardex>("Error 304 clskardex-GuardarKardexManualBulk " + ex);
                return objrespuesta;
            }
        }



    }
}
