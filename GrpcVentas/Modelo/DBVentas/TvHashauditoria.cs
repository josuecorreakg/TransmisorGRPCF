using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcVentas.Modelo.DBVentas
{
    [Table("tv_hashauditoria")]
    public class TvHashauditoria
    {
        [Key]
        [Column(Order = 1)] // Usado para definir el orden de la clave compuesta (útil en EF 6, buena práctica en EF Core)
        [MaxLength(20)] // Define el límite de varchar(20)
        public string idfran { get; set; } // varchar(20) NOT NULL

        [Key]
        [Column(Order = 2)]
        public int anio { get; set; } // int NOT NULL

        [Key]
        [Column(Order = 3)]
        public byte mes { get; set; } // tinyint NOT NULL (Usamos 'byte' para Tinyint)

        [Key]
        [Column(Order = 4)]
        public byte idOperacion { get; set; } // tinyint NOT NULL

        // ------------------------------------------
        // Campos HASH (varchar(200) DEFAULT '0')
        // ------------------------------------------
        [MaxLength(200)]
        public string hash1 { get; set; } = "0"; // DEFAULT '0'
        [MaxLength(200)]
        public string hash2 { get; set; } = "0";
        [MaxLength(200)]
        public string hash3 { get; set; } = "0";
        [MaxLength(200)]
        public string hash4 { get; set; } = "0";
        [MaxLength(200)]
        public string hash5 { get; set; } = "0";
        [MaxLength(200)]
        public string hash6 { get; set; } = "0";
        [MaxLength(200)]
        public string hash7 { get; set; } = "0";
        [MaxLength(200)]
        public string hash8 { get; set; } = "0";
        [MaxLength(200)]
        public string hash9 { get; set; } = "0";
        [MaxLength(200)]
        public string hash10 { get; set; } = "0";
        [MaxLength(200)]
        public string hash11 { get; set; } = "0";
        [MaxLength(200)]
        public string hash12 { get; set; } = "0";
        [MaxLength(200)]
        public string hash13 { get; set; } = "0";
        [MaxLength(200)]
        public string hash14 { get; set; } = "0";
        [MaxLength(200)]
        public string hash15 { get; set; } = "0";
        [MaxLength(200)]
        public string hash16 { get; set; } = "0";
        [MaxLength(200)]
        public string hash17 { get; set; } = "0";
        [MaxLength(200)]
        public string hash18 { get; set; } = "0";
        [MaxLength(200)]
        public string hash19 { get; set; } = "0";
        [MaxLength(200)]
        public string hash20 { get; set; } = "0";
        [MaxLength(200)]
        public string hash21 { get; set; } = "0";
        [MaxLength(200)]
        public string hash22 { get; set; } = "0";
        [MaxLength(200)]
        public string hash23 { get; set; } = "0";
        [MaxLength(200)]
        public string hash24 { get; set; } = "0";
        [MaxLength(200)]
        public string hash25 { get; set; } = "0";
        [MaxLength(200)]
        public string hash26 { get; set; } = "0";
        [MaxLength(200)]
        public string hash27 { get; set; } = "0";
        [MaxLength(200)]
        public string hash28 { get; set; } = "0";
        [MaxLength(200)]
        public string hash29 { get; set; } = "0";
        [MaxLength(200)]
        public string hash30 { get; set; } = "0";
        [MaxLength(200)]
        public string hash31 { get; set; } = "0";

        // ------------------------------------------
        // Campos CONTADOR (tinyint DEFAULT '0')
        // ------------------------------------------
        public byte? contador1 { get; set; } = 0; // Usamos 'byte?' para permitir NULL (aunque el default sea 0)
        public byte? contador2 { get; set; } = 0;
        public byte? contador3 { get; set; } = 0;
        public byte? contador4 { get; set; } = 0;
        public byte? contador5 { get; set; } = 0;
        public byte? contador6 { get; set; } = 0;
        public byte? contador7 { get; set; } = 0;
        public byte? contador8 { get; set; } = 0;
        public byte? contador9 { get; set; } = 0;
        public byte? contador10 { get; set; } = 0;
        public byte? contador11 { get; set; } = 0;
        public byte? contador12 { get; set; } = 0;
        public byte? contador13 { get; set; } = 0;
        public byte? contador14 { get; set; } = 0;
        public byte? contador15 { get { return _contador15; } set { _contador15 = value; } } // Usamos propiedad completa para ejemplo de campo.
        private byte? _contador15 = 0;

        // ... Campos 16 a 31 (el patrón se repite)
        public byte? contador16 { get; set; } = 0;
        public byte? contador17 { get; set; } = 0;
        public byte? contador18 { get; set; } = 0;
        public byte? contador19 { get; set; } = 0;
        public byte? contador20 { get; set; } = 0;
        public byte? contador21 { get; set; } = 0;
        public byte? contador22 { get; set; } = 0;
        public byte? contador23 { get; set; } = 0;
        public byte? contador24 { get; set; } = 0;
        public byte? contador25 { get; set; } = 0;
        public byte? contador26 { get; set; } = 0;
        public byte? contador27 { get; set; } = 0;
        public byte? contador28 { get; set; } = 0;
        public byte? contador29 { get; set; } = 0;
        public byte? contador30 { get; set; } = 0;
        public byte? contador31 { get; set; } = 0;
    }
}
