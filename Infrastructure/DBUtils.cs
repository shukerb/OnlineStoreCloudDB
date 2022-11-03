using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace Infrastructure
{
    public class DBUtils : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DBUtils(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        private void ConfigModel<TEntity>(ModelBuilder modelBuilder, string containerName) where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .ToContainer(containerName)
                .UseETagConcurrency()
                .HasNoDiscriminator();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("OnlineStoreDB");

            ConfigModel<User>(modelBuilder,nameof(Users));
            modelBuilder.Entity<User>()
                .HasPartitionKey(user => user.Id);

            ConfigModel<Review>(modelBuilder,nameof(Reviews));
            modelBuilder.Entity<Review>()
                .HasPartitionKey(review => review.Id);

            ConfigModel<Product>(modelBuilder,nameof(Products));
            modelBuilder.Entity<Product>()
                .HasPartitionKey(product => product.Id);

            ConfigModel<Order>(modelBuilder,nameof(Orders));
            modelBuilder.Entity<Order>()
                .HasPartitionKey(order => order.Id);

        }
    }
}
