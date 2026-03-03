using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GrpcVentas.Modelo.DBCorporativo;

public partial class CorporativoContext : DbContext
{
    public CorporativoContext()
    {
    }

    public CorporativoContext(DbContextOptions<CorporativoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DatosCorporativo> DatosCorporativos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseMySql("server=localhost;database=corporativo_siif;user=root;password=hola", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
    => optionsBuilder.UseMySql("server=ironman.mysql.database.azure.com;database=corporativo_siif;user=kgadmin;password=SaM345?84!d2x5)", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DatosCorporativo>(entity =>
        {
            entity.HasKey(e => e.Dominio)
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("datos_corporativo")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Dominio)
                .HasMaxLength(50)
                .HasColumnName("dominio");
            entity.Property(e => e.Uuid)
                .HasMaxLength(45)
                .HasColumnName("UUID");
            entity.Property(e => e.ActualizaDescuentosFacturacion)
                .HasDefaultValueSql("'0'")
                .HasColumnName("actualiza_descuentos_facturacion");
            entity.Property(e => e.AplicaBitacora)
                .HasDefaultValueSql("'0'")
                .HasColumnName("aplica_bitacora");
            entity.Property(e => e.AplicaDocker)
                .HasDefaultValueSql("'0'")
                .HasColumnName("aplicaDocker");
            entity.Property(e => e.AplicaFacturacionEnLinea)
                .HasDefaultValueSql("'0'")
                .HasColumnName("aplica_facturacion_en_linea");
            entity.Property(e => e.AplicaMonitor)
                .HasDefaultValueSql("'0'")
                .HasColumnName("aplica_monitor");
            entity.Property(e => e.Cnxn)
                .HasMaxLength(50)
                .HasColumnName("cnxn");
            entity.Property(e => e.Corporativo).HasColumnName("corporativo");
            entity.Property(e => e.DbSyncro2)
                .HasMaxLength(50)
                .HasColumnName("db_syncro2");
            entity.Property(e => e.DiasAudit)
                .HasMaxLength(50)
                .HasColumnName("dias_audit");
            entity.Property(e => e.DiasEvaluar).HasColumnName("dias_evaluar");
            entity.Property(e => e.DmnNcptd)
                .HasMaxLength(100)
                .HasColumnName("dmnNCPTD");
            entity.Property(e => e.Dsn)
                .HasMaxLength(45)
                .HasComment("U")
                .HasColumnName("dsn");
            entity.Property(e => e.Dsn1)
                .HasMaxLength(45)
                .HasComment("FE")
                .HasColumnName("dsn1");
            entity.Property(e => e.Dsnasistencia)
                .HasMaxLength(45)
                .HasComment("Conexión para sistema de registro de asistencia.")
                .HasColumnName("dsnasistencia");
            entity.Property(e => e.Dsntr)
                .HasMaxLength(45)
                .HasColumnName("dsntr");
            entity.Property(e => e.ErpDb)
                .HasMaxLength(50)
                .HasColumnName("erp_db");
            entity.Property(e => e.ErpPss)
                .HasMaxLength(50)
                .HasColumnName("erp_pss");
            entity.Property(e => e.ErpUsr)
                .HasMaxLength(50)
                .HasColumnName("erp_usr");
            entity.Property(e => e.FacturacionEnAzure)
                .HasDefaultValueSql("'0'")
                .HasColumnName("facturacion_en_azure");
            entity.Property(e => e.HoraAudit)
                .HasMaxLength(50)
                .HasColumnName("hora_audit");
            entity.Property(e => e.Hst)
                .HasMaxLength(50)
                .HasColumnName("hst");
            entity.Property(e => e.MaxCnnSyncro2).HasColumnName("max_cnn_syncro2");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreCorto)
                .HasMaxLength(50)
                .HasColumnName("nombreCorto");
            entity.Property(e => e.PonderacionYdesabasto)
                .HasDefaultValueSql("'0'")
                .HasColumnName("ponderacionYDesabasto");
            entity.Property(e => e.PresupuestosDb)
                .HasMaxLength(45)
                .HasColumnName("presupuestosDb");
            entity.Property(e => e.PresupuestosHost)
                .HasMaxLength(45)
                .HasColumnName("presupuestosHost");
            entity.Property(e => e.PresupuestosPss)
                .HasMaxLength(45)
                .HasColumnName("presupuestosPss");
            entity.Property(e => e.PresupuestosUsr)
                .HasMaxLength(45)
                .HasColumnName("presupuestosUsr");
            entity.Property(e => e.PssAsis)
                .HasMaxLength(45)
                .HasColumnName("pss_asis");
            entity.Property(e => e.PssSyncro2)
                .HasMaxLength(150)
                .HasColumnName("pss_syncro2");
            entity.Property(e => e.Razonsocial)
                .HasMaxLength(75)
                .HasColumnName("razonsocial");
            entity.Property(e => e.Rfc)
                .HasMaxLength(15)
                .HasColumnName("rfc");
            entity.Property(e => e.UsrAsis)
                .HasMaxLength(45)
                .HasColumnName("usr_asis");
            entity.Property(e => e.UsrSyncro2)
                .HasMaxLength(50)
                .HasColumnName("usr_syncro2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
