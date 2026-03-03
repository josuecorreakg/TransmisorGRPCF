using Google.Protobuf;
using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;

namespace GrpcVentas.General
{
    public class clsConfiguracion
    {
        public static DataResponseConfiguracion GuardarYObtenerConfiguracion(protodataConfiguracion request, DatosCorporativo objCorporativo, DateTime dtInicioProceso)
        {
            DataResponseConfiguracion objrespuesta = new DataResponseConfiguracion();

            try
            {
                string Sclave = request.Clave;

                if (!string.IsNullOrEmpty(Sclave))
                {
                    Franquicia objfranquicia = clsGeneralDatos.GetFranquicia(Sclave, objCorporativo);
                    if (objfranquicia != null)
                    {
                        //Actualizar la versión
                        bool bresponse = clsConfiguracionDatos.SetVersionSyncro2(objCorporativo, objfranquicia, Convert.ToDouble(request.Version));

                        //Obtener los datos de los catalogos
                        List<DeskCatalogoOperacion> lstDeskOperacion = clsConfiguracionDatos.GetCatalogoOperacion(objCorporativo, objfranquicia);

                        //Obtener la versión que deberia de tener la sucursal
                        var versionActual = clsConfiguracionDatos.GetVersionLiberada(objCorporativo, objfranquicia, "AppVenta");
                        // --- Serialización y Compresión ---

                        //Remplazar el anterior por la tabla de transmisión
                        List<TvTransmision> lstTvtransmision = clsConfiguracionDatos.GetTvtransmision(objCorporativo, objfranquicia);


                        // 2. Mapear List<DeskCatalogoOperacion> a protoListaOperaciones
                        var protoLista = new protoListaOperaciones();
                        protoLista.Version = versionActual ?? 0.0;

                        foreach (var item in lstDeskOperacion)
                        {
                            protoLista.LsprotoOperaciones.Add(new protoDeskCatalogoOperacion
                            {
                                IdOperacion = item.idOperacion,
                                Nombre = item.Nombre,
                                Estatus = (int)item.Estatus,
                                DiasAuditar = (int)item.DiasAuditar,
                                Frecuencia = (int)item.Frecuencia,
                                HoraInicio = item.HoraInicio?.ToString(),
                                HoraFin = item.HoraFin?.ToString(),
                                NumeroTransmision = (int)item.NumeroTransmision
                            });
                        }

                        
                        foreach (var item in lstTvtransmision)
                        {
                            protoLista.Lsprototransmision.Add(new protoTvTransmision
                            {
                                Idoperacion = item.IdOperacion,
                                Fechainicio = item.FechaInicio.ToString(),
                                Fechafin = item.FechaFin.ToString()
                            });
                        }


                        // 3. Serializar el objeto Protobuf a un array de bytes binario
                        byte[] protoBytes = protoLista.ToByteArray();

                        // 4. Comprimir los bytes con GZip
                        byte[] compressedData = clsGeneral.CompressData(protoBytes);

                        // 5. Crear y poblar el objeto DataResponseConfiguracion
                        objrespuesta = new DataResponseConfiguracion
                        {
                            MensajeRespuesta = "Datos serializados y comprimidos correctamente.",
                            EstatusCodigo = 200,
                            MensajeError = "",
                            Compressdata = ByteString.CopyFrom(compressedData)
                        };

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
                //objrespuesta = clsRespuestaFactory.CrearRespuestaError<DataResponseFacturas>("Error clsFactura-GuardarFacturasBulk " + ex);
                return objrespuesta;
            }
        }


    }
}
