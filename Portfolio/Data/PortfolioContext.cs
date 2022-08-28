using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Portfolio.Models;
using Portfolio.Models.LoginSystem;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Portfolio.Data
{
    public partial class PortfolioContext : DbContext
    {
        public PortfolioContext()
        {
        }

        public PortfolioContext(DbContextOptions<PortfolioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fruits> Fruits { get; set; }
        public virtual DbSet<Movies> Movies { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fruits>(entity =>
            {
                entity.HasKey(e => e.FruitId)
                    .HasName("PK_Fruits_1");

                entity.Property(e => e.FruitId).ValueGeneratedNever();

                entity.Property(e => e.FruitName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movies>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
