using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthFoodDeliveryApp.FoodDeliveryApp.Models
{
    public class FoodDeliveryDbCOntext : DbContext
    {
        public FoodDeliveryDbCOntext(DbContextOptions<FoodDeliveryDbCOntext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<ProductsSold> ProductsSold { get; set; }

        public DbSet<Party> Parties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key
            modelBuilder.Entity<ProductsSold>()
                .HasKey(ps => new { ps.ProductId, ps.SaleId });


            // Fix decimal precision warnings

            modelBuilder.Entity<Food>()
                .Property(f => f.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>()
                .Property(c => c.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductsSold>()
                .Property(ps => ps.TotalProductAmount)
                .HasPrecision(18, 2);
        }

    }
}
