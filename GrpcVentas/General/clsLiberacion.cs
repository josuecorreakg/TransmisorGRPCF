using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsLiberacion
    {
        public static protoRespuestaLiberaciones ObtenerLiberacionesPendientes(protodataConfiguracion request, DatosCorporativo objCorporativo)
        {
            protoRespuestaLiberaciones objrespuesta = new protoRespuestaLiberaciones();

            try
            {
                string sClave = request.Clave;

                if (!string.IsNullOrEmpty(sClave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(sClave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        List<TvLiberaciones> lstLiberaciones = clsLiberacionDatos.GetLiberacionesPendientes(objCorporativo, objfranquicia);

                        foreach (var item in lstLiberaciones)
                        {
                            objrespuesta.LsLiberaciones.Add(new protoLiberacionEjecutable
                            {
                                Idfran = item.IdFran,
                                Clave = item.Clave,
                                NombreSistema = item.NombreSistema ?? "",
                                RutaEvaluacion = item.RutaEvaluacion ?? "",
                                FechaLiberacion = item.FechaLiberacion?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                                FechaInstalacion = item.FechaInstalacion?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                                Estatus = (EstatusLiberacion)(item.Estatus ?? 0)
                            });
                        }

                        objrespuesta.EstatusCodigo = 200;
                    }
                    else
                    {
                        objrespuesta.MensajeError = "Cliente no encontrado." + sClave;
                        objrespuesta.EstatusCodigo = 104;
                    }
                }
                else
                {
                    objrespuesta.MensajeError = "Clave no encontrada." + sClave;
                    objrespuesta.EstatusCodigo = 304;
                }

                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta.MensajeError = "Error clsLiberacion-ObtenerLiberacionesPendientes " + ex;
                objrespuesta.EstatusCodigo = 500;
                return objrespuesta;
            }
        }

        public static DataResponseConfiguracion ActualizarEstatusLiberacion(protoLiberacionEjecutable request, DatosCorporativo objCorporativo)
        {
            DataResponseConfiguracion objrespuesta = new DataResponseConfiguracion();

            try
            {
                string sClave = request.Clave;

                if (!string.IsNullOrEmpty(sClave) && !string.IsNullOrEmpty(request.NombreSistema))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(sClave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        bool bResultado = clsLiberacionDatos.ActualizarEstatusLiberacion(objCorporativo, objfranquicia, request.NombreSistema, (int)request.Estatus);

                        objrespuesta.EstatusCodigo = bResultado ? 200 : 404;
                        objrespuesta.MensajeRespuesta = bResultado ? "Estatus actualizado correctamente." : "No se encontro el registro de liberacion.";
                    }
                    else
                    {
                        objrespuesta.MensajeError = "Cliente no encontrado." + sClave;
                        objrespuesta.EstatusCodigo = 104;
                    }
                }
                else
                {
                    objrespuesta.MensajeError = "Clave no encontrada." + sClave;
                    objrespuesta.EstatusCodigo = 304;
                }

                return objrespuesta;
            }
            catch (Exception ex)
            {
                objrespuesta.MensajeError = "Error clsLiberacion-ActualizarEstatusLiberacion " + ex;
                objrespuesta.EstatusCodigo = 500;
                return objrespuesta;
            }
        }
    }
}
