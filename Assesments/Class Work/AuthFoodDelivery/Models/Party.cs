using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class Party
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsExternal { get; set; }
    }
}
