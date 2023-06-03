﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrivateVcs.Data;

#nullable disable

namespace PrivateVcs.Migrations
{
    [DbContext(typeof(PrivateVcDatabase))]
    [Migration("20230414174101_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("PrivateVcs")
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PrivateVcs.Models.PrivateVcConfig", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("AllowedRoles")
                        .HasColumnType("longtext");

                    b.Property<string>("ChannelFilterRegex")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatorRoles")
                        .HasColumnType("longtext");

                    b.Property<ulong>("PrivateCategoryId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("WaitingVcName")
                        .HasColumnType("longtext");

                    b.HasKey("GuildId");

                    b.ToTable("PrivateVcConfigs", "PrivateVcs");
                });
#pragma warning restore 612, 618
        }
    }
}
