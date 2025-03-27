﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TSWMS.ProductService.Data;

#nullable disable

namespace TSWMS.ProductService.Data.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    partial class ProductsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TSWMS.ProductService.Shared.Models.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableStock")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = new Guid("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"),
                            AvailableStock = 100,
                            Name = "HDMI Cable 1.5m"
                        },
                        new
                        {
                            ProductId = new Guid("e8794f64-d634-4b74-a0a2-35f4fbf7b486"),
                            AvailableStock = 150,
                            Name = "USB-C Cable 2m"
                        },
                        new
                        {
                            ProductId = new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"),
                            AvailableStock = 200,
                            Name = "Lightning Cable 1m"
                        },
                        new
                        {
                            ProductId = new Guid("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"),
                            AvailableStock = 120,
                            Name = "HDMI Cable 3m"
                        },
                        new
                        {
                            ProductId = new Guid("4d76bc3a-168d-4054-8154-6f6246355e53"),
                            AvailableStock = 90,
                            Name = "USB-C Cable 1.5m"
                        },
                        new
                        {
                            ProductId = new Guid("bcc9673c-5c3f-4798-808f-b48e372b1dae"),
                            AvailableStock = 110,
                            Name = "Lightning Cable 2m"
                        },
                        new
                        {
                            ProductId = new Guid("15d94488-db9b-4767-9e92-d6328b735e0e"),
                            AvailableStock = 80,
                            Name = "HDMI Cable 5m"
                        },
                        new
                        {
                            ProductId = new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"),
                            AvailableStock = 70,
                            Name = "USB-C Cable 0.5m"
                        },
                        new
                        {
                            ProductId = new Guid("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"),
                            AvailableStock = 75,
                            Name = "Lightning Cable 1.5m"
                        },
                        new
                        {
                            ProductId = new Guid("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"),
                            AvailableStock = 95,
                            Name = "HDMI Cable 10m"
                        },
                        new
                        {
                            ProductId = new Guid("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"),
                            AvailableStock = 60,
                            Name = "USB-C Cable 3m"
                        },
                        new
                        {
                            ProductId = new Guid("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"),
                            AvailableStock = 85,
                            Name = "Lightning Cable 5m"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
