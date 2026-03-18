using FoodDelivery.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
    }
}
