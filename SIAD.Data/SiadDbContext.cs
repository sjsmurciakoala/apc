using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SIAD.Core.Entities;

namespace SIAD.Data;

public partial class SiadDbContext : DbContext
{
    public SiadDbContext(DbContextOptions<SiadDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<barrio> barrios { get; set; }

    public virtual DbSet<categoria_servicio> categoria_servicios { get; set; }

    public virtual DbSet<ciclo> ciclos { get; set; }

    public virtual DbSet<cliente_detalle> cliente_detalles { get; set; }

    public virtual DbSet<cliente_maestro> cliente_maestros { get; set; }

    public virtual DbSet<configuracion_tasa> configuracion_tasas { get; set; }

    public virtual DbSet<configuracion_tasas_detalle> configuracion_tasas_detalles { get; set; }

    public virtual DbSet<historialme> historialmes { get; set; }

    public virtual DbSet<historicomedicion> historicomedicions { get; set; }

    public virtual DbSet<historicosinmedidor> historicosinmedidors { get; set; }

    public virtual DbSet<maestro_medidor> maestro_medidors { get; set; }

    public virtual DbSet<servicio> servicios { get; set; }

    public virtual DbSet<tarifas_catalogo> tarifas_catalogos { get; set; }

    public virtual DbSet<tarifas_contador> tarifas_contadors { get; set; }

    public virtual DbSet<tipo_uso_servicio> tipo_uso_servicios { get; set; }

    public virtual DbSet<transaccion_abonado> transaccion_abonados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<barrio>(entity =>
        {
            entity.HasKey(e => e.barrio_codigo).HasName("barrio_pkey");

            entity.ToTable("barrio");

            entity.Property(e => e.barrio_codigo).HasMaxLength(3);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<categoria_servicio>(entity =>
        {
            entity.HasKey(e => e.categoria_servicio_id).HasName("categoria_servicio_pkey");

            entity.ToTable("categoria_servicio");

            entity.Property(e => e.categoria_servicio_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(9L, null, null, null, null, null);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<ciclo>(entity =>
        {
            entity.HasKey(e => e.ciclos_id).HasName("ciclos_id_pkey");

            entity.Property(e => e.ciclos_id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ciclos_codigo).HasMaxLength(50);
            entity.Property(e => e.ciclos_descripcioncorta).HasMaxLength(100);
            entity.Property(e => e.ciclos_descripcionlarga).HasMaxLength(300);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<cliente_detalle>(entity =>
        {
            entity.HasKey(e => e.detalle_cliente_id).HasName("cliente_detalle_pkey");

            entity.ToTable("cliente_detalle");

            entity.Property(e => e.detalle_cliente_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(46839L, null, null, null, null, null);
            entity.Property(e => e.clave).HasMaxLength(32767);
            entity.Property(e => e.descuento_valor).HasPrecision(18, 2);
            entity.Property(e => e.detalle_cliente_color_casa).HasMaxLength(100);
            entity.Property(e => e.detalle_cliente_direccion).HasMaxLength(200);
            entity.Property(e => e.detalle_cliente_email).HasMaxLength(100);
            entity.Property(e => e.detalle_cliente_inquilino).HasMaxLength(100);
            entity.Property(e => e.detalle_cliente_movil).HasMaxLength(15);
            entity.Property(e => e.detalle_cliente_telefono).HasMaxLength(15);
            entity.Property(e => e.empresa_direccion).HasMaxLength(200);
            entity.Property(e => e.empresa_nombre).HasMaxLength(100);
            entity.Property(e => e.empresa_telefono).HasMaxLength(50);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.negocio_clave_catastral).HasMaxLength(50);
            entity.Property(e => e.negocio_nombre).HasMaxLength(100);
            entity.Property(e => e.negocio_telefono).HasMaxLength(15);
            entity.Property(e => e.numero_contrato).HasColumnType("character varying");
            entity.Property(e => e.observaciones).HasColumnType("character varying");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);

            entity.HasOne(d => d.maestro_cliente).WithMany(p => p.cliente_detalles)
                .HasForeignKey(d => d.maestro_cliente_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maestro_cliente_id_cliente_detalle_fkey");

            entity.HasOne(d => d.maestro_medidor).WithMany(p => p.cliente_detalles)
                .HasForeignKey(d => d.maestro_medidor_id)
                .HasConstraintName("maestro_medidor_id_cliente_detalle_fkey");
        });

        modelBuilder.Entity<cliente_maestro>(entity =>
        {
            entity.HasKey(e => e.maestro_cliente_id).HasName("cliente_maestro_pkey");

            entity.ToTable("cliente_maestro");

            entity.HasIndex(e => e.maestro_cliente_clave, "cliente_maestro_unique").IsUnique();

            entity.Property(e => e.maestro_cliente_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(102789L, null, null, null, null, null);
            entity.Property(e => e.barrio_codigo).HasMaxLength(5);
            entity.Property(e => e.bloqueado_cobranza).HasDefaultValue(false);
            entity.Property(e => e.clave_sure).HasMaxLength(40);
            entity.Property(e => e.cliente_fecha_nac).HasColumnType("timestamp without time zone");
            entity.Property(e => e.contador).HasMaxLength(50);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.letracodigo).HasMaxLength(10);
            entity.Property(e => e.maestro_cliente_clave).HasMaxLength(20);
            entity.Property(e => e.maestro_cliente_fecha_baja).HasColumnType("timestamp without time zone");
            entity.Property(e => e.maestro_cliente_indicativo_ruta).HasMaxLength(25);
            entity.Property(e => e.maestro_cliente_secuencia).HasMaxLength(6);
            entity.Property(e => e.maestro_cliente_tiene_medidor).HasDefaultValue(false);
            entity.Property(e => e.tipo_uso_codigo).HasMaxLength(2);
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);

            entity.HasOne(d => d.barrio_codigoNavigation).WithMany(p => p.cliente_maestros)
                .HasForeignKey(d => d.barrio_codigo)
                .HasConstraintName("barrio_codigo_cliente_maestro_fkey");

            entity.HasOne(d => d.categoria_servicio).WithMany(p => p.cliente_maestros)
                .HasForeignKey(d => d.categoria_servicio_id)
                .HasConstraintName("categoria_servicio_id_cliente_maestro_fkey");

            entity.HasOne(d => d.ciclos).WithMany(p => p.cliente_maestros)
                .HasForeignKey(d => d.ciclos_id)
                .HasConstraintName("ciclos_id_cliente_maestro_fkey");

            entity.HasOne(d => d.tipo_uso_codigoNavigation).WithMany(p => p.cliente_maestros)
                .HasForeignKey(d => d.tipo_uso_codigo)
                .HasConstraintName("tipo_uso_codigo_cliente_maestro_fkey");
        });

        modelBuilder.Entity<configuracion_tasa>(entity =>
        {
            entity.HasKey(e => e.configuracion_tasas_id).HasName("configuracion_tasas_id_pkey");

            entity.HasIndex(e => new { e.maestro_cliente_id, e.tarifa_catalogo_id }, "ix_configuracion_tasas_cliente_tarifa");

            entity.Property(e => e.configuracion_tasas_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(38784L, null, null, null, null, null);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);

            entity.HasOne(d => d.maestro_cliente).WithMany(p => p.configuracion_tasas)
                .HasForeignKey(d => d.maestro_cliente_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("maestro_cliente_id_fkey");

            entity.HasOne(d => d.tarifa_catalogo).WithMany(p => p.configuracion_tasas)
                .HasForeignKey(d => d.tarifa_catalogo_id)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_configuracion_tasas_tarifas_catalogo");
        });

        modelBuilder.Entity<configuracion_tasas_detalle>(entity =>
        {
            entity.HasKey(e => e.configuracion_tasas_detalle_id).HasName("configuracion_tasas_detalle_id_pkey");

            entity.ToTable("configuracion_tasas_detalle");

            entity.Property(e => e.configuracion_tasas_detalle_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(149941L, null, null, null, null, null);
            entity.Property(e => e.configuracion_tasas_detalle_monto).HasPrecision(11, 4);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);

            entity.HasOne(d => d.configuracion_tasas).WithMany(p => p.configuracion_tasas_detalles)
                .HasForeignKey(d => d.configuracion_tasas_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("configurcion_tasas_id_fkey");
        });

        modelBuilder.Entity<historialme>(entity =>
        {
            entity.HasKey(e => new { e.ano, e.mes, e.ciclo }).HasName("historialmes_pkey");

            entity.Property(e => e.ano).HasPrecision(4);
            entity.Property(e => e.mes).HasPrecision(2);
            entity.Property(e => e.ciclo)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e._2_Sep)
                .HasMaxLength(32767)
                .HasColumnName("2-Sep");
            entity.Property(e => e.cerrado)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.cerrarperiodo)
                .HasMaxLength(1)
                .HasDefaultValueSql("'P'::bpchar");
            entity.Property(e => e.fecha).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechaperiodo).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ruta)
                .HasMaxLength(10)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.sep)
                .HasPrecision(1)
                .HasDefaultValueSql("NULL::numeric");
            entity.Property(e => e.sep2)
                .HasPrecision(1)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.usuarioapertura)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.usuariocierre)
                .HasMaxLength(20)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
        });

        modelBuilder.Entity<historicomedicion>(entity =>
        {
            entity.HasKey(e => e.ide).HasName("historicomedicion_pkey");

            entity.ToTable("historicomedicion");

            entity.Property(e => e.ide)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(1119616L, null, null, null, null, null);
            entity.Property(e => e.ajuste)
                .HasMaxLength(2)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength();
            entity.Property(e => e.ano).HasPrecision(4);
            entity.Property(e => e.categoria)
                .HasMaxLength(1)
                .HasDefaultValueSql("'0'::bpchar");
            entity.Property(e => e.categoriacliente)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.cerrado)
                .HasMaxLength(1)
                .HasDefaultValueSql("'0'::bpchar");
            entity.Property(e => e.ciclo)
                .HasMaxLength(2)
                .HasDefaultValueSql("'0'::bpchar")
                .IsFixedLength();
            entity.Property(e => e.clave)
                .HasMaxLength(15)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.codinfo)
                .HasMaxLength(2)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength();
            entity.Property(e => e.comentario)
                .HasMaxLength(145)
                .HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.condicion)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.consumo)
                .HasPrecision(12)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.consumoant)
                .HasPrecision(12)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.contador)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.descuentoapp)
                .HasPrecision(14, 2)
                .HasDefaultValueSql("NULL::numeric");
            entity.Property(e => e.lec_prom)
                .HasPrecision(12)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.lect_act)
                .HasPrecision(12)
                .HasDefaultValueSql("NULL::numeric");
            entity.Property(e => e.lect_ant)
                .HasPrecision(12)
                .HasDefaultValueSql("NULL::numeric");
            entity.Property(e => e.mes).HasPrecision(2);
            entity.Property(e => e.mes2)
                .HasMaxLength(2)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength();
            entity.Property(e => e.numerofactura)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.numeroord)
                .HasPrecision(12)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.observacion)
                .HasMaxLength(300)
                .HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.orden)
                .HasMaxLength(1)
                .HasDefaultValueSql("'0'::bpchar");
            entity.Property(e => e.otros)
                .HasMaxLength(20)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.propietario)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.revision)
                .HasPrecision(1)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.revision2)
                .HasMaxLength(1)
                .HasDefaultValueSql("'0'::bpchar");
            entity.Property(e => e.ruta)
                .HasMaxLength(25)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength();
            entity.Property(e => e.secuencia)
                .HasMaxLength(9)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.sel)
                .HasPrecision(1)
                .HasDefaultValueSql("'0'::numeric");
            entity.Property(e => e.ser1)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser10)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser2)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser3)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser4)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser5)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser6)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser7)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser8)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.ser9)
                .HasMaxLength(1)
                .HasDefaultValueSql("NULL::bpchar");
            entity.Property(e => e.taservi1)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi10)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi2)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi3)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi4)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi5)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi6)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi7)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi8)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.taservi9)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00");
            entity.Property(e => e.tipolectura)
                .HasMaxLength(1)
                .HasDefaultValueSql("''::bpchar");
            entity.Property(e => e.tservi1)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi10)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi2)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi3)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi4)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi5)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi6)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi7)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi8)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.tservi9)
                .HasMaxLength(3)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.ubicacion)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::bpchar")
                .IsFixedLength();
            entity.Property(e => e.usuario)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying");
        });

        modelBuilder.Entity<historicosinmedidor>(entity =>
        {
            entity.HasKey(e => e.ide).HasName("historicosinmedidor_pkey");

            entity.ToTable("historicosinmedidor");

            entity.Property(e => e.ide).UseIdentityAlwaysColumn();
            entity.Property(e => e.cuenta).HasMaxLength(15);
            entity.Property(e => e.fecha).HasColumnType("timestamp without time zone");
            entity.Property(e => e.numerofactura).HasMaxLength(50);
            entity.Property(e => e.usuario).HasMaxLength(50);
        });

        modelBuilder.Entity<maestro_medidor>(entity =>
        {
            entity.HasKey(e => e.maestro_medidor_id).HasName("maestro_medidor_id_pkey");

            entity.ToTable("maestro_medidor");

            entity.Property(e => e.maestro_medidor_id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(23718L, null, null, null, null, null);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.maestro_medidor_acueducto).HasMaxLength(20);
            entity.Property(e => e.maestro_medidor_diametro).HasPrecision(4, 2);
            entity.Property(e => e.maestro_medidor_empleado).HasMaxLength(50);
            entity.Property(e => e.maestro_medidor_fecha_instala).HasColumnType("timestamp without time zone");
            entity.Property(e => e.maestro_medidor_marca).HasMaxLength(50);
            entity.Property(e => e.maestro_medidor_numero).HasMaxLength(50);
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<servicio>(entity =>
        {
            entity.HasKey(e => e.servicios_id).HasName("servicios_id_pkey");

            entity.Property(e => e.servicios_id).UseIdentityAlwaysColumn();
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.servicios_codigo).HasMaxLength(50);
            entity.Property(e => e.servicios_descripcioncorta).HasMaxLength(100);
            entity.Property(e => e.servicios_descripcionlarga).HasMaxLength(300);
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<tarifas_catalogo>(entity =>
        {
            entity.HasKey(e => e.tarifa_catalogo_id).HasName("tarifas_catalogo_pkey");

            entity.ToTable("tarifas_catalogo");

            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.cargo_fijo).HasPrecision(11, 4);
            entity.Property(e => e.descripcion).HasMaxLength(250);
            entity.Property(e => e.fecha_creacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.fecha_modificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.nombre).HasMaxLength(120);
            entity.Property(e => e.precio_base).HasPrecision(11, 4);
        });

        modelBuilder.Entity<tarifas_contador>(entity =>
        {
            entity.HasKey(e => e.ide).HasName("tarifas_contador_pkey");

            entity.ToTable("tarifas_contador");

            entity.Property(e => e.ide)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(17L, null, null, null, null, null);
            entity.Property(e => e.alquiler).HasPrecision(12, 2);
            entity.Property(e => e.codigo).HasColumnType("character varying");
            entity.Property(e => e.cuota).HasPrecision(12, 2);
            entity.Property(e => e.descripcion).HasColumnType("character varying");
            entity.Property(e => e.maximo).HasPrecision(12);
            entity.Property(e => e.minimo).HasPrecision(12);
            entity.Property(e => e.valor_base).HasPrecision(12, 2);
        });

        modelBuilder.Entity<tipo_uso_servicio>(entity =>
        {
            entity.HasKey(e => e.tipo_uso_codigo).HasName("tipo_uso_codigo_pkey");

            entity.ToTable("tipo_uso_servicio");

            entity.Property(e => e.tipo_uso_codigo).HasMaxLength(2);
            entity.Property(e => e.fechacreacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.fechamodificacion).HasColumnType("timestamp without time zone");
            entity.Property(e => e.usuariocreacion).HasMaxLength(256);
            entity.Property(e => e.usuariomodificacion).HasMaxLength(256);
        });

        modelBuilder.Entity<transaccion_abonado>(entity =>
        {
            entity.HasKey(e => e.ide).HasName("transaccion_abonado_pkey");

            entity.ToTable("transaccion_abonado");

            entity.Property(e => e.ide)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(3075446L, null, null, null, null, null);
            entity.Property(e => e.aplicar_alca).HasColumnType("character varying");
            entity.Property(e => e.banco).HasColumnType("character varying");
            entity.Property(e => e.ciclo).HasColumnType("character varying");
            entity.Property(e => e.cliente_clave).HasColumnType("character varying");
            entity.Property(e => e.codigoplan).HasColumnType("character varying");
            entity.Property(e => e.creditos).HasPrecision(12, 2);
            entity.Property(e => e.debitos).HasPrecision(12, 2);
            entity.Property(e => e.descripcion).HasColumnType("character varying");
            entity.Property(e => e.docufuente2).HasColumnType("character varying");
            entity.Property(e => e.estado).HasColumnType("character varying");
            entity.Property(e => e.motivo).HasColumnType("character varying");
            entity.Property(e => e.periodo).HasColumnType("character varying");
            entity.Property(e => e.ruta).HasColumnType("character varying");
            entity.Property(e => e.saldo).HasPrecision(12, 2);
            entity.Property(e => e.saldo_detalle).HasPrecision(18, 2);
            entity.Property(e => e.secuencia).HasColumnType("character varying");
            entity.Property(e => e.tasa).HasColumnType("character varying");
            entity.Property(e => e.tiene_med).HasColumnType("character varying");
            entity.Property(e => e.tipo_partida).HasColumnType("character varying");
            entity.Property(e => e.tipo_servicio).HasColumnType("character varying");
            entity.Property(e => e.tipotransaccion).HasColumnType("character varying");
            entity.Property(e => e.trans_aplicar).HasColumnType("character varying");
            entity.Property(e => e.usuario).HasColumnType("character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
