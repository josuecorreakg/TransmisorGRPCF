using GrpcVentas.Modelo.DBCorporativo;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using System;
using System.Collections.Generic;

namespace GrpcVentas.Modelo.DBVentas;

public partial class VentasContext : DbContext
{
    private string sCliente;
    private string sHost;
    private string Database;
    private string Uid;
    private string Pwd;

    public VentasContext()
    {
    }

    public VentasContext(DatosCorporativo obj)
    {
        this.sCliente = obj.NombreCorto;
        this.sHost = obj.Hst;
        this.Database = obj.DbSyncro2;
        this.Uid = obj.UsrSyncro2;
        this.Pwd = obj.PssSyncro2;

    }

    public VentasContext(DbContextOptions<VentasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoriacomercial> Categoriacomercials { get; set; }

    public virtual DbSet<Categoriacomercialproducto> Categoriacomercialproductos { get; set; }

    public virtual DbSet<ConfiguracionFarmacium> ConfiguracionFarmacia { get; set; }

    public virtual DbSet<ConsultaConsultorioturnodetalle> ConsultaConsultorioturnodetalles { get; set; }

    public virtual DbSet<ConsultaRespuestum> ConsultaRespuesta { get; set; }

    public virtual DbSet<ErpCiKardex> ErpCiKardices { get; set; }

    public virtual DbSet<ErpSposSeguridadCategorium> ErpSposSeguridadCategoria { get; set; }

    public virtual DbSet<ErpSposSeguridadRol> ErpSposSeguridadRols { get; set; }

    public virtual DbSet<ErpSposSeguridadTarea> ErpSposSeguridadTareas { get; set; }

    public virtual DbSet<ErpSposUsuarioRol> ErpSposUsuarioRols { get; set; }

    public virtual DbSet<ErpSposUsuarioporrol> ErpSposUsuarioporrols { get; set; }

    public virtual DbSet<FacturacionFactura> FacturacionFacturas { get; set; }

    public virtual DbSet<FacturacionReferencium> FacturacionReferencia { get; set; }

    public virtual DbSet<Franquicia> Franquicias { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<InventarioFisicoCompleto> InventarioFisicoCompletos { get; set; }

    public virtual DbSet<InventarioOtro> InventarioOtros { get; set; }

    public virtual DbSet<InventarioOtrosDetalle> InventarioOtrosDetalles { get; set; }

    public virtual DbSet<InventarioSurtido> InventarioSurtidos { get; set; }

    public virtual DbSet<InventarioSurtidoDetalle> InventarioSurtidoDetalles { get; set; }

    public virtual DbSet<InventarioSurtidoFranquicium> InventarioSurtidoFranquicia { get; set; }

    public virtual DbSet<InventarioTraspaso> InventarioTraspasos { get; set; }

    public virtual DbSet<InventarioTraspasoDetalle> InventarioTraspasoDetalles { get; set; }

    public virtual DbSet<Operacion> Operacions { get; set; }

    public virtual DbSet<Operacionglobal> Operacionglobals { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<ProductoPorlistaprecio> ProductoPorlistaprecios { get; set; }

    public virtual DbSet<ProductoSuplementoFranquicium> ProductoSuplementoFranquicia { get; set; }

    public virtual DbSet<Productofranquicium> Productofranquicia { get; set; }

    public virtual DbSet<TomaTemperatura> TomaTemperaturas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VentaDescuento> VentaDescuentos { get; set; }

    public virtual DbSet<VentaPago> VentaPagos { get; set; }

    public virtual DbSet<VentaProducto> VentaProductos { get; set; }

    public virtual DbSet<VentaProductoDesglose> VentaProductoDesgloses { get; set; }

    public virtual DbSet<VentaRecetaControlado> VentaRecetaControlados { get; set; }

    public virtual DbSet<Ventafran> Ventafrans { get; set; }

    public virtual DbSet<Ventafrandium> Ventafrandia { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    public DbSet<CostoOp> CostoOps { get; set; }

    public DbSet<DeskCatalogoOperacion> DeskCatalogoOperacion { get; set; }

    public DbSet<TvHashauditoria> TvHashauditoria { get; set; }
    public DbSet<Catsistemas> CatSistemas { get; set; }
    public DbSet<ControlVersion> ControlVersiones { get; set; }

    public DbSet<TvTransmision> Tvtransmision { get; set; }

    public DbSet<TvLiberaciones> TvLiberaciones { get; set; }

    public DbSet<KardexControlMensual> KardexControlMensual { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=" + this.sHost + ";database=" + this.Database + "; user=" + this.Uid + "; password=" + this.Pwd + "; AllowLoadLocalInfile=true; AllowZeroDateTime=true;ConvertZeroDateTime=true;", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoriacomercial>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaComercial).HasName("PRIMARY");

            entity
                .ToTable("categoriacomercial")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.IdCategoriaComercial)
                .ValueGeneratedNever()
                .HasColumnName("Id_CategoriaComercial");
            entity.Property(e => e.Grupo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("grupo");
            entity.Property(e => e.Nombre).HasMaxLength(30);
            entity.Property(e => e.Orden).HasColumnName("orden");
        });

        modelBuilder.Entity<Categoriacomercialproducto>(entity =>
        {
            entity.HasKey(e => new { e.IdCategoriaComercial, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("categoriacomercialproducto")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.IdProducto, "idx_p");

            entity.Property(e => e.IdCategoriaComercial).HasColumnName("Id_CategoriaComercial");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
        });

        modelBuilder.Entity<ConfiguracionFarmacium>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.Fran, e.IdConfiguracion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("configuracion_farmacia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.IdAlmacen, "idx_alm");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.Fran)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("fran");
            entity.Property(e => e.IdConfiguracion).HasColumnName("id_configuracion");
            entity.Property(e => e.Afiliacionbancaria)
                .HasMaxLength(10)
                .HasColumnName("afiliacionbancaria");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(60)
                .HasColumnName("ciudad");
            entity.Property(e => e.Codigopostal).HasColumnName("codigopostal");
            entity.Property(e => e.Colonia)
                .HasMaxLength(80)
                .HasColumnName("colonia");
            entity.Property(e => e.Correoavisoprivacidad)
                .HasMaxLength(80)
                .HasColumnName("correoavisoprivacidad");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(200)
                .HasColumnName("domicilio");
            entity.Property(e => e.Domiciliofiscal1)
                .HasMaxLength(100)
                .HasColumnName("domiciliofiscal1");
            entity.Property(e => e.Domiciliofiscal2)
                .HasMaxLength(100)
                .HasColumnName("domiciliofiscal2");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.Facturacionelectronica).HasColumnName("facturacionelectronica");
            entity.Property(e => e.FechaCaptura)
                .HasColumnType("datetime")
                .HasColumnName("fecha_captura");
            entity.Property(e => e.Fechaapertura)
                .HasColumnType("datetime")
                .HasColumnName("fechaapertura");
            //entity.Property(e => e.HoraActualizacion).HasMaxLength(45);
            entity.Property(e => e.Horacierre)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("horacierre");
            entity.Property(e => e.IdAlmacen)
                .HasMaxLength(6)
                .HasColumnName("id_almacen");
            entity.Property(e => e.IdCategoriaventa).HasColumnName("id_categoriaventa");
            entity.Property(e => e.IdDivision)
                .HasMaxLength(3)
                .HasColumnName("id_division");
            entity.Property(e => e.IdFarmacia)
                .HasMaxLength(6)
                .HasColumnName("id_farmacia");
            //entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
            entity.Property(e => e.IdNegocioFranquicia).HasColumnName("id_negocio_franquicia");
            entity.Property(e => e.IdPais)
                .HasMaxLength(3)
                .HasColumnName("id_pais");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(15)
                .HasColumnName("id_usuario");
            //entity.Property(e => e.Iepspos).HasColumnName("IEPSPOS");
            entity.Property(e => e.Ipcontivity)
                .HasMaxLength(15)
                .HasColumnName("ipcontivity");
            entity.Property(e => e.Iva)
                .HasPrecision(5, 2)
                .HasColumnName("iva");
            entity.Property(e => e.Leyendafiscal)
                .HasMaxLength(250)
                .HasColumnName("leyendafiscal");
            entity.Property(e => e.Modificado).HasColumnName("modificado");
            entity.Property(e => e.Modificadoiva).HasColumnName("modificadoiva");
            entity.Property(e => e.Municipio)
                .HasMaxLength(100)
                .HasColumnName("municipio");
            entity.Property(e => e.Noexterior)
                .HasMaxLength(100)
                .HasColumnName("noexterior");
            entity.Property(e => e.Nointerior)
                .HasMaxLength(100)
                .HasColumnName("nointerior");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(55)
                .HasColumnName("pais");
            entity.Property(e => e.Passwordcontivity)
                .HasMaxLength(50)
                .HasColumnName("passwordcontivity");
            entity.Property(e => e.Razonsocial)
                .HasMaxLength(150)
                .HasColumnName("razonsocial");
            entity.Property(e => e.Regimenfiscal)
                .HasMaxLength(150)
                .HasColumnName("regimenfiscal");
            entity.Property(e => e.Requerirasistencia).HasColumnName("requerirasistencia");
            entity.Property(e => e.Rfc1)
                .HasMaxLength(4)
                .HasColumnName("rfc1");
            entity.Property(e => e.Rfc2)
                .HasMaxLength(6)
                .HasColumnName("rfc2");
            entity.Property(e => e.Rfc3)
                .HasMaxLength(3)
                .HasColumnName("rfc3");
            entity.Property(e => e.RutaRdis)
                .HasMaxLength(150)
                .HasColumnName("ruta_rdis");
            entity.Property(e => e.Surtidodirecto).HasColumnName("surtidodirecto");
            entity.Property(e => e.Tienegondolas).HasColumnName("tienegondolas");
            entity.Property(e => e.Transaccionenlinea).HasColumnName("transaccionenlinea");
            entity.Property(e => e.Usuariocontivity)
                .HasMaxLength(50)
                .HasColumnName("usuariocontivity");
            entity.Property(e => e.WebserviceSincroniza)
                .HasMaxLength(100)
                .HasColumnName("webservice_sincroniza");
            entity.Property(e => e.WebserviceVentas)
                .HasMaxLength(100)
                .HasColumnName("webservice_ventas");
        });

        modelBuilder.Entity<ConsultaConsultorioturnodetalle>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdConsultorioTurno })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("consulta_consultorioturnodetalle")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdConsultorioTurno).HasColumnName("Id_ConsultorioTurno");
            entity.Property(e => e.FechaCaptura).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.IdConsultorio)
                .HasMaxLength(1)
                .HasColumnName("Id_Consultorio");
            entity.Property(e => e.IdMotivoSinConsultas).HasColumnName("Id_MotivoSinConsultas");
            entity.Property(e => e.IdTurno)
                .HasMaxLength(1)
                .HasColumnName("Id_Turno");
            entity.Property(e => e.Observaciones).HasMaxLength(255);
            entity.Property(e => e.UsuarioCaptura).HasMaxLength(15);
        });

        modelBuilder.Entity<ConsultaRespuestum>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdConsultorioTurno, e.IdConsultaPregunta })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("consulta_respuesta")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdConsultorioTurno).HasColumnName("Id_ConsultorioTurno");
            entity.Property(e => e.IdConsultaPregunta).HasColumnName("Id_ConsultaPregunta");
            entity.Property(e => e.Respuesta).HasMaxLength(50);
        });

        modelBuilder.Entity<ErpCiKardex>(entity =>
        {
            entity.HasKey(e => new { e.IdFran, e.FechaOperacion, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("erp_ci_kardex")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.IdFran).HasColumnName("id_fran");
            entity.Property(e => e.FechaOperacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_operacion");
            entity.Property(e => e.IdProducto)
                .HasColumnName("id_producto")
                .HasMaxLength(10);
            entity.Property(e => e.Clave)
                .HasMaxLength(6)
                .HasColumnName("clave");
            entity.Property(e => e.EntradaCompraProveedores).HasColumnName("entrada_compra_proveedores");
            entity.Property(e => e.EntradaDevoluciones).HasColumnName("entrada_devoluciones");
            entity.Property(e => e.EntradasAjustes).HasColumnName("entradas_ajustes");
            entity.Property(e => e.EntradasCompras).HasColumnName("entradas_compras");
            entity.Property(e => e.EntradasTraspaso).HasColumnName("entradas_traspaso");
            entity.Property(e => e.Franquicia)
                .HasMaxLength(50)
                .HasColumnName("franquicia");
            entity.Property(e => e.InventarioFinal).HasColumnName("inventario_final");
            entity.Property(e => e.InventarioInicial).HasColumnName("inventario_inicial");
            entity.Property(e => e.PrecioCompra)
                .HasPrecision(19, 2)
                .HasColumnName("precio_compra");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(19, 2)
                .HasColumnName("precio_venta");
            entity.Property(e => e.PrecioVentaSinIva)
                .HasPrecision(19, 2)
                .HasColumnName("precio_venta_sin_iva");
            entity.Property(e => e.Producto)
                .HasMaxLength(100)
                .HasColumnName("producto");
            entity.Property(e => e.SalidasAjustes).HasColumnName("salidas_ajustes");
            entity.Property(e => e.SalidasNotasCredito).HasColumnName("salidas_notas_credito");
            entity.Property(e => e.SalidasTraspaso).HasColumnName("salidas_traspaso");
            entity.Property(e => e.SalidasVentas).HasColumnName("salidas_ventas");
        });

        modelBuilder.Entity<ErpSposSeguridadCategorium>(entity =>
        {
            entity.HasKey(e => new { e.IdCategoria, e.Idfran })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("erp_spos_seguridad_categoria");

            entity.Property(e => e.IdCategoria).HasColumnName("Id_categoria");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<ErpSposSeguridadRol>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdRol, e.IdCategoria })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("erp_spos_seguridad_rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
        });

        modelBuilder.Entity<ErpSposSeguridadTarea>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdTarea, e.IdCategoria })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("erp_spos_seguridad_tarea");

            entity.Property(e => e.IdTarea).HasColumnName("Id_tarea");
            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<ErpSposUsuarioRol>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdRol })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("erp_spos_usuario_rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.EstatusRegistro).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(30);
        });

        modelBuilder.Entity<ErpSposUsuarioporrol>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdUsuario, e.IdRol })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("erp_spos_usuarioporrol");

            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
        });

        modelBuilder.Entity<FacturacionFactura>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.Serie, e.Folio })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("facturacion_factura")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.Serie).HasMaxLength(10);
            entity.Property(e => e.FechaCancelacion).HasColumnType("datetime");
            entity.Property(e => e.FechaFacturacion).HasColumnType("datetime");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(13)
                .HasColumnName("Id_Cliente");
            entity.Property(e => e.SifeEstatus)
                .HasDefaultValueSql("'0'")
                .HasColumnName("sife_estatus");
        });

        modelBuilder.Entity<FacturacionReferencium>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.Serie, e.Folio, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0, 0 });

            entity
                .ToTable("facturacion_referencia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.Serie).HasMaxLength(10);
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.GranTotal).HasPrecision(19, 4);
            entity.Property(e => e.Iva)
                .HasPrecision(19, 4)
                .HasColumnName("IVA");
            entity.Property(e => e.ReferenciaTicket).HasMaxLength(13);
            entity.Property(e => e.SubTotalNeto).HasPrecision(19, 4);
            entity.Property(e => e.TotalNeto).HasPrecision(19, 4);
        });

        modelBuilder.Entity<Franquicia>(entity =>
        {
            entity.HasKey(e => e.Idfran).HasName("PRIMARY");

            entity
                .ToTable("franquicias")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.Empresa, "idx_emp");

            entity.Property(e => e.Idfran)
                .ValueGeneratedNever()
                .HasColumnName("idfran");
            entity.Property(e => e.Activa).HasColumnName("activa");
            entity.Property(e => e.Bono)
                .HasPrecision(5, 2)
                .HasColumnName("bono");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .HasColumnName("ciudad")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Clave)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("clave");
            entity.Property(e => e.Colonia)
                .HasMaxLength(50)
                .HasColumnName("colonia");
            entity.Property(e => e.Comision)
                .HasPrecision(4, 2)
                .HasColumnName("comision");
            entity.Property(e => e.Correo)
                .HasMaxLength(250)
                .HasColumnName("correo")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Cp)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("cp");
            entity.Property(e => e.Delmun)
                .HasMaxLength(50)
                .HasColumnName("delmun")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Diaemergente).HasColumnName("diaemergente");
            entity.Property(e => e.Diapedido).HasColumnName("diapedido");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(151)
                .HasColumnName("domicilio");
            entity.Property(e => e.Empresa).HasColumnName("empresa");
            entity.Property(e => e.Estado)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Fax)
                .HasMaxLength(20)
                .HasColumnName("fax");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("'2015-01-01'")
                .HasColumnName("fecha_apertura");
            entity.Property(e => e.IdfranAnt).HasColumnName("idfran_ant");
            entity.Property(e => e.Iva)
                .HasPrecision(3, 2)
                .HasColumnName("iva");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Premiomv)
                .HasPrecision(9, 2)
                .HasColumnName("premiomv");
            entity.Property(e => e.Regpatron)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("regpatron");
            entity.Property(e => e.Segnegocio)
                .HasMaxLength(20)
                .HasColumnName("segnegocio");
            entity.Property(e => e.Sinoperacion).HasColumnName("sinoperacion");
            entity.Property(e => e.Supervisor)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("supervisor");
            entity.Property(e => e.Tel1)
                .HasMaxLength(20)
                .HasColumnName("tel1");
            entity.Property(e => e.Tel2)
                .HasMaxLength(20)
                .HasColumnName("tel2");
            entity.Property(e => e.Tipo)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("tipo");
            entity.Property(e => e.Transmite).HasColumnName("transmite");
            entity.Property(e => e.VersionSyncro2)
                .HasDefaultValueSql("'0.000'")
                .HasColumnType("double(5,3)")
                .HasColumnName("version_syncro2");
            entity.Property(e => e.VersionSyncro2Activa)
                .HasDefaultValueSql("'0'")
                .HasColumnName("version_syncro2_activa");
            entity.Property(e => e.Zona)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("zona");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("inventario")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(45)
                .HasColumnName("id_Producto");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Modificacion");
            entity.Property(e => e.UltimaActualizacion).HasColumnType("datetime");
        });

        modelBuilder.Entity<InventarioFisicoCompleto>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.FechaOperacion, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("inventario_fisico_completo")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => new { e.FechaOperacion, e.Idfran, e.IdProducto, e.ExistenciaFinal }, "InvFecha");

            entity.HasIndex(e => e.IdProducto, "InvProd");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.Costo).HasPrecision(19, 6);
        });

        modelBuilder.Entity<InventarioOtro>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdRegistro })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("inventario_otros")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdRegistro).HasColumnName("Id_Registro");
            entity.Property(e => e.Documento)
                .HasMaxLength(15)
                .HasDefaultValueSql("''");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Captura");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.IdMovimiento).HasColumnName("Id_Movimiento");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Referencia).HasMaxLength(20);
            entity.Property(e => e.Signo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SincRef).HasMaxLength(64);
        });

        modelBuilder.Entity<InventarioOtrosDetalle>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdRegistro, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("inventario_otros_detalle")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdRegistro).HasColumnName("Id_Registro");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
        });

        modelBuilder.Entity<InventarioSurtido>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdSurtido, e.IdSurtidoLocal })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("inventario_surtido")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.Documento, "IX_Inventario_Surtido");

            entity.HasIndex(e => e.SincRef, "SincRef").IsUnique();

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdSurtido).HasColumnName("Id_Surtido");
            entity.Property(e => e.IdSurtidoLocal).HasColumnName("Id_Surtido_Local");
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.Documento).HasMaxLength(15);
            entity.Property(e => e.Estatus)
                .HasDefaultValueSql("'1'")
                .HasComment("1:Pendiente, 2:Capturada, 3:?");
            entity.Property(e => e.Factura)
                .HasMaxLength(20)
                .HasComment("para buscar el documento en el websevice");
            entity.Property(e => e.FacturaFiscal)
                .HasMaxLength(20)
                .HasComment("para buscar el documento en el websevice")
                .HasColumnName("Factura_Fiscal");
            entity.Property(e => e.FacturaFiscalRef)
                .HasMaxLength(20)
                .HasColumnName("factura_fiscal_ref");
            entity.Property(e => e.FechaFacturacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Facturacion");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Captura");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.FechaOperacionDescarga)
                .HasColumnType("datetime")
                .HasColumnName("FechaOperacion_Descarga");
            entity.Property(e => e.Fechavencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechavencimiento");
            entity.Property(e => e.IdFarmaciaSurtido)
                .HasMaxLength(6)
                .HasColumnName("Id_FarmaciaSurtido");
            entity.Property(e => e.IdMovimiento).HasColumnName("Id_Movimiento");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasDefaultValueSql("''")
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Impuesto).HasPrecision(19, 4);
            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Referencia).HasMaxLength(15);
            entity.Property(e => e.Respaldo)
                .HasMaxLength(1)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Signo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SincRef).HasMaxLength(64);
            entity.Property(e => e.Subtotal).HasPrecision(19, 4);
            entity.Property(e => e.Total).HasPrecision(19, 4);
        });

        modelBuilder.Entity<InventarioSurtidoDetalle>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdSurtido, e.IdSurtidoLocal, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("inventario_surtido_detalle")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdSurtido).HasColumnName("Id_Surtido");
            entity.Property(e => e.IdSurtidoLocal).HasColumnName("Id_Surtido_Local");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.CostoUnitario).HasPrecision(19, 4);
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.Descuentoporciento)
                .HasPrecision(19, 4)
                .HasColumnName("descuentoporciento");
            entity.Property(e => e.Impuesto).HasPrecision(19, 4);
            entity.Property(e => e.Ivaporciento)
                .HasPrecision(19, 4)
                .HasColumnName("ivaporciento");
            entity.Property(e => e.Subtotal)
                .HasPrecision(19, 4)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total).HasPrecision(19, 4);
        });

        modelBuilder.Entity<InventarioSurtidoFranquicium>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdSurtido, e.IdSurtidoLocal })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("inventario_surtido_franquicia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.Documento, "IX_Inventario_Surtido");

            entity.HasIndex(e => e.SincRef, "SincRef").IsUnique();

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdSurtido).HasColumnName("Id_Surtido");
            entity.Property(e => e.IdSurtidoLocal).HasColumnName("Id_Surtido_Local");
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.Documento).HasMaxLength(15);
            entity.Property(e => e.Estatus)
                .HasDefaultValueSql("'1'")
                .HasComment("1:Pendiente, 2:Capturada, 3:?");
            entity.Property(e => e.Factura)
                .HasMaxLength(20)
                .HasComment("para buscar el documento en el websevice");
            entity.Property(e => e.FacturaFiscal)
                .HasMaxLength(20)
                .HasComment("para buscar el documento en el websevice")
                .HasColumnName("Factura_Fiscal");
            entity.Property(e => e.FacturaFiscalRef)
                .HasMaxLength(20)
                .HasColumnName("factura_fiscal_ref");
            entity.Property(e => e.FechaFacturacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Facturacion");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Captura");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.FechaOperacionDescarga)
                .HasColumnType("datetime")
                .HasColumnName("FechaOperacion_Descarga");
            entity.Property(e => e.Fechavencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechavencimiento");
            entity.Property(e => e.IdFarmaciaSurtido)
                .HasMaxLength(6)
                .HasColumnName("Id_FarmaciaSurtido");
            entity.Property(e => e.IdMovimiento).HasColumnName("Id_Movimiento");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_Proveedor");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasDefaultValueSql("''")
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Impuesto).HasPrecision(19, 4);
            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Referencia).HasMaxLength(15);
            entity.Property(e => e.Respaldo)
                .HasMaxLength(1)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Signo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SincRef).HasMaxLength(64);
            entity.Property(e => e.Subtotal).HasPrecision(19, 4);
            entity.Property(e => e.Total).HasPrecision(19, 4);
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<InventarioTraspaso>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdFarmaciaEntrega, e.IdTraspaso, e.FechaOperacion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("inventario_traspaso")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdFarmaciaEntrega)
                .HasMaxLength(6)
                .HasColumnName("Id_Farmacia_Entrega");
            entity.Property(e => e.IdTraspaso).HasColumnName("Id_Traspaso");
            entity.Property(e => e.Documento)
                .HasMaxLength(15)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Estatus).HasComment("Solicitud, Autorizado, Rechazado y Capturado");
            entity.Property(e => e.FechaHoraAutorizacion)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Autorizacion");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Captura");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.IdConcepto).HasColumnName("Id_Concepto");
            entity.Property(e => e.IdFarmaciaPedido)
                .HasMaxLength(6)
                .HasColumnName("Id_Farmacia_Pedido");
            entity.Property(e => e.IdMovimiento).HasColumnName("Id_Movimiento");
            entity.Property(e => e.IdUsuarioAutoriza)
                .HasMaxLength(15)
                .HasComment("La persona que autoriza o rechaza el traspaso")
                .HasColumnName("Id_Usuario_Autoriza");
            entity.Property(e => e.IdUsuarioCaptura)
                .HasMaxLength(6)
                .HasComment("La persona que captura la salida (tienda origen), Captura el traspaso (tienda destino)")
                .HasColumnName("Id_Usuario_Captura");
            entity.Property(e => e.IdfranPedido).HasColumnName("idfran_pedido");
            entity.Property(e => e.Referencia)
                .HasMaxLength(15)
                .HasDefaultValueSql("''");
            entity.Property(e => e.SincRef).HasMaxLength(64);
            entity.Property(e => e.Total)
                .HasPrecision(19, 4)
                .HasColumnName("total");
        });

        modelBuilder.Entity<InventarioTraspasoDetalle>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdFarmaciaEntrega, e.IdTraspaso, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("inventario_traspaso_detalle")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdFarmaciaEntrega)
                .HasMaxLength(6)
                .HasDefaultValueSql("''")
                .HasColumnName("Id_Farmacia_Entrega");
            entity.Property(e => e.IdTraspaso).HasColumnName("Id_Traspaso");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.Importe)
                .HasPrecision(19, 4)
                .HasColumnName("importe");
            entity.Property(e => e.Precio)
                .HasPrecision(19, 4)
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Operacion>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.FechaOperacion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("operacion")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.EstatusPv).HasColumnName("EstatusPV");
            entity.Property(e => e.FechaHoraApertura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Apertura");
            entity.Property(e => e.FechaHoraCierre)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Cierre");
            entity.Property(e => e.FechaHoraEnvio)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Envio");
            entity.Property(e => e.GranTotalApertura).HasPrecision(19, 4);
            entity.Property(e => e.GranTotalCierre).HasPrecision(19, 4);
            entity.Property(e => e.IdUsuarioApertura)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Apertura");
            entity.Property(e => e.IdUsuarioCajero)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Cajero");
            entity.Property(e => e.IdUsuarioCierre)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Cierre");
            entity.Property(e => e.IdUsuarioVendedor)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Vendedor");
        });

        modelBuilder.Entity<Operacionglobal>(entity =>
        {
            entity.HasKey(e => e.Idfran).HasName("PRIMARY");

            entity
                .ToTable("operacionglobal")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Idfran)
                .ValueGeneratedNever()
                .HasColumnName("idfran");
            entity.Property(e => e.Versiondb).HasColumnName("versiondb");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdPedido })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("pedido")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdPedido).HasColumnName("Id_Pedido");
            entity.Property(e => e.Estimado)
                .HasPrecision(19, 4)
                .HasColumnName("estimado");
            entity.Property(e => e.FechaHoraCaptura)
                .HasColumnType("datetime")
                .HasColumnName("FechaHora_Captura");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.FechaPedido).HasColumnType("datetime");
            entity.Property(e => e.Folioconfirmacion)
                .HasMaxLength(20)
                .HasColumnName("folioconfirmacion")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Foliopedido)
                .HasMaxLength(20)
                .HasColumnName("foliopedido")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.IdAlmacenSurtido)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("id_almacen_surtido")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.IdFinanciamiento).HasColumnName("id_financiamiento");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.Pedidoemergente).HasColumnName("pedidoemergente");
            entity.Property(e => e.Semanal).HasComment("1 Semanal, 0 Resurtido Emergente");
            entity.Property(e => e.Sincref)
                .HasMaxLength(40)
                .HasColumnName("sincref")
                .UseCollation("utf8_general_ci");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdPedido, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("pedido_detalle")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdPedido).HasColumnName("Id_Pedido");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.CostoUnitario).HasPrecision(19, 4);
            entity.Property(e => e.UltimaVenta).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProductoPorlistaprecio>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdListaprecio, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("producto_porlistaprecio")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => new { e.Idfran, e.IdProducto }, "idx_busqueda");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdListaprecio).HasColumnName("id_listaprecio");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("id_producto");
            entity.Property(e => e.Estatusregistro).HasColumnName("estatusregistro");
            entity.Property(e => e.Precio)
                .HasPrecision(19, 4)
                .HasColumnName("precio");
            entity.Property(e => e.Ultimaactualizacion)
                .HasComment("Fecha que actualiza syncro2")
                .HasColumnType("datetime")
                .HasColumnName("ultimaactualizacion");
        });

        modelBuilder.Entity<ProductoSuplementoFranquicium>(entity =>
        {
            entity.HasKey(e => new { e.IdFran, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("producto_suplemento_franquicia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.IdFran).HasColumnName("id_Fran");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(50)
                .HasColumnName("id_Producto");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Ultima_Actualizacion");
        });

        modelBuilder.Entity<Productofranquicium>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("productofranquicia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.DescripcionCorta)
                .HasMaxLength(20)
                .HasColumnName("Descripcion_Corta");
            entity.Property(e => e.FechaInclusion).HasColumnType("datetime");
            entity.Property(e => e.IdArticulo).HasColumnName("Id_Articulo");
            entity.Property(e => e.IdNivel1)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("Id_Nivel1");
            entity.Property(e => e.IdNivel2)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("Id_Nivel2");
            entity.Property(e => e.IdNivel3)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("Id_Nivel3")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.IdPresentacion).HasColumnName("Id_Presentacion");
            entity.Property(e => e.IdProductosat)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id_productosat");
            entity.Property(e => e.Ieps)
                .HasPrecision(10, 4)
                .HasColumnName("ieps");
            entity.Property(e => e.Iva)
                .HasPrecision(10, 4)
                .HasColumnName("IVA");
            entity.Property(e => e.MarcaEconomica).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Otc).HasColumnName("OTC");
            entity.Property(e => e.Precio).HasPrecision(19, 4);
            entity.Property(e => e.PrecioCompra).HasPrecision(19, 4);
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Ultima_Actualizacion");
            entity.Property(e => e.UltimoCosto).HasPrecision(19, 4);
        });

        modelBuilder.Entity<TomaTemperatura>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdToma })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("toma_temperatura")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdToma).HasColumnName("id_toma");
            entity.Property(e => e.Fechaoperacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaoperacion");
            entity.Property(e => e.HoraToma)
                .HasMaxLength(5)
                .HasColumnName("hora_toma");
            entity.Property(e => e.Humedad).HasColumnName("humedad");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Temperatura).HasColumnName("temperatura");
            entity.Property(e => e.UsuarioToma)
                .HasMaxLength(6)
                .HasColumnName("usuario_toma");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdUsuario })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("usuario")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.FechaUltimoCambio).HasPrecision(19, 6);
            entity.Property(e => e.IdIdioma).HasColumnName("Id_Idioma");
            entity.Property(e => e.LlaveAcceso).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<VentaDescuento>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo, e.IdDescuento })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

            entity
                .ToTable("venta_descuento")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.IdDescuento).HasColumnName("Id_Descuento");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(60)
                .HasColumnName("Cliente_Nombre");
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.DescuentoPorciento).HasPrecision(19, 4);
            entity.Property(e => e.DoctorCedula)
                .HasMaxLength(15)
                .HasColumnName("Doctor_Cedula");
            entity.Property(e => e.DoctorNombre)
                .HasMaxLength(60)
                .HasColumnName("Doctor_Nombre");
            entity.Property(e => e.Receta).HasMaxLength(15);
            entity.Property(e => e.Referencia).HasMaxLength(20);
            entity.Property(e => e.Tarjeta).HasMaxLength(30);
        });

        modelBuilder.Entity<VentaPago>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo, e.IdFormaPago })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

            entity
                .ToTable("venta_pago")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => new { e.Idfran, e.IdVenta }, "TendIdx");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.IdFormaPago)
                .HasMaxLength(6)
                .HasColumnName("Id_FormaPago");
            entity.Property(e => e.Importe).HasPrecision(19, 4);
            entity.Property(e => e.TipoCambio).HasPrecision(19, 4);
        });

        modelBuilder.Entity<VentaProducto>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

            entity
                .ToTable("venta_producto")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.IdProducto, "Venta_Producto");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.Cantidad).HasPrecision(19, 4);
            entity.Property(e => e.Descuento).HasPrecision(19, 4);
            entity.Property(e => e.DescuentoPorciento).HasPrecision(19, 15);
            entity.Property(e => e.Iva)
                .HasPrecision(10, 4)
                .HasColumnName("IVA");
            entity.Property(e => e.IvaImporte)
                .HasPrecision(19, 4)
                .HasColumnName("IVA_Importe");
            entity.Property(e => e.IvaPorciento)
                .HasPrecision(10, 4)
                .HasColumnName("IVA_Porciento");
            entity.Property(e => e.Precio).HasPrecision(19, 4);
        });

        modelBuilder.Entity<VentaProductoDesglose>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo, e.IdProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

            entity
                .ToTable("venta_producto_desglose")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => e.IdProducto, "idx_p");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.IdProducto)
                .HasMaxLength(10)
                .HasColumnName("Id_Producto");
            entity.Property(e => e.Cantidad).HasPrecision(19, 4);
            entity.Property(e => e.Importe).HasPrecision(19, 4);
            entity.Property(e => e.Iva)
                .HasPrecision(10, 4)
                .HasColumnName("IVA");
            entity.Property(e => e.Precio).HasPrecision(19, 4);
        });

        modelBuilder.Entity<VentaRecetaControlado>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.Fechacaptura })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("venta_receta_controlados")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Fechacaptura, "idxfc");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Cedula)
                .HasMaxLength(25)
                .HasColumnName("cedula");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(250)
                .HasColumnName("domicilio");
            entity.Property(e => e.Fechacaptura)
                .HasColumnType("datetime")
                .HasColumnName("fechacaptura");
            entity.Property(e => e.IdMedico).HasColumnName("id_medico");
            entity.Property(e => e.IdReceta)
                .HasMaxLength(20)
                .HasColumnName("id_receta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Retenerreceta)
                .HasColumnType("bit(1)")
                .HasColumnName("retenerreceta");
            entity.Property(e => e.Tipo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Ventafran>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.Mes, e.Ano })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("ventafran")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.Mes)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("mes");
            entity.Property(e => e.Ano)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("ano");
            entity.Property(e => e.Cepip)
                .HasPrecision(19, 4)
                .HasColumnName("cepip");
            entity.Property(e => e.Clientes)
                .HasDefaultValueSql("'0'")
                .HasColumnName("clientes");
            entity.Property(e => e.Conmedm)
                .HasDefaultValueSql("'0'")
                .HasColumnName("conmedm");
            entity.Property(e => e.Conmedn)
                .HasDefaultValueSql("'0'")
                .HasColumnName("conmedn");
            entity.Property(e => e.Conmedv)
                .HasDefaultValueSql("'0'")
                .HasColumnName("conmedv");
            entity.Property(e => e.Conmedx).HasColumnName("conmedx");
            entity.Property(e => e.Descuentos)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("descuentos");
            entity.Property(e => e.Gravados)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("gravados");
            entity.Property(e => e.Idcat)
                .HasDefaultValueSql("'0'")
                .HasColumnName("idcat");
            entity.Property(e => e.Iva)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("iva");
            entity.Property(e => e.IvaSuple)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("iva_suple");
            entity.Property(e => e.IvaSupleT)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("iva_suple_t");
            entity.Property(e => e.Naturistas)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("naturistas");
            entity.Property(e => e.PartVta)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("part_vta");
            entity.Property(e => e.ProdPremio)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("prodPremio");
            entity.Property(e => e.Similares)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("similares");
            entity.Property(e => e.Venta)
                .HasPrecision(19, 4)
                .HasColumnName("venta");
            entity.Property(e => e.VentaPe)
                .HasPrecision(19, 4)
                .HasColumnName("ventaPE");
            entity.Property(e => e.VentaPn)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("ventaPN");
            entity.Property(e => e.Vitaminas)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("vitaminas");
        });

        modelBuilder.Entity<Ventafrandium>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.Mes, e.Ano, e.Dia })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("ventafrandia")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.Mes)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("mes");
            entity.Property(e => e.Ano)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("ano");
            entity.Property(e => e.Dia).HasColumnName("dia");
            entity.Property(e => e.FechaCalculo).HasColumnType("datetime");
            entity.Property(e => e.Inventario)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("inventario");
            entity.Property(e => e.InventarioCompra)
                .HasPrecision(19, 4)
                .HasColumnName("inventarioCompra");
            entity.Property(e => e.Naturistas)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("naturistas");
            entity.Property(e => e.Venta)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("venta");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => new { e.Idfran, e.IdVenta, e.IdVentaLocal, e.IdVentaConsecutivo, e.FechaOperacion, e.TipoOperacion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("venta")
                .HasCharSet("utf8")
                .UseCollation("utf8_spanish_ci");

            entity.HasIndex(e => new { e.TipoOperacion, e.Estatus, e.FechaOperacion }, "idx_auxten");

            entity.Property(e => e.Idfran).HasColumnName("idfran");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.IdVentaLocal).HasColumnName("Id_Venta_Local");
            entity.Property(e => e.IdVentaConsecutivo).HasColumnName("Id_Venta_Consecutivo");
            entity.Property(e => e.FechaHoraCancelacion).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraCobro).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraVenta).HasColumnType("datetime");
            entity.Property(e => e.FechaOperacion).HasColumnType("datetime");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.IdMovimiento).HasColumnName("Id_Movimiento");
            entity.Property(e => e.IdRegistradoraCobro).HasColumnName("Id_Registradora_Cobro");
            entity.Property(e => e.IdRegistradoraVenta).HasColumnName("Id_Registradora_Venta");
            entity.Property(e => e.IdUsuarioCancelacion)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Cancelacion");
            entity.Property(e => e.IdUsuarioCobro)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Cobro");
            entity.Property(e => e.IdUsuarioVenta)
                .HasMaxLength(6)
                .HasColumnName("Id_Usuario_Venta");
            entity.Property(e => e.IdVentaReferencia)
                .HasMaxLength(13)
                .HasColumnName("Id_Venta_Referencia");
            entity.Property(e => e.IdVentaRegistradora).HasColumnName("Id_Venta_Registradora");
        });

        modelBuilder.Entity<CostoOp>(entity =>
        {
            entity.ToTable("erp_ci_costo_oportunidad");
            entity.HasKey(e => new { e.IdFran, e.IdProducto});
            entity.Property(e => e.IdFran).HasColumnName("Id_fran");
            entity.Property(e => e.Clave).HasMaxLength(6);
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.Producto).HasMaxLength(100);
            entity.Property(e => e.FechaOperacion).HasColumnName("Fecha_operacion");
            entity.Property(e => e.FechaTransmision).HasColumnName("FechaTransmision");

            for (int i = 1; i <= 7; i++)
            {
                entity.Property(typeof(int?), $"VentasDia{i}").HasColumnName($"VentasDia{i}");
                entity.Property(typeof(int?), $"PromDia{i}").HasColumnName($"PromDia{i}");
            }
        });

        modelBuilder.Entity<TvHashauditoria>()
            .HasKey(e => new { e.idfran, e.anio, e.mes, e.idOperacion });

        modelBuilder.Entity<ControlVersion>()
            .HasKey(cv => new { cv.IdSistema, cv.IdFran });

        modelBuilder.Entity<TvTransmision>()
            .HasKey(t => new { t.IdFran, t.IdOperacion });

        modelBuilder.Entity<TvLiberaciones>()
            .HasKey(l => new { l.IdFran, l.Clave, l.NombreSistema });

        modelBuilder.Entity<KardexControlMensual>(entity =>
        {
            entity.HasKey(e => new { e.IdFran, e.IdProducto, e.Mes, e.Periodo });

            foreach (var property in entity.Metadata.GetProperties())
            {
                if (property.ClrType == typeof(decimal))
                {
                    property.SetPrecision(19);
                    property.SetScale(2);
                }
            }
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
