using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("desk_catalogo_operacion")]
    public class DeskCatalogoOperacion
    {
        [Key] // Indica que este es el campo de la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Necesario si idOperacion no es auto-incrementable
        public int idOperacion { get; set; } // int NOT NULL (PK)

        public string Nombre { get; set; } // varchar(45) DEFAULT NULL

        public int? Estatus { get; set; } // int DEFAULT NULL (Usamos 'int?' para permitir valores nulos)

        public int? DiasAuditar { get; set; } // int DEFAULT NULL

        public int? Frecuencia { get; set; } // int DEFAULT NULL

        // Usamos string o TimeSpan para el tipo TIME. String es la opción más sencilla y compatible.
        public TimeSpan? HoraInicio { get; set; } // time DEFAULT NULL 

        public TimeSpan? HoraFin { get; set; } // time DEFAULT NULL

        public int? NumeroTransmision { get; set; } // int DEFAULT NULL
    }
}
