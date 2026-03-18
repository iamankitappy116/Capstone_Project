using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

}