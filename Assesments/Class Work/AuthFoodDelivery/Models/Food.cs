using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        public int categId { get; set; }

        [ForeignKey("categId")]
        public Category? Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }
    }
}