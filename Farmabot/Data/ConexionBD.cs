using System;
using System.Collections.Generic;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Data;

public partial class ConexionBD : DbContext
{
    public ConexionBD(DbContextOptions<ConexionBD> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<LotesMedicamento> LotesMedicamentos { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<MovimientosStock> MovimientosStocks { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("clientes");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true, "DF_clientes_activo")
                .HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<LotesMedicamento>(entity =>
        {
            entity.HasKey(e => e.IdLote);

            entity.ToTable("lotes_medicamentos");

            entity.HasIndex(e => new { e.IdMedicamento, e.FechaVencimiento }, "IX_lotes_medicamento_vencimiento");

            entity.HasIndex(e => e.FechaVencimiento, "IX_lotes_vencimiento");

            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.FechaIngreso)
                .HasDefaultValueSql("(getdate())", "DF_lotes_fecha_ingreso")
                .HasColumnType("datetime")
                .HasColumnName("fecha_ingreso");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdMedicamento).HasColumnName("id_medicamento");
            entity.Property(e => e.NumeroLote)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero_lote");
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_compra");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.LotesMedicamentos)
                .HasForeignKey(d => d.IdMedicamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lotes_medicamentos_medicamentos");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.IdMedicamento);

            entity.ToTable("medicamentos");

            entity.HasIndex(e => e.IdCategoria, "IX_medicamentos_categoria");

            entity.Property(e => e.IdMedicamento).HasColumnName("id_medicamento");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true, "DF_medicamentos_activo")
                .HasColumnName("activo");
            entity.Property(e => e.Concentracion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("concentracion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Laboratorio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("laboratorio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_venta");
            entity.Property(e => e.Presentacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("presentacion");
            entity.Property(e => e.PrincipioActivo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("principio_activo");
            entity.Property(e => e.RequiereReceta).HasColumnName("requiere_receta");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_medicamentos_categorias");
        });

        modelBuilder.Entity<MovimientosStock>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento);

            entity.ToTable("movimientos_stock");

            entity.HasIndex(e => new { e.IdLote, e.Fecha }, "IX_movimientos_lote_fecha");

            entity.HasIndex(e => e.IdUsuario, "IX_movimientos_usuario");

            entity.HasIndex(e => e.IdVenta, "IX_movimientos_venta");

            entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())", "DF_movimientos_fecha")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("motivo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_movimientos_stock_lotes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_movimientos_stock_usuarios");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_movimientos_stock_ventas");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ_usuarios_email").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true, "DF_usuarios_activo")
                .HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.ContrasenaHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena_hash");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.ToTable("ventas");

            entity.HasIndex(e => e.IdCliente, "IX_ventas_cliente");

            entity.HasIndex(e => new { e.IdUsuario, e.Fecha }, "IX_ventas_usuario_fecha");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())", "DF_ventas_fecha")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_ventas_clientes");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ventas_usuarios");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdDetalle);

            entity.ToTable("venta_detalle");

            entity.HasIndex(e => e.IdLote, "IX_detalle_lote");

            entity.HasIndex(e => e.IdVenta, "IX_detalle_venta");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_venta_detalle_lotes");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_venta_detalle_ventas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
