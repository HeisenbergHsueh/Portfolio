using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Portfolio.Models;
using Portfolio.Models.LoginSystem;
using Portfolio.Models.JobRecordSystem;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Portfolio.Data
{
    public partial class PortfolioContext : DbContext
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserLogin> UserLogin { get; set; }

        #region JobRecordSystem DbSet
        public virtual DbSet<JobRecords> JobRecords { get; set; }

        public virtual DbSet<JobRecordsCategory> JobRecordsCategory { get; set; }

        public virtual DbSet<JobRecordsOsversion> JobRecordsOsversion { get; set; }

        public virtual DbSet<JobRecordsOwner> JobRecordsOwner { get; set; }

        public virtual DbSet<JobRecordsProductType> JobRecordsProductType { get; set; }

        public virtual DbSet<JobRecordsSite> JobRecordsSite { get; set; }

        public virtual DbSet<JobRecordsStatus> JobRecordsStatus { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            #region UserLogin Entity
            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserAccount)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserRole)
                    .IsRequired(false)
                    .HasMaxLength(300);

                entity.Property(e => e.IsEmailAuthenticated);

                entity.Property(e => e.AuthCode)
                    .IsRequired(false)
                    .HasMaxLength(100);
            });
            #endregion

            #region JobRecordSystem Entity

            #region JobRecords Entity
            modelBuilder.Entity<JobRecords>(entity =>
            {
                entity.Property(e => e.CaseId);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CaseDescription)
                    .IsRequired();

                entity.Property(e => e.ClosedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OSVersion)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OnsiteName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BuildDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.CaseStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsCategory Entity
            modelBuilder.Entity<JobRecordsCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsOsversion Entity
            modelBuilder.Entity<JobRecordsOsversion>(entity =>
            {
                entity.HasKey(e => e.OsversionId);

                entity.ToTable("JobsRecordOSVersion");

                entity.Property(e => e.OsversionId)
                    .HasColumnName("OSVersionId")
                    .ValueGeneratedNever();

                entity.Property(e => e.OsversionName)
                    .IsRequired()
                    .HasColumnName("OSVersionName")
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsOwner Entity
            modelBuilder.Entity<JobRecordsOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsProductType Entity
            modelBuilder.Entity<JobRecordsProductType>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsSite Entity
            modelBuilder.Entity<JobRecordsSite>(entity =>
            {
                entity.HasKey(e => e.SiteId);

                entity.Property(e => e.SiteName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region JobRecordsStatus Entity
            modelBuilder.Entity<JobRecordsStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion
           
            #endregion

            #region
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
