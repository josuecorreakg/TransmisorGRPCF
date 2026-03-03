using GrpcVentas.AccesoDato;
using GrpcVentas.Modelo.DBCorporativo;
using GrpcVentas.Modelo.DBVentas;
using Newtonsoft.Json;
using System.Data;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace GrpcVentas.General
{
    public class clsGeneral
    {
        private static readonly TimeZoneInfo ZonaHoraria = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        public static byte[] CompressData(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return memoryStream.ToArray();
            }
        }


        public static byte[] DecompressData(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                // Copia los datos descomprimidos al MemoryStream de destino
                gzipStream.CopyTo(decompressedStream);
                return decompressedStream.ToArray();
            }
        }

        public static List<T> ActualizarIdFran<T>(List<T> lista, int nuevoIdFran) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("Idfran");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, nuevoIdFran);
                }
            }
            return lista;
        }

        public static List<T> ActualizarIdFranCosto<T>(List<T> lista, int nuevoIdFran) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("IdFran");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, nuevoIdFran);
                }
            }
            return lista;
        }

        public static List<T> ActualizarIdFranPedido<T>(List<T> lista, DatosCorporativo objCorporativo) where T : class
        {
            foreach (var item in lista)
            {
                // Obtener el valor de Id_Farmacia_Pedido desde el objeto actual en la lista
                var idFarmaciaPedidoProperty = item.GetType().GetProperty("IdFarmaciaPedido");
                if (idFarmaciaPedidoProperty != null)
                {
                    string idFarmaciaPedido = idFarmaciaPedidoProperty.GetValue(item) as string;

                    // Si el valor de IdFarmaciaPedido no es nulo o vacío, obtener la franquicia
                    if (!string.IsNullOrEmpty(idFarmaciaPedido))
                    {
                        // Obtener la franquicia usando el IdFarmaciaPedido
                        string claveFarmaciaPedido = clsGeneral.ConvertirF(idFarmaciaPedido);
                        Franquicia objFranquicia = clsGeneralDatos.GetFranquicia(claveFarmaciaPedido, objCorporativo);

                        // Verificar si se encontró la franquicia
                        if (objFranquicia != null)
                        {
                            // Obtener el idFran de la franquicia
                            int idFran = objFranquicia.Idfran;

                            // Actualizar el campo IdFranPedido
                            var property = item.GetType().GetProperty("IdfranPedido");
                            if (property != null && property.CanWrite)
                            {
                                property.SetValue(item, idFran);
                            }
                        }
                    }
                }
            }
            return lista;
        }

        public static List<T> ActualizarIdFranProducto<T>(List<T> lista, int nuevoIdFran) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("Idfran");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, nuevoIdFran);
                }
            }
            return lista;
        }

        public static List<T> ActualizarIdFranSuplemento<T>(List<T> lista, int nuevoIdFran) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("IdFran");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, nuevoIdFran);
                }
            }
            return lista;
        }

        public static List<T> ActualizarIdFranKardex<T>(List<T> lista, int nuevoIdFran) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("IdFran");
                if (property != null && property.CanWrite)
                {
                    property.SetValue(item, nuevoIdFran);
                }
            }
            return lista;
        }

        public static List<T> ActualizarFecha<T>(List<T> lista, string nuevaFecha) where T : class
        {
            foreach (var item in lista)
            {
                var property = item.GetType().GetProperty("Ultimaactualizacion");
                if (property != null && property.CanWrite && property.PropertyType == typeof(string))
                {
                    property.SetValue(item, nuevaFecha);
                }

                var property2 = item.GetType().GetProperty("UltimaActualizacion");
                if (property2 != null && property2.CanWrite && property2.PropertyType == typeof(string))
                {
                    property2.SetValue(item, nuevaFecha);
                }

                var property3 = item.GetType().GetProperty("Ultima_Actualizacion");
                if (property3 != null && property3.CanWrite && property3.PropertyType == typeof(string))
                {
                    property3.SetValue(item, nuevaFecha);
                }
            }
            return lista;
        }

        public static List<TEntity> ConvertirListaAEntidad<TProto, TEntity>(List<TProto> protoLista) where TEntity : new()
        {
            var entidadLista = new List<TEntity>();

            foreach (var proto in protoLista)
            {
                var entidad = new TEntity();
                var protoProperties = typeof(TProto).GetProperties();
                var entityProperties = typeof(TEntity).GetProperties();

                foreach (var protoProp in protoProperties)
                {
                    var entityProp = entityProperties.FirstOrDefault(p =>
        p.Name.Equals(protoProp.Name, StringComparison.OrdinalIgnoreCase));

                    if (entityProp != null && entityProp.CanWrite)
                    {
                        var value = protoProp.GetValue(proto);

                        if (value != null)
                        {
                            object convertedValue = value;

                            // Conversiones específicas de tipo
                            if (entityProp.PropertyType == typeof(decimal) && value is double doubleValue)
                            {
                                convertedValue = (decimal)doubleValue;
                            }
                            else if (entityProp.PropertyType == typeof(short) && value is int intValueForShort)
                            {
                                convertedValue = (short)intValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is int intValueForByte)
                            {
                                convertedValue = (byte)intValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is uint uintValueForByte)
                            {
                                convertedValue = (byte)uintValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(decimal?) && value is double doubleNullableValue)
                            {
                                convertedValue = (decimal?)doubleNullableValue;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is int intNullableValueForByte)
                            {
                                convertedValue = (byte?)intNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is uint uintNullableValueForByte)
                            {
                                convertedValue = (byte?)uintNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(short?) && value is int intNullableValueForShort)
                            {
                                convertedValue = (short?)intNullableValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte) && value is int intValueForSByte)
                            {
                                convertedValue = (sbyte)intValueForSByte;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte?) && value is int intNullableValueForSByte)
                            {
                                convertedValue = (sbyte?)intNullableValueForSByte;
                            }
                            else if ((entityProp.PropertyType == typeof(DateTime) || entityProp.PropertyType == typeof(DateTime?)) && value is string stringValue)
                            {
                                if (DateTime.TryParse(stringValue, out DateTime dateValue))
                                {
                                    convertedValue = dateValue;
                                }
                                else
                                {
                                    convertedValue = entityProp.PropertyType == typeof(DateTime?) ? (DateTime?)null : DateTime.MinValue;
                                }
                            }
                            // Nueva conversión: Boolean a sbyte?
                            else if (entityProp.PropertyType == typeof(sbyte?) && value is bool boolValue)
                            {
                                convertedValue = boolValue ? (sbyte?)1 : (sbyte?)0;
                            }

                            // Conversión para int (Int32)
                            else if (entityProp.PropertyType == typeof(int) && value is long longValueToInt)
                            {
                                // Si el valor es long pero la entidad espera int, hacemos un cast explícito.
                                convertedValue = (int)longValueToInt;
                            }
                            else if (entityProp.PropertyType == typeof(int) && value is uint uintValueToInt)
                            {
                                // Si el valor es uint pero la entidad espera int.
                                convertedValue = (int)uintValueToInt;
                            }
                            // Conversión para int? (Nullable<Int32>)
                            else if (entityProp.PropertyType == typeof(int?) && value is long longValueToIntNullable)
                            {
                                // Si el valor es long pero la entidad espera int?, hacemos un cast explícito.
                                convertedValue = (int?)longValueToIntNullable;
                            }

                            entityProp.SetValue(entidad, convertedValue);
                        }
                    }
                }
                entidadLista.Add(entidad);
            }

            return entidadLista;
        }

        public static List<TEntity> ConvertirListaAEntidadKardexCompras<TProto, TEntity>(List<TProto> protoLista) where TEntity : new()
        {
            var entidadLista = new List<TEntity>();

            foreach (var proto in protoLista)
            {
                var entidad = new TEntity();
                var protoProperties = typeof(TProto).GetProperties();
                var entityProperties = typeof(TEntity).GetProperties();

                string NormalizeName(string name)
                {
                    // Elimina guiones bajos y convierte a minúsculas para una comparación estricta de contenido
                    return name.Replace("_", "").ToLowerInvariant();
                }


                foreach (var protoProp in protoProperties)
                {
                    // Normaliza el nombre del proto para buscar
                    var protoPropNameNormalized = NormalizeName(protoProp.Name);

                    var entityProp = entityProperties.FirstOrDefault(p => NormalizeName(p.Name) == protoPropNameNormalized);

                    if (entityProp != null && entityProp.CanWrite)
                    {
                        var value = protoProp.GetValue(proto);

                        if (value != null)
                        {
                            object convertedValue = value;

                            // Conversiones específicas de tipo
                            if (entityProp.PropertyType == typeof(decimal) && value is double doubleValue)
                            {
                                convertedValue = (decimal)doubleValue;
                            }
                            else if (entityProp.PropertyType == typeof(short) && value is int intValueForShort)
                            {
                                convertedValue = (short)intValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is int intValueForByte)
                            {
                                convertedValue = (byte)intValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is uint uintValueForByte)
                            {
                                convertedValue = (byte)uintValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(decimal?) && value is double doubleNullableValue)
                            {
                                convertedValue = (decimal?)doubleNullableValue;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is int intNullableValueForByte)
                            {
                                convertedValue = (byte?)intNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is uint uintNullableValueForByte)
                            {
                                convertedValue = (byte?)uintNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(short?) && value is int intNullableValueForShort)
                            {
                                convertedValue = (short?)intNullableValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte) && value is int intValueForSByte)
                            {
                                convertedValue = (sbyte)intValueForSByte;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte?) && value is int intNullableValueForSByte)
                            {
                                convertedValue = (sbyte?)intNullableValueForSByte;
                            }
                            else if ((entityProp.PropertyType == typeof(DateTime) || entityProp.PropertyType == typeof(DateTime?)) && value is string stringValue)
                            {
                                if (DateTime.TryParse(stringValue, out DateTime dateValue))
                                {
                                    convertedValue = dateValue;
                                }
                                else
                                {
                                    convertedValue = entityProp.PropertyType == typeof(DateTime?) ? (DateTime?)null : DateTime.MinValue;
                                }
                            }
                            // Nueva conversión: Boolean a sbyte?
                            else if (entityProp.PropertyType == typeof(sbyte?) && value is bool boolValue)
                            {
                                convertedValue = boolValue ? (sbyte?)1 : (sbyte?)0;
                            }

                            // Conversión para int (Int32)
                            else if (entityProp.PropertyType == typeof(int) && value is long longValueToInt)
                            {
                                // Si el valor es long pero la entidad espera int, hacemos un cast explícito.
                                convertedValue = (int)longValueToInt;
                            }
                            else if (entityProp.PropertyType == typeof(int) && value is uint uintValueToInt)
                            {
                                // Si el valor es uint pero la entidad espera int.
                                convertedValue = (int)uintValueToInt;
                            }
                            // Conversión para int? (Nullable<Int32>)
                            else if (entityProp.PropertyType == typeof(int?) && value is long longValueToIntNullable)
                            {
                                // Si el valor es long pero la entidad espera int?, hacemos un cast explícito.
                                convertedValue = (int?)longValueToIntNullable;
                            }

                            entityProp.SetValue(entidad, convertedValue);
                        }
                    }
                }
                entidadLista.Add(entidad);
            }

            return entidadLista;
        }

        public static List<TEntity> ConvertirListaAEntidadSurtido<TProto, TEntity>(List<TProto> protoLista) where TEntity : new()
        {
            var entidadLista = new List<TEntity>();

            foreach (var proto in protoLista)
            {
                // Verifica si el campo Estatus es igual a 4
                var estatusProp = typeof(TProto).GetProperty("Estatus");
                if (estatusProp == null || (int)estatusProp.GetValue(proto) != 4)
                {
                    continue; // Si el estatus no es 4, omite la conversión para este elemento
                }

                var entidad = new TEntity();
                var protoProperties = typeof(TProto).GetProperties();
                var entityProperties = typeof(TEntity).GetProperties();

                foreach (var protoProp in protoProperties)
                {
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == protoProp.Name);

                    if (entityProp != null && entityProp.CanWrite)
                    {
                        var value = protoProp.GetValue(proto);

                        if (value != null)
                        {
                            object convertedValue = value;

                            // Conversiones específicas de tipo
                            if (entityProp.PropertyType == typeof(decimal) && value is double doubleValue)
                            {
                                convertedValue = (decimal)doubleValue;
                            }
                            else if (entityProp.PropertyType == typeof(short) && value is int intValueForShort)
                            {
                                convertedValue = (short)intValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is int intValueForByte)
                            {
                                convertedValue = (byte)intValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte) && value is uint uintValueForByte)
                            {
                                convertedValue = (byte)uintValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(decimal?) && value is double doubleNullableValue)
                            {
                                convertedValue = (decimal?)doubleNullableValue;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is int intNullableValueForByte)
                            {
                                convertedValue = (byte?)intNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(byte?) && value is uint uintNullableValueForByte)
                            {
                                convertedValue = (byte?)uintNullableValueForByte;
                            }
                            else if (entityProp.PropertyType == typeof(short?) && value is int intNullableValueForShort)
                            {
                                convertedValue = (short?)intNullableValueForShort;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte) && value is int intValueForSByte)
                            {
                                convertedValue = (sbyte)intValueForSByte;
                            }
                            else if (entityProp.PropertyType == typeof(sbyte?) && value is int intNullableValueForSByte)
                            {
                                convertedValue = (sbyte?)intNullableValueForSByte;
                            }
                            else if ((entityProp.PropertyType == typeof(DateTime) || entityProp.PropertyType == typeof(DateTime?)) && value is string stringValue)
                            {
                                if (DateTime.TryParse(stringValue, out DateTime dateValue))
                                {
                                    convertedValue = dateValue;
                                }
                                else
                                {
                                    convertedValue = entityProp.PropertyType == typeof(DateTime?) ? (DateTime?)null : DateTime.MinValue;
                                }
                            }

                            // Asigna el valor convertido
                            entityProp.SetValue(entidad, convertedValue);
                        }
                    }
                }
                entidadLista.Add(entidad);
            }

            return entidadLista;
        }

        public static List<protoInventarioSurtidoFranquicium> ConvertirAProtoInventarioSurtidoFranquicium(List<protoInventarioSurtido> listaSurtido)
        {
            var listaFranquicium = new List<protoInventarioSurtidoFranquicium>();

            foreach (var surtido in listaSurtido)
            {
                var franquicium = new protoInventarioSurtidoFranquicium
                {
                    Idfran = surtido.Idfran,
                    IdSurtido = surtido.IdSurtido,
                    IdSurtidoLocal = surtido.IdSurtidoLocal,
                    IdMovimiento = surtido.IdMovimiento,
                    Documento = surtido.Documento,
                    Referencia = surtido.Referencia,
                    FechaOperacion = surtido.FechaOperacion,
                    FechaHoraCaptura = surtido.FechaHoraCaptura,
                    FechaOperacionDescarga = surtido.FechaOperacionDescarga,
                    FechaFacturacion = surtido.FechaFacturacion,
                    IdUsuario = surtido.IdUsuario,
                    Factura = surtido.Factura,
                    FacturaFiscal = surtido.FacturaFiscal,
                    SurtidoElectronico = surtido.SurtidoElectronico,
                    IdProveedor = surtido.IdProveedor,
                    IdFarmaciaSurtido = surtido.IdFarmaciaSurtido,
                    Estatus = surtido.Estatus,
                    Observacion = surtido.Observacion,
                    Subtotal = surtido.Subtotal,
                    Descuento = surtido.Descuento,
                    Impuesto = surtido.Impuesto,
                    Total = surtido.Total,
                    Respaldo = surtido.Respaldo,
                    Conteo = surtido.Conteo,
                    SincRef = surtido.SincRef,
                    Signo = surtido.Signo,
                    Fechavencimiento = surtido.Fechavencimiento,
                    FacturaFiscalRef = surtido.FacturaFiscalRef,

                    // Puedes asignar un valor aquí para "ultimaActualizacion"
                    // Por ejemplo, usando DateTime.UtcNow para establecer la última actualización en tiempo actual
                    UltimaActualizacion = DateTime.UtcNow.ToString("o") // ISO 8601 format
                };

                listaFranquicium.Add(franquicium);
            }

            return listaFranquicium;
        }

        public static DateTime ConvertirAZonaHoraria(DateTime fecha)
        {
            // Establecer el comportamiento predeterminado de TimeZoneInfo (solo necesario en .NET 6+)
            AppContext.SetSwitch("System.Globalization.TimeZoneInfo.Default", true);

            // Definir la zona horaria de Ciudad de México
            TimeZoneInfo zonaMexico = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City");

            // Verificar si la fecha está en UTC para usar la conversión correcta
            if (fecha.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(fecha, zonaMexico);
            }
            else
            {
                return TimeZoneInfo.ConvertTime(fecha, zonaMexico);
            }
        }

        public static string ConvertirF(string clave)
        {
            string p1, p2;
            if (clave.Length <= 5)
            {
                p1 = clave.Substring(0, 1);
                p2 = clave.Substring(1, clave.Length - 1);
                if (p2.Length == 3)
                {
                    p2 = "0" + p2;
                }
                clave = "F" + p2;
            }
            return clave;
        }

        public static List<protoVentafran> ActualizarProdPremio(List<protoVentafran> ventas)
        {
            foreach (var venta in ventas)
            {
                venta.ProdPremio = venta.Naturistas;
            }
            return ventas;
        }

        public static List<protoVentafrandium> FormatearMesYActualizarFecha(List<protoVentafrandium> ventas)
        {
            foreach (var venta in ventas)
            {
                venta.Mes = venta.Mes.Trim();
                if (venta.Mes.Length == 1)
                {
                    venta.Mes = "0" + venta.Mes;
                }
                venta.FechaCalculo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return ventas;
        }

        public static string MD5Hash(string input)
        {
            // Usa using para asegurar que el objeto MD5 se desecha correctamente
            using (MD5 md5 = MD5.Create())
            {
                // Convierte la cadena a bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Calcula el hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convierte el array de bytes a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    // Formato "x2" para tener siempre 2 dígitos hexadecimales, en minúsculas
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
