﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using taur_bot_api.Database;

#nullable disable

namespace taur_bot_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240219192908_WalletsAdded")]
    partial class WalletsAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("taur_bot_api.Database.Models.Investment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("DealSum")
                        .HasColumnType("numeric");

                    b.Property<int>("InvestmentTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalPercent")
                        .HasColumnType("numeric");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.InvestmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("MinValue")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("InvestmentType");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.ReferralNode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Inline")
                        .HasColumnType("integer");

                    b.Property<long>("ReferralId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReferrerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ReferralId");

                    b.HasIndex("ReferrerId");

                    b.ToTable("ReferralNodes");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<decimal>("InvestedBalance")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("LocalId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<string>("ReferralCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ReferrerId")
                        .HasColumnType("bigint");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ReferrerId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.Wallet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NetworkType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.Investment", b =>
                {
                    b.HasOne("taur_bot_api.Database.Models.InvestmentType", "InvestmentType")
                        .WithMany("Investments")
                        .HasForeignKey("InvestmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taur_bot_api.Database.Models.User", "User")
                        .WithMany("Investments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvestmentType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.ReferralNode", b =>
                {
                    b.HasOne("taur_bot_api.Database.Models.User", "Referral")
                        .WithMany("ReferralNodes")
                        .HasForeignKey("ReferralId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taur_bot_api.Database.Models.User", "Referrer")
                        .WithMany("ReferrerNodes")
                        .HasForeignKey("ReferrerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Referral");

                    b.Navigation("Referrer");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.User", b =>
                {
                    b.HasOne("taur_bot_api.Database.Models.User", "Referrer")
                        .WithMany("Referrals")
                        .HasForeignKey("ReferrerId");

                    b.Navigation("Referrer");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.Wallet", b =>
                {
                    b.HasOne("taur_bot_api.Database.Models.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.InvestmentType", b =>
                {
                    b.Navigation("Investments");
                });

            modelBuilder.Entity("taur_bot_api.Database.Models.User", b =>
                {
                    b.Navigation("Investments");

                    b.Navigation("ReferralNodes");

                    b.Navigation("Referrals");

                    b.Navigation("ReferrerNodes");

                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
