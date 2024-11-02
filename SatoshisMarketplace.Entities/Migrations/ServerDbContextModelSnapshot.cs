﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SatoshisMarketplace.Entities;

#nullable disable

namespace SatoshisMarketplace.Entities.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    partial class ServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SatoshisMarketplace.Entities.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varbinary(64)");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SatoshisMarketplace.Entities.UserLog", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(1);

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasMaxLength(39)
                        .HasColumnType("nvarchar(39)");

                    b.HasKey("Username", "Timestamp");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("SatoshisMarketplace.Entities.UserLog", b =>
                {
                    b.HasOne("SatoshisMarketplace.Entities.User", "User")
                        .WithMany("UserLogs")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SatoshisMarketplace.Entities.User", b =>
                {
                    b.Navigation("UserLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
