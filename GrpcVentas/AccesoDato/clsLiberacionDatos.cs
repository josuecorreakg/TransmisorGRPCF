using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.AccesoDato
{
    public class clsLiberacionDatos
    {
        //No se atrapa la excepcion aqui a proposito: si la consulta falla (BD caida, tabla
        //bloqueada, etc.) debe propagarse para que el llamador (clsLiberacion) responda un
        //error real (500) en vez de "200 OK, 0 pendientes", que seria indistinguible de que
        //efectivamente no hay nada que instalar.
        public static List<TvLiberaciones> GetLiberacionesPendientes(DatosCorporativo objCorporativo, Franquicia objfranquicia)
        {
            using (var context = new VentasContext(objCorporativo))
            {
                return (from lst in context.TvLiberaciones
                        where lst.IdFran == objfranquicia.Idfran
                        && lst.Clave == objfranquicia.Clave
                        && lst.Estatus == 0
                        select lst).ToList();
            }
        }

        public static bool ActualizarEstatusLiberacion(DatosCorporativo objCorporativo, Franquicia objfranquicia, string nombreSistema, int nuevoEstatus)
        {
            bool bResultado = false;

            try
            {
                using (var context = new VentasContext(objCorporativo))
                {
                    var registro = context.TvLiberaciones.FirstOrDefault(l =>
                        l.IdFran == objfranquicia.Idfran
                        && l.Clave == objfranquicia.Clave
                        && l.NombreSistema == nombreSistema);

                    if (registro != null)
                    {
                        registro.Estatus = (sbyte)nuevoEstatus;

                        if (nuevoEstatus == 1) //Instalado
                        {
                            registro.FechaInstalacion = DateTime.Now;
                        }

                        int filasAfectadas = context.SaveChanges();
                        bResultado = filasAfectadas > 0;
                    }
                }
            }
            catch (Exception)
            {
                bResultado = false;
            }

            return bResultado;
        }
    }
}
