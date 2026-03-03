using GrpcVentas.Modelo;

namespace GrpcVentas.AccesoDato
{
    public class clsRespuestaFactory
    {
        //Listado de correcto
        public static T CrearRespuestaExito<T>(string mensaje) where T : class, new()
        {
            var respuesta = new T();

            // Usamos "dynamic" para acceder a las propiedades directamente.
            dynamic respuestaDinamica = respuesta;
            respuestaDinamica.EstatusCodigo = StatusCodes.Status200OK;
            respuestaDinamica.MensajeRespuesta = mensaje;

            return respuesta;
        }

        //Listado de error
        public static T CrearRespuestaError<T>(string mensajeError) where T : class, new()
        {
            var respuesta = new T();

            // Usamos "dynamic" para acceder a las propiedades directamente.
            dynamic respuestaDinamica = respuesta;
            respuestaDinamica.EstatusCodigo = StatusCodes.Status500InternalServerError;
            respuestaDinamica.MensajeError = mensajeError;
            respuestaDinamica.MensajeRespuesta = "Ocurrió un error al procesar la solicitud.";

            return respuesta;
        }
    }
}
