using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DesarrolloAprendeLibre.Models;

public partial class AplDbContext : DbContext
{
    public AplDbContext()
    {
    }

    public AplDbContext(DbContextOptions<AplDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Clase> Clases { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Moderador> Moderadors { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ADMINISTRADOR");

            entity.Property(e => e.Clave)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__CATEGORI__A3C02A108360E812");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.IdClase).HasName("PK__CLASE__003FCC6FCA224734");

            entity.ToTable("CLASE");

            entity.Property(e => e.Imagen).HasMaxLength(250);
            entity.Property(e => e.Materia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubirArchivo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.IdLibro).HasName("PK__LIBROS__3E0B49AD609EA85A");

            entity.ToTable("LIBROS");

            entity.Property(e => e.Autor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imagen).HasMaxLength(250);
            entity.Property(e => e.NombrePortada)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LIBROS__IdCatego__5629CD9C");
        });

        modelBuilder.Entity<Moderador>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MODERADOR");

            entity.Property(e => e.Clave)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.IdRecurso).HasName("PK__RECURSOS__B91948E9E5DDF57E");

            entity.ToTable("RECURSOS");

            entity.Property(e => e.Imagen).HasMaxLength(250);
            entity.Property(e => e.Materia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__ROLES__2A49584CC6DA0553");

            entity.ToTable("ROLES");

            entity.Property(e => e.NombreRol)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIO__5B65BF974CA1A883");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__USUARIO__IdRol__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
