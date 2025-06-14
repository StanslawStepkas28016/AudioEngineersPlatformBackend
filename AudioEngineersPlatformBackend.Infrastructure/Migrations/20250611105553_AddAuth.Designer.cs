﻿// <auto-generated />
using System;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AudioEngineersPlatformBackend.Infrastructure.Migrations
{
    [DbContext(typeof(EngineersPlatformDbContext))]
    [Migration("20250611105553_AddAuth")]
    partial class AddAuth
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRole")
                        .HasName("PK_Role");

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            IdRole = new Guid("5c9a3c43-8f4e-4c1e-a5f3-8e3cdbe0158a"),
                            RoleName = "Administrator"
                        },
                        new
                        {
                            IdRole = new Guid("aaaa3c43-8f4e-4c1e-a5f3-8e3cdbe0158a"),
                            RoleName = "Client"
                        },
                        new
                        {
                            IdRole = new Guid("bbbb3c43-8f4e-4c1e-a5f3-8e3cdbe0158a"),
                            RoleName = "Audio engineer"
                        });
                });

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IdRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdUserLog")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUser")
                        .HasName("PK_User");

                    b.HasIndex("IdRole");

                    b.HasIndex("IdUserLog");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.UserLog", b =>
                {
                    b.Property<Guid>("IdUserLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDeleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateLastLogin")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExp")
                        .HasColumnType("datetime2");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VerificationCodeExpiration")
                        .HasColumnType("datetime2");

                    b.HasKey("IdUserLog")
                        .HasName("PK_UserLog");

                    b.ToTable("UserLog", (string)null);
                });

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.User", b =>
                {
                    b.HasOne("AudioEngineersPlatformBackend.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_User_Role");

                    b.HasOne("AudioEngineersPlatformBackend.Domain.Entities.UserLog", "UserLog")
                        .WithMany("Users")
                        .HasForeignKey("IdUserLog")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_User_UserLog");

                    b.Navigation("Role");

                    b.Navigation("UserLog");
                });

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AudioEngineersPlatformBackend.Domain.Entities.UserLog", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
