﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RecoverUnsoldApi.Data;

#nullable disable

namespace RecoverUnsoldApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220730083704_CreateReviewsTable")]
    partial class CreateReviewsTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Alert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AlertType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Trigger")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("CustomerId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.EmailVerification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("EmailVerifications");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.FcmToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("UserId");

                    b.ToTable("FcmTokens");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("ProductId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Point>("Coordinates")
                        .IsRequired()
                        .HasColumnType("geography (point)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DistributorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Indication")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("DistributorId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("Beneficiaries")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DistributorId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Duration")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("DistributorId");

                    b.HasIndex("LocationId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Opinion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("OrderId");

                    b.ToTable("Opinions");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("WithdrawalDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OfferId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.PasswordReset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("PasswordResets");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("OfferId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("CustomerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("EmailVerifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Customer", b =>
                {
                    b.HasBaseType("RecoverUnsoldApi.Entities.User");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Distributor", b =>
                {
                    b.HasBaseType("RecoverUnsoldApi.Entities.User");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Rccm")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TaxId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.HasIndex("Rccm")
                        .IsUnique();

                    b.HasIndex("TaxId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Distributor");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Alert", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Customer", "Customer")
                        .WithMany("Alerts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.FcmToken", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.User", null)
                        .WithMany("FcmTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Image", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Location", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Distributor", "Distributor")
                        .WithMany("Locations")
                        .HasForeignKey("DistributorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Offer", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Distributor", "Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecoverUnsoldApi.Entities.Location", "Location")
                        .WithMany("Offers")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Opinion", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Order", "Order")
                        .WithMany("Opinions")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Order", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecoverUnsoldApi.Entities.Offer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Product", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Offer", "Offer")
                        .WithMany("Products")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Review", b =>
                {
                    b.HasOne("RecoverUnsoldApi.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Location", b =>
                {
                    b.Navigation("Offers");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Offer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Order", b =>
                {
                    b.Navigation("Opinions");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Product", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.User", b =>
                {
                    b.Navigation("FcmTokens");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Customer", b =>
                {
                    b.Navigation("Alerts");
                });

            modelBuilder.Entity("RecoverUnsoldApi.Entities.Distributor", b =>
                {
                    b.Navigation("Locations");
                });
#pragma warning restore 612, 618
        }
    }
}
