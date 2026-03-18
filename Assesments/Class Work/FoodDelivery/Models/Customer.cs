using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
    
}
