using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using MySqlConnector;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using Dapper;
using GrpcVentas.Notificaciones;
using static GrpcVentas.AccesoDato.clsGeneralDatos;

namespace GrpcVentas.AccesoDato
{
    public class clsGeneralDatos
    {

        public static ResultadoConsulta<DatosCorporativo> GetDatosCorporativo()
        {
            try
            {
                using (var context = new CorporativoContext())
                {
                    var datos = context.DatosCorporativos.ToList();

                    return new ResultadoConsulta<DatosCorporativo>
                    {
                        Datos = datos,
                        Error = "ok"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultadoConsulta<DatosCorporativo>
                {
                    Datos = [],
                    Error = ex.ToString()
                };
            }
        }


        public class ResultadoConsulta<T>
        {
            public List<T> Datos { get; set; } = [];
            public string? Error { get; set; }
        }


        public static DatosCorporativo GetDatosCnn(string sNombreCorto)
        {
            try
            {
                DatosCorporativo objCorporativo = new DatosCorporativo();
                using (var context = new CorporativoContext())
                {
                    objCorporativo = (from corporativo in context.DatosCorporativos
                                      where corporativo.NombreCorto.Equals(sNombreCorto)
                                      select corporativo).FirstOrDefault();
                }
                return objCorporativo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Franquicia GetFranquicia(string sClave, DatosCorporativo objCorporativo)
        {
            try
            {
                Franquicia Fran = new Franquicia();

                using (var context = new VentasContext(objCorporativo))
                {
                    Fran = (from fran in context.Franquicias
                            where fran.Activa == true && fran.Clave.Equals(sClave)
                            select fran).FirstOrDefault();
                }
                return Fran;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetFranquiciaPrueba(string sClave, DatosCorporativo objCorporativo)
        {
            try
            {
                Franquicia Fran = new Franquicia();

                using (var context = new VentasContext(objCorporativo))
                {
                    Fran = (from fran in context.Franquicias
                            where fran.Activa == true && fran.Clave.Equals(sClave)
                            select fran).FirstOrDefault();
                }
                return "ok";
            }
            catch (Exception ex)
            {
                string response = ex.ToString();
                return response;
            }
        }

        public static bool EjecutaString(string sQuery, DatosCorporativo cn)
        {
            try
            {

                using (var db = new MySqlConnection("server = " + cn.Hst + "; user = " + cn.UsrSyncro2 + "; password = " + cn.PssSyncro2 + "; database = " + cn.DbSyncro2))
                {
                    var Result = db.Execute(sQuery);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        static public DataTable getDataTable(string cadena, string nombre)
        {

            MySqlConnection cn = new MySqlConnection(Conexion.GetConnectionString());
            try
            {
                cn.Open();
                DataTable datos = new DataTable(nombre);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cadena, cn);
                adapter.Fill(datos);
                return datos;
            }
            catch
            {
                return null;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
                MySqlConnection.ClearPool(cn);
                MySqlConnection.ClearAllPools();
            }

        }


        public static int IntQuery(string sQuery, DatosCorporativo cn)
        {
            try
            {
                int ire = 0;

                using (var db = new MySqlConnection("server = " + cn.Hst + "; user = " + cn.UsrSyncro2 + "; password = " + cn.PssSyncro2 + "; database = " + cn.DbSyncro2))
                {

                    /*
                         var orderDetails = connection.Query<OrderDetail>(sql).ToList();*/
                    var iref = db.Query(sQuery).FirstOrDefault();

                    foreach (var ind in iref)
                    {
                        ire = ind.Value;
                    }
                    // ire = iref;
                }

                return ire;
            }
            catch (Exception ex)
            {

                return 0;
            }

        }


    }
}
