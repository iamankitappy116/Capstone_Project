using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int RestaurantId { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
    }
}

