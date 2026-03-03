using GrpcVentas.General;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using MySqlConnector;
using Newtonsoft.Json;

namespace GrpcVentas.AccesoDato
{
    public class clsConfiguracionDatos
    {

        public static bool SetVersionSyncro2(DatosCorporativo objCorporativo, Franquicia objfranquicia, double nuevaVersion)
        {
            bool bResultado = false;

            try
            {
                // 1. Usar el contexto de la base de datos
                using (var context = new VentasContext(objCorporativo))
                {
                    // 2. Localizar la entidad Franquicia que se desea actualizar.
                    // Es crucial adjuntar o encontrar la entidad existente para poder modificarla.
                    // Asumimos que la llave primaria es Idfran o una combinación de Empresa y Clave.
                    // Usaremos .Find() si es por PK, o .FirstOrDefault() si es por otros campos.

                    // Opción 1: Usando .Find() si Idfran es la Primary Key y está poblada en objfranquicia
                    var franquiciaAActualizar = context.Franquicias.Find(objfranquicia.Idfran);

                    // Si .Find() retorna null, intenta buscar por otros campos clave si es necesario.
                    if (franquiciaAActualizar == null)
                    {
                        // Opción 2 (alternativa): Buscar por Clave y Empresa
                        franquiciaAActualizar = context.Franquicias
                            .FirstOrDefault(f => f.Empresa == objfranquicia.Empresa && f.Clave == objfranquicia.Clave);
                    }

                    if (franquiciaAActualizar != null)
                    {
                        // 3. Modificar la propiedad deseada
                        franquiciaAActualizar.VersionSyncro2 = nuevaVersion;


                        // 4. Guardar los cambios en la base de datos
                        int filasAfectadas = context.SaveChanges();

                        // 5. Verificar si se actualizó al menos una fila
                        if (filasAfectadas > 0)
                        {
                            bResultado = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loggear la excepción 'ex' para saber qué falló.
                bResultado = false;
            }

            return bResultado;
        }

        public static List<DeskCatalogoOperacion> GetCatalogoOperacion(DatosCorporativo objCorporativo, Franquicia objfranquicia)
        {
            List<DeskCatalogoOperacion> objRespuesta = new List<DeskCatalogoOperacion>();

            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    objRespuesta = (from lst in context.DeskCatalogoOperacion
                                    select lst).ToList();

                }
                return objRespuesta;
            }
            catch (Exception)
            {

            }

            return objRespuesta;
        }

        public static double? GetVersionLiberada(DatosCorporativo objCorporativo, Franquicia objfranquicia, string nombreSistema)
        {
            double objRespuesta = 0.0;

            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    int idFran = objfranquicia.Idfran;

                    Catsistemas obCatalogo = (from lst in context.CatSistemas
                                              where lst.NombreSistema.Equals(nombreSistema)
                                              select lst).FirstOrDefault();

                    objRespuesta = (from lstp in context.ControlVersiones
                                              where lstp.IdSistema.Equals(obCatalogo.IdSistema)
                                              && lstp.IdFran.Equals(idFran)
                                              select lstp.Version).FirstOrDefault();

                    return objRespuesta;
                }
            }
            catch (Exception ex)
            {
                return objRespuesta;
            }
        }

        public static List<TvTransmision> GetTvtransmision(DatosCorporativo objCorporativo, Franquicia objfranquicia)
        {
            List<TvTransmision> lstTvtransmision = new List<TvTransmision>();

            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    int idFran = objfranquicia.Idfran;

                    List<TvTransmision> objTvtransmision = (from lst in context.Tvtransmision
                                              where lst.IdFran.Equals(objfranquicia.Idfran)
                                              where lst.Status.Equals(0)
                                              select lst).ToList();

                    if (objTvtransmision.Count > 0)
                    {
                        foreach (var item in objTvtransmision)
                        {
                            item.Status = 1;
                        }
                        context.SaveChanges();
                    }

                    return objTvtransmision;
                }
            }
            catch (Exception ex)
            {
                return lstTvtransmision;
            }
        }


    }
}
