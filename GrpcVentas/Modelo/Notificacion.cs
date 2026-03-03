namespace GrpcVentas.Modelo
{
    public class Notificacion
    {
        public int TipoOperacion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public Notificacion() { } // constructor vacío para deserialización

        public Notificacion(int operacion, string fechaInicio, string fechaFin)
        {
            TipoOperacion = operacion;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }
    }
}
