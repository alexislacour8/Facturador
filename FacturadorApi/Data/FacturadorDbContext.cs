using System;
using System.Collections.Generic;
using FacturadorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturadorApi.Data;

public partial class FacturadorDbContext : DbContext
{
    public FacturadorDbContext()
    {
    }

    public FacturadorDbContext(DbContextOptions<FacturadorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Factura_Cabecera> Factura_Cabeceras { get; set; }

    public virtual DbSet<Factura_Detalle> Factura_Detalles { get; set; }

    public virtual DbSet<vw_Factura_Cliente> vw_Factura_Clientes { get; set; }

    public virtual DbSet<vw_Factura_Resuman> vw_Factura_Resumen { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=FacturadorDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.ART_ID).HasName("PK__Articulo__FCD631073C2B2A6B");

            entity.ToTable("Articulo");

            entity.Property(e => e.ART_ID).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Stock).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Cli_ID).HasName("PK__Cliente__83531C5AED7604DF");

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.CUIT, "UQ__Cliente__F46C15988F9158F2").IsUnique();

            entity.Property(e => e.CUIT).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.RazonSocial).HasMaxLength(255);
        });

        modelBuilder.Entity<Factura_Cabecera>(entity =>
        {
            entity.HasKey(e => e.FC_ID).HasName("PK__Factura___20E541923A7AB0EE");

            entity.ToTable("Factura_Cabecera");

            entity.Property(e => e.Estado).HasMaxLength(50);

            entity.HasOne(d => d.Cli).WithMany(p => p.Factura_Cabeceras)
                .HasForeignKey(d => d.Cli_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FC_Cliente");
        });

        modelBuilder.Entity<Factura_Detalle>(entity =>
        {
            entity.HasKey(e => new { e.Fact_ID, e.FC_DTL_ID });

            entity.ToTable("Factura_Detalle");

            entity.Property(e => e.FC_DTL_ID).ValueGeneratedOnAdd();
            entity.Property(e => e.ART_ID).HasMaxLength(50);
            entity.Property(e => e.Cant).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ART).WithMany(p => p.Factura_Detalles)
                .HasForeignKey(d => d.ART_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FD_ART");

            entity.HasOne(d => d.Fact).WithMany(p => p.Factura_Detalles)
                .HasForeignKey(d => d.Fact_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FD_FC");
        });

        modelBuilder.Entity<vw_Factura_Cliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Factura_Cliente");

            entity.Property(e => e.CUIT).HasMaxLength(50);
            entity.Property(e => e.RazonSocial).HasMaxLength(255);
        });

        modelBuilder.Entity<vw_Factura_Resuman>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Factura_Resumen");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.TotalFactura).HasColumnType("decimal(38, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
