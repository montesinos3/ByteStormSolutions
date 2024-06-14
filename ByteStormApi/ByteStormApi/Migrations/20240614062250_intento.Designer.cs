﻿// <auto-generated />
using System;
using ByteStormApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ByteStormApi.Migrations
{
    [DbContext(typeof(ByteStormContext))]
    [Migration("20240614062250_intento")]
    partial class intento
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("ByteStormApi.Models.Equipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<long?>("IdMision")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdMision");

                    b.ToTable("Equipos");
                });

            modelBuilder.Entity("ByteStormApi.Models.Mision", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .HasColumnType("TEXT");

                    b.Property<long?>("IdOperativo")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IdOperativo");

                    b.ToTable("Misiones");
                });

            modelBuilder.Entity("ByteStormApi.Models.Operativo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nombre")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rol")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Operativos");
                });

            modelBuilder.Entity("ByteStormApi.Models.Equipo", b =>
                {
                    b.HasOne("ByteStormApi.Models.Mision", "Mision")
                        .WithMany("Equipos")
                        .HasForeignKey("IdMision");

                    b.Navigation("Mision");
                });

            modelBuilder.Entity("ByteStormApi.Models.Mision", b =>
                {
                    b.HasOne("ByteStormApi.Models.Operativo", "Operativo")
                        .WithMany("Misiones")
                        .HasForeignKey("IdOperativo");

                    b.Navigation("Operativo");
                });

            modelBuilder.Entity("ByteStormApi.Models.Mision", b =>
                {
                    b.Navigation("Equipos");
                });

            modelBuilder.Entity("ByteStormApi.Models.Operativo", b =>
                {
                    b.Navigation("Misiones");
                });
#pragma warning restore 612, 618
        }
    }
}
