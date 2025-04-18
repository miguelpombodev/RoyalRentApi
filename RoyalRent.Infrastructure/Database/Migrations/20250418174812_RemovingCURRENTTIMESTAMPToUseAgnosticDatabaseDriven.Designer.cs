﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RoyalRent.Infrastructure.Database;

#nullable disable

namespace RoyalRent.Infrastructure.Database.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20250418174812_RemovingCURRENTTIMESTAMPToUseAgnosticDatabaseDriven")]
    partial class RemovingCURRENTTIMESTAMPToUseAgnosticDatabaseDriven
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RoyalRent.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("CHAR(11)")
                        .HasColumnName("CPF");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("created_on");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("email");

                    b.Property<char>("Gender")
                        .HasColumnType("CHAR(1)")
                        .HasColumnName("gender");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(650)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("name");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("CHAR(12)")
                        .HasColumnName("phone");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("updated_on");

                    b.HasKey("Id");

                    b.HasIndex("Email", "Cpf", "Telephone");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RoyalRent.Domain.Entities.UserAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("address");

                    b.Property<string>("Cep")
                        .HasColumnType("CHAR(8)")
                        .HasColumnName("CEP");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("city");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("created_on");

                    b.Property<string>("FederativeUnit")
                        .HasColumnType("CHAR(2)")
                        .HasColumnName("UF");

                    b.Property<string>("Neighborhood")
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("neighborhood");

                    b.Property<string>("Number")
                        .HasMaxLength(5)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("number");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("updated_on");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("UUID")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("Cep", "Address", "City", "Id");

                    b.ToTable("UsersAddresses", (string)null);
                });

            modelBuilder.Entity("RoyalRent.Domain.Entities.UserDriverLicense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("DATE")
                        .HasColumnName("birthdate");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("created_on");

                    b.Property<DateOnly?>("DocumentExpirationDate")
                        .HasColumnType("DATE")
                        .HasColumnName("document_expiration_date");

                    b.Property<string>("DriverLicenseNumber")
                        .HasColumnType("CHAR(9)")
                        .HasColumnName("CNH");

                    b.Property<string>("RG")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("CHAR(2)")
                        .HasColumnName("state");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TIMESTAMP WITH TIME ZONE")
                        .HasColumnName("updated_on");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("UUID")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UsersDriverLicenses", (string)null);
                });

            modelBuilder.Entity("RoyalRent.Domain.Entities.UserAddress", b =>
                {
                    b.HasOne("RoyalRent.Domain.Entities.User", "User")
                        .WithOne("UserAddress")
                        .HasForeignKey("RoyalRent.Domain.Entities.UserAddress", "UserId")
                        .HasConstraintName("FK_USER_USER_ADDRESSES");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RoyalRent.Domain.Entities.UserDriverLicense", b =>
                {
                    b.HasOne("RoyalRent.Domain.Entities.User", "User")
                        .WithOne("UserDriverLicense")
                        .HasForeignKey("RoyalRent.Domain.Entities.UserDriverLicense", "UserId")
                        .HasConstraintName("FK_USER_USER_LICENSE");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RoyalRent.Domain.Entities.User", b =>
                {
                    b.Navigation("UserAddress");

                    b.Navigation("UserDriverLicense");
                });
#pragma warning restore 612, 618
        }
    }
}
