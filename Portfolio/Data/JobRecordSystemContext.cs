using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Portfolio.Models.JobRecordSystem;

namespace Portfolio.Data
{
    public partial class JobRecordSystemContext : DbContext
    {
        //建構子
        public JobRecordSystemContext()
        {
        }

        public JobRecordSystemContext(DbContextOptions<JobRecordSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<JobRecords> DbSet_JobRecords { get; set; }

        public virtual DbSet<JobRecordsCategory> DbSet_JobRecordsCategory { get; set; }

        public virtual DbSet<JobRecordsOsversion> DbSet_JobRecordsOsversion { get; set; }

        public virtual DbSet<JobRecordsOwner> DbSet_JobRecordsOwner { get; set; }

        public virtual DbSet<JobRecordsProductType> DbSet_JobRecordsProductType { get; set; }

        public virtual DbSet<JobRecordsSite> DbSet_JobRecordsSite { get; set; }

        public virtual DbSet<JobRecordsStatus> DbSet_JobRecordsStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobRecords>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Hostname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Osversion)
                    .IsRequired()
                    .HasColumnName("OSVersion")
                    .HasMaxLength(50);

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Progress)
                    .IsRequired();

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
