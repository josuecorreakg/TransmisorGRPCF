using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using GrpcVentas.AccesoDato;

namespace GrpcVentas.General
{
    public class clsInventario
    {
        public static DataResponseInventario GuardarInventarioManualBulk(InventarioDataRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseInventario objrespuesta = new DataResponseInventario();
            DateTime fechaNow = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionInventario> lsconfiguracion = request.LspconfiguracionInventario.ToList();
                List<protoInventarioFisicoCompleto> lsinventariofisicocompleto = request.Lspinventariofisicocompleto.ToList();
                List<protoInventarioOtro> lsinventariootros = request.Lspinventariootros.ToList();
                List<protoInventarioOtrosDetalle> lsinventariootrosdetalle = request.Lspinventariootrosdetalle.ToList();
                List<protoInventarioSurtido> lsinventariosurtido = request.Lspinventariosurtido.ToList();
                List<protoInventarioSurtidoDetalle> lsinventariosurtidodetalle = request.LspinventariosurtidoDetalle.ToList();
                List<protoInventarioSurtidoFranquicium> lsinventariosurtidofranquicia = clsGeneral.ConvertirAProtoInventarioSurtidoFranquicium(lsinventariosurtido);
                List<protoInventarioTraspaso> lsinventariotraspaso = request.Lspinventariotraspaso.ToList();
                List<protoInventarioTraspasoDetalle> lsinventariotraspasodetalle = request.Lspinventariotraspasodetalle.ToList();
                List<protoInventario> lsinventario = request.Lspinventario.ToList();


                //Se actualiza el id de la sucursal
                string Sclave = lsconfiguracion[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    lsinventariofisicocompleto = clsGeneral.ActualizarIdFran(lsinventariofisicocompleto, objfranquicia.Idfran);
                    lsinventariootros = clsGeneral.ActualizarIdFran(lsinventariootros, objfranquicia.Idfran);
                    lsinventariootrosdetalle = clsGeneral.ActualizarIdFran(lsinventariootrosdetalle, objfranquicia.Idfran);
                    lsinventariosurtido = clsGeneral.ActualizarIdFran(lsinventariosurtido, objfranquicia.Idfran);
                    lsinventariosurtidodetalle = clsGeneral.ActualizarIdFran(lsinventariosurtidodetalle, objfranquicia.Idfran);
                    lsinventariosurtidofranquicia = clsGeneral.ActualizarIdFran(lsinventariosurtidofranquicia, objfranquicia.Idfran);
                    lsinventariotraspaso = clsGeneral.ActualizarIdFran(lsinventariotraspaso, objfranquicia.Idfran);
                    lsinventariotraspasodetalle = clsGeneral.ActualizarIdFran(lsinventariotraspasodetalle, objfranquicia.Idfran);
                    lsinventario = clsGeneral.ActualizarIdFran(lsinventario, objfranquicia.Idfran);

                    //Actualizar el IdFranPedido
                    lsinventariotraspaso = clsGeneral.ActualizarIdFranPedido(lsinventariotraspaso, objCorporativo);

                    lsinventario = clsGeneral.ActualizarFecha(lsinventario, fechaNow.ToString("yyyy-MM-dd HH:mm:ss"));

                    objrespuesta = clsInventariosDatos.InsertarInventarioManual(lsinventariofisicocompleto, lsinventariootros, lsinventariootrosdetalle, lsinventariosurtido, lsinventariosurtidodetalle, lsinventariosurtidofranquicia, lsinventariotraspaso, lsinventariotraspasodetalle, lsinventario, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Error 304 clsInventario-GuardarInventarioManualBulk " + ex);
                return objrespuesta;
            }
        }


        public static DataResponseInventario GuardarInventarioTiempoRealBulk(InventarioTiempoRealRequest request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseInventario objrespuesta = new DataResponseInventario();
            DateTime fechaNow = clsGeneral.ConvertirAZonaHoraria(DateTime.Now);

            try
            {
                //Se acomodan las listas recibidas
                List<protoconfiguracionInventario> lsconfiguracionInventario = request.LspconfiguracionInventariotiemporeal.ToList();
                List<protoInventarioFisicoCompleto> lsInventarioFisicoCompleto = request.Lspinventariofisicocompletotiemporeal.ToList();
                List<protoInventario> lsInventario = request.Lspinventariotiemporeal.ToList();

                ////Se actualiza el id de la sucursal
                string Sclave = lsconfiguracionInventario[0].IdFran.ToString();
                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    lsInventarioFisicoCompleto = clsGeneral.ActualizarIdFran(lsInventarioFisicoCompleto, objfranquicia.Idfran);
                    lsInventario = clsGeneral.ActualizarIdFran(lsInventario, objfranquicia.Idfran);

                    lsInventario = clsGeneral.ActualizarFecha(lsInventario, fechaNow.ToString("yyyy-MM-dd HH:mm:ss"));

                    objrespuesta = clsInventariosDatos.InsertarInventarioTiempoRealManual(lsInventarioFisicoCompleto, lsInventario, objCorporativo, objfranquicia, dtInicioProceso);
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
                objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseInventario>("Error 304 clsInventario-GuardarInventarioTiempoRealBulk " + ex);
                return objrespuesta;
            }
        }


    }
}
