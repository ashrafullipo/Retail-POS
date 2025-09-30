using Microsoft.EntityFrameworkCore;
using Retail_POS.Models;

namespace Retail_POS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            // Purchase → Supplier relation
            modelBuilder.Entity<Purchase>()
               .HasOne(p => p.Supplier)
               .WithMany(s => s.Purchases)
               .HasForeignKey(p => p.SupplierId);

            // PurchaseItem → Purchase relation
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(p => p.Purchase)
                .WithMany(p => p.PurchaseItems)
                .HasForeignKey(p => p.PurchaseId);

            // PurchaseItem → Product relation
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Product)
                .WithMany()
                .HasForeignKey(pi => pi.ProductId);

        }
    }
}
