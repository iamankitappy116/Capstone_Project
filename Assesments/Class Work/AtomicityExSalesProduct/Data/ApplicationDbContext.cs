using AtomicityExSalesProduct.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtomicityExSalesProduct.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<AppUser>     AppUsers     { get; set; }
        public DbSet<Product>     Products     { get; set; }
        public DbSet<CartItem>    CartItems    { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------------------------------------
            // Seed Products — 3 sample items available from day one
            // ---------------------------------------------------------
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop",  Price = 999.99m },
                new Product { Id = 2, Name = "Mouse",   Price = 19.99m  },
                new Product { Id = 3, Name = "USB Hub", Price = 29.99m  }
            );

            // ---------------------------------------------------------
            // Seed one demo user so AddToCart works without sign-up
            // ---------------------------------------------------------
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, Name = "Alice", Email = "alice@demo.com" }
            );
        }
    }
}

