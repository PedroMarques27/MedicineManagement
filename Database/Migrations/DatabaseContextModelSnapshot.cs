﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [ExcludeFromCodeCoverage]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Database.Models.MedicineModel", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Name");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("Database.Models.PrescriptionMedicineModel", b =>
                {
                    b.Property<Guid>("PrescriptionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("MedicineName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("PrescriptionId", "MedicineName");

                    b.HasIndex("MedicineName");

                    b.ToTable("PrescriptionMedicineModel");
                });

            modelBuilder.Entity("Database.Models.PrescriptionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserEmail");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("Database.Models.UserModel", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Database.Models.PrescriptionMedicineModel", b =>
                {
                    b.HasOne("Database.Models.MedicineModel", "Medicine")
                        .WithMany("PrescriptionMedicines")
                        .HasForeignKey("MedicineName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.PrescriptionModel", "PrescriptionModel")
                        .WithMany("MedicineList")
                        .HasForeignKey("PrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("PrescriptionModel");
                });

            modelBuilder.Entity("Database.Models.PrescriptionModel", b =>
                {
                    b.HasOne("Database.Models.UserModel", "User")
                        .WithMany("PrescriptionList")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Database.Models.MedicineModel", b =>
                {
                    b.Navigation("PrescriptionMedicines");
                });

            modelBuilder.Entity("Database.Models.PrescriptionModel", b =>
                {
                    b.Navigation("MedicineList");
                });

            modelBuilder.Entity("Database.Models.UserModel", b =>
                {
                    b.Navigation("PrescriptionList");
                });
#pragma warning restore 612, 618
        }
    }
}
