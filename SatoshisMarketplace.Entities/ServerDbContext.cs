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
        }
    }
}
