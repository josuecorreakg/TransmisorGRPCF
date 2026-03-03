using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsOperacion
    {
        /// <summary>
        /// Metodo del procesado de operacion
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseOperacion GuardarOperacionManualBulk(lsOperacionDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseOperacion objrespuesta = new DataResponseOperacion();
            

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionoperacion> lsconfiguracion = request.LspOperacionConfiguracion.ToList();
                List<protoConsultaRespuesta> lsOperacionConsultaRespuesta = request.LspOperacionConsultaRespuesta.ToList();
                List<protoConfiguracionFarmaciaOperacion> lsConfigruacionFarmaciaOperacion = request.LspConfiguracionFarmaciaOperacion.ToList();
                List<protoOperacion> lsOperacion = request.LspOperacion.ToList();
                List<protoUsuario> lsUsuario = request.LspOperacionUsuario.ToList();
                List<protoCategoriaComercial> lsCategoriaComercial = request.LspOperacionCategoriaComercial.ToList();
                List<protoCategoriaComercialProducto> lsCategoriaComercialProducto = request.LspOperacionCategoriaComercialProducto.ToList();
                List<protoConsultaConsultorioTurnoDetalle> lsConsultaConsultorioTurnoDetalle = request.LspOperacionConsultaConsultorioTurnoDetalle.ToList();
                List<protoOperacionGlobal> lsOperacionGlobal = request.LspOperacionGlobal.ToList();
                List<protoTomaTemperatura> lsTemperatura = request.LspTemperatura.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (!string.IsNullOrEmpty(Sclave)) {
                        lsOperacionConsultaRespuesta = clsGeneral.ActualizarIdFran(lsOperacionConsultaRespuesta, objfranquicia.Idfran);
                        lsConfigruacionFarmaciaOperacion = clsGeneral.ActualizarIdFran(lsConfigruacionFarmaciaOperacion, objfranquicia.Idfran);
                        lsOperacion = clsGeneral.ActualizarIdFran(lsOperacion, objfranquicia.Idfran);
                        lsUsuario = clsGeneral.ActualizarIdFran(lsUsuario, objfranquicia.Idfran);
                        lsCategoriaComercial = clsGeneral.ActualizarIdFran(lsCategoriaComercial, objfranquicia.Idfran);
                        lsCategoriaComercialProducto = clsGeneral.ActualizarIdFran(lsCategoriaComercialProducto, objfranquicia.Idfran);
                        lsConsultaConsultorioTurnoDetalle = clsGeneral.ActualizarIdFran(lsConsultaConsultorioTurnoDetalle, objfranquicia.Idfran);
                        lsOperacionGlobal = clsGeneral.ActualizarIdFran(lsOperacionGlobal, objfranquicia.Idfran);
                        lsTemperatura = clsGeneral.ActualizarIdFran(lsTemperatura, objfranquicia.Idfran);

                        objrespuesta = clsOperacionDatos.InsertarOperacionManual(lsOperacionConsultaRespuesta, lsConfigruacionFarmaciaOperacion, lsOperacion, lsUsuario, lsCategoriaComercial, lsCategoriaComercialProducto, lsConsultaConsultorioTurnoDetalle, lsOperacionGlobal, lsTemperatura, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error 304 clsOperacion-GuardarOperacionManualBulk " + ex);
                return objrespuesta;
            }
        }

        /// <summary>
        /// Metodo para almacenar temperatura
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseOperacion GuardarTemperaturaManualBulk(lsTemperaturaRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseOperacion objrespuesta = new DataResponseOperacion();

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionoperacion> lsconfiguracion = request.LspOperacionConfiguracion.ToList();
                List<protoTomaTemperatura> lsTomaTemperatura = request.LsTomaTemperatura.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null) {
                        lsTomaTemperatura = clsGeneral.ActualizarIdFran(lsTomaTemperatura, objfranquicia.Idfran);

                        objrespuesta = clsOperacionDatos.InsertarTemperaturaManual(lsTomaTemperatura, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error 304 clsOperacion-GuardarTemperaturaManualBulk " + ex);
                return objrespuesta;
            }
        }

        /// <summary>
        /// Metodo para almacenar operacion
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseOperacion GuardarOperacionAuditoriaManualBulk(lsOperacionAuditoriaDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseOperacion objrespuesta = new DataResponseOperacion();

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionoperacion> lsconfiguracion = request.LspOperacionConfiguracion.ToList();
                List<protoConsultaRespuesta> lsOperacionConsultaRespuesta = request.LspOperacionConsultaRespuesta.ToList();
                List<protoConfiguracionFarmaciaOperacion> lsConfigruacionFarmaciaOperacion = request.LspConfiguracionFarmaciaOperacion.ToList();
                List<protoOperacion> lsOperacion = request.LspOperacion.ToList();
                List<protoUsuario> lsUsuario = request.LspOperacionUsuario.ToList();
                List<protoCategoriaComercial> lsCategoriaComercial = request.LspOperacionCategoriaComercial.ToList();
                List<protoCategoriaComercialProducto> lsCategoriaComercialProducto = request.LspOperacionCategoriaComercialProducto.ToList();
                List<protoConsultaConsultorioTurnoDetalle> lsConsultaConsultorioTurnoDetalle = request.LspOperacionConsultaConsultorioTurnoDetalle.ToList();
                List<protoOperacionGlobal> lsOperacionGlobal = request.LspOperacionGlobal.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null) {
                        lsOperacionConsultaRespuesta = clsGeneral.ActualizarIdFran(lsOperacionConsultaRespuesta, objfranquicia.Idfran);
                        lsConfigruacionFarmaciaOperacion = clsGeneral.ActualizarIdFran(lsConfigruacionFarmaciaOperacion, objfranquicia.Idfran);
                        lsOperacion = clsGeneral.ActualizarIdFran(lsOperacion, objfranquicia.Idfran);
                        lsUsuario = clsGeneral.ActualizarIdFran(lsUsuario, objfranquicia.Idfran);
                        lsCategoriaComercial = clsGeneral.ActualizarIdFran(lsCategoriaComercial, objfranquicia.Idfran);
                        lsCategoriaComercialProducto = clsGeneral.ActualizarIdFran(lsCategoriaComercialProducto, objfranquicia.Idfran);
                        lsConsultaConsultorioTurnoDetalle = clsGeneral.ActualizarIdFran(lsConsultaConsultorioTurnoDetalle, objfranquicia.Idfran);
                        lsOperacionGlobal = clsGeneral.ActualizarIdFran(lsOperacionGlobal, objfranquicia.Idfran);

                        objrespuesta = clsOperacionDatos.InsertarOperacionAuditoriaManual(lsOperacionConsultaRespuesta, lsConfigruacionFarmaciaOperacion, lsOperacion, lsUsuario, lsCategoriaComercial, lsCategoriaComercialProducto, lsConsultaConsultorioTurnoDetalle, lsOperacionGlobal, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseOperacion>("Error clsOperacion-GuardarOperacionAuditoriaManualBulk " + ex);
                return objrespuesta;
            }
        }


    }
}
