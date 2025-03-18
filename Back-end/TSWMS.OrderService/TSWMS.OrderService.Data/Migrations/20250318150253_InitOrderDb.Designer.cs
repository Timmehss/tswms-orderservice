﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TSWMS.OrderService.Data;

#nullable disable

namespace TSWMS.OrderService.Data.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20250318150253_InitOrderDb")]
    partial class InitOrderDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TSWMS.OrderService.Shared.Models.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            OrderId = new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                            OrderDate = new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TotalAmount = 160.00m,
                            UserId = new Guid("52348777-7a0e-4139-9489-87dff9d47b7e")
                        },
                        new
                        {
                            OrderId = new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                            OrderDate = new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TotalAmount = 125.00m,
                            UserId = new Guid("7ee4caea-21e9-4261-947d-8305df18ff45")
                        },
                        new
                        {
                            OrderId = new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"),
                            OrderDate = new DateTime(2024, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TotalAmount = 210.00m,
                            UserId = new Guid("27d0bb84-4420-441c-a503-264f1e365c05")
                        });
                });

            modelBuilder.Entity("TSWMS.OrderService.Shared.Models.OrderItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.ToTable("OrderItems");

                    b.HasData(
                        new
                        {
                            OrderId = new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                            ProductId = new Guid("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"),
                            Price = 30.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                            ProductId = new Guid("e8794f64-d634-4b74-a0a2-35f4fbf7b486"),
                            Price = 40.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                            ProductId = new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"),
                            Price = 50.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                            ProductId = new Guid("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"),
                            Price = 40.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                            ProductId = new Guid("4d76bc3a-168d-4054-8154-6f6246355e53"),
                            Price = 25.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                            ProductId = new Guid("bcc9673c-5c3f-4798-808f-b48e372b1dae"),
                            Price = 30.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                            ProductId = new Guid("15d94488-db9b-4767-9e92-d6328b735e0e"),
                            Price = 30.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                            ProductId = new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"),
                            Price = 40.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"),
                            ProductId = new Guid("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"),
                            Price = 50.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"),
                            ProductId = new Guid("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"),
                            Price = 55.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"),
                            ProductId = new Guid("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"),
                            Price = 55.00m,
                            Quantity = 1
                        },
                        new
                        {
                            OrderId = new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"),
                            ProductId = new Guid("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"),
                            Price = 50.00m,
                            Quantity = 1
                        });
                });

            modelBuilder.Entity("TSWMS.OrderService.Shared.Models.OrderItem", b =>
                {
                    b.HasOne("TSWMS.OrderService.Shared.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("TSWMS.OrderService.Shared.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
