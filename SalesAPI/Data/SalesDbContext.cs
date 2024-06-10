using Microsoft.EntityFrameworkCore;
using SalesAPI.Models;

namespace SalesAPI.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }
     

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SalesOrderDetailEntity> SalesOrderDetailEntities { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.SalesOrderDetails)
                .WithOne(s => s.Order)
                .HasForeignKey(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalesOrderDetailEntity>()
                .HasOne(s => s.Order)
                .WithMany(o => o.SalesOrderDetails)
                .HasForeignKey(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
               .HasMany(o => o.SalesOrderDetails)
               .WithOne(d => d.Order)
               .HasForeignKey(d => d.OrderID);

            modelBuilder.Entity<SalesOrderDetailEntity>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductID);

            modelBuilder.Entity<SalesOrderDetailEntity>()
               .HasOne(d => d.Product)
               .WithMany()
               .HasForeignKey(d => d.ProductID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(p => p.CategoryName)
                .IsUnique();


            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }



    }
}
