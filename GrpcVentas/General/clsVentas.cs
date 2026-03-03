using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo;
//using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Configuration;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsVentas
    {
        /// <summary>
        /// Metodo del procesado de las ventas
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponse GuardarVentasManualBulk(VentasDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponse objrespuesta = new DataResponse();

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracion> lsconfiguracion = request.Lspconfiguracionfranquicia.ToList();
                List<protoVentaDescuento> lsventaDescuento = request.Lspventadescuento.ToList();
                List<protoVentaProducto> lsventaProdu = request.Lspventaproducto.ToList();
                List<protoVentaPago> lsventaPagos = request.Lspventapago.ToList();
                List<protoVentum> lsventa = request.Lspventas.ToList();
                List<protoVentafrandium> lsventafrandia = request.Lspventafrandia.ToList();
                List<protoVentaProductoDesglose> lsventaproductoDesgloce = request.LspVentaProductoDesglose.ToList();
                List<protoVentaRecetaControlado> lsventaRecetaControlado = request.LspVentaRecetaControlado.ToList();

                lsventafrandia = clsGeneral.FormatearMesYActualizarFecha(lsventafrandia);

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        lsventaDescuento = clsGeneral.ActualizarIdFran(lsventaDescuento, objfranquicia.Idfran);
                        lsventaProdu = clsGeneral.ActualizarIdFran(lsventaProdu, objfranquicia.Idfran);
                        lsventaPagos = clsGeneral.ActualizarIdFran(lsventaPagos, objfranquicia.Idfran);
                        lsventa = clsGeneral.ActualizarIdFran(lsventa, objfranquicia.Idfran);
                        lsventafrandia = clsGeneral.ActualizarIdFran(lsventafrandia, objfranquicia.Idfran);
                        lsventaproductoDesgloce = clsGeneral.ActualizarIdFran(lsventaproductoDesgloce, objfranquicia.Idfran);
                        lsventaRecetaControlado = clsGeneral.ActualizarIdFran(lsventaRecetaControlado, objfranquicia.Idfran);

                        objrespuesta = clsVentasDatos.InsertarVentasManual(lsventaDescuento, lsventaProdu, lsventaPagos, lsventa, lsventafrandia, lsventaproductoDesgloce, lsventaRecetaControlado, objCorporativo, objfranquicia, dtInicioProceso, objfranquicia.Idfran);
                    }
                    else
                    {
                        objrespuesta.MensajeRespuesta = "Error 304, No se encontro información de la sucursal, clsVentas - GuardarVentasManualBulk";
                        objrespuesta.MensajeError = "Sucursal" + Sclave + " - " + objfranquicia;
                        objrespuesta.EstatusCodigo = 304;
                    }
                }
                else {
                    objrespuesta.MensajeError = "Clave no encontrada." + Sclave;
                    objrespuesta.EstatusCodigo = 304;
                }
                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error clsVentas-GuardarVentasBulk " + ex);
                return objrespuesta;
            }
        }

        /// <summary>
        /// Metodo del procesado de historicos
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponse GuardarHistoricosManualBulk(HistoricosDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponse objrespuesta = new DataResponse();
            
            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracion> lsconfiguracion = request.Lspconfiguracion.ToList();
                List<protoVentafran> lsventaFran = request.Lspventafran.ToList();
                List<protoVentafrandium> lsventaFranDia = request.Lspventafrandia.ToList();

                
                lsventaFran = clsGeneral.ActualizarProdPremio(lsventaFran);
                lsventaFranDia = clsGeneral.FormatearMesYActualizarFecha(lsventaFranDia);

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null){
                        lsventaFran = clsGeneral.ActualizarIdFran(lsventaFran, objfranquicia.Idfran);
                        lsventaFranDia = clsGeneral.ActualizarIdFran(lsventaFranDia, objfranquicia.Idfran);

                        objrespuesta = clsVentasDatos.InsertarHistoricosManual(lsventaFran, lsventaFranDia, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error 304 clsVentas-GuardarHistoricosManualBulk " + ex);
                return objrespuesta;
            }
        }

        /// <summary>
        /// Metodo del procesado de las ventas
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponse GuardarCostoOpBulk(CostoOpDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponse objrespuesta = new DataResponse();

            try
            {
                //Se acomodan las listas recibidas
                List<protoCostoOp> lsCostoOp = request.LspCostoOp.ToList();

                //Se actualiza el Id de la farmacia con la clave
                string Sclave = lsCostoOp[0].Clave.ToString();
                Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                if (objfranquicia != null)
                {
                    lsCostoOp = clsGeneral.ActualizarIdFranCosto(lsCostoOp, objfranquicia.Idfran);

                    objrespuesta = clsVentasDatos.InsertarCostoOp(lsCostoOp, objCorporativo, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponse>("Error 304 clsVentas-GuardarCostoOpBulk " + ex);
                return objrespuesta;
            }
        }



    }
}
