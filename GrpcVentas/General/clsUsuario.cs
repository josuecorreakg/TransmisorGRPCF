using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsUsuario
    {
        /// <summary>
        /// Metodo de productos
        /// </summary>
        /// <param>Recibe los listados proto</param>
        /// <param name="objCorporativo">Datos de conexion</param>
        /// <returns>
        /// </returns>
        public static DataResponseUsuario GuardarUsuarioManualBulk(lsUsuarioDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseUsuario objrespuesta = new DataResponseUsuario();
            

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionUsuario> lsconfiguracion = request.LspUsuarioConfiguracion.ToList();
                List<protoSeguridadCategoria> lsSeguridadCategoria = request.LspUsuarioSeguridadCategoria.ToList();
                List<protoSeguridadRol> lsSeguridadRol = request.LspUsuarioSeguridadRol.ToList();
                List<protoSeguridadTarea> lsSeguridadTarea = request.LspUsuarioSeguridadTarea.ToList();
                List<protoUsuarioRol> lsUsuarioRol = request.LspUsuarioRol.ToList();
                List<protoUsuarioPorRol> lsUsuarioPorRol = request.LspUsuarioPorRol.ToList();

                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        lsSeguridadCategoria = clsGeneral.ActualizarIdFran(lsSeguridadCategoria, objfranquicia.Idfran);
                        lsSeguridadRol = clsGeneral.ActualizarIdFran(lsSeguridadRol, objfranquicia.Idfran);
                        lsSeguridadTarea = clsGeneral.ActualizarIdFran(lsSeguridadTarea, objfranquicia.Idfran);
                        lsUsuarioRol = clsGeneral.ActualizarIdFran(lsUsuarioRol, objfranquicia.Idfran);
                        lsUsuarioPorRol = clsGeneral.ActualizarIdFran(lsUsuarioPorRol, objfranquicia.Idfran);

                        objrespuesta = clsUsuarioDatos.InsertarUsuariosManual(lsSeguridadCategoria, lsSeguridadRol, lsSeguridadTarea, lsUsuarioRol, lsUsuarioPorRol, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseUsuario>("Error 304 clsUsuario-GuardarUsuarioManualBulk " + ex);
                return objrespuesta;
            }
        }
    }
}
