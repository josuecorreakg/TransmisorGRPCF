namespace GrpcVentas.Notificaciones
{

    public class Conexion
    {
        public static string Server;
        public static string Database;
        public static string Uid;
        public static string Password;

        static Conexion()
        {
            Server = "localhost";
            Database = "test_erp";
            Uid = "root";
            Password = "hola";

            //Server = "ironman.mysql.database.azure.com";
            //Database = "corporativo_siif";
            //Uid = "kgadmin";
            //Password = "SaM345?84!d2x5";
        }
        public static string GetConnectionString()
        {
            return $"SERVER={Server}; DATABASE={Database}; UID={Uid}; PASSWORD={Password}; Pooling=true; Min Pool Size=0;";
        }


    }
}
