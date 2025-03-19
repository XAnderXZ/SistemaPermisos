﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaPermisos.Data;

#nullable disable

namespace SistemaPermisos.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaPermisos.Models.OmisionMarca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaOmision")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoOmision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("OmisionesMarca");
                });

            modelBuilder.Entity("SistemaPermisos.Models.Permiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaRegreso")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaSalida")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaSolicitud")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Justificado")
                        .HasColumnType("bit");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RutaComprobante")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RutaJustificacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Permisos");
                });

            modelBuilder.Entity("SistemaPermisos.Models.ReporteDano", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaReporte")
                        .HasColumnType("datetime2");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ReportesDanos");
                });

            modelBuilder.Entity("SistemaPermisos.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Departamento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("FotoPerfil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UltimaActualizacion")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Correo = "admin@escuela.edu",
                            FechaRegistro = new DateTime(2025, 3, 18, 21, 53, 21, 367, DateTimeKind.Local).AddTicks(1136),
                            Nombre = "Administrador",
                            Password = "$2a$11$p.qAmUsgX/bptfWmOLS4zO0N73DYb6zTO5USfE3hoMY16riDLxl/W",
                            Rol = "Admin",
                            UltimaActualizacion = new DateTime(2025, 3, 18, 21, 53, 21, 367, DateTimeKind.Local).AddTicks(1153)
                        },
                        new
                        {
                            Id = 2,
                            Correo = "docente@escuela.edu",
                            FechaRegistro = new DateTime(2025, 3, 18, 21, 53, 21, 495, DateTimeKind.Local).AddTicks(8378),
                            Nombre = "Docente Demo",
                            Password = "$2a$11$VufOGrj1aYRUAOjqbYaDpuu2cJI4G8t7Zk/duAbtD.TNejBNZUdgi",
                            Rol = "Docente",
                            UltimaActualizacion = new DateTime(2025, 3, 18, 21, 53, 21, 495, DateTimeKind.Local).AddTicks(8393)
                        });
                });

            modelBuilder.Entity("SistemaPermisos.Models.OmisionMarca", b =>
                {
                    b.HasOne("SistemaPermisos.Models.Usuario", "Usuario")
                        .WithMany("OmisionesMarca")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SistemaPermisos.Models.Permiso", b =>
                {
                    b.HasOne("SistemaPermisos.Models.Usuario", "Usuario")
                        .WithMany("Permisos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SistemaPermisos.Models.ReporteDano", b =>
                {
                    b.HasOne("SistemaPermisos.Models.Usuario", "Usuario")
                        .WithMany("ReportesDanos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SistemaPermisos.Models.Usuario", b =>
                {
                    b.Navigation("OmisionesMarca");

                    b.Navigation("Permisos");

                    b.Navigation("ReportesDanos");
                });
#pragma warning restore 612, 618
        }
    }
}
