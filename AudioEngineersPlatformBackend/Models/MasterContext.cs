using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Models;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Advert> Adverts { get; set; }

    public virtual DbSet<AdvertCategoryDict> AdvertCategoryDicts { get; set; }

    public virtual DbSet<AdvertLog> AdvertLogs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<PlaylistLink> PlaylistLinks { get; set; }

    public virtual DbSet<PlaylistTypeDict> PlaylistTypeDicts { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewLog> ReviewLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

    public virtual DbSet<SocialMediaTypeDict> SocialMediaTypeDicts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=master;User Id=sa;Password=bazaTestowa1234;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advert>(entity =>
        {
            entity.HasKey(e => e.IdAdvert).HasName("Advert_pk");

            entity.ToTable("Advert");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAdvertLogNavigation).WithMany(p => p.Adverts)
                .HasForeignKey(d => d.IdAdvertLog)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Advert_AdvertLog");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Adverts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Advert_User");

            entity.HasMany(d => d.IdAdvertCategoryDicts).WithMany(p => p.IdAdverts)
                .UsingEntity<Dictionary<string, object>>(
                    "AdvertCategory",
                    r => r.HasOne<AdvertCategoryDict>().WithMany()
                        .HasForeignKey("IdAdvertCategoryDict")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Table_10_AdvertCategoryDict"),
                    l => l.HasOne<Advert>().WithMany()
                        .HasForeignKey("IdAdvert")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Table_10_Advert"),
                    j =>
                    {
                        j.HasKey("IdAdvert", "IdAdvertCategoryDict").HasName("AdvertCategories_pk");
                        j.ToTable("AdvertCategories");
                    });
        });

        modelBuilder.Entity<AdvertCategoryDict>(entity =>
        {
            entity.HasKey(e => e.IdAdvertCategoryDict).HasName("AdvertCategoryDict_pk");

            entity.ToTable("AdvertCategoryDict");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AdvertLog>(entity =>
        {
            entity.HasKey(e => e.IdAdvertLog).HasName("AdvertLog_pk");

            entity.ToTable("AdvertLog");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdUserModifier).HasColumnName("IdUser_Modifier");

            entity.HasOne(d => d.IdUserModifierNavigation).WithMany(p => p.AdvertLogs)
                .HasForeignKey(d => d.IdUserModifier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdvertLog_User");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("Image_pk");

            entity.ToTable("Image");

            entity.Property(e => e.ImageLink)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAdvertNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.IdAdvert)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Image_Advert");
        });

        modelBuilder.Entity<PlaylistLink>(entity =>
        {
            entity.HasKey(e => e.IdPlaylistLink).HasName("PlaylistLink_pk");

            entity.ToTable("PlaylistLink");

            entity.Property(e => e.Link)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAdvertNavigation).WithMany(p => p.PlaylistLinks)
                .HasForeignKey(d => d.IdAdvert)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistLink_Advert");

            entity.HasOne(d => d.IdPlaylistTypeNavigation).WithMany(p => p.PlaylistLinks)
                .HasForeignKey(d => d.IdPlaylistType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistLink_PlaylistTypeDict");
        });

        modelBuilder.Entity<PlaylistTypeDict>(entity =>
        {
            entity.HasKey(e => e.IdPlaylistTypeDict).HasName("PlaylistTypeDict_pk");

            entity.ToTable("PlaylistTypeDict");

            entity.Property(e => e.PlaylistTypeName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("Review_pk");

            entity.ToTable("Review");

            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IdUserReviewer).HasColumnName("IdUser_Reviewer");

            entity.HasOne(d => d.IdAdvertNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdAdvert)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_Advert");

            entity.HasOne(d => d.IdReviewLogNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdReviewLog)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_ReviewLog");

            entity.HasOne(d => d.IdUserReviewerNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdUserReviewer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_User");
        });

        modelBuilder.Entity<ReviewLog>(entity =>
        {
            entity.HasKey(e => e.IdReviewLog).HasName("ReviewLog_pk");

            entity.ToTable("ReviewLog");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("Role_pk");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SocialMediaLink>(entity =>
        {
            entity.HasKey(e => e.IdSocialMediaLink).HasName("SocialMediaLink_pk");

            entity.ToTable("SocialMediaLink");

            entity.Property(e => e.SocialMediaLink1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("SocialMediaLink");

            entity.HasOne(d => d.IdSocialMediaTypeDictNavigation).WithMany(p => p.SocialMediaLinks)
                .HasForeignKey(d => d.IdSocialMediaTypeDict)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SocialMediaLink_SocialMediaTypeDict");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.SocialMediaLinks)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SocialMediaLink_User");
        });

        modelBuilder.Entity<SocialMediaTypeDict>(entity =>
        {
            entity.HasKey(e => e.IdSocialMediaType).HasName("SocialMediaTypeDict_pk");

            entity.ToTable("SocialMediaTypeDict");

            entity.Property(e => e.SocialMediaTypeName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Role");

            entity.HasOne(d => d.IdUserLogNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserLog)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_UserLog");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasKey(e => e.IdUserLog).HasName("UserLog_pk");

            entity.ToTable("UserLog");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateDeleted).HasColumnType("datetime");
            entity.Property(e => e.DateLastLogin).HasColumnType("datetime");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.VerificationCodeExpiration).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
