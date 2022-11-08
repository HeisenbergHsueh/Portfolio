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

        public virtual DbSet<JobRecordsOSVersion> JobRecordsOSVersion { get; set; }

        public virtual DbSet<JobRecordsOnsiteList> JobRecordsOnsiteList { get; set; }

        public virtual DbSet<JobRecordsProductType> JobRecordsProductType { get; set; }

        public virtual DbSet<JobRecordsLocationItem> JobRecordsLocationItem { get; set; }

        public virtual DbSet<JobRecordsCaseStatusItem> JobRecordsCaseStatusItem { get; set; }

        public virtual DbSet<JobRecordsReply> JobRecordsReply { get; set; }
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
                entity.Property(e => e.CaseId).ValueGeneratedOnAdd();

                entity.Property(e => e.CaseStatus).IsRequired();

                entity.Property(e => e.CaseTitle).IsRequired().HasMaxLength(50);

                entity.Property(e => e.BuildDate).HasColumnType("datetime");

                entity.Property(e => e.CaseDescription).IsRequired();

                entity.Property(e => e.Location).IsRequired();

                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.OnsiteName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.HostName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.ProductType).IsRequired().HasMaxLength(50);

                entity.Property(e => e.OSVersion).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Category).IsRequired();

                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.ClosedOnsiteName).HasMaxLength(50);

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

            #region JobRecordsOSVersion Entity
            modelBuilder.Entity<JobRecordsOSVersion>(entity =>
            {
                entity.HasKey(e => e.OSVersionId);

                entity.ToTable("JobsRecordOSVersion");

                entity.Property(e => e.OSVersionId).HasColumnName("OSVersionId").ValueGeneratedNever();

                entity.Property(e => e.OSVersionName).IsRequired().HasColumnName("OSVersionName").HasMaxLength(50);
            });
            #endregion

            #region JobRecordsOnsiteList Entity
            modelBuilder.Entity<JobRecordsOnsiteList>(entity =>
            {
                entity.HasKey(e => e.OnsiteId);

                entity.Property(e => e.OnsiteName).IsRequired().HasMaxLength(50);

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

            #region JobRecordsLocationItem Entity
            modelBuilder.Entity<JobRecordsLocationItem>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationName).IsRequired().HasMaxLength(50);

            });
            #endregion

            #region JobRecordsCaseStatusItem Entity
            modelBuilder.Entity<JobRecordsCaseStatusItem>(entity =>
            {
                entity.HasKey(e => e.CaseStatusId);

                entity.Property(e => e.CaseStatusId).ValueGeneratedNever();

                entity.Property(e => e.CaseStatusName).IsRequired().HasMaxLength(50);

            });
            #endregion

            #region JobRecordsReply Entity
            modelBuilder.Entity<JobRecordsReply>(entity =>
            {
                entity.HasKey(e => e.ReplyId);

                entity.Property(e => e.ReplyId).ValueGeneratedOnAdd();

                entity.Property(e => e.RelatedWithJobRecordsId).IsRequired();

                entity.Property(e => e.ReplyPersonName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.ReplyDateTime).HasColumnType("datetime");

                entity.Property(e => e.ReplyContent).IsRequired();

                entity.Property(e => e.IsThereAttachment).HasMaxLength(10);

                entity.Property(e => e.AttachmentName).HasMaxLength(50);
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
