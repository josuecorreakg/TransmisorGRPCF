namespace GrpcVentas.Modelo
{
    public class NotificacionSignal
    {
        public NotificacionSignal(int Operacion, string FchInicio, string FchFin)
        {
            TipoOperacion = Operacion;
            FechaInicio = FchInicio;
            FechaFin = FchFin;
        }


        public int TipoOperacion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}
