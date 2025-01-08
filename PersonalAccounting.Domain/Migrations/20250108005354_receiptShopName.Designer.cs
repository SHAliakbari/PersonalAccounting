﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalAccounting.Domain.Data;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250108005354_receiptShopName")]
    partial class receiptShopName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountNo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CardNo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InviteCode")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsInviteUsed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("TelegramUser")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInformation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageFileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastEditDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserFullName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MerchantName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PaidByUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PaidByUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PaidByUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShopName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("BLOB");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.ReceiptItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Quantity")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuantityUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ReceiptId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("UnitPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.ToTable("ReceiptItems");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.TransferRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("DestinationAmount")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationCurrencyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Fee")
                        .HasColumnType("TEXT");

                    b.Property<string>("FeeCurrencyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FromUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FromUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FromUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastEditDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserFullName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastEditUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverAccountNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverCardNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverNote")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("RequestDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SourceAmount")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceCurrencyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ToUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ToUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ToUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TransferRequests");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.TransferRequestDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserFullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Thumbnail")
                        .HasColumnType("BLOB");

                    b.Property<int>("TransferRequestId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TransferRequestId");

                    b.ToTable("TransferRequestDetails");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PersonalAccounting.Domain.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PersonalAccounting.Domain.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonalAccounting.Domain.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PersonalAccounting.Domain.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.ReceiptItem", b =>
                {
                    b.HasOne("PersonalAccounting.Domain.Data.Receipt", "Receipt")
                        .WithMany("Items")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.TransferRequestDetail", b =>
                {
                    b.HasOne("PersonalAccounting.Domain.Data.TransferRequest", "TransferRequest")
                        .WithMany("Details")
                        .HasForeignKey("TransferRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransferRequest");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.Receipt", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("PersonalAccounting.Domain.Data.TransferRequest", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
