using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class ServerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserLog> UserLogs { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductFile> ProductFiles { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductTag> ProductTag { get; set; }

        public ServerDbContext(DbContextOptions<ServerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Username);

                entity.Property(user => user.Username)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.HasKey(log => new { log.Username, log.Timestamp });

                entity
                    .HasOne(log => log.User)
                    .WithMany(user => user.UserLogs)
                    .HasPrincipalKey(user => user.Username)
                    .HasForeignKey(log => log.Username)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(tag => tag.Id);

                entity.Property(tag => tag.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(cat => cat.Id);

                entity.Property(cat => cat.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(cat => cat.ParentCategoryId)
                    .IsRequired(false);

                entity
                   .HasMany(cat => cat.SubCategories)
                   .WithOne(cat => cat.ParentCategory)
                   .HasForeignKey(cat => cat.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                entity
                    .HasOne(p => p.Owner)
                    .WithMany(u => u.Products)
                    .HasPrincipalKey(u => u.Username)
                    .HasForeignKey(p => p.OwnerUsername)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductFile>(entity =>
            {
                entity.HasKey(pf => pf.Id);

                entity.Property(pf => pf.Id)
                    .ValueGeneratedOnAdd();

                entity
                    .HasOne(pf => pf.Product)
                    .WithMany(p => p.ProductFiles)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(pc => new { pc.ProductId, pc.CategoryId });

                entity
                    .HasOne(pc => pc.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(pc => pc.ProductId);

                entity
                    .HasOne(pc => pc.Category)
                    .WithMany(c => c.ProductCategories)
                    .HasForeignKey(pc => pc.CategoryId);
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.HasKey(pc => new { pc.ProductId, pc.TagId });

                entity
                    .HasOne(pt => pt.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(pc => pc.ProductId);

                entity
                    .HasOne(pt => pt.Tag)
                    .WithMany(t => t.ProductTags)
                    .HasForeignKey(pt => pt.TagId);
            });
        }
    }
}
