﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eShop.Viber.DbContexts;

#nullable disable

namespace eShop.Viber.Migrations
{
    [DbContext(typeof(ViberDbContext))]
    partial class ViberDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eShop.Viber.Entities.ViberChatSettings", b =>
                {
                    b.Property<Guid>("ViberUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.HasKey("ViberUserId");

                    b.ToTable("ViberChatSettings");
                });

            modelBuilder.Entity("eShop.Viber.Entities.ViberUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActiveContext")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSubcribed")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("ViberUsers");
                });

            modelBuilder.Entity("eShop.Viber.Entities.ViberChatSettings", b =>
                {
                    b.HasOne("eShop.Viber.Entities.ViberUser", "ViberUser")
                        .WithOne("ChatSettings")
                        .HasForeignKey("eShop.Viber.Entities.ViberChatSettings", "ViberUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ViberUser");
                });

            modelBuilder.Entity("eShop.Viber.Entities.ViberUser", b =>
                {
                    b.Navigation("ChatSettings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
