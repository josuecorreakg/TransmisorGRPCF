using MySqlConnector;
using System.Data;
using System.Text;
using GrpcVentas.Modelo;
using Azure;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrpcVentas.Notificaciones
{
    public class ConnectionData
    {
        public static List<ConnectionClient> ConnectionList()
        {
            List<ConnectionClient> list = new List<ConnectionClient>();
            try
            {

                string sql = "SELECT ConnectionId, Cliente, Clave, Nombre, FechaConnection FROM connectionClient;";
                DataTable dtConection = clsGeneralDatos.getDataTable(sql, "Client");

                list = (from row in dtConection.AsEnumerable()
                        select new ConnectionClient()
                        {
                            ConnectionId = row["ConnectionId"].ToString(),
                            Cliente = row["Cliente"].ToString(),
                            Clave = row["Clave"].ToString(),
                            Nombre = row["Nombre"].ToString(),
                            FechaConnection = DateTime.Now,
                        }).ToList();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public static bool JoinConnection(string idconnectClient, string cliente, string clave, string nombre, int Operacion)
        {
            string nombrecliente = "";

            try
            {
                //Cliente
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(cliente);
                if (objCorporativo != null)
                {
                    using (var context = new VentasContext(objCorporativo))
                    {
                        string selectQuery = "SELECT Nombre FROM connectionClient WHERE cliente = @cliente AND Clave = @clave";

                        using (var command = context.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = selectQuery;
                            command.Parameters.Add(new MySqlConnector.MySqlParameter("@cliente", cliente));
                            command.Parameters.Add(new MySqlConnector.MySqlParameter("@clave", clave));

                            context.Database.OpenConnection();
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read() && !reader.IsDBNull(0))
                                {
                                    nombrecliente = reader.GetString(0);
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(nombrecliente))
                        {
                            string insertQuery = @"INSERT INTO connectionClient 
                                                   (Cliente, Clave, Nombre, ConnectionId, Id_Operacion, FechaConnection) 
                                                   VALUES (@cliente, @clave, @nombre, @idconnectClient, @Operacion, @fecha)";

                            using (var command = context.Database.GetDbConnection().CreateCommand())
                            {
                                command.CommandText = insertQuery;
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@cliente", cliente));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@clave", clave));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@nombre", nombre ?? "Desconocido"));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@idconnectClient", idconnectClient));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@Operacion", Operacion));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string updateQuery = "UPDATE connectionClient SET ConnectionId = @idconnectClient, FechaConnection='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"' WHERE Cliente = @cliente AND Clave = @clave";

                            using (var command = context.Database.GetDbConnection().CreateCommand())
                            {
                                command.CommandText = updateQuery;
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@idconnectClient", idconnectClient));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@cliente", cliente));
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@clave", clave));

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool ExitConnection(string idconnectClient, string nombrecliente)
        {
            string clave = "";
            try
            {
                DatosCorporativo objCorporativo = clsGeneralDatos.GetDatosCnn(nombrecliente);
                if (objCorporativo != null)
                {
                    using (var context = new VentasContext(objCorporativo))
                    {
                        string selectQuery = "SELECT Clave FROM connectionclient WHERE ConnectionId = @idconnectClient";

                        using (var command = context.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = selectQuery;
                            command.Parameters.Add(new MySqlConnector.MySqlParameter("@idconnectClient", idconnectClient));

                            context.Database.OpenConnection();
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read() && !reader.IsDBNull(0))
                                {
                                    clave = reader.GetString(0);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(clave)) {
                            string updateQuery = "UPDATE connectionclient SET ConnectionId = '0'  WHERE Clave = @clave";

                            using (var command = context.Database.GetDbConnection().CreateCommand())
                            {
                                command.CommandText = updateQuery;
                                command.Parameters.Add(new MySqlConnector.MySqlParameter("@clave", clave));
                                command.ExecuteNonQuery();
                            }
                        }

                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
